using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 我的宴会功能入口点的Mono
    /// </summary>
    public class GTDHomeEntryPointMono_SelfDinner : _AGTDHomeEntryPointMono_Base
    {
        [ALHeader("动画animation")]
        public Animation pointAnimation;
        [ALHeader("当前玩家有举办宴会的时候的动画名")]
        public string hasSelfDinnerAniName;
        [ALHeader("常态的动画名")]
        public string normalAniName;
    }
}