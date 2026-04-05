using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 物品概率详情界面
    /// </summary>
    public class GGUIWndItemPercentDetailGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoItemPercentDetailGridItem>
    {
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonItem _m_itemWndWnd;  // 物品
        // </AutoGen:WndDeclaration>
        private NPCommonCostItem _m_item;
        private int _m_iWeight;
        private int _m_iTotalWeight;
        public GGUIWndItemPercentDetailGridItem(GGUIMonoItemPercentDetailGridItem _wnd) : base(_wnd)
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
        public void setInfo(NPCommonCostItem _item, int _weight, int _totalWeight)
        {
            _m_item = _item;
            _m_iWeight = _weight;
            _m_iTotalWeight = _totalWeight;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.percentTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (1.0f * _m_iWeight / _m_iTotalWeight * 100).ToString("F2")));
            if(_m_itemWndWnd != null)
            {
                _m_itemWndWnd.showWnd();
                _m_itemWndWnd.setItem(_m_item);
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
