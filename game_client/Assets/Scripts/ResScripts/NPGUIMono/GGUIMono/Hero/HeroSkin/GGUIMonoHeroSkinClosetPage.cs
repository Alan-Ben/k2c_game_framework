using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤衣柜页签
    /// </summary>
    public class GGUIMonoHeroSkinClosetPage : _AALBasicUIWndMono
    {
        [ALHeader("皮肤名称")]
        public Text txtName;
        [ALHeader("皮肤介绍")]
        public Text txtDesc;
        [ALHeader("穿戴按钮")]
        public GameObject btnWear;
        [ALHeader("解锁按钮")]
        public GameObject btnUnlock;
        [ALHeader("解锁道具")]
        public NPGGUIMonoCommonItem monoUnlockItem;
        [ALHeader("已穿戴时需要置灰的列表")]
        public List<MaskableGraphic> goWearGrayList;
        [ALHeader("已解锁时需要展示的GO列表")]
        public List<GameObject> goUnlockShowList;
        [ALHeader("已解锁时需要隐藏的GO列表")]
        public List<GameObject> goUnlockHideList;
    }
}

