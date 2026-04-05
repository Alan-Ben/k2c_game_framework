using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase加载的资源管理器
    /// </summary>
    public class ShowCaseResMgr
    {
        private static ShowCaseResMgr _g_instance = new ShowCaseResMgr();
        public static ShowCaseResMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ShowCaseResMgr();
                return _g_instance;
            }
        }
        
        //舞台的cache管理器
        private ShowcaseTemplate_CacheMgr _m_showcaseTemplateCacheMgr = new ShowcaseTemplate_CacheMgr("showcaseTemplate");
        //所有加载单位的cache管理器
        private ShowcaseUnit_CacheMgr _m_showcaseUnitCacheMgr = new ShowcaseUnit_CacheMgr("showcaseUnit");

        //销毁
        public void discardAll()
        {
            //销毁舞台缓存池管理器
            if(null != _m_showcaseTemplateCacheMgr)
                _m_showcaseTemplateCacheMgr.discardAll();
            
            //销毁单位缓存池管理器
            if(null != _m_showcaseUnitCacheMgr)
                _m_showcaseUnitCacheMgr.discardAll();
        }

        /// <summary>
        /// 释放空闲内存
        /// </summary>
        public void discardAllUnUseCacheItems()
        {
            //释放空闲内存舞台缓存池管理器
            if(null != _m_showcaseTemplateCacheMgr)
                _m_showcaseTemplateCacheMgr.discardAllUnUseCacheItem();
            
            //释放空闲内存单位缓存池管理器
            if(null != _m_showcaseUnitCacheMgr)
                _m_showcaseUnitCacheMgr.discardAllUnUseCacheItem();
        }
        
        
          /// <summary>
        /// pop一个舞台
        /// </summary>
        public void popTemplate(NPGShowcaseIndex _showcaseIndex, Action<BasicResIndexInfo, NPShowcaseTemplateMono> _onLoaded)
        {
            if (null == _m_showcaseTemplateCacheMgr)
            {
                if (null != _onLoaded)
                    _onLoaded(_showcaseIndex, null);
                return;
            }
            
            _m_showcaseTemplateCacheMgr.popItem(_showcaseIndex,
                delegate(BasicResIndexInfo _info, NPShowcaseTemplateMono _mono)
                {
                    if (null != _onLoaded)
                        _onLoaded(_info, _mono);
                });
        }

        /// <summary>
        /// pushBack一个舞台
        /// </summary>
        public void pushBackTemplate(BasicResIndexInfo _index, NPShowcaseTemplateMono _mono)
        {
            if(null == _m_showcaseTemplateCacheMgr)
                return;
            
            _m_showcaseTemplateCacheMgr.pushBackItem(_index, _mono);
        }
        
        /// <summary>
        /// pop一个单位
        /// </summary>
        public void popUnit(BasicResIndexInfo _showcaseIndex, Action<BasicResIndexInfo, GameObject> _onLoaded)
        {
            if (null == _m_showcaseUnitCacheMgr)
            {
                if (null != _onLoaded)
                    _onLoaded(_showcaseIndex, null);
                return;
            }
            
            _m_showcaseUnitCacheMgr.popItem(_showcaseIndex,
                delegate(BasicResIndexInfo _info, GameObject _mono)
                {
                    if (null != _onLoaded)
                        _onLoaded(_info, _mono);
                });
        }

        /// <summary>
        /// pushBack一个单位
        /// </summary>
        public void pushBackUnit(BasicResIndexInfo _index, GameObject _mono)
        {
            if(null == _m_showcaseUnitCacheMgr)
                return;
            
            _m_showcaseUnitCacheMgr.pushBackItem(_index, _mono);
        }
    }
}