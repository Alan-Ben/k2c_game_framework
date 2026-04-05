using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 事件显示结果
    /// </summary>
    public interface _IEventResultShowInfo
    {
        /// <summary>
        /// 奖励列表
        /// </summary>
        List<NPCommon.NPCommon_ItemInfo> rewardList { get; }

        /// <summary>
        /// 额外信息
        /// </summary>
        byte[] extraInfo { get; }
    }
}