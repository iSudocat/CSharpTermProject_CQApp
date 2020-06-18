using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttentionSpace
{
    public class ListenerInfo
    {
        public String Listener { get; set; }

        public int Count { get; set; }

        public ListenerInfo(String l, int c)
        {
            this.Listener = l;
            this.Count = c;
        }
    }
}
