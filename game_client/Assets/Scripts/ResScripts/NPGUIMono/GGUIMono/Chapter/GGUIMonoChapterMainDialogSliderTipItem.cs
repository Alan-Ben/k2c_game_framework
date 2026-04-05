using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡剧情进度条tip
    /// </summary>
    public class GGUIMonoChapterMainDialogSliderTipItem : _AALBasicUIWndMono
    {
        [ALHeader("经过展示的动画")]
        public Animation targetAnimation;
        [ALHeader("经过展示的动画名字")]
        public string targetAnimationName;
        
        [ALHeader("完成展示的go列表")]
        public List<GameObject> completeShowGoList;
    }
}