using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会排行榜界面
    /// </summary>
    public class GGUIMonoMiddayDungeonRank : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
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

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5403); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5403); } }
    }
}