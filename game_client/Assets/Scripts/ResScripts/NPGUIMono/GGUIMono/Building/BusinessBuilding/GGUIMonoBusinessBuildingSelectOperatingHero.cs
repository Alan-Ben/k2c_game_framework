
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingSelectOperatingHero : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("已选择的随从列表")]
        public GGUIMonoBusinessBuildingOperatingHeroContainer monoHeroContainer;
        [ALHeader("当前的总委任加成")]
        public Text txtTotalBonus;
        [ALHeader("选择随从的列表")]
        public GGUIMonoBusinessBuildingOperatingHeroSelectGrid monoHeroSelectGrid;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1103); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1103); } }
    }
}