using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤奖励展示
    /// </summary>
    public class GGUIPrefabMonoPlayerSkinRewardShow : _AALBasicUIWndMono
    {
        [ALHeader("皮肤形象")]
        public GGUIMonoCommonShowCase monoShowCase;

        [ALHeader("皮肤名")]
        public TextEx txtSkinName;

        [ALHeader("详情信息按钮")]
        public GameObject btnDetail;
    }
}