using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALPackage;

namespace ALBasicProtocolPack
{
    public class ALBasicProtocolDispather
    {
        protected ALBasicProtocolMainOrderDealer[] _m_lDealMap;

        public ALBasicProtocolDispather()
        {
            _m_lDealMap = new ALBasicProtocolMainOrderDealer[256];

            //初始化所有的256个处理对象队列
            for (int i = 0; i < 256; i++)
            {
                _m_lDealMap[i] = null;
            }
        }

        /// <summary>
        /// 获取对应主协议号的处理对象
        /// </summary>
        /// <param name="_dispathRegister"></param>
        public ALBasicProtocolMainOrderDealer GetMainOrderDealer(byte _mainOrder)
        {
            if(null == _m_lDealMap || _mainOrder >= _m_lDealMap.Length)
                return null;

            return _m_lDealMap[_mainOrder];
        }

        /****************
         * 注册主协议处理对象
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:29:20 AM
         */
        public void RegProtocol(ALBasicProtocolMainOrderDealer _dispathRegister)
        {
            int i = ALProtocolCommon.byte2int(_dispathRegister.getMainOrder());
            _m_lDealMap[i] = _dispathRegister;
        }


        /***********************
         * 协议处理函数
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:31:49 AM
         */
        public bool DealProtocol(_IALProtocolDealer _dealer, byte[] _msg)
        {
            ALProtocolBuf buffer = new ALProtocolBuf(_msg);
            //获取主协议编号
            byte mainOrder = buffer.get();

            //获取协议处理对象
            int iIndex = ALProtocolCommon.byte2int(mainOrder);
            if (iIndex >= _m_lDealMap.Length)
            {
                UnityEngine.Debug.LogError("Err protocol main order: " + iIndex);
                return false;
            }

            ALBasicProtocolMainOrderDealer dealer = _m_lDealMap[iIndex];

            if (null == dealer)
                return false;

            bool res = false;
            try
            {
                res = dealer.dispathProtocol(_dealer, buffer);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }

            return res;
        }
    }
}
