using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP角色特殊奖励列表item
    /// </summary>
    public class GGUIMonoVIPActorItemContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("背景图片")]
        public RawImage imgBg;
        [ALHeader("半身像图片")]
        public RawImage imgBody;
        [ALHeader("伙伴相性")]
        public RawImage imgHeroAttr;
        [ALHeader("伙伴需要展示的GO列表")]
        public List<GameObject> goHeroShowList;
        [ALHeader("情人需要展示的GO列表")]
        public List<GameObject> goConsortShowList;
        [ALHeader("不同奖励状态显示的GO列表")]
        public List<NPCommonEnumStatInfo<ECommonRewardType>> rewardStatList;
    }
}
