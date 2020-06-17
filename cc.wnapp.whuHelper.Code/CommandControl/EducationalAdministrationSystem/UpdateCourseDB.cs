using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eas;
using static Eas.EasOP;

namespace cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem
{
    public class UpdateCourseDB : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                Reply("正在尝试更新课程数据……");
                EasOP.UpdateCourse(fromQQ);
                Reply("更新成功！");
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
