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
        public void Add(String SourceQQ, String Attention, String GroupNum)
        {
            using (var dbcontext = new AttentionContext())
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
        public Boolean Remove(String SourceQQ, String Attention, String GroupNum)
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
        public Boolean Update(String SourceQQ, String OldAttention, String NewAttention, String GroupNum = "")
        {
            //检查是否存在，如有
            //删除之，并插入新的
            //允许GroupNum为空，这样就只要查询所有的Attention字段相等的部分
            using (var dbcontext = new AttentionContext())
            {
                var quary = dbcontext.Attentions
                    .Where(att => att.AttentionPoint == OldAttention && (att.Group == GroupNum || GroupNum == ""));
                foreach (Attention att in quary)
                {
                    att.AttentionPoint = NewAttention;
                }
                dbcontext.SaveChanges();
            }
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
        public List<Attention> Query(String SourceQQ = "", String AttentionPoint = "", String GroupNum = "")
        {
            List<Attention> result = new List<Attention>();
            foreach (Attention att in result)
            {
                if ((SourceQQ == "" || SourceQQ == att.Noticer)
                    && (AttentionPoint == "" || att.AttentionPoint == AttentionPoint)
                    && (GroupNum == "" || GroupNum == att.Group))
                    result.Add(att);
            }
            return result;
        }

        //模糊匹配算法
        private Boolean ApproximateMatch(String Sentence, String AttentionPoint)
        {
            return Sentence.Contains(AttentionPoint);
        }

        private Boolean AccurateMatch(String Sentence, String AttentionPoint)
        {
            return Sentence.Contains(AttentionPoint);
        }

        //监听:返回关注点，用于输出
        public List<String> Listening(String Message, String GroupNum)
        {
            //创建监听者qq号列表对象
            List<String> listeners = new List<String>();
            //查询群号相关的项，保存在内存中
            List<Attention> attentions = Query("", "", GroupNum);
            //遍历所有表项，匹配到相关的关注点，就将监听者增加到列表中
            foreach (Attention att in attentions)
            {
                if (MatchType == "Approximate")
                {
                    if (ApproximateMatch(Message, att.AttentionPoint))
                        listeners.Add(att.Noticer);
                }
                else if (MatchType == "Accurate")
                {
                    if (AccurateMatch(Message, att.AttentionPoint))
                        listeners.Add(att.Noticer);
                }
            }
            //返回监听者的qq号的列表
            return listeners;
        }
    }
}
