using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟申请列表item
    /// </summary>
    public class GGUIMonoGuildOthersApplyListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("拒绝按钮")]
        public GameObject btnRefuse;
        [ALHeader("同意按钮")]
        public GameObject btnAgree;
    }
}
