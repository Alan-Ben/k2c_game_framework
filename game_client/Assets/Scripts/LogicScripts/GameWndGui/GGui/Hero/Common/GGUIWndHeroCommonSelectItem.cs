using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndHeroCommonSelectItem : _ANPGGUIBasicGridItemWnd<GGUIMonoHeroCommonSelectItem>
    {
        private readonly Action<_IHeroCardShow> _m_onItemClick;

        private GGUIWndHeroCommonCardItem _m_heroCard;
        private _IHeroCardShow _m_heroInfo;
        private int _m_selectIndex;

        public GGUIWndHeroCommonSelectItem(GGUIMonoHeroCommonSelectItem _wnd, Action<_IHeroCardShow> _onItemClick)  : base(_wnd)
        {
            _m_onItemClick = _onItemClick;

            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_heroCard?.showWnd();
            
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_heroCard?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_heroCard?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (_m_heroCard != null)
            {
                _m_heroCard.discard();
                _m_heroCard.ClickAction -= _onHeroClick;
                _m_heroCard = null;
            }
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroCard != null)
            {
                _m_heroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);
                _m_heroCard.ClickAction = _onHeroClick;
            }
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void refreshWnd(_IHeroCardShow _heroInfo, int _selectIndex)
        {
            _m_heroInfo = _heroInfo;
            _m_selectIndex = _selectIndex;
            refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void refreshWnd()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtSelectNum, _m_selectIndex > 0 ? _m_selectIndex.ToString() : "");
            ALUGUICommon.setGameObjEnable(wnd.listSelectShow, _m_selectIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.listSelectHide, _m_selectIndex <= 0);
            _m_heroCard?.setInfo(_m_heroInfo);

        }
        
        private void _onHeroClick(GGUIWndHeroCommonCardItem _item)
        {
            if (wnd == null || _item == null)
                return;
            
            HeroInfo heroInfo = null;
            if (_item.heroCardShow is HeroInfo)
                heroInfo = _item.heroCardShow as HeroInfo;
            else if (_item.heroCardShow is HeroCardShowInfo showInfo)
                heroInfo = showInfo.heroInfo;

            _m_onItemClick?.Invoke(heroInfo);
        }
    }
}
