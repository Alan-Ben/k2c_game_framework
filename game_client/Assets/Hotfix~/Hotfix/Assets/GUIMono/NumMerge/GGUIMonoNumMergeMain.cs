
using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public class GGUIMonoNumMergeMain : _AHotfixBaseMono
    {
        [HotfixMono("开始按钮")]
        public GameObject btnStart;
        
        [HotfixMono("活动正在进行中需要显示的GO列表")]
        public List<GameObject> goRunningShowList;
        [HotfixMono("活动结束需要显示的GO列表")]
        public List<GameObject> goEndShowList;
        [HotfixMono("活动结束时需要置灰的列表")]
        public List<MaskableGraphic> goGrayList;

        [ALHeader("图鉴按钮")]
        public GameObject btnHandbook;

        [ALHeader("首次进入活动对话id")]
        public long firstEnterShowDialogId;
    }
}