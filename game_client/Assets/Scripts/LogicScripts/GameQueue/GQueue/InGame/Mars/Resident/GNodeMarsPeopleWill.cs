using System;

namespace GOE
{
    /// <summary>
    /// 民意Node
    /// </summary>
    public class GNodeMarsPeopleWill : BaseQueueNode
    {
        private EMarsPopularWillTabType _m_eSelectTabType = EMarsPopularWillTabType.NONE;
        private Action _m_aOnEnterNode;
        private Action _m_aOnQuitNode;
        private bool _m_bIsFirstEnter;
        
        public GNodeMarsPeopleWill(EMarsPopularWillTabType _tabType = EMarsPopularWillTabType.NONE, Action _onEnterNode = null, Action _onQuitNode = null) 
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_POPULAR_WILL)
        {
            _m_eSelectTabType = _tabType;
            _m_aOnEnterNode = _onEnterNode;
            _m_aOnQuitNode = _onQuitNode;
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
        public override bool IsMainViewNode { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        public override bool isOnlyUINode { get { return true; } }
        
        public override void EnterNode()
        {
            GGUIWndMarsPopularWill.instance.regLoadDoneDelegate(() =>
            {
                if(_m_bIsFirstEnter)
                    GGUIWndMarsPopularWill.instance.setSelectTab(_m_eSelectTabType);
                _m_bIsFirstEnter = false;
                
                GGUIWndMarsPopularWill.instance.showWnd();
            });
            
            _m_aOnEnterNode?.Invoke();
        }

        public override void QuitNode()
        {
            GGUIWndMarsPopularWill.instance.hideWnd();
            _m_aOnQuitNode?.Invoke();
        }

        public override void onEnterQueue()
        {
            GGUIWndMarsPopularWill.instance.load();
        }

        public override void onClose()
        {
            GGUIWndMarsPopularWill.instance.discard();
        }
    }
}