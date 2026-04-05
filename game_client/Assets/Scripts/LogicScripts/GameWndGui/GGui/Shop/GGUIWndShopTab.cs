using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 商店页签
    public class GGUIWndShopTab : _ANPGGUIBasicSubWnd<GGUIMonoShopTab>
    {
        // 点击事件
        private Action<GGUIWndShopTab> _m_dClickEvent;
        private NPGGUIWndCommonTab _m_tab;
        private bool _m_isSelected = false;
        private int _m_lRedTipNum;

        public GGUIWndShopTab(GGUIMonoShopTab _wnd) : base(_wnd)
        {
            initWnd();
        }

        public Action<GGUIWndShopTab> onClickTab { get { return _m_dClickEvent; } set { _m_dClickEvent = value; } }
        public long shopMainRefId { get { return wnd.shopMainRefId; } }
        public int redTipNum { get { return _m_lRedTipNum; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_tab?.discard();
            _m_tab = null;
            _m_dClickEvent = default(Action<GGUIWndShopTab>);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_tab = new NPGGUIWndCommonTab(wnd.monoTab);
            _m_tab.clickDelegate += _onClickTab;
        }

        /// <summary>
        /// 点击页签
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickTab(bool _obj)
        {
            if (null != _m_dClickEvent)
            {
                _m_dClickEvent(this);
            }
        }

        public void playIsSelectAni(bool _isSelect)
        {
            if (null != wnd.tabAnimation)
            {
                if (_isSelect)
                    wnd.tabAnimation.ForcePlay(wnd.selectAnimationStr);
                else
                    wnd.tabAnimation.ForcePlay(wnd.unselectAnimationStr);
            }
        }

        public void playIsShowAni(bool _isShow)
        {
            if (null != wnd.tabAnimation)
            {
                if (_isShow)
                    wnd.tabAnimation.Play(wnd.showAnimationStr);
                else
                    wnd.tabAnimation.Play(wnd.hideAnimationStr);
            }
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void refreshWnd(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            _refreshWnd();
        }

        /// <summary>
        /// 显示隐藏小红点
        /// </summary>
        /// <param name="_num"></param>
        public void showRedTipNum(int _num)
        {
            if (_m_tab != null)
                _m_tab.showRedTipNum(_num);
            _m_lRedTipNum = _num;
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ShopMainRefObj shopMainRefObj = GRefdataCoreMgr.instance.shopMainRefCore.getRef(shopMainRefId);
            if (null == shopMainRefObj || !shopMainRefObj.unlock_cond.IsEnable(null))
            {
                ALUGUICommon.setGameObjEnable(wnd.goLockedHideList,false);
                ALUGUICommon.setGameObjEnable(wnd.goLockedShowList,true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goLockedShowList,false);
                ALUGUICommon.setGameObjEnable(wnd.goLockedHideList,true);
            }
            _m_tab?.setSelected(_m_isSelected);
        }
    }
}
