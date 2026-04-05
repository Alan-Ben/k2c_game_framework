using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡地图窗口
    /// </summary>
    public class GGUIMonoChapterMap : _ANPBasicUIWndResBarMono
    {
        [ALHeader("章节名称")]
        public Text txtChapterName;
        [ALHeader("章节名称2")]
        public Text txtChapterName2;
        [ALHeader("章节描述")]
        public Text txtChapterDesc;

        [ALHeader("背景图")]
        public RawImage bgTex;
        
        [ALHeader("故事按钮")]
        public GameObject btnStory;
        
        [ALHeader("章列表加载位置")]
        public Transform chapterListLoadParent;
        
        [ALHeader("当前关卡消耗描述")]
        public Text txtCostDesc;
        
        [ALHeader("自动按钮")]
        public GameObject btnAuto;
        [ALHeader("取消托管前进按钮")]
        public GameObject btnCancelAutoForward;
        [ALHeader("托管前进展示的go列表")]
        public List<GameObject> autoForwardShowGoList;
        [ALHeader("托管前进隐藏的go列表")]
        public List<GameObject> autoForwardHideGoList;
        
        
        [ALHeader("有未解锁建筑时显示物体列表")]
        public List<GameObject> hasUnlockBuildingShowGoList;
        [ALHeader("解锁的建筑图标")]
        public RawImage unlockBuildingImg;
        [ALHeader("解锁建筑描述")]
        public TextEx txtUnlockBuildingDesc;

        
        [ALHeader("章节未解锁展示的go列表")]
        public List<GameObject> lockShowGoList;
        [ALHeader("章节未解锁描述")]
        public Text txtLockDesc;
        [ALHeader("未解锁时候的引导按钮")]
        public GameObject btnLockSimpleTutorial;
        
        [ALHeader("窗口动画")]
        public Animation ani;
        [ALHeader("章节新开放动画")]
        public string openNewChapterAniName;
        
        [ALHeader("最后一关时候展示的go列表")]
        public List<GameObject> allPassShowGoList;
        [ALHeader("最后一关时候隐藏的go列表")]
        public List<GameObject> allPassHideGoList;
        
        [ALHeader("粒子id")]
        public long specialParticleId;
         
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2130); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2130); } }
    }
}