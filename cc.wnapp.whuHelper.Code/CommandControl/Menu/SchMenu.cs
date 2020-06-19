using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class SchMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         日程菜单         [CQ:emoji,id=11088]\n" +
                "－私聊(群聊)命令示例－\n" +
                "－群聊命令仅管理员可用－\n" +
                "－字段间请注意空格分隔－\n" +
                "[CQ:emoji,id=128310]添加(群)日程 2020/6/2 18:30 内容\n" +
                "[CQ:emoji,id=128310]添加(群)周日程 2020/6/2 18:30 内容 周数\n" +
                "[CQ:emoji,id=128310]删除(群)日程 日程序号\n" +
                "[CQ:emoji,id=128310]删除(群)周日程 日程序号\n" +
                "[CQ:emoji,id=128310]修改(群)日程 日程序号 2020/6/2 18:30 内容\n" +
                "[CQ:emoji,id=128310]修改(群)周日程 日程序号 2020/6/2 18:30 内容 周数\n" +
                "[CQ:emoji,id=128310]查看(群)日程\n" +
                "[CQ:emoji,id=128310]查看(群)周日程\n" +
                "[CQ:emoji,id=10024]       Whu Helper        [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
