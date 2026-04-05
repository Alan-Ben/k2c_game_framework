using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsScoreRewardBarContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("人物奖励，其它奖励使用的 item ")]
        public NPGGUIMonoCommonItem monoCharacterRewardItem;
        public NPGGUIMonoCommonItem monoOtherRewardItem;
        [ALHeader("需求分数")]
        public Text txtScoreRequire;
        [ALHeader("领奖按钮")]
        public GameObject btnGainReward;
        [ALHeader("人物奖励，其它奖励展示的列表")]
        public List<GameObject> listCharacterRewardShow;
        public List<GameObject> listOtherRewardShow;
        [ALHeader("任务完成，任务未完成，已领奖的展示列表")]
        public List<GameObject> listCompleteShow;
        public List<GameObject> listUnCompleteShow;
        public List<GameObject> listRewardGainedShow;


        public void setItemType(ENPItemType _itemType)
        {
            ALUGUICommon.setGameObjEnable(listCharacterRewardShow, false);
            ALUGUICommon.setGameObjEnable(listOtherRewardShow, false);
         
            bool isCharacter = _itemType is ENPItemType.HERO or ENPItemType.CONSORT;   
            ALUGUICommon.setGameObjEnable(isCharacter ? listCharacterRewardShow : listOtherRewardShow, true);
        }
        public void setTaskState(bool _hadGetReward, bool _scoreEnough)
        {
            ALUGUICommon.setGameObjEnable(listCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listUnCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listRewardGainedShow, false);
            
            if (_hadGetReward)
                ALUGUICommon.setGameObjEnable(listRewardGainedShow, true);
            else if (_scoreEnough)
                ALUGUICommon.setGameObjEnable(listCompleteShow, true);
            else
                ALUGUICommon.setGameObjEnable(listUnCompleteShow, true);
        }
    }
}