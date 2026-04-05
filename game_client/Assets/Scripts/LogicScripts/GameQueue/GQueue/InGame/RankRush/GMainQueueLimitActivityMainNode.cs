using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 限时活动界面节点
    /// </summary>
    public class GMainQueueLimitActivityMainNode : UIQueueBaseNode
    {
        //页签类型
        private ELimitActivityTabType _m_eTabType;
        //进入完成之后调用的函数
        private Action _m_aOnEnterDone;

        public GMainQueueLimitActivityMainNode(ELimitActivityTabType _tabType, Action _onEnterDone = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_LIMIT_ACTIVITY_MAIN)
        {
            _m_eTabType = _tabType;
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
            GUIMainSceneLimitActivityMain.instance.regInitDelegate(() =>
            {
                GUISceneMain.instance.showMainScene(GUIMainSceneLimitActivityMain.instance, () =>
                {
                    //设置选中页签类型
                    GUIMainSceneLimitActivityMain.instance.setInfo(_m_eTabType);

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
            _m_eTabType = GGUIWndLimitActivityMain.instance.getCurSelectTabType();

            GUIMainSceneLimitActivityMain.instance.hideScene();
        }

        public override void onEnterQueue()
        {
            base.onEnterQueue();
            GUIMainSceneLimitActivityMain.instance.enterScene();
        }

        public override void onClose()
        {
            base.onClose();
            GUIMainSceneLimitActivityMain.instance.quitScene();
        }

        public override void onRollBackQuit()
        {
            base.onRollBackQuit();
        }
    }
}
