using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelNpcItem: _ANPGGUIBasicSubWnd<GGUIMonoTravelNpcItem>
    {
        private NPNPCRefObj _m_npcRef;
        private NPGGuiWndTexture _m_iconWnd;
        private bool _m_isLeave;
        private bool _m_isUnlock = true;


        public GGUIWndTravelNpcItem(GGUIMonoTravelNpcItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (null != wnd.icon)
            {
                _m_iconWnd = new NPGGuiWndTexture(wnd.icon);
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_npcRef"></param>
        /// <param name="_isLeave"></param>
        public void setInfo(NPNPCRefObj _npcRef, bool _isLeave = false, bool _isUnlock = true)
        {
            _m_npcRef = _npcRef;
            _m_isLeave = _isLeave;
            _m_isUnlock = _isUnlock;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            if(null == _m_npcRef)
                return;
            
            if (null != _m_iconWnd)
            {
                _m_iconWnd.showWnd();
                _m_iconWnd.setTexture(_m_npcRef.npcIcon);
            }

            ALUGUICommon.setLabelTxt(wnd.name, TextTranslate.instance.getLanguage(_m_npcRef.npcName));
            ALUGUICommon.setGameObjEnable(wnd.leaveShowList, _m_isLeave);
            _refreshUnlock();
        }

        private void _refreshUnlock()
        {
            if (!_m_isUnlock)
            {
                GGameCommonInfo.grayImage(wnd.lockGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);
            }
        }
    }
}