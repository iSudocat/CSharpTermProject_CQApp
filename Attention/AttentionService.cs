using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttentionSpace
{
public    class AttentionService
    {   
        //匹配方式
        public String MatchType;

        //关注列表
        public List<Attention> Attentions;//绑定UI

        //添加关注
        public void Add(String SourceQQ,String Attention,String GroupNum) 
        {
            using(var dbcontext = new AttentionContext())
            {
                Attention newatt = new Attention(SourceQQ, GroupNum, Attention);
                if (dbcontext.Attentions.FirstOrDefault(p => p.Noticer == SourceQQ && p.Group == GroupNum && p.AttentionPoint == Attention) == null)
                    return; //or throw exception
                dbcontext.Attentions.Add(newatt);
                dbcontext.SaveChanges();
                this.Attentions = QueryAll();
            }
            //检查是否包含，如无
            //向数据库插入一条记录
            //用数据库来更新list
        }

        //删除关注
        public Boolean Remove(String SourceQQ,String Attention,String GroupNum) 
        {
            using (var dbcontext = new AttentionContext())
            {
                Attention temp = dbcontext.Attentions.FirstOrDefault(p => p.Noticer == SourceQQ && p.Group == GroupNum && p.AttentionPoint == Attention);
                if (temp != null)
                {
                    dbcontext.Attentions.Remove(temp);
                    dbcontext.SaveChanges();
                    this.Attentions = QueryAll();
                    return true;
                }
                else
                    return false;
            }
        }

        //更新关注
        public Boolean Update(String SourceQQ,String OldAttention,String NewAttention, String GroupNum) 
        {
            //检查是否存在，如有
            //删除之，并插入新的
            return true;
        }

        public List<Attention> QueryAll()
        {
            using (var dbcontext = new AttentionContext())
            {
                var queryall = dbcontext.Attentions;
                if (queryall != null)
                    return queryall.ToList();
                else throw new Exception("没有监听事件");
            }
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
           
            return true;
        }

        //监听:返回关注点，用于输出
        public String Listening(String SourceQQ, String Attention, String GroupNum, String MatchType) 
        {

            return null;
        }
    }
}
