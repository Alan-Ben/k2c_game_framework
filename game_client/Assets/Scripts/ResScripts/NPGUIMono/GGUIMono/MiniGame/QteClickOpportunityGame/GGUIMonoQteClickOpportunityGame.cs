using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoQteClickOpportunityGame : _AALBasicUIWndMono
    {
        [ALHeader("游戏prefab父节点")]
        public Transform prefabParent;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10500); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10500);} }
    }
}