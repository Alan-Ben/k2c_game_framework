namespace GOE
{
    //好友拜访node
    public class GFriendVisitNode : UIQueueBaseNode
    {
        private NPCommonSimplePlayerInfo _m_playerInfo;

        public GFriendVisitNode(NPCommonSimplePlayerInfo _info)
            : base(EUIQueueStageType.MAIN, UINodeTagConst_Friends.C_ADD_FRIEND_VISIT_NODE)
        {
            _m_playerInfo = _info;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }
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
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            NPPlayer.instance.friendsComp.setVisitFriendInfo(_m_playerInfo);

            _afterTransBkAction();
        }
    
        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance);

            GGUIWndFriendsVisit.instance.regLoadDoneDelegate(
                () =>
                {
                    GGUIWndFriendsVisit.instance.showWnd();
                    GGUIWndFriendsVisit.instance.setPlayerInfo(_m_playerInfo);
                });
        }

        public override void QuitNode()
        {
            NPPlayer.instance.friendsComp.setVisitFriendInfo(null);

            //隐藏窗口
            if (null != GGUIWndFriendsVisit.instance)
                GGUIWndFriendsVisit.instance.hideWnd();
        }

        public override void onEnterQueue()
        {
            GGUIWndFriendsVisit.instance.load();
        }

        public override void onClose()
        {
            base.onClose();
            //释放窗口
            GGUIWndFriendsVisit.instance.discard();
        }
    }
}
