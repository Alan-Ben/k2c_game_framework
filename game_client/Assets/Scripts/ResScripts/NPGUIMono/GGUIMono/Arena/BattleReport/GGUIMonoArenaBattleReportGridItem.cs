using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战报列表item
    /// </summary>
    public class GGUIMonoArenaBattleReportGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("击败伙伴数量")]
        public Text txtDefeatHeroCount;
        [ALHeader("影响力变更")]
        public Text txtInfluenceChg;
        [ALHeader("时间")]
        public Text txtTime;
    }
}