using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 质量排名
    /// </summary>
    public class GGUIMonoTreasureHuntOreMassRank : _AALBasicUIWndMono
    {
        [ALHeader("标题文本")]
        public TextEx txtTitle; //{0}矿石质量排名
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("排行item列表, 从排行高到低")]
        public List<GGUIMonoTreasureHuntOreMassRankItem> rankItemList;
        [ALHeader("自己的排行文本")]
        public TextEx selfRankText;
        [ALHeader("没有排行显示")]
        public List<GameObject> noRankShow;
        
        [ALHeader("矿石信息")]
        public GGUIMonoTreasureHuntOreInfo monoOreInfo;
        
        [ALHeader("奖励进度条")]
        public GGUIMonoCommonRewardSliderPro monoRewardSlider;
        [ALHeader("奖励进度上item预制路径")]
        public NPCommonAssetPathInfo rewardSliderItemAssetPath;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6823); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6823); } }
    }
}