using Native.Tool.IniConfig.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Native.Tool.IniConfig
{
    public static class ini
    {

        /// <summary>
        /// 读配置项 - 读取指定配置文件中指定项目的文本内容。
        /// </summary>
        /// <param name="Path">指定配置文件的路径名称，通常以.ini作为文件名后缀。</param>
        /// <param name="Section">欲读入配置项所处节的名称。</param>
        /// <param name="Item">欲读入配置项在其节中的名称</param>
        /// <param name="Value">“默认文本”，可以被省略。如果指定配置项不存在，将返回此默认文本。如果指定配置项不存在且本参数被省略，将返回空文本。</param>
        /// <returns>返回配置项文本。</returns>
        public static string Read(string Path, string Section, string Item, string Value="")
        {
            IniConfig ini = new IniConfig(Path);
            ini.Load();

            if(ini.Object[Section].TryGetValue(Item, out IValue v)) //配置项存在
            {
                return ini.Object[Section][Item];
            }
            else
            {
                return Value;
            }
        }

        /// <summary>
        /// 写配置项 - 将指定文本内容写入指定配置项中或者删除指定的配置项或节，如果指定配置文件不存在，将会自动创建。
        /// </summary>
        /// <param name="Path">指定配置文件的路径名称，通常以.ini作为文件名后缀。</param>
        /// <param name="Section">欲写入配置项所处节的名称。</param>
        /// <param name="Item">欲写入配置项在其节中的名称</param>
        /// <param name="Value">指定欲写入到指定配置项中的文本。如果参数值为空，则删除所指定配置项。</param>
        public static void Write(string Path, string Section, string Item, string Value = "")
        {
            IniConfig ini = new IniConfig(Path);
            ini.Load();
            if(Value == "")
            {
                ini.Object[Section].Remove(Item);
            }
            else
            {
                ini.Object[Section][Item] = Value;
            }
            ini.Save();
        }
    }
}
