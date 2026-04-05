using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗对手信息附加窗口
    /// </summary>
    public class GGUIMonoArenaBattleSubOpponentInfo : _AALBasicUIWndMono
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("影响力")]
        public Text txtInfluence;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("伙伴数量")]
        public Text txtHeroCount;
        [ALHeader("形象显示")]
        public GGUIMonoCommonShowCase monoPlayerShowcase;
    }
}