using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class CourseMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         课程菜单         [CQ:emoji,id=11088]\n" +
                "－－－ 私聊命令 －－－\n" +
                "[CQ:emoji,id=128310]展示课表\n" +
                "[CQ:emoji,id=128310]下载课表\n" +
                "[CQ:emoji,id=128310]按课头号查询|查询内容\n" +
                "[CQ:emoji,id=128310]按课程名查询|查询内容\n" +
                "[CQ:emoji,id=128310]按学分查询|查询内容\n" +
                "[CQ:emoji,id=128310]按授课学院查询|查询内容\n" +
                "[CQ:emoji,id=128310]按专业查询|查询内容\n" +
                "[CQ:emoji,id=128310]按授课教师查询|查询内容\n" +
                "[CQ:emoji,id=10024]      Whu Helper       [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
