namespace cc.wnapp.whuHelper.Code.CommandRouter
{
    public enum MatchType
    {
        /// <summary>
        /// 开头匹配
        /// </summary>
        Any = 0x0,

        /// <summary>
        /// 开头匹配
        /// </summary>
        StartsWith = 0x1,

        /// <summary>
        /// 中间匹配（包括开头）
        /// </summary>
        Contains = 0x2
    }
}