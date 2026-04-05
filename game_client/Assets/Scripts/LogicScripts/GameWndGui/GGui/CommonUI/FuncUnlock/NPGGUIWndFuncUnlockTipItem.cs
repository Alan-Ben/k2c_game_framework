using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndFuncUnlockTipItem : _ATALBasicUISubWnd<NPGGUIMonoFuncUnlockTipItem>
    {
        private FuncUnlockInfo _m_funcUnlockInfo;

        private NPAccessInfo _m_accessInfo;
        private NPGGuiWndTexture _m_wIconWnd;

        public NPGGUIWndFuncUnlockTipItem(NPGGUIMonoFuncUnlockTipItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refresh();
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_wIconWnd != null) 
                _m_wIconWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wIconWnd != null) 
                _m_wIconWnd.discard();
            _m_wIconWnd = null;

            _m_funcUnlockInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //图标
            if (wnd.texIcon != null)
            {
                _m_wIconWnd = new NPGGuiWndTexture(wnd.texIcon);
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(FuncUnlockInfo _funcUnlock)
        {
            if(null == _funcUnlock)
                return;

            _m_funcUnlockInfo = _funcUnlock;

            _refresh();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refresh()
        {
            if(null == wnd)
                return;
            
            if(null == _m_funcUnlockInfo)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_funcUnlockInfo.funcName);

            if (null != _m_wIconWnd)
            {
                _m_wIconWnd.setTexture(_m_funcUnlockInfo.texIcon);
            }
        }
      
    }
}
