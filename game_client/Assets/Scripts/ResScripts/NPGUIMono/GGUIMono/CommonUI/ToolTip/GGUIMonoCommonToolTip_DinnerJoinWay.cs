using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会消耗 积分获得详情跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_DinnerJoinWay : NPGGUIMonoCommonToolTip
    {
        [ALHeader("基础人气")]
        public TextEx txtBasic;
        [ALHeader("评级加成")] 
        public TextEx txtRankBonus;
        [ALHeader("伙伴觉醒")] 
        public TextEx txtFellowTalent;
        [ALHeader("家人关系")] 
        public TextEx txtFamilyRelationShip;

    }
}