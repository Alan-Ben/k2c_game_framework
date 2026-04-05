using System;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 带勾选框的两个按钮弹窗
    /// </summary>
    public class NPMesDealer_TwoBtn_Toggle : _ANPMesDealer
    {
        private string _m_sTxtLeftBtn;
        private Action<bool> _m_aClickLeftBtn;
        private string _m_sTxtRightBtn;
        private Action<bool> _m_aClickRightBtn;

        private string _m_sTxtTitle;
        private string _m_sTxtContent;
        private string _m_sTxtToggleDesc;

        private bool _m_bNeedTransBk;
        private Action _m_aTransBkClick;

        public NPMesDealer_TwoBtn_Toggle(
            string _txtLeftBtn, 
            Action<bool> _onClickLeftBtn, 
            string _txtRightBtn, 
            Action<bool> _onClickRightBtn,
            string _txtTitle, 
            string _txtContent, 
            string _txtToggleDesc,
            bool _needTransBk,
            Action _transBkClick)
        {
            _m_sTxtLeftBtn = _txtLeftBtn;
            _m_aClickLeftBtn = _onClickLeftBtn;
            _m_sTxtRightBtn = _txtRightBtn;
            _m_aClickRightBtn = _onClickRightBtn;

            _m_sTxtTitle = _txtTitle;
            _m_sTxtContent = _txtContent;
            _m_sTxtToggleDesc = _txtToggleDesc;

            _m_bNeedTransBk = _needTransBk;
            _m_aTransBkClick = _transBkClick;
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            NPGGUIWndCommonToggleDialog.instance.load(() => 
            {
                NPGGUIWndCommonToggleDialog.instance.setInfo(_m_sTxtLeftBtn, _onClickLeftBtn, _m_sTxtRightBtn, _onClickRightBtn, _m_sTxtTitle, _m_sTxtContent, _m_sTxtToggleDesc);
                NPGGUIWndCommonToggleDialog.instance.showWnd();
            });
        }

        protected override void _onHideMes()
        {
            NPGGUIWndCommonToggleDialog.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }

        protected override void _onDiscard()
        {
            NPGGUIWndCommonToggleDialog.instance.discard();
        }

        public override void dealClickTransBk()
        {
            _m_aTransBkClick?.Invoke();
        }

        private void _onClickLeftBtn(bool _toggle)
        {
            _m_aClickLeftBtn?.Invoke(_toggle);
            setDealerDone();
        }

        private void _onClickRightBtn(bool _toggle)
        {
            _m_aClickRightBtn?.Invoke(_toggle);
            setDealerDone();
        }
    }
}
