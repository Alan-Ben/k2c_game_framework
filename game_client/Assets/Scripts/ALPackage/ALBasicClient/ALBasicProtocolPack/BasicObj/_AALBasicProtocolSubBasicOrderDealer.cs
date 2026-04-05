using System;
using ALPackage;

namespace ALBasicProtocolPack
{
    public abstract class _AALBasicProtocolSubBasicOrderDealer<T> : _IALBasicProtocolSubOrderInterface where T : _IALProtocolStructure
    {
        /** 实例化对象，用于获取信息使用 */
        private T basicInfoObj;

        public _AALBasicProtocolSubBasicOrderDealer()
        {
            basicInfoObj = _createProtocolObj();
        }

        /*********************
         * 消息带入的处理函数，将协议从字节中读取出并带入实际处理函数
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:52:19 AM
         */
        public void dealProtocol(_IALProtocolDealer _dealer, ALProtocolBuf _msgBuffer)
        {
            T protocolObj = _createProtocolObj();
            try
            {
                protocolObj.readPackage(_msgBuffer);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Reading Protocol: " + _msgBuffer.getBuf()[0] + " - " + _msgBuffer.getBuf()[1] + " Error!\n" + ex.ToString());
            }

            //处理数据，并返回结果
            _dealProtocol(_dealer, protocolObj);

            //解除关联
            protocolObj = default(T);
        }

        /************
         * 自动根据处理的消息对象获取本处理对象处理的协议主，副协议号
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:36:41 AM
         */
        public byte getMainOrder() { return basicInfoObj.getMainOrder(); }
        public byte getSubOrder() { return basicInfoObj.getSubOrder(); }

        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected abstract T _createProtocolObj();

        /**********************
         * 消息处理函数，直接带入对应的消息结构体
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:52:30 AM
         */
        protected abstract void _dealProtocol(_IALProtocolDealer _dealer, T _msg);
    }
}
