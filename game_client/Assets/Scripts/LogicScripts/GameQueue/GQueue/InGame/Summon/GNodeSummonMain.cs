using ALPackage;

namespace GOE
{
    /// <summary>
    /// 召唤系统主页面
    /// </summary>
    public class GNodeSummonMain : BaseQueueNode
    {
        // //回退按钮
        // private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;
        
        public GNodeSummonMain() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_SUMMON_MAIN)
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
        
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(GGUIAddSceneSummon.instance, () =>
            {
                _showBackBtn();
            });
        }

        public override void QuitNode()
        {
            // if (null != _m_cbCommonBackObj)
            // {
            //     NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
            //     _m_cbCommonBackObj = null;
            // }
            
            GGUIAddSceneSummon.instance.hideScene();
        }

        public override void onEnterQueue()
        {
        }

        public override void onClose()
        {
        }
        
        private void _showBackBtn()
        {
            // if (null != _m_cbCommonBackObj)
            // {
            //     NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
            //     _m_cbCommonBackObj = null;
            // }
            // //加载通用回退按钮
            // _m_cbCommonBackObj = NPUIInstanceCommonBackController.instance.showCommonBack(EALUIWndLayer.NORMAL, () => { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_MAIN); }, UIResPathConst.WIN_COMMON_BACK);
            //
            // //放到最后
            // if (_m_cbCommonBackObj != null) 
            //     GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
        }
    }
}