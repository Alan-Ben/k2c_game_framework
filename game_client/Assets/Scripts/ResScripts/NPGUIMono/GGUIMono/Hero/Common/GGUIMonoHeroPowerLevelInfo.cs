using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用伙伴属性信息附加窗口
    /// </summary>
    public class GGUIMonoHeroPowerLevelInfo : _AALBasicUIWndMono
    {
        [ALHeader("总实力")]
        public Text txtTotalPower;
        [ALHeader("实力不展示大数字（选中不展示）")]
        public bool powerNoShowLargeNum;
        [ALHeader("实力详情按钮")]
        public GameObject btnPowerInfo;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("资质")]
        public Text txtTalent;
        [ALHeader("村庄收益")]
        public Text txtVillageIncome;
        [ALHeader("套系数量")]
        public Text txtSuitCount;
        [ALHeader("套系按钮")]
        public GameObject btnSuitDetail;
        [ALHeader("无套系时需要隐藏的GO列表")]
        public List<GameObject> goNoSuitHideList;
        [ALHeader("升级时总实力数值变化使用时间")]
        public float upgradeTotalPowerChgTime = 0.5f;
    }
}