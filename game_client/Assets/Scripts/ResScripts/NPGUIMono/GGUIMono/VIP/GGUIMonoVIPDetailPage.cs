using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP详情加载页面
    /// </summary>
    public class GGUIMonoVIPDetailPage : _AALBasicUIWndMono
    {
        [ALHeader("形象列表")]
        public List<GGUIMonoSubVIPDetailActor> monoActorList;
        [ALHeader("领取奖励条件提示")]
        public Text txtGetRewardTip;
    }
}