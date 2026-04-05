using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡 - 故事item
    /// </summary>
    public class GGUIMonoChapterStoryItem : _AALBasicUIWndMono
    {
        [ALHeader("故事名")]
        public TextEx storyName;

        [ALHeader("banner图")]
        public RawImage bannerImg;

        [ALHeader("进度百分比")]
        public TextEx txtProgressPercentage;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("有剧情奖励未领取时显示")]
        public List<GameObject> goHasPlotRewardUnDrawShow;
    }
}