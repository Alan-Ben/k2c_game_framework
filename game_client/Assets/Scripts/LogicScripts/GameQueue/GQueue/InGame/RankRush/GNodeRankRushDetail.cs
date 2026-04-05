using System;

namespace GOE
{
    /// <summary>
    /// 冲榜详情界面节点
    /// </summary>
    public class GNodeRankRushDetail : UIQueueBaseNode
    {
        //冲榜信息
        private ActivityRankRushInfo _m_rankRushInfo;
        //页签类型
        private ERankRushDetailTabType _m_eTabType;
        //是否需要展示礼包按钮
        private bool _m_bNeedShowGiftBtn;
        //进入完成之后调用的函数
        private Action _m_aOnEnterDone;

        public GNodeRankRushDetail(ActivityRankRushInfo _info, ERankRushDetailTabType _selectTabType, bool _needShowGiftBtn = false, Action _onEnterDone = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_RANK_RUSH_DETAIL)
        {
            _m_rankRushInfo = _info;
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
            GUIMainSceneRankRushDetail.instance.regInitDelegate(() =>
            {
                GUISceneMain.instance.showMainScene(GUIMainSceneRankRushDetail.instance, () =>
                {
                    //设置选中页签类型
                    GUIMainSceneRankRushDetail.instance.setInfo(_m_rankRushInfo, _m_eTabType, _m_bNeedShowGiftBtn);

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
            _m_eTabType = GGUIWndRankRushDetail.instance.getCurSelectTabType();

            GUIMainSceneRankRushDetail.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GUIMainSceneRankRushDetail.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GUIMainSceneRankRushDetail.instance.quitScene();
            GGUIWndRankRushDetail.instance.resetRankList();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
        }
    }
}
