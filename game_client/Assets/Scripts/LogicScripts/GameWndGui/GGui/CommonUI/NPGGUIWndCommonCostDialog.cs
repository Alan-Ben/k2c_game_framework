using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带勾选框的消耗确认弹窗
    /// </summary>
    public class NPGGUIWndCommonCostDialog : _ANPGGUIBasicWnd<NPGGUIMonoCommonCostDialog>
    {
        private static NPGGUIWndCommonCostDialog _g_instance;
        public static NPGGUIWndCommonCostDialog instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonCostDialog();
                return _g_instance;
            }
        }

        private Action _m_aOnConfirm;//点击确认回调
        private Action _m_aOnCancel;//点击取消回调
        private NPGGUIWndCommonItem _m_wCostItem;//item


        private NPGGUIWndCommonCostDialog() : base(EALUIWndLayer.ADDITION)
        {

        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCommonCostDialog.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonCostDialog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();

            _m_aOnConfirm = null;
            _m_aOnCancel = null;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            _m_aOnConfirm = null;
            _m_aOnCancel = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickBtnConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onClickBtnCancel);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //道具
            if (wnd.monoCostItem != null)
            {
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
            }

            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickBtnConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onClickBtnCancel);
        }


        #region 点击事件

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnConfirm(GameObject _go)
        {
            _m_aOnConfirm?.Invoke();
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnCancel(GameObject _go)
        {
            _m_aOnCancel?.Invoke();
        }

        #endregion


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_costItem"></param>
        /// <param name="_onConfirm"></param>
        /// <param name="_txtTitle"></param>
        public void setInfo(NPCommonCostItem _costItem, Action _onConfirm, Action _onCancel, string _txtTitle, string _txtContent)
        {
            if (wnd == null)
                return;

            _m_wCostItem?.setItem(_costItem);
            _m_aOnConfirm = _onConfirm;
            _m_aOnCancel = _onCancel;
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_txtTitle));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_txtContent));
        }
    }
}
