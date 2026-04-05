using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnSpecialGuestInfo : _ATALBasicUIWnd<GGUIMonoInnSpecialGuestInfo>
    {
        [NotNull] public static GGUIWndInnSpecialGuestInfo instance { get { return _g_instance ??= new GGUIWndInnSpecialGuestInfo(); } }
        private static GGUIWndInnSpecialGuestInfo _g_instance;

        
        private InnSpecialGuestHandbookInfo _m_guestInfo;

        private NPGGuiWndTexture _m_iconWnd;
        private GGUISubWndInnGuestUnlockTipContainer _m_unlockTipContainer;
        

        public GGUIWndInnSpecialGuestInfo()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnSpecialGuestInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnSpecialGuestInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_unlockTipContainer?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.showWnd();
            _m_unlockTipContainer?.showWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_unlockTipContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            _m_unlockTipContainer?.discard();
            _m_unlockTipContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnStory, _onBtnStoryClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.monoUnlockTipContainer != null)
                _m_unlockTipContainer = new GGUISubWndInnGuestUnlockTipContainer(wnd.monoUnlockTipContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnStory, _onBtnStoryClick);
        }
        
        
        public void refreshWnd(InnSpecialGuestHandbookInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;
            
            _m_iconWnd?.setTexture(_m_guestInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_guestInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_guestInfo.descTranslated);
            _m_unlockTipContainer?.refreshWnd(_m_guestInfo);
            wnd.setUnlock(_m_guestInfo.isUnlock);
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_SPECIAL_GUEST_INFO);
        }
        private void _onBtnStoryClick(GameObject _)
        {
            // todo:
        }
    }
} 