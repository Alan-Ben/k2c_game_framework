
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 聊天气泡的缓存管理器
    /// </summary>
    public class NPGGUICacheMgrChatMsgItemTextBubble
    {
        // bubbleId对应的气泡cache
        [NotNull] private readonly Dictionary<long, NPGGUICacheChatMsgItemTextBubble> _m_bubbleId2CacheL = new Dictionary<long, NPGGUICacheChatMsgItemTextBubble>();
        [NotNull] private readonly Dictionary<long, NPGGUICacheChatMsgItemTextBubble> _m_bubbleId2CacheR = new Dictionary<long, NPGGUICacheChatMsgItemTextBubble>();
        // 在缓存池中的对象都会在这个父对象下
        [NotNull] private readonly GameObject _m_cacheRoot;

        public NPGGUICacheMgrChatMsgItemTextBubble()
        {
            // 创建一个缓存池的父对象
            _m_cacheRoot = new GameObject
            {
                name = "chatMsgItemTextBubbleCache"
            };
            _m_cacheRoot.transform.position = Vector3.up * -10000;

            // 设置父对象不被销毁
            Object.DontDestroyOnLoad(_m_cacheRoot);
        }

        public void getTemplate(long _bubbleId, bool _isMyMsg, Action<NPGGUIMonoChatMsgItemTextBubble> _action)
        {
            // 如果回调为null就直接返回
            if (_action == null)
                return;

            // 临时的cache
            NPGGUICacheChatMsgItemTextBubble cache = null;

            // Cache加载完后的处理
            void onCacheLoaded()
            {
                // 如果加载完了模板为null，说明加载失败了，直接调用回调，返回空
                if (cache == null || cache.template == null)
                {
                    ALLog.Error($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache加载模板失败了，请检查资源是否存在");
                    _action.Invoke(null);
                    return;
                }

                // 一切都结束了之后调用回调
                _action.Invoke(cache.template);
            }

            Dictionary<long, NPGGUICacheChatMsgItemTextBubble> cacheDic = _isMyMsg ? _m_bubbleId2CacheR : _m_bubbleId2CacheL;
            
            // 尝试获取对应的cache
            if (cacheDic.TryGetValue(_bubbleId, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 尝试获取配置
                NPPlayerBubbleRefObj refObj = GRefdataCoreMgr.instance.getPlayerBubbleRefObj(_bubbleId);
                if (refObj == null)
                {
                    ALLog.Error($"没有找到id为{_bubbleId}以及默认气泡 id{GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id}的气泡框配置");
                    _action.Invoke(null);
                    return;
                }

                // 如果没有获取cache，就构建一个新的
                cache = new NPGGUICacheChatMsgItemTextBubble(refObj.asset_path, _isMyMsg ? refObj.obj_name_r : refObj.obj_name_l, _m_cacheRoot);
                // 添加到字典中
                cacheDic[_bubbleId] = cache;

                // 开始加载，并注册加载完成后的回调
                cache.load();
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
        }

        public void popItem(long _bubbleId, Transform _parent, bool _isMyMsg, Action<NPGGUIWndChatMsgItemTextBubble> _onLoaded)
        {
            // 如果回调为null，或者没有指定对应的msgInfo就直接返回
            if (_onLoaded == null)
                return;

            // 临时的cache
            NPGGUICacheChatMsgItemTextBubble cache = null;
            // Cache加载完后的处理
            void onCacheLoaded()
            {
                if (cache == null)
                {
                    ALLog.Error($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache为null");
                    _onLoaded.Invoke(null);
                    return;
                }

                NPGGUIWndChatMsgItemTextBubble wnd = cache.popItem();
                if (wnd == null || wnd.wnd == null)
                {
                    ALLog.Error($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache popItem出现问题了，pop出的item为null!");
                    _onLoaded.Invoke(null);
                    return;
                }
                
                wnd.wnd.transform.SetParent(_parent, false);
                wnd.wnd.transform.localPosition = Vector3.zero;

                // 一切都结束了之后调用回调
                _onLoaded(wnd);
            }
            
            Dictionary<long, NPGGUICacheChatMsgItemTextBubble> cacheDic = _isMyMsg ? _m_bubbleId2CacheR : _m_bubbleId2CacheL;

            // 尝试获取对应的cache
            if (cacheDic.TryGetValue(_bubbleId, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 尝试获取配置
                NPPlayerBubbleRefObj refObj = GRefdataCoreMgr.instance.getPlayerBubbleRefObj(_bubbleId);
                if (refObj == null)
                {
                    ALLog.Error($"没有找到id为{_bubbleId}以及默认气泡 id{GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id}的气泡框配置");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 如果没有获取cache，就构建一个新的
                cache = new NPGGUICacheChatMsgItemTextBubble(refObj.asset_path, _isMyMsg ? refObj.obj_name_r : refObj.obj_name_l, _m_cacheRoot);
                // 添加到字典中
                cacheDic[_bubbleId] = cache;

                // 开始加载，并注册加载完成后的回调
                cache.load();
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
        }
        public void pushBackItem(long _bubbleId, bool _isMyMsg, NPGGUIWndChatMsgItemTextBubble _item)
        {
            // 如果item为null或者索引msgInfo为null，都认为无效
            if (_item == null)
                return;
            
            Dictionary<long, NPGGUICacheChatMsgItemTextBubble> cacheDic = _isMyMsg ? _m_bubbleId2CacheR : _m_bubbleId2CacheL;
            // 尝试找到对应的缓存池
            if (cacheDic.TryGetValue(_bubbleId, out NPGGUICacheChatMsgItemTextBubble cache) && cache != null)
            {
                // 放回对应的缓存池
                cache.pushBackCacheItem(_item);
            }
            else
            {
                // 如果没有找到缓存池，弹出错误提示
                ALLog.Error($"出现了未知的异常，Cache回收资源时，传入了不是这里pop出来的GameObject，正常不直接访问internal的成员不应该触发这段代码");
                // 并释放对应的GameObject，你都要回收了，我回收不了，就给你扬咯
                ALUnityCommon.releaseGameObj(_item.wnd);
            }
        }
        /// <summary>
        /// 清空所有内容，包括缓存池中的所有item，所有缓存池，所有设置
        /// </summary>
        public void discard()
        {
            // 清空缓存池中所有的item
            foreach (NPGGUICacheChatMsgItemTextBubble item in _m_bubbleId2CacheL.Values)
            {
                item?.discard();
            }

            foreach (NPGGUICacheChatMsgItemTextBubble item in _m_bubbleId2CacheR.Values)
            {
                item?.discard();
            }

            // 清空缓存池
            _m_bubbleId2CacheR.Clear();
            _m_bubbleId2CacheL.Clear();
        }
    }
}