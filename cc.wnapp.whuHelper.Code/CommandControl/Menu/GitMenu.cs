using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class GitMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         Git菜单         [CQ:emoji,id=11088]\n" +
                "－－－ 私聊命令 －－－\n" +
                "[CQ:emoji,id=128310]绑定Github账户\n" +
                "[CQ:emoji,id=128310]查询Github账户\n" +
                "[CQ:emoji,id=128310]解绑Github账户#[账户名]#\n" +
                "[CQ:emoji,id=128310]绑定仓库#[仓库名]#\n" +
                "[CQ:emoji,id=128310]查询仓库#[仓库名]#\n" +
                "[CQ:emoji,id=128310]解绑仓库#[仓库名]#\n" +
                "－群聊命令(管理可用)－\n" +
                "[CQ:emoji,id=128310]绑定仓库#[仓库名]#\n" +
                "[CQ:emoji,id=128310]查询仓库#[仓库名]#\n" +
                "[CQ:emoji,id=128310]解绑仓库#[仓库名]#\n" +
                "[CQ:emoji,id=10024]      Whu Helper       [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
