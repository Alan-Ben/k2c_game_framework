using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AConsortChatInfo : _AALBasicLoadObj
    {
        /// <summary>
        /// 当前内存中拥有的聊天消息列表
        /// </summary>
        [NotNull] protected readonly List<_AConsortChatMsgInfo> _m_listMsgInfo;
        // 加载序列号，每次load或者discard的时候会自增
        private long _m_lLoadSerialize;
        
        /// <summary>
        /// 聊天会话的唯一标识
        /// </summary>
        public virtual string id { get; }
        
        /// <summary>
        /// 当接受到新消息后触发的事件
        /// </summary>
        public event Action<_AConsortChatMsgInfo> onReceiveMsg;
        public event Action<bool> changeTypingState;
        
        protected _AConsortChatInfo()
        {
            // 构造固定存在的列表
            _m_listMsgInfo = new List<_AConsortChatMsgInfo>();
        }
        
        // 加载转到UnreadMsg中处理
        protected sealed override void _loadOp()
        {
            _m_lLoadSerialize = ALSerializeOpMgr.next();
            
            _doLoadOp(_initDone);
        }
        // 内部的初始化完成方法
        private void _initDone(List<_AConsortChatMsgInfo> _unreadMsgList)
        {
            if (_unreadMsgList != null)
            {
                // 将未读的消息加入到消息列表当中
                _m_listMsgInfo.AddRange(_unreadMsgList);
            }

            // 设置加载完成
            _setLoadDone();
        }
        
        // 销毁聊天会话
        protected sealed override void _discard()
        {
            _m_lLoadSerialize = ALSerializeOpMgr.next();

            // 释放消息列表
            _m_listMsgInfo.Clear();
        }
        /// <summary>
        /// 收到了新消息
        /// </summary>
        public void receiveMsg(_AConsortChatMsgInfo _msgInfo)
        {
            if (_msgInfo == null)
                return;

            _m_listMsgInfo.Add(_msgInfo);
            onReceiveMsg?.Invoke(_msgInfo);
        }

        public void setShowTyping(bool _showTyping)
        {
            changeTypingState?.Invoke(_showTyping);
        }
        /// <summary>
        /// 获取最新的历史消息记录
        /// </summary>
        /// <param name="_msgCount">需要历史记录的数量</param>
        /// <param name="_action">获取完成之后的回调</param>
        public void getHistoryList(int _msgCount, Action<List<_AConsortChatMsgInfo>> _action)
        {
            getHistoryList(-1, _msgCount, _action);
        }
        /// <summary>
        /// 获取任意时刻的历史消息
        /// </summary>
        /// <param name="_msgId">指定某条消息以前的历史消息</param>
        /// <param name="_msgCount">需要的数量</param>
        /// <param name="_action">获取完成后的回调</param>
        public void getHistoryList(long _msgId, int _msgCount, Action<List<_AConsortChatMsgInfo>> _action)
        {
            // 如果没有回调，或者并不需要历史消息，就不处理了
            if (_action == null || _msgCount <= 0)
            {
                // 直接调用完成回调，给一个空列表
                _action?.Invoke(null);
                return;
            }

            if (!isInited)
            {
                Debug.LogError_EditorOnly($"id为（{id}）的聊天会话在没有调用load的情况下就使用了getHistoryList方法");
            }

            long serialize = _m_lLoadSerialize;
            // 如果还没有加载完，就等待到加载完再尝试获取历史消息
            regLoadDoneDelegate(() =>
            {
                // 如果加载序列号都不一样了，这里就不处理了，直接给一个空列表
                if (serialize != _m_lLoadSerialize)
                {
                    _action.Invoke(null);
                    return;
                }

                // 先从客户都缓存里取
                List<_AConsortChatMsgInfo> result = getHistoryListSync(_msgId, _msgCount);
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
                            _action.Invoke(null);
                            return;
                        }

                        // 讲拿到的历史消息添加到队列首
                        if (_list != null && _list.Count > 0)
                        {
                            result.InsertRange(0, _list);
                            // 如果这个消息是可以加入到本地的，就加入到本地缓存起来
                            if (canAddToLocal)
                            {
                                _AConsortChatMsgInfo lastOldMsg = _m_listMsgInfo.Count > 0 ? _m_listMsgInfo[0] : null;
                                if (null == lastOldMsg)
                                {
                                    _m_listMsgInfo.InsertRange(0, _list);
                                    // _m_dataMgr.onChatInfoGetNewHistoryMsgList(_list);
                                }
                                else
                                {
                                    for (int i = _list.Count - 1; i > -1; i--)
                                    {
                                        if(lastOldMsg.msgId <= _list[i].msgId)
                                            _list.RemoveAt(i);
                                    }
                                    _m_listMsgInfo.InsertRange(0, _list);
                                    // _m_dataMgr.onChatInfoGetNewHistoryMsgList(_list);
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
        /// 获取从某条消息之后的历史消息
        /// </summary>
        /// <param name="_msgId">指定需要哪一条消息之后的历史消息，如果传入的数不大于0，则说明需要的是从最新消息开始算的历史消息</param>
        /// <param name="_msgCount">需要多少条消息</param>
        /// <param name="_action">获取完成之后的回调</param>
        protected abstract void _getHistoryList(long _msgId, int _msgCount, Action<List<_AConsortChatMsgInfo>> _action);
        
        
        /// <summary>
        /// 获取这个聊天当前未读的所有消息
        /// </summary>
        /// <param name="_action">获取之后务必调用这个委托</param>
        protected abstract void _doLoadOp(Action<List<_AConsortChatMsgInfo>> _action);
        
        /// <summary>
        /// 同步获得历史消息，只能获得到客户端内存中的
        /// </summary>
        [NotNull]
        public List<_AConsortChatMsgInfo> getHistoryListSync(long _msgId, int _msgCount)
        {
            // 准备一个新的结果列表
            List<_AConsortChatMsgInfo> result = new List<_AConsortChatMsgInfo>(_msgCount);

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
                        _AConsortChatMsgInfo msgInfo = _m_listMsgInfo[i];
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
                    for (int i = startIndex; i < _m_listMsgInfo.Count && _msgCount > 0; i++, _msgCount--)
                    {
                        _AConsortChatMsgInfo msgInfo = _m_listMsgInfo[i];
                        if (msgInfo == null)
                            continue;
                            
                        if (_msgId <= msgInfo.msgId)
                            break;

                        // 添加到结果列表中
                        result.Add(msgInfo);
                    }
                }
            }

            return result;
        }
    }
}