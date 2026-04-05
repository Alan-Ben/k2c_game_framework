using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 新晋者列表界面
    /// </summary>
    public class GGUIWndGraveNewProminentGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGraveNewProminentGridItem>
    {
        private GraveObj_NewInfo _m_info;
        // <AutoGen:WndDeclaration>
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        // </AutoGen:WndDeclaration>
        private GGUIWndGraveTitleGrid _m_titleGridWnd;
        
        public GGUIWndGraveNewProminentGridItem(GGUIMonoGraveNewProminentGridItem _wnd) : base(_wnd)
        {
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
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            // </AutoGen:_onDiscard>
            _m_titleGridWnd?.discard();
            _m_titleGridWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            // </AutoGen:_onWndInitDone>
            if (wnd.titleGrid != null)
                _m_titleGridWnd = new GGUIWndGraveTitleGrid(wnd.titleGrid);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GraveObj_NewInfo _data)
        {
            _m_info = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if(_m_info == null)
                return;
            if (_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd();
                GCommon.reqPlayerInfo(_m_info.getCid(), (playerInfo) =>
                {
                    _m_playerInfoWnd.setPlayerInfo(playerInfo);
                });
            }
            
            if (_m_titleGridWnd != null)
            {
                GCommon.reqPlayerTitleRecordList(_m_info.getCid(),(_list) =>
                {
                    _m_titleGridWnd?.showWnd();
                    _m_titleGridWnd?.showItemList(_list);
                });
              
            }
          
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
