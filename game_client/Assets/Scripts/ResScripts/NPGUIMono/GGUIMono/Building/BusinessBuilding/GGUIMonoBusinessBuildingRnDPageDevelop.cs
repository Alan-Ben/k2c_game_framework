using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingRnDPageDevelop : _AALBasicUIWndMono
    {
        [ALHeader("当前的建筑等级")]
        public Text txtBuildingLevel;
        [ALHeader("业务的总加成")]
        public GGUIMonoCommonPropertyItem monoDevelopBonus;
        [ALHeader("业务列表")]
        public GGUIMonoBusinessBuildingRnDPageDevelopContainer monoDevelopContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1118); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1118); } }
    }
}