using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class ScoreMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         成绩菜单         [CQ:emoji,id=11088]\n" +
                "－私聊(群聊)命令示例－\n" +
                "－至少使用一个操作字段－\n" +
                "[CQ:emoji,id=128310]计算成绩 操作1|操作2|操作3\n" +
                "[CQ:emoji,id=128310]查询成绩 操作1|操作2|操作3\n" +
                "可选操作：去除公选、去除公必、去除专必、去除专选、去除非本院、20XX、第n学期、课程名\n" +
                "[CQ:emoji,id=10024]       Whu Helper        [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
