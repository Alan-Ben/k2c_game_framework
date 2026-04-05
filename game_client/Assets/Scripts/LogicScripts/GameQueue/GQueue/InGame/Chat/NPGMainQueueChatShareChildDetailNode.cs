using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 聊天子嗣分享详情node
    /// </summary>
    public class NPGMainQueueChatShareChildDetailNode : UIQueueBaseNode
    {
        private NPCommon_ChatContent_ChildShare _m_showData;
        
        public NPGMainQueueChatShareChildDetailNode(NPCommon_ChatContent_ChildShare _childShow) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_Main_Chat_SHARE_CHILD_DETAIL)
        {
            _m_showData = _childShow;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndShareChildDetail.instance, () =>
            {
                GGUIWndShareChildDetail.instance.setInfo(_m_showData);
                GGUIWndShareChildDetail.instance.showWnd();
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GGUIWndShareChildDetail.instance.hideWnd();
        }
    }
}
