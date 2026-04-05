using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 显示正在跟踪的任务
    /// </summary>
    public class GGUICustomMonoStageGoal : _ANPGGUIMonoCustomBasicWnd
    {
        [ALHeader("阶段标题文本")]
        public Text stageTtitleTxt;

        [ALHeader("当前阶段有可领取显示的GoList")]
        public List<GameObject> canGetRewardShowGoList;

        [ALHeader("当前阶段不可领取显示的GoList")]
        public List<GameObject> noGetRewardShowGoList;

        [ALHeader("阶段全部完成需要隐藏的GoList")]
        public List<GameObject> allDoneHideGoList;

        protected override void _onCustomUIEnable()
        {
#if NP_GAME
            _refresh(); 
            WinMsg.RegisterMsgAct(WinMsgType.ON_STAGE_GOAL_CHG, _onStageGoalChgMsg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _onStageGoalTaskChgMsg);

#endif
        }

        protected override void _onCustomUIDisable()
        {
#if NP_GAME
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STAGE_GOAL_CHG, _onStageGoalChgMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STAGE_GOAL_TASK_CHG, _onStageGoalTaskChgMsg);
#endif
        }

#if NP_GAME

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refresh()
        {
            if (null != NPPlayer.instance.stageGoalComp.stageRefObj)
                ALUGUICommon.setLabelTxt(stageTtitleTxt, NPPlayer.instance.stageGoalComp.stageRefObj.getTitle);

            bool canGetReward = NPPlayer.instance.stageGoalComp.subStepCanGetReward();
            ALUGUICommon.setGameObjEnable(canGetRewardShowGoList, canGetReward);
            ALUGUICommon.setGameObjEnable(noGetRewardShowGoList, !canGetReward);
            if (NPPlayer.instance.stageGoalComp.isAllDone)
                ALUGUICommon.setGameObjEnable(allDoneHideGoList, false);
        }

        private void _onStageGoalChgMsg()
        {
            _refresh();
        }

        private void _onStageGoalTaskChgMsg()
        {
            _refresh();
        }
#endif
    }
}