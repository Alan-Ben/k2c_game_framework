using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店主界面
    /// </summary>
    public class GGUIWndInnMain : _ANPGGUIBasicResBarWnd<GGUIMonoInnMain>
    {
        [NotNull] public static GGUIWndInnMain instance { get { return _g_instance ??= new GGUIWndInnMain(); } }
        private static GGUIWndInnMain _g_instance;


        private GGUISubWndInnMainSignboard _m_signboardWnd;
        private GGUISubWndInnMainCreateGuest _m_createGuestWnd;

        private InnViewMgr _m_viewMgr;

        private long _m_lWndShowSerialize = -1;

        public GGUIWndInnMain() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GGUISubWndInnMainCreateGuest createGuestWnd { get { return _m_createGuestWnd; } }


        protected override void _onShowWnd()
        {
            _m_lWndShowSerialize = ALSerializeOpMgr.next();
                
            _m_signboardWnd?.showWnd();
            _m_createGuestWnd?.showWnd();

            refreshWnd();

            NPPlayer.instance.innComp.onLevelChg += refreshLevel;
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg += refreshSpecialGuestShow;
            NPPlayer.instance.rescourceComp.onResourceCountChg += _onResourceCountChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MAIN_STATION_BUTTON, _onBtnStationListClick);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MAIN_MENU_BUTTON, _onBtnMenuClick);
            WinMsg.RegisterMsg(WinMsgType.ON_INN_GET_CASH_REGISTER_REWARD_NODE_CLOSE, _onGetCashRegisterRewardNodeClose);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MAIN_STATION_BUTTON, _onBtnStationListClick);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MAIN_MENU_BUTTON, _onBtnMenuClick);
            WinMsg.UnregisterMsg(WinMsgType.ON_INN_GET_CASH_REGISTER_REWARD_NODE_CLOSE, _onGetCashRegisterRewardNodeClose);
            NPPlayer.instance.innComp.onMaybeSpecialGuestListChg -= refreshSpecialGuestShow;
            NPPlayer.instance.innComp.onLevelChg -= refreshLevel;
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onResourceCountChg;
            
            _m_signboardWnd?.hideWnd();
            _m_createGuestWnd?.hideWnd();

            if (wnd != null)
            {
                _sampleAni(wnd.wndAnimation, wnd.getCashRegisterRewardGuestNumTipShowAniName, 0f);
            }
            
            _m_lWndShowSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_signboardWnd?.resetWnd();
            _m_createGuestWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_signboardWnd?.discard();
            _m_createGuestWnd?.discard();
            
            _m_signboardWnd = null;
            _m_createGuestWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnStationList, _onBtnStationListClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnMenu, _onBtnMenuClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGift, _onBtnGiftClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGuestList, _onBtnGuestListClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoSignboard != null)
                _m_signboardWnd = new GGUISubWndInnMainSignboard(wnd.monoSignboard);
            if (wnd.monoCreateGuest != null)
                _m_createGuestWnd = new GGUISubWndInnMainCreateGuest(wnd.monoCreateGuest);
            
            ALUGUICommon.combineBtnClick(wnd.btnStationList, _onBtnStationListClick);
            ALUGUICommon.combineBtnClick(wnd.btnMenu, _onBtnMenuClick);
            ALUGUICommon.combineBtnClick(wnd.btnGift, _onBtnGiftClick);
            ALUGUICommon.combineBtnClick(wnd.btnGuestList, _onBtnGuestListClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(InnViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_viewMgr == null)
                return;

            _m_signboardWnd?.refreshWnd();
            _m_createGuestWnd?.refreshWnd(_m_viewMgr);
            refreshAffectionNum();
            refreshLevel();
            refreshSpecialGuestShow();
        }
        public void refreshAffectionNum()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            long itemCount = GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.INN_AFFECTION);
            ALUGUICommon.setLabelTxt(wnd.txtAffectionNum, itemCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }
        public void refreshLevel()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long innLevel = NPPlayer.instance.innComp.levelRef?.level ?? 0;
            wnd.setLevel((int)innLevel);
        }
        public void refreshSpecialGuestShow()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            InnSpecialGuestInfo specialGuestInfo = NPPlayer.instance.innComp.tryGetFirstSpecialGuest();
            bool hasSpecialGuest = specialGuestInfo != null;
            wnd.setHasSpecialGuest(hasSpecialGuest);
        }
        
        
        private void _onResourceCountChg(ECurrency _type, long _srcValue, long _destValue)
        {
            if (_type != ECurrency.INN_AFFECTION)
                return;
            
            refreshAffectionNum();
        }
        private void _onBtnStationListClick()
        {
            if (wnd == null)
                return;
            
            _onBtnStationListClick(wnd.btnStationList);
        }
        private void _onBtnStationListClick(GameObject _)
        {
            if (_m_viewMgr == null)
                return;

            GGUIWndInnStationList.instance.refreshWnd(_m_viewMgr);
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndInnStationList.instance, UINodeTagConst.C_INN_STATION_LIST);
        }
        private void _onBtnMenuClick()
        {
            if (wnd == null)
                return;
            
            _onBtnMenuClick(wnd.btnMenu);
        }
        private void _onBtnMenuClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndInnMenu.instance, UINodeTagConst.C_INN_MENU);
        }
        private void _onBtnGiftClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMuseumItemList.instance, GGUIWndMuseumItemList.instance.showWnd, UINodeTagConst.C_MUSEUM_ITEM_LIST);
        }
        private void _onBtnGuestListClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnGuestList.instance, GGUIWndInnGuestList.instance.showWnd, UINodeTagConst.C_INN_GUEST_LIST);
        }
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_MAIN);
        }

        private void _onGetCashRegisterRewardNodeClose(params object[] _objs)
        {
            if(wnd == null || !isShow || _objs == null || _objs.Length < 1 || !(_objs[0] is Common.InnObj.Inn_SettleInfo innSettleInfo))
                return;

            if (innSettleInfo.getReceiveNum() > 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtGetCashRegisterRewardGuestNumTip,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, innSettleInfo.getReceiveNum()));
                _playAni(wnd.wndAnimation, wnd.getCashRegisterRewardGuestNumTipShowAniName);
            }
        }

        #region 动画

        private void _playAni(Animation _ani, string _aniName, Action _playDone = null)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            long serialize = _m_lWndShowSerialize;
            _ani.Play(_aniName, ()=>
            {
                if(serialize != _m_lWndShowSerialize)
                    return;
                
                _playDone?.Invoke();
            });
        }

        private void _sampleAni(Animation _ani, string _aniName, float _normalizeTime)
        {
            if(_ani == null || string.IsNullOrEmpty(_aniName))
                return;
            
            _ani.Sample(_aniName, _normalizeTime);
        }
        
        #endregion
    }
}