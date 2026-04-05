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
    public class GGUIWndFriendListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoFriendListGridItem>
    {
        //显示数据
        private PlayerFriendItemData _m_info;

        //点击回调
        private NPGGUIWndPlayerIcon _m_playerIcon;
        private bool _m_isDrag = false;//是否在滑动中
        private Vector2 _m_startDragPos;
        private Tweener _m_moveTweener;
        private Vector2 _m_StartAnchoredPosition;
        private RectTransform _m_curTargettrans;

        #region override
        public GGUIWndFriendListGridItem(GGUIMonoFriendListGridItem _wnd)
           : base(_wnd) 
        {
            initWnd();
        }

        public PlayerFriendItemData info { get => _m_info; }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            
            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnChat, _clickChat);
            ALUGUICommon.combineBeginDrag(wnd.dragObj, _beginDrag);
            ALUGUICommon.combineDrag(wnd.dragObj, _onDrag);
            ALUGUICommon.combineEndDrag(wnd.dragObj, _endDrag);
            ALUGUICommon.combineBtnClick(wnd.btnRemove, _clickRemove);
            _moveTo(wnd.dragRightPos,true);
            _setIsDragIn(false);
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
            _m_playerIcon?.resetWnd();
        }
        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            _m_playerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;
            
            _m_playerIcon?.discard();
            _m_playerIcon = null;
            
            _m_moveTweener?.Kill();
            _m_moveTweener = null;

            _m_curTargettrans = null;
        }
        #endregion

        // 初始化UI
        public void setInfo(PlayerFriendItemData _info)
        {
            _m_info = _info;
            _refresh();
        }

        private void _refresh()
        {
            if (null == wnd)
                return;
            if (null == _m_info)
                return;
            
            _m_playerIcon?.showWnd();
            _m_playerIcon?.setPlayerInfoForChatPlayerCard(_m_info.playerInfo);

            bool isOnline = _m_info.playerInfo.isOnline;
            ALUGUICommon.setGameObjEnable(wnd.onlineShow, isOnline);
            ALUGUICommon.setGameObjEnable(wnd.onlineHide, !isOnline);
            ALUGUICommon.setLabelTxt(wnd.offLineTxt, TextTranslate.instance.getLanguage(
                TransKeyConst.friends_offline_time_str,
                TimeUtil.getPassTimeShow(_m_info.playerInfo.lastOfflineMs)));
            bool hasGuild = _m_info.playerInfo.guildId > 0;
            wnd.setHasGuildShow(hasGuild);
            ALUGUICommon.setLabelTxt(wnd.txtGuildName, TextTranslate.instance.getLanguage(TransKeyConst.common_parentheses_str, _m_info.playerInfo.guildSimpleName));
            
            _moveTo(wnd.dragRightPos, true);
            _setIsDragIn(false);
        }

        private void _clickChat(GameObject _)
        {
            if (_m_isDrag)
            { 
                return;
            }
            GCommon.jumpToPrivateChat(_m_info.cid);
        }

        /// <summary>
        /// 移除聊天
        /// </summary>
        /// <param name="_"></param>
        private void _clickRemove(GameObject _)
        {
            //发消息
            _moveTo(wnd.dragRightPos, true);
            _setIsDragIn(false);
            
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.friends_del_friend_tip_desc)
                ,TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                ,null
                ,TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                , () =>
                {
                    NPPlayer.instance.friendsComp.reqRemoveFriend(_m_info.cid);
                });
            
        }

        /// <summary>
        /// 开始滑动
        /// </summary>
        /// <param name="_data"></param>
        private void _beginDrag(PointerEventData _data)
        {
            _m_isDrag = true;
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
            Vector2 endPos = _data.position;
            float distance = endPos.x - _m_startDragPos.x;
            //看是否需要有UI跟随滑动而有滑动效果
            if(null == wnd.dragMoveGo)
                return;
            float targetAnchoredPosX = wnd.movePosXRange.limitInRange(_m_StartAnchoredPosition.x + distance);
            wnd.dragMoveGo.anchoredPosition = new Vector2(targetAnchoredPosX, _m_StartAnchoredPosition.y);
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
            if (Mathf.Abs(distance) > wnd.swipeThreshold)
            {
                if (distance > 0)//向右滑动
                {
                    _moveTo(wnd.dragRightPos);
                    _setIsDragIn(false);
                }
                else //向左滑动
                {
                    _moveTo(wnd.dragLeftPos);
                    _setIsDragIn(true);
                }
            }
            else
            {
               //滑动，不足阈值，恢复上一次的位置
               _moveTo(_m_curTargettrans);
            }
            _m_isDrag = false;
        }

        private void _setIsDragIn(bool _isIn)
        {
            ALUGUICommon.setGameObjEnable(wnd.dragInShow, _isIn);
            ALUGUICommon.setGameObjEnable(wnd.dragInHide, !_isIn);
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
        /// 强制执行显示动画
        /// </summary>
        /// <param name="_dealDone"></param>
        public void dealShowAni(Action _dealDone = null)
        {
            _dealAniAction(wnd.forceShowAniName, 0,_dealDone,null);   
        }

        /// <summary>
        /// 强制执行显示动画
        /// </summary>
        /// <param name="_dealDone"></param>
        public void dealShowWndAni(Action _dealDone = null)
        {
            _dealAniAction(wnd.showAniName, 0,_dealDone,null);   
        }

        /// <summary>
        /// 强制执行显示动画
        /// </summary>
        /// <param name="_dealDone"></param>
        public void dealHideAni(Action _dealDone = null)
        {
            _dealAniAction(wnd.forceHideAniName, 0,_dealDone,null);   
        }
    }
}
