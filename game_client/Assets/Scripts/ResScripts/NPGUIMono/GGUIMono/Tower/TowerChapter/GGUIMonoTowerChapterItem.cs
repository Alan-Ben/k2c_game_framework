using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class TowerChapterItemShow
    {
        [ALHeader("章节特殊显示信息")]
        public long chapterId; //章节ID
        [ALHeader("章节Level")]
        public Text txtTotalLevel;
        [ALHeader("需要显示的GameObject列表")]
        public List<GameObject> showGos = new List<GameObject>(); //需要显示的GameObject列表
    }
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoTowerChapterItem : _TALUGUIMonoGridItem
    {
        [ALHeader("挑战按钮")]
        public GameObject btnChallenge;
        [ALHeader("关卡名称文本")]
        public Text txtLevelName;
        [ALHeader("守卫名称文本")]
        public Text txtBossName;
        [ALHeader("章节Level")]
        public Text txtTotalLevel;
        [ALHeader("每日获得金币数量文本")]
        public Text txtDailyCoins;
        [ALHeader("战力")]
        public Text txtPower;
        [ALHeader("BossIcon图片")]
        public RawImage texIcon;
        [ALHeader("背景图片")]
        public RawImage texBg;
        [ALHeader("我当前所在的关卡需要显示的Go")]
        public List<GameObject> curLevelShowGos;
        [ALHeader("有玩家信息时需要显示的Go")]
        public List<GameObject> hasPlayerInfoShowGos;
        [ALHeader("无玩家信息时需要显示的Go")]
        public List<GameObject> hasPlayerInfoHideGos;
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon playerInfo;
        [ALHeader("玩家名字颜色控制")]
        public Graphic playerColorGraphic; //玩家颜色显示
        [ALHeader("玩家本人名字颜色")]
        public Color myPlayerColor = Color.cyan;
        [ALHeader("其他玩家的名字颜色")]
        public Color otherPlayerColor = Color.white;
        [ALHeader("有科技需要显示的Go")]
        public List<GameObject> researchShowGos;
        [ALHeader("研究关卡收益加成")]
        public Text txtResearchEarnBonus;
        [ALHeader("章节Name")]
        public List<Text> txtChapterNameList;
        [ALHeader("每个章节需要的特殊显示")]
        public List<TowerChapterItemShow> chapterItemShows = new List<TowerChapterItemShow>();
    
        [ALHeader("背景颜色控制")]
        public Graphic bgColorGraphic; //玩家颜色显示
        [ALHeader("通用背景交叉颜色")]
        public Color bgCommonColor1 = Color.white;
        public Color bgCommonColor2 = Color.gray;
        [ALHeader("玩家当前所在关卡背景颜色")]
        public Color bgMyPlayerColor = new Color(103/255f, 78/255f, 190/255f);
        [ALHeader("文本颜色控制")]
        public List<Graphic> txtColorGraphic; //玩家颜色显示
        [ALHeader("通用文本颜色")]
        public Color txtCommonColor = Color.black;
        [ALHeader("玩家当前所在关卡文本颜色")]
        public Color txtMyPlayerColor = Color.white;
        [ALHeader("玩家或boss详情")]
        public GameObject btnInfoDetail;
        [ALHeader("玩家或boss详情信息位置")]
        public RectTransform detailTipTarget;   
        [ALHeader("玩家或boss详情信息偏移")]
        public float playerTipInterval = 0;
    }
}
