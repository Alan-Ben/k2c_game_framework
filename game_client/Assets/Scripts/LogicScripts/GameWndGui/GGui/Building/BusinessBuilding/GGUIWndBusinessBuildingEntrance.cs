
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBusinessBuildingEntranceFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoBusinessBuildingEntrance, GGUIWndBusinessBuildingEntrance>
    {
        [NotNull] private readonly GResPathIndex _m_resIndex;
        private BusinessBuildingInfo _m_buildingInfo;
        
        
        public GGUIWndBusinessBuildingEntranceFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1102);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        protected override GGUIWndBusinessBuildingEntrance _createItemWnd(GGUIMonoBusinessBuildingEntrance _wndMono)
        {
            GGUIWndBusinessBuildingEntrance wnd = new GGUIWndBusinessBuildingEntrance(_wndMono);
            wnd.refreshWnd(_m_buildingInfo);
            wnd.showWnd();
            return wnd;
        }


        public void setBuildingInfo(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            wnd?.refreshWnd(_m_buildingInfo);
        }
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
        public void refreshCanSettleHero()
        {
            wnd?.refreshCanSettleHero();
        }
        public void refreshCanUnlockProduct()
        {
            wnd?.refreshCanUnlockProduct();
        }
    }
    public class GGUIWndBusinessBuildingEntrance : _ATALGGUIWndCommonFollowItem<GGUIMonoBusinessBuildingEntrance>
    {
        private BusinessBuildingInfo _m_buildingInfo;
        private bool _m_canSettleHero;
        private bool _m_canUnlockProduct;
        
        
        public GGUIWndBusinessBuildingEntrance(GGUIMonoBusinessBuildingEntrance _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.building_hudName_str_num, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.name), _m_buildingInfo.level));
            ALUGUICommon.setLabelTxt(wnd.txtEmployeeNum, _m_buildingInfo.employeeNum);
            refreshCanSettleHero();
            refreshCanUnlockProduct();
        }
        public void refreshCanSettleHero()
        {
            if (_m_buildingInfo == null)
                return;
            
            _m_canSettleHero = _m_buildingInfo.needShowSettleHeroTip();
            _refreshRedTip();
        }
        public void refreshCanUnlockProduct()
        {
            if (_m_buildingInfo == null)
                return;

            _m_canUnlockProduct = _m_buildingInfo.needShowUnlockProductTip();
            _refreshRedTip();
        }


        private void _refreshRedTip()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.listRedTipShow, _m_canSettleHero || _m_canUnlockProduct);
        }

        private void _onCustomReload()
        {
            _refreshRedTip();
        }
    }
}