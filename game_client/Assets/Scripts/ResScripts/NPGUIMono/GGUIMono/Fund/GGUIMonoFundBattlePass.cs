using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoFundBattlePass : _AALBasicUIWndMono
    {
        [ALHeader("活动时间")]
        public Text txtActivityTime;
        [ALHeader("奖励预览按钮")]
        public GameObject btnPreview;
        [ALHeader("商店按钮")]
        public GameObject btnShop;
        [ALHeader("积分获取方式按钮")]
        public GameObject btnTaskDetail;
        [ALHeader("基金名称")]
        public Text txtName;
        [ALHeader("激活文字介绍")]
        public Text txtActivateTip;
        [ALHeader("已激活提示文字")]
        public Text txtActivatedTip;
        [ALHeader("性价比提示文字")]
        public Text txtProfitTip;
        [ALHeader("性价比物品展示")]
        public NPGGUIMonoCommonItem monoProfitItem;
        [ALHeader("横幅图片")]
        public RawImage imgBanner;
        [ALHeader("投放的大奖")]
        public NPGGUIMonoCommonItem monoSpecialItem;
        [ALHeader("购买按钮")]
        public GameObject btnBuy;
        [ALHeader("一键领取按钮")]
        public GameObject btnClaimAll;
        [ALHeader("经验名称文本")]
        public Text txtExpName;
        [ALHeader("奖励列表")]
        public GGUIMonoFundBattlePassGrid monoStepGrid;
        [ALHeader("下一个目标的奖励")]
        public GGUIMonoFundBattlePassGridItem monoLastStep;
        [ALHeader("已激活时的显示和隐藏内容")]
        public List<GameObject> listActivatedShow;
        public List<GameObject> listActivatedHide;
        [ALHeader("大奖的显示相关内容")
        ,ALInfo("依次是：有大奖物品（没获得），有大奖物品（已获得），无大奖物品")]
        public List<GameObject> listHasSpecialItemShow;
        public List<GameObject> listGotSpecialItemShow;
        public List<GameObject> listNoSpecialItemShow;
        [ALHeader("和活动有关的显示内容")]
        public List<GameObject> listActivityRelatedShow;
        public List<GameObject> listActivityRelatedHide;
        [ALHeader("有奖励可领取时的显示内容")]
        public List<GameObject> listCanDrawShow;
        public List<GameObject> listCanDrawHide;
        
        
        public void setActivatedState(bool _activated)
        {
            ALUGUICommon.setGameObjEnable(listActivatedShow, false);
            ALUGUICommon.setGameObjEnable(listActivatedHide, false);
            ALUGUICommon.setGameObjEnable(_activated ? listActivatedShow : listActivatedHide, true);
        }
        public void setSpecialItemState(bool _hasSpecialItem, bool _gotSpecialItem)
        {
            ALUGUICommon.setGameObjEnable(listHasSpecialItemShow, false);
            ALUGUICommon.setGameObjEnable(listGotSpecialItemShow, false);
            ALUGUICommon.setGameObjEnable(listNoSpecialItemShow, false);

            if (_hasSpecialItem)
                ALUGUICommon.setGameObjEnable(_gotSpecialItem ? listGotSpecialItemShow : listHasSpecialItemShow, true);
            else
                ALUGUICommon.setGameObjEnable(listNoSpecialItemShow, true);
        }
        public void setActivityRelatedState(bool _related)
        {
            ALUGUICommon.setGameObjEnable(listActivityRelatedShow, false);
            ALUGUICommon.setGameObjEnable(listActivityRelatedHide, false);
            ALUGUICommon.setGameObjEnable(_related ? listActivityRelatedShow : listActivityRelatedHide, true);
        }
        public void setCanDrawState(bool _canDraw)
        {
            ALUGUICommon.setGameObjEnable(listCanDrawShow, false);
            ALUGUICommon.setGameObjEnable(listCanDrawHide, false);
            ALUGUICommon.setGameObjEnable(_canDraw ? listCanDrawShow : listCanDrawHide, true);
        }
    }
}