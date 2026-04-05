using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    /// 宴会宾客历史item
    /// </summary>
    public class GGUIMonoDinnerGuestItem : _TALUGUIMonoGridItem
    {
        [ALHeader("序号")]
        public Text txtIndex;
        [ALHeader("参宴会玩家名称")]
        public Text txtJoinName;
        [ALHeader("参与礼物")]
        public Text txtJoinGift;
        [ALHeader("参宴会玩家积分")]
        public Text txtJoinCoin;
        [ALHeader("参宴会玩家积分")]
        public Text txtJoinScore;
        [ALHeader("座位上的玩家信息")]
        public NPGGUIMonoPlayerIcon playerIcon;
        [ALHeader("骑士信息")] 
        public GGUIMonoHeroIconItem monoHeroInfo;
        [ALHeader("不同状态显示配置")]
        public List<NPCommonEnumStatInfo<EDinnerJoinerType>> statInfos;

    }
}
