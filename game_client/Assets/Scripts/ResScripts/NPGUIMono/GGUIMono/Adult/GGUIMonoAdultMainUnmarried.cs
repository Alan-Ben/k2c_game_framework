
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultMainUnmarried : _AALBasicUIWndMono
    {
        [ALHeader("人数上限提示")]
        public Text txtLimitDesc;
        [ALHeader("子嗣数量")]
        public Text txtAdultNum;
        [ALHeader("未婚子嗣的列表")]
        public GGUIMonoAdultUnmarriedGrid monoAdultGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2511); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2511); } }
    }
}