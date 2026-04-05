using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星建筑修复时间Follower控制器
    /// </summary>
    public class GGUIWndMarsBuildingRepairTimeFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingRepairTime, GGUIWndMarsBuildingRepairTimeFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        /// <summary>
        /// 队伍信息
        /// </summary>
        private MarsExploreTeamInfo _m_teamInfo;


        public GGUIWndMarsBuildingRepairTimeFollowerController()
        {
            _m_resIndex = new GResPathIndex(7321);
        }
        public GGUIWndMarsBuildingRepairTimeFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingRepairTimeFollower _createItemWnd(GGUIMonoMarsBuildingRepairTime _wndMono)
        {
            GGUIWndMarsBuildingRepairTimeFollower wnd = new GGUIWndMarsBuildingRepairTimeFollower(_wndMono);
            wnd.refreshWnd(_m_teamInfo);
            wnd.showWnd();
            return wnd;
        }

        /// <summary>
        /// 刷新界面，外部传入剩余时间
        /// </summary>
        public void refreshWnd(MarsExploreTeamInfo _teamInfo)
        {
            _m_teamInfo = _teamInfo;
            wnd?.refreshWnd(_m_teamInfo);
        }

        /// <summary>
        /// tick刷新时间显示
        /// </summary>
        public void tick()
        {
            wnd?.tick();
        }
    }

    /// <summary>
    /// 火星建筑修复时间Follower窗口
    /// </summary>
    public class GGUIWndMarsBuildingRepairTimeFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingRepairTime>
    {
        /// <summary>
        /// 队伍信息
        /// </summary>
        private MarsExploreTeamInfo _m_teamInfo;


        public GGUIWndMarsBuildingRepairTimeFollower(GGUIMonoMarsBuildingRepairTime _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
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


        /// <summary>
        /// 刷新界面，外部传入剩余时间
        /// </summary>
        public void refreshWnd(MarsExploreTeamInfo _teamInfo)
        {
            _m_teamInfo = _teamInfo;
            refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshTime();
        }

        /// <summary>
        /// tick刷新时间显示
        /// </summary>
        public void tick()
        {
            _refreshTime();
        }

        /// <summary>
        /// 刷新时间显示
        /// </summary>
        private void _refreshTime()
        {
            if (wnd == null)
                return;

            if(_m_teamInfo == null || _m_teamInfo.state != EMarsExploreTeamState.REPAIR)
            {
                ALUGUICommon.setLabelTxt(wnd.txtTime, "");
                ALUGUICommon.setLabelTxt(wnd.tmpTime, "");
                return;
            }
            
            string timeStr = TimeUtil.millisecondsToTime_DayHourOrHMS(_m_teamInfo.stateRemainTimeMs);
            ALUGUICommon.setLabelTxt(wnd.txtTime, timeStr);
            ALUGUICommon.setLabelTxt(wnd.tmpTime, timeStr);
        }
    }
}
