using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE.FollowItem
{
    /// <summary>
    /// Item做场景跟随的父窗口通用实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATNPGGUIWndCommonFollowRootWnd<T> : _ATALGGUIWndCommonFollowRootWnd<T> where T : ALGGUIMonoCommonFollowRootWnd
    {
        /// <summary>
        /// 返回本窗口用于缓存跟随UI的缓存管理器对象
        /// </summary>
        protected override ALGGUIWndCommonFollowItemCacheMgr _followItemCacheMgr { get { return NPGGUIWndCommonFollowItemCacheMgr.instance; } }
    }
}