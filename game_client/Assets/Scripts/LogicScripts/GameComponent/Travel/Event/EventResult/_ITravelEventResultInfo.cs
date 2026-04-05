using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 游历事件结果数据接口
    /// </summary>
    public interface _ITravelEventResultInfo
    {
        public _ATravelEventInfo eventInfo { get; }
        /// <summary>
        /// 事件结果描述
        /// </summary>
        public string eventResultDesc { get; }
        
        /// <summary>
        /// 奖励列表
        /// </summary>
        public List<_IItem> showRewardItemList { get; }
        /// <summary>
        /// 增加经验
        /// </summary>
        public long addExp { get; }
        /// <summary>
        /// 旧的收益
        /// </summary>
        public long oldEarnings { get; }
    }
}