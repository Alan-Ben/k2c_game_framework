using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 剧情对话节点
    /// </summary>
    public class GMainQueuePlotDialogueNode : UIQueueBaseNode
    {
        private NPDialogueRefObj _m_refObj;//对话配置
        private Action _m_aOnStart;//对话开始回调
        private Action _m_aDoneAction;//对话结束回调
        private Action _m_aOnClose;//关闭回调
        
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //序列号
        private long _m_lSerialize;
        
        public GMainQueuePlotDialogueNode(NPDialogueRefObj _refObj, Action _onStart, Action _doneAction, Action _onClose = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_PLOT_DIALOG)
        {
            _m_refObj = _refObj;
            _m_aOnStart = _onStart;
            _m_aDoneAction = _doneAction;
            _m_aOnClose = _onClose;
        }

        public bool isMainNode { get { return null != _m_refObj && _m_refObj.is_main_node; } }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return isMainNode; } }
        public override bool isOnlyUINode { get { return true; } }
        
        public bool isNeedBk { get { return null == _m_refObj ? false : _m_refObj.is_need_bk; } }
        
        public override void EnterNode()
        {
            //如果全屏窗口，直接展示，也不需要背景模糊
            if (isMainNode)
            {
                GUISceneMain.instance.showMainWnd(GGUIWndPlotDialogue.instance, () =>
                {
                    GGUIWndPlotDialogue.instance.showWnd();
                    GGUIWndPlotDialogue.instance.setInfo(_m_refObj, _m_aDoneAction);
                    _m_aOnStart?.Invoke();
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
                            GGUIWndPlotDialogue.instance
                            , () => { QueueMgr.instance.forceCloseNode(this); }
                            , () =>
                            {
                                if (serialize != _m_lSerialize)
                                    return;

                                _afterTransBkAction();
                            });
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
            if (!GGUIWndPlotDialogue.instance.isLoaded)
                GGUIWndPlotDialogue.instance.load();
            GGUIWndPlotDialogue.instance.regLoadDoneDelegate(
                () =>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (isNeedBk && _m_wTransBk != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndPlotDialogue.instance.getGameObj());
                    else
                        //将窗口移到最前
                        GCommon.moveTransformToLastAndRefreshLayer(GGUIWndPlotDialogue.instance.getGameObj());

                    GGUIWndPlotDialogue.instance.showWnd();
                    GGUIWndPlotDialogue.instance.setInfo(_m_refObj, _m_aDoneAction);
                    _m_aOnStart?.Invoke();
                });
        }
        
        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            GGUIWndPlotDialogue.instance.hideWnd();

            //发送对话结束的id
            if (null != _m_refObj)
                WinMsg.SendMsg(WinMsgType.DIALOG_END, _m_refObj.id);

            //发送mini游戏结束的引导消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.DIALOG_END);
            
            WinMsg.SendMsg(WinMsgType.MSG_NEXT_STEP_TRIGGER, ENextStepTriggerMsgType.DIALOG_END);
        }

        public override void onClose()
        {
            if (_m_aOnClose != null)
                _m_aOnClose();
            _m_aOnClose = null;
        }
    }
}
