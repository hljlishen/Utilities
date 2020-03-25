using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Rules;

namespace Utilities.InputValidation
{
    public class InputValidator
    {
        private Dictionary<Control, StringRule> ruleMap = new Dictionary<Control, StringRule>();
        public ErrorProvider errorProvider = new ErrorProvider() {BlinkStyle = ErrorBlinkStyle.NeverBlink };
        public event Action<Control, StringRule> UnvalidatedInput;
        public Control CueControl { get; set; }
        public void AddValidation(Control c, StringRule rule)
        {
            if (ruleMap.ContainsKey(c))
                return;

            c.TextChanged += Tb_TextChanged;
            ruleMap.Add(c, rule);
        }

        public void RemoveValidation(Control c)
        {
            if (ruleMap.ContainsKey(c))
                ruleMap.Remove(c);
        }

        private void Tb_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as Control;
            StringRule rule = ruleMap[tb];

            if (rule.Pass(tb.Text))
            {
                errorProvider.SetError(tb, "");
                return;
            }
            else
            {
                errorProvider.SetError(tb, rule.Hint);
                UnvalidatedInput?.Invoke(tb, rule);
            }
        }

        public bool IsAllInputsValidate()
        {
            foreach (var tb in ruleMap.Keys)
            {
                if (errorProvider.GetError(tb) != "")
                    return false;
            }
            return true;
        }

        public void Cue()
        {
            var timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;
            timer.Start();
            errorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            errorProvider.BlinkRate = 200;
            if(CueControl != null)
            {
                if (CueControl.ForeColor != Color.Red)
                    CueControl.ForeColor = Color.Red;
                CueControl.Text = "输入存在错误，请检查";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var t = sender as Timer;
            t.Stop();
            t.Tick -= Timer_Tick;
            t.Dispose();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            if (CueControl != null)
            {
                CueControl.Text = "";
            }
        }
    }
}
