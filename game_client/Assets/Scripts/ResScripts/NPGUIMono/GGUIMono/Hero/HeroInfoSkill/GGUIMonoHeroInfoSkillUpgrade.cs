using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 展示技能等级的动画类型
    /// </summary>
    public enum EShowSkillLevelAniType
    {
        [InspectorName("SHOW_LEVEL_CHG（显示等级提升值动画）")]
        SHOW_LEVEL_CHG,
        [InspectorName("HIDE_LEVEL_CHG（隐藏等级提升值动画）")]
        HIDE_LEVEL_CHG,
    }

    /// <summary>
    /// 骑士信息技能升级弹窗
    /// </summary>
    public class GGUIMonoHeroInfoSkillUpgrade : _AALBasicUIWndMono
    {
        [ALHeader("资质技能名字")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("当前等级")]
        public Text txtCurLevel;
        [ALHeader("升级需要技能点")]
        public Text txtSkillPoint;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("技能点不足时需要置灰的列表")]
        public List<MaskableGraphic> grayList;
        [ALHeader("满级时需要展示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("长按连续升级间隔（秒）")]
        [Min(0.1f)]
        public float longPressUpgradeInterval = 0.1f;

        [ALInfo("升级表现配置")]
        [ALHeader("item升级特效id")]
        public long itemUpgradeSfxId;
        [ALHeader("item升级特效后延时展示等级文本变化时间")]
        public float delayShowLevelChg;
        [ALHeader("等级跳动时间")]
        public float showLevelDuration = 0.5f;
        [ALHeader("等级滚动列表")]
        public GGUIMonoCommonShowScrollTextGrid monoLevelScrollGrid;
        [ALHeader("展示等级及资质提升的动画")]
        public CommonAnimationShowTypeInfo<EShowSkillLevelAniType> showAniInfo;

        [ALInfo("连续点提示配置")]
        [ALHeader("两次点击需要弹提示的间隔时间")]
        public float doubleClickShowTipDuration;
        [ALHeader("长按提示展示时间（如果小于等于0则一直显示）")]
        public float pressTipShowTime;
        [ALHeader("连续点提示GO")]
        public List<GameObject> goPressTipList;
    }
}

