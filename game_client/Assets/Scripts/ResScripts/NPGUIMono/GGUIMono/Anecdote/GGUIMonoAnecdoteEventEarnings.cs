using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAnecdoteEventEarnings : _AALBasicUIWndMono
    {
        [ALHeader("进度条")]
        public Slider sldProgress;
        [ALHeader("当前赚速和目标赚速")]
        public Text txtCurEarnings;
        public Text txtTargetEarnings;
        [ALHeader("赚速目标的描述")]
        public Text txtEarningsTargetDesc;
        [ALHeader("完成目标的奖励")]
        public NPGGUIMonoCommonItemContainer monoResultRewardList;
        [ALHeader("角色半身像")]
        public RawImage imgHeroRewardImage;
        public RawImage imgHeroRewardImageAdditional;
        [ALHeader("奖励有无角色时显示的内容")]
        public List<GameObject> listHasHeroRewardShow;
        public List<GameObject> listHasHeroRewardHide;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("领奖按钮")]
        public GameObject btnGainReward;

        [ALHeader("赚速任务完成时的显示和隐藏对象")]
        public List<GameObject> listCompleteShow;
        public List<GameObject> listCompleteHide;


        public void setComplete(bool _complete)
        {
            ALUGUICommon.setGameObjEnable(listCompleteShow, false);
            ALUGUICommon.setGameObjEnable(listCompleteHide, false);
            ALUGUICommon.setGameObjEnable(_complete ? listCompleteShow : listCompleteHide, true);
        }
        public void setHasHeroReward(bool _hasHeroReward)
        {
            ALUGUICommon.setGameObjEnable(listHasHeroRewardShow, false);
            ALUGUICommon.setGameObjEnable(listHasHeroRewardHide, false);
            ALUGUICommon.setGameObjEnable(_hasHeroReward ? listHasHeroRewardShow : listHasHeroRewardHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3406); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3406); } }
    }
}