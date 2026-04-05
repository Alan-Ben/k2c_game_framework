using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 矿石详情toolTip
    /// </summary>
    public class NPGGUIMonoCommonToolTip_TreasureHuntOreDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("普通矿石图标")]
        public RawImage imgNormalOreIcon;
        [ALHeader("高级矿石图标")]
        public RawImage imgAdvancedOreIcon;
        
        [ALHeader("品质背景")]
        public Image qualityBg;
        [ALHeader("是否需要加载品质物品特效")]
        public bool needQualityItemSfx;
        [ALHeader("品质物品特效父节点")]
        public Transform qualityItemSfxParent;
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;
        
        [ALHeader("不同矿石状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntOreState>> oreStateShowList;
        
        [ALHeader("矿石名称")]
        public Text txtOreName;

        [ALHeader("矿石持有数量文本")]
        public Text txtNum;
        [ALHeader("矿石持有数量使用的key")]
        public string txtNumKey;

        [ALHeader("矿石描述文本")]
        public Text txtOreDesc;
        [ALHeader("未获取矿石描述")]
        public TextEx txtNotGetOreDesc;
        
        public void refreshOreState(ETreasureHuntOreState _state)
        {
            if(oreStateShowList == null)
                return;
            
            NPCommonEnumStatInfo<ETreasureHuntOreState>.setStat(oreStateShowList, _state);
        }
    }
}