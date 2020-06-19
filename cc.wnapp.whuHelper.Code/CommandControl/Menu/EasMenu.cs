using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class EasMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]         教务菜单         [CQ:emoji,id=11088]\n" +
                "－－－ 私聊命令 －－－\n" +
                "[CQ:emoji,id=128310]绑定教务系统 学号|密码\n" +
                "[CQ:emoji,id=128310]更新成绩（需先绑定）\n" +
                "[CQ:emoji,id=128310]更新课程（需先绑定）\n" +
                "[CQ:emoji,id=10024]      Whu Helper       [CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
