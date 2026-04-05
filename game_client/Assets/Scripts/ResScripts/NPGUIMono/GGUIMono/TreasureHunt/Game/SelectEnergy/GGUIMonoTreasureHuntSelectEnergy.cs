using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 选择能源窗口
    /// </summary>
    public class GGUIMonoTreasureHuntSelectEnergy : _AALBasicUIWndMono
    {
        [ALHeader("能源列表")]
        public GGUIMonoTreasureHuntSelectEnergyItemContainer monoEnergyItemContainer;

        [ALHeader("选中能源详情信息")]
        public GGUISubMonoCommonItemDetail selectedItemDetail;

        [ALHeader("关闭窗口按钮")]
        public GameObject btnClose;
        
        [ALHeader("使用能源按钮")]
        public GameObject btnUse;
        [ALHeader("不能使用能源的置灰列表")]
        public List<MaskableGraphic> cannotUseEnergyGrayList;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6811); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6811); } }
    }
}