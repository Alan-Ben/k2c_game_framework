using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 显示冷却时间的按钮
    /// </summary>
    public class NPGGUIWndCommonCdBtn : _ATNPGGUIWndStateBtn<ENPCommonCdBtnStatus, NPGGUIMonoCommonCdBtn, NPGGUIWndCommonCdBtn>
    {
        private long _m_lCdEndTimeStamp = 0;//cd结束时间戳
        private ALCommonEnableTaskController _m_tRefreshTask;//刷新任务

        public NPGGUIWndCommonCdBtn(NPGGUIMonoCommonCdBtn _mono) : base(_mono)
        {
            initWnd();
        }

        public long cdEndTimeStamp { get { return _m_lCdEndTimeStamp; } }
        public long cdLeftTimeMs { get { return Math.Max(0, _m_lCdEndTimeStamp - FpsAndPingMgr.instance.serverTimeTag); } }

        protected override void _onReset()
        {
            base._onReset();
            _m_lCdEndTimeStamp = 0;
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_lCdEndTimeStamp = 0;
        }


        private void _initRefreshTask()
        {
            if (wnd == null)
                return;

            _discardRefreshTask();
            _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refresh, wnd.refreshInterval);
        }

        private void _discardRefreshTask()
        {
            _m_tRefreshTask.setDisable();
        }

        private void _refresh()
        {
            if (wnd == null)
            {
                _discardRefreshTask();
                return;
            }

            //刷新cd文本
            ALUGUICommon.setLabelTxt(wnd.txtCdLeftTime, TextTranslate.instance.getLanguage(wnd.leftTimeKey, TimeUtil.millisecondsToTime_Max(cdLeftTimeMs)));

            //cd结束改变状态
            if (cdLeftTimeMs <= 0)
            {
                setState(ENPCommonCdBtnStatus.NORMAL);
                _discardRefreshTask();
            }
        }


        #region 外部调用

        /// <summary>
        /// 设置cd结束时间戳
        /// </summary>
        /// <param name="_endTimeMs"></param>
        public void setCdEndTimeStamp(long _endTimeMs)
        {
            _m_lCdEndTimeStamp = _endTimeMs;
            setState(ENPCommonCdBtnStatus.CD);
            _initRefreshTask();
        }

        /// <summary>
        /// 设置cd剩余时间
        /// </summary>
        /// <param name="_leftTimeMs"></param>
        public void setCdLeftTimeMs(long _leftTimeMs)
        {
            setCdEndTimeStamp(FpsAndPingMgr.instance.serverTimeTag + _leftTimeMs);
        }

        #endregion
    }
}
