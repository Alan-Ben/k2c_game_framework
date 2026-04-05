namespace GOE
{
    public class GNodeTreasureHuntSelectArea : BaseQueueNode
    {
        public GNodeTreasureHuntSelectArea() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_SELECT_AREA)
        {
            TreasureHuntAreaRefObj selectedAreaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(NPPlayer.instance.treasureHuntComponent.saver?.getAreaId() ?? 0);
            if (selectedAreaRefObj == null)
                selectedAreaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.refList?.GetFirst();
            GGUIWndTreasureHuntSelectArea.instance.setSelectedArea(selectedAreaRefObj);
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

        public override bool isOnlyUINode { get { return true; } }
        
        public override void onEnterQueue()
        {
            GGUIWndTreasureHuntSelectArea.instance.load();
        }

        public override void onClose()
        {
            GGUIWndTreasureHuntSelectArea.instance.discard();
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndTreasureHuntSelectArea.instance);
        }

        public override void QuitNode()
        {
            GGUIWndTreasureHuntSelectArea.instance.hideWnd();
        }
    }
}