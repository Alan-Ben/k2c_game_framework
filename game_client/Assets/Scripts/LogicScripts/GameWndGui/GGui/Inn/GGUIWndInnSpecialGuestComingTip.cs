using ALPackage;
using GC2GS.p034_InnOp;
using GS2GC.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 旅店特殊客人来访提示窗口
    /// </summary>
    public class GGUIWndInnSpecialGuestComingTip : _ATALBasicUIWnd<GGUIMonoInnSpecialGuestComingTip>
    {
        [NotNull] public static GGUIWndInnSpecialGuestComingTip instance { get { return _g_instance ??= new GGUIWndInnSpecialGuestComingTip(); } }
        private static GGUIWndInnSpecialGuestComingTip _g_instance;


        private InnSpecialGuestInfo _m_guestInfo;
        private NPGGuiWndTexture _m_guestIconWnd;


        public GGUIWndInnSpecialGuestComingTip() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoInnSpecialGuestComingTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnSpecialGuestComingTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_guestIconWnd?.showWnd();

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_SPECIAL_GUEST_SERVE_BUTTON, _onSimulateClick);

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_guestIconWnd?.hideWnd();

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_SPECIAL_GUEST_SERVE_BUTTON, _onSimulateClick);
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

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgGuestIcon != null)
                _m_guestIconWnd = new NPGGuiWndTexture(wnd.imgGuestIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }


        public void refreshWnd([NotNull] InnSpecialGuestInfo _guestInfo)
        {
            _m_guestInfo = _guestInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_guestInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtGuestName, _m_guestInfo.nameTranslated);
            _m_guestIconWnd?.setTexture(_m_guestInfo.refObj.small_icon);
        }


        private void _onSimulateClick()
        {
            if (wnd == null)
                return;

            _onBtnClick(wnd.btnClick);
        }
        private void _onBtnClick(GameObject _obj)
        {
            if (_m_guestInfo == null)
                return;

            InnSpecialGuestInfo guestInfo = _m_guestInfo;
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            NPGSClientListener.sendRequestByLog(new GC2GS_034_009_ReqInnServeSpecialGuest(guestInfo.guestId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_034_009_RetInnServeSpecialGuest>((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(serialize);
                    if (!_isSuc)
                        return;

                    ALProcess.CreateProcess()
                        .addDelegateProcess(_complete => { GCommon.enterDialogueNode(guestInfo.refObj.serve_dialogue_id, _complete); })
                        .addDelegateProcess(_complete =>
                        {
                            InnSpecialGuestChoiceRefObj guestChoiceRef = GRefdataCoreMgr.instance.innSpecialGuestChoiceRefCore.getRef(guestInfo.refObj.serve_choice_id);
                            if (guestChoiceRef != null)
                            {
                                GGUIWndInnSpecialGuestServeChoice.instance.refreshWnd(guestChoiceRef);
                                QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndInnSpecialGuestServeChoice.instance, _complete, UINodeTagConst.C_INN_SPECIAL_GUEST_SERVE_CHOICE));
                            }
                            else
                                _complete?.Invoke();
                        })
                        .addDelegateProcess(_complete => { GCommon.enterDialogueNode(guestInfo.refObj.serve_done_dialogue_id, _complete); })
                        // .addDelegateProcess(_complete =>
                        // {
                        //     GGUIWndInnSpecialGuestServed.instance.refreshWnd(_m_guestInfo);
                        //     QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndInnSpecialGuestServed.instance, _complete, UINodeTagConst.C_INN_SPECIAL_GUEST_SERVED));
                        // })
                        .addProcess(() =>
                        {
                            MuseumItemInfo museumItem = NPPlayer.instance.museumComp.getItemInfoById(guestInfo.refObj.first_gain_gift_id);
                            GGUIWndMuseumItemGet.instance.refreshWnd(museumItem);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMuseumItemGet.instance, GGUIWndMuseumItemGet.instance.showWnd, UINodeTagConst.C_MUSEUM_ITEM_GET);
                        })
                        .deal();
                }));
        }
    }
}