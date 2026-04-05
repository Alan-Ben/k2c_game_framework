using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 机关分享-好友页签
    /// </summary>
    public class NPGGUIMonoCommonShareFriendPage : _AALBasicUIWndMono
    {
        [ALHeader("分享列表")]
        public NPGGUIMonoCommonShareContainer monoItemContainer;
        [ALHeader("一键分享按钮")]
        public GameObject btnOneKeyShare;
    }
}
