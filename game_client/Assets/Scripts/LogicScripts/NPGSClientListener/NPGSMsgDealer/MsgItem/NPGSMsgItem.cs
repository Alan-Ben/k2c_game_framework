using System;
using System.Collections.Generic;

using ALPackage;
using ALBasicProtocolPack;


/// <summary>
/// 存储发送消息数据的结构体，使用stuct避免内存碎片
/// </summary>
public struct NPGSMsgItem
{
    //客户端回调处理序列号，如果无回调处理为0
    private long _m_lClientSerialize;
    //对应发送消息的内容
    private _IALProtocolStructure _m_pProtocol;

    public NPGSMsgItem(long _clientSerialize, _IALProtocolStructure _protocol)
    {
        _m_lClientSerialize = _clientSerialize;
        _m_pProtocol = _protocol;
    }

    public long clientSerialize { get { return _m_lClientSerialize; } }
    public _IALProtocolStructure protocolObj { get { return _m_pProtocol; } }
}
