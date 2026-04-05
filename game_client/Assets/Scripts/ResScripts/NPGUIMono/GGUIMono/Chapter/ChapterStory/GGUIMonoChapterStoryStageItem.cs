using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChapterStoryStageItem : _AALBasicUIWndMono
    {
        [ALHeader("阶段名")]
        public TextEx txtStageName;

        [ALHeader("是否展开toggle")]
        public NPGGUIMonoCommonToggleEx monoToggle;
        [ALHeader("折叠时, item高度")]
        public float onFoldItemHeight;

        [ALHeader("有未查看剧情时")]
        public List<GameObject> hasUnCheckedPlotShowGoList;
        
        [ALHeader("剧情列表")]
        public GGUIMonoChapterStoryStagePlotContainer monoStagePlotContainer;
    }
}