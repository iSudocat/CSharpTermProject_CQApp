using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeScore
{
  static  class LevenshteinDistance
    {
        ///<summary>
        ///Contains approximate string matching
        ///</summary>
        ///
        public static int ComputeDistance(string first,string second)
        {
            int n = first.Length;
            int m = second.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
                return m;

            if (m == 0)
                return n;

            for(int i = 0; i <= n; d[i,0] = i++)
            {
            }

            for(int j = 0; j <= m; d[0,j] = j++)
            {
            }

            for(int i = 1; i <= n; i++)
            {
                for(int j = 1; j <= m; j++)
                {
                    int cost = (second[j - 1] == first[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(d[i - 1, j] + 1,Math.Min(d[i, j - 1] + 1, d[i - 1, j - 1] + cost));
                }
            }
            return d[n, m];
        }
    }
}
