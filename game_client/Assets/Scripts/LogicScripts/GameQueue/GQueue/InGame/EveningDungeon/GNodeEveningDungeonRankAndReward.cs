using System;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜
    /// </summary>
    public class GNodeEveningDungeonRankAndReward : UIQueueBaseNode
    {
        //页签类型
        private EEveningDungeonRankAndRewardDetailTabType _m_eTabType;
        //进入完成之后调用的函数
        private Action _m_aOnEnterDone;

        public GNodeEveningDungeonRankAndReward(EEveningDungeonRankAndRewardDetailTabType _selectTabType, Action _onEnterDone = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_EVENING_DUNGEON_RANK_AND_REWARD_DETAIL)
        {
            _m_eTabType = _selectTabType;
            _m_aOnEnterDone = _onEnterDone;
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
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneEveningDungeonRankAndReward.instance, () =>
            {
                //设置选中页签类型
                GMainGUIAddSceneEveningDungeonRankAndReward.instance.setInfo(_m_eTabType);

                _m_aOnEnterDone?.Invoke();
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            //记录当前选中的页签类型
            _m_eTabType = GGUIWndEveningDungeonRankAndRewardDetail.instance.getCurSelectTabType();

            GMainGUIAddSceneEveningDungeonRankAndReward.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GMainGUIAddSceneEveningDungeonRankAndReward.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIAddSceneEveningDungeonRankAndReward.instance.quitScene();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
        }
    }
}