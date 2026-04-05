using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingRnDPageProduct : _AALBasicUIWndMono
    {
        [ALHeader("当前员工数量")]
        public Text txtCurEmployeeCount;
        [ALHeader("产品的加成")]
        public GGUIMonoCommonPropertyItem monoProductBonus;
        [ALHeader("产品列表")]
        public GGUIMonoBusinessBuildingRnDPageProductGrid monoProductGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1119); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1119); } }
    }
}