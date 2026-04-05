using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物详情toolTip
    /// </summary>
    public class NPGGUIMonoCommonToolTip_TreasureHuntTreasureDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("奇物图标")]
        public RawImage imgTreasureIcon;
        
        [ALHeader("不同奇物状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntTreasureState>> treasureStateShowList;
        
        [ALHeader("奇物名称")]
        public Text txtTreasureName;

        [ALHeader("奇物描述文本")]
        public Text txtTreasureDesc;
        [ALHeader("未获取奇物描述")]
        public Text txtNotGetTreasureDesc;
        
        public void refreshTreasureState(ETreasureHuntTreasureState _state)
        {
            if(treasureStateShowList == null)
                return;
            
            NPCommonEnumStatInfo<ETreasureHuntTreasureState>.setStat(treasureStateShowList, _state);
        }
    }
}