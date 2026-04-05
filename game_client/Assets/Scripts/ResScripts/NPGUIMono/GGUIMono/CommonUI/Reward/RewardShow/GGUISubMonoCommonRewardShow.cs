using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用奖励展示子窗口
    /// </summary>
    public class GGUISubMonoCommonRewardShow : _AALBasicUIWndMono
    {
        [ALHeader("奖励展示父节点")]
        public Transform rewardShowParent;
        
        [ALHeader("玩家皮肤展示资源路径(资源上配置脚本GGUIPrefabMonoPlayerSkinRewardShow)")]
        public NPCommonAssetPathInfo playerSkinShowAssetPath;
    }
}