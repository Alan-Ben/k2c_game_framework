
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 对话notice
    /// </summary>
    public class NoticeDealer_EnterDialogue : NPUINoticeMgr._ANPUINoticeDealer
    {
        private readonly NPDialogueRefObj _m_dialogueRef;
        private Action _m_dealDone;
        private bool _m_bNeedAutoPlay;//是否需要自动播放

        public NoticeDealer_EnterDialogue(long _dialogueId, Action _dealDone = null, bool _needAutoPlay = false)
        {
            _m_dialogueRef = GRefdataCoreMgr.instance.dialogueMap.getRef(_dialogueId);
            _m_dealDone = _dealDone;
            _m_bNeedAutoPlay = _needAutoPlay;
        }
        
        public NoticeDealer_EnterDialogue(NPDialogueRefObj _dialogueRef, Action _dealDone = null, bool _needAutoPlay = false)
        {
            _m_dialogueRef = _dialogueRef;
            _m_dealDone = _dealDone;
            _m_bNeedAutoPlay = _needAutoPlay;
        }
        
        public bool isMainNode { get { return null != _m_dialogueRef && _m_dialogueRef.is_main_node; } }
        
        public bool isNeedBk { get { return null == _m_dialogueRef ? false : _m_dialogueRef.is_need_bk; } }

        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return !isMainNode && isNeedBk; } }
        public override _AALBasicLoadUIWndBasicClass uiObj { get { return NPGGUIWndDialogue.instance; } }
        public override bool isNoticeFullScreen { get { return isMainNode; } }
        public override bool isOnlyUINode { get { return true; } }
        public override string nodeTag { get { return UINodeTagConst.C_MAIN_SPACE_DIALOG; } }


        public override void dealShowNotice()
        {
            if (_m_dialogueRef == null)
            {
                setDealerDone();
                return;
            }
            if (isMainNode)
            {
                GUISceneMain.instance.showMainWnd(NPGGUIWndDialogue.instance, showDelegate);
            }
            else
            {
                GUISceneMain.instance.showAddWnd(NPGGUIWndDialogue.instance, showDelegate);
            }

            void showDelegate()
            {
                //这边要把窗口移到最下面，因为对话窗口是不卸载的，notice的node基类有写TODO notice下配置背景遮罩的窗口都需要卸载，hide再show层级会有问题
                if(null != NPGGUIWndDialogue.instance && NPGGUIWndDialogue.instance.wnd)
                {
                    GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndDialogue.instance.wnd.transform);
                }

                NPGGUIWndDialogue.instance.showWnd();
                if(!NPGGUIWndDialogue.instance.isShowingDialogue)
                    NPGGUIWndDialogue.instance.setInfo(_m_dialogueRef, setDealerDone, _m_bNeedAutoPlay);
            }
        }

        public override void dealHideNotice()
        {
        }

        protected override void _onDealerDone()
        {
            _m_dealDone?.Invoke();
            _m_dealDone = null;
            NPGGUIWndDialogue.instance.hideWnd();
        }
    }
}
