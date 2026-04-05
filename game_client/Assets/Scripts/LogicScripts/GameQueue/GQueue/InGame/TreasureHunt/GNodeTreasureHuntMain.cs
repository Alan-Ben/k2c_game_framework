namespace GOE
{
    public class GNodeTreasureHuntMain : BaseQueueNode
    {
        private GNodeTreasureHuntMain() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_MAIN)
        {
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
            // MainAdditionTreasureHuntMainEntryTDScene.instance.enterScene();
            GMainGUIAddSceneTreasureHuntMainEntry.instance.enterScene();
        }

        public override void onClose()
        {
            // MainAdditionTreasureHuntMainEntryTDScene.instance.quitScene();
            GMainGUIAddSceneTreasureHuntMainEntry.instance.quitScene();
        }

        public override void EnterNode()
        {
            // GTDSceneMain.instance.showMainScene(MainAdditionTreasureHuntMainEntryTDScene.instance);
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneTreasureHuntMainEntry.instance);
        }

        public override void QuitNode()
        {
            // MainAdditionTreasureHuntMainEntryTDScene.instance.hideScene();
            // GMainGUIAddSceneTreasureHuntMainEntry.instance.hideScene();
        }

        #region 添加Node方法

        public static void addNode()
        {
            if ((QueueMgr.instance.findLastNode(typeof(GNodeTreasureHuntMain)) is GNodeTreasureHuntMain treasureHuntMainNode))
            {
                QueueMgr.instance.QuitUntilCanStop((_node) => _node == treasureHuntMainNode);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeTreasureHuntMain());
            }
        }
        
        #endregion
    }
}