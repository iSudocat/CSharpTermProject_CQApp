using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attention
{
    //实体类
    public class Attention
    {
        //消息关注者
        [Key, Column(Order = 0)]
        String noticer { get; set; }

        //消息群
        [Key, Column(Order = 1)]
        String group { get; set; }

        //关注点
        [Key, Column(Order = 2)]
        String attention { get; set; }
    }
}
