namespace GOE
{
    /// <summary>
    /// 主城可以跳转到其他界面的推送通知(默认返回时要重新打开, 不需要的话可以重写_onGotoOtherMainViewNode方法)
    /// </summary>
    public abstract class _AMainCityCanJumpPushNotice : _AMainCityPushNotice
    {
        protected bool _m_bIsGotoOtherMainViewNode = false;//是否前往了其他MainView节点, 因为若进入MainView节点才会执行前面节点的QuitNode方法, 导致执行dealHideNotice
        
        protected _AMainCityCanJumpPushNotice(EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
        }
        
        public override void showNotice()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);//先反监听
            WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);//再监听, 避免多次注册
            
            base.showNotice();
        }

        protected sealed override void _onDealerDone()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);//反监听
            
            __onDealerDone();
        }
        
        protected abstract void __onDealerDone();
        
        /// <summary>
        /// 当Node节点变化时
        /// </summary>
        private void _onNodeChg()
        {
            bool selfInNodeQueue = QueueMgr.instance.findLastNode(nodeTag) != null;
            if(!selfInNodeQueue)// 若自身不在队列中，不处理
                return;
            
            BaseQueueNode lastEnableNode = QueueMgr.instance._lastEnableNode;
            if(lastEnableNode == null)
                return;
            
            // Node队列中最后节点是自身节点，则表示没有前往其他节点, 或从其他节点返回
            if (lastEnableNode.nodeTag == nodeTag)
            {
                _m_bIsGotoOtherMainViewNode = false;
            }
            else if(lastEnableNode.IsMainViewNode && !_m_bIsGotoOtherMainViewNode)// 若最后节点为MainView节点，且之前没有前往过其他MainView节点，则表示从当前节点前往了其他MainView节点
            {
                _m_bIsGotoOtherMainViewNode = true;

                _onGotoOtherMainViewNode();
            }
        }

        /// <summary>
        /// 从当前节点前往了其他MainView节点
        /// </summary>
        protected virtual void _onGotoOtherMainViewNode()
        {
            // 默认重置Dealer状态, 以便下次回到主城时可以重新展示
            resetDealer();
        }
    }
}