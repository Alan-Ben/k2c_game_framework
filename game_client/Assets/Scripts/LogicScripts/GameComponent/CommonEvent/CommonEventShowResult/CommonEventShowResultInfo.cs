using System.Collections.Generic;
using Common.EventObj;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 通用事件显示信息
    /// </summary>
    public class CommonEventShowResultInfo : _IEventResultShowInfo
    {
        private string _m_sResultTitle;
        private string _m_sResultDesc;
        private List<NPCommon_ItemInfo> _m_oriRewardItemList;//原本的奖励列表（只剔除不需要展示部分）
        private List<NPCommon_ItemInfo> _m_lRewardList;//普通奖励列表
        private GCommon.GainItemFilterData _m_iGainItemFilterData;//特殊展示奖励列表
        private byte[] _m_iExtraInfo;
        
        public string resultTitle { get { return _m_sResultTitle; } }
        public string resultDesc { get { return _m_sResultDesc; } }

        /// <summary>
        /// 原本的奖励列表（只剔除不需要展示部分）
        /// </summary>
        public List<NPCommon_ItemInfo> oriRewardItemList { get { return _m_oriRewardItemList; } }
        public List<NPCommon_ItemInfo> rewardList { get { return _m_lRewardList; } }
        public GCommon.GainItemFilterData gainItemFilterData { get { return _m_iGainItemFilterData; } }
        public byte[] extraInfo { get { return _m_iExtraInfo; } }

        public CommonEventShowResultInfo(string _resultTitle, string _resultDesc, CommonEvent_DoneInfo _eventDoneInfo)
        {
            updateShowInfo(_resultTitle, _resultDesc, _eventDoneInfo);
        }
        
        public CommonEventShowResultInfo(string _resultTitle, string _resultDesc, List<NPCommon_ItemInfo> _rewardList, byte[] _extraInfo)
        {
            updateShowInfo(_resultTitle, _resultDesc, _rewardList, _extraInfo);
        }

        public void updateShowInfo(string _resultTitle, string _resultDesc, CommonEvent_DoneInfo _eventDoneInfo)
        {
            _m_sResultTitle = _resultTitle;
            _m_sResultDesc = _resultDesc;
            
            if(_m_iGainItemFilterData == null)
                _m_iGainItemFilterData = new GCommon.GainItemFilterData();
            _m_iGainItemFilterData.clear();
            _m_lRewardList?.Clear();
            if (_m_oriRewardItemList == null)
                _m_oriRewardItemList = new List<NPCommon_ItemInfo>();
            _m_oriRewardItemList.Clear();
            if (_eventDoneInfo != null && _eventDoneInfo.getRewardList() != null)
            {
                _m_oriRewardItemList.AddRange(_eventDoneInfo.getRewardList());
                _m_lRewardList = GCommon.commonDealGainSpecialItem(_eventDoneInfo.getRewardList(), ref _m_iGainItemFilterData);
            }
            
            _m_iExtraInfo = _eventDoneInfo?.getExtraInfo();
        }
        
        public void updateShowInfo(string _resultTitle, string _resultDesc, List<NPCommon_ItemInfo> _rewardList, byte[] _extraInfo)
        {
            _m_sResultTitle = _resultTitle;
            _m_sResultDesc = _resultDesc;
            
            if(_m_iGainItemFilterData == null)
                _m_iGainItemFilterData = new GCommon.GainItemFilterData();
            _m_iGainItemFilterData.clear();
            _m_lRewardList?.Clear();
            if (_m_oriRewardItemList == null)
                _m_oriRewardItemList = new List<NPCommon_ItemInfo>();
            _m_oriRewardItemList.Clear();
            if (_rewardList != null)
            {
                _m_oriRewardItemList.AddRange(_rewardList);
                _m_lRewardList = GCommon.commonDealGainSpecialItem(_rewardList, ref _m_iGainItemFilterData);
                _m_lRewardList = GCommon.filterPopWndEnableShowItemList(_m_lRewardList);//过滤掉在弹窗展示奖励时不需要显示的道具
            }
            
            _m_iExtraInfo = _extraInfo;
        }
    }
}