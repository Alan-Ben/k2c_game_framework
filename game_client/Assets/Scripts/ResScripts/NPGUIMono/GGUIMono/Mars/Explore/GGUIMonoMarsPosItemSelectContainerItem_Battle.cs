using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsPosItemSelectContainerItem_Battle : _AALBasicUIWndMono
    {
        [ALHeader("等级文本")]
        public Text txtLevel;
        [ALHeader("图标图片")]
        public RawImage imgIcon;
        [ALHeader("名称文本")]
        public Text txtName;
        [ALHeader("推荐战力文本")]
        public Text txtRecommendPower;
        [ALHeader("探索按钮")]
        public GameObject btnGo;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7419); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7419); } }
    }
}