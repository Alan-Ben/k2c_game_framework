
using System;
using ALPackage;
using UnityEngine;
using ChatPackage.Internal;
using JetBrains.Annotations;
using System.Collections.Generic;
using GOE;
using UnityEngine.EventSystems;

namespace ChatPackage
{
    /// <summary>
    /// 显示一个ChatInfo聊天列表的wnd
    /// </summary>
    /// <remarks>
    /// 这个wnd可以自己显示一个chatInfo的信息列表，并且可以自动更新信息的变动情况
    /// <para>内部所有的消息列表都遵循“旧到新”的顺序排列</para>
    /// <para>主要方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Wnd的通用操作</term>
    ///             <description>
    ///             可以正常使用showWnd，hideWnd之类的wnd操作来控制窗口
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>setShowData</term>
    ///             <description>
    ///             传入chatInfo就可以开始运作，这个方法可以传入null，用来清空wnd的数据
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>clearShowData</term>
    ///             <description>
    ///             完全相当于上面的setShowData方法传入null
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>setNeedForceRefreshList</term>
    ///             <description>
    ///             完全相当于上面的setShowData方法传入null
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>你可以重载下列方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>_initMsgListModify</term>
    ///             <description>
    ///             在wnd refresh的时候，第一次获取消息列表之后会调用这个方法，你可以重写这个方法，对消息列表做自定义的修改
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_historyMsgListModify</term>
    ///             <description>
    ///             在wnd上拉到顶部，获取历史消息之后，会调用这个方法，你可以重写这个方法，对获取到的历史消息列表做自定义的修改
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_beforeReceiveMsgModify</term>
    ///             <description>
    ///             在wnd接收到了新消息更新的时候会调用这个方法，你可以重写这个方法，在接收到新消息时做额外处理
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>你可以使用的protected内容：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>showMsgList</term>
    ///             <description>
    ///             你可以用这个属性获取现在在wnd列表里的所有消息，包括了不在视口范围里的所有，但是并不是chatInfo里的所有消息
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>showMsgItemList</term>
    ///             <description>
    ///             你可以用这个属性获取现在在wnd列表里的所有消息item，包括了不在视口范围里的所有item
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_addLastMsgItem</term>
    ///             <description>
    ///             你可以使用这个方法，往最新消息方向，自定义添加消息。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>_addFirstMsgItem</term>
    ///             <description>
    ///             你可以使用这个方法，往最老消息方向也就是历史消息的方向，自定义添加消息。
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>更多内容可以查阅<see cref="ALUGUIWndVerticalMultiSizeLayout{T_MONO}"/></para>
    /// </remarks>
    public class GUISubWndChatMsgList<T_MONO> : ALUGUIWndVerticalMultiSizeLayout<T_MONO>
        where T_MONO : GUIMonoChatMsgList
    {
        // 现在在wnd列表里的所有消息item，包括了不在视口范围里的所有item
        [NotNull] private readonly List<_IGUISubWndChatMsgListItem> _m_showMsgItemList;
        // 这个列表将和上面的列表保持同步更新，内部的数据都是一样的，这个列表会专门传给refresh方法
        [NotNull] private readonly List<_IALBasicMultiSizeLayoutItem> _m_showMsgItemListForRefresh;
        // 这个列表的chatItem缓存
        [NotNull] private readonly GUICacheMgrChatMsgItem _m_itemCacheMgr;
        // 当前wnd展示的chatInfo
        private _AChatInfo _m_chatInfo;
        // 当前显示的最老的一条消息，因为_m_showMsgList可能包含有子类自定义添加进来的消息，所以这里额外记录一下最老的消息是啥
        private MsgInfo _m_oldestMsg;
        // 刷新序列号
        private int _m_iRefreshSerialize;
        // 是否初始化完成了
        private bool _m_bIsInit;
        // 是否需要刷新列表
        private bool _m_bIsNeedRefreshList;
        // 是否保持显示最新消息
        private bool _m_bSeeNewest
        {
            get { return __m_bSeeNewest; } 
            set { if (__m_bSeeNewest != value) { onSeeNewest?.Invoke(value); __m_bSeeNewest = value; } }
        }
        private bool __m_bSeeNewest;

        public GUISubWndChatMsgList(T_MONO _wnd, [NotNull] GUICacheMgrChatMsgItem _itemCacheMgr) : base(_wnd)
        {
            // list初始化
            _m_showMsgItemList = new List<_IGUISubWndChatMsgListItem>();
            _m_showMsgItemListForRefresh = new List<_IALBasicMultiSizeLayoutItem>();
            _m_itemCacheMgr = _itemCacheMgr;

            initWnd();
        }

        /// <summary>
        /// 当当前窗口保持展示最新消息时，true 为展示最新消息，false 为不展示最新消息
        /// </summary>
        public event Action<bool> onSeeNewest;
        /// <summary>
        /// 当前的聊天会话
        /// </summary>
        public _AChatInfo chatInfo { get { return _m_chatInfo; } }
        /// <summary>
        /// 现在在wnd列表里的所有消息item，包括了不在视口范围里的所有item
        /// </summary>
        [NotNull] protected List<_IGUISubWndChatMsgListItem> showMsgItemList { get { return _m_showMsgItemList; } }
        
        /// <summary>
        /// 传入chatInfo就可以开始运作，这个方法可以传入null，用来清空wnd的数据
        /// </summary>
        public void setShowData(_AChatInfo _chatInfo)
        {
            _AChatInfo oldChatInfo = _m_chatInfo;
            _m_chatInfo = _chatInfo;

            // 如果窗口已经展示了，说明已经调用过showWnd了，这里要重新调用刷新，并且绑定事件
            if (_m_bIsShow)
            {
                // 如果之前有数据，要先解除事件的绑定
                if (oldChatInfo != null)
                    oldChatInfo.onReceiveMsg -= _onReceiveMsg;
                // 重置内部和数据有关的东西
                _resetInfo();
                // 刷新
                _refreshWnd();
                // 重新绑定事件
                if (_m_chatInfo != null)
                    _m_chatInfo.onReceiveMsg += _onReceiveMsg;
            }
        }
        /// <summary>
        /// 完全相当于上面的setShowData方法传入null
        /// </summary>
        public void clearShowData()
        {
            setShowData(null);
        }
        /// <summary>
        /// 让列表在下一帧时强制刷新一次
        /// </summary>
        public void setNeedForceRefreshList()
        {
            _m_bIsNeedRefreshList = true;
        }
        
        // 界面显示的时候调用
        protected override void _onShowWnd()
        {
            // 每次打开界面都会进行刷新
            _refreshWnd();

            // 绑定新消息事件
            if (_m_chatInfo != null)
            {
                _m_chatInfo.onReceiveMsg += _onReceiveMsg;
                _m_chatInfo.onNetConnected += _onNetReconnected;
                _m_chatInfo.onNetDisconnected += _onNetDisconnected;
            }
            
            // 绑定ScrollRect的滚动事件
            if (wnd != null)
            {
                if (wnd.scrollRect != null && wnd.scrollRect.onValueChanged != null)
                    wnd.scrollRect.onValueChanged.AddListener(_scrollerValueChg);
                
                if (_m_chatInfo != null)
                    wnd.setNetState(_m_chatInfo.isNetConnected);
                else
                    wnd.setNetState(true);
            }
        }
        // 界面隐藏时调用
        protected override void _onHideWnd()
        {
            // 增加刷新序列号，表示之前的所有刷新内容都作废了
            _m_iRefreshSerialize = ALSerializeOpMgr.next();

            // 解除绑定新消息事件
            if (_m_chatInfo != null)
            {
                _m_chatInfo.onReceiveMsg -= _onReceiveMsg;
                _m_chatInfo.onNetConnected -= _onNetReconnected;
                _m_chatInfo.onNetDisconnected -= _onNetDisconnected;
            }

            // 解除绑定ScrollRect的滚动事件
            if (wnd != null && wnd.scrollRect != null && wnd.scrollRect.onValueChanged != null)
                wnd.scrollRect.onValueChanged.RemoveListener(_scrollerValueChg);
            
            // 每次显示窗口都会进行重新刷新，所以这个清空所有和这次数据有关的内容
            _resetInfo();
        }
        // 界面重置时调用，重置界面底层会调用hideWnd，所以不用做多余的处理
        protected override void _onReset()
        {
            _m_chatInfo = null;
        }
        // 销毁界面时调用，销毁界面底层会调用hideWnd，所以不用做多余的处理
        protected override void _onDiscard()
        {
            _m_chatInfo = null;
        }
        // 当wnd初始化完成时调用
        protected override void _onWndInitDone()
        {
            // 这里要求强行上对齐，内部逻辑依赖上对齐模式
            setAlignment(EALVerticalLayoutChildAlignment.Up);
        }

        /// <summary>
        /// 在wnd refresh的时候，第一次获取消息列表之后会调用这个方法，你可以重写这个方法，对消息列表做自定义的修改
        /// </summary>
        /// <param name="_msgInfoList">初始化时的消息列表，参数为列表的copy，可以随意修改</param>
        protected virtual List<_IMsgItemData> _initMsgListModify(List<MsgInfo> _msgInfoList)
        {
            if (_msgInfoList == null)
                return null;

            List<_IMsgItemData> itemDataList = new List<_IMsgItemData>();
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                itemDataList.Add(_msgInfoList[i].detailInfo);
            }

            return itemDataList;
        }
        /// <summary>
        /// 在wnd上拉到顶部，获取历史消息之后，会调用这个方法，你可以重写这个方法，对获取到的历史消息列表做自定义的修改
        /// </summary>
        /// <param name="_msgInfoList">历史消息列表，参数为列表的copy，可以随意修改</param>
        protected virtual List<_IMsgItemData> _historyMsgListModify(List<MsgInfo> _msgInfoList)
        {
            if (_msgInfoList == null)
                return null;

            List<_IMsgItemData> itemDataList = new List<_IMsgItemData>();
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                itemDataList.Add(_msgInfoList[i].detailInfo);
            }

            return itemDataList;
        }
        /// <summary>
        /// 在wnd接收到了新消息更新的时候会调用这个方法，你可以重写这个方法，在接收到新消息时做额外处理
        /// </summary>
        /// <param name="_newMsg">新的消息，这个消息不能修改</param>
        protected virtual void _beforeReceiveMsgModify(MsgInfo _newMsg)
        {
        }
        /// <summary>
        /// 往最新消息方向，自定义添加消息
        /// </summary>
        /// <param name="_msgInfoList">要添加的消息列表</param>
        protected void _addLastMsgItem(List<_IMsgItemData> _msgInfoList)
        {
            // 添加消息到待处理列表
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                _IGUISubWndChatMsgListItem wnd = _m_itemCacheMgr.createWnd<_IGUISubWndChatMsgListItem>(_msgInfoList[i], itemContainer);
                if (wnd == null)
                    continue;
                
                _m_showMsgItemList.Add(wnd);
                _m_showMsgItemListForRefresh.Add(wnd);
                _bindItemWnd(wnd);
            }
            
            // 让列表进行刷新
            setNeedForceRefreshList();
        }
        /// <summary>
        /// 往最新消息方向，自定义添加消息
        /// </summary>
        /// <param name="_msgInfo">要添加的消息</param>
        protected void _addLastMsgItem(_IMsgItemData _msgInfo)
        {
            // 添加消息到待处理列表
            _IGUISubWndChatMsgListItem wnd = _m_itemCacheMgr.createWnd<_IGUISubWndChatMsgListItem>(_msgInfo, itemContainer);
            if (wnd == null)
                return;
                
            _m_showMsgItemList.Add(wnd);
            _m_showMsgItemListForRefresh.Add(wnd);
            _bindItemWnd(wnd);
            
            // 让列表进行刷新
            setNeedForceRefreshList();
        }
        /// <summary>
        /// 往最老消息方向也就是历史消息的方向，自定义添加消息
        /// </summary>
        /// <param name="_msgInfoList">要添加的消息列表</param>
        protected void _addFirstMsgItem(List<_IMsgItemData> _msgInfoList)
        {
            if (_msgInfoList == null)
                return;

            // 添加消息到待处理列表
            for (int i = _msgInfoList.Count - 1; i >= 0; i--)
            {
                _IGUISubWndChatMsgListItem wnd = _m_itemCacheMgr.createWnd<_IGUISubWndChatMsgListItem>(_msgInfoList[i], itemContainer);
                if (wnd == null)
                    continue;
                
                _m_showMsgItemList.Insert(0, wnd);
                _m_showMsgItemListForRefresh.Insert(0, wnd);
                _bindItemWnd(wnd);
            }
            
            // 让列表进行刷新
            setNeedForceRefreshList();
        }
        /// <summary>
        /// 往最老消息方向也就是历史消息的方向，自定义添加消息。还可以附带一个这些消息加入完成之后的回调
        /// </summary>
        /// <param name="_msgInfo">要添加的消息</param>
        protected void _addFirstMsgItem(_IMsgItemData _msgInfo)
        {
            _IGUISubWndChatMsgListItem wnd = _m_itemCacheMgr.createWnd<_IGUISubWndChatMsgListItem>(_msgInfo, itemContainer);
            if (wnd == null)
                return;
            
            _m_showMsgItemList.Insert(0, wnd);
            _m_showMsgItemListForRefresh.Insert(0, wnd);
            _bindItemWnd(wnd);
            
            // 让列表进行刷新
            setNeedForceRefreshList();
        }
        /// <summary>
        /// 每帧都会被执行的方法
        /// </summary>
        /// <remarks>
        /// 在这里用来保证每帧最多都只会刷新一次列表
        /// </remarks>
        protected sealed override void _onFrameRefresh()
        {
            base._onFrameRefresh();

            // 这帧是否被标记为需要刷新
            if (_m_bIsNeedRefreshList)
            {
                // 记录当前显示的内容
                _IALBasicMultiSizeLayoutItem curTopItem = _m_lItemList.Count > 0 ? _m_lItemList[0] : null;
                // 用当前的列表刷新一次
                refresh(_m_showMsgItemListForRefresh);
                // 尽量还原当前显示的内容
                _fixCurViewport(curTopItem);
                // 刷新过后标记为不用再刷新了
                _m_bIsNeedRefreshList = false;
                // 还原之后再刷新一次
                frameRefresh();
            }
        }
        // 根据上一次顶端的item，修正当前的视口
        private void _fixCurViewport(_IALBasicMultiSizeLayoutItem _lastTopItem)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            if (_m_bSeeNewest)
            {
                wnd.scrollRect.verticalNormalizedPosition = 0;
            }
            else if (_lastTopItem != null)
            {
                if (itemContainer != null)
                {
                    itemContainer.localPosition += Mathf.Abs(_lastTopItem.layoutPosY - wnd.topPadding) * Vector3.up;
                    // todo:找一个下移之后不影响操作手感的方法
                    // 如果不停止拖动会有问题
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    // wnd.scrollRect.StopMovement();
                    wnd.scrollRect.OnEndDrag(pointerEventData);
                }
            }
        }

        // 刷新窗口
        private void _refreshWnd()
        {
            // 增加刷新序列号，表示之前的所有刷新内容都作废了
            _m_iRefreshSerialize = ALSerializeOpMgr.next();
            // 默认都认为是要显示最新消息
            _m_bSeeNewest = true;
            
            // 先清除上一次显示的内容
            refresh(null);

            if (_m_chatInfo != null)
            {
                // 存下刷新序列号
                int refreshSerialize = _m_iRefreshSerialize;
                // 获取最新的若干条消息
                _m_chatInfo.getHistoryList(wnd.initMsgCount, (_msgList) =>
                {
                    // 如果序列号发生变化，说明这次刷新已经被废弃了，就不做处理了
                    if (refreshSerialize != _m_iRefreshSerialize)
                        return;

                    // 如果列表是空就不做处理
                    if (_msgList == null || _msgList.Count <= 0)
                    {
                        _m_bIsInit = true;
                        return;
                    }
                    // 列表排列顺序是从旧到新，第一个消息是最老的
                    _m_oldestMsg = _msgList[0];
                    // 添加新消息
                    _addLastMsgItem(_initMsgListModify(_msgList));
                    // 初始化完成
                    _m_bIsInit = true;
                });
            }
        }
        // 当收到了新消息时
        private void _onReceiveMsg(MsgInfo _msgInfo)
        {
            // 如果是空就不处理了
            if (_msgInfo == null)
                return;

            // 如果之前还没有赋值最老消息，说明初始化的时候消息为空，这时
            if (_m_oldestMsg == null)
                _m_oldestMsg = _msgInfo;

            // 触发收到新消息时的自定义处理
            _beforeReceiveMsgModify(_msgInfo);
            // 添加新消息
            _addLastMsgItem(_msgInfo.detailInfo);
        }
        // 获取历史消息部分的成员变量
        // 是否已经拖到顶部了，避免一直触发请求
        private bool _m_bIsDragToTop = false;
        // 当前用来请求历史消息的最老消息
        private long _m_iTryToGetOldMsgId = -1;
        // 当滚动条进行滚动时
        private void _scrollerValueChg(Vector2 _chgValue)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            // 如果内容的高度大于显示高度，就进行是否要实时显示最新消息的判断
            if (contentHeight > viewportHeight)
            {
                // 计算出当前内容离开最底端的距离，如果小于配置距离，就认为用户需要一直看最新消息
                if (wnd.scrollRect.verticalNormalizedPosition * (contentHeight - viewportHeight) <= wnd.bottomCheckPosition)
                    _m_bSeeNewest = true;
                else
                    // 否则的话认为不需要显示最新消息
                    _m_bSeeNewest = false;
            }
            else
                // 如果视野足够显示所有的内容，就认为用户一直都想看最新消息
                _m_bSeeNewest = true;

            // 拖动到最顶部时触发
            if (wnd.scrollRect.verticalNormalizedPosition > 1)
            {
                // 如果已经拖动到顶部，或正在加载就不处理，或者还没有初始化完成也不处理
                if (_m_bIsDragToTop || !_m_bIsInit)
                    return;

                // 标记已经拖动到顶部了
                _m_bIsDragToTop = true;
                
                if (_m_chatInfo == null)
                    return;

                // 当有消息的时候取最老消息的数据来请求历史记录
                if (_m_oldestMsg != null)
                {
                    // 判断这个消息的消息id是否有被用来请求历史消息，有的话就返回不处理
                    if (_m_oldestMsg.msgId == _m_iTryToGetOldMsgId)
                        return;

                    // 赋值，使用这个消息id来请求历史消息
                    _m_iTryToGetOldMsgId = _m_oldestMsg.msgId;
                }
                else
                {
                    // 如果没有消息列表，就用0这个消息id来请求历史消息
                    if (_m_iTryToGetOldMsgId == 0)
                        return;

                    // 请求历史消息
                    _m_iTryToGetOldMsgId = 0;
                }

                // 存在刷新序列号
                int refreshSerialize = _m_iRefreshSerialize;
                // 根据最老的消息id来请求历史消息
                _m_chatInfo.getHistoryList(_m_iTryToGetOldMsgId, wnd.getHistoryMsgCount, (_msgList) =>
                {
                    // 如果序列号发生变化，说明这次刷新已经被废弃了，就不做处理了
                    if (refreshSerialize != _m_iRefreshSerialize)
                        return;

                    // 如果没有历史消息了，就直接不处理
                    if (_msgList == null || _msgList.Count <= 0)
                        return;

                    // 列表排列顺序是从旧到新，第一个消息是最老的
                    _m_oldestMsg = _msgList[0];

                    // 开始添加消息
                    _addFirstMsgItem(_historyMsgListModify(_msgList));
                });
            }
            else // 如果消息列表没有拉到最上面，就把这个标识赋值回来
            {
                _m_bIsDragToTop = false;
            }
        }

        // 当与聊天服务器重新连接时
        private void _onNetReconnected()
        {
            // 设置网络连接正常
            if (wnd != null)
                wnd.setNetState(true);

            // 重新刷新一次
            _refreshWnd();
        }
        // 当与聊天服务器断开连接时
        private void _onNetDisconnected()
        {
            // 设置网络连接中断
            if (wnd != null)
                wnd.setNetState(false);
        }
        private void _resetInfo()
        {
            // 释放所有的item
            for (int i = 0; i < _m_showMsgItemList.Count; i++)
            {
                // 处理这个item
                _IGUISubWndChatMsgListItem item = _m_showMsgItemList[i];
                if (item == null)
                    continue;
                
                // 对item进行释放
                _releaseItemWnd(item);
            }
            // 把这两个反应了当前刷新情况的列表清掉
            _m_showMsgItemList.Clear();
            _m_showMsgItemListForRefresh.Clear();
            // 清掉最老消息
            _m_oldestMsg = null;
            // 重置历史消息请求状态
            _m_iTryToGetOldMsgId = -1;
            _m_bIsDragToTop = false;
            // 数据清掉之后也意味着这里并没有初始化完成
            _m_bIsInit = false;
        }
        // 对item进行相关绑定和加载
        private void _bindItemWnd(_IGUISubWndChatMsgListItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            // 在item高度发生变化的时候，进行列表刷新
            _itemWnd.onHeightChg += setNeedForceRefreshList;
            // 让item开始加载自己的模板
            _itemWnd.loadTemplate();
        }
        // 对item进行相关解除绑定，是上面的反操作
        private void _releaseItemWnd(_IGUISubWndChatMsgListItem _itemWnd)
        {
            if (_itemWnd == null)
                return;
            
            // 解除事件绑定
            _itemWnd.onHeightChg -= setNeedForceRefreshList;
            // 告诉item这里不需要使用item的模板了
            _itemWnd.discardTemplate();
            // 如果解除之前在视口内，调用一次出视口的操作
            if (_itemWnd.isInViewport)
                _itemWnd.outViewport();
        }
    }
    /// <inheritdoc/>
    public class GUISubWndChatMsgList : GUISubWndChatMsgList<GUIMonoChatMsgList>
    {
        public GUISubWndChatMsgList(GUIMonoChatMsgList _wnd, [NotNull] GUICacheMgrChatMsgItem _itemCacheMgr) : base(_wnd, _itemCacheMgr)
        {
        }
    }
}