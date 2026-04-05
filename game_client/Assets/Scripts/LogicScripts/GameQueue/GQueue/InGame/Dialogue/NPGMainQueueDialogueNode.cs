using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 对话节点
    /// </summary>
    public class NPGMainQueueDialogueNode : UIQueueBaseNode
    {
        private NPDialogueRefObj _m_refObj;//对话配置
        private Action _m_aDoneAction;//对话展示完成回调(这时对话Node还未退出)
        private bool _m_bNeedAutoPlay;//是否需要自动播放
        private Action _m_aDealCloseDialog;//关闭对话处理
        
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //序列号
        private long _m_lSerialize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_refObj"></param>
        /// <param name="_doneAction">对话展示完成回调(这时对话Node还未退出)</param>
        /// <param name="_needAutoPlay"></param>
        /// <param name="_dealCloseDialog">关闭对话处理, 若非null, 则不会自动退出对话, 会调用这个方法, 应该由外部调用者关闭对话</param>
        public NPGMainQueueDialogueNode(NPDialogueRefObj _refObj, Action _doneAction, bool _needAutoPlay = false, Action _dealCloseDialog = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_SPACE_DIALOG)
        {
            _m_refObj = _refObj;
            _m_aDoneAction = _doneAction;
            _m_bNeedAutoPlay = _needAutoPlay;
            _m_aDealCloseDialog = _dealCloseDialog;
        }
        
        public bool isMainNode { get { return null != _m_refObj && _m_refObj.is_main_node; } }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return isMainNode; } }
        public override bool isOnlyUINode { get { return true; } }

        public bool isNeedBk { get { return null == _m_refObj ? false : _m_refObj.is_need_bk; } }
        
        public override void EnterNode()
        {
            GDialogueShowMgr.instance.setIsDialogueShow(true);
            //如果全屏窗口，直接展示，也不需要背景模糊
            if (isMainNode)
            {
                GUISceneMain.instance.showMainWnd(NPGGUIWndDialogue.instance, () =>
                {
                    NPGGUIWndDialogue.instance.showWnd();
                    NPGGUIWndDialogue.instance.setInfo(_m_refObj, _onDialogDone, _m_bNeedAutoPlay, _m_aDealCloseDialog);

                    //通用对话开始，（政务窗口需要展示在最上层，这里发送消息确保对话窗口已经打开再设置政务窗口位置）
                    WinMsg.SendMsg(WinMsgType.ON_COMMON_DIALOG_START);
                });
            }
            //非全屏窗口处理模糊背景
            else
            {
                //是否需要遮罩
                if (isNeedBk)
                {
                    if (null != _m_wTransBk)
                    {
                        _m_wTransBk.showWnd();
                        _afterTransBkAction();
                    }
                    else
                    {
                        _m_lSerialize = ALSerializeOpMgr.next();
                        long serialize = _m_lSerialize;

                        _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                            NPGGUIWndDialogue.instance
                            , () => { onCannotEscBack(); }
                            , () =>
                            {
                                if (serialize != _m_lSerialize)
                                    return;

                                _afterTransBkAction();
                            });
                    
                        //对话这边背景遮罩的压暗值单独设置
                        if (_m_wTransBk != null)
                            _m_wTransBk.regLoadDoneDelegate(() => { _m_wTransBk.setColorInfo(GRefdataCoreMgr.instance.npGeneral.dialog_wnd_blue_bk_alpha); });
                    }
                }
                else
                {
                    _afterTransBkAction();
                }
            }
        }
        
        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            //加载窗口再显示
            //防止重复load
            if (!NPGGUIWndDialogue.instance.isLoaded)
                NPGGUIWndDialogue.instance.load();
            NPGGUIWndDialogue.instance.regLoadDoneDelegate(
                () =>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (isNeedBk && _m_wTransBk != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), NPGGUIWndDialogue.instance.getGameObj());
                    else
                        //将窗口移到最前
                        GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndDialogue.instance.getGameObj());
                    
                    if (!NPGGUIWndDialogue.instance.isShow)
                    {
                        NPGGUIWndDialogue.instance.showWnd();
                        NPGGUIWndDialogue.instance.setInfo(_m_refObj, _onDialogDone, _m_bNeedAutoPlay);
                    }

                    //通用对话开始，（政务窗口需要展示在最上层，这里发送消息确保对话窗口已经打开再设置政务窗口位置）
                    WinMsg.SendMsg(WinMsgType.ON_COMMON_DIALOG_START);
                });
        }

        public override void QuitNode()
        {
            
            GDialogueShowMgr.instance.setIsDialogueShow(false);
            _m_lSerialize = ALSerializeOpMgr.next();
            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            NPGGUIWndDialogue.instance.hideWnd();

            //发送对话结束的id
            if(null != _m_refObj)
                WinMsg.SendMsg(WinMsgType.DIALOG_END, _m_refObj.id);

            //发送mini游戏结束的引导消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.DIALOG_END);
            
            WinMsg.SendMsg(WinMsgType.MSG_NEXT_STEP_TRIGGER, ENextStepTriggerMsgType.DIALOG_END);
        }

        public override void onClose()
        {
            _m_aDealCloseDialog = null;
            
            _onDialogDone();
        }

        private void _onDialogDone()
        {
            Action action = _m_aDoneAction;
            _m_aDoneAction = null;
            action?.Invoke();
        }
        
        /// <summary>
        /// 禁止esc回退
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }
        public override void onCannotEscBack()
        {
            NPGGUIWndDialogue.instance.forceDialogueEnd();
        }

        public static void forceCloseNode()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGMainQueueDialogueNode));
        }
    }
}
