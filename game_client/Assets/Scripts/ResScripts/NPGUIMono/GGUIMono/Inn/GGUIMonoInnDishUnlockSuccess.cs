using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnDishUnlockSuccess : _AALBasicUIWndMono
    {
        [ALHeader("菜品名字")]
        public Text txtDishName;
        [ALHeader("菜品图标")]
        public RawImage imgDishIcon;
        [ALHeader("菜品描述")]
        public Text txtDishDesc;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6414); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6414); } }
    }
}
