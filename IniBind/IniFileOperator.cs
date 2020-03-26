using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities.IniBind
{
    public class IniFileOperator
    {
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key, string def, byte[] retVal, int size, string filePath);

        private readonly string fullPath;

        public IniFileOperator(string fileName)
        {
            if (!File.Exists(fileName))
                throw new Exception($"<{fileName}>文件不存在");
            fullPath = Path.GetFullPath(fileName);
        }
        public long ReadIniData(string section, string key, out string result)
        {
            return ReadIniData(fullPath, section, key, out result);
        }

        public static long ReadIniData(string absFilePath, string section, string key, out string result)
        {
            StringBuilder temp = new StringBuilder(1024);
            var ret = GetPrivateProfileString(section, key, "", temp, 1024, absFilePath);
            result = temp.ToString();
            return ret;
        }
        public long WriteIniData(string section, string key, string value) => WriteIniData(fullPath, section, key, value);

        public static long WriteIniData(string absFilePath, string section, string key, string value) => WritePrivateProfileString(section, key, value, absFilePath);

        public IEnumerable<string> GetKeys(string section) => GetKeys(fullPath, section);

        public static IEnumerable<string> GetKeys(string absFilePath, string section)
        {
            byte[] buf = new byte[65536];
            uint len = GetPrivateProfileStringA(section, null, null, buf, buf.Length, absFilePath);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    yield return Encoding.Default.GetString(buf, j, i - j);
                    j = i + 1;
                }
            }
        }

        public long DeleteKey(string section, string key) => DeleteKey(fullPath, section, key);

        public static long DeleteKey(string absFilePath, string section, string key) => WritePrivateProfileString(section, key, null, absFilePath);

        public bool HasKey(string section, string key) => HasKey(fullPath, section, key);

        public static bool HasKey(string absFilePath, string section, string key) => GetKeys(absFilePath, section).Contains(key);
    }
}
