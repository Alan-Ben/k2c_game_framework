using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 客户端连接服务器后的登录流程管理对象
    /// </summary>
    public class NPGSClientLoginCounter
    {
        private NPGSClientListener _m_clClientListener;

        /** 登录阶段统计对象 */
        private static ALStepCounter _g_scLoginStepCounter;

        public NPGSClientLoginCounter(NPGSClientListener _clientListener)
        {
            _m_clClientListener = _clientListener;

            _g_scLoginStepCounter = new ALStepCounter();
        }

        public static ALStepCounter loginStepCounter { get { return _g_scLoginStepCounter; } }

        /// <summary>
        /// 准备开始登录流程，在登录完成之后调用回调
        /// </summary>
        /// <param name="_doneAction"></param>
        public void readyForLogin(Action _doneDelegate)
        {
            _g_scLoginStepCounter.resetAll();
            _g_scLoginStepCounter.chgTotalStepCount(1);
            _g_scLoginStepCounter.regAllDoneDelegate(_doneDelegate);
        }
    }
}
