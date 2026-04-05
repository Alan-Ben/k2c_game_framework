using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoFindThingsGame : _AALBasicUIWndMono
    {
        [ALHeader("游戏prefab挂载父节点")]
        public Transform parent;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10100); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10100);} }
    }
}