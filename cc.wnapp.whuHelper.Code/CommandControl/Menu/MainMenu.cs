using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code.CommandControl.Menu
{
    public class MainMenu : MsgEventControl
    {
        public override int HandleImpl()
        {
            var re =
                "\n[CQ:emoji,id=11088]     菜单     [CQ:emoji,id=11088]\n" +
                "     教务菜单\n" +
                "     课程菜单\n" +
                "     成绩菜单\n" +
                "     日程菜单\n" +
                "     关注菜单\n" +
                "     Git菜单\n" +
                "[CQ:emoji,id=10024]Whu Helper[CQ:emoji,id=10024]";
            Reply(re);
            return 1;
        }
    }
}
