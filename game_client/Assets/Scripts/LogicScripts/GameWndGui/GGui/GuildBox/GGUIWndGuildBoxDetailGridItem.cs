using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱详情
    /// </summary>
    public class GGUIWndGuildBoxDetailGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildBoxDetailGridItem>
    {
        private NPCommonCostItem _m_costItem;
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonItem _m_itemWndWnd;  // 物品
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildBoxDetailGridItem(GGUIMonoGuildBoxDetailGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_itemWndWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_itemWndWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_itemWndWnd?.discard();
            _m_itemWndWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.itemWnd != null)
                _m_itemWndWnd = new NPGGUIWndCommonItem(wnd.itemWnd);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(NPCommonCostItem _data)
        {
            _m_costItem = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if(_m_itemWndWnd != null)
            {
                _m_itemWndWnd.showWnd();
                _m_itemWndWnd.setItem(_m_costItem);
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
