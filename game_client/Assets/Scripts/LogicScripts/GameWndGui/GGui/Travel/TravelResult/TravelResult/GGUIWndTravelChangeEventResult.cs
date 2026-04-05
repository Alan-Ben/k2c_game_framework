using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelChangeEventResult : _ATravelSpecificEventResultWnd<GGUIMonoTravelChangeEventResult, TravelChangeEventInfo, _ATravelSpecificEventResultInfo<TravelChangeEventInfo>>
    {
        private CommonItemData _m_TravelLazyCdRewardItem;
        
        public GGUIWndTravelChangeEventResult(_ATravelSpecificEventResultInfo<TravelChangeEventInfo> _eventResultInfo) : base(_eventResultInfo, GGUIMonoTravelChangeEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
            if (_eventResultInfo != null && _eventResultInfo.showRewardItemList != null)
            {
                long lazyCdId = GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id;
                _m_TravelLazyCdRewardItem = new CommonItemData(ENPItemType.LAZY_CD, lazyCdId, 0);

                _IItem rewardItem = null;
                for (int i = _eventResultInfo.showRewardItemList.Count - 1; i >= 0; i--)
                {
                    rewardItem = _eventResultInfo.showRewardItemList[i];
                    if (rewardItem != null && rewardItem.getItemType() == ENPItemType.LAZY_CD && rewardItem.subId == lazyCdId)
                    {
                        _m_TravelLazyCdRewardItem.setCount(_m_TravelLazyCdRewardItem.getCount() + rewardItem.getCount());
                        
                        // 取出体力道具后, 从奖励列表中移除
                        _eventResultInfo.showRewardItemList.RemoveAt(i);
                    }
                }
            }
        }

        protected override void _onWndInitDoneSub()
        {
        }

        protected override void _onDiscardSub()
        {
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
        }

        protected override void _onResetSub()
        {
        }

        protected override void _onRefreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_TravelLazyCdRewardItem != null && _m_TravelLazyCdRewardItem.getCount() > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdShow, true);
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdHide, false);

                string travelLazyCdRewardKey = string.IsNullOrEmpty(wnd.gainTravelCostLazyCdKey) ? TransKeyConst.common_somethingAdd_str_num : wnd.gainTravelCostLazyCdKey;
                ALUGUICommon.setLabelTxt(wnd.gainTravelCostLazyCd,
                    TextTranslate.instance.getLanguage(travelLazyCdRewardKey, _m_TravelLazyCdRewardItem.getItemName(),
                        _m_TravelLazyCdRewardItem.getCount()));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdShow, false);
                ALUGUICommon.setGameObjEnable(wnd.hasAddLazyCdHide, true);
            }
        }
    }
}