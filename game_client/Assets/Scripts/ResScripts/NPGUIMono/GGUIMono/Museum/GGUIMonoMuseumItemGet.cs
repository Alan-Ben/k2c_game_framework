using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMuseumItemGet : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("珍宝图标")]
        public RawImage imgMuseumItemIcon;
        [ALHeader("珍宝名字")]
        public Text txtMuseumItemName;
        [ALHeader("珍宝描述")]
        public Text txtMuseumItemDesc;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6424); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6424); } }
    }
}