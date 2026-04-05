using System;

namespace GOE
{
    /// <summary>
    /// 关卡派遣事件Node
    /// </summary>
    public class GNodeCommonDispatchEvent : BaseQueueNode
    {
        private CommonSimpleDispatchEventAgent _m_iDispatchEventShowInfo;//派遣事件信息 
        private Action _m_onStart;//事件开始回调
        private Action _m_aOnDealDone;//事件处理完成回调
        private Action _m_aOnCloseNode;//关闭节点回调
        private bool _m_bIsOnlyUINode;//是否是纯UI节点
        
        public GNodeCommonDispatchEvent(CommonSimpleDispatchEventAgent _dispatchEventShowInfo,Action _onStart, Action _onDealDone, Action _onCloseNode, bool _isOnlyUINode = false) : base(EUIQueueStageType.MAIN)
        {
            _m_iDispatchEventShowInfo = _dispatchEventShowInfo;
            _m_onStart = _onStart;
            _m_aOnDealDone = _onDealDone;
            _m_aOnCloseNode = _onCloseNode;
            _m_bIsOnlyUINode = _isOnlyUINode;
        }

        public override bool NeedAutoRemove { get { return false; } }
        public override bool IsMainViewNode { get { return true; } }
        public override bool isOnlyUINode { get { return _m_bIsOnlyUINode; } }

        /// <summary>
        /// 不允许回退退出
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }

        public override void onEnterQueue()
        {
            GUIAddSceneCommonDispatchEvent.instance.enterScene();
            GUIAddSceneCommonDispatchEvent.instance.regInitDelegate(() =>
            {
                GUIAddSceneCommonDispatchEvent.instance.setShowSelectHeroWnd(true);//节点进入队列时设置Scene显示的窗口为事件窗口
            });
        }

        public override void onClose()
        {
            GUIAddSceneCommonDispatchEvent.instance.quitScene();

            _m_aOnCloseNode?.Invoke();
        }

        public override void EnterNode()
        {
            GUIAddSceneCommonDispatchEvent.instance.setShowData(_m_iDispatchEventShowInfo, _m_aOnDealDone);
            GUIAddSceneCommonDispatchEvent.instance.showScene();
            _m_onStart?.Invoke();
        }

        public override void QuitNode()
        {
            GUIAddSceneCommonDispatchEvent.instance.hideScene();
        }

        public override void onCannotEscBack()
        {
            doCloseNode(true);
        }
        
        /// <summary>
        /// _forceClose
        /// </summary>
        /// <param name="_forceClose">为true时, 不管处于什么窗口, 都直接退出节点。为false会先判断若处于选择大臣窗口, 则不退出, 显示事件窗口</param>
        public static void doCloseNode(bool _forceClose)
        {
            if (GUIAddSceneCommonDispatchEvent.instance.showSelectHeroWnd && !_forceClose)//若当前显示的是选择大臣窗口，不退出节点, 显示事件窗口
            {
                GUIAddSceneCommonDispatchEvent.instance.showGGUIWndCommonDispatchEvent();
            }
            else
            {
                QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonDispatchEvent));
            }
        }
    }
}