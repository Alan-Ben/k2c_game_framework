using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    public class ConsortChatMsgCacheMgr
    {
        private static ConsortChatMsgCacheMgr _g_instance = new ConsortChatMsgCacheMgr();
        [NotNull] 
        public static ConsortChatMsgCacheMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new ConsortChatMsgCacheMgr();
                return _g_instance;
            }
        }

        [NotNull] private readonly Dictionary<long, ConsortChatCache> _m_uiResId2Cache = new Dictionary<long, ConsortChatCache>();
        // 在缓存池中的对象都会在这个父对象下
        [NotNull] private readonly GameObject _m_cacheRoot;

        #region GOCache

        private Go_CacheMgr _m_goCacheMgr ;

        public Go_CacheMgr goCacheMgr
        {
            get
            {
                if(_m_goCacheMgr == null)
                    _m_goCacheMgr = new Go_CacheMgr("ConsortImageGroup", 1, 3);
                return _m_goCacheMgr;
            }
        }

        #endregion
        
        public ConsortChatMsgCacheMgr()
        {
            // 创建一个缓存池的父对象
            _m_cacheRoot = new GameObject
            {
                name = "ConsortChatMsgCache"
            };
            _m_cacheRoot.transform.position = Vector3.up * -10000;

            // 设置父对象不被销毁
            Object.DontDestroyOnLoad(_m_cacheRoot);
        }
        // /// <summary>
        // /// 创建一个wnd类实例
        // /// </summary>
        // /// <param name="_parent">构建这个wnd类所需的父对象</param>
        // /// <returns>返回对于的wnd类实例</returns>
        // public GGUIWndConsortChatMsgListItem createWnd(long _uiPathId, Transform _parent)
        // {
        //     return new GGUIWndConsortChatMsgListItem();
        // }
        
        /// <summary>
        /// 获取一个对应的mono
        /// </summary>
        /// <typeparam name="T_MONO">指定mono的类型</typeparam>
        /// <param name="_msgInfo">指定的msgInfo</param>
        /// <param name="_parent">父对象</param>
        /// <param name="_onLoaded">获取完后的回调</param>
        public void popItem<T_MONO>(long _uiPathId, Transform _parent, Action<T_MONO> _onLoaded)
            where T_MONO : _AALBasicUIWndMono
        {
            NPCommonAssetPathInfo pathInfo = UIResPathAssistant.getAssetInfo(_uiPathId);
            // 如果回调为null，或者没有指定对应的msgInfo就直接返回
            if (_onLoaded == null || pathInfo == null)
                return;

            // 临时的cache
            ConsortChatCache cache = null;
            // Cache加载完后的处理
            void onCacheLoaded()
            {
                // 如果没有Cache直接返回，虽然现有逻辑（2022.1.5）这里可以保证不会没有cache
                if (cache == null)
                {
                    Debug.LogError_EditorOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache为null");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 从对应的缓存池中获取GameObject
                GameObject go = cache.popItem();
                if (go == null)
                {
                    Debug.LogError_EditorOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache popItem出现问题了，pop出的item为null!");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 尝试从获取的GameObject上获取对应的mono
                T_MONO mono = go.GetComponent<T_MONO>();
                if (mono == null)
                {
                    Debug.LogError_EditorOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，上面没有挂载mono: {typeof(T_MONO)}!");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 设置到父对象下
                mono.transform.SetParent(_parent, false);
                mono.transform.localPosition = Vector3.zero;
                // 一切都结束了之后调用回调，传入获取到的mono
                _onLoaded(mono);
            }

            // 尝试获取对应的cache
            if (_m_uiResId2Cache.TryGetValue(_uiPathId, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 使用设置构建cache
                cache = new ConsortChatCache(pathInfo, _m_cacheRoot, 1, 10);
                // 添加到cache的字典中
                _m_uiResId2Cache[_uiPathId] = cache;

                // 开始加载，并注册加载完成后的回调
                cache.load(); 
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
        }

       /// <summary>
        /// 把item放回缓存池
        /// </summary>
        /// <param name="_msgInfo">缓存池对应的索引msgInfo</param>
        /// <param name="_item">需要被放回的item</param>
        public void pushBackItem(long _uiPathId, GameObject _item)
        {
            // 如果item为null或者都认为无效
            if (_item == null)
                return;
            
            // 尝试找到对应的缓存池
            if (_m_uiResId2Cache.TryGetValue(_uiPathId, out ConsortChatCache cache) && cache != null)
            {
                // 转移到池的root
                _item.transform.SetParent(_m_cacheRoot.transform, false);
                _item.transform.localPosition = Vector3.zero;
                // 放回对应的缓存池
                cache.pushBackCacheItem(_item);
            }
            else
            {
                // 如果没有找到缓存池，弹出错误提示
                Debug.LogError_EditorOnly($"出现了未知的异常，Cache回收资源时，传入了不是这里pop出来的GameObject，正常不直接访问internal的成员不应该触发这段代码");
                // 并释放对应的GameObject，你都要回收了，我回收不了，就给你扬咯
                ALUnityCommon.releaseGameObj(_item);
            }
        }
       
        /// <summary>
        /// 根据数据类型获取模板对象
        /// </summary>
        /// <param name="_msgInfo">指定需要获取哪条消息的模板对象</param>
        /// <param name="_action">获取到模板对象后的回调</param>
        /// <typeparam name="T_MONO">指定模板对象的类型</typeparam>
        public void getTemplate<T>(long _uiPathId, Action<T> _action) where T : _AALBasicUIWndMono
        {
            NPCommonAssetPathInfo pathInfo = UIResPathAssistant.getAssetInfo(_uiPathId);
            // 如果回调为null，或者没有指定对应的pathInfo就直接返回
            if (_action == null || pathInfo == null)
            {
                _action?.Invoke(null);
                return;
            }

           
            // 临时的cache
            ConsortChatCache cache = null;
            // Cache加载完后的处理
            void onCacheLoaded()
            {
                // 如果加载完了模板为null，说明加载失败了，直接调用回调，返回空
                if (cache == null || cache.template == null)
                {
                    Debug.LogError_EditorOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache加载模板失败了，请检查资源是否存在");
                    _action.Invoke(null);
                    return;
                }

                // 尝试获取模板对应的mono
                T mono = cache.template.GetComponent<T>();
                if (mono == null)
                {
                    Debug.LogError_EditorOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，上面没有挂载mono:GGUIMonoConsortChatMsgListItem!");
                    _action.Invoke(null);
                    return;
                }
                
                // 一切都结束了之后调用回调，传入构建好的wnd
                _action.Invoke(mono);
            }

            // 尝试获取对应的cache
            if (_m_uiResId2Cache.TryGetValue(_uiPathId, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 使用设置构建cache
                cache = new ConsortChatCache(pathInfo, _m_cacheRoot, 1, 10);
                // 添加到cache的字典中
                _m_uiResId2Cache[_uiPathId] = cache;

                // 开始加载，并注册加载完成后的回调
                cache.load();
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
        
        }

        public void discard()
        {
            _m_goCacheMgr?.discardAll();
        }
    }
}