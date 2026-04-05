using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 游历好感度事件结果信息
    /// </summary>
    public class TravelConsortLikeEventResultInfo : _ATravelConsortEventResultInfo
    {
        private TravelConsortLikeEventInfo _m_ConsortLikeEventInfo;//好感度事件信息
        private _IConsortShowInfo _m_ConsortShowInfo;
        private Common.TravelObj.Travel_EventResultExt_ConsoleLike _m_TravelEventResultExtData;//事件结果扩展数据

        public TravelConsortLikeEventResultInfo(TravelConsortLikeEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings) : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            _m_ConsortLikeEventInfo = _eventInfo;
            _m_ConsortShowInfo = new ConsortRefShowInfo(_m_ConsortLikeEventInfo?.consortLikeEventRefObj?.consort_id ?? 0);

            if(_extData != null && _extData.Length > 0)
            {
                _m_TravelEventResultExtData = new Common.TravelObj.Travel_EventResultExt_ConsoleLike();
                _m_TravelEventResultExtData.readPackage(_extData);
            }
        }

        public override _IConsortShowInfo consortShowInfo { get { return _m_ConsortShowInfo; } }

        /// <summary>
        /// 增加好感度事件, 亲密度不变
        /// </summary>
        public override long addIntimacy { get { return 0; } }

        public override long afterEventIntimacy {
            get
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_ConsortShowInfo?.consortId ?? 0);
                return consortInfo?.intimacy ?? 0;
            }
        }

        public override long addLike { get { return _m_ConsortLikeEventInfo?.consortLikeEventRefObj?.add_like ?? 0; } }

        public override long afterEventLike { get { return (_m_TravelEventResultExtData?.getPreLike() ?? 0) + addLike; } }
    }
}