namespace GOE
{
    /// <summary>
    /// 一些事件通用的展示接口
    /// </summary>
    public interface _ICommonEventShowInfo
    {
        /// <summary>
        /// 事件名
        /// </summary>
        string eventName { get; }
        
        /// <summary>
        /// 事件简易描述
        /// </summary>
        string eventSimpleDesc { get; }
        
        /// <summary>
        /// 事件在列表中icon
        /// </summary>
        NPGTextureIndex inEventListIcon { get; }
     
        /// <summary>
        /// 事件详细描述
        /// </summary>
        string eventDetailDesc { get; }
    }
}