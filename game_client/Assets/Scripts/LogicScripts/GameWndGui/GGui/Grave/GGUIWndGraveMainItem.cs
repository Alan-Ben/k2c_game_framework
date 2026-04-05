using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 杰出者主界面item
    /// </summary>
    public class GGUISubWndGraveMainItem : _ANPGGUIBasicSubWnd<GGUIMonoGraveMainItem>
    {
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        
        private NPGGuiWndTexture _m_titleIconWnd; // 称号图标
        // </AutoGen:WndDeclaration>
        private long _m_playerCid = 0;
        private long _m_titleId = 0;
        private NPCommonSimplePlayerInfo _m_playerInfo;

        private GraveTypeRefObj _m_graveTypeRef;
        
        bool _m_isDragging = false;
        
        public int typeId => _m_graveTypeRef?.id ?? 0;
        
        
        public GGUISubWndGraveMainItem([NotNull]GraveTypeRefObj _graveTypeRef, GGUIMonoGraveMainItem _wnd) : base(_wnd)
		{
            _m_graveTypeRef = _graveTypeRef;
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
                
                if (wnd.clickMono != null) 
                    wnd.clickMono.onClick -= onClickDetail;
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
            if (wnd.clickMono != null) 
                wnd.clickMono.onClick += onClickDetail;
        }


        public void setInfo(long _cid, long _titleId)
        {
            _m_playerCid = _cid;
            _m_titleId = _titleId;
            if (_m_playerCid > 0)
            {
                GCommon.reqPlayerInfo(_m_playerCid, (playerInfo) =>
                {
                    _m_playerInfo = playerInfo;
                    _refreshWnd();
                });
            }
            else
            {
                _m_playerInfo = null;
                _refreshWnd();
            }
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;


            if (_m_graveTypeRef != null)
            {
                string name = TextTranslate.instance.getLanguage(_m_graveTypeRef.type_name);
                ALUGUICommon.setLabelTxt(wnd.txtName, name);
                ALUGUICommon.setLabelTxt(wnd.txtMeshProName, name);

            }
            bool hasPlayer = _m_playerCid > 0;

            // <AutoGen:_refreshWnd>
            // <UserCode name="hasPlayerShowGos">
            // </UserCode>
            // <UserCode name="noPlayerShowGos">
            // </UserCode>
            // <UserCode name="txtName">
            // </UserCode>
            // <UserCode name="playerInfo">
            if(hasPlayer && _m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd();
                _m_playerInfoWnd.setPlayerInfo(_m_playerInfo);
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
                else
                {
                    _m_titleIconWnd.hideWnd();
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

        public void onClickDetail()
        {
            if (wnd != null) 
                _onClickbtnDetail(wnd.btnDetail);
        }
        // <AutoGen:Method>
        // 杰出者详情按钮点击事件
        private void _onClickbtnDetail(GameObject go)
        {
            if (_m_isDragging)
            {
                return;
            }
            // <UserCode name="btnDetail">
            if (wnd != null)
            {
                GGUIWndGravePlayerDetail.instance.setInfo(typeId, _m_playerInfo, _m_titleId);
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGravePlayerDetail.instance,  UINodeTagConst.C_GRAVE_PLAYER_DETAIL, null, null ,0);
            }
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}