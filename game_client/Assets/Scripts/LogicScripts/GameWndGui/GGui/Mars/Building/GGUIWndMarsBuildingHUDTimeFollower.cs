using ALPackage;
using Common.GuildEnum;
using Common.MarsEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingHUDTimeFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingHUDTime, GGUIWndMarsBuildingHUDTimeFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private MarsBuildingInfo _m_buildingInfo;


        public GGUIWndMarsBuildingHUDTimeFollowerController()
        {
            _m_resIndex = new GResPathIndex(7104);
        }
        public GGUIWndMarsBuildingHUDTimeFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingHUDTimeFollower _createItemWnd(GGUIMonoMarsBuildingHUDTime _wndMono)
        {
            GGUIWndMarsBuildingHUDTimeFollower wnd = new GGUIWndMarsBuildingHUDTimeFollower(_wndMono);
            wnd.refreshWnd(_m_buildingInfo);
            wnd.showWnd();
            return wnd;
        }
        public void refreshWnd(MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            wnd?.refreshWnd(_m_buildingInfo);
        }
        public void tick()
        {
            wnd?.tick();
        }
    }

    public class GGUIWndMarsBuildingHUDTimeFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingHUDTime>
    {
        private MarsBuildingInfo _m_buildingInfo;
        
        
        public GGUIWndMarsBuildingHUDTimeFollower(GGUIMonoMarsBuildingHUDTime _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;

        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;

        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
        }
        protected override void _onWndInitDone()
        {
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }



        public void refreshWnd(MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_buildingInfo == null)
                return;

            _refreshTime();
            bool canAssist = _m_buildingInfo != null && _m_buildingInfo.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
            ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);
        }
        
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            var buildingRef = GRefdataCoreMgr.instance.marsBuildingRefCore.getRef(_buildingId);
            if (buildingRef != null && buildingRef.building_type == EMarsBuildingType.HELP)
            {
                if (_newState == MarsBuildingInfo.StateType.Normal)
                    refreshWnd();
            }
        }
        public void tick()
        {
            _refreshTime();
        }

        public void _refreshTime()
        {
            if (wnd == null || _m_buildingInfo == null)
                return;
            long remainingTime = _m_buildingInfo.remainingBuildOrUpgradeTime;
            string timeStr = TimeUtil.millisecondsToTime_DayHourOrHMS(remainingTime);
            ALUGUICommon.setLabelTxt(wnd.txtTime, timeStr);
            ALUGUICommon.setLabelTxt(wnd.tmpTime, timeStr);
        }
        
        
        private void _onClickAssist(GameObject _obj)
        {
            if (_m_buildingInfo == null) return;
            bool canAssist = _m_buildingInfo != null && _m_buildingInfo.guildHelpId <= 0 && NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding();
            if (!canAssist) return;
            NPPlayer.instance.guildMarsHelpComp.reqSendMarsHelp(EGuildMarsHelpObjType.BUILDING_QUEUE, _m_buildingInfo.queueId,
                (_isSuc) =>
                {
                    refreshWnd();
                });
        }
    }
}