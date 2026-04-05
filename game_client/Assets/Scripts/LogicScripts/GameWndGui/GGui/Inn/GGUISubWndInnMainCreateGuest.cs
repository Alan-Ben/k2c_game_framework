using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnMainCreateGuest : _ATALBasicUISubWnd<GGUIMonoInnMainCreateGuest>
    {
        private InnViewMgr _m_viewMgr;
        private GGUIWndCommonLazyCDCountResume _m_lazyCd;
        private NPGGUIWndCommonToggleEx _m_oneKeyToggle;
        private List<NPPlayerEffectSerializeInfo> _m_specialGuestEffectList;
        
        
        public GGUISubWndInnMainCreateGuest(GGUIMonoInnMainCreateGuest _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_lazyCd?.showWnd();
            _m_oneKeyToggle?.showWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_CREATE_GUEST_BUTTON, _onSimulateCreateGuestClick);
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_lazyCd?.hideWnd();
            _m_oneKeyToggle?.hideWnd();
            
            NPGUIAddSceneCenterTip.instance.clearAllTip();
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_CREATE_GUEST_BUTTON, _onSimulateCreateGuestClick);
        }
        protected override void _onReset()
        {
            _m_lazyCd?.resetWnd();
            _m_oneKeyToggle?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_lazyCd?.discard();
            if (_m_oneKeyToggle != null)
            {
                _m_oneKeyToggle.clickDelegate -= _onOneKeyToggleClick;
                _m_oneKeyToggle.discard();
            }

            _m_lazyCd = null;
            _m_oneKeyToggle = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCreateGuest, _onBtnCreateGuestClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoLazyCD != null)
                _m_lazyCd = new GGUIWndCommonLazyCDCountResume(wnd.monoLazyCD);
            if (wnd.monoOneKeyToggle != null)
            {
                _m_oneKeyToggle = new NPGGUIWndCommonToggleEx(wnd.monoOneKeyToggle);
                _m_oneKeyToggle.clickDelegate += _onOneKeyToggleClick;
            }
            
            _m_specialGuestEffectList = NPPlayerEffectSerializeInfo.readEffectList(wnd.specialGuestEffectStr);

            ALUGUICommon.combineBtnClick(wnd.btnCreateGuest, _onBtnCreateGuestClick);
        }


        public void refreshWnd(InnViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            refreshPersonNum();
            _m_lazyCd?.setInfo(GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_lazy_cd_id);
            if (_m_oneKeyToggle != null)
            {
                bool isOneKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.inn_one_key_receive_guest_unlock_id);
                _m_oneKeyToggle.setSelected(isOneKeyUnlock ? AccountSettingMgr.instance.accountSetting.isInnOneKeyCreateGuest : false);
            }
        }
        public void refreshPersonNum()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            long personNum = _m_viewMgr?.guestNum ?? 0;
            ALUGUICommon.setLabelTxt(wnd.txtPersonNum, personNum);
            ALUGUICommon.setGameObjEnable(wnd.listNoPersonHide, personNum > 0);
        }
        public void playPersonAddEffect(long _personAdd)
        {
            if (wnd == null || !_m_bIsShow || _personAdd <= 0)
                return;

            if (wnd.transPersonAddTipPos != null && wnd.personAddTipId > 0)
            {
                Vector2 screenPos = GCommon.getUIRootPos(wnd.transPersonAddTipPos.position);
                string text = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _personAdd);
                NPGUIAddSceneCenterTip.instance.showTextTip(text, wnd.personAddTipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); });
            }

            if (wnd.animPersonAdd != null)
                wnd.animPersonAdd.Play(wnd.animNamePersonAdd);
        }


        private void _onBtnCreateGuestClick(GameObject _)
        {
            InnSpecialGuestInfo specialGuest = NPPlayer.instance.innComp.tryGetFirstSpecialGuest();
            if (specialGuest != null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_serveTheSpcialGuestFirst_none);
                NPPlayerEffectSerializeInfo.dealEffect(_m_specialGuestEffectList, null);
                return;
            }

            int guestNum = 1;
            if (_m_oneKeyToggle is { isOn: true })
                guestNum = NPPlayer.instance.lazyCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_lazy_cd_id);

            _m_viewMgr?.createGuest(guestNum);
        }
        private void _onOneKeyToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_oneKeyToggle == null)
                return;
            
            NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.inn_receive_guest_lazy_cd_id);
            bool isOneKeyUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
            if (!isOneKeyUnlock)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlockRef.getUnlockTip());
                return;
            }
            
            _m_oneKeyToggle.setSelected(!_m_oneKeyToggle.isOn);
            AccountSettingMgr.instance.accountSetting.setInnOneKeyCreateGuest(_m_oneKeyToggle.isOn);
        }
        
        private void _onSimulateCreateGuestClick()
        {
            if (wnd == null) return;
            _onBtnCreateGuestClick(wnd.btnCreateGuest);
        }
    }
}