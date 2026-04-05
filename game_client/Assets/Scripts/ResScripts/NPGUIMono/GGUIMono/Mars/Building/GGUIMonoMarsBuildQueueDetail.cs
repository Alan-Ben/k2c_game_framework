using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsBuildQueueDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("建造队列详情容器")]
        public GGUIMonoMarsBuildQueueDetailContainer monoContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7126); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7126); } }
    }
}