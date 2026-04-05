using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 万能活动主界面
    /// </summary>
    public class GGUIMonoRegularEventMain : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("开始按钮")]
        public GameObject btnStart;
        [HotfixMonoAttribute("消耗商店按钮")]
        public GameObject btnShop;
        [HotfixMonoAttribute("活动正在进行中需要显示的GO列表")]
        public List<GameObject> goRunningShowList;
        [HotfixMonoAttribute("活动结束需要显示的GO列表")]
        public List<GameObject> goEndShowList;
        [HotfixMonoAttribute("活动结束时需要置灰的列表")]
        public List<MaskableGraphic> goGrayList;
    }
}