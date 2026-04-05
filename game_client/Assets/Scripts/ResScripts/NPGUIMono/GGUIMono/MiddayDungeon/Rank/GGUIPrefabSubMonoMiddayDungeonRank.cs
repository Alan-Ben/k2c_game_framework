using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会排行榜界面
    /// </summary>
    public class GGUIPrefabSubMonoMiddayDungeonRank : _AALBasicUIWndMono
    {
        [ALHeader("前几名玩家信息展示，排名按配置顺序")]
        public List<GGUIMonoRankFixedTopPlayerInfo> monoTopPlayerList;
        [ALHeader("排行榜列表")]
        public GGUIMonoMiddayDungeonRankGrid monoRankGrid;
        [ALHeader("自己的排名")]
        public Text txtSelfRank;
        [ALHeader("自己的积分")]
        public Text txtRankScore;
        [ALHeader("没有积分需要展示的GO")]
        public List<GameObject> noScoreShowList;
        [ALHeader("没有积分需要隐藏的GO")]
        public List<GameObject> noScoreHideList;
    }
}