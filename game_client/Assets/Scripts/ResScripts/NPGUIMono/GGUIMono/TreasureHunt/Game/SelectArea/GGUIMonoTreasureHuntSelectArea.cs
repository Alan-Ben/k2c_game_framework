using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntSelectArea : _AALBasicUIWndMono
    {
        [ALHeader("区域脚本列表")]
        public List<GGUISubMonoTreasureHuntArea> areaMonoList;
        
        [ALHeader("选中区域名称文本")]
        public TextEx txtSelectedAreaName;

        [ALHeader("选中区域可获取的矿石信息子窗口")]
        public GGUIMonoTreasureHuntOreItemContainer monoCanGainOreContainer;
        [ALHeader("展示选中区域可获取所有按钮")]
        public GameObject btnShowAllCanGain;

        [ALHeader("普通矿石收集进度")]
        public TextEx txtNormalOreCollectProgress;
        [ALHeader("普通矿石收集进度key(两个参数, 1.当前进度 2.总进度)")]
        public string txtNormalOreCollectProgressKey;
        
        [ALHeader("高级矿石收集进度")]
        public TextEx txtAdvancedOreCollectProgress;
        [ALHeader("高级矿石收集进度key(两个参数, 1.当前进度 2.总进度")]
        public string txtAdvancedOreCollectProgressKey;

        [ALHeader("奇物收集进度")]
        public TextEx txtTreasureCollectProgress;
        [ALHeader("奇物收集进度Key, 两个参数(1.当前收集数量, 2.总数量)")]
        public string txtTreasureCollectProgressKey;
        
        [ALHeader("选中区域状态显示列表")]
        public List<NPCommonEnumStatInfo<ECommonLockState>> selectedAreaStateShowList;

        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6808); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6808); } }
    }
}