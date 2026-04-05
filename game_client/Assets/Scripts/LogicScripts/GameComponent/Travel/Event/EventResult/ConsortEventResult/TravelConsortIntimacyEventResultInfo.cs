using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 游历亲密度事件结果信息
    /// </summary>
    public class TravelConsortIntimacyEventResultInfo : _ATravelConsortEventResultInfo
    {
        private TravelConsortIntimacyEventInfo _m_ConsortIntimacyEventInfo;//亲密度事件信息
        private _IConsortShowInfo _m_ConsortShowInfo;
        private Common.TravelObj.Travel_EventResultExt_ConsoleIntimacy _m_TravelEventResultExtData;//事件结果扩展数据
        
        public TravelConsortIntimacyEventResultInfo(TravelConsortIntimacyEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings) : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            _m_ConsortIntimacyEventInfo = _eventInfo;
            _m_ConsortShowInfo = new ConsortRefShowInfo(_m_ConsortIntimacyEventInfo?.consortIntimacyEventRefObj?.consort_id ?? 0);

            if(_extData != null && _extData.Length > 0)
            {
                _m_TravelEventResultExtData = new Common.TravelObj.Travel_EventResultExt_ConsoleIntimacy();
                _m_TravelEventResultExtData.readPackage(_extData);
            }
            
        }

        public override _IConsortShowInfo consortShowInfo { get { return _m_ConsortShowInfo; } }

        /// <summary>
        /// 增加好感度事件, 亲密度不变
        /// </summary>
        public override long addIntimacy { get { return _m_ConsortIntimacyEventInfo?.consortIntimacyEventRefObj?.add_intimacy ?? 0; } }

        public override long afterEventIntimacy { get { return (_m_TravelEventResultExtData?.getPreIntimacy() ?? 0) + addIntimacy; } }

        public override long addLike { get { return 0; } }

        public override long afterEventLike
        {
            get
            {
                TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(_m_ConsortShowInfo?.consortId ?? 0);
                return travelConsortInfo?.like ?? 0;
            }
        }
    }
}