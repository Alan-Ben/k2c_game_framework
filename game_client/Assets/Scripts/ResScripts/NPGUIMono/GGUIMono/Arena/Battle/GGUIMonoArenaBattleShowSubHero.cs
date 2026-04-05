using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 血量延时变化参数
    /// </summary>
    [System.Serializable]
    public class GGUIArenaBattleBloodDelayChangeParam
    {
        [ALHeader("开始变化延时时间秒（相对于窗口打开的时间）")]
        public float delayTimeSec;
        [ALInfo("最后一条这个百分比配置会根据前几条配置的百分比计算，保证总的是100%")]
        [ALHeader("变化的血量占总变化血量的百分比")]
        public float changePercentage;
    }

    /// <summary>
    /// 竞技场战斗展示界面伙伴信息附加窗口
    /// </summary>
    public class GGUIMonoArenaBattleShowSubHero : _AALBasicUIWndMono
    {
        [ALHeader("伙伴头像")]
        public RawImage imgHeroIcon;
        [ALHeader("伙伴头像品质背景")]
        public Image imgHeroIconBg;
        [ALHeader("伙伴形象")]
        public RawImage imgHero;
        [ALHeader("伙伴名字")]
        public Text txtHeroName;
        [ALHeader("伙伴等级")]
        public Text txtHeroLevel;
        [ALHeader("血量进度条")]
        public GGUIMonoCommonBlood monoHP;
        [ALHeader("血量延时变化参数列表")]
        public List<GGUIArenaBattleBloodDelayChangeParam> bloodDelayChangeParmList;
    }
}