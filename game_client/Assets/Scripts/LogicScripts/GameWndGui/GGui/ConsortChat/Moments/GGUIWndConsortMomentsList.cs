using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndConsortMomentsList : ALUGUIWndVerticalMultiSizeLayout<GGUIMonoConsortMomentsList>
    {
        // 现在在wnd列表里的所有消息item，包括了不在视口范围里的所有item
        [NotNull] private readonly List<GGUIWndConsortMomentsListItem> _m_showMsgItemList;
        // 这个列表将和上面的列表保持同步更新，内部的数据都是一样的，这个列表会专门传给refresh方法
        [NotNull] private readonly List<_IALBasicMultiSizeLayoutItem> _m_showMsgItemListForRefresh;
        
        // 当前wnd展示的chatInfo
        private _AConsortChatInfo _m_chatInfo;
        
        // 当前显示的最老的一条消息，因为_m_showMsgList可能包含有子类自定义添加进来的消息，所以这里额外记录一下最老的消息是啥
        private _AConsortChatMsgInfo _m_oldestMsg;
        // 刷新序列号
        private int _m_iRefreshSerialize;
        // 是否需要刷新列表
        private bool _m_bIsNeedRefreshList;
        // 是否初始化完成了
        private bool _m_bIsInit;
        // 是否保持显示最新消息
        private bool _m_bSeeNewest;

        private Action<long> _m_onSelectMomentComment;
        
        public GGUIWndConsortMomentsList(GGUIMonoConsortMomentsList _wnd, Action<long> _onSelectMomentComment) : base(_wnd)
        {
            // list初始化
            _m_showMsgItemList = new List<GGUIWndConsortMomentsListItem>();
            _m_showMsgItemListForRefresh = new List<_IALBasicMultiSizeLayoutItem>();
            _m_onSelectMomentComment = _onSelectMomentComment;
            initWnd();
        }

        /// <summary>
        /// 传入chatInfo就可以开始运作，这个方法可以传入null，用来清空wnd的数据
        /// </summary>
        public void setShowData(_AConsortChatInfo _chatInfo)
        {
            _AConsortChatInfo oldChatInfo = _m_chatInfo;
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
        
        protected override void _onShowWnd()
        {
            // 绑定ScrollRect的滚动事件
            if (wnd != null)
            {
                if (wnd.scrollRect != null && wnd.scrollRect.onValueChanged != null)
                    wnd.scrollRect.onValueChanged.AddListener(_scrollerValueChg);
            }
        }

        protected override void _onHideWnd()
        {
            // 增加刷新序列号，表示之前的所有刷新内容都作废了
            _m_iRefreshSerialize = ALSerializeOpMgr.next();
            // 解除绑定ScrollRect的滚动事件
            if (wnd != null && wnd.scrollRect != null && wnd.scrollRect.onValueChanged != null)
                wnd.scrollRect.onValueChanged.RemoveListener(_scrollerValueChg);
            if (_m_chatInfo != null)
            {
                _m_chatInfo.onReceiveMsg -= _onReceiveMsg;
                _m_chatInfo = null;
            }
            
            // 每次显示窗口都会进行重新刷新，所以这个清空所有和这次数据有关的内容
            _resetInfo();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }
        
        private void _resetInfo()
        {
            // 释放所有的item
            for (int i = 0; i < _m_showMsgItemList.Count; i++)
            {
                // 处理这个item
                var item = _m_showMsgItemList[i];
                if (item == null)
                    continue;
                
                // 对item进行释放
                _releaseItemWnd(item);
            }
            // 把这两个反应了当前刷新情况的列表清掉
            _m_showMsgItemList.Clear();
            _m_showMsgItemListForRefresh.Clear();
            // 数据清掉之后也意味着这里并没有初始化完成
            _m_bIsInit = false;
        }

        public void refreshShowedItems()
        {
            foreach (_IALBasicMultiSizeLayoutItem item in _m_lItemList)
            {
                GGUIWndConsortMomentsListItem momentsListItem = item as GGUIWndConsortMomentsListItem;
                if (momentsListItem == null)
                    continue;
                momentsListItem.refreshWnd();
            }
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
                    // 拿第一个消息给最老消息赋值，列表排列顺序是从旧到新
                    _m_oldestMsg = _msgList.GetFirst();
                    // 添加新消息
                    _addLastMsgItem(_initMsgListModify(_msgList));
                    // 初始化完成
                    _m_bIsInit = true;
                });
                if (wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.noMomentsItemShow, NPPlayer.instance.consortChatComp.consortMomentsInfo.getMomentsCount() <= 0);
            }
        }
        // 当收到了新消息时
        private void _onReceiveMsg(_AConsortChatMsgInfo _msgInfo)
        {
            // 如果是空就不处理了
            if (_msgInfo == null)
                return;

            // 如果之前还没有赋值最老消息，说明初始化的时候消息为空，这时
            if (_m_oldestMsg == null)
                _m_oldestMsg = _msgInfo;
            if(isShow)
                AccountSettingMgr.instance.consortMomentSaverMgr.setRead();

            _m_bSeeNewest = true;
            // 添加新消息
            _addLastMsgItem(_msgInfo);
            
            if (wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noMomentsItemShow, NPPlayer.instance.consortChatComp.consortMomentsInfo.getMomentsCount() <= 0);
        }
                
        /// <summary>
        /// 往最老消息方向也就是历史消息的方向，自定义添加消息
        /// </summary>
        /// <param name="_msgInfoList">要添加的消息列表</param>
        protected void _addFirstMsgItem(List<_AConsortChatMsgInfo> _msgInfoList)
        {
            // 添加消息到待处理列表
            for (int i = _msgInfoList.Count - 1; i >= 0; i--)
            {
                GGUIWndConsortMomentsListItem wnd = new GGUIWndConsortMomentsListItem(_msgInfoList[i], itemContainer, _m_onSelectMomentComment);
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
        /// <param name="_msgInfoList">要添加的消息列表</param>
        protected void _addLastMsgItem(List<_AConsortChatMsgInfo> _msgInfoList)
        {
            // 添加消息到待处理列表
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                GGUIWndConsortMomentsListItem wnd = new GGUIWndConsortMomentsListItem(_msgInfoList[i], itemContainer, _m_onSelectMomentComment);
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
        /// 往最新消息方向，自定义添加消息
        /// </summary>
        /// <param name="_msgInfo">要添加的消息</param>
        protected void _addLastMsgItem(_AConsortChatMsgInfo _msgInfo)
        {
            // 添加消息到待处理列表
            GGUIWndConsortMomentsListItem wnd = new GGUIWndConsortMomentsListItem(_msgInfo, itemContainer, _m_onSelectMomentComment);
            if (wnd == null)
                return;
                
            _m_showMsgItemList.Insert(0, wnd);
            _m_showMsgItemListForRefresh.Insert(0, wnd);
            _bindItemWnd(wnd);
            
            // 让列表进行刷新
            setNeedForceRefreshList();
        }
        /// <summary>
        /// 在wnd refresh的时候，第一次获取消息列表之后会调用这个方法，你可以重写这个方法，对消息列表做自定义的修改
        /// </summary>
        /// <param name="_msgInfoList">初始化时的消息列表，参数为列表的copy，可以随意修改</param>
        /// 
        protected virtual List<_AConsortChatMsgInfo> _initMsgListModify(List<_AConsortChatMsgInfo> _msgInfoList)
        {
            if (_msgInfoList == null)
                return null;

            List<_AConsortChatMsgInfo> itemDataList = new List<_AConsortChatMsgInfo>();
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                itemDataList.Add(_msgInfoList[i]);
            }

            return itemDataList;
        }
        /// <summary>
        /// 在wnd上拉到顶部，获取历史消息之后，会调用这个方法，你可以重写这个方法，对获取到的历史消息列表做自定义的修改
        /// </summary>
        /// <param name="_msgInfoList">历史消息列表，参数为列表的copy，可以随意修改</param>
        protected virtual List<_AConsortChatMsgInfo> _historyMsgListModify(List<_AConsortChatMsgInfo> _msgInfoList)
        {
            if (_msgInfoList == null)
                return null;

            List<_AConsortChatMsgInfo> itemDataList = new List<_AConsortChatMsgInfo>();
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                itemDataList.Add(_msgInfoList[i]);
            }

            return itemDataList;
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

            // 拖动到最顶部时触发
            if (wnd.scrollRect.verticalNormalizedPosition < 0)
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
                
                    // 拿第一个消息给最老消息赋值，列表排列顺序是从旧到新
                    _m_oldestMsg = _msgList.GetFirst();
                
                    // 开始添加消息
                    _addFirstMsgItem(_historyMsgListModify(_msgList));
                });
            }
            else // 如果消息列表没有拉到最上面，就把这个标识赋值回来
            {
                _m_bIsDragToTop = false;
            }
        }
        
        // 对item进行相关绑定和加载
        private void _bindItemWnd(GGUIWndConsortMomentsListItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            // 在item高度发生变化的时候，进行列表刷新
            _itemWnd.onHeightChg += setNeedForceRefreshList;
            // 让item开始加载自己的模板
            _itemWnd.loadTemplate();
        }
        // 对item进行相关解除绑定，是上面的反操作
        private void _releaseItemWnd(GGUIWndConsortMomentsListItem _itemWnd)
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
        
        // 根据上一次顶端的item，修正当前的视口
        private void _fixCurViewport(_IALBasicMultiSizeLayoutItem _lastTopItem)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            if (_m_bSeeNewest)
            {
                wnd.scrollRect.verticalNormalizedPosition = 1;
                _m_bSeeNewest = false;
            }
            // else if (_lastTopItem != null)
            // {
            //     if (itemContainer != null)
            //     {
            //         itemContainer.localPosition += Mathf.Abs(_lastTopItem.layoutPosY - wnd.topPadding) * Vector3.up;
            //         // todo:找一个下移之后不影响操作手感的方法
            //         // 如果不停止拖动会有问题
            //         PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            //         // wnd.scrollRect.StopMovement();
            //         wnd.scrollRect.OnEndDrag(pointerEventData);
            //     }
            // }
        }
        /// <summary>
        /// 让列表在下一帧时强制刷新一次
        /// </summary>
        public void setNeedForceRefreshList()
        {
            _m_bIsNeedRefreshList = true;
        }

        /// <summary>
        /// 设置互动消息tip的item高度
        /// </summary>
        public void setInteractionItemHeight(float _height)
        {
            if (wnd == null)
                return;

            // 设置顶部padding高度，用于展示互动消息tip按钮
            wnd.topPadding = _height;
            setNeedForceRefreshList();
        }
    }
}
