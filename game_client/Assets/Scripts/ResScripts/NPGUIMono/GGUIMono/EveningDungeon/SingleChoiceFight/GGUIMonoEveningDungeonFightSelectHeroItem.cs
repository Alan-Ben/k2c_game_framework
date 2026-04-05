using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单选大臣出战状态
    /// </summary>
    public enum EEveningDungeonHeroFightState
    {
        NONE,
        [InspectorName("未出战过")]
        NEVER_FIGHT,
        [InspectorName("已出战过, 且还有可出战次数")]
        ALREADY_FIGHT_HAS_FIGHT_COUNT,
        [InspectorName("已出战过, 且没有可出战次数")]
        ALREADY_FIGHT_NO_FIGHT_COUNT,
    }
    
    /// <summary>
    /// 晚间活动出战大臣item
    /// </summary>
    public class GGUIMonoEveningDungeonFightSelectHeroItem : _TALUGUIMonoGridItem
    {
        [ALHeader("伙伴基础信息item")]
        public GGUIMonoHeroCommonCardItem monoCardItem;
        [ALHeader("伙伴头像信息")] 
        public GGUIMonoHeroIconItem monoHeroInfo;
        [ALHeader("战斗攻击力")]
        public TextEx txtFightATK;
        [ALHeader("战斗攻击力描述key(一个参数, 大臣攻击力)")]
        public string txtFightATKKey;
        
        [ALHeader("出战次数")]
        public TextEx txtFightCount;
        [ALHeader("有出战次数时文本的颜色")]
        public Color hasFightCountColor;
        [ALHeader("无出战次数时文本的颜色")]
        public Color noFightCountColor;
        
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
        
        [ALHeader("不同战斗状态的显示")]
        public List<NPCommonEnumStatInfo<EEveningDungeonHeroFightState>> fightStateShowList;
    }
}