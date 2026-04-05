namespace GOE
{
    /// <summary>
    /// 本地推送数据项
    /// </summary>
    public struct LocalPushItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 内容
        /// </summary>
        public string content;
        /// <summary>
        /// 剩余时间（秒）
        /// </summary>
        public long leftTimeSec;

        public LocalPushItem(string _title, string _content, long _leftTimeSec)
        {
            this.title = _title;
            this.content = _content;
            this.leftTimeSec = _leftTimeSec;
        }
    }
}
