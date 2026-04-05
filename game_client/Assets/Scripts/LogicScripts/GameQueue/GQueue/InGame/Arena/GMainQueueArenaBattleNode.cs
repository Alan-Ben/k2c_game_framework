
namespace GOE
{
    /// <summary>
    /// 竞技场战斗界面节点
    /// </summary>
    public class GMainQueueArenaBattleNode : UIQueueBaseNode
    {
        public GMainQueueArenaBattleNode() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_ARENA_BATTLE)
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
            GMainGUIMainSceneArenaBattle.instance.regInitDelegate(() =>
            {
                GUISceneMain.instance.showMainScene(GMainGUIMainSceneArenaBattle.instance);
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GMainGUIMainSceneArenaBattle.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GMainGUIMainSceneArenaBattle.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIMainSceneArenaBattle.instance.quitScene();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
            //如果在战斗中，强制关闭主界面
            if (NPPlayer.instance.arenaComp.isInBattle())
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_MAIN);
        }
    }
}
