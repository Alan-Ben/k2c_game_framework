using System.Collections.Generic;
using UnityEngine;

namespace GOE.MiniGame
{
    /// <summary>
    /// 拖箱子游戏玩家Q版形象脚本
    /// </summary>
    public class GTDMonoDragBoxGamePlayerCuteActor : MonoBehaviour
    {
        [ALHeader("玩家Q版形象父节点")]
        public Transform playerCuteActorParent;
        [ALHeader("玩家Q版形象动画控制器注册名")]
        public string playerAnimatorControlRegName;

        [ALHeader("玩家跟随物体GoIndex")]
        public NPGGoIndex playerFollowerGo;
        [ALHeader("玩家跟随物体父节点")]
        public Transform playerFollowerGoParent;
        [ALHeader("玩家跟随物体动画控制器注册名")]
        public string playerFollowerAnimatorControlRegName;
        
        [ALHeader("跑步时脚步音效id列表")]
        public List<long> runFootstepsAudioIdList;
    }
}