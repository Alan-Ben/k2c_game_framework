using ALPackage;
using GC2GS.p034_InnOp;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnGuestListNormalPageGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoInnGuestListNormalPageGridItem>
    {
        private InnNormalGuestHandbookInfo _m_guestInfo;
        private NPGGuiWndTexture _m_iconWnd;
        
        
        public GGUISubWndInnGuestListNormalPageGridItem(GGUIMonoInnGuestListNormalPageGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onBtnGetRewardClick);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _resetGridItem()
        {
        }


        public void refreshWnd(InnNormalGuestHandbookInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_guestInfo.nameTranslated);
            _m_iconWnd?.setTexture(_m_guestInfo.refObj.icon);
            wnd.setUnlock(_m_guestInfo.isUnlock);
            wnd.setCanGetReward(!_m_guestInfo.hadDrawReward && _m_guestInfo.isUnlock);
        }


        private void _onBtnGetRewardClick(GameObject _)
        {
            if (_m_guestInfo == null)
                return;
            
            NPGSClientListener.sendMsgByLog(new GC2GS_034_010_ReqDrawGuestHandbookReward(_m_guestInfo.guestId));
        }
        private void _onBtnDetailClick(GameObject _)
        {
            if (_m_guestInfo == null)
                return;

            GGUIWndInnNormalGuestInfo.instance.refreshWnd(_m_guestInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnNormalGuestInfo.instance, GGUIWndInnNormalGuestInfo.instance.showWnd, UINodeTagConst.C_INN_NORMAL_GUEST_INFO);
        }
    }
}