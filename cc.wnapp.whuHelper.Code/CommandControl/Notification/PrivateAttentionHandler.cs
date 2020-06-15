namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    ///  检测是否要注册新的关注点
    /// </summary>
    public class PrivateAttentionHandler : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            //如果包含“取消关注”、“删除关注”...之类的词语，
            //      解析出群号和消息内容/只有群号/只有消息内容
            //      启动AttentionService的Remove线程

            //如果包含“关注”、“监听”、“订阅”...的词语，
            //      解析出关注的语句和关注的群
            //      启动AttentionService的Add线程

            //如果包含 “更改关注”、“更新关注”....的词语，
            //      解析出两个变更的群号/变更的消息内容
            //      启动AttentionService的Update线程

            //如果包含“查看所有监听”/“查看所有关注”的词语，
            //      如果其中有群号，则将群号解析出来并传入线程
            //      启动AttentionService的Get线程

            throw new System.NotImplementedException();
        }

    }
}