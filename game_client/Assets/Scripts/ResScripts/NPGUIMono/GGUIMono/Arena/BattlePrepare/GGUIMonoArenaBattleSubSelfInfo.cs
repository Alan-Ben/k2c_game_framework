using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗自己信息附加窗口
    /// </summary>
    public class GGUIMonoArenaBattleSubSelfInfo : _AALBasicUIWndMono
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("影响力")]
        public Text txtInfluence;
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("形象显示")]
        public GGUIMonoCommonShowCase monoPlayerShowcase;
    }
}