using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attention
{
    class AttentionService
    {
        //匹配方式
        public String MatchType;

        //关注列表
        public List<Attention> att;

        //添加关注
        public void Add(String SourceQQ,String Attention,String GroupNum) 
        {
            //检查是否包含，如无
            //向数据库插入一条记录
        }

        //删除关注
        public Boolean Remove(String SourceQQ,String Attention,String GroupNum) 
        {
            //检查是否存在，如有
            //删除一条记录
            return true;
        }

        //更新关注
        public Boolean Update(String SourceQQ,String OldAttention,String NewAttention, String GroupNum) 
        {
            //检查是否存在，如有
            //删除之，并插入新的
            return true;
        }

        //查询关注
        public List<Attention> Query(String SourceQQ,String Attention,String GroupNum) 
        {
            //可以以任意项来查询，要分类讨论
            return null;
        }

        //模糊匹配算法
        private Boolean ApproximateMatch(String Sentence, String Attention) 
        {
            return true;
        }

        private Boolean AccurateMatch(String Sentence, String Attention) 
        {
            //isContent()
            return true;
        }
        //匹配:返回关注点，用于输出
        public String IsCare(String SourceQQ, String Attention, String GroupNum) 
        {
            return null;
        }
    }
}
