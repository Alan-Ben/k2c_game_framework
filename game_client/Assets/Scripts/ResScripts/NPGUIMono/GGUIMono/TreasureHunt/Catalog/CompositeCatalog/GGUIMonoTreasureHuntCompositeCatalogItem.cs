using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴item
    /// </summary>
    public class GGUIMonoTreasureHuntCompositeCatalogItem : _TALUGUIMonoGridItem
    {
        [ALHeader("品质显示物体")]
        public GGUISubMonoQualityShowGo qualityShowGoMono;

        [ALHeader("组合名")]
        public TextEx txtCompositeName;
        
        [ALHeader("矿石列表")]
        public GGUIMonoTreasureHuntOreItemContainer monoOreItemContainer;
        
        [ALHeader("不同组合图鉴状态显示的列表")]
        public List<NPCommonEnumStatInfo<ETreasureHuntCompositeCatalogState>> compositeCatalogStateShowList;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("红点")]
        public NPGGUIMonoCommonRedTip monoRedTip;
    }
}