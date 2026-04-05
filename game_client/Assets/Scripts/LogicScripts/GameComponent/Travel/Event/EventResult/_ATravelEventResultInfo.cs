using System.Collections.Generic;
using CommonEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历事件结果数据抽象类
    /// </summary>
    public abstract class _ATravelEventResultInfo : _ITravelEventResultInfo
    {
        protected _ATravelEventInfo _m_iEventInfo;
        protected List<_IItem> _m_lRewardList;
        protected byte[] _m_extData;
        protected long _m_lAddExp;
        protected long _m_lOldEarnings;//旧的收益
        
        public _ATravelEventResultInfo(_ATravelEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings)
        {
            _m_iEventInfo = _eventInfo;

            _m_lRewardList = new List<_IItem>();
            _m_lAddExp = 0;
            if (_rewardList != null)
            {
                foreach (NPCommon_ItemInfo itemInfo in _rewardList)
                {
                    if(itemInfo == null)
                        continue;

                    // 将玩家经验从奖励列表种分离出来
                    if (itemInfo.getItemType() == (int) ENPItemType.CURRENCY && itemInfo.getSubId() == (long) ECurrency.P_EXP)
                    {
                        _m_lAddExp += itemInfo.getCount();
                        continue;
                    }
                
                    _m_lRewardList.Add(new CommonItemData(itemInfo));
                }
            }
            
            _m_extData = _extData;
            _m_lOldEarnings = _oldEarnings;
        }

        public virtual string eventResultDesc
        {
            get
            {
                TravelEventRefObj travelEventRefObj = _m_iEventInfo?.travelEventRefObj;
                if (travelEventRefObj == null || string.IsNullOrEmpty(travelEventRefObj.eventResultDesc))
                    return string.Empty;

                if (travelEventRefObj.lEventResultDescArgs == null)
                {
                    return TextTranslate.instance.getLanguage(travelEventRefObj.eventResultDesc);
                }
                else
                {
                    return TextTranslate.instance.getLanguage(travelEventRefObj.eventResultDesc, travelEventRefObj.lEventResultDescArgs);
                }
            }
        }

        public _ATravelEventInfo eventInfo { get { return _m_iEventInfo; } }
        public List<_IItem> showRewardItemList { get { return _m_lRewardList; } }
        public long addExp { get { return _m_lAddExp; } }
        public long oldEarnings { get { return _m_lOldEarnings; } }
    }
    
    /// <summary>
    /// 游历事件结果数据 - 指定事件类
    /// </summary>
    public class _ATravelSpecificEventResultInfo<T_EventInfo> : _ATravelEventResultInfo where T_EventInfo : _ATravelEventInfo
    {
        private T_EventInfo _m_specificEventInfo;
        public _ATravelSpecificEventResultInfo(T_EventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings) : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            _m_specificEventInfo = _eventInfo;
        }
        
        public T_EventInfo specificEventInfo { get { return _m_specificEventInfo; } }
    }
}