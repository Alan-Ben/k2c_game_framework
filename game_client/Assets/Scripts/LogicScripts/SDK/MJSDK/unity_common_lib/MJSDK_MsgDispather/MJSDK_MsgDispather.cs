using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MJSDK_Package
{
    /*****************
     * 协议的主协议标记具体处理对象基类
     * 此对象管理所有子协议处理数据集合，并进行二层分发
     */
    public class MJSDK_MsgDispather
    {
        private static MJSDK_MsgDispather _g_instance = new MJSDK_MsgDispather();
        public static MJSDK_MsgDispather instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new MJSDK_MsgDispather();
                return _g_instance;
            }
        }

        ///消息处理中的异常处理接口对象
        /// <summary>
        /// 主协议处理对象的管理数据集
        /// </summary>
        private Dictionary<string, MJSDK_MsgMainDealer> _m_htMainDealerTable;

        protected MJSDK_MsgDispather()
        {
            _m_htMainDealerTable = new Dictionary<string, MJSDK_MsgMainDealer>();
        }

        /// <summary>
        /// 尝试注册一个协议处理对象，会根据主协议标记进行分类管理
        /// </summary>
        /// <param name="_mainOrder"></param>
        /// <param name="_subOrder"></param>
        /// <param name="_dealer"></param>
        public void regSubDealer(string _mainOrder, string _subOrder, _AMJSDK_MsgSubDealer _dealer)
        {
            MJSDK_MsgMainDealer mainDealer = null;
            if (_m_htMainDealerTable.TryGetValue(_mainOrder, out mainDealer) || null == mainDealer)
            {
                mainDealer = new MJSDK_MsgMainDealer(_mainOrder);
                _m_htMainDealerTable.Add(_mainOrder, mainDealer);
            }

            //将实际处理对象放入主协议管理器
            mainDealer.regSubDealer(_subOrder, _dealer);
        }

        /// <summary>
        /// 实际处理消息的处理函数，带入相关参数，将会根据注册情况调用实际的处理对象进行处理
        /// </summary>
        /// <param name="_mainOrder"></param>
        /// <param name="_subOrder"></param>
        /// <param name="_msg"></param>
        /// <param name="_commiter"></param>
        /// <returns></returns>
        public bool dealMsg(string _mainOrder, string _subOrder, string _msg)
        {
            //获取对应的主协议处理对象
            MJSDK_MsgMainDealer mainDealer = null;
            if (!_m_htMainDealerTable.TryGetValue(_mainOrder, out mainDealer) || null == mainDealer)
            {
                // MJSDK_Log.mjsdkLog("Can not find main dealer: " + _mainOrder, E_MJSDK_BusType.Error_DealSDKMsgErr);
                return false;
            }

            //调用派发对象的处理
            return mainDealer.dealMsg(_subOrder, _msg);
        }
    }
}