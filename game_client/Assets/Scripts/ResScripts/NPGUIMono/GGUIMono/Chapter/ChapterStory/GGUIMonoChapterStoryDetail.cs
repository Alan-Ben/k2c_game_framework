using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 故事详情窗口
    /// </summary>
    public class GGUIMonoChapterStoryDetail : _AALBasicUIWndMono
    {
        [ALHeader("故事名")]
        public TextEx storyName;

        [ALHeader("banner图")]
        public RawImage bannerImg;

        [ALHeader("进度百分比")]
        public TextEx txtProgressPercentage;
        
        [ALHeader("节容器")]
        public GGUIMonoChapterStoryStageContainer monoStageContainer;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2118); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2118); } }
    }
}