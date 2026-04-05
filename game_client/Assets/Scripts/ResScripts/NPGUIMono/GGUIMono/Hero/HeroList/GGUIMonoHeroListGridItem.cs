using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 红点配置
    /// </summary>
    [System.Serializable]
    public class HeroCardRedTipParam
    {
        [ALHeader("注释")]
        public string annotation;
        [ALHeader("红点id")]
        public long redId;
        [ALHeader("条件满足显示的列表")]
        public List<GameObject> goShowList;
    }

    /// <summary>
    /// 伙伴列表item
    /// </summary>
    public class GGUIMonoHeroListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("伙伴基础信息item")]
        public GGUIMonoHeroCommonCardItem monoCardItem;
        [ALHeader("红点列表，红点互斥且以第一个为准")]
        public List<HeroCardRedTipParam> redTipList;
    }
}