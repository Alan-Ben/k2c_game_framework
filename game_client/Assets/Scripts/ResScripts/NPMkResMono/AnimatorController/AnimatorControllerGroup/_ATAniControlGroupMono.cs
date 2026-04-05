using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    [Serializable]
    public class _ATAniControlGroupRegItemInfo<T> where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
    {
        [ALHeader("动画单元对象")]
        public _ATAniControlMono<T> controlMono;
        [ALHeader("过滤的单体字符串，在本字符串有效的前提下，只有匹配这个字符串的control内部对象才会被注册控制")]
        public string siftName;

    }

    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    public class _ATAniControlGroupMono<GI_T, T> : MonoBehaviour where GI_T : _ATAniControlGroupRegItemInfo<T> where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
    {
        [ALHeader("动画对象注册集合名称")]
        public string controlGroupName;
        [ALHeader("绑定动画控制对象")]
        public List<GI_T> regItemList;


        private void Awake()
        {
            AnimatorControllerGroupMgr.instance.regMono(this);
        }

        private void OnDestroy()
        {
            AnimatorControllerGroupMgr.instance.unregMono(this);
        }
    }
}
