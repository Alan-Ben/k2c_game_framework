using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalPageTabList : GGUISubWndCommonPageTabList<GGUIMonoStageGoalTabType, GGUIMonoStageGoalPageTabList, GGUIMonoStageGoalPageTabListItem>
    {
        public GGUISubWndStageGoalPageTabList(GGUIMonoStageGoalPageTabList _wnd) 
            : base(_wnd, false)
        {
            initWnd();
        }

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoStageGoalTabType _type, Transform _wndParent)
        {
            return _type switch
            {
                GGUIMonoStageGoalTabType.OVERVIEW => new GGUIPrefabSubWndStageGoalPageOverview(_wndParent, _jumpToTask),
                GGUIMonoStageGoalTabType.TASK => new GGUIPrefabSubWndStageGoalPageTask(_wndParent),
                GGUIMonoStageGoalTabType.PEAK => new GGUIWndStageGoalPagePeak(_wndParent),
                _ => null
            };
        }

        /// <summary>
        /// 点击选中页签之后事件
        /// </summary>
        /// <param name="_type"></param>
        protected override void _onAfterClickSelectTab(GGUIMonoStageGoalTabType _type)
        {
            //切换节点用于触发引导
            switch (_type)
            {
                case GGUIMonoStageGoalTabType.OVERVIEW:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_STAGE_GOAL_OVERVIEW_PAGE);
                    break;
                case GGUIMonoStageGoalTabType.TASK:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_STAGE_GOAL_TASK_PAGE);
                    break;
                case GGUIMonoStageGoalTabType.PEAK:
                    QueueMgr.instance.addNode_OnlyOp(UINodeTagConst.C_STAGE_GOAL_PEAK_PAGE);
                    break;
            }
        }

        private void _jumpToTask()
        {
            setSelectTab(GGUIMonoStageGoalTabType.TASK);
        }
    }
}