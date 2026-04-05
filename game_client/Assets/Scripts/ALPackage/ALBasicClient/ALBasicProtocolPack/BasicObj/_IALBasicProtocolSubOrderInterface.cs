using System;
using ALPackage;

namespace ALBasicProtocolPack
{
    public interface _IALBasicProtocolSubOrderInterface
    {
        /*********************
         * 消息带入的处理函数，将协议从字节中读取出并带入实际处理函数
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:52:19 AM
         */
        void dealProtocol(_IALProtocolDealer _dealer, ALProtocolBuf _msgBuffer);

        /************
         * 自动根据处理的消息对象获取本处理对象处理的协议主，副协议号
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 11:36:41 AM
         */
        byte getMainOrder();
        byte getSubOrder();
    }
}
