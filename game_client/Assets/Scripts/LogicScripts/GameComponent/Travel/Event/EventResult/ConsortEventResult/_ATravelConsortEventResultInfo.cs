using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 妃子事件结果信息
    /// </summary>
    public abstract class _ATravelConsortEventResultInfo : _ATravelEventResultInfo
    {
        public _ATravelConsortEventResultInfo(_ATravelEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings) : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            
        }

        public abstract _IConsortShowInfo consortShowInfo { get; }
        
        /// <summary>
        /// 增加的亲密度
        /// </summary>
        public abstract long addIntimacy { get; }
        
        /// <summary>
        /// 在事件完成后的亲密度
        /// </summary>
        public abstract long afterEventIntimacy { get; }

        /// <summary>
        /// 增加的好感度
        /// </summary>
        public abstract long addLike { get; }
        
        /// <summary>
        /// 在事件完成后的好感度
        /// </summary>
        public abstract long afterEventLike { get; }
    }
}