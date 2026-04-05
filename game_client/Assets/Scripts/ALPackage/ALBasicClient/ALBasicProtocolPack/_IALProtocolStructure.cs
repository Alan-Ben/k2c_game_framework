using System;
using System.Collections.Generic;

namespace ALBasicProtocolPack
{
    public interface _IALProtocolStructure
    {
        /******************
         * 获取主协议编号
         * 
         * @author alzq.z
         * @time   Jan 15, 2013 9:53:29 PM
         */
        byte getMainOrder();
        /*******************
         * 获取副协议编号
         * 
         * @author alzq.z
         * @time   Jan 15, 2013 9:53:41 PM
         */
        byte getSubOrder();

        int GetBufSize();
        int GetFullPackBufSize();

        /**********
         * 创建完整的数据包，包含协议处理编号部分
         * 
         * @author alzq.z
         * @time   Feb 25, 2013 10:56:58 PM
         */
        byte[] makeFullPackage();
        void makeFullPackage(ALProtocolBuf _recBuf);
        byte[] makePackage();
        void readPackage(ALProtocolBuf _buf);
    }
}
