using System;
using ALPackage;

namespace GOE
{
    public class NPMesDealer_OneBtn : _ANPMesDealer
    {
        /** 显示信息 */
        private string _m_sTitle;
        private string _m_sMes;
        private string _m_sBtnTxt;

        /** 点击的操作回调函数 */
        private Action _m_dClickDelegate;

        private bool _m_bNeedTransBk;

        public NPMesDealer_OneBtn(string _mes, string _btnTxt, Action _clickDelegate, bool _needTransBk, string _titleKey)
        {
            _m_sMes = _mes;
            _m_sBtnTxt = _btnTxt;

            _m_dClickDelegate = _clickDelegate;

            _m_bNeedTransBk = _needTransBk;

            _m_sTitle = TextTranslate.instance.getLanguage(_titleKey);
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            //显示信息
            NPPGUIWndMes_OneBtn.instance.loadAndShowWnd(_m_sMes, _m_sBtnTxt, _onClick, _m_sTitle);
        }

        protected override void _onHideMes()
        {
            _m_dClickDelegate = null;
            NPPGUIWndMes_OneBtn.instance.discard();
        }

        public override void dealCloseMes()
        {
            NPPGUIWndMes_OneBtn.instance.comfirmMes();
        }

        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;
            NPPGUIWndMes_OneBtn.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }

        /** 处理点击操作 */
        protected void _onClick()
        {
            if (null != _m_dClickDelegate)
                _m_dClickDelegate();
            _m_dClickDelegate = null;

            setDealerDone();
        }
    }
}
