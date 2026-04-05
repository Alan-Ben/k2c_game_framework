using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带勾选框的消耗确认弹窗
    /// </summary>
    public class NPGGUIWndCommonCostToggleDialog : _ANPGGUIBasicWnd<NPGGUIMonoCommonCostToggleDialog>
    {
        private static NPGGUIWndCommonCostToggleDialog _g_instance;
        public static NPGGUIWndCommonCostToggleDialog instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonCostToggleDialog();
                return _g_instance;
            }
        }

        private Action<bool> _m_aOnConfirm;//点击确认回调
        private Action _m_aOnCancel;//点击取消回调
        private NPGGUIWndCommonItem _m_wCostItem;//item
        private NPGGUIWndCommonToggleEx _m_wToggleWnd;//勾选框


        private NPGGUIWndCommonCostToggleDialog() : base(EALUIWndLayer.ADDITION)
        {

        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCommonCostToggleDialog.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonCostToggleDialog.objName; } }
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

            _m_wToggleWnd?.resetWnd();

            _m_aOnConfirm = null;
            _m_aOnCancel = null;
        }

        protected override void _onDiscard()
        {
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (_m_wToggleWnd != null)
            {
                _m_wToggleWnd.clickDelegate -= _onClickToggle;
                _m_wToggleWnd.discard();
            }
            _m_wToggleWnd = null;

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

            //勾选框
            if (wnd.monoToggle != null)
            {
                _m_wToggleWnd = new NPGGUIWndCommonToggleEx(wnd.monoToggle);
                _m_wToggleWnd.setSelected(false);
                _m_wToggleWnd.clickDelegate += _onClickToggle;
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
            if (_m_wToggleWnd == null)
                _m_aOnConfirm?.Invoke(false);
            else
                _m_aOnConfirm?.Invoke(_m_wToggleWnd.isOn);
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnCancel(GameObject _go)
        {
            _m_aOnCancel?.Invoke();
        }

        /// <summary>
        /// 点击勾选框
        /// </summary>
        /// <param name="_toggleWnd"></param>
        private void _onClickToggle(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if (_toggleWnd == null)
                return;

            _toggleWnd.setSelected(!_toggleWnd.isOn);
        }

        #endregion


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_costItem"></param>
        /// <param name="_onConfirm"></param>
        /// <param name="_onCancel"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        public void setInfo(NPCommonCostItem _costItem, Action<bool> _onConfirm, Action _onCancel, string _txtTitle, string _txtContent, string _txtToggleDesc)
        {
            if (wnd == null)
                return;

            _m_wCostItem?.setItem(_costItem);
            _m_aOnConfirm = _onConfirm;
            _m_aOnCancel = _onCancel;
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_txtTitle));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_txtContent));
            ALUGUICommon.setLabelTxt(wnd.txtToggleDesc, TextTranslate.instance.getLanguage(_txtToggleDesc));
        }
    }
}
