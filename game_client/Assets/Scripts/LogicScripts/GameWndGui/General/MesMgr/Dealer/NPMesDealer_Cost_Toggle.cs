using System;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 带勾选框的消耗确认弹窗
    /// </summary>
    public class NPMesDealer_Cost_Toggle : _ANPMesDealer
    {
        private NPCommonCostItem _m_costItem;
        private Action<bool> _m_aOnConfirm;
        private Action _m_aOnCancel;

        private string _m_sTxtTitle;
        private string _m_sTxtContent;
        private string _m_sTxtToggleDesc;

        private bool _m_bNeedTransBk;
        private bool _m_isConfirm;

        public NPMesDealer_Cost_Toggle(
            NPCommonCostItem _costItem,
            Action<bool> _onConfirm,
            Action _onCancel,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc,
            bool _needTransBk
            )
        {
            _m_costItem = _costItem;

            _m_aOnConfirm = _onConfirm;
            _m_aOnCancel = _onCancel;

            _m_sTxtTitle = _txtTitle;
            _m_sTxtContent = _txtContent;
            _m_sTxtToggleDesc = _txtToggleDesc;

            _m_bNeedTransBk = _needTransBk;

            _m_isConfirm = false;
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            NPGGUIWndCommonCostToggleDialog.instance.load(() =>
            {
                NPGGUIWndCommonCostToggleDialog.instance.setInfo(_m_costItem, _onConfirm, _onCancel, _m_sTxtTitle, _m_sTxtContent, _m_sTxtToggleDesc);
                NPGGUIWndCommonCostToggleDialog.instance.showWnd();
            });
        }

        protected override void _onHideMes()
        {
            NPGGUIWndCommonCostToggleDialog.instance.discard();
        }

        protected override void _onDealerDone()
        {
            if (!_m_isConfirm)
                _m_aOnCancel?.Invoke();

            _m_isConfirm = false;
        }

        protected override void _onDiscard()
        {
            NPGGUIWndCommonCostToggleDialog.instance.discard();
        }

        private void _onConfirm(bool _toggle)
        {
            _m_isConfirm = true;
            _m_aOnConfirm?.Invoke(_toggle);
            setDealerDone();
        }

        private void _onCancel()
        {
            _m_isConfirm = false;
            setDealerDone();
        }
    }
}
