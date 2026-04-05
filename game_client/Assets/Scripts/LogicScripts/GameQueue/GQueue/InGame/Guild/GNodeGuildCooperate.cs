
namespace GOE
{
    /// <summary>
    /// 联盟协作Node
    /// </summary>
    public class GNodeGuildCooperate : UIQueueBaseNode
    {
        public GNodeGuildCooperate() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_GUIlD_COOPERATE_MAIN)
        {
        }
        
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
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

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            base.onEnterQueue();

            // 记录本次进入的联盟协作刷新时间
            NPPlayer.instance.guildCooperateComp.recordCurShowRefreshTimeMs = NPPlayer.instance.guildCooperateComp.nextRefreshTimeMs;
        }

        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            // 重置当前选中的区域id
            GGUIWndGuildCooperateMain.instance.resetCurShowAreaId();
            // 重置记录刷新时间
            NPPlayer.instance.guildCooperateComp.recordCurShowRefreshTimeMs = NPPlayer.instance.guildCooperateComp.nextRefreshTimeMs;
        }

        public override void EnterNode()
        {
            // 打开界面
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneGuildCooperate.instance);
        }

        public override void QuitNode()
        {
        }
    }
}