using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠详情 跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_GuildDonateDetailInfo : NPGGUIMonoCommonToolTip
    {
        [ALHeader("今日获取的联盟经验")]
        public TextEx txtTodayGainGuildExp;
        [ALHeader("今日获取的联盟经验key(一个参数, 1.增加的经验值)")]
        public string txtTodayGainGuildExpKey;

        [ALHeader("每日获取联盟经验上限")]
        public TextEx txtDailyGainGuildExpLimit;
        [ALHeader("每日获取联盟经验上限key(一个参数, 1.上限值)")]
        public string txtDailyGainGuildExpLimitKey;
     
        [ALHeader("每日获取的联盟财富")]
        public TextEx txtDailyGainGuildWealth;
        [ALHeader("每日获取的联盟财富key(一个参数, 1.增加的财富值)")]
        public string txtDailyGainGuildWealthKey;
        
        [ALHeader("每日获取联盟财富上限")]
        public TextEx txtDailyGainGuildWealthLimit;
        [ALHeader("每日获取联盟财富上限key(一个参数, 1.上限值)")]
        public string txtDailyGainGuildWealthLimitKey;
    }
}