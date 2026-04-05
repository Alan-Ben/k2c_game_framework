using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndCustomAddList:_ANPGGUIBasicSubWnd<GGUIMonoCustomAddList>
    {
        public event Action onClickHide;
        public GGUIWndCustomAddList(GGUIMonoCustomAddList _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            onClickHide = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickClose);
        }

        private void _clickClose(GameObject obj)
        {
            hideWnd();
            onClickHide?.Invoke();
        }
        public void hideWndToAniEnd()
        {
            _sampleAnimation(wnd.hideAniName, 1);
            hideWndWithoutAni();
        }

        private void _sampleAnimation(string _animationName, float _normalizeTime)
        {
            if (wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_animationName))
                return;
            
            wnd.wndAnimation.Sample(_animationName, _normalizeTime);
        }
    }
}