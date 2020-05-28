using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class textOp
    {
        public static string GetMiddleText(string t, string k, string j) //取出中间文本
        {
            try
            {
                var kn = t.IndexOf(k, StringComparison.Ordinal) + k.Length;
                var jn = t.IndexOf(j, kn, StringComparison.Ordinal);
                return t.Substring(kn, jn - kn);
            }
            catch
            {
                return "发现异常错误！";
            }
        }

        public static string GetLeftText(string str, string s)  //取出左边文本
        {
            string t = str.Substring(0, str.IndexOf(s));
            return t;
        }

        public static string GetRightText(string str, string s)     //取出右边文本
        {
            string t = str.Substring(str.IndexOf(s) + 1, str.Length - str.Substring(0, str.IndexOf(s)).Length - 1);
            return t;
        }
    }
}
