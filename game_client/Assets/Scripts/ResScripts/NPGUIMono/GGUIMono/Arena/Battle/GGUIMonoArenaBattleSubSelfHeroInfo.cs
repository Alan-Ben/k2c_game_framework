using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗自己伙伴信息附加窗口
    /// </summary>
    public class GGUIMonoArenaBattleSubSelfHeroInfo : _AALBasicUIWndMono
    {
        [ALHeader("实力详情按钮")]
        public GameObject btnPowerDetail;
        [ALHeader("玩家名字")]
        public Text txtPlayerName;
        [ALHeader("伙伴形象")]
        public RawImage imgHero;
        [ALHeader("伙伴实力")]
        public Text txtHeroPower;
        [ALHeader("临时加成百分比")]
        public Text txtBuffAddPer;
        [ALHeader("血量条")]
        public Slider sldBlood;
        [ALHeader("血量文本")]
        public Text txtHP;
        [ALHeader("伙伴实力详情tip的X偏移")]
        public float powerDetailIntervalX;
        [ALHeader("伙伴实力详情tip的Y偏移")]
        public float powerDetailIntervalY;
    }
}