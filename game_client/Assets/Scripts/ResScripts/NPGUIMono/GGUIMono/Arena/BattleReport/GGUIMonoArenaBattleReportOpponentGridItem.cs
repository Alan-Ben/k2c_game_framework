using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战报对手列表item
    /// </summary>
    public class GGUIMonoArenaBattleReportOpponentGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("击败伙伴数量")]
        public Text txtDefeatHeroCount;
        [ALHeader("影响力变更")]
        public Text txtInfluenceChg;
        [ALHeader("时间")]
        public Text txtTime;
        [ALHeader("反击按钮")]
        public GameObject btnFight;
        [ALHeader("已反击时需要显示的GO列表")]
        public List<GameObject> goFightBackShowList;
        [ALHeader("已反击时需要隐藏的GO列表")]
        public List<GameObject> goFightBackHideList;
    }
}