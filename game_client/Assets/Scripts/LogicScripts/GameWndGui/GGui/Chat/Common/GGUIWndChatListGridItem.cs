using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using GS2GC.p004_PlayerOp;
using ChatPackage;
using ChatPackage.Internal;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace GOE
{
    //私聊会话item
    public class GGUIWndChatListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoChatListGridItem>
    {
        private GGUIWndChatChannleItem _m_channleItemWnd;
        
        //显示数据
        private _INPChatInfo _m_info;

        //点击回调
        private Action<_INPChatInfo> _m_clickAction;
        private NPGGUIWndPlayerIcon _m_playerIcon;
        private bool _m_isDrag = false;//是否在滑动中
        private bool _m_isRealDrag = false;//是否达到阈值开始真正拖拽
        private bool _m_isSkipDrag = false;//是否跳过拖拽（Y轴超过阈值）
        private ScrollRect _m_parentScrollRect;
        private Vector2 _m_startDragPos;
        private Tweener _m_moveTweener;
        private Vector2 _m_StartAnchoredPosition;
        private NPGGUIWndCommonToggleEx _m_togSelected;
        private bool _m_isSelected;
        private EChatChannleOperationStat _m_curOperationStat;
        private RectTransform _m_curTargettrans;
        public event Action<_INPChatInfo> onSelectedItem;//选中
        public event Action<_INPChatInfo> onDisSelectedItem;//取消选中

        #region override
        public GGUIWndChatListGridItem(GGUIMonoChatListGridItem _wnd, Action<_INPChatInfo> _clickAction, ScrollRect _scrollRect)
           : base(_wnd) 
        {
            _m_clickAction = _clickAction;
            _m_parentScrollRect = _scrollRect;
            
            initWnd();
        }

        public bool isSelected { get => _m_isSelected; }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            if (null != wnd.channleItem)
            {
                _m_channleItemWnd = new GGUIWndChatChannleItem(wnd.channleItem, null);
            }

            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }

            if (null != wnd.togSelected)
            {
                _m_togSelected = new NPGGUIWndCommonToggleEx(wnd.togSelected);
                _m_togSelected.clickDelegate += _clickSelectedToggle;
            }
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickChannleItem);
            ALUGUICommon.combineBeginDrag(wnd.dragObj, _beginDrag);
            ALUGUICommon.combineDrag(wnd.dragObj, _onDrag);
            ALUGUICommon.combineEndDrag(wnd.dragObj, _endDrag);
            ALUGUICommon.combineBtnClick(wnd.btnUp, _clickUp);
            ALUGUICommon.combineBtnClick(wnd.btnDisUp, _clickDisUp);
            ALUGUICommon.combineBtnClick(wnd.btnRemove, _clickRemove);
            _moveToRight(true);
            _m_isSelected = false;
            _m_curOperationStat = EChatChannleOperationStat.NORMAL;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_UP_TO_TOP_CHG, _refreshUpToTop);
        }

        protected override void _onHideWnd()
        {
            _m_channleItemWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_UP_TO_TOP_CHG, _refreshUpToTop);
        }

        protected override void _onReset()
        {
            _m_channleItemWnd?.resetWnd();
            _m_playerIcon?.resetWnd();
        }
        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            _m_channleItemWnd?.resetWnd();
            _m_playerIcon?.resetWnd();
            _m_isSelected = false;
            _m_curOperationStat = EChatChannleOperationStat.NORMAL;
        }

        protected override void _onDiscard()
        {
            if (_m_info != null)
            {
                _AChatInfo oldInfo = _m_info as _AChatInfo;
                oldInfo.onReceiveMsg -= onRecieveMsg;
                _m_info = null;
            }
            
            if (null == wnd)
                return;
            
            _m_channleItemWnd?.discard();
            _m_channleItemWnd = null;
            
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            
            _m_moveTweener?.Kill();
            _m_moveTweener = null;

            _m_curTargettrans = null;
        }
        #endregion

        // 初始化UI
        public void setInfo(_INPChatInfo _info)
        {
            if (null != _m_info)
            {
                _AChatInfo oldInfo = _m_info as _AChatInfo;
                oldInfo.onReceiveMsg -= onRecieveMsg;
            }
            _m_info = _info;
            _AChatInfo chatInfo = _m_info as _AChatInfo;
            if (null != chatInfo)
                chatInfo.onReceiveMsg += onRecieveMsg;

            _refresh();
        }

        private void _refresh()
        {
            if (null == _m_info)
                return;

            if (null != _m_channleItemWnd)
            {
                _m_channleItemWnd.showWnd();
                _m_channleItemWnd.setInfo(_m_info);
            }
            
            NPPrivateChatInfo privateChatInfo = _m_info as NPPrivateChatInfo;
            if (null != privateChatInfo)
            {
                _m_playerIcon?.showWnd();
                _m_playerIcon?.setPlayerInfoForChatPlayerCard(privateChatInfo.userInfo);
            }
            _refreshOperationStat();
            _refreshUpToTop();
        }

        //收到新消息
        private void onRecieveMsg(MsgInfo info)
        {
            _refresh();
        }

        private void _clickChannleItem(GameObject _)
        {
            if (_m_isDrag)
            { 
                return;
            }

            _moveToRight(true);
            if (null != _m_clickAction)
                _m_clickAction(_m_info);
        }

        /// <summary>
        /// 点击置顶
        /// </summary>
        /// <param name="_"></param>
        private void _clickUp(GameObject _)
        {
            NPPlayer.instance.chatComp.setUpToTop(_m_info,true);
            _moveToRight( true);
        }

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="_"></param>
        private void _clickDisUp(GameObject _)
        {
            NPPlayer.instance.chatComp.setUpToTop(_m_info,false);
            _moveToRight(true);
        }

        /// <summary>
        /// 移除聊天
        /// </summary>
        /// <param name="_"></param>
        private void _clickRemove(GameObject _)
        {
            //发消息
            NPPlayer.instance.chatComp.removePrivateChannel(_m_info);
            _moveToRight(true);
        }
        
        /// <summary>
        /// 点击选中toggle
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSelectedToggle(NPGGUIWndCommonToggleEx obj)
        {
            _setSelectedTogOn(!_m_isSelected);
            if (_m_isSelected)
            {
                onSelectedItem?.Invoke(_m_info);
            }
            else
            {
                onDisSelectedItem?.Invoke(_m_info);
            }
        }

        public void setSelected(bool _isSelected)
        {
            _setSelectedTogOn(_isSelected);
        }

        private void _setSelectedTogOn(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            _m_togSelected?.setSelected(_m_isSelected);
        }

        /// <summary>
        /// 显示选中状态
        /// </summary>
        private void _showSelected()
        {
            _m_curOperationStat = EChatChannleOperationStat.CAN_SELECTE;
            _setSelectedTogOn(false);
            _refreshOperationStat();
        }

        /// <summary>
        /// 取消显示选中状态
        /// </summary>
        private void _hideSelected()
        {
            _m_curOperationStat = EChatChannleOperationStat.NORMAL;
            _setSelectedTogOn(false);
            _refreshOperationStat();
        }

        /// <summary>
        /// 刷新操作状态显示
        /// </summary>
        private void _refreshOperationStat()
        {
            NPCommonEnumStatInfo<EChatChannleOperationStat>.setStat(wnd.statInfos, _m_curOperationStat);
        }

        /// <summary>
        /// 开始滑动
        /// </summary>
        /// <param name="_data"></param>
        private void _beginDrag(PointerEventData _data)
        {
            _m_isDrag = true;
            _m_isRealDrag = false;
            _m_isSkipDrag = false;
            _m_startDragPos = _data.position;
            _m_StartAnchoredPosition = wnd.dragMoveGo.anchoredPosition;
        }

        /// <summary>
        /// 滑动过程
        /// </summary>
        /// <param name="_data"></param>
        private void _onDrag(PointerEventData _data)
        {
            if(!_m_isDrag)
                return;

            if (_m_isSkipDrag)
                return;

            Vector2 endPos = _data.position;
            float distanceX = endPos.x - _m_startDragPos.x;
            float distanceY = endPos.y - _m_startDragPos.y;

            if (!_m_isRealDrag && !_m_isSkipDrag)
            {
                if (Mathf.Abs(distanceY) >= wnd.offsetThreshold)
                {
                    _m_isSkipDrag = true;
                    return;
                }

                if (Mathf.Abs(distanceX) >= wnd.offsetThreshold)
                {
                    _m_isRealDrag = true;
                    _realBeginDrag(_data);
                }
            }

            if (!_m_isRealDrag)
                return;

            //看是否需要有UI跟随滑动而有滑动效果
            if(null == wnd.dragMoveGo)
                return;
            float targetAnchoredPosX = wnd.movePosXRange.limitInRange(_m_StartAnchoredPosition.x + distanceX);
            wnd.dragMoveGo.anchoredPosition = new Vector2(targetAnchoredPosX, _m_StartAnchoredPosition.y);
        }

        /// <summary>
        /// 真正开始拖拽
        /// </summary>
        /// <param name="_data"></param>
        private void _realBeginDrag(PointerEventData _data)
        {
            if (_m_parentScrollRect != null)
                _m_parentScrollRect.enabled = false;
        }

        /// <summary>
        /// 结束滑动
        /// </summary>
        /// <param name="_data"></param>
        private void _endDrag(PointerEventData _data)
        {
            if(!_m_isDrag)
                return;
            Vector2 endPos = _data.position;
            float distance = endPos.x - _m_startDragPos.x;

            if (_m_isRealDrag)
            {
                _realEndDrag(_data);

                if (Mathf.Abs(distance) > wnd.swipeThreshold)
                {
                    if (distance > 0)//向右滑动
                    {
                        _moveToRight();
                    }
                    else //向左滑动
                    {
                        _moveToLeft();
                    }
                }
                else
                {
                   //滑动，不足阈值，恢复上一次的位置
                   _moveTo(_m_curTargettrans);
                }
            }
            else if (!_m_isSkipDrag)
            {
               //滑动，不足阈值，恢复上一次的位置
               _moveTo(_m_curTargettrans);
            }
            _m_isDrag = false;
            _m_isRealDrag = false;
            _m_isSkipDrag = false;
        }

        /// <summary>
        /// 真正结束拖拽
        /// </summary>
        /// <param name="_data"></param>
        private void _realEndDrag(PointerEventData _data)
        {
            if (_m_parentScrollRect != null)
                _m_parentScrollRect.enabled = true;
        }

        private void _moveToLeft(bool isGoreTime = false)
        {
            if (null == wnd)
                return;
            _moveTo(wnd.dragLeftPos, isGoreTime);
            ALUGUICommon.setGameObjEnable(wnd.showOnDragLeft, true);
            ALUGUICommon.setGameObjEnable(wnd.hideOnDragLeft, false);
        }
        private void _moveToRight(bool isGoreTime = false)
        {
            if (null == wnd)
                return;
            _moveTo(wnd.dragRightPos, isGoreTime);
            ALUGUICommon.setGameObjEnable(wnd.showOnDragLeft, false);
            ALUGUICommon.setGameObjEnable(wnd.hideOnDragLeft, true);
        }

        private void _moveTo(RectTransform _moveToPos,bool isGoreTime = false)
        {
            if (null == _moveToPos || null == wnd.dragMoveGo)
                return;
            _m_curTargettrans = _moveToPos;
            _m_moveTweener?.Kill();
            float curPosX = wnd.dragMoveGo.anchoredPosition.x;
            float targetPosX = _moveToPos.anchoredPosition.x;
            float time = Mathf.Abs(targetPosX - curPosX) / wnd.swipeVelocity;
            if (isGoreTime)
            {
                time = 0;
            }
            _m_moveTweener = DOTween.To(_value => curPosX = _value, curPosX, targetPosX, time).OnUpdate(() =>
            {
                wnd.dragMoveGo.anchoredPosition = new Vector2(curPosX, wnd.dragMoveGo.anchoredPosition.y);
            }).SetAutoKill(true);
        }
        
        /// <summary>
        /// 刷新置顶标志
        /// </summary>
        private void _refreshUpToTop()
        {
            bool isUpToTop = NPPlayer.instance.chatComp.getIsUpToTop(_m_info);
            ALUGUICommon.setGameObjEnable(wnd.showUpToTop, isUpToTop);
            ALUGUICommon.setGameObjEnable(wnd.hideUpToTop, !isUpToTop);
        }

        public void setSelectedTogOn(bool _isSelectedTogOn)
        {
            if(_isSelectedTogOn)
                _showSelected();
            else
                _hideSelected();
        }
    }
}
