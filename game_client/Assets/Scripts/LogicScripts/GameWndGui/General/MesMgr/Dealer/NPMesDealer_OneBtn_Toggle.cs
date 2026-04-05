using System;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 带勾选框的单个按钮弹窗
    /// </summary>
    public class NPMesDealer_OneBtn_Toggle : _ANPMesDealer
    {
        private string _m_sTxtBtn;
        private Action<bool> _m_aClickBtn;

        private string _m_sTxtTitle;
        private string _m_sTxtContent;
        private string _m_sTxtToggleDesc;

        private bool _m_bNeedTransBk;

        public NPMesDealer_OneBtn_Toggle(
            string _txtBtn,
            Action<bool> _onClickBtn,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc,
            bool _needTransBk)
        {
            _m_sTxtBtn = _txtBtn;
            _m_aClickBtn = _onClickBtn;

            _m_sTxtTitle = _txtTitle;
            _m_sTxtContent = _txtContent;
            _m_sTxtToggleDesc = _txtToggleDesc;

            _m_bNeedTransBk = _needTransBk;
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            NPGGUIWndCommonToggleDialog_OneBtn.instance.load(() =>
            {
                NPGGUIWndCommonToggleDialog_OneBtn.instance.setInfo(_m_sTxtBtn, _onClickBtn, _m_sTxtTitle, _m_sTxtContent, _m_sTxtToggleDesc);
                NPGGUIWndCommonToggleDialog_OneBtn.instance.showWnd();
            });
        }

        protected override void _onHideMes()
        {
            NPGGUIWndCommonToggleDialog_OneBtn.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }

        protected override void _onDiscard()
        {
            NPGGUIWndCommonToggleDialog_OneBtn.instance.discard();
        }

        private void _onClickBtn(bool _toggle)
        {
            _m_aClickBtn?.Invoke(_toggle);
            setDealerDone();
        }
    }
}
