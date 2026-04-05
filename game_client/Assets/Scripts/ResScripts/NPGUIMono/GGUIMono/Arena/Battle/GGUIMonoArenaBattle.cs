using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIArenaBattleOpponentHeroItem
    {
        [ALHeader("伙伴item")]
        public GGUIMonoArenaBattleSubHeroInfoItem monoHeroItem;
        [ALHeader("选中该伙伴时需要播的动画")]
        public CommonAnimationSingleInfo aniSelect;
    }

    /// <summary>
    /// 竞技场战斗主界面
    /// </summary>
    public class GGUIMonoArenaBattle : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;
        [ALHeader("便捷设置按钮")]
        public GameObject btnConvenientSetting;
        [ALHeader("选择增益按钮")]
        public GameObject btnSelectBuff;
        [ALHeader("一键设置按钮")]
        public GameObject btnOneKeySetting;
        [ALHeader("连胜数量")]
        public Text txtWinCount;
        [ALHeader("没有连胜次数时需要隐藏的GO列表")]
        public List<GameObject> goNoWinCountHideList;
        [ALHeader("对手信息")]
        public GGUIMonoArenaBattleSubOpponentInfo monoSubOpponentInfo;
        [ALHeader("自己伙伴详细信息")]
        public GGUIMonoArenaBattleSubSelfHeroInfo monoSubSelfHeroInfo;
        [ALHeader("战场上自己伙伴信息item")]
        public GGUIMonoArenaBattleSubHeroInfoItem monoSelfHeroItem;
        [ALHeader("战场上对手伙伴信息item列表")]
        public List<GGUIArenaBattleOpponentHeroItem> monoSubOpponentHeroItemList;
        [ALHeader("一键战斗未解锁时显示的GO列表")]
        public List<GameObject> goOneKeyLockShowList;
        [ALHeader("一键战斗未解锁时隐藏的GO列表")]
        public List<GameObject> goOneKeyLockHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5209); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5209); } }
    }
}
