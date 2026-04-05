using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 招募兑换按钮
    /// </summary>
    public class GGUISubWndRecruitExchangeBtn : _ATALBasicUISubWnd<GGUISubMonoRecruitExchangeBtn>
    {
        protected RecruitShopInfo _m_iRecruitShopInfo;
        protected _ARecruitItemInfo _m_rRecruitItemInfo;
        
        private NPGGUIWndCommonItem _m_wndCommonCostItem;//招募消耗道具item
        
        public GGUISubWndRecruitExchangeBtn(GGUISubMonoRecruitExchangeBtn _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public event Action onRecruitBtnClick;//招募按钮点击事件

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoCostItem != null)
                _m_wndCommonCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnRecruit, _onRecruitBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnGotoRecruit, _onGotoRecruitBtnClick);
        }
        
        protected override void _onDiscard()
        {
            onRecruitBtnClick = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnRecruit, _onRecruitBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGotoRecruit, _onGotoRecruitBtnClick);
            }

            if (_m_wndCommonCostItem != null)
            {
                _m_wndCommonCostItem.discard();
            }   
            _m_wndCommonCostItem = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            
            _m_wndCommonCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndCommonCostItem?.resetWnd();
        }

        public void setData(RecruitShopInfo _recruitShopInfo, _ARecruitItemInfo _recruitItemInfo)
        {
            _m_iRecruitShopInfo = _recruitShopInfo;
            _m_rRecruitItemInfo = _recruitItemInfo;

            _refreshWnd();
        }
        
        protected void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_rRecruitItemInfo != null && _m_rRecruitItemInfo.recruitRefObj != null && _m_wndCommonCostItem != null)
            {
                _m_wndCommonCostItem.showWnd();
                _m_wndCommonCostItem.setItem(_m_rRecruitItemInfo.recruitRefObj.cost_item);
            }

            ALUGUICommon.setLabelTxt(wnd.txtGotoRecruitDesc, TextTranslate.instance.getLanguage(_m_rRecruitItemInfo?.recruitRefObj?.can_goto_recruit_desc ?? String.Empty));
            
            ERecruitState recruitState = _m_iRecruitShopInfo?.getRecruitState(_m_rRecruitItemInfo, false) ?? ERecruitState.NONE;
            wnd.setRecruitState(recruitState);
        }
        
        /// <summary>
        /// 点击招募按钮
        /// </summary>
        /// <param name="_go"></param>
        protected virtual void _onRecruitBtnClick(GameObject _go)
        {
            //若有自定义点击事件则执行自定义点击事件
            if (onRecruitBtnClick != null)
            {
                onRecruitBtnClick?.Invoke();
                return;
            }
            
            if(_m_rRecruitItemInfo == null || _m_rRecruitItemInfo.recruitRefObj == null)
                return;
            
            ERecruitState recruitState = _m_iRecruitShopInfo?.getRecruitState(_m_rRecruitItemInfo, true) ?? ERecruitState.NONE;
            if (recruitState == ERecruitState.CAN_EXCHANGE_RECRUIT)
            {
                NPMesMgr.instance.showCostItemMes(_m_rRecruitItemInfo.recruitRefObj.cost_item, () =>
                {
                    NPPlayer.instance.recruitComp.reqRecruit(_m_rRecruitItemInfo.recruitId, (_msg) =>
                    {
                    }, null);
                }, null, TransKeyConst.recruit_exchangeSecondCheckWndTitle_none, _m_rRecruitItemInfo.recruitRefObj.exchange_second_check_content);
            }
        }

        protected virtual void _onGotoRecruitBtnClick(GameObject _go)
        {
            if(_m_rRecruitItemInfo == null || _m_rRecruitItemInfo.recruitRefObj == null)
                return;
            
            ERecruitState recruitState = _m_iRecruitShopInfo?.getRecruitState(_m_rRecruitItemInfo, true) ?? ERecruitState.NONE;
            if (recruitState == ERecruitState.CAN_GO_TO_RECRUIT && _m_rRecruitItemInfo.recruitRefObj.can_goto_recruit_goto_effect != null)
            {
                _m_rRecruitItemInfo.recruitRefObj.can_goto_recruit_goto_effect.dealEffect();
            }
        }
        
        /// <summary>
        /// 当背包物品变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onBagItemChg(params object[] _objs)
        {
            if(_m_rRecruitItemInfo == null || _m_rRecruitItemInfo.recruitRefObj == null || _m_rRecruitItemInfo.recruitRefObj.cost_item == null || _m_rRecruitItemInfo.recruitRefObj.cost_item.getItemType() != ENPItemType.BAG_ITEM)
                return;
            
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem))
                return;

            BagItem chgBagItem = (BagItem) _objs[0];
            if (chgBagItem != null && chgBagItem.itemId == _m_rRecruitItemInfo.recruitRefObj.cost_item.subId)
            {
                _refreshWnd();
            }
        }
    }
}