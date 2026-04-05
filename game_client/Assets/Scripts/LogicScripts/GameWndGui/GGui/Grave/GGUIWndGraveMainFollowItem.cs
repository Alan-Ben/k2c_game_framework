using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.DinnerEnum;
using NPEnum;
using GOE.FollowItem;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 杰出者大厅主界面跟随窗口，
    /// </summary>
    public class GGUIWndGraveMainFollowItem : _ATALGGUIWndCommonFollowItem<GGUIMonoGraveMainFollowItem>
    {
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        
        private NPGGuiWndTexture _m_titleIconWnd; // 称号图标
        // </AutoGen:WndDeclaration>
        private int _m_graveTypeId = 0; // 杰出者类型Id
        private long _m_playerCid = 0;
        private long _m_titleId = 0;
        private NPCommonSimplePlayerInfo _m_playerInfo;
        bool _m_isDragging = false;
        
        public GGUIWndGraveMainFollowItem(int _graveTypeId, long _cid, long _titleId, GGUIMonoGraveMainFollowItem _wnd) : base(_wnd)
        {
            _m_graveTypeId = _graveTypeId;
            _m_playerCid = _cid;
            _m_titleId = _titleId;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            _m_titleIconWnd?.hideWnd();
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            _m_titleIconWnd?.hideWnd();
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            _m_playerCid = 0;
            _m_playerInfo = null;
            // <AutoGen:_onDiscard>
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            _m_titleIconWnd?.discard();
            _m_titleIconWnd = null;
            // </AutoGen:_onDiscard>
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickbtnDetail);
                ALUGUICommon.uncombineBeginDrag(wnd.btnDetail, onBtnDetalBeginDrag);
                ALUGUICommon.uncombineEndDrag(wnd.btnDetail, onBtnDetailEndDrag);
            }
        }



        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            if (wnd.titleIcon != null)
                _m_titleIconWnd = new NPGGuiWndTexture(wnd.titleIcon);
            // </AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickbtnDetail);
            ALUGUICommon.combineBeginDrag(wnd.btnDetail, onBtnDetalBeginDrag);
            ALUGUICommon.combineEndDrag(wnd.btnDetail, onBtnDetailEndDrag);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            GraveTypeRefObj typeRefObj = GRefdataCoreMgr.instance.graveTypeRefCore.getRef(_m_graveTypeId);

            if (typeRefObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(typeRefObj.type_name));
            bool hasPlayer = _m_playerCid > 0;

            // <AutoGen:_refreshWnd>
            // <UserCode name="hasPlayerShowGos">
            // </UserCode>
            // <UserCode name="noPlayerShowGos">
            // </UserCode>
            // <UserCode name="txtName">
            // </UserCode>
            // <UserCode name="playerInfo">
           
            if(_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd();
                if (hasPlayer)
                {
                    GCommon.reqPlayerInfo(_m_playerCid, (playerInfo) =>
                    {
                        _m_playerInfo = playerInfo;
                        _m_playerInfoWnd.setPlayerInfo(_m_playerInfo);
                    });
                }
            }
            // </UserCode>
            
            // <UserCode name="titleIcon">
            if(_m_titleIconWnd != null)
            {
                PlayerTitleRefObj titleRef = GRefdataCoreMgr.instance.playerTitleRefCore.getRef(_m_titleId);
                if (titleRef != null)
                {
                    _m_titleIconWnd.setTexture(titleRef.icon);
                    _m_titleIconWnd.showWnd();
                }
            }
            // </UserCode>
            // </AutoGen:_refreshWnd>
            
            
            ALUGUICommon.setGameObjEnable(wnd.hasPlayerShowGos, hasPlayer);
            ALUGUICommon.setGameObjEnable(wnd.noPlayerShowGos, !hasPlayer);
        }
        
        private void onBtnDetailEndDrag(PointerEventData _obj)
        {
            _m_isDragging = false;
        }

        private void onBtnDetalBeginDrag(PointerEventData _obj)
        {
            _m_isDragging = true;
        }
        // <AutoGen:Method>
        // 杰出者详情按钮点击事件
        private void _onClickbtnDetail(GameObject go)
        {
            if (_m_isDragging)
            {
                Debug.Log("Dragging, ignore click event.");
                return;
            }
            // <UserCode name="btnDetail">
            if (wnd != null)
            {
                GGUIWndGravePlayerDetail.instance.setInfo(_m_graveTypeId, _m_playerInfo, _m_titleId);
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGravePlayerDetail.instance,  UINodeTagConst.C_GRAVE_PLAYER_DETAIL, null, null, 0);
            }
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}