using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 本地推送接口
    /// </summary>
    public interface _ILocalPushDealer
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        bool isValid { get; }
        /// <summary>
        /// 获取推送数据项列表
        /// </summary>
        IReadOnlyList<LocalPushItem> getPushItemList();
    }
}
