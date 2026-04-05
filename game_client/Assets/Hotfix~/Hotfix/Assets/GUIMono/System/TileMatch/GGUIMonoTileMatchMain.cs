using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 三消活动页面
    /// </summary>
    public class GGUIMonoTileMatchMain : _AHotfixBaseMono
    {
        [HotfixMono("开始按钮")]
        public GameObject btnStart;
        
        [HotfixMonoAttribute("活动正在进行中需要显示的GO列表")]
        public List<GameObject> goRunningShowList;
        [HotfixMonoAttribute("活动结束需要显示的GO列表")]
        public List<GameObject> goEndShowList;
        [HotfixMonoAttribute("活动结束时需要置灰的列表")]
        public List<MaskableGraphic> goGrayList;

        [ALHeader("首次进入三消活动对话id")]
        public long firstEnterShowDialogId;
    }
}