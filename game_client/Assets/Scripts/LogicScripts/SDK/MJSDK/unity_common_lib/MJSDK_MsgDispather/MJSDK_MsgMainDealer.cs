using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MJSDK_Package
{
    /// <summary>
    /// 协议的主协议标记具体处理对象基类
    /// 此对象管理所有子协议处理数据集合，并进行二层分发
    /// </summary>
    public class MJSDK_MsgMainDealer
    {
        //主协议标记
        private string _m_sMainOrder;
        //存储子协议处理对象的数据集
        private Dictionary<string, _AMJSDK_MsgSubDealer> _m_htSubDealerTable;

        public MJSDK_MsgMainDealer(string _mainOrder)
        {
            _m_sMainOrder = _mainOrder;
            _m_htSubDealerTable = new Dictionary<string, _AMJSDK_MsgSubDealer>();
        }

        public string getMainOrder() { return _m_sMainOrder; }

        /// <summary>
        /// 注册子处理对象
        /// </summary>
        /// <param name="_subOrder"></param>
        /// <param name="_dealer"></param>
        public void regSubDealer(string _subOrder, _AMJSDK_MsgSubDealer _dealer)
        {
            _m_htSubDealerTable.Add(_subOrder, _dealer);
        }

        /// <summary>
        /// 实际处理消息的处理函数
        /// </summary>
        /// <param name="_subOrder"></param>
        /// <param name="_msg"></param>
        /// <returns></returns>
        public bool dealMsg(string _subOrder, string _msg)
        {
            //获取对应的处理对象
            _AMJSDK_MsgSubDealer subDealer = null;
            if (!_m_htSubDealerTable.TryGetValue(_subOrder, out subDealer) || null == subDealer)
            {
                MJSDK_Log.mjsdkLog("Can not find sub dealer: " + _m_sMainOrder + " - " + _subOrder, E_MJSDK_BusType.Error_DealSDKMsgErr);
                return false;
            }

            //进行处理操作
            try
            {
                subDealer.dealMsg(_msg);
            }
            catch (Exception _e)
            {
                MJSDK_Log.mjsdkLog("Deal Msg: " + _m_sMainOrder + " - " + _subOrder + " Error [" + _e.Message + "]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return false;
            }

            return true;
        }
    }
}
