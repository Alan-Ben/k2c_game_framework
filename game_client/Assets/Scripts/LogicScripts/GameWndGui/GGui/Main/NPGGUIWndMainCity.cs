using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 主城界面
    /// </summary>
    public class NPGGUIWndMainCity : _ANPGGUIBasicWnd<NPGGUIMonoMainCity>
    {
        private static NPGGUIWndMainCity _g_instance = new NPGGUIWndMainCity();
        public static NPGGUIWndMainCity instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndMainCity();
                return _g_instance;
            }
        }

        //显示序列
        private long _m_lShowSerialize;
        //是否已经发送帧率埋点
        private bool _m_bIsSendFPSReport;

        public NPGGUIWndMainCity() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoMainCity.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoMainCity.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.SendMsg(WinMsgType.MAIN_CITY_WND_SHOW);

            //发送当前帧率值埋点
            _dealSendFPSReport();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_bIsSendFPSReport = false;
        }


        //发送当前帧率值埋点
        private void _dealSendFPSReport()
        {
            //已发送不再发送
            if (_m_bIsSendFPSReport)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lShowSerialize;

            //延迟5秒后如果还在当前界面就发送埋点
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (!isShow || serialize != _m_lShowSerialize || _m_bIsSendFPSReport)
                    return;

                _m_bIsSendFPSReport = true;
                //发送卧室帧率值埋点
                GCommon.sendStepReport(TraceConst.CITY_FPS.setMark($"{(int)FpsAndPingMgr.instance.fpsValue}"));
            }, 5f);
        }
    }
}
