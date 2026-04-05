using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoShopTab:_AALBasicUIWndMono
    {
        [ALHeader("商店主表id")]
        public long shopMainRefId;
        [ALHeader("页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("未解锁显示的列表")]
        public List<GameObject> goLockedShowList;
        [ALHeader("未解锁隐藏的列表")]
        public List<GameObject> goLockedHideList;

        [ALHeader("播放的动画")]
        public Animation tabAnimation;
        [ALHeader("出现的时候播放的动画名称")]
        public string showAnimationStr;
        [ALHeader("隐藏的时候播放的动画名称")]
        public string hideAnimationStr;
        [ALHeader("选中时播放的动画名称")]
        public string selectAnimationStr;
        [ALHeader("取消选中播放的动画名称")]
        public string unselectAnimationStr;
    }
}