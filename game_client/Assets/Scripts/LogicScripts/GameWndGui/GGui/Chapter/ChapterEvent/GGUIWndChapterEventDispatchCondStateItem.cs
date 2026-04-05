using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndChapterEventDispatchCondStateItem : _ATALBasicUISubWnd<GGUIMonoChapterEventDispatchCondStateItem>
    {
        private bool _m_isSuc;
        public GGUIWndChapterEventDispatchCondStateItem(GGUIMonoChapterEventDispatchCondStateItem _wnd) : base(_wnd)
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
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        public void setInfo(bool _data)
        {
            _m_isSuc = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setGameObjEnable(wnd.hasInfoShow, _m_isSuc);
            ALUGUICommon.setGameObjEnable(wnd.hasInfoHide, !_m_isSuc);
        }
    }
}
