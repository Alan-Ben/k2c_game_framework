namespace GOE
{
    /// <summary>
    /// 招募商店Node
    /// </summary>
    public class GNodeRecruitShop : BaseQueueNode
    {
        private RecruitShopInfo _m_iShopInfo;//招募商店信息
        private long _m_lUIResPathId;//商店UI资源路径ID

        private GGUIWndRecruitShopNew _m_wnd;
        
        public GNodeRecruitShop(long _shopId, long _uiResPathId) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_RECRUIT_SHOP)
        {
            RecruitShopRefObj shopRefObj = GRefdataCoreMgr.instance.recruitShopRefCore.getRef(_shopId);
            if (shopRefObj == null)
            {
                Debug.LogError($"[GNodeRecruitShop ctor] recruit_shop表中找不到 shopId = {_shopId} 的商店配置数据");
            }
            else
            {
                _m_iShopInfo = new RecruitShopInfo(shopRefObj);
            }
            
            _m_lUIResPathId = _uiResPathId;
        }
        
        public GNodeRecruitShop(RecruitShopInfo _shopInfo, long _uiResPathId) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_RECRUIT_SHOP)
        {
            _m_iShopInfo = _shopInfo;
            _m_lUIResPathId = _uiResPathId;
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
        
        public override void onEnterQueue()
        {
            _m_wnd = new GGUIWndRecruitShopNew(_m_iShopInfo, _m_lUIResPathId);
        }

        public override void onClose()
        {
            _m_wnd?.discard();
            _m_wnd = null;
        }

        public override void EnterNode()
        {
            if(_m_wnd == null)
                return;
            
            GUISceneMain.instance.showMainWnd(_m_wnd, () =>
            {
                
            });
        }

        public override void QuitNode()
        {
            _m_wnd?.hideWnd();
        }
    }
}