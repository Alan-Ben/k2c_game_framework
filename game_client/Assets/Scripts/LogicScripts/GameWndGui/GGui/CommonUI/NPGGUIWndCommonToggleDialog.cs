using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带勾选框的确认弹窗
    /// </summary>
    public class NPGGUIWndCommonToggleDialog : _ANPGGUIBasicWnd<NPGGUIMonoCommonToggleDialog>
    {
        private static NPGGUIWndCommonToggleDialog _g_instance;
        public static NPGGUIWndCommonToggleDialog instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonToggleDialog();
                return _g_instance;
            }
        }

        private Action<bool> _m_aOnClickLeftBtn;//点击左边按钮
        private Action<bool> _m_aOnClickRightBtn;//点击右边按钮
        private NPGGUIWndCommonToggleEx _m_wToggleWnd;//勾选框


        private NPGGUIWndCommonToggleDialog() : base(EALUIWndLayer.ADDITION)
        {

        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCommonToggleDialog.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonToggleDialog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_wToggleWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_wToggleWnd?.resetWnd();

            _m_aOnClickLeftBtn = null;
            _m_aOnClickRightBtn = null;
        }

        protected override void _onDiscard()
        {
            if (_m_wToggleWnd != null)
            {
                _m_wToggleWnd.clickDelegate -= _onClickToggle;
                _m_wToggleWnd.discard();
            }
            _m_wToggleWnd = null;

            _m_aOnClickLeftBtn = null;
            _m_aOnClickRightBtn = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _onClickLeftBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnRight, _onClickRightBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //勾选框
            if (wnd.monoToggle != null)
            {
                _m_wToggleWnd = new NPGGUIWndCommonToggleEx(wnd.monoToggle);
                _m_wToggleWnd.setSelected(false);
                _m_wToggleWnd.clickDelegate += _onClickToggle;
            }

            ALUGUICommon.combineBtnClick(wnd.btnLeft, _onClickLeftBtn);
            ALUGUICommon.combineBtnClick(wnd.btnRight, _onClickRightBtn);
        }


        #region 点击事件

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftBtn(GameObject _go)
        {
            if (_m_wToggleWnd == null)
                _m_aOnClickLeftBtn?.Invoke(false);
            else
                _m_aOnClickLeftBtn?.Invoke(_m_wToggleWnd.isOn);
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightBtn(GameObject _go)
        {
            if (_m_wToggleWnd == null)
                _m_aOnClickRightBtn?.Invoke(false);
            else
                _m_aOnClickRightBtn?.Invoke(_m_wToggleWnd.isOn);
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
        /// <param name="_txtLeftBtn"></param>
        /// <param name="_onClickLeftBtn"></param>
        /// <param name="_txtRightBtn"></param>
        /// <param name="_onClickRightBtn"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        public void setInfo(
            string _txtLeftBtn,
            Action<bool> _onClickLeftBtn,
            string _txtRightBtn,
            Action<bool> _onClickRightBtn,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc)
        {
            if (wnd == null)
                return;

            _m_aOnClickLeftBtn = _onClickLeftBtn;
            _m_aOnClickRightBtn = _onClickRightBtn;
            ALUGUICommon.setLabelTxt(wnd.txtLeftBtn, TextTranslate.instance.getLanguage(_txtLeftBtn));
            ALUGUICommon.setLabelTxt(wnd.txtRightBtn, TextTranslate.instance.getLanguage(_txtRightBtn));
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_txtTitle));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_txtContent));
            ALUGUICommon.setLabelTxt(wnd.txtToggleDesc, TextTranslate.instance.getLanguage(_txtToggleDesc));
        }
    }
}
