using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChapterStoryStagePlotItem : _AALBasicUIWndMono
    {
        [ALHeader("剧情名")]
        public TextEx txtPlotName;

        [ALHeader("播放按钮")]
        public GameObject btnPlay;

        [ALHeader("不同状态显示物体列表")]
        public List<NPCommonEnumStatInfo<EGameCommonUnlockRewardType>> statInfoList;
    }
}