using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTreasureHuntSelectEnergyItem : _AALBasicUIWndMono
    {
        [ALHeader("道具mono")]
        public GGUIMonoCommonSimpleItem monoItem;

        [ALHeader("没有道具数量时置灰列表")]
        public List<MaskableGraphic> noItemNumGrayList;
        
        [ALHeader("当前正在使用时显示物体列表")]
        public List<GameObject> usingShowGoList;

        [ALHeader("选中时显示物体列表")]
        public List<GameObject> selectedShowGoList;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}