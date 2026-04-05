using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的注册管理对象
    /// </summary>
    public class NPRequestCallbackMgr
    {
        private static NPRequestCallbackMgr _g_instance = new NPRequestCallbackMgr();
        public static NPRequestCallbackMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPRequestCallbackMgr();

                return _g_instance;
            }
        }

        /** 序列号生成对象 */
        private long _m_lSerializeMaker;
        /** 注册的处理对象表 */
        private Dictionary<long, _INPRequestCallbackDealer> _m_hmDealerMap;

        protected NPRequestCallbackMgr()
        {
            _m_lSerializeMaker = 1;
            _m_hmDealerMap = new Dictionary<long, _INPRequestCallbackDealer>();
        }

        /******************
         * 注册一个处理对象
         * @param _dealer
         * @return
         */
        public long regDealer(_INPRequestCallbackDealer _dealer)
        {
            if (null == _dealer)
                return 0;

            long serialzie = _m_lSerializeMaker;
            _m_lSerializeMaker++;

            //注册到列表
            _m_hmDealerMap.Add(serialzie, _dealer);

            //添加回调定时监听，按照最长30秒等待进行处理
            NPRequestCallbackMonitorMgr.instance.addCallback(serialzie);

            //返回序列号
            return serialzie;
        }
        public long regDealerWithNoMonitor(_INPRequestCallbackDealer _dealer)
        {
            if (null == _dealer)
                return 0;

            long serialzie = _m_lSerializeMaker;
            _m_lSerializeMaker++;

            //注册到列表
            _m_hmDealerMap.Add(serialzie, _dealer);

            //返回序列号
            return serialzie;
        }

        /*********************
         * 使用操作序列号取出回调对象
         * @param _dealSerialize
         * @return
         */
        public _INPRequestCallbackDealer popCallbackDealer(long _dealSerialize)
        {
            //先尝试获取值
            _INPRequestCallbackDealer dealer = null;
            if (!_m_hmDealerMap.TryGetValue(_dealSerialize, out dealer))
                return null;

            _m_hmDealerMap.Remove(_dealSerialize);

            return dealer;
        }

        /*********************
         * 直接执行回调对象
         * @param _dealSerialize
         * @return
         */
        public bool dealCallbackSucDealer(long _dealSerialize, byte[] _protocolBytes)
        {
            //先尝试获取值
            _INPRequestCallbackDealer dealer = null;
            if (!_m_hmDealerMap.TryGetValue(_dealSerialize, out dealer))
                return false;

            _m_hmDealerMap.Remove(_dealSerialize);
            if (null == dealer)
                return false;

            //并调用处理
            dealer.dealSuc(_protocolBytes);

            return true;
        }
        public bool dealCallbackFailDealer(long _dealSerialize, int _errCode, byte[] _protocolBytes)
        {
            //先尝试获取值
            _INPRequestCallbackDealer dealer = null;
            if (!_m_hmDealerMap.TryGetValue(_dealSerialize, out dealer))
                return false;

            _m_hmDealerMap.Remove(_dealSerialize);
            if (null == dealer)
                return false;

            dealer.dealFail(_errCode, _protocolBytes);

            return true;
        }
        /***************
         * 系统监听超时后的处理失败回调
         * @param _dealSerialize
         */
        public void dealCallbackTimeoutFailDealer(NPRequestCallbackTimerInfo _timerInfo)
        {
            //先尝试获取值
            _INPRequestCallbackDealer dealer = null;
            if (!_m_hmDealerMap.TryGetValue(_timerInfo.getCallbackSerialize(), out dealer))
                return;

            _m_hmDealerMap.Remove(_timerInfo.getCallbackSerialize());
            if (null == dealer)
                return;

            //报错
            ALLog.Sys("Callback Serialize: " + _timerInfo.getCallbackSerialize() + " System timeout!!! OutTimeMS: "
                        + (ALCommon.getNowTimeMill() - _timerInfo.getTimeoutTimeMS() + NPRequestCallbackMonitorMgr.C_MonitorTimeMS) + " Type: " + dealer);

            ALMonoTaskMgr.instance.addMonoTask(new NPRequestCallbackDealFailTask(dealer, ErrorCodeConst.COMMON_ERROR_CODE, null));
        }
    }
}
