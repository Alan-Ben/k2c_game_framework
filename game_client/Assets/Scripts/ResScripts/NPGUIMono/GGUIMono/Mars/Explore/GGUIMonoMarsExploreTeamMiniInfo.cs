using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsExploreTeamMiniInfo : _AALBasicUIWndMono
    {
        [ALHeader("展示按钮和展示动画名")]
        public GameObject btnShow;
        public string showAnimName;
        [ALHeader("隐藏按钮和隐藏动画名")]
        public GameObject btnHide;
        public string hideAnimName;
        [ALHeader("队伍信息容器")]
        public GGUIMonoMarsExploreTeamMiniInfoContainer monoTeamContainer;
    }
}