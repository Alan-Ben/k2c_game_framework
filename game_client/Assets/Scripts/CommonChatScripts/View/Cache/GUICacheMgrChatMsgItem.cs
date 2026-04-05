
using System;
using System.Collections.Generic;

using ALPackage;
using ChatPackage.Internal;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChatPackage
{
    /// <summary>
    /// <see cref="GUISubWndChatMsgList"/>所使用的缓存池
    /// </summary>
    /// <remarks>
    /// 这个缓存池以单例的形式运行，在理解了缓存池的运作方式之后，你也可以引入Internal包来使用缓存池，来实现你自己的消息列表
    /// <para>主要使用方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>registerCache</term>
    ///             <description>
    ///             注册一个缓存的设置内容，要使用这个类你需要先使用这个方法来设定对于不同msgType的处理方法，你也可以直接使用registerCache来设置，详情可以查看<see cref="Chat"/>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>unregisterCache</term>
    ///             <description>
    ///             取消一个cache的设置，如果你需要中途改变对msgType的处理方法，可以使用这个方法取消掉一个设置
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>getTemplate</term>
    ///             <description>
    ///             获取对应msgType的模板对象，这个模板对象是AB包中最原始的对象，你可以使用这个对象来获取item的高度
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>popItem</term>
    ///             <description>
    ///             从缓存池中取出一个对应msgType的带着对应mono的prefab，这里的泛型请务必要和设置的一样
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>pushBackItem</term>
    ///             <description>
    ///             把之前popItem取出的东西放回缓存池中，请务必和popItem取出时的msgType对应上
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>getCacheSetting</term>
    ///             <description>
    ///             获取registerCache注册进去的设置
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>discard</term>
    ///             <description>
    ///             清空缓存池中的所有内容，这个discard也会被Chat.stop调用，详情可以查看<see cref="Chat"/>
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class GUICacheMgrChatMsgItem
    {
        // 由msgType和isMyMsg组成的一个int的key，由这个key映射到设置
        [NotNull] private readonly Dictionary<int, GUICacheMgrChatMsgItemSetting> _m_msgType2Setting = new Dictionary<int, GUICacheMgrChatMsgItemSetting>();
        // 由msgType和isMyMsg组成的一个int的key，由这个key映射到缓存池
        [NotNull] private readonly Dictionary<int, ChatGameObjectCache> _m_msgType2Cache = new Dictionary<int, ChatGameObjectCache>();
        // 在缓存池中的对象都会在这个父对象下
        [NotNull] private readonly GameObject _m_cacheRoot;

        public GUICacheMgrChatMsgItem()
        {
            // 创建一个缓存池的父对象
            _m_cacheRoot = new GameObject
            {
                name = "chatMsgItemCache"
            };
            _m_cacheRoot.transform.position = Vector3.up * -10000;

            // 设置父对象不被销毁
            Object.DontDestroyOnLoad(_m_cacheRoot);
        }

        /// <summary>
        /// 注册对应msgType和isMyMsg对应的wnd的设置
        /// </summary>
        /// <typeparam name="T_WND">指定对应的wnd类型</typeparam>
        /// <typeparam name="T_MONO">指定对应的mono类型</typeparam>
        /// <typeparam name="T_DATA">指定对应的data类型</typeparam>
        /// <param name="_msgType">对应的msgType</param>
        /// <param name="_myMsg">对应的是否是自己的消息</param>
        /// <param name="_assetPath">指定对应的资源路径</param>
        /// <param name="_objName">指定对应的资源名</param>
        /// <param name="_resourceCore">指定对应的resourceCore</param>
        /// <param name="_minCacheCount">最小缓存数量</param>
        /// <param name="_maxCacheCount">最大缓存数量</param>
        public void registerCache<T_WND, T_MONO, T_DATA>(int _msgType, bool _myMsg, string _assetPath, string _objName, _AALResourceCore _resourceCore, int _minCacheCount = 3, int _maxCacheCount = 10)
            where T_WND : _AGUISubWndChatMsgListItem<T_MONO, T_DATA>
            where T_MONO : _AGUIMonoChatMsgListItem
            where T_DATA : _IMsgItemData
        {
            // 构建一个唯一key
            int dicKey = ChatUtility.makeMsgItemDataKey(_msgType, _myMsg);

            // 重复注册保护
            if (_m_msgType2Setting.ContainsKey(dicKey))
            {
                ChatUtility.logError_DebugOnly($"传入的_msgType和_myMsg已经被注册过了，msgType为（{_msgType}），_myMsg为（{_myMsg}）");
                return;
            }

            // 合法检查，这里不允许对于不同的myMsg注册两个不一样的wnd，mono，data类型
            if (_m_msgType2Setting.TryGetValue(ChatUtility.makeMsgItemDataKey(_msgType, !_myMsg), out GUICacheMgrChatMsgItemSetting setting) && setting != null &&
                (setting.monoType != typeof(T_MONO) || setting.wndType != typeof(T_WND) || setting.dataType != typeof(T_DATA)))
            {
                ChatUtility.logError_DebugOnly($"暂不支持对于相同的msgType，不同的myMsg，设置不一样的mono,wnd,data类型");
                return;
            }

            // 添加设置到字典中
            _m_msgType2Setting.Add(dicKey, new GUICacheMgrChatMsgItemSetting()
            {
                monoType = typeof(T_MONO),
                wndType = typeof(T_WND),
                dataType = typeof(T_DATA),

                assetPath = _assetPath,
                objName = _objName,
                resourceCore = _resourceCore,

                minCacheCount = _minCacheCount,
                maxCacheCount = _maxCacheCount
            });
        }
        /// <summary>
        /// 反注册对应的设置
        /// </summary>
        public bool unregisterCache(int _msgType, bool _myMsg)
        {
            // 构建一个唯一key
            int dicKey = ChatUtility.makeMsgItemDataKey(_msgType, _myMsg);
            // 直接从字典中移除
            return _m_msgType2Setting.Remove(dicKey);
        }
        /// <summary>
        /// 创建一个wnd类实例
        /// </summary>
        /// <param name="_msgInfo">根据这个消息类型获取对于的wnd类</param>
        /// <param name="_parent">构建这个wnd类所需的父对象</param>
        /// <typeparam name="T_WND">指定wnd类的类型</typeparam>
        /// <returns>返回对于的wnd类实例</returns>
        public T_WND createWnd<T_WND>(_IMsgItemData _msgInfo, Transform _parent)
            where T_WND : class, _IGUISubWndChatMsgListItem
        {
            // 没有指定对应的msgInfo就直接返回
            if (_msgInfo == null)
                return null;

            // 构建一个唯一key
            int msgType = _msgInfo.msgType;
            bool isMyMsg = _msgInfo.isMyMsg;
            int dicKey = ChatUtility.makeMsgItemDataKey(msgType, isMyMsg);

            // 尝试获取对应的设置
            if (_m_msgType2Setting.TryGetValue(dicKey, out GUICacheMgrChatMsgItemSetting setting) && setting?.wndType != null)
            {
                // 构建对于的wnd类并返回
                return Activator.CreateInstance(setting.wndType, _msgInfo, _parent, this) as T_WND;
            }
            else
            {
                // 没有获取到对应的设置，输出错误提示
                ChatUtility.logError_DebugOnly($"你没有注册msgType为（{msgType}）myMsg为（{isMyMsg}）的设置，请使用registerCache注册窗口信息");
                return null;
            }
        }
        /// <summary>
        /// 根据数据类型获取模板对象
        /// </summary>
        /// <param name="_msgInfo">指定需要获取哪条消息的模板对象</param>
        /// <param name="_action">获取到模板对象后的回调</param>
        /// <typeparam name="T_MONO">指定模板对象的类型</typeparam>
        public void getTemplate<T_MONO>(_IMsgItemData _msgInfo, Action<T_MONO> _action) 
            where T_MONO : _AGUIMonoChatMsgListItem
        {
            // 如果回调为null，或者没有指定对应的msgInfo就直接返回
            if (_action == null || _msgInfo == null)
            {
                _action?.Invoke(null);
                return;
            }

            // 构建一个唯一key
            int msgType = _msgInfo.msgType;
            bool isMyMsg = _msgInfo.isMyMsg;
            int dicKey = ChatUtility.makeMsgItemDataKey(msgType, isMyMsg);

            // 尝试获取对应的设置
            if (_m_msgType2Setting.TryGetValue(dicKey, out GUICacheMgrChatMsgItemSetting setting) && setting != null)
            {
                // 临时的cache
                ChatGameObjectCache cache = null;
                // Cache加载完后的处理
                void onCacheLoaded()
                {
                    // 如果加载完了模板为null，说明加载失败了，直接调用回调，返回空
                    if (cache == null || cache.template == null)
                    {
                        ChatUtility.logError_DebugOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache加载模板失败了，请检查资源是否存在");
                        _action.Invoke(null);
                        return;
                    }

                    // 尝试获取模板对应的mono
                    T_MONO mono = cache.template.GetComponent(setting.monoType) as T_MONO;
                    if (mono == null)
                    {
                        ChatUtility.logError_DebugOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，上面没有挂载mono: {setting.monoType}!");
                        _action.Invoke(null);
                        return;
                    }
                    
                    // 一切都结束了之后调用回调，传入构建好的wnd
                    _action.Invoke(mono);
                }

                // 尝试获取对应的cache
                if (_m_msgType2Cache.TryGetValue(dicKey, out cache) && cache != null)
                {
                    // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                    cache.regLoadDoneDelegate(onCacheLoaded);
                }
                else
                {
                    // 如果没有获取cache，就构建一个新的
                    cache = new ChatGameObjectCache(setting, _m_cacheRoot, setting.minCacheCount, setting.maxCacheCount);
                    // 添加到字典中
                    _m_msgType2Cache[dicKey] = cache;

                    // 开始加载，并注册加载完成后的回调
                    cache.load();
                    cache.regLoadDoneDelegate(onCacheLoaded);
                }
            }
            else
            {
                // 没有获取到对应的设置，输出错误提示
                ChatUtility.logError_DebugOnly($"你没有注册msgType为（{msgType}）myMsg为（{isMyMsg}）的设置，请使用registerCache注册窗口信息");
                _action.Invoke(null);
            }
        }

        /// <summary>
        /// 获取一个对应的mono
        /// </summary>
        /// <typeparam name="T_MONO">指定mono的类型</typeparam>
        /// <param name="_msgInfo">指定的msgInfo</param>
        /// <param name="_parent">父对象</param>
        /// <param name="_onLoaded">获取完后的回调</param>
        public void popItem<T_MONO>(_IMsgItemData _msgInfo, Transform _parent, Action<T_MONO> _onLoaded)
            where T_MONO : _AGUIMonoChatMsgListItem
        {
            // 如果回调为null，或者没有指定对应的msgInfo就直接返回
            if (_onLoaded == null || _msgInfo == null)
                return;

            // 构建一个唯一key
            int msgType = _msgInfo.msgType;
            bool isMyMsg = _msgInfo.isMyMsg;
            int dicKey = ChatUtility.makeMsgItemDataKey(msgType, isMyMsg);

            // 临时的cache
            ChatGameObjectCache cache = null;
            // Cache加载完后的处理
            void onCacheLoaded()
            {
                // 如果没有Cache直接返回，虽然现有逻辑（2022.1.5）这里可以保证不会没有cache
                if (cache == null)
                {
                    ChatUtility.logError_DebugOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache为null");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 从对应的缓存池中获取GameObject
                GameObject go = cache.popItem();
                if (go == null)
                {
                    ChatUtility.logError_DebugOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，对应的cache popItem出现问题了，pop出的item为null!");
                    _onLoaded.Invoke(null);
                    return;
                }

                // 尝试从获取的GameObject上获取对应的mono
                T_MONO mono = go.GetComponent<T_MONO>();
                if (mono == null)
                {
                    ChatUtility.logError_DebugOnly($"prefab路径为{cache.assetPath} {cache.objName}的资源，上面没有挂载mono: {typeof(T_MONO)}!");
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
            if (_m_msgType2Cache.TryGetValue(dicKey, out cache) && cache != null)
            {
                // 如果cache已经有了，就在cache加载完后调用加载完后的处理
                cache.regLoadDoneDelegate(onCacheLoaded);
            }
            else
            {
                // 如果cache没有，就就先尝试获取对应的设置
                if (_m_msgType2Setting.TryGetValue(dicKey, out GUICacheMgrChatMsgItemSetting setting) && setting != null)
                {
                    // 使用设置构建cache
                    cache = new ChatGameObjectCache(setting, _m_cacheRoot, setting.minCacheCount, setting.maxCacheCount);
                    // 添加到cache的字典中
                    _m_msgType2Cache[dicKey] = cache;

                    // 开始加载，并注册加载完成后的回调
                    cache.load(); 
                    cache.regLoadDoneDelegate(onCacheLoaded);
                }
                else
                {
                    // 没有获取到对应的设置，输出错误提示
                    ChatUtility.logError_DebugOnly($"【ERROR】你没有注册msgType为（{dicKey}）myMsg为（{isMyMsg}）的设置，请使用registerCache注册窗口信息");
                    _onLoaded.Invoke(null);
                }
            }
        }
        /// <summary>
        /// 把item放回缓存池
        /// </summary>
        /// <param name="_msgInfo">缓存池对应的索引msgInfo</param>
        /// <param name="_item">需要被放回的item</param>
        public void pushBackItem(_IMsgItemData _msgInfo, GameObject _item)
        {
            // 如果item为null或者索引msgInfo为null，都认为无效
            if (_item == null || _msgInfo == null)
                return;

            // 构建一个唯一key
            int dicKey = ChatUtility.makeMsgItemDataKey(_msgInfo);

#if UNITY_EDITOR
            // 在Editor环境下做一次类型的检查
            if (_m_msgType2Setting.TryGetValue(dicKey, out GUICacheMgrChatMsgItemSetting setting) && setting != null)
            {
                Component mono = _item.GetComponent(setting.monoType);
                if (mono == null)
                {
                    ChatUtility.logError_DebugOnly($"出现了未知的异常，Cache在回收资源时，回收的GameObject和设置中的类型不一样，正常不直接访问internal的成员不应该触发这个问题");
                    return;
                }
            }
            else
            {
                ChatUtility.logError_DebugOnly($"出现了未知的异常，正常不直接访问internal的成员不应该触发这段代码");
                return;
            }
#endif
            // 尝试找到对应的缓存池
            if (_m_msgType2Cache.TryGetValue(dicKey, out ChatGameObjectCache cache) && cache != null)
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
                ChatUtility.logError_DebugOnly($"出现了未知的异常，Cache回收资源时，传入了不是这里pop出来的GameObject，正常不直接访问internal的成员不应该触发这段代码");
                // 并释放对应的GameObject，你都要回收了，我回收不了，就给你扬咯
                ALUnityCommon.releaseGameObj(_item);
            }
        }
        /// <summary>
        /// 清空所有内容，包括缓存池中的所有item，所有缓存池，所有设置
        /// </summary>
        public void discard()
        {
            // 清空缓存池中所有的item
            foreach (ChatGameObjectCache item in _m_msgType2Cache.Values)
            {
                item?.discard();
            }

            // 清空缓存池
            _m_msgType2Cache.Clear();
            // 清空设置
            _m_msgType2Setting.Clear();
        }
        /// <summary>
        /// 尝试获取对应的设置
        /// </summary>
        /// <param name="_msgType">用于索引的msgType</param>
        /// <param name="_myMsg">用于索引的myMsg</param>
        /// <returns></returns>
        public GUICacheMgrChatMsgItemSetting getCacheSetting(int _msgType, bool _myMsg)
        {
            // 构建一个唯一key
            int dicKey = ChatUtility.makeMsgItemDataKey(_msgType, _myMsg);
            // 使用唯一key获取对应的设置
            if (_m_msgType2Setting.TryGetValue(dicKey, out GUICacheMgrChatMsgItemSetting setting))
            {
                return setting;
            }

            return null;
        }
    }
}

