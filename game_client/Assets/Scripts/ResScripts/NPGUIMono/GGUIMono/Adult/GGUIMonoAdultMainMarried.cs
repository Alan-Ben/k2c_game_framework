
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultMainMarried : _AALBasicUIWndMono
    {
        [ALHeader("总共结婚子嗣数量")]
        public Text txtTotalNum;
        [ALHeader("子嗣最大显示数量的文本描述")]
        public Text txtGridMaxCount;
        [ALHeader("已婚子嗣的列表")]
        public GGUIMonoAdultMarriedGrid monoAdultGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2512); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2512); } }
    }
}