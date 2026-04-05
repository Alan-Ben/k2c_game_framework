using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 邀约结果item
    /// </summary>
    public class GGUIMonoConsortInviteRewardItem : _AALBasicUIWndMono
    {
        [ALHeader("妃子头像Mono")]
        public GGUIMonoConsortIconItem consortHeadMono;

        [ALHeader("妃子卡牌Mono")]
        public GGUIMonoConsortCardItem consortCardMono;

        [ALHeader("增加加护点数")]
        public TextEx txtAddCharmPoint;
        [ALHeader("增加加护点数key")]
        public string txtAddCharmPointKey;

        [ALHeader("邀约触发故事描述")]
        public TextEx txtInviteStoryDesc;
        
        [ALHeader("有CG奖励时显示")]
        public List<GameObject> hasCgRewardShow;
        [ALHeader("cg奖励文本")]
        public TextEx txtCgReward;
        [ALHeader("cg奖励文本key")]
        public string txtCgRewardKey;
    }
}