using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 新晋者列表界面
    /// </summary>
    public class GGUIWndGraveTitleGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGraveTitleGridItem>
    {
        
        // <AutoGen:WndDeclaration>
        private NPGGuiWndTexture _m_titleIconWnd; // 称号图标
        // </AutoGen:WndDeclaration>
        private long _m_titleId; // 称号ID
        
        public GGUIWndGraveTitleGridItem(GGUIMonoGraveTitleGridItem _wnd) : base(_wnd)
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
            // <AutoGen:_onDiscard>
            _m_titleIconWnd?.discard();
            _m_titleIconWnd = null;
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.titleIcon != null)
                _m_titleIconWnd = new NPGGuiWndTexture(wnd.titleIcon);
            // </AutoGen:_onWndInitDone>
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(long _titleId)
        {
            _m_titleId = _titleId;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
                
                // <AutoGen:_refreshWnd>
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
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
