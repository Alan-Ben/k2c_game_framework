using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnDishLevelUp : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("菜品名字")]
        public Text txtDishName;
        [ALHeader("菜品编号")]
        public Text txtDishNum;
        [ALHeader("菜品描述")]
        public Text txtDishDesc;
        [ALHeader("菜品图标")]
        public RawImage imgDishIcon;
        [ALHeader("菜品的人气值加成")]
        public Text txtPopularityAdd;
        [ALHeader("菜品的心意值加成")]
        public Text txtAffectionAdd;
        [ALHeader("菜品的熟练度加成")]
        public Text txtFinesseAdd;
        [ALHeader("菜品的相性图片")]
        public RawImage imgDishAttr;
        [ALHeader("等级变化展示")]
        public CommonUpgradePropertyShow<Text> levelUpgradeShow;
        [ALHeader("加成类型")]
        public EBonusPropertyType bonusPropertyType;
        [ALHeader("加成值是否展示为百分比")]
        public bool isPercentage = true;
        [ALHeader("加成的描述")]
        public Text txtBonusDesc;
        [ALHeader("加成变化展示")]
        public CommonUpgradePropertyShow<Text> bonusUpgradeShow;
        [ALHeader("当前的熟练度进度")]
        public NPGGUIMonoProgress monoFinesseProgress;
        [ALHeader("升级按钮")]
        public GameObject btnLevelUp;
        [ALHeader("可以升级时显示的内容")]
        public List<GameObject> listCanUpgradeShow;
        [ALHeader("是否满级显示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("下一个菜品的按钮")]
        public GameObject btnNextDish;
        public List<GameObject> listHasNextDishShow;
        [ALHeader("上一个菜品的按钮")]
        public GameObject btnPrevDish;
        public List<GameObject> listHasPrevDishShow;


        public void setCanUpgrade(bool _canUpgrade)
        {
            ALUGUICommon.setGameObjEnable(listCanUpgradeShow, _canUpgrade);
        }
        public void setLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        public void setHasNextDish(bool _hasNextDish)
        {
            ALUGUICommon.setGameObjEnable(listHasNextDishShow, _hasNextDish);
        }
        public void setHasPrevDish(bool _hasPrevDish)
        {
            ALUGUICommon.setGameObjEnable(listHasPrevDishShow, _hasPrevDish);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6412); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6412); } }
    }
}