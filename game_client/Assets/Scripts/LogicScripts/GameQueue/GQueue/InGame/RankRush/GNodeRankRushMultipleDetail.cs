using System;

namespace GOE
{
    /// <summary>
    /// 多个冲榜详情界面节点
    /// </summary>
    public class GNodeRankRushMultipleDetail : UIQueueBaseNode
    {
        //活动ID
        private long _m_lActivityId;
        //页签类型
        private ERankRushDetailTabType _m_eTabType;
        //是否需要展示礼包按钮
        private bool _m_bNeedShowGiftBtn;
        //进入完成之后调用的函数
        private Action _m_aOnEnterDone;

        public GNodeRankRushMultipleDetail(long _activityId, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn = false, Action _onEnterDone = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_RANK_RUSH_MULTIPLE_DETAIL)
        {
            _m_lActivityId = _activityId;
            _m_eTabType = _selectTabType;
            _m_bNeedShowGiftBtn = _needShowGiftBtn;
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
            GUIMainSceneRankRushMultipleDetail.instance.regInitDelegate(() =>
            {
                GUISceneMain.instance.showMainScene(GUIMainSceneRankRushMultipleDetail.instance, () =>
                {
                    //设置选中页签类型
                    GUIMainSceneRankRushMultipleDetail.instance.setInfo(_m_lActivityId, _m_eTabType, _m_bNeedShowGiftBtn);

                    _m_aOnEnterDone?.Invoke();
                });
            });
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            //记录当前选中的页签类型
            _m_eTabType = GGUIWndRankRushMultipleDetail.instance.getCurSelectTabType();

            GUIMainSceneRankRushMultipleDetail.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GUIMainSceneRankRushMultipleDetail.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GUIMainSceneRankRushMultipleDetail.instance.quitScene();
            GGUIWndRankRushMultipleDetail.instance.resetRankList();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
        }
    }
}
