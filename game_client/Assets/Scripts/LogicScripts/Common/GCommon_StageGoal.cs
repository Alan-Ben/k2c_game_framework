using NPCommon;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 处理阶段目标获取奖励解锁新阶段的逻辑
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_lastStageGoalRef"></param>
        public static void dealStageGoalGetRewardProcess(List<NPCommon_ItemInfo> _itemList, StageGoalRefObj _lastStageGoalRef, Action _onDone = null)
        {
            if (_lastStageGoalRef == null)
                return;

            //筛选特殊道具列表
            GCommon.GainItemFilterData filterData = new GCommon.GainItemFilterData();
            List<NPCommon_ItemInfo> remainItems = GCommon.commonDealGainSpecialItem(_itemList, ref filterData);
            List<NPCommon_ItemInfo> showItems = GCommon.filterPopWndEnableShowItemList(remainItems);
            //是否需要展示建造过程
            bool needShowBuildingBuild = NPPlayer.instance.stageGoalComp.needShowBigStepBuildingBuild(true);

            //增加一个空的notice，保证展示流程都在notice中，不会触发新的notice
            NoticeDealer_CustomAction notice = new NoticeDealer_CustomAction(_complete => { }, NPNoticeType.g_AllTypeArr);
            NPUINoticeMgr.instance.addDealer(notice);

            ALProcess process = ALProcess.CreateProcess();
            process.addDelegateProcess(_complete => _showStageGoalStepComplete(_lastStageGoalRef, showItems, _complete));//展示阶段完成界面
            process.addDelegateProcess(_complete => _showStageGoalDialog(_lastStageGoalRef.dialogue_id, _complete));//展示对话
            // process.addDelegateProcess(_complete => _showStageGoalSmallStepUnlock(_lastStageGoalRef, _complete));//展示小阶段解锁界面
            process.addDelegateProcess(_complete => _showStageGoalBuildingBuild(needShowBuildingBuild, _complete));//展示建造
            process.addDelegateProcess(_showStageGoalBigStepUnlock);//展示大阶段解锁界面
            process.addProcess(() => CommonRewardDealer.showSpecial(filterData, null));//展示特殊道具界面
            process.addProcess(notice.setDealerDone);//结束展示流程的notice
            process.addProcess(_onDone);//结束回调
            process.deal();
        }

        //展示大小阶段完成界面
        private static void _showStageGoalStepComplete(StageGoalRefObj _lastStageGoalRef, List<NPCommon_ItemInfo> _showItems, Action _complete)
        {
            // //如果不是大阶段的最后一个小阶段，则展示小阶段完成界面，否则展示大阶段完成界面
            // if (!NPPlayer.instance.stageGoalComp.isLastSmallStep(_lastStageGoalRef))
            // {
            //     GGUIWndStageGoalComplete.instance.refreshWnd(_lastStageGoalRef, _showItems);
            //     QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndStageGoalComplete.instance, _complete, UINodeTagConst.C_STAGE_GOAL_COMPLETE));
            // }
            // else
            // {
            //     StageGoalBigStepRefObj bigStepRefObj = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)_lastStageGoalRef.step);
            //     GGUIWndStageGoalBigStepComplete.instance.setInfo(bigStepRefObj, _showItems);
            //     QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndStageGoalBigStepComplete.instance, _complete, UINodeTagConst.C_STAGE_GOAL_BIG_STEP_COMPLETE));
            // }

            GGUIWndStageGoalComplete.instance.refreshWnd(_lastStageGoalRef, _showItems);
            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndStageGoalComplete.instance, _complete, UINodeTagConst.C_STAGE_GOAL_COMPLETE));
        }
        private static void _showStageGoalDialog(long _dialogueId, Action _complete)
        {
            if (_dialogueId <= 0)
            {
                _complete?.Invoke();
                return;
            }

            GCommon.enterDialogueNode(_dialogueId, _complete);
        }
        // private static void _showStageGoalSmallStepUnlock(StageGoalRefObj _lastStageGoalRef, Action _complete)
        // {
        //     //如果不是大阶段的最后一个小阶段，则展示下一个小阶段解锁界面
        //     if (_lastStageGoalRef != null && !NPPlayer.instance.stageGoalComp.isLastSmallStep(_lastStageGoalRef))
        //     {
        //         StageGoalRefObj stageGoalRefObj = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(_lastStageGoalRef.step + 1);
        //         GGUIWndStageGoalUnlock.instance.refreshWnd(stageGoalRefObj);
        //         QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndStageGoalUnlock.instance, _complete, UINodeTagConst.C_STAGE_GOAL_UNLOCK));
        //     }
        //     else
        //         _complete?.Invoke();
        // }
        private static void _showStageGoalBuildingBuild(bool _needShowBuildingBuild, Action _complete)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL);
            StageGoalBigStepRefObj curBigStepRef = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            if (!_needShowBuildingBuild || curBigStepRef == null)
            {
                _complete?.Invoke();
                return;
            }

            ALCommonActionMonoTask.addMonoTask(_complete, curBigStepRef.build_sfx_delay);
        }
        private static void _showStageGoalBigStepUnlock(Action _complete)
        {
            if (NPPlayer.instance.stageGoalComp.isAllDone)
            {
                _complete?.Invoke();
                return;
            }
            StageGoalRefObj stageGoalRefObj = NPPlayer.instance.stageGoalComp.stageRefObj;
            int step = (int)(stageGoalRefObj?.step ?? 0);
            StageGoalBigStepRefObj bigStepRefObj = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj(step);
            StageGoalBigStepRefObj lastBigStepRefObj = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj(step - 1);

            if (bigStepRefObj == lastBigStepRefObj)
            {
                _complete?.Invoke();
                return;
            }
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                GGUIWndStageGoalBigStepUnlock.instance.refreshWnd(bigStepRefObj);
                QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndStageGoalBigStepUnlock.instance, _complete, UINodeTagConst.C_STAGE_GOAL_BIG_STEP_UNLOCK));
            });
        }
    }
}
