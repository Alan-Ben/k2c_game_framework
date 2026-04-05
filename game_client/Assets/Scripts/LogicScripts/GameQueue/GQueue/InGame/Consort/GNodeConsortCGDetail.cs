namespace GOE
{
    /// <summary>
    /// 妃子CG窗口
    /// </summary>
    public class GNodeConsortCGDetail : BaseQueueNode
    {
        private ConsortCGRefObj _m_ConsortCGRefObj;//妃子CG表数据
        public GNodeConsortCGDetail(ConsortCGRefObj _consortCgRef) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_CONSORT_CG_DETAIL)
        {
            _m_ConsortCGRefObj = _consortCgRef;
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
        public override bool IsMainViewNode { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }
        
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            GGUIWndConsortCGDetail.instance.load();
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            GGUIWndConsortCGDetail.instance.discard();
        }
        
        public override void EnterNode()
        {
            GGUIWndConsortCGDetail.instance.regLoadDoneDelegate(() =>
            {
                GGUIWndConsortCGDetail.instance.showWnd();
                GGUIWndConsortCGDetail.instance.setData(_m_ConsortCGRefObj);
            });
        }

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
        }

        public override void QuitNode()
        {
            GGUIWndConsortCGDetail.instance.hideWnd();
        }
        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}