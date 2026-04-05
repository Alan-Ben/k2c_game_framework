using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnSpecialGuestServeChoice : _AALBasicUIWndMono
    {
        [ALHeader("玩家名字")]
        public Text txtPlayerName;
        [ALHeader("描述文本")]
        public Text txtDesc;
        [ALHeader("选项容器")]
        public GGUIMonoInnSpecialGuestServeChoiceOptionContainer optionContainer;
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6442); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6442); } }
    }
}