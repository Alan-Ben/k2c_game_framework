using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndBuildingBuildFollowBtnFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoBuildingBuildFollowBtn, GGUIWndBuildingBuildFollowBtn>
    {
        private readonly GResPathIndex _m_resIndex;
        private BuildingInfo _m_buildingInfo;
        
        
        public GGUIWndBuildingBuildFollowBtnFollowItemController()
        {
            _m_resIndex = new GResPathIndex(1110);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndBuildingBuildFollowBtn _createItemWnd(GGUIMonoBuildingBuildFollowBtn _wndMono)
        {
            GGUIWndBuildingBuildFollowBtn wnd = new GGUIWndBuildingBuildFollowBtn(_wndMono);
            wnd.refreshWnd(_m_buildingInfo);
            wnd.showWnd();
            return wnd;
        }
        
        
        public void setBuildingInfo(BuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            wnd?.refreshWnd(_m_buildingInfo);
        }
    }
    public class GGUIWndBuildingBuildFollowBtn : _ATALGGUIWndCommonFollowItem<GGUIMonoBuildingBuildFollowBtn>
    {
        private BuildingInfo _m_buildingInfo;
        
        
        public GGUIWndBuildingBuildFollowBtn(GGUIMonoBuildingBuildFollowBtn _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD, _onSimulateClickBuildingBuild);
            WinMsg.RegisterMsgAct(WinMsgType.BUILD_BUILD_STATE_REFRESH, refreshWnd);

        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_BUILDING_BUILD, _onSimulateClickBuildingBuild);
            WinMsg.UnregisterMsgAct(WinMsgType.BUILD_BUILD_STATE_REFRESH, refreshWnd);
            
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onClickBuildBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onClickBuildBtn);
        }


        public void refreshWnd(BuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingInfo == null)
                return;

            bool isUnlocked = _m_buildingInfo.baseRef.build_condition.IsEnable(null);
            wnd.setUnlock(isUnlocked);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.build_condition_desc, _m_buildingInfo.baseRef.build_condition_desc_params));
        }


        private void _onClickBuildBtn(GameObject _)
        {
            if (_m_buildingInfo == null)
                return;

            // 策划又要统一表现了，这边先注释
            // if (!_m_buildingInfo.isHasBusinessFunction())
            // {
            //     NPPlayer.instance.buildingComp.reqBuildingBuild(_m_buildingInfo.id);
            //     return;
            // }
            
            GGUIWndBuildingBuild.instance.refreshWnd(_m_buildingInfo);
            GGUIWndBuildingBuild.instance.setFocusPos(MainAdditionBuildingTDScene.instance.getBuildingFocusPos(_m_buildingInfo.id));
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBuildingBuild.instance, GGUIWndBuildingBuild.instance.showWnd, EUIQueueStageType.MAIN, string.Empty, false, false);
        }

        private void _onSimulateClickBuildingBuild(object[] _params)
        {
            if (_m_buildingInfo == null)
                return;
            
            if (_params is not { Length: > 0 } || _params[0] is not long buildingId)
                return;
            
            if (buildingId != _m_buildingInfo.id)
                return;
            
            _onClickBuildBtn(null);
        }
    }
}