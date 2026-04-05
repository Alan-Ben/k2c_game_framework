using System;

namespace GOE
{
    /// <summary>
    /// 日常窗口
    /// </summary>
    public class GMainDailyMainNode : UIQueueBaseNode
    {
        //进入回调
        private Action _m_onEnter;
        public GMainDailyMainNode(Action _onEnter = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_DAILY)
        {
            _m_onEnter = _onEnter;
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
            GMainGUIAddSceneDailyMainInfo.instance.regInitDelegate(
                () =>
                {
                    GUISceneMain.instance.showMainScene(GMainGUIAddSceneDailyMainInfo.instance);
                });

            _m_onEnter?.Invoke();
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GMainGUIAddSceneDailyMainInfo.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            GMainGUIAddSceneDailyMainInfo.instance.enterScene();
        }

        public override void onClose()
        {
            GGUIWndDailyMain.instance.resetSelect();
            GMainGUIAddSceneDailyMainInfo.instance.quitScene();
        }
    }
}
