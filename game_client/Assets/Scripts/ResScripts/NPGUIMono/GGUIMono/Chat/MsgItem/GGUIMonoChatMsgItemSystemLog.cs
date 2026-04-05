using ChatPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChatMsgItemSystemLog : _AGUIMonoChatMsgListItem
    {
        [ALHeader("加载ui父节点")]
        public Transform uiParent;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1377); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1377);} }
    }
}