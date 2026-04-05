namespace GOE
{
    public interface _IMarsEventInfo
    {
        /// <summary>
        /// 事件配表id
        /// </summary>
        long eventRefId { get; }
        
        /// <summary>
        /// 事件配表数据
        /// </summary>
        MarsEventRefObj eventRefObj { get; }

        /// <summary>
        /// buff_id
        /// </summary>
        long buffId { get; }

        /// <summary>
        /// buff配表数据
        /// </summary>
        NPPlayerBuffRefObj buffRefObj { get; }

        /// <summary>
        /// buff数据
        /// </summary>
        NPPlayerBuffInfo buffInfo { get; }
    }
}