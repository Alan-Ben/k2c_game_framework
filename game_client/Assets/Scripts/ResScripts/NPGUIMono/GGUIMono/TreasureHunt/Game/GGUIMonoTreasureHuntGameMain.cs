using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntGameMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("太空舱等级子窗口")]
        public GGUISubMonoTreasureHuntStationLevel monoStationLevel;

        [ALHeader("当前太空区域名称")]
        public TextEx txtNowAreaName;
        
        [ALHeader("切换太空区域按钮")]
        public GameObject btnChgArea;

        [ALHeader("切换能源按钮")]
        public GameObject btnChgEnergy;

        [ALHeader("开始按钮")]
        public GameObject btnPlay;

        [ALHeader("正在使用的能源")]
        public NPGGUIMonoCommonItem monoUsingEnergy;
        [ALHeader("交换的能源")]
        public NPGGUIMonoCommonItem monoExchangeEnergy;

        [ALHeader("有正在使用的能源显示对象")]
        public GameObject hasUsingEnergyShowGo;
        [ALHeader("有交换的能源显示对象")]
        public GameObject hasExchangeEnergyShowGo;
        
        [ALHeader("一键单次游玩开关")]
        public NPGGUIMonoCommonTab monoAkeyOncePlayToggle;
        [ALHeader("一键多次游玩开关")]
        public NPGGUIMonoCommonTab monoAkeyMultiPlayToggle;
        
        [ALHeader("材料室按钮")]
        public GameObject btnMaterialsRoom;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6807); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6807); } }
    }
}