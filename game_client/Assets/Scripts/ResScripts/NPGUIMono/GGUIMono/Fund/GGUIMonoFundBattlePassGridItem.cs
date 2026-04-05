using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundBattlePassGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("这个 step 达成的进度")]
        public Slider sldProgress;
        [ALHeader("当前阶段文本")]
        public Text txtStep;
        [ALHeader("目标值文本")]
        public Text txtGoalValue;
        [ALHeader("目标值简短文本")]
        public Text txtGoalValueBrief;
        [ALHeader("免费档位的奖励容器")]
        public GGUIMonoCommonRewardContainer monoFreeRewardContainer;
        [ALHeader("付费档位的奖励容器")]
        public GGUIMonoCommonLockRewardContainer monoPaidRewardContainer;
        [ALHeader("两个领取按钮")
        ,ALInfo("功能一样的，可以免费档位放一个，付费档位放一个")]
        public GameObject btnClaimAll;
        public GameObject btnClaimAll2;
        [ALHeader("有奖励免费可领取时的显示内容")]
        public List<GameObject> listCanDrawShow;
        public List<GameObject> listCanDrawHide;
        [ALHeader("有奖励付费可领取时的显示内容")]
        public List<GameObject> listCanDrawPaidShow;
        public List<GameObject> listCanDrawPaidHide;
        [ALHeader("是否是特殊阶段的显示内容")]
        public List<GameObject> listSpecialStepShow;
        public List<GameObject> listSpecialStepHide;
        
        
        public void setCanDrawState(bool _canDraw)
        {
            ALUGUICommon.setGameObjEnable(listCanDrawShow, false);
            ALUGUICommon.setGameObjEnable(listCanDrawHide, false);
            ALUGUICommon.setGameObjEnable(_canDraw ? listCanDrawShow : listCanDrawHide, true);
        }
        public void setCanDrawPaidState(bool _canDrawPaid)
        {
            ALUGUICommon.setGameObjEnable(listCanDrawPaidShow, false);
            ALUGUICommon.setGameObjEnable(listCanDrawPaidHide, false);
            ALUGUICommon.setGameObjEnable(_canDrawPaid ? listCanDrawPaidShow : listCanDrawPaidHide, true);
        }
        public void setSpecialStepState(bool _isSpecialStep)
        {
            ALUGUICommon.setGameObjEnable(listSpecialStepShow, false);
            ALUGUICommon.setGameObjEnable(listSpecialStepHide, false);
            ALUGUICommon.setGameObjEnable(_isSpecialStep ? listSpecialStepShow : listSpecialStepHide, true);
        }
    }
}