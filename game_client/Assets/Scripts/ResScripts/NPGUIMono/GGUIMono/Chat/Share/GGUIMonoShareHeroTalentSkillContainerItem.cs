using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴分享资质列表item
    /// </summary>
    public class GGUIMonoShareHeroTalentSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("资质图标")]
        public RawImage texIcon;
        [ALHeader("增加资质值")]
        public Text txtAddTalent;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时需要置灰的列表")]
        public List<MaskableGraphic> lockGrayList;
    }
}
