using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class AttMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         关注菜单         [CQ:emoji,id=11088]\n" +
                "－－－ 私聊命令 －－－\n" +
                "－字段间请注意空格分隔－\n" +
                "[CQ:emoji,id=128310]添加关注 关注内容 群号\n" +
                "[CQ:emoji,id=128310]删除关注 关注内容 群号\n" +
                "[CQ:emoji,id=128310]更新关注 旧关注点 新关注点 群号\n" +
                "[CQ:emoji,id=128310]查询关注\n" +
                "[CQ:emoji,id=10024]      Whu Helper       [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
