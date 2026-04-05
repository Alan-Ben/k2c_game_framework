using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndChapterEventDispatchCondItem : _ATALBasicUISubWnd<GGUIMonoChapterEventDispatchCondItem>
    {
        private ChapterEventDispatchConditionInfo _m_data;
        public GGUIWndChapterEventDispatchCondItem(GGUIMonoChapterEventDispatchCondItem _wnd) : base(_wnd)
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

        public void setInfo(ChapterEventDispatchConditionInfo _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_data)
                return;
            if (_m_data.refObj != null)
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_data.refObj.desc, _m_data.refObj.desc_args));
        }
    }
}
