using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴列表item
    /// </summary>
    public class GGUIMonoArenaBattleSelectHeroGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("伙伴基础信息item")]
        public GGUIMonoHeroCommonCardItem monoCardItem;
        [ALHeader("竞技场实力")]
        public Text txtAttackPower;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        [ALHeader("已经战斗过时需要显示的GO列表")]
        public List<GameObject> goAlreadyFightShowList;
        [ALHeader("已经战斗过时需要隐藏的GO列表")]
        public List<GameObject> goAlreadyFightHideList;
    }
}