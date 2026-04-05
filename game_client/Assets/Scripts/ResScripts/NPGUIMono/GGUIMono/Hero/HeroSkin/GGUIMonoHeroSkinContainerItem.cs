using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤item
    /// </summary>
    public class GGUIMonoHeroSkinContainerItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("皮肤图标")]
        public RawImage imgIcon;
        [ALHeader("穿戴中GO")]
        public GameObject goCurWear;
        [ALHeader("皮肤等级")]
        public Text txtLevel;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("未解锁时需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("未解锁时需要置灰的列表")]
        public List<MaskableGraphic> goLockGrayList;
        [ALHeader("默认皮肤时需要显示的GO列表")]
        public List<GameObject> goDefaultShowList;
        [ALHeader("默认皮肤时需要隐藏的GO列表")]
        public List<GameObject> goDefaultHideList;
    }
}