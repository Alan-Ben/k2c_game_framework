using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndSevenDayGoalsDayContentPageTask : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoSevenDayGoalsDayContentPageTask>
    {
        private GGUISubWndSevenDayGoalsDayContentTaskContainer _m_taskContainer;
        private int _m_day;
        
        
        public GGUIPrefabSubWndSevenDayGoalsDayContentPageTask(Transform _parent) 
            : base(_parent)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoSevenDayGoalsDayContentPageTask.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSevenDayGoalsDayContentPageTask.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_taskContainer?.showWnd();

            refreshWnd(true);

            NPPlayer.instance.sevenDayGoalsComp.data.onTaskChg += _onTaskChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.sevenDayGoalsComp.data.onTaskChg -= _onTaskChg;
            
            _m_taskContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_taskContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_taskContainer?.discard();
            _m_taskContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoTaskContainer != null)
                _m_taskContainer = new GGUISubWndSevenDayGoalsDayContentTaskContainer(wnd.monoTaskContainer);
        }


        public void refreshWnd(int _day, bool _reset = false)
        {
            _m_day = _day;
            refreshWnd(_reset);
        }
        public void refreshWnd(bool _reset = false)
        {
            if (wnd == null || !_m_bIsShow)
                return;

            List<SevenDayGoalsTaskRewardRefObj> taskListRef = GRefdataCoreMgr.instance.getSevenDayGoalsDayTasks(_m_day);
            List<SevenDayGoalsTaskRewardRefObj> taskList = new List<SevenDayGoalsTaskRewardRefObj>(taskListRef ?? new List<SevenDayGoalsTaskRewardRefObj>(0));
            if (_m_taskContainer != null)
            {
                taskList.Sort(_taskListSort);
                _m_taskContainer.refreshWnd(taskList);
                if (_reset)
                    _m_taskContainer.moveToTop();
            }
        }


        private void _onTaskChg(long _taskId)
        {
            refreshWnd(false);
        }
        private int _taskListSort(SevenDayGoalsTaskRewardRefObj _a, SevenDayGoalsTaskRewardRefObj _b)
        {
            ECommonRewardType aRewardType = _getTaskRewardType(_a);
            ECommonRewardType bRewardType = _getTaskRewardType(_b);
            if (aRewardType != bRewardType)
            {
                if (aRewardType == ECommonRewardType.CAN_GET_REWARD)
                    return -1;
                if (bRewardType == ECommonRewardType.CAN_GET_REWARD)
                    return 1;
                if (aRewardType == ECommonRewardType.NOT_GET_REWARD)
                    return -1;
                if (bRewardType == ECommonRewardType.NOT_GET_REWARD)
                    return 1;
            }

            return _a.id.CompareTo(_b.id);
        }
        private ECommonRewardType _getTaskRewardType(SevenDayGoalsTaskRewardRefObj _refObj)
        {
            if (_refObj == null)
                return ECommonRewardType.NONE;

            long curProgress = NPPlayer.instance.sevenDayGoalsComp.data.getTaskCount(_refObj.id);
            long targetProgress = _refObj.goal_count;
            bool hadGetReward = NPPlayer.instance.sevenDayGoalsComp.data.isTaskHadDrawReward(_refObj.id);
            bool isCompleted = curProgress >= targetProgress;
            ECommonRewardType rewardType = ECommonRewardType.NONE;
            if (hadGetReward)
                rewardType = ECommonRewardType.HAS_GET_REWARD;
            else if (isCompleted)
                rewardType = ECommonRewardType.CAN_GET_REWARD;
            else
                rewardType = ECommonRewardType.NOT_GET_REWARD;

            return rewardType;
        }
    }
}