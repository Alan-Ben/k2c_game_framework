using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndDinnerVideo : _ATALBasicUISubWnd<GGUIMonoDinnerVideo>
    {
        private GVideoClipIndex _m_videoClip;

        public GVideoClipIndex videoClip => _m_videoClip;

        private GGUIWndSimpleVideo _m_videoWnd;
        
        public GGUIWndDinnerVideo(GGUIMonoDinnerVideo _wnd) : base(_wnd)
        {
            initWnd();
        }
    
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_videoWnd?.hideWnd();
            _m_videoClip = null;
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            if (_m_videoWnd != null) 
                _m_videoWnd.discard();
            _m_videoWnd = null;
            _m_videoClip = null;

        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if(null != wnd.videoMono)
                _m_videoWnd = new GGUIWndSimpleVideo(wnd.videoMono);
        }

        public void showWndVideo(GVideoClipIndex _videoClip, Action _onClipPrepared = null)
        {
            _m_videoClip = _videoClip;
            if (wnd != null) 
                wnd.transform.SetAsLastSibling();
            if (_m_videoWnd == null)
            {
                _onClipPrepared?.Invoke();
                return;
            }
            _m_videoWnd.setVideoClip(_m_videoClip);
            _m_videoWnd.playVideoLoop(()=>
            {
                showWnd();
                _onClipPrepared?.Invoke();
            });
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
        }
    }
}