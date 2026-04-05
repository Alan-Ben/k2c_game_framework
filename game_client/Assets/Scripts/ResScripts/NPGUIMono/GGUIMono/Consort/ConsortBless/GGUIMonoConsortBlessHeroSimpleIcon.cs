using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子加护大臣icon
    /// </summary>
    public class GGUIMonoConsortBlessHeroSimpleIcon : _AALBasicUIWndMono
    {
        [ALHeader("大臣SimpleIcon")]
        public GGUIMonoHeroConsortSimpleIconContainerItem monoHeroIcon;

        [ALHeader("加成的实力值")]
        public TextEx txtAddPower;
        [ALHeader("加成的实力值Key")]
        public string txtAddPowerKey;

        [ALHeader("有大臣时显示")]
        public List<GameObject> hasHeroShow;
        [ALHeader("没有大臣时显示")]
        public List<GameObject> noHeroShow;

        [ALHeader("特效播放父节点")]
        public Transform sfxParent;
    }
}