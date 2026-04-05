using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会列表功能入口点的Mono
    /// </summary>
    public class GTDHomeEntryPointMono_DinnerList : _AGTDHomeEntryPointMono_Base
    {
        [ALHeader("动画相关表现接口，有需要加对应mono托")]
        [ALInfo("ShowCaseCommonResObjAniEffect_Animation 动画资源是Animation用这个" +
                "\n ShowCaseCommonResObjAniEffect_Animator 动画资源是Animator用这个" +
                "\n ShowCaseCommonResObjAniEffect_Spine 动画资源是Spine用这个")]
        public _AShowCaseCommonResObjAniEffect aniShowInterface;
        [ALHeader("当前有其它玩家举办宴会的时候的动画名")]
        public string hasOtherDinnerAniName;
        [ALHeader("当前没有其它玩家举办宴会的时候的动画名")]
        public string normalAniName;
        [ALHeader("初始的用于判断是否需要播放的动画名，重复播放同一个动画会跳过")]
        public string defaultCheckName = "Idle1-2";
    }
}