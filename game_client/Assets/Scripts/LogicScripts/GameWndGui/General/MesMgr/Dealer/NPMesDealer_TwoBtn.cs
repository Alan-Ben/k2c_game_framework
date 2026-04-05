using System;

using ALPackage;

namespace GOE
{
    public class NPMesDealer_TwoBtn : _ANPMesDealer
    {
        /** 显示信息 */
        private string _m_sTitle;
        private string _m_sMes;

        private string _m_sLeftBtnTxt;
        private Action _m_dLeftBtnDelegate;

        private string _m_sRightBtnTxt;
        private Action _m_dRightBtnDelegate;

        private Action _m_dCancelDelegate;

        private bool _m_bNeedTransBk;

        public NPMesDealer_TwoBtn(string _mes, string _leftBtnTxt, Action _clickLeftBtnDelegate, string _rightBtnTxt, Action _clickRightBtnDelegate, Action _cancelDelegate, bool _needTransBk, string _titleKey)
        {
            _m_sMes = _mes;
            _m_sLeftBtnTxt = _leftBtnTxt;
            _m_sRightBtnTxt = _rightBtnTxt;

            _m_dLeftBtnDelegate = _clickLeftBtnDelegate;
            _m_dRightBtnDelegate = _clickRightBtnDelegate;

            _m_dCancelDelegate = _cancelDelegate;

            _m_bNeedTransBk = _needTransBk;
            _m_sTitle = TextTranslate.instance.getLanguage(_titleKey);
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            //显示信息
            NPPGUIWndMes_TwoBtn.instance.loadAndShowWnd(_m_sMes, _m_sLeftBtnTxt, _onClickLeft, _m_sRightBtnTxt, _onClickRight, _m_sTitle);
        }

        protected override void _onHideMes()
        {
            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
            _m_dCancelDelegate = null;
            NPPGUIWndMes_TwoBtn.instance.discard();
        }

        public override void dealCloseMes()
        {
            NPPGUIWndMes_TwoBtn.instance.comfirmMes(_m_dCancelDelegate);
        }

        /**************
         * 清掉该信息显示
         **/
        protected override void _onDiscard()
        {
            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
            _m_dCancelDelegate = null;
            NPPGUIWndMes_TwoBtn.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }

        /** 处理点击操作 */
        protected void _onClickLeft()
        {
            if (null != _m_dLeftBtnDelegate)
                _m_dLeftBtnDelegate();

            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
            _m_dCancelDelegate = null;

            setDealerDone();
        }

        protected void _onClickRight()
        {
            if (null != _m_dRightBtnDelegate)
                _m_dRightBtnDelegate();

            _m_dLeftBtnDelegate = null;
            _m_dRightBtnDelegate = null;
            _m_dCancelDelegate = null;

            setDealerDone();
        }

    }
}
