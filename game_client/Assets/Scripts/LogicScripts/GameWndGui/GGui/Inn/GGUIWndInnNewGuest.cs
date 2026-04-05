using ALPackage;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店新客人窗口
    /// </summary>
    public class GGUIWndInnNewGuest : _ATALBasicUIWnd<GGUIMonoInnNewGuest>
    {
        [NotNull] public static GGUIWndInnNewGuest instance { get { return _g_instance ??= new GGUIWndInnNewGuest(); } }
        private static GGUIWndInnNewGuest _g_instance;

        private InnNormalGuestHandbookInfo _m_guestInfo;
        private NPGGuiWndTexture _m_guestIconWnd;
        
        
        public GGUIWndInnNewGuest() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnNewGuest.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnNewGuest.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_guestIconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_guestIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_guestIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_guestIconWnd?.discard();
            _m_guestIconWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCheckGuest, _onBtnCheckGuestClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgGuestIcon != null)
                _m_guestIconWnd = new NPGGuiWndTexture(wnd.imgGuestIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnCheckGuest, _onBtnCheckGuestClick);
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

            _m_guestIconWnd?.setTexture(_m_guestInfo.refObj.small_icon);
        }
        
        
        private void _onBtnCheckGuestClick(GameObject _obj)
        {
            if (_m_guestInfo == null)
                return;
            
            InnNormalGuestHandbookInfo guestInfo = _m_guestInfo;
            NPGSClientListener.sendRequestByLog(new GC2GS_034_008_ReqInnUnlockGuest(guestInfo.guestId),
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    GCommon.enterDialogueNode(guestInfo.refObj.unlock_dialogue_id, () =>
                    {
                        GGUIWndInnNewGuestUnlockSuccess.instance.refreshWnd(guestInfo);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnNewGuestUnlockSuccess.instance, GGUIWndInnNewGuestUnlockSuccess.instance.showWnd,
                            UINodeTagConst.C_INN_NEW_GUEST_UNLOCK_SUCCESS);
                    });
                }));
        }
    }
}