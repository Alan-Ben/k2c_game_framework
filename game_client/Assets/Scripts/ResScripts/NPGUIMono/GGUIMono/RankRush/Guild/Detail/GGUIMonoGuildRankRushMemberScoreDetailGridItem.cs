using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜联盟成员积分详情成员列表item
    /// </summary>
    public class GGUIMonoGuildRankRushMemberScoreDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("排名")]
        public Text txtRank;
        [ALHeader("玩家名称")]
        public Text txtName;
        [ALHeader("积分")]
        public Text txtScore;
    }
}
