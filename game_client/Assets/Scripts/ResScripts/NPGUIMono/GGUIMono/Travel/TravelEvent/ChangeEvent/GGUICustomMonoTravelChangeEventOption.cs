using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历交换事件选项
    /// </summary>
    public class GGUICustomMonoTravelChangeEventOption : MonoBehaviour
    {
        [ALHeader("是否是要交换的选项")]
        public bool isChangeOption;
        
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;

        // [ALHeader("交换道具列表")]
        // public NPGGUIMonoCommonItemContainer monoExchangeItemList;

        [ALHeader("选择按钮")]
        public GameObject btnSelect;

        // [ALHeader("点击效果")]
        // public _NPPlayerEffectSerializeInfo clickEffect;

#if NP_GAME
        /// <summary>
        /// 消耗道具
        /// </summary>
        private NPGGUIWndCommonItem _m_wCostItem;
        
        // /// <summary>
        // /// 交换得到的道具列表
        // /// </summary>
        // private NPGGUIWndCommonItemContainer _m_wExchangeItemList;  
#endif

        private void Awake()
        {
#if NP_GAME
            if(monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(monoCostItem);
            
            // if(monoExchangeItemList != null)
            //     _m_wExchangeItemList = new NPGGUIWndCommonItemContainer(monoExchangeItemList);      
#endif
            
            ALUGUICommon.combineBtnClick(btnSelect, _onBtnSelect);
        }

        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnSelect, _onBtnSelect);

#if NP_GAME
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            
            // _m_wExchangeItemList?.discard();
            // _m_wExchangeItemList = null;
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            _ATravelEventInfo curDealEvent = NPPlayer.instance.travelComp.curDealEvent;
            if(curDealEvent == null || !(curDealEvent is TravelChangeEventInfo changeEventInfo) || changeEventInfo.changeEventRefObj == null)
                return;

            if (isChangeOption && changeEventInfo.changeEventRefObj.event_cost != null && changeEventInfo.changeEventRefObj.event_cost.IsValid)
            {
                if (_m_wCostItem != null)
                {
                    _m_wCostItem.showWnd();
                    _m_wCostItem.setItem(changeEventInfo.changeEventRefObj.event_cost);
                }
            }
            else
            {
                _m_wCostItem?.hideWnd();
            }

            // if (_m_wExchangeItemList != null)
            // {
            //     _m_wExchangeItemList.showWnd();
            //     _m_wExchangeItemList.showItemList(changeEventInfo.changeEventRefObj.exchange_item_list);
            // }
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            _m_wCostItem?.hideWnd();
            // _m_wExchangeItemList?.hideWnd();
#endif
        }

        private void _onBtnSelect(GameObject _go)
        {
#if NP_GAME
            _ATravelEventInfo curDealEvent = NPPlayer.instance.travelComp.curDealEvent;
            if(curDealEvent == null || !(curDealEvent is TravelChangeEventInfo changeEventInfo) || changeEventInfo.changeEventRefObj == null)
                return;
            
            if(isChangeOption && changeEventInfo.changeEventRefObj.event_cost != null && changeEventInfo.changeEventRefObj.event_cost.IsValid && !GCommon.isItemEnough(changeEventInfo.changeEventRefObj.event_cost, true))
                return;

            if (isChangeOption)//若是需要交换的选项
            {
                changeEventInfo.needChange = true;
                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_DIALOG_OPTION, changeEventInfo.changeEventRefObj.change_dialog_option_id);

                // 【优化-2】游玩-体力事件，增加上浮提示
                // https://www.teambition.com/task/67ff1787caae141d7896b463
                if (changeEventInfo.changeEventRefObj.exchange_item_list != null)
                {
                    foreach (var rewardItem in changeEventInfo.changeEventRefObj.exchange_item_list)
                    {
                        if (rewardItem != null)
                        {
                            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_somethingAdd_str_num, rewardItem.getItemName(), rewardItem.count));
                            break;
                        }
                    }
                }
            }
            else
            {
                changeEventInfo.needChange = false;
                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_DIALOG_OPTION, changeEventInfo.changeEventRefObj.not_change_dialog_option_id);
            }
            
            // if(clickEffect != null && !clickEffect.isEmpty)
            //     clickEffect.dealEffect();
            
#endif
        }
    }
}