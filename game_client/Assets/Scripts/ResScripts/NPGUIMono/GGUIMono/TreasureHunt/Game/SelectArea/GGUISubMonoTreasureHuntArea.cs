using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubMonoTreasureHuntArea : _AALBasicUIWndMono
    {
        [ALHeader("区域id")]
        public long areaId;
        
        [ALHeader("区域名")]
        public TextEx txtAreaName;
        
        [ALHeader("未解锁时显示的物体列表")]
        public List<GameObject> lockShowGoList;
        [ALHeader("解锁时显示的物体列表")]
        public List<GameObject> unlockShowGoList;
        
        [ALHeader("解锁红点显示脚本")]
        public NPGGUIMonoCommonRedTip unlockRedTip;
        
        [ALHeader("选中时显示的物体列表")]
        public List<GameObject> selectedShowGoList;
        [ALHeader("未选中时显示的物体列表")]
        public List<GameObject> unSelectedShowGoList;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("收集进度文本")]
        public TextEx txtCollectionProgress;

        [ALHeader("选中进度文本颜色")]
        public Color selectedProgressColor = Color.black;
        [ALHeader("未选中进度文本颜色")]
        public Color unselectedProgressColor = Color.white;
    }
}