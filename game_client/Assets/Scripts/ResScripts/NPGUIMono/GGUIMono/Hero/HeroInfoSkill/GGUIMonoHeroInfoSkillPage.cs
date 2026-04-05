using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士信息技能页签
    /// </summary>
    public class GGUIMonoHeroInfoSkillPage : _AALBasicUIWndMono
    {
        [ALHeader("技能点")]
        public Text txtSkillPoint;
        [ALHeader("技能提升页面")]
        public GGUIMonoHeroInfoSkillUpgrade monoSkillUpgrade;
        [ALHeader("关闭技能提升页面按钮")]
        public GameObject btnCloseUpgrade;

        [ALHeader("打开提升页面动画")]
        public Animation showUpgradePageAni;
        [ALHeader("打开提升页面动画名")]
        public string showUpgradePageAniName;
    }
}

