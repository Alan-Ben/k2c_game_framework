using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场结算弹窗
    /// </summary>
    public class GGUIMonoArenaBattleFinalResult : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("标题")]
        public TextMeshProUGUIEx txtTitleEx;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("我方影响力变化")]
        public Text txtSelfInfluenceChg;
        [ALHeader("对方影响力变化")]
        public Text txtOpponentInfluenceChg;
        [ALHeader("伙伴半身像")]
        public RawImage imgHero;
        [ALHeader("伙伴形象")]
        public GGUIMonoCommonShowCase monoHeroShowCase;
        [ALHeader("我方影响力图标")]
        public RawImage imgSelfInfluence;
        [ALHeader("对方影响力图标")]
        public RawImage imgOpponentInfluence;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoCommonItemContainer;
        [ALHeader("有奖励时显示的GO列表")]
        public List<GameObject> goHaveRewardShowList;
        [ALHeader("有奖励时隐藏的GO列表")]
        public List<GameObject> goHaveRewardHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5213); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5213); } }
    }
}