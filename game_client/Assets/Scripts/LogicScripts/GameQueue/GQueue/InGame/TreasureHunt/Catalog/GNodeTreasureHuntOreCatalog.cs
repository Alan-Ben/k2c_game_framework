namespace GOE
{
    /// <summary>
    /// 矿石图鉴Node
    /// </summary>
    public class GNodeTreasureHuntOreCatalog : BaseQueueNode
    {
        private long _m_lSelectTabRefId = -1;
        private bool _m_bUseDefaultFilterType;// 是否使用默认筛选类型
        private ETreasureHuntOreCatalogFilterType _m_eSelectFilterType;//选择的筛选类型

        private bool _m_bIsFirstEnter = true;//是否第一次进入

        public GNodeTreasureHuntOreCatalog() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_ORE_CATALOG)
        {
            _m_lSelectTabRefId = -1;
            _m_bUseDefaultFilterType = true;
        }
        
        public GNodeTreasureHuntOreCatalog(long _selectTabRefId) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_ORE_CATALOG)
        {
            _m_lSelectTabRefId = _selectTabRefId;
            _m_bUseDefaultFilterType = true;
        }
        
        public GNodeTreasureHuntOreCatalog(ETreasureHuntOreCatalogFilterType _filterType) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_ORE_CATALOG)
        {
            _m_lSelectTabRefId = -1;
            _m_bUseDefaultFilterType = false;
            _m_eSelectFilterType = _filterType;
        }
        
        public GNodeTreasureHuntOreCatalog(long _selectTabRefId, ETreasureHuntOreCatalogFilterType _filterType) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_ORE_CATALOG)
        {
            _m_lSelectTabRefId = _selectTabRefId;
            _m_bUseDefaultFilterType = false;
            _m_eSelectFilterType = _filterType;
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
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        public override bool isOnlyUINode { get { return true; } }

        public override void onEnterQueue()
        {
            GGUIWndTreasureHuntOreCatalog.instance.load();
        }

        public override void onClose()
        {
            GGUIWndTreasureHuntOreCatalog.instance.discard();
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndTreasureHuntOreCatalog.instance, () =>
            {
                if (_m_bIsFirstEnter)
                {
                    if(_m_bUseDefaultFilterType)
                        GGUIWndTreasureHuntOreCatalog.instance.selectDefaultFilterType();
                    else
                        GGUIWndTreasureHuntOreCatalog.instance.setFilterType(_m_eSelectFilterType);
                    
                    GGUIWndTreasureHuntOreCatalog.instance.setSelectTab(_m_lSelectTabRefId);
                }
                
                _m_bIsFirstEnter = false;
            });
        }

        public override void QuitNode()
        {
            GGUIWndTreasureHuntTreasureCatalog.instance.hideWnd();
        }
    }
}