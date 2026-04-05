using System;

namespace GOE
{
    /// <summary>
    /// 消耗确认弹窗
    /// </summary>
    public class NPMesDealer_Cost : _ANPMesDealer
    {
        private NPCommonCostItem _m_costItem;
        private Action _m_aOnConfirm;
        private Action _m_aOnCancel;

        private string _m_sTxtTitle;
        private string _m_sTxtContent;

        private bool _m_bNeedTransBk;

        public NPMesDealer_Cost(
            NPCommonCostItem _costItem,
            Action _onConfirm,
            Action _onCancel,
            string _txtTitle,
            string _txtContent,
            bool _needTransBk)
        {
            _m_costItem = _costItem;

            _m_aOnConfirm = _onConfirm;
            _m_aOnCancel = _onCancel;

            _m_sTxtTitle = _txtTitle;
            _m_sTxtContent = _txtContent;

            _m_bNeedTransBk = _needTransBk;
        }

        public override bool needTransBk { get { return _m_bNeedTransBk; } }

        protected override void _onShowMes()
        {
            NPGGUIWndCommonCostDialog.instance.load(() =>
            {
                NPGGUIWndCommonCostDialog.instance.setInfo(_m_costItem, _onConfirm, _onCancel, _m_sTxtTitle, _m_sTxtContent);
                NPGGUIWndCommonCostDialog.instance.showWnd();
            });
        }

        protected override void _onHideMes()
        {
            NPGGUIWndCommonCostDialog.instance.discard();
        }

        protected override void _onDealerDone()
        {

        }

        protected override void _onDiscard()
        {
            NPGGUIWndCommonCostDialog.instance.discard();
        }

        private void _onConfirm()
        {
            _m_aOnConfirm?.Invoke();
            setDealerDone();
        }

        private void _onCancel()
        {
            _m_aOnCancel?.Invoke();
            setDealerDone();
        }
    }
}
