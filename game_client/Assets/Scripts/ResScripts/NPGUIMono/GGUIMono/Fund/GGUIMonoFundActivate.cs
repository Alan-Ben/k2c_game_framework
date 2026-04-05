using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundActivate : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("基金名称")]
        public Text txtName;
        [ALHeader("基金横幅")]
        public RawImage imgBanner;
        [ALHeader("激活提示")]
        public Text txtActivateTip;
        [ALHeader("性价比文本")]
        public Text txtProfitTip;
        [ALHeader("性价比物品展示")]
        public NPGGUIMonoCommonItem monoProfitItem;
        [ALHeader("激活后立即可获得的奖励")]
        public NPGGUIMonoCommonItemContainer monoActivateToObtain;
        [ALHeader("激活后可获得更多目标奖励")]
        public NPGGUIMonoCommonItemContainer monoActivateMoreGoals;
        [ALHeader("购买按钮")]
        public GGUIMonoCommonBuyButton monoBuyButton;
        [ALHeader("无激活可获得时的显示内容")]
        public List<GameObject> noItemObtainShow;
        public List<GameObject> noItemObtainHide;
        [ALHeader("无更多可获得时的显示内容")]
        public List<GameObject> noItemMoreGoalsShow;
        public List<GameObject> noItemMoreGoalsHide;


        public void setNoItemObtainState(bool _noItem)
        {
            ALUGUICommon.setGameObjEnable(noItemObtainShow, false);
            ALUGUICommon.setGameObjEnable(noItemObtainHide, false);
            ALUGUICommon.setGameObjEnable(_noItem ? noItemObtainShow : noItemObtainHide, true);
        }
        public void setNoItemMoreGoalsState(bool _noItem)
        {
            ALUGUICommon.setGameObjEnable(noItemMoreGoalsShow, false);
            ALUGUICommon.setGameObjEnable(noItemMoreGoalsHide, false);
            ALUGUICommon.setGameObjEnable(_noItem ? noItemMoreGoalsShow : noItemMoreGoalsHide, true);
        }
    }
}
