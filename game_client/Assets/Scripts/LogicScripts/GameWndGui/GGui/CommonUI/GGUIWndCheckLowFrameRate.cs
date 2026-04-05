using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 低帧率检测提示弹窗
    /// </summary>
    public class GGUIWndCheckLowFrameRate : _ANPGGUIBasicWnd<NPGGUIMonoCheckLowFrameRate>
    {
        private static GGUIWndCheckLowFrameRate _g_instance;
        public static GGUIWndCheckLowFrameRate instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCheckLowFrameRate();
                return _g_instance;
            }
        }

        private Action _m_aOnClick;//点击事件
        private NPGGUIWndCommonToggleEx _m_wToggleWnd;//勾选框
        private ENPGameQuality _m_eTargetQuality;//目标画质

        private GGUIWndCheckLowFrameRate() : base(EALUIWndLayer.TOP)
        {
        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCheckLowFrameRate.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCheckLowFrameRate.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            //显示toggle
            _m_wToggleWnd?.showWnd();

            //刷新窗口
            _refreshWnd();

            //关闭esc回退
            QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_LOW_FRAME_RATE);
        }

        protected override void _onHideWnd()
        {
            //开启esc回退
            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_LOW_FRAME_RATE);
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

            if (wnd == null)
                return;

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

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onClick"></param>
        public void setInfo(Action _onClick)
        {
            _m_aOnClick = _onClick;

            //设置目标画质
            _m_eTargetQuality = ENPGameQuality.VERY_LOW;
            switch (GameSetting.instance.gameQuality)
            {
                case ENPGameQuality.LOW:
                    _m_eTargetQuality = ENPGameQuality.VERY_LOW;
                    break;
                case ENPGameQuality.NORMAL:
                    _m_eTargetQuality = ENPGameQuality.LOW;
                    break;
                case ENPGameQuality.HIGH:
                    _m_eTargetQuality = ENPGameQuality.NORMAL;
                    break;
                case ENPGameQuality.ULTRA:
                    _m_eTargetQuality = ENPGameQuality.HIGH;
                    break;
            }
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLeftBtn, TextTranslate.instance.getLanguage(TransKeyConst.cancel));
            ALUGUICommon.setLabelTxt(wnd.txtRightBtn, TextTranslate.instance.getLanguage(TransKeyConst.common_optimize_none));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(TransKeyConst.common_lowFrameRateTipDesc_none));
            ALUGUICommon.setLabelTxt(wnd.txtToggleDesc, TextTranslate.instance.getLanguage(TransKeyConst.no_longer_tips_today_none));
        }


        #region 点击事件

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickRightBtn(GameObject _go)
        {
            //勾选了今日不再提示
            if (_m_wToggleWnd != null && _m_wToggleWnd.isOn)
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CHECK_LOW_FRAME_RATE);

            //发送埋点
            GCommon.sendStepReport(TraceConst.LOW_FRAME_RATE_TIP.setMarkParam((int)FpsAndPingMgr.instance.fpsValue, GameSetting.instance.gameQuality, _m_eTargetQuality, true));

            //调低一档画质
            GameSetting.instance.gameQuality = _m_eTargetQuality;

            _m_aOnClick?.Invoke();
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLeftBtn(GameObject _go)
        {
            //勾选了今日不再提示
            if (_m_wToggleWnd != null && _m_wToggleWnd.isOn)
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CHECK_LOW_FRAME_RATE);

            //发送埋点
            GCommon.sendStepReport(TraceConst.LOW_FRAME_RATE_TIP.setMarkParam((int)FpsAndPingMgr.instance.fpsValue, GameSetting.instance.gameQuality, _m_eTargetQuality, false));

            _m_aOnClick?.Invoke();
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
    }
}
