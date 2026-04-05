using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带勾选框的确认弹窗
    /// </summary>
    public class GGUIWndChapterSkipToggleDialog : _ANPGGUIBasicWnd<GGUIMonoChapterSkipToggleDialog>
    {
        private static GGUIWndChapterSkipToggleDialog _g_instance;
        public static GGUIWndChapterSkipToggleDialog instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndChapterSkipToggleDialog();
                return _g_instance;
            }
        }

        private Action _m_cancelAction;
        private NPGGUIWndCommonToggleEx _m_wToggleWnd;//勾选框
        
        private GGUIWndChapterSkipToggleDialog() : base(EALUIWndLayer.ADDITION)
        {

        }


        protected override string _monoAssetPath { get { return GGUIMonoChapterSkipToggleDialog.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChapterSkipToggleDialog.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_wToggleWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_cancelAction)
                _m_cancelAction();
            _m_cancelAction = null;
        }

        protected override void _onReset()
        {
            _m_wToggleWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wToggleWnd != null)
            {
                _m_wToggleWnd.clickDelegate -= _onClickToggle;
                _m_wToggleWnd.discard();
            }
            _m_wToggleWnd = null;
            _m_cancelAction = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnLeft, _onClickLeftBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnRight, _onClickRightBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
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
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        
        public void setCancelAction(Action _closeAction)
        {
            _m_cancelAction = _closeAction;
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftBtn(GameObject _go)
        {
            //设置是否跳过剧情
            AccountSettingMgr.instance.accountSetting.setIsChapterQuickForwardSkipDialog(true);
            
            //设置进入是否提示
            if (null != _m_wToggleWnd && _m_wToggleWnd.isOn)
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CHAPTER_QUICK_FORWARD);

            _m_cancelAction = null;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_SKIP_TOGGLE_DIALOG);
            
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightBtn(GameObject _go)
        {
            //设置是否跳过剧情
            AccountSettingMgr.instance.accountSetting.setIsChapterQuickForwardSkipDialog(false);
            //设置进入是否提示
            if (null != _m_wToggleWnd && _m_wToggleWnd.isOn)
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CHAPTER_QUICK_FORWARD);
            
            _m_cancelAction = null;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_SKIP_TOGGLE_DIALOG);
        }
        

        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Chapter.C_CHAPTER_SKIP_TOGGLE_DIALOG);
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
    }
}
