using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnDishUnlock : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("菜品图标")]
        public RawImage imgDishIcon;
        [ALHeader("菜品名称")]
        public Text txtDishName;
        [ALHeader("菜品编号")]
        public Text txtDishNum;
        [ALHeader("菜品描述")]
        public Text txtDishDesc;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        [ALHeader("解锁按钮")]
        public GameObject btnUnlock;
        [ALHeader("解锁花费")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("可否解锁菜品时的显隐列表")]
        public List<GameObject> listCanUnlockShow;
        public List<GameObject> listCanUnlockHide;
        [ALHeader("满足条件但未获得菜谱时的显隐列表")]
        public List<GameObject> listConditionMetNoRecipeShow;
        [ALHeader("下一个菜品的按钮")]
        public GameObject btnNextDish;
        public List<GameObject> listHasNextDishShow;
        [ALHeader("上一个菜品的按钮")]
        public GameObject btnPrevDish;
        public List<GameObject> listHasPrevDishShow;


        public void setCanUnlock(bool _canUnlock, bool _conditionMetNoRecipe = false)
        {
            ALUGUICommon.setGameObjEnable(listCanUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listCanUnlockHide, false);
            ALUGUICommon.setGameObjEnable(listConditionMetNoRecipeShow, false);
            if (_conditionMetNoRecipe)
                ALUGUICommon.setGameObjEnable(listConditionMetNoRecipeShow, true);
            else
                ALUGUICommon.setGameObjEnable(_canUnlock ? listCanUnlockShow : listCanUnlockHide, true);
        }
        public void setHasNextDish(bool _hasNextDish)
        {
            ALUGUICommon.setGameObjEnable(listHasNextDishShow, _hasNextDish);
        }
        public void setHasPrevDish(bool _hasPrevDish)
        {
            ALUGUICommon.setGameObjEnable(listHasPrevDishShow, _hasPrevDish);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6413); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6413); } }
    }
}