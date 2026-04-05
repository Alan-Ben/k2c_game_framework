using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoInnMenuGridItemState
    {
        [InspectorName("有菜谱且配置条件满足 === HAS_RECIPE_AND_CONDITION_ENABLE")]
        HAS_RECIPE_AND_CONDITION_ENABLE,
        [InspectorName("有菜谱但配置条件不满足 === HAS_RECIPE_BUT_CONDITION_DISABLE")]
        HAS_RECIPE_BUT_CONDITION_DISABLE,
        [InspectorName("已解锁且可升级 === UNLOCKED_AND_CAN_UPGRADE")]
        UNLOCKED_AND_CAN_UPGRADE,
        [InspectorName("已解锁但不可升级 === UNLOCKED_BUT_CANNOT_UPGRADE")]
        UNLOCKED_BUT_CANNOT_UPGRADE,
        [InspectorName("无菜谱 === NO_RECIPE")]
        NO_RECIPE,
        [InspectorName("无菜谱但配置条件满足 === NO_RECIPE_BUT_CONDITION_ENABLE")]
        NO_RECIPE_BUT_CONDITION_ENABLE,
    }
    public class GGUIMonoInnMenuGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("菜品名字")]
        public Text txtDishName;
        [ALHeader("菜品图片")]
        public RawImage imgDishIcon;
        [ALHeader("菜品编号")]
        public Text txtDishNum;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("解锁提示 icon ")]
        public RawImage imgUnlockSmallTipIcon;
        [ALHeader("解锁提示文本")]
        public Text txtUnlockSmallTip;
        [ALHeader("各种状态下的显示对象")
        ,ALInfo("依次是:\n" +
                "0. 有菜谱且配置条件满足\n" +
                "1. 有菜谱但配置条件不满足\n" +
                "2. 已解锁且可升级\n" +
                "3. 已解锁但不可升级\n" +
                "4. 无菜谱")]
        public List<GameObject> listHasRecipeAndConditionEnableShow;
        public List<GameObject> listHasRecipeButConditionDisableShow;
        public List<GameObject> listUnlockedAndCanUpgradeShow;
        public List<GameObject> listUnlockedButCannotUpgradeShow;
        public List<GameObject> listNoRecipeShow;


        public void setShowState(GGUIMonoInnMenuGridItemState _state)
        {
            ALUGUICommon.setGameObjEnable(listHasRecipeAndConditionEnableShow, false);
            ALUGUICommon.setGameObjEnable(listHasRecipeButConditionDisableShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockedAndCanUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockedButCannotUpgradeShow, false);
            ALUGUICommon.setGameObjEnable(listNoRecipeShow, false);
            
            switch (_state)
            {
                case GGUIMonoInnMenuGridItemState.HAS_RECIPE_AND_CONDITION_ENABLE:
                    ALUGUICommon.setGameObjEnable(listHasRecipeAndConditionEnableShow, true);
                    break;
                case GGUIMonoInnMenuGridItemState.HAS_RECIPE_BUT_CONDITION_DISABLE:
                    ALUGUICommon.setGameObjEnable(listHasRecipeButConditionDisableShow, true);
                    break;
                case GGUIMonoInnMenuGridItemState.UNLOCKED_AND_CAN_UPGRADE:
                    ALUGUICommon.setGameObjEnable(listUnlockedAndCanUpgradeShow, true);
                    break;
                case GGUIMonoInnMenuGridItemState.UNLOCKED_BUT_CANNOT_UPGRADE:
                    ALUGUICommon.setGameObjEnable(listUnlockedButCannotUpgradeShow, true);
                    break;
                case GGUIMonoInnMenuGridItemState.NO_RECIPE:
                case GGUIMonoInnMenuGridItemState.NO_RECIPE_BUT_CONDITION_ENABLE:
                    ALUGUICommon.setGameObjEnable(listNoRecipeShow, true);
                    break;
            }
        }
    }
} 