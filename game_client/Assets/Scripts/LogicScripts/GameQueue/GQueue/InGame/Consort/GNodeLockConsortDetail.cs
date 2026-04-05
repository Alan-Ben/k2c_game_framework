using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子详细信息
    /// </summary>
    public class GNodeLockConsortDetail : BaseQueueNode
    {
        private List<_IConsortShowInfo> _m_lAllConsortShowInfoList;//所有妃子信息
        private int _m_iCurConsortIndex;//当前选中妃子下标
        private ELockConsortDetailWndTabType _m_eCurTabType;//当前选中的tab类型
        
        private bool _m_bIsFirstEnter = true;
        
        public GNodeLockConsortDetail(List<_IConsortShowInfo> _allConsortShowInfoList, int _curConsortIndex, ELockConsortDetailWndTabType _curTabType = ELockConsortDetailWndTabType.NONE): base(EUIQueueStageType.MAIN, UINodeTagConst.C_LOCK_CONSORT_DETAIL)
        {
            if (_m_lAllConsortShowInfoList == null)
                _m_lAllConsortShowInfoList = new List<_IConsortShowInfo>();
            _m_lAllConsortShowInfoList.Clear();
            if(_allConsortShowInfoList != null)
                _m_lAllConsortShowInfoList.AddRange(_allConsortShowInfoList);
            
            _m_iCurConsortIndex = _curConsortIndex;
            _m_eCurTabType = _curTabType;

            _m_bIsFirstEnter = true;
        }

        public GNodeLockConsortDetail(_IConsortShowInfo _consortShowInfo, ELockConsortDetailWndTabType _curTabType = ELockConsortDetailWndTabType.NONE) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_LOCK_CONSORT_DETAIL)
        {
            if (_m_lAllConsortShowInfoList == null)
                _m_lAllConsortShowInfoList = new List<_IConsortShowInfo>();
            _m_lAllConsortShowInfoList.Clear();
            
            _m_lAllConsortShowInfoList.Add(_consortShowInfo);
            _m_iCurConsortIndex = 0;
            _m_eCurTabType = _curTabType;

            _m_bIsFirstEnter = true;
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
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        public override void EnterNode()
        {
            GUISceneMain.instance.showMainScene(GMainGUIMainSceneConsort.instance, () =>
            {
                GMainGUIMainSceneConsort.instance.showScene();
                GMainGUIMainSceneConsort.instance.showMainWndWithResBar(GGUIWndLockConsortDetail.instance, () =>
                {
                    GGUIWndLockConsortDetail.instance.showWnd();
    
                    // 只有第一次enter时使用Node传入的页签
                    if(_m_bIsFirstEnter)
                        GGUIWndLockConsortDetail.instance.setTabAndPageShowType(_m_eCurTabType);
                    // 只有第一次enter时使用Node传入的index
                    GGUIWndLockConsortDetail.instance.setConsortShowInfo(_m_lAllConsortShowInfoList, _m_bIsFirstEnter ? _m_iCurConsortIndex : -1, true);

                    _m_bIsFirstEnter = false;
                    // // 只有首次进入时使用Node指定的显示妃子和页签, 在进入Node后重复enterNode就使用页面自己记录的显示妃子和页签
                    // _m_eCurTabType = ELockConsortDetailWndTabType.NONE;
                    // _m_iCurConsortIndex = -1;
                });
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
            GMainGUIMainSceneConsort.instance.hideScene();
        }
        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}