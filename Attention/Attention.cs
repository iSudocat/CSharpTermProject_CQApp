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
        public String Listener { get; set; }

        //消息群
        [Key, Column(Order = 1)]
        public String Group { get; set; }

        //关注点
        [Key, Column(Order = 2)]
        public String AttentionPoint { get; set; }

        public Attention() 
        {
            return;
        }
        public Attention(String N,String G,String A)
        {
            Listener = N;
            Group = G;
            AttentionPoint = A;
        }
    }
}
