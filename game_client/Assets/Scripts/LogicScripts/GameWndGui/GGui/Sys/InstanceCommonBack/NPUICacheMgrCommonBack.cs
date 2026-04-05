using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using JetBrains.Annotations;


namespace GOE
{
    /// <summary>
    /// 通用返回对象池mgr
    /// </summary>
    public class NPUICacheMgrCommonBack 
    {
        private static NPUICacheMgrCommonBack _g_instance = new NPUICacheMgrCommonBack();
        [NotNull]public static NPUICacheMgrCommonBack instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPUICacheMgrCommonBack();
                return _g_instance;
            }
        }

        //是否初始化
        private bool _m_isInit;
        //cache字典
        [NotNull]private Dictionary<long, NPUICacheCommonBack> _m_cacheDict = new Dictionary<long, NPUICacheCommonBack>();

        //初始化
        public void initCache()
        {
            if(_m_isInit)
                return;
            _m_isInit = true;
            
            _m_cacheDict.Add(UIResPathConst.WIN_COMMON_BACK, new NPUICacheCommonBack(UIResPathConst.WIN_COMMON_BACK));
            _m_cacheDict.Add(UIResPathConst.WIN_COMMON_BACK_SCENE_BG, new NPUICacheCommonBack(UIResPathConst.WIN_COMMON_BACK_SCENE_BG));
        }

        //销毁
        public void discard()
        {
            foreach (NPUICacheCommonBack npuiCacheCommonBack in _m_cacheDict.Values)
            {
                if (npuiCacheCommonBack != null) 
                    npuiCacheCommonBack.discard();
            }
            _m_cacheDict.Clear();
            _m_isInit = false;
        }

        //取出一个对象名称显示对象
        public NPGGUIWndInstanceCommonBack popItem(long _uiResPath)
        {
            if (!_m_isInit)
            {
                Debug.LogError($"NPUICacheMgrCommonBack 没有初始化时候就开始调用");
                return null;
            }

            if (_m_cacheDict.TryGetValue(_uiResPath, out NPUICacheCommonBack commonBackCache) && null != commonBackCache)
            {
                return commonBackCache.popItem();
            }
            return null;
        }
        
        //将名称操作对象放回缓存队列
        public void pushBackCacheItem(long _uiResPath, NPGGUIWndInstanceCommonBack _bkWnd)
        {
            if (!_m_isInit)
            {
                Debug.LogError($"NPUICacheMgrCommonBack 没有初始化时候就开始调用");
            }

            if (_m_cacheDict.TryGetValue(_uiResPath, out NPUICacheCommonBack commonBackCache) && null != commonBackCache)
            {
                commonBackCache.pushBackCacheItem(_bkWnd);
            }
        }
    }
}
