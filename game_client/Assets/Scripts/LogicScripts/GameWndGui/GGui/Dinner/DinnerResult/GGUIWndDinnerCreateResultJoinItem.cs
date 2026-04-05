using System;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndDinnerCreateResultJoinItem : _ATALBasicUISubWnd<GGUIMonoDinnerCreateResultJoinItem>
    {
        private int _m_index;
        GDinnerGuestInfo _m_data;
        public GGUIWndDinnerCreateResultJoinItem(GGUIMonoDinnerCreateResultJoinItem _wnd) : base(_wnd)
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

        public void setInfo(GDinnerGuestInfo _data, int _index)
        {
            _m_data = _data;
            _m_index = _index;
            if (_m_data != null)
                _m_data.regDetailInfo((info) => { _refreshWnd(); });
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_data)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_index);
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.name);
            ALUGUICommon.setLabelTxt(wnd.txtScore, _m_data.score);
        }
    }
}
