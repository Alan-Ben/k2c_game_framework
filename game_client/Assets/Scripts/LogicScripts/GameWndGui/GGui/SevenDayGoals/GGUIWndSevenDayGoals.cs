using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 七日目标任务主界面
    /// </summary>
    public class GGUIWndSevenDayGoals : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoSevenDayGoals>
    {
        private string _m_sAssetPath; //窗口对象的资源 加载路径
        private string _m_sObjName; //窗口对象的资源 名字
        private GGUISubWndSevenDayGoalsScoreRewardBar _m_scoreBar;
        private GGUISubWndSevenDayGoalsDayBtnContainer _m_dayBtnContainer;

        private GGUISubWndSevenDayGoalsDayContent _m_dayContent;

        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        private int _m_selectDay;


        public GGUIWndSevenDayGoals(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }


        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            NPPlayer.instance.sevenDayGoalsComp.data.onTaskChg += _onTaskChg;
            WinMsg.RegisterMsgAct(WinMsgType.ON_SEVEN_DAY_GOAL_RED_TIP_CHG, _onRedTipChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_SEVEN_DAY_GOAL_INIT_DONE, refreshWnd);

            _m_scoreBar?.showWnd();
            _m_dayBtnContainer?.showWnd();
            _m_dayContent?.showWnd();
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.sevenDayGoalsComp.data.onTaskChg -= _onTaskChg;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_SEVEN_DAY_GOAL_RED_TIP_CHG, _onRedTipChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_SEVEN_DAY_GOAL_INIT_DONE, refreshWnd);

            _m_scoreBar?.hideWnd();
            _m_dayBtnContainer?.hideWnd();
            _m_dayContent?.hideWnd();
            _m_iTickTask.setDisable();
        }

        protected override void _onReset()
        {
            _m_scoreBar?.resetWnd();
            _m_dayBtnContainer?.resetWnd();
            _m_dayContent?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_scoreBar?.discard();
            _m_dayBtnContainer?.discard();
            _m_dayContent?.discard();

            _m_scoreBar = null;
            _m_dayBtnContainer = null;
            _m_dayContent = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoScoreBar != null)
                _m_scoreBar = new GGUISubWndSevenDayGoalsScoreRewardBar(wnd.monoScoreBar);
            if (wnd.monoDayBtnContainer != null)
                _m_dayBtnContainer =
                    new GGUISubWndSevenDayGoalsDayBtnContainer(wnd.monoDayBtnContainer, _onDayBtnClick);
            if (wnd.monoDayContent != null)
                _m_dayContent = new GGUISubWndSevenDayGoalsDayContent(wnd.monoDayContent);
        }


        public void refreshWnd(int _selectDay)
        {
            _m_selectDay = _selectDay;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_scoreBar?.refreshWnd();
            _m_dayBtnContainer?.refreshWnd(_m_selectDay);
            _m_selectDay = _m_dayBtnContainer?.selectedDay ?? 1;
            _m_dayContent?.refreshWnd(_m_selectDay, false);
            _refreshLeftTime();
        }

        //刷新剩余时间
        private void _refreshLeftTime()
        {
            //显示活动倒计时
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 0.2f);
        }

        //每秒倒计时
        private void _tick()
        {
            if (wnd == null)
                return;

            long leftTimeMs = 0;
            _ABaseActivityInfo activityInfo = NPPlayer.instance.sevenDayGoalsComp.data.activityInfo;
            if (activityInfo != null && activityInfo.isPlaying)
                leftTimeMs = activityInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            if (leftTimeMs < 0)
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");
            else
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TimeUtil.millisecondsToTime_hms(leftTimeMs));
        }

        private void _onDayBtnClick()
        {
            _m_selectDay = _m_dayBtnContainer?.selectedDay ?? 1;
            _m_dayContent?.refreshWnd(_m_selectDay, true);
        }

        private void _onTaskChg(long _taskId)
        {
            _m_dayBtnContainer?.refreshAllItem();
        }

        private void _onRedTipChg()
        {
            //刷新天数列表红点
            _m_dayBtnContainer?.refreshAllItem();
            //刷新内容页红点
            _m_dayContent?.refreshRedTip();
        }
    }
}