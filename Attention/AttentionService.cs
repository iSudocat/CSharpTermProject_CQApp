using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace AttentionSpace
{
    public class AttentionService
    {
        //匹配方式
        public String MatchType;

        //关注列表
        public List<Attention> Attentions;//绑定UI

        //监听者列表
        public List<ListenerInfo> Listeners;

        //构造函数
        public AttentionService() 
        {
            Boolean same = false;
            using (var dbcontext = new AttentionContext()) 
            {
                this.Attentions = dbcontext.Attentions.ToList();
            }
            this.MatchType = "Approximate";
            this.Listeners = new List<ListenerInfo>();
            foreach (Attention att in Attentions)
            {
                same = false;
                foreach (ListenerInfo listenerInfo in Listeners)
                {
                    if (listenerInfo.Listener.Equals(att.Listener))
                    {
                        same = true;
                        listenerInfo.Count++;
                        break;
                    }
                }
                if (!same)
                {
                    this.Listeners.Add(new ListenerInfo(att.Listener, 1));
                }
            }
        }

        public void UpdateListeners() 
        {
            Boolean same = false;
            Listeners = new List<ListenerInfo>();
            foreach (Attention att in Attentions)
            {
                same = false;
                foreach (ListenerInfo listenerInfo in Listeners)
                {
                    if (listenerInfo.Listener.Equals(att.Listener))
                    {
                        same = true;
                        listenerInfo.Count++;
                        break;
                    }
                }
                if (!same)
                {
                    this.Listeners.Add(new ListenerInfo(att.Listener, 1));
                }
            }
        }

        //添加关注
        public void Add(String SourceQQ, String Attention, String GroupNum)
        {
            if (Attention.Contains(" "))
                throw new Exception("不允许输入空格和空字符串");
            using (var dbcontext = new AttentionContext())
            {
                Attention newatt = new Attention(SourceQQ, GroupNum, Attention);
                dbcontext.Attentions.Add(newatt);
                dbcontext.SaveChanges();
                this.Attentions = QueryAll();
            }
            UpdateListeners();
        }

        //删除关注
        public Boolean Remove(String SourceQQ, String Attention, String GroupNum)
        {
            using (var dbcontext = new AttentionContext())
            {
                Attention temp = dbcontext.Attentions.FirstOrDefault(p => p.Listener == SourceQQ && p.Group == GroupNum && p.AttentionPoint == Attention);
                if (temp != null)
                {
                    dbcontext.Attentions.Remove(temp);
                    dbcontext.SaveChanges();
                    this.Attentions = QueryAll();
                    UpdateListeners();
                    return true;
                }
                else
                    return false;
            }
        }

        //更新关注
        public Boolean Update(String SourceQQ, String OldAttention, String NewAttention, String GroupNum)
        {
            //检查是否存在，
            //如有删除之，并插入新的
            //允许GroupNum为空，这样就只要查询所有的Attention字段相等的部分
            using (var dbcontext = new AttentionContext())
            {
                var quary = dbcontext.Attentions
                    .Where(att => att.AttentionPoint == OldAttention && att.Group == GroupNum);
                foreach (Attention att in quary)
                {
                    dbcontext.Attentions.Remove(att);
                    dbcontext.Attentions.Add(new Attention(SourceQQ, GroupNum, NewAttention));
                }
                dbcontext.SaveChanges();
                this.Attentions = QueryAll();
            }
            return true;
        }

        public List<Attention> QueryAll()
        {
            using (var dbcontext = new AttentionContext())
            {
                return dbcontext.Attentions.ToList();
            }
        }

        //查询关注
        public List<Attention> Query(String SourceQQ = "", String AttentionPoint = "", String GroupNum = "")
        {
            List<Attention> result = new List<Attention>();
            foreach (Attention att in Attentions)
            {
                if ((SourceQQ == "" || SourceQQ == att.Listener)
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
            //遍历所有表项，匹配到相关的关注点，就将监听者增加到列表中
            foreach (Attention att in Attentions)
            {
                if (!att.Group.Equals(GroupNum))
                    continue;
                if (MatchType == "Approximate")
                {
                    if (ApproximateMatch(Message, att.AttentionPoint))
                        listeners.Add(att.Listener);
                }
                else if (MatchType == "Accurate")
                {
                    if (AccurateMatch(Message, att.AttentionPoint))
                        listeners.Add(att.Listener);
                }
            }
            //返回监听者的qq号的列表
            return listeners;
        }
    }
}
