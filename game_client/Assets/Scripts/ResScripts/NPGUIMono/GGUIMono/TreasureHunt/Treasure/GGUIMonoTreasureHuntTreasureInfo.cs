using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 奇物信息窗口
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureInfo : _AALBasicUIWndMono
    {
        [ALHeader("图片")]
        public RawImage texIcon;

        [ALHeader("不同奇物状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntTreasureState>> treasureStateShowList;

        [ALHeader("奇物名")]
        public TextEx txtTreasureName;
        [ALHeader("奇物描述")]
        public TextEx txtTreasureDesc;
        [ALHeader("未获取奇物描述")]
        public TextEx txtNotGetTreasureDesc;
        
        [ALHeader("品质背景")]
        public Image qualityBg;
        [ALHeader("是否需要加载品质物品特效")]
        public bool needQualityItemSfx;
        [ALHeader("品质物品特效父节点")]
        public Transform qualityItemSfxParent;
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;
        
        [ALHeader("是产出奇物时显示")]
        public List<GameObject> isOutputTreasureShow;
        [ALHeader("是产出奇物时隐藏")]
        public List<GameObject> isOutputTreasureHide;
        [ALHeader("是产出奇物前提下, 有产出可领取时显示")]
        public List<GameObject> hasOutputCanDrawShow;
        [ALHeader("是产出奇物前提下, 无产出可领取时显示")]
        public List<GameObject> noOutputCanDrawShow;
        
        [ALHeader("奇物点击按钮")]
        public GameObject btnTreasure;
        [ALHeader("点击时是否显示物品详情")]
        public bool isClickShowDetailToolTip = false;
        [ALHeader("详情ToolTip弹窗资源路径")]
        public NPCommonAssetPathInfo detailToolTipAssetPath;
        [ALHeader("物品详情偏移值")]
        public Vector2 detailToolTipInterval;
        
        public void refreshTreasureState(ETreasureHuntTreasureState _state)
        {
            if(treasureStateShowList == null)
                return;
            
            NPCommonEnumStatInfo<ETreasureHuntTreasureState>.setStat(treasureStateShowList, _state);
        }
    }
}