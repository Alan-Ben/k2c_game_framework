
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBuildingMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("阶段目标的入口按钮")]
        public GGUIMonoStageGoalBtn monoStageGoalBtn;
        [ALHeader("聊天入口")]
        public NPGGUIMonoMiniChat monoMiniChat;
        [ALHeader("展开bar按钮")]
        public GameObject btnExpandBar;
        [ALHeader("展开bar动画")]
        public CommonAnimationSingleInfo aniExpandBar;
        [ALHeader("收缩bar按钮")]
        public GameObject btnContractBar;
        [ALHeader("收缩bar动画")]
        public CommonAnimationSingleInfo aniContractBar;
        [ALHeader("VIP入口图标")]
        public RawImage imgVIPEntryIcon;
        [ALHeader("VIP入口是顾问时显示的GO列表")]
        public List<GameObject> goVIPHeroShowList;
        [ALHeader("VIP入口是情人时显示的GO列表")]
        public List<GameObject> goVIPConsortShowList;
        [ALHeader("初始VIP入口展示VIP等级对应图标")]
        public long initShowVIPLevel;
        [ALHeader("情人收集入口")]
        public GGUIMonoLoverCollectBtn monoLoverCollectBtn;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1198); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1198); } }
    }
}