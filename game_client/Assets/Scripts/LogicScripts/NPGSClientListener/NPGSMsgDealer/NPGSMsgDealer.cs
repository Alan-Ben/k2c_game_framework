using System;
using System.Collections.Generic;

using ALPackage;
using ALBasicProtocolPack;


public class NPGSMsgDealer
{
    private static NPGSMsgDealer _g_instance = new NPGSMsgDealer();
    public static NPGSMsgDealer instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new NPGSMsgDealer();

            return _g_instance;
        }
    }
    public static void resetMsgDealer()
    {
        if (null != _g_instance)
        {
            _g_instance.discard();
            _g_instance = null;
        }

        _g_instance = new NPGSMsgDealer();
    }

    //最后一个发送的消息序列号
    private int _m_iListFirstSendedMsgSerialize;
    //未确认的返回的消息队列
    private List<NPGSMsgItem> _m_lSendBackMsg;
    //最后一个收到的消息序列号
    private int _m_iReceivedMsgCount;

    public NPGSMsgDealer()
    {
        _m_iListFirstSendedMsgSerialize = 0;
        _m_lSendBackMsg = new List<NPGSMsgItem>();
        _m_iReceivedMsgCount = 0;
    }

    public int getListFirstSendedMsgSerialize() { return _m_iListFirstSendedMsgSerialize; }
    public int getReceivedMsgCount() { return _m_iReceivedMsgCount; }
    public int sendBackMsgCount { get { return _m_lSendBackMsg.Count; } }

    //清空消息
    public void discard()
    {
        _m_lSendBackMsg.Clear();
    }

    /****************
     * 添加一个返回的消息
     */
    public int addSendBackMsg(long _clientSerialize, _IALProtocolStructure _msg)
    {
        if (null == _msg)
            return -1;

        //添加到队列
        _m_lSendBackMsg.Add(new NPGSMsgItem(_clientSerialize, _msg));

        return _m_iListFirstSendedMsgSerialize + _m_lSendBackMsg.Count;
    }

    /***************
     * 确认发送了的消息的数量
     */
    public void checkSendedMsg(int _sendedMsgCount)
    {
        _m_lSendBackMsg.RemoveRange(0, _sendedMsgCount - _m_iListFirstSendedMsgSerialize);
        _m_iListFirstSendedMsgSerialize = _sendedMsgCount;
    }

    /***************
     * 检测当前消息数量并获取当前需要重新发送的消息
     */
    public List<NPGSMsgItem> getNeedSendBackMsg(int _sendedMsgCount)
    {
        checkSendedMsg(_sendedMsgCount);

        if (_m_lSendBackMsg.Count <= 0)
            return null;

        List<NPGSMsgItem> list = new List<NPGSMsgItem>();
        //将所有消息放到队列中
        list.AddRange(_m_lSendBackMsg);

        return list;
    }

    /**************
     * 累加收到的消息数量
     */
    public int checkClientMsgSerialize(int _serverMsgSerialize)
    {
        if (_serverMsgSerialize <= _m_iReceivedMsgCount)
            return 0;

        if (_serverMsgSerialize - _m_iReceivedMsgCount != 1)
            return -1;

        _m_iReceivedMsgCount++;

        return _m_iReceivedMsgCount;
    }
}
