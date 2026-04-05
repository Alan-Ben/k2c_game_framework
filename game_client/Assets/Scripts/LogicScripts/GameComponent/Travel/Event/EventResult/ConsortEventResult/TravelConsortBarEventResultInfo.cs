using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 游历酒馆事件结果
    /// </summary>
    public class TravelConsortBarEventResultInfo : _ATravelConsortEventResultInfo
    {
        private TravelConsortBarEventInfo _m_consortBarEventInfo;//好感度事件信息

        public TravelConsortBarEventResultInfo(TravelConsortBarEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings) : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            _m_consortBarEventInfo = _eventInfo;
        }

        public override _IConsortShowInfo consortShowInfo { get { return _m_consortBarEventInfo?.selectConsortShowInfo; } }

        // 妃子酒馆事件描述比较特殊, 需要代码赋值选中妃子
        public override string eventResultDesc 
        {
            get
            {
                if (_m_iEventInfo == null || _m_iEventInfo.travelEventRefObj == null || string.IsNullOrEmpty(_m_iEventInfo.travelEventRefObj.event_result_desc))
                    return string.Empty;

                return TextTranslate.instance.getLanguage(_m_iEventInfo.travelEventRefObj.event_result_desc, consortShowInfo?.consortTransName);
            }
        }
        
        /// <summary>
        /// 增加好感度事件, 亲密度不变
        /// </summary>
        public override long addIntimacy
        {
            get
            {
                if (_m_consortBarEventInfo == null || _m_consortBarEventInfo.selectBarCostRefObj == null)
                    return 0;

                // 若选择妃子解锁状态为解锁，则该事件增加亲密度
                if (_m_consortBarEventInfo.selectConsortUnlockStat is ETravelConsortUnlockStat.UNLOCK)
                {
                    return _m_consortBarEventInfo.selectBarCostRefObj.add_intimacy;
                }
                
                return 0;
            }
        }

        public override long afterEventIntimacy 
        {
            get
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortBarEventInfo?.selectConsortShowInfo?.consortId ?? 0);
                return consortInfo?.intimacy ?? 0;
            }
        }

        public override long addLike
        {
            get
            {
                if (_m_consortBarEventInfo == null || _m_consortBarEventInfo.selectBarCostRefObj == null)
                    return 0;

                // 若选择妃子解锁状态不为解锁状态，则该事件增加好感度
                if (!(_m_consortBarEventInfo.selectConsortUnlockStat is ETravelConsortUnlockStat.UNLOCK))
                {
                    return _m_consortBarEventInfo.selectBarCostRefObj.add_like;
                }
                
                return 0;
            }
        }

        public override long afterEventLike
        {
            get
            {
                TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(_m_consortBarEventInfo?.selectConsortShowInfo?.consortId ?? 0);
                return travelConsortInfo?.like ?? 0;
            }
        }
    }
}