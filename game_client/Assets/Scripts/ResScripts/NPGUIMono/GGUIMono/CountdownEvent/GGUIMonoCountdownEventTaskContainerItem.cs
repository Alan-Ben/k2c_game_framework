using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 倒计时事件任务列表item
    /// </summary>
    public class GGUIMonoCountdownEventTaskContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("任务描述")]
        public Text txtDesc;
        [ALHeader("任务完成时需要显示的GO列表")]
        public List<GameObject> goDoneShowList;
        [ALHeader("任务完成时需要隐藏的GO列表")]
        public List<GameObject> goDoneHideList;
        [ALHeader("任务完成时计数颜色")]
        public Color doneColor = Color.green;
        [ALHeader("任务未完成时计数颜色")]
        public Color undoneColor = Color.red;
        [ALHeader("任务进度条")]
        public Slider sldProcess;
    }
}
