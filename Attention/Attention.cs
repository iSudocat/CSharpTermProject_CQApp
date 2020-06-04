using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttentionSpace
{
    //实体类
    public class Attention
    {
        //消息关注者
        [Key, Column(Order = 0)]
        String Noticer { get; set; }

        //消息群
        [Key, Column(Order = 1)]
        String Group { get; set; }

        //关注点
        [Key, Column(Order = 2)]
        String AttentionPoint { get; set; }

        public Attention(String N,String G,String A)
        {
            Noticer = N;
            Group = G;
            AttentionPoint = A;
        }
    }
}
