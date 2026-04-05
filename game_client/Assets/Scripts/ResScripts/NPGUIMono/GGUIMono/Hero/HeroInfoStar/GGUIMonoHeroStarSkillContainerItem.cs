using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能列表item
    /// </summary>
    public class GGUIMonoHeroStarSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("觉醒图标")]
        public RawImage texIcon;
        [ALHeader("当前等级")]
        public Text txtLevel;
        [ALHeader("下个等级")]
        public Text txtNextLevel;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("详情按钮")]
        public GameObject btnInfo;
        [ALHeader("满级时需要展示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("展示下一等级时需要显示的GO列表")]
        public List<GameObject> goShowNextLevelShowList;
        [ALHeader("展示下一等级时需要隐藏的GO列表")]
        public List<GameObject> goShowNextLevelHideList;
        [ALHeader("升级成功特效id")]
        public long upgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
        [ALHeader("详情tip的X偏移")]
        public float detailTipIntervalX;
        [ALHeader("详情tip的Y偏移")]
        public float detailTipIntervalY;
    }
}
