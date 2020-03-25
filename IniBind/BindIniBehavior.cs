using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace IniBind
{
    public class BindIniBehavior : IInterceptionBehavior
    {
        public bool WillExecute => true;

        private bool firstTimeCalled = true;

        private bool shouldProcess = true;

        private IniFileOperator config = null;

        private readonly BindStrategy bindStrategy;

        public BindIniBehavior(BindStrategy bindStrategy)
        {
            this.bindStrategy = bindStrategy;
        }

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var result = getNext()(input, getNext);
            if (!bindStrategy.IsIniPropertyCall(input) || !shouldProcess)
                return result;

            //Console.WriteLine($"{nameof(BindIniBehavior)} called...");

            var property = input.Target.GetType().GetProperty(input.MethodBase.Name.Substring(4));  //input.MethodBase.Name.Substring(4)可以得到属性的名字
            var fullName = bindStrategy.GetFilePath(input, property);

            if (!File.Exists(fullName))
            {
                CreateIniFile(input);
                firstTimeCalled = false;
            }
            else
            {
                if(config == null)
                    config = new IniFileOperator(fullName);
            }

            if (firstTimeCalled)             //第一次调用任何绑定Ini的字段前，先读取ini文件数据
            {
                firstTimeCalled = false;
                SynchronizeIniData(input);    //同步数据
            }

            if (IsSetterCalled(input))           //属性的setter被调用
            {
                UpdateIniKeyValue(input);     //更新对应的字段
            }

            return result;
        }
        private void CreateIniFile(IMethodInvocation input)
        {
            var property = BindStrategy.GetCalledProperty(input);
            var fullName = bindStrategy.GetFilePath(input, property);
            var stream = File.CreateText(fullName);
            stream.Close();

            config = new IniFileOperator(fullName);

            shouldProcess = false;
            foreach (var p in bindStrategy.GetIniProperties(input))
            {
                var value = p.GetValue(input.Target);
                var valueStr = value != null ? value.ToString() : "";
                string section = bindStrategy.GetSection(input, p);
                string key = bindStrategy.GetKey(input, p);
                config.WriteIniData(section, key, valueStr);
            }
            shouldProcess = true;
        }

        private static bool IsSetterCalled(IMethodInvocation input) => input.MethodBase.Name.StartsWith("set_");

        private void SynchronizeIniData(IMethodInvocation input)
        {
            shouldProcess = false;
            foreach (var p in bindStrategy.GetIniProperties(input))
            {
                if (config.HasKey(bindStrategy.GetSection(input, p), bindStrategy.GetKey(input, p)))
                {
                    UpdatePropertyValue(input, p);
                }
                else
                {
                    CreateKey(input, p);
                }
            }
            shouldProcess = true;
        }
        private void UpdatePropertyValue(IMethodInvocation input, PropertyInfo p)
        {
            var obj = input.Target;
            _ = config.ReadIniData(bindStrategy.GetSection(input, p), bindStrategy.GetKey(input, p), out string valueStr);
            var parseMethod = p.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
            if (parseMethod == null)
                p.SetValue(obj, valueStr);
            else
                p.SetValue(obj, parseMethod.Invoke(obj, new object[] { valueStr }));
        }
        private void CreateKey(IMethodInvocation input, PropertyInfo p)
        {
            var value = p.GetValue(input.Target) ?? "";
            string section = bindStrategy.GetSection(input, p);
            string key = bindStrategy.GetKey(input, p);
            config.WriteIniData(section, key, value.ToString());
        }
        private void UpdateIniKeyValue(IMethodInvocation input)
        {
            var p = BindStrategy.GetCalledProperty(input);
            if (p == null)  //如果调用了set_开头_ini结尾的函数，返回的p会是null
                return;
            string section = bindStrategy.GetSection(input, p);
            string key = bindStrategy.GetKey(input, p);
            config.WriteIniData(section, key, input.Arguments[0].ToString());
        }
    }
}
