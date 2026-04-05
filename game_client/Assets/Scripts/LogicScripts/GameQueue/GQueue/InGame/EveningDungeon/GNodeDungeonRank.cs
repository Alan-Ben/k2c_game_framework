using System;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行榜
    /// </summary>
    public class GNodeDungeonRank : UIQueueBaseNode
    {
        //页签类型
        private EDungeonRankTab _m_eInitRankTabType;
        private EEveningDungeonRankAndRewardDetailTabType _m_eInitEveningDungeonRankTabType;
        private bool _m_bIsFirstEnter = true;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_rankTab">若这个参数传NONE, 进入窗口默认配置的页签</param>
        /// <param name="_initEveningDungeonRankTabType"></param>
        public GNodeDungeonRank(EDungeonRankTab _rankTab, EEveningDungeonRankAndRewardDetailTabType _initEveningDungeonRankTabType) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_DUNGEON_RANK)
        {
            _m_eInitRankTabType = _rankTab;
            _m_eInitEveningDungeonRankTabType = _initEveningDungeonRankTabType;
            
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
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneDungeonRank.instance, () =>
            {
                if (_m_bIsFirstEnter && _m_eInitRankTabType != EDungeonRankTab.NONE)
                {
                    switch (_m_eInitRankTabType)
                    {
                        case EDungeonRankTab.MIDDAY:
                            GGUIWndDungeonRank.instance.setSelectTab(_m_eInitRankTabType, false);
                            break;
                        
                        case EDungeonRankTab.EVENING:
                            GGUIWndDungeonRank.instance.setSelectEveningDungeonTab(_m_eInitEveningDungeonRankTabType);
                            break;
                    }
                }

                _m_bIsFirstEnter = false;
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            GMainGUIAddSceneDungeonRank.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GMainGUIAddSceneDungeonRank.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GMainGUIAddSceneDungeonRank.instance.quitScene();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
        }
    }
}