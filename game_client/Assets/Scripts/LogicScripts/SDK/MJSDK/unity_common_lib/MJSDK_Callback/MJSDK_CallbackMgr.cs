using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK消息发送后的回调处理对象的管理器
    /// 本管理器需要提供注册和处理的相关操作
    /// </summary>
    public class MJSDK_CallbackMgr
    {
        private static MJSDK_CallbackMgr _g_instance = new MJSDK_CallbackMgr();
        public static MJSDK_CallbackMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new MJSDK_CallbackMgr();

                return _g_instance;
            }
        }

        private static long _g_lCallbackSerialize = 1;
        /// <summary>
        /// 获取新的回调序列号并返回
        /// </summary>
        /// <returns></returns>
        protected static long MakeNewSerialize() { return _g_lCallbackSerialize++; }

        //回调注册和管理的容器
        private Dictionary<long, _IMJSDK_CallbackDealer> _m_dicCallbackDic;

        protected MJSDK_CallbackMgr()
        {
            _m_dicCallbackDic = new Dictionary<long, _IMJSDK_CallbackDealer>();
        }

        /// <summary>
        /// 注册一个新的回调对象，并返回一个回调序列号
        /// </summary>
        /// <param name="_dealer"></param>
        /// <returns></returns>
        public long regCallback(_IMJSDK_CallbackDealer _dealer)
        {
            if (null == _dealer)
                return 0;

            //加锁避免跨线程调用
            lock (this)
            {
                long callbackSerialize = MakeNewSerialize();
                //注册到容器中
                _m_dicCallbackDic.Add(callbackSerialize, _dealer);

                //返回序列号
                return callbackSerialize;
            }
        }

        /// <summary>
        /// 根据回调序列号从容器中将回调对象取出
        /// </summary>
        /// <param name="_callbackSerialize"></param>
        /// <returns></returns>
        public _IMJSDK_CallbackDealer popCallback(long _callbackSerialize)
        {
            lock (this)
            {
                //尝试取出
                _IMJSDK_CallbackDealer dealer = null;
                if (!_m_dicCallbackDic.TryGetValue(_callbackSerialize, out dealer))
                {
                    UnityEngine.Debug.LogError($"Can not find SDK callback {_callbackSerialize}");
                }

                return dealer;
            }
        }
    }
}
