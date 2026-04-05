
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingDetail : _AALBasicUIWndMono
    {
        [ALHeader("相性 icon ")]
        public RawImage imgBuildingAttrIcon;
        [ALHeader("建筑 icon ")]
        public RawImage imgBuildingIcon;
        [ALHeader("建筑名字")]
        public Text txtName;
        [ALHeader("建筑的每秒总收益")]
        public Text txtTotalEarningsPerS;
        [ALHeader("每秒产出的金币数")]
        public Text txtTotalEarningsCoinPerS;
        [ALHeader("每秒总基础收益")]
        public Text txtBaseEarningsPerS;
        [ALHeader("伙伴经营能力")]
        public Text txtHeroEarningsPerS;
        [ALHeader("员工总收益")]
        public Text txtEmployeeEarningsPerS;
        [ALHeader("太空运输员工收益加成")]
        public Text txtEmployeeInnEarningsPerS;
        [ALHeader("太空打捞员工收益加成")]
        public Text txtEmployeeTreasureHuntEarningsPerS;
        [ALHeader("收益总加成")]
        public Text txtEarningsBonus;
        [ALHeader("委任伙伴的额外加成")]
        public Text txtPlacedHeroBonus;
        [ALHeader("店铺升级的额外加成")]
        public Text txtUpgradeBonus;
        [ALHeader("爬塔的收益加成")]
        public Text txtTowerEarningsPerS;
        [ALHeader("月卡的收益加成")]
        public Text txtMonthCardPerS;
        [ALHeader("年卡的收益加成")]
        public Text txtYearCardPerS;
        [ALHeader("其它来自别的系统的加成对象")]
        public GGUIMonoCommonPropertyDetail monoPropertyDetail;
        [ALHeader("建筑的描述")]
        public Text txtBuildingDesc;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1101); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1101); } }
    }
}