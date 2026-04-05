using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoUnlockConsortDetailInteractionStory : _AALBasicUIWndMono
    {
        [ALHeader("故事列表")]
        // public GGUIMonoConsortStoryGrid storyGrid;
        public GGUIMonoConsortStoryContainer storyContainer;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1425); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1425);} }
    }
}