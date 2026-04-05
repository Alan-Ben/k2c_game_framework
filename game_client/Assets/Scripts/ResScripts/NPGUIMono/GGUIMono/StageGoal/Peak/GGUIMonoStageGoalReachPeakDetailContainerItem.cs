using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标到达时代之巅详情弹窗列表item
    /// </summary>
    public class GGUIMonoStageGoalReachPeakDetailContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("到达时间")]
        public Text txtTime;
        [ALHeader("是自己时需要显示的GO列表")]
        public List<GameObject> goSelfShowList;
        [ALHeader("是自己时需要隐藏的GO列表")]
        public List<GameObject> goSelfHideList;
    }
}
