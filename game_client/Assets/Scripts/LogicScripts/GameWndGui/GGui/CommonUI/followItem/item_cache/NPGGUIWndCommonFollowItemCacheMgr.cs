using UnityEngine;
using ALPackage;
using System;

using System.Collections.Generic;

namespace GOE.FollowItem
{
    /// <summary>
    /// 全局中用于跟随场景坐标进行跳转的Item的缓存管理对象
    /// 此对象作为单例，便于全局统一管理
    /// </summary>
    public class NPGGUIWndCommonFollowItemCacheMgr : ALGGUIWndCommonFollowItemCacheMgr
    {
        private static NPGGUIWndCommonFollowItemCacheMgr _g_instance = new NPGGUIWndCommonFollowItemCacheMgr();
        public static NPGGUIWndCommonFollowItemCacheMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndCommonFollowItemCacheMgr();

                return _g_instance;
            }
        }

        protected NPGGUIWndCommonFollowItemCacheMgr()
            : base(GameResCore.instance)
        {

        }
    }
}

