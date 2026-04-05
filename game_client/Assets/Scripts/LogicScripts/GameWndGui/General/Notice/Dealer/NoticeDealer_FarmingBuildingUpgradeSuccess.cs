
namespace GOE
{
    public class NoticeDealer_FarmingBuildingUpgradeSuccess : NPUINoticeMgr._ANPUINoticeDealer
    {
        private bool _m_bWndLoaded;
        private FarmingBuildingRefObj _m_buildingRef;
        private int _m_currentLevel;
        
        
        public NoticeDealer_FarmingBuildingUpgradeSuccess(FarmingBuildingRefObj _buildingRef, int _currentLevel)
        {
            _m_buildingRef = _buildingRef;
            _m_currentLevel = _currentLevel;
        }
        
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        

        public override void dealShowNotice()
        {
            //未加载的时候加载
            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;

                GGUIWndFarmingBuildingUpgradeSuccess.instance.load(GGUIWndFarmingBuildingUpgradeSuccess.instance.showWnd);
            }
            else
            {
                //已经加载的，需要刷新层级
                if (null != GGUIWndFarmingBuildingUpgradeSuccess.instance.wnd)
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndFarmingBuildingUpgradeSuccess.instance.rectTransform);
            }
            
            GGUIWndFarmingBuildingUpgradeSuccess.instance.refreshWnd(_m_buildingRef, _m_currentLevel);
        }
        public override void dealHideNotice()
        {
            //已加载的时候才卸载，同时重置状态
            if (_m_bWndLoaded)
            {
                GGUIWndFarmingBuildingUpgradeSuccess.instance.discard();
                _m_bWndLoaded = false;
            }
        }

        protected override void _onDealerDone()
        {
        }
    }
}