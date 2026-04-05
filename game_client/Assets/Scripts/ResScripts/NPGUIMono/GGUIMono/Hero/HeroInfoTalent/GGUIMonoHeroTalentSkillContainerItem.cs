using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴资质列表item
    /// </summary>
    public class GGUIMonoHeroTalentSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("资质图标")]
        public RawImage texIcon;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中按钮")]
        public GameObject btnSelect;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时需要置灰的列表")]
        public List<MaskableGraphic> lockGrayList;
        [ALHeader("点击详情按钮")]
        public GameObject btnClickDetail;
        [ALHeader("升级成功特效id")]
        public long upgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
        [ALHeader("解锁动画")]
        public CommonAnimationSingleInfo aniUnlock;
        [ALHeader("可升级红点")]
        public GameObject goUpgradeRedTip;
    }
}
