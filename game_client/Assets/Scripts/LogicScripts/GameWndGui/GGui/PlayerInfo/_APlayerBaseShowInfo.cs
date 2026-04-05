namespace GOE
{
    /// <summary>
    /// 玩家信息的展示数据抽象类，在UI中存储本数据进行展示处理
    /// </summary>
    public abstract class _APlayerBaseShowInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public abstract long id { get; }
        /// <summary>
        /// 是否上锁
        /// </summary>
        public abstract bool isLock { get; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public abstract bool isExpired { get; }
        /// <summary>
        /// 类型
        /// </summary>
        public abstract NPEnum.ENPItemType itemType { get; }
        /// <summary>
        /// 过期时间戳
        /// </summary>
        public abstract long expiredTimeS { get; }
        /// <summary>
        /// 是否可以显示红点提示
        /// </summary>
        public abstract bool canShowRedTip { get; }
        /// <summary>
        /// 设置为查看状态
        /// </summary>
        public abstract void setIsViewed();
    }
}
