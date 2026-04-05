namespace GOE
{
    /// <summary>
    /// 组合图鉴
    /// </summary>
    public class GNodeTreasureHuntCompositeCatalog : BaseQueueNode
    {
        private bool _m_bUseDefaultFilterType; // 是否使用默认筛选类型
        private ETreasureHuntCompositeCatalogFilterType _m_eSelectFilterType; // 选择的筛选类型
        
        private bool _m_bIsFirstEnter = true; // 是否第一次进入
        
        public GNodeTreasureHuntCompositeCatalog() : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_COMPOSITE_CATALOG)
        {
            _m_bUseDefaultFilterType = true;
        }
        
        public GNodeTreasureHuntCompositeCatalog(ETreasureHuntCompositeCatalogFilterType _filterType) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_TREASURE_HUNT_COMPOSITE_CATALOG)
        {
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
            GGUIWndTreasureHuntCompositeCatalog.instance.load();
        }

        public override void onClose()
        {
            GGUIWndTreasureHuntCompositeCatalog.instance.discard();
        }

        public override void EnterNode()
        {
            GUISceneMain.instance.showMainWnd(GGUIWndTreasureHuntCompositeCatalog.instance, () =>
            {
                if (_m_bIsFirstEnter)
                {
                    if(_m_bUseDefaultFilterType)
                        GGUIWndTreasureHuntCompositeCatalog.instance.selectDefaultFilterType(true);
                    else
                        GGUIWndTreasureHuntCompositeCatalog.instance.setFilterType(_m_eSelectFilterType, true);
                }

                _m_bIsFirstEnter = false;
            });
        }

        public override void QuitNode()
        {
            GGUIWndTreasureHuntCompositeCatalog.instance.hideWnd();
        }
    }
}