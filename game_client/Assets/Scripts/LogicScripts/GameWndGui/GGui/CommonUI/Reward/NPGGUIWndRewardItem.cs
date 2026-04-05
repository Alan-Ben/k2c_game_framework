using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;

namespace GOE
{
    public class NPGGUIWndRewardItem : _ATALUGUIBasicGridItemWnd<NPGGUIMonoCommonItem>
    {
        private NPGGUIWndCommonItem _m_wItemWnd;

        public NPGGUIWndRewardItem(NPGGUIMonoCommonItem _wnd)
            : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
            if(_m_wItemWnd != null)
                _m_wItemWnd.showWnd();
        }

        protected override void _onHideWnd()
        {

        }

        /// <summary>
        /// 重置grid item
        /// </summary>
        protected override void _resetGridItem()
        {
            if(_m_wItemWnd != null)
                _m_wItemWnd.resetWnd();
        }

        protected override void _onReset()
        {
            if(_m_wItemWnd != null)
                _m_wItemWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_wItemWnd != null)
                _m_wItemWnd.discard();
            _m_wItemWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if(wnd != null)
                _m_wItemWnd = new NPGGUIWndCommonItem(wnd);
        }

        public void setTaskRewardItem(CommonItemData _data)
        {
            if(_m_wItemWnd != null)
                _m_wItemWnd.showWnd(_data);
        }
    }
}
