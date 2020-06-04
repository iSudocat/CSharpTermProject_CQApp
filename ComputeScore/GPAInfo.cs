using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeScore
{
  public  class GPAInfo
    {
        public float GPA;
        
        public float AverageScore;

        public float CreditSum;

        public GPAInfo(float GPA,float AverageScore,float CreditSum)
        {
            this.GPA = GPA;
            this.AverageScore = AverageScore;
            this.CreditSum = CreditSum;
        }
    }
}
