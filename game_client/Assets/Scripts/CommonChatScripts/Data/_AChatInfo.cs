
using System;
using System.Collections.Generic;

using ALPackage;
using UnityEngine;
using ChatPackage.Internal;
using JetBrains.Annotations;

namespace ChatPackage
{
    /// <summary>
    /// 一个聊天会话的抽象
    /// </summary>
    /// <remarks>
    /// <para>这个类主要用来派生出聊天室类<see cref="_ARoomChatInfo"/>和私聊类<see cref="_APrivateChatInfo"/>，你也可以自己继承这个类自定义会话的相关内容</para>
    /// <para>需要注意，内部的消息列表的排列顺序是从旧到新</para>
    /// <para>需要注意，这里用了底层的<see cref="_AALBasicLoadObj"/>来实现加载接口，所以load和discard会被暴露在外部，但是聊天包内部会自动调用load和discard方法，在外部请不要随意调用</para>
    /// <para>主要使用方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>onReceiveMsg</term>
    ///             <description>
    ///             可以往这里注册收到消息之后的处理
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>网络相关内容</term>
    ///             <description>
    ///             你可以在这里找到关于这个ChatInfo的网络连接的状态以及事件，包括当前的连接状态，连接成功与断开的事件
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>regLoadDoneDelegate</term>
    ///             <description>
    ///             要注意这个类是需要调用load来进行加载之后才有消息列表的，所以一些相关的操作要使用这个委托包起来
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>id</term>
    ///             <description>
    ///             这个会话有一个唯一的string标识，但是唯一性要由服务端来保证，聊天室的话是聊天室roomId，私聊是对方的聊天tag
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>getHistoryList</term>
    ///             <description>
    ///             获取历史消息的方法，外部只能通过这个方法来获取已经接收到的消息内容，你可以指定需要的消息数量，或是哪一条消息后开始算的
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>继承后你需要实现：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>id</term>
    ///             <description>
    ///             这个聊天的唯一标识，必须保证唯一
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>sendMsg</term>
    ///             <description>
    ///             你必须实现这个聊天会话将会如何发送消息，比如聊天室会直接发往聊天服务器，而私聊则是会交给游戏客户端自己去处理，这也是框架的设计问题，客户端的角度来看是统一的逻辑做在这里比较合理
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_initByUnreadMsg</term>
    ///             <description>
    ///             当调用load方法的时候就会开始加载未读的消息，并把这些消息都存到本地，完成后请务必要调用这里的回调
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_onDiscard</term>
    ///             <description>
    ///             当调用discard时会调用的方法，你可以在这里释放所有的内容
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_getHistoryList</term>
    ///             <description>
    ///             获取历史消息的方法，获取历史消息时会先看看内存中的数据够不够，不够的话就会调用这个方法来让子类具体处理怎么获得历史消息，比如聊天室会跟服务器请求，而私聊会去本地存档里找，要注意返回的消息列表要从旧到新排列
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public abstract class _AChatInfo : _AALBasicLoadObj
    {
        /// <summary>
        /// 当前内存中拥有的聊天消息列表
        /// </summary>
        [NotNull] protected readonly List<MsgInfo> _m_listMsgInfo;
        // 加载序列号，每次load或者discard的时候会自增
        private long _m_lLoadSerialize;
        // 聊天数据管理器的实例
        private ChatDataMgr _m_dataMgr;
        // 用来返回的没啥用的空列表
        private List<MsgInfo> _m_listEmpty = new List<MsgInfo>();
        // 上一次发送的时间
        private long _m_lLastSendTime; 

        protected _AChatInfo()
        {
            // 构造固定存在的列表
            _m_listMsgInfo = new List<MsgInfo>();
        }
        
        /// <summary>
        /// 当接受到新消息后触发的事件
        /// </summary>
        public event Action<MsgInfo> onReceiveMsg;
        /// <summary>
        /// 当这个ChatInfo的网络连接上时的事件
        /// </summary>
        public event Action onNetConnected;
        /// <summary>
        /// 当这个ChatInfo的网络连接断开时的事件
        /// </summary>
        public event Action onNetDisconnected;
        
        /// <summary>
        /// 获取这个ChatInfo对应所属的数据管理器
        /// </summary>
        protected ChatDataMgr dataMgr { get { return _m_dataMgr; } }
        /// <summary>
        /// 聊天会话的唯一标识
        /// </summary>
        public abstract string id { get; }
        /// <summary>
        /// 这个ChatInfo所属的网络连接的连接状态
        /// </summary>
        public bool isNetConnected
        {
            get
            {
                if (_m_dataMgr != null) return _m_dataMgr.chatData.isConnected;
                return false;
            }
        }
        /// <summary>
        /// 上一次发送消息的时间
        /// </summary>
        public long lastSendTime { get { return _m_lLastSendTime; } }
        /// <summary>
        /// 同步获得历史消息，只能获得到客户端内存中的
        /// </summary>
        [NotNull]
        public List<MsgInfo> getHistoryListSync(int _msgCount)
        {
            return getHistoryListSync(-1, _msgCount);
        }
        /// <summary>
        /// 同步获得历史消息，只能获得到客户端内存中的
        /// </summary>
        [NotNull]
        public List<MsgInfo> getHistoryListSync(long _msgId, int _msgCount)
        {
            // 准备一个新的结果列表
            List<MsgInfo> result = new List<MsgInfo>(_msgCount);

            // 如果本地就有消息列表，就判断一下看看所需要的历史消息是不是这里就够应付了
            if (_m_listMsgInfo.Count > 0)
            {
                // 计算需要从哪里开始获取历史消息
                int startIndex = -1;
                // 如果msgId无效，就认为要的是最新的消息
                if (_msgId <= 0)
                    // 往最新消息回头算应该获取的最老的消息是哪一个
                    startIndex = Mathf.Max(0, _m_listMsgInfo.Count - _msgCount);
                else
                {
                    // 开始寻找是需要哪一条消息后的历史消息
                    for (int i = _m_listMsgInfo.Count - 1; i >= 0; i--)
                    {
                        MsgInfo msgInfo = _m_listMsgInfo[i];
                        if (msgInfo == null)
                            continue;

                        // 比对成功
                        if (_msgId > msgInfo.msgId)
                        {
                            // 计算这条消息之前的多少条历史消息
                            startIndex = Mathf.Max(0, i - _msgCount + 1);
                            break;
                        }
                    }
                }

                // 如果上面由成功赋值，这个index会是不小于0的数，说明可以从本地消息中拿到一定数量的历史记录
                if (startIndex >= 0)
                {
                    // 从本地的缓存中获取历史消息，并且拿一条就把msgCount减1
                    for (int i = startIndex; i < _m_listMsgInfo.Count && _msgCount > 0; i++)
                    {
                        MsgInfo msgInfo = _m_listMsgInfo[i];
                        if (msgInfo == null)
                            continue;

                        if (_msgId > 0 && _msgId <= msgInfo.msgId)
                            break;

                        // 添加到结果列表中
                        result.Add(msgInfo);
                        _msgCount--;
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// 获取最新的历史消息记录
        /// </summary>
        /// <param name="_msgCount">需要历史记录的数量</param>
        /// <param name="_action">获取完成之后的回调</param>
        public void getHistoryList(int _msgCount, Action<List<MsgInfo>> _action)
        {
            getHistoryList(-1, _msgCount, _action);
        }
        /// <summary>
        /// 获取任意时刻的历史消息
        /// </summary>
        /// <param name="_msgId">指定某条消息以前的历史消息</param>
        /// <param name="_msgCount">需要的数量</param>
        /// <param name="_action">获取完成后的回调</param>
        public void getHistoryList(long _msgId, int _msgCount, Action<List<MsgInfo>> _action)
        {
            // 如果没有回调，或者并不需要历史消息，就不处理了
            if (_action == null || _msgCount <= 0)
            {
                // 直接调用完成回调，给一个空列表
                _action?.Invoke(_m_listEmpty);
                return;
            }

            if (!isInited)
            {
                ChatUtility.logWarning_DebugOnly($"id为（{id}）的聊天会话在没有调用load的情况下就使用了getHistoryList方法");
            }

            long serialize = _m_lLoadSerialize;
            // 如果还没有加载完，就等待到加载完再尝试获取历史消息
            regLoadDoneDelegate(() =>
            {
                // 如果加载序列号都不一样了，这里就不处理了，直接给一个空列表
                if (serialize != _m_lLoadSerialize)
                {
                    _action.Invoke(_m_listEmpty);
                    return;
                }

                // 先从客户都缓存里取
                List<MsgInfo> result = getHistoryListSync(_msgId, _msgCount);
                _msgCount -= result.Count;

                // 如果消息已经都取完了，就直接调用回调，返回
                if (_msgCount <= 0)
                    _action.Invoke(result);
                else
                {
                    // 否则的话，交给子类，看子类还能不能从哪里弄到历史消息
                    serialize = _m_lLoadSerialize;
                    // 如果上面已经找到了一部分的历史消息的话，就从result列表最老的那一条开始找历史消息
                    // 如果上面一条消息都没有找到，那么直接用传入的参数来找历史消息
                    long msgId = result.Count > 0 ? result[0].msgId : _msgId;
                    // 判断获取完历史消息之后是否可以加入到本地
                    bool canAddToLocal = msgId < 0 ||   // msgId小于0说明是从最新开始的消息，说明本地内存中的消息列表为空，可以加入
                                         (_m_listMsgInfo.Count > 0 && _m_listMsgInfo[0].msgId == msgId); // 否则就要确保请求到的历史消息是接在当前内存中历史消息的后面
                    // 尝试获取历史消息，这里的_msgCount是已经经过上面的逻辑，自减过了
                    _getHistoryList(msgId, _msgCount, (_list) =>
                    {
                        // 如果加载序列号都不一样了，这里就不处理了，直接给一个空列表
                        if (serialize != _m_lLoadSerialize)
                        {
                            _action.Invoke(_m_listEmpty);
                            return;
                        }

                        // 讲拿到的历史消息添加到队列首
                        if (_list != null && _list.Count > 0)
                        {
                            result.InsertRange(0, _list);
                            // 如果这个消息是可以加入到本地的，就加入到本地缓存起来
                            if (canAddToLocal)
                            {
                                MsgInfo lastOldMsg = _m_listMsgInfo.Count > 0 ? _m_listMsgInfo[0] : null;
                                if (null == lastOldMsg)
                                {
                                    _m_listMsgInfo.InsertRange(0, _list);
                                    _m_dataMgr.onChatInfoGetNewHistoryMsgList(_list);
                                }
                                else
                                {
                                    for (int i = _list.Count - 1; i > -1; i--)
                                    {
                                        if(lastOldMsg.msgId <= _list[i].msgId)
                                            _list.RemoveAt(i);
                                    }
                                    _m_listMsgInfo.InsertRange(0, _list);
                                    _m_dataMgr.onChatInfoGetNewHistoryMsgList(_list);
                                }
                            }
                        }

                        // 调用回调
                        _action.Invoke(result);
                    });
                }
            });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public bool sendMsg(_AMsgDetailInfo _msgInfo)
        {
            //判断是否能发送消息
            if (!_checkCanSendMsg(true))
                return false;
            
            _m_lLastSendTime = _getNowTime();
            _sendMsg(_msgInfo);
            return true;
        }

        // 加载转到UnreadMsg中处理
        protected sealed override void _loadOp()
        {
            _m_lLoadSerialize = ALSerializeOpMgr.next();
            _initByUnreadMsg(_initDone);
        }
        // 销毁聊天会话
        protected sealed override void _discard()
        {
            _m_lLoadSerialize = ALSerializeOpMgr.next();

            // 释放消息列表
            _m_listMsgInfo.Clear();
            
            _onDiscard();
        }

        protected virtual long _getNowTime()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        protected abstract void _sendMsg(_AMsgDetailInfo _msgInfo);

        /// <summary>
        /// 判断当前是否可以发送
        /// </summary>
        /// <param name="_needPopTip">不可发送时候是否需要提示</param>
        /// <returns></returns>
        protected abstract bool _checkCanSendMsg(bool _needPopTip);
        
        /// <summary>
        /// 获取这个聊天当前未读的所有消息
        /// </summary>
        /// <param name="_action">获取之后务必调用这个委托</param>
        protected abstract void _initByUnreadMsg(Action<List<MsgInfo>> _action);
        /// <summary>
        /// 当这个会话被销毁时调用
        /// </summary>
        protected abstract void _onDiscard();
        /// <summary>
        /// 获取从某条消息之后的历史消息
        /// </summary>
        /// <param name="_msgId">指定需要哪一条消息之后的历史消息，如果传入的数不大于0，则说明需要的是从最新消息开始算的历史消息</param>
        /// <param name="_msgCount">需要多少条消息</param>
        /// <param name="_action">获取完成之后的回调</param>
        protected abstract void _getHistoryList(long _msgId, int _msgCount, Action<List<MsgInfo>> _action);

        /// <summary>
        /// 收到了新消息
        /// </summary>
        internal void recieveMsg(MsgInfo _msgInfo)
        {
            if (_msgInfo == null)
                return;

            _m_listMsgInfo.Add(_msgInfo);
            onReceiveMsg?.Invoke(_msgInfo);
        }
        /// <summary>
        /// 设置这个ChatInfo所属的数据管理器
        /// </summary>
        internal void setDataMgr(ChatDataMgr _chatDataMgr)
        {
            // 先清除上一次注册的内容
            resetDataMgr();

            // 赋值并绑定事件
            _m_dataMgr = _chatDataMgr;
            if (_m_dataMgr != null)
            {
                _m_dataMgr.chatData.onConnected += _onNetConnected;
                _m_dataMgr.chatData.onDisconnected += _onNetDisconnected;
            }
        }
        /// <summary>
        /// 清除这个ChatInfo所属的数据管理器
        /// </summary>
        internal void resetDataMgr()
        {
            // 如果本来就不存在就不处理
            if (_m_dataMgr == null)
                return;
            
            // 解除相关事件的绑定
            _m_dataMgr.chatData.onConnected -= _onNetConnected;
            _m_dataMgr.chatData.onDisconnected -= _onNetDisconnected;
            // 释放引用
            _m_dataMgr = null;
        }

        // 内部的初始化完成方法
        private void _initDone(List<MsgInfo> _unreadMsgList)
        {
            if (_unreadMsgList != null)
            {
                // 将未读的消息加入到消息列表当中
                _m_listMsgInfo.AddRange(_unreadMsgList);
            }

            // 设置加载完成
            _setLoadDone();
        }
        // 网络连接上时触发的回调
        private void _onNetConnected()
        {
            onNetConnected?.Invoke();
        }
        // 网络断开时触发的回调
        private void _onNetDisconnected()
        {
            onNetDisconnected?.Invoke();
        }
    }
}