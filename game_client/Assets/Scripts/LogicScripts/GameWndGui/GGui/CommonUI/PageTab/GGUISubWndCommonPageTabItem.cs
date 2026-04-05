using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndCommonPageTabItem<T> : _ANPGGUIBasicSubWnd<GGUIMonoCommonPageTabItem<T>>
        where T : Enum
    {
        private GGUIMonoCommonPageTabItemShowState _m_curShowState;

        private Func<T, _AALBasicLoadUIWndBasicClass> _m_createPageWndFunc;
        private _AALBasicLoadUIWndBasicClass _m_pageWnd;
        
        public GGUISubWndCommonPageTabItem(GGUIMonoCommonPageTabItem<T> _wnd, Func<T, _AALBasicLoadUIWndBasicClass> _createPageWndFunc) : base(_wnd)
        {
            _m_createPageWndFunc = _createPageWndFunc;
            initWnd();
        }

        public event Action<GGUISubWndCommonPageTabItem<T>> onClick;

        protected override void _onShowWnd()
        {
            if (_m_curShowState == GGUIMonoCommonPageTabItemShowState.SELECTED)
                _m_pageWnd?.regLoadDoneDelegate(_m_pageWnd.showWnd);
        }

        protected override void _onHideWnd()
        {
            if (_m_curShowState == GGUIMonoCommonPageTabItemShowState.SELECTED)
                _m_pageWnd?.regLoadDoneDelegate(_m_pageWnd.hideWnd);
        }

        protected override void _onReset()
        {
            _m_pageWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_pageWnd?.discard();
            _m_pageWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelectClicked);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelectClicked);
            setSelectShow(_m_curShowState);
        }

        public void setSelectShow(GGUIMonoCommonPageTabItemShowState _showState)
        {
            _m_curShowState = _showState;
            
            if (wnd == null)
                return;

            wnd.selectShow.setShowData(_showState);
            switch (_showState)
            {
                case GGUIMonoCommonPageTabItemShowState.SELECTED:
                    if (_m_pageWnd == null)
                    {
                        _m_pageWnd = _m_createPageWndFunc?.Invoke(wnd.tabType);
                        _m_pageWnd?.load();
                    }
                    if (_m_bIsShow)
                        _m_pageWnd?.regLoadDoneDelegate(_m_pageWnd.showWnd);
                    break;
                case GGUIMonoCommonPageTabItemShowState.UNSELECTED:
                    if (_m_bIsShow)
                        _m_pageWnd?.regLoadDoneDelegate(_m_pageWnd.hideWnd);
                    break;
            }
        }

        /// <summary>
        /// 设置红点显隐
        /// </summary>
        /// <param name="_isShow"></param>
        public void setRedTipShow(bool _isShow)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, _isShow);
        }

        public _AALBasicLoadUIWndBasicClass getPageWnd()
        {
            return _m_pageWnd;
        }

        private void _onBtnSelectClicked(GameObject _)
        {
            onClick?.Invoke(this);
        }
    }
}