namespace GOE
{
    public class GNodeTreasureHuntCollectAchieve : BaseQueueNode
    {
        private bool _m_bIsFirstEnter;
        
        public GNodeTreasureHuntCollectAchieve() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_COLLECT_ACHIEVE)
        {
            _m_bIsFirstEnter = true;
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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        public override void onEnterQueue()
        {
            GGUIWndTreasureHuntCollectAchieve.instance.load();
        }

        public override void onClose()
        {
            GGUIWndTreasureHuntCollectAchieve.instance.discard();
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndTreasureHuntCollectAchieve.instance, () =>
            {
                if(_m_bIsFirstEnter)
                    GGUIWndTreasureHuntCollectAchieve.instance.moveAchieveStepGrid();
                
                _m_bIsFirstEnter = false;
            });
        }

        public override void QuitNode()
        {
            GGUIWndTreasureHuntCollectAchieve.instance.hideWnd();
        }
    }
}