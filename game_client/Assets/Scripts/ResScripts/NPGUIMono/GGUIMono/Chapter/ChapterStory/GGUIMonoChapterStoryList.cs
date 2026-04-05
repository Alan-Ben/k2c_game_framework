using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChapterStoryList : _AALBasicUIWndMono
    {
        [ALHeader("关卡 - 故事列表")]
        public GGUIMonoChapterStoryContainer monoChapterStoryContainer;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2117); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2117); } }
    }
}