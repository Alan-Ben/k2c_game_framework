using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 矿石信息窗口
    /// </summary>
    public class GGUIMonoTreasureHuntOreInfo : _AALBasicUIWndMono
    {
        [ALHeader("普通矿石图片")]
        public RawImage texNormalIcon;
        [ALHeader("高级矿石图片")]
        public RawImage texAdvancedIcon;

        [ALHeader("不同矿石状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntOreState>> oreStateShowList;

        [ALHeader("矿石名")]
        public TextEx txtOreName;
        [ALHeader("矿石描述")]
        public TextEx txtOreDesc;
        [ALHeader("未获取矿石描述")]
        public TextEx txtNotGetOreDesc;
        
        [ALHeader("矿石数量")]
        public TextEx txtNum;
        [ALHeader("矿石数量使用的key, 一个参数:矿石数量")]
        public string txtNumKey;
        
        [ALHeader("矿石质量")]
        public TextEx txtMass;
        [ALHeader("矿石质量使用的key, 一个参数:矿石质量")]
        public string txtMassKey;
        
        [ALHeader("品质背景")]
        public Image qualityBg;
        [ALHeader("是否需要加载品质物品特效")]
        public bool needQualityItemSfx;
        [ALHeader("品质物品特效父节点")]
        public Transform qualityItemSfxParent;
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;
        
        [ALHeader("矿石点击按钮")]
        public GameObject btnOre;
        [ALHeader("点击时是否显示物品详情ToolTip")]
        public bool isClickShowDetailToolTip = false;
        [ALHeader("详情ToolTip弹窗资源路径")]
        public NPCommonAssetPathInfo detailToolTipAssetPath;
        [ALHeader("物品详情偏移值")]
        public Vector2 detailToolTipInterval;
        
        public void refreshOreState(ETreasureHuntOreState _state)
        {
            if(oreStateShowList == null)
                return;
            
            NPCommonEnumStatInfo<ETreasureHuntOreState>.setStat(oreStateShowList, _state);
        }
    }
}