using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带勾选框的确认弹窗
    /// </summary>
    public class NPGGUIWndCommonToggleDialog_OneBtn : _ANPGGUIBasicWnd<NPGGUIMonoCommonToggleDialog_OneBtn>
    {
        private static NPGGUIWndCommonToggleDialog_OneBtn _g_instance;
        public static NPGGUIWndCommonToggleDialog_OneBtn instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonToggleDialog_OneBtn();
                return _g_instance;
            }
        }

        private Action<bool> _m_aOnClickBtn;//点击按钮
        private NPGGUIWndCommonToggleEx _m_wToggleWnd;//勾选框


        private NPGGUIWndCommonToggleDialog_OneBtn() : base(EALUIWndLayer.ADDITION)
        {

        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCommonToggleDialog_OneBtn.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonToggleDialog_OneBtn.objName; } }
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

            _m_aOnClickBtn = null;
        }

        protected override void _onDiscard()
        {
            if (_m_wToggleWnd != null)
            {
                _m_wToggleWnd.clickDelegate -= _onClickToggle;
                _m_wToggleWnd.discard();
            }
            _m_wToggleWnd = null;

            _m_aOnClickBtn = null;

            ALUGUICommon.uncombineBtnClick(wnd.btn, _onClickBtn);
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

            ALUGUICommon.combineBtnClick(wnd.btn, _onClickBtn);
        }


        #region 点击事件

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtn(GameObject _go)
        {
            if (_m_wToggleWnd == null)
                _m_aOnClickBtn?.Invoke(false);
            else
                _m_aOnClickBtn?.Invoke(_m_wToggleWnd.isOn);
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
        /// <param name="_txtBtn"></param>
        /// <param name="_onClickBtn"></param>
        /// <param name="_txtTitle"></param>
        /// <param name="_txtContent"></param>
        /// <param name="_txtToggleDesc"></param>
        public void setInfo(
            string _txtBtn,
            Action<bool> _onClickBtn,
            string _txtTitle,
            string _txtContent,
            string _txtToggleDesc)
        {
            if (wnd == null)
                return;

            _m_aOnClickBtn = _onClickBtn;
            ALUGUICommon.setLabelTxt(wnd.txtBtn, TextTranslate.instance.getLanguage(_txtBtn));
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_txtTitle));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_txtContent));
            ALUGUICommon.setLabelTxt(wnd.txtToggleDesc, TextTranslate.instance.getLanguage(_txtToggleDesc));
        }
    }
}
