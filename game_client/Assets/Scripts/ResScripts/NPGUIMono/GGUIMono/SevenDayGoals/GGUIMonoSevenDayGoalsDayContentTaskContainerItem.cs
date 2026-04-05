using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsDayContentTaskContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("任务标题")]
        public Text txtTile;
        [ALHeader("目标进度")]
        public NPGGUIMonoProgress monoGoalProgress;
        [ALHeader("任务奖励")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        [ALHeader("领奖按钮")]
        public GameObject btnGetReward;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        [ALHeader("积分数值")]
        public Text txtScore;

        [ALHeader("任务完成，任务未完成，已领奖的展示列表")]
        public List<GameObject> listCompleteShow;
        public List<GameObject> listUnCompleteShow;
        public List<GameObject> listRewardGainedShow;
        
        
        public void setTaskState(ECommonRewardType _stage)
        {
            ALUGUICommon.setGameObjEnable(listCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listUnCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listRewardGainedShow, false);
            
            switch (_stage)
            {
                case ECommonRewardType.CAN_GET_REWARD:
                    ALUGUICommon.setGameObjEnable(listCompleteShow, true);
                    break;
                case ECommonRewardType.NOT_GET_REWARD:
                    ALUGUICommon.setGameObjEnable(listUnCompleteShow, true);
                    break;
                case ECommonRewardType.HAS_GET_REWARD:
                    ALUGUICommon.setGameObjEnable(listRewardGainedShow, true);
                    break;
            }
        }
    }
}