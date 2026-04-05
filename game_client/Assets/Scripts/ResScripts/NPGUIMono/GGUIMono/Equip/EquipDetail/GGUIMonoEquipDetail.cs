using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品主界面
    /// </summary>
    public class GGUIMonoEquipDetail : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("分解按钮")]
        public GameObject btnRecycle;
        [ALHeader("锁定按钮")]
        public GameObject btnLock;
        [ALHeader("描述详情按钮")]
        public GameObject btnDescDetail;
        [ALHeader("上一个按钮")]
        public GameObject btnPre;
        [ALHeader("下一个按钮")]
        public GameObject btnNext;
        [ALHeader("品质GO父节点")]
        public Transform goQualityParent;
        [ALHeader("等级附加图标")]
        public RawImage imgAdditionQualityIcon;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("描述2")]
        public Text txtDesc2;
        [ALHeader("获取途径")]
        public Text txtSource;
        [ALHeader("藏品图标")]
        public RawImage imgEquip;
        [ALHeader("初始资质值")]
        public Text txtInitTalent;
        [ALHeader("初始技能数")]
        public Text txtInitSkillNum;
        [ALHeader("锁定时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("锁定时需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("有获取途径时需要显示的GO列表")]
        public List<GameObject> goHaveSourceShowList;
        [ALHeader("有获取途径时需要隐藏的GO列表")]
        public List<GameObject> goHaveSourceHideList;
        [ALHeader("描述tip的x偏移量")]
        public float descTipIntervalX;
        [ALHeader("描述tip的y偏移量")]
        public float descTipIntervalY;
        [ALHeader("单次升级成功特效id")]
        public long singleUpgradeSfxId;
        [ALHeader("十连升级成功特效id")]
        public long tenUpgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
    }
}
