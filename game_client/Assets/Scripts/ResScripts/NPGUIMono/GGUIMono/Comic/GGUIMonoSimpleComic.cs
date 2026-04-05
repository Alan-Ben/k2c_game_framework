using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoSimpleComic : _AALBasicUIWndMono
    {
        [ALHeader("挂载父节点")]
        public Transform pageParent;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(22400); } }
        public static string objName { get { return UIResPathAssistant.getObjName(22400); } }
    }
}