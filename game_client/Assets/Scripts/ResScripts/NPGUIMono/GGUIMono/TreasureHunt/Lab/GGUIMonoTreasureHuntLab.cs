using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 实验室
    /// </summary>
    public class GGUIMonoTreasureHuntLab : _ANPBasicUIWndResBarMono
    {
        [ALHeader("实验室名")]
        public TextEx txtLabName;
        
        [ALHeader("可放入奇物信息子窗口")]
        public GGUIMonoTreasureHuntTreasureInfo canPutInTreasureInfoMono;
        [ALHeader("可放入的奇物数量")]
        public TextEx txtCanPutInTreasureNum;
        [ALHeader("有可放入的奇物时显示")]
        public List<GameObject> hasCanPutInTreasureShow;
        [ALHeader("放入按钮")]
        public GameObject btnPutIn;

        [ALHeader("选中的奇物信息子窗口")]
        public GGUIMonoTreasureHuntTreasureInfo selectedTreasureInfoMono;

        [ALHeader("奇物列表")]
        public GGUIMonoTreasureHuntTreasureSelectItemContainer monoTreasureContainer;
        
        [ALHeader("切换实验室按钮")]
        public GameObject btnChgLab;
        [ALHeader("上一个实验室按钮")]
        public GameObject btnPreLab;
        [ALHeader("上一个实验室按钮红点")]
        public List<GameObject> preLabBtnRedTip;
        [ALHeader("下一个实验室按钮")]
        public GameObject btnNextLab;
        [ALHeader("下一个实验室按钮红点")]
        public List<GameObject> nextLabBtnRedTip;
        
        [ALHeader("图鉴按钮")]
        public GameObject btnCatalog;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6803); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6803); } }
    }
}