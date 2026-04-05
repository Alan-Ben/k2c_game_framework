using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 阶段任务冲榜活动任务Grid
    /// </summary>
    public class GGUIWndStepTaskRushRankActivityTaskGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoStepTaskRushRankActivityTaskItem, GGUIMonoStepTaskRushRankActivityTaskGrid, GGUIWndStepTaskRushRankActivityTaskItem>
    {
        /// <summary>
        /// 任务显示数据结构
        /// </summary>
        private struct TaskItemDisplayStruct
        {
            public ActivityStepRewardInfo stepRewardInfo;
            public ActivityStepRewardSetEventTaskRefObj eventTaskRef;
        }

        // 显示列表
        [NotNull] private List<TaskItemDisplayStruct> _m_showList = new List<TaskItemDisplayStruct>();
        // 临时事件任务配置列表
        private List<ActivityStepRewardSetEventTaskRefObj> _m_lTmpEventTaskRefList;


        public GGUIWndStepTaskRushRankActivityTaskGrid(GGUIMonoStepTaskRushRankActivityTaskGrid _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override GGUIWndStepTaskRushRankActivityTaskItem _createItemWnd(GGUIMonoStepTaskRushRankActivityTaskItem _itemMono)
        {
            return new GGUIWndStepTaskRushRankActivityTaskItem(_itemMono);
        }

        protected override void _onRefreshItemWnd(GGUIWndStepTaskRushRankActivityTaskItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_showList.Count || _itemMono == null)
                return;

            TaskItemDisplayStruct info = _m_showList[_itemIdx];
            _itemMono.setInfo(info.stepRewardInfo, info.eventTaskRef);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lTmpEventTaskRefList?.Clear();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_showList.Clear();

            _m_lTmpEventTaskRefList?.Clear();
            _m_lTmpEventTaskRefList = null;
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_stepRewardInfoList">阶段奖励信息列表</param>
        public void setInfo(List<ActivityStepRewardInfo> _stepRewardInfoList)
        {
            if (_stepRewardInfoList == null)
                return;

            _m_showList.Clear();
            if (_m_lTmpEventTaskRefList == null)
                _m_lTmpEventTaskRefList = new List<ActivityStepRewardSetEventTaskRefObj>();

            // 遍历每个阶段奖励信息，获取对应的事件任务配置
            foreach (var stepRewardInfo in _stepRewardInfoList)
            {
                if (stepRewardInfo == null)
                    continue;

                _m_lTmpEventTaskRefList.Clear();
                GRefdataCoreMgr.instance.getStepRewardSetEventTaskRefList(stepRewardInfo.stepRewardSetId, _m_lTmpEventTaskRefList);

                foreach (var eventTaskRef in _m_lTmpEventTaskRefList)
                {
                    if (eventTaskRef == null)
                        continue;

                    // 加入显示列表中
                    _m_showList.Add(new TaskItemDisplayStruct()
                    {
                        stepRewardInfo = stepRewardInfo,
                        eventTaskRef = eventTaskRef
                    });
                }
            }

            // // 按配置id排序
            // _m_showList.Sort(_sort);

            setItemCount(_m_showList.Count);
        }

        // /// <summary>
        // /// 排序方法：按事件任务配置id从小到大排序
        // /// </summary>
        // /// <param name="_a"></param>
        // /// <param name="_b"></param>
        // /// <returns></returns>
        // private int _sort(TaskItemDisplayStruct _a, TaskItemDisplayStruct _b)
        // {
        //     if (_b.eventTaskRef == null)
        //         return -1;
        //     if (_a.eventTaskRef == null)
        //         return 1;
        //
        //     return _a.eventTaskRef.id.CompareTo(_b.eventTaskRef.id);
        // }
    }
}
