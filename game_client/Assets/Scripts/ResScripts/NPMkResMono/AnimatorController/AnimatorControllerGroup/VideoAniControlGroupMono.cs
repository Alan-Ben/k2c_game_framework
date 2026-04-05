using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    [Serializable]
    public class VideoAniControlGroupRegItemInfo : _ATAniControlGroupRegItemInfo<AnimatorControlRegInfo>
    {
    }

    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    public class VideoAniControlGroupMono : _ATAniControlGroupMono<AnimatorControlGroupRegItemInfo, AnimatorControlRegInfo>
    {
    }
}
