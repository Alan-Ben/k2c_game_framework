using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoTreasureHuntLabSelectContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("实验室名称")]
        public TextEx txtLabName;
        
        [ALHeader("banner图")]
        public RawImage bannerImage;

        [ALHeader("放入奇物进度")]
        public TextEx txtPutInTreasureProgress;
        [ALHeader("放入奇物进度key")]
        public string txtPutInTreasureProgressKey;
        
        [ALHeader("解锁状态显示")]
        public List<GameObject> unlockShow;
        [ALHeader("未解锁状态显示")]
        public List<GameObject> lockShow;
        
        [ALHeader("当前选中实验室显示列表")]
        public List<GameObject> nowSelectShow;
        
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
    }
}