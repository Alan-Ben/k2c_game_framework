using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空寻宝主页面
    /// </summary>
    public class GGUIMonoTreasureHuntMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("太空舱等级子窗口")]
        public GGUISubMonoTreasureHuntStationLevel monoStationLevel;

        [ALHeader("实验室按钮")]
        public GameObject btnLab;

        [ALHeader("标本间按钮")]
        public GameObject btnSpecimenRoom;

        [ALHeader("获取能源按钮")]
        public GameObject btnGainEnergy;

        [ALHeader("游玩按钮")]
        public GameObject btnPlay;

        [ALHeader("图鉴按钮")]
        public GameObject btnCatalog;
        
        [ALHeader("材料室按钮")]
        public GameObject btnMaterialsRoom;

        [ALHeader("收集奖励按钮")]
        public GameObject btnCollectReward;

        [ALHeader("当前所在区域图标")]
        public RawImage curInAreaIcon;
        [ALHeader("当前所在区域名称文本")]
        public TextEx txtCurInAreaName;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6800); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6800); } }
    }
}