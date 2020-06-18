using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eas;
using static Eas.EasOP;

namespace cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem
{
    /// <summary>
    /// 更新成绩信息
    /// 命令格式：更新成绩
    /// </summary>
    public class UpdateScoreDB : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                Reply("正在尝试更新成绩数据……");
                var sList = EasOP.UpdateScore(fromQQ);
                string re = "";
                if (sList.Count != 0)
                {
                    foreach (var s in sList)
                    {
                        re = re + "\n课程：" + s.LessonName + "\n成绩：" + s.Mark + "\n——————————";
                    }
                }
                if (re != "")
                {
                    Reply("更新成功，新出成绩如下：" + re);
                }
                else
                {
                    Reply("更新成功，当前没有新出成绩。");
                }
                
                return 1;
            }
            catch(UpdataErrorException ex)
            {
                Reply("更新失败 发生错误：\n" + ex.Message);
                return 1;
            }
        }
    }
}
