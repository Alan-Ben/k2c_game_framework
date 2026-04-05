package NPGateServer.NPGCMsgMgr;

import ALServerLog.ALServerLog;
import NP2GS.p001_BasicOp.NP2GS_001_001_SendbackUserMsg;
import NP2GS.p001_BasicOp.NP2GS_001_010_SendbackUserClientRequest;
import NPCommon.Log.CommLog;
import NPGateServer.GateServerConf;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem.NPMsgNormalItem;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem.NPMsgRequestItem;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem._ANPGSMsgItem;
import NPGateServer.NPGSGCMgr.NPGSGCMgr;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Map;
import java.util.Map.Entry;
import java.util.concurrent.locks.ReentrantLock;

/***************
 * 客户端消息收发处理对象
 * @author Administrator
 *
 */
public class NPGCMsgDealer
{
    private String _m_sUid;
    //对应消息处理的序列号
    private int _m_iSerialize;
    //对应客户端的连接Id
    private long _m_lGCSessionId;

    //最后一个发送的消息序列号
    private int _m_iListFirstSendedMsgSerialize;
    //未确认的返回的消息队列
    private LinkedList<_ANPGSMsgItem> _m_lSendBackMsg;
    //发送回去的消息总大小
    private long _m_lSendBackMsgTotalSize;
    //最后一个收到的消息序列号
    private int _m_iReceivedMsgCount;

    /**
     * 是否已经检测过重发处理部分
     */
    private boolean _m_bIsCheckReconnect;
    //消息处理对象是否有效，如无效则设置无效
    private boolean _m_bIsEnable;

    //是否需要打印协议数据
    private boolean _m_bNeedPrintProtocol;

    //锁对象
    private ReentrantLock _m_mutex;

    public NPGCMsgDealer(String _uid, int _serialzie)
    {
        _m_sUid = _uid;
        _m_iSerialize = _serialzie;
        _m_lGCSessionId = 0;

        _m_iListFirstSendedMsgSerialize = 0;
        _m_lSendBackMsg = new LinkedList<_ANPGSMsgItem>();
        _m_lSendBackMsgTotalSize = 0;

        _m_iReceivedMsgCount = 0;

        _m_bIsCheckReconnect = false;
        _m_bIsEnable = true;

        _m_bNeedPrintProtocol = true;

        _m_mutex = new ReentrantLock();
    }

    public String getUid()
    {
        return _m_sUid;
    }

    public int getSerialize()
    {
        return _m_iSerialize;
    }

    public long getGCSessionId()
    {
        return _m_lGCSessionId;
    }

    public int getListFirstSendedMsgSerialize()
    {
        return _m_iListFirstSendedMsgSerialize;
    }

    public int getReceivedMsgCount()
    {
        return _m_iReceivedMsgCount;
    }

    public int getSendBackMsgCount()
    {
        return _m_lSendBackMsg.size();
    }

    public boolean isEnable()
    {
        return _m_bIsEnable;
    }

    public void setDisable()
    {
        _m_bIsEnable = false;
    }

    public boolean getIsCheckReconnect()
    {
        return _m_bIsCheckReconnect;
    }

    //设置未检测
    public void setNotCheckReconnect()
    {
        _m_bIsCheckReconnect = false;
    }

    //设置已经检测
    public void setCheckReconnectDone()
    {
        _m_bIsCheckReconnect = true;
    }

    public void setGCSessionId(long _sessionid)
    {
        _m_lGCSessionId = _sessionid;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**********
     * 检测是否允许发送，如允许则返回true，如不允许则返回false
     * @param _msg
     */
    public boolean checkSendAble(NP2GS_001_001_SendbackUserMsg _msg)
    {
        _lock();

        try
        {
            if (_m_bIsCheckReconnect)
                return true;

            //添加到集合
            _addSendBackMsg(new NPMsgNormalItem(_msg.getMsg()), true);

            //返回不允许发送
            return false;
        } finally
        {
            _unlock();
        }
    }

    public boolean checkSendAble(NP2GS_001_010_SendbackUserClientRequest _msg)
    {
        _lock();

        try
        {
            if (_m_bIsCheckReconnect)
                return true;

            //添加到集合
            _addSendBackMsg(new NPMsgRequestItem(_msg.getClientRequestSerialize(), _msg.getRes(), _msg.getErrCode(), _msg.getRetMsg()), true);

            //返回不允许发送
            return false;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 添加一个返回的消息
     */
    public _ANPGSMsgItem addSendBackMsg(NP2GS_001_001_SendbackUserMsg _msg, boolean _isOnlineUser)
    {
        return _addSendBackMsg(new NPMsgNormalItem(_msg.getMsg()), _isOnlineUser);
    }

    public _ANPGSMsgItem addSendBackMsg(NP2GS_001_010_SendbackUserClientRequest _msg, boolean _isOnlineUser)
    {
        return _addSendBackMsg(new NPMsgRequestItem(_msg.getClientRequestSerialize(), _msg.getRes(), _msg.getErrCode(), _msg.getRetMsg()), _isOnlineUser);
    }

    private _ANPGSMsgItem _addSendBackMsg(_ANPGSMsgItem _msg, boolean _isOnlineUser)
    {
        if (null == _msg)
            return null;

        _lock();

        try
        {
            if (!_m_bIsEnable)
                return null;

            //超过1024条消息则直接当作连接失败
            //if(_m_lSendBackMsg.size() >= 1024)
            int clientSocketSizeDouble = GateServerConf.getInstance().getClientSocketCacheSize() * 2;
            //替换为如果缓存消息总长度超过玩家缓存区2倍则不处理，增加100字节以作扩展，此判断只在用户不在线才有效，如用户在线则不处理
            if (!_isOnlineUser && _m_lSendBackMsgTotalSize + 100 >= clientSocketSizeDouble)
            {
                //只有不在线用户才能设置为无效
                setDisable();

                //输出用户在此时设置为无效，会引起客户端重登处理
                ALServerLog.Sys("User Msg Too long to set Msg Dealer Disable! AccId: " + getUid() + "   sessionId: " + getGCSessionId());
                ALServerLog.Info("_m_lSendBackMsgTotalSize: " + _m_lSendBackMsgTotalSize + " >= cache_size*2 " + clientSocketSizeDouble);
                _printSendBackMsgList();

                //非在线用户清空缓存
                _m_lSendBackMsg.clear();
                _m_lSendBackMsgTotalSize = 0;

                return null;
            }

            //如果尺寸超过允许范围则直接返回-1，不添加消息
            int clientSocketSizeMax = GateServerConf.getInstance().getClientSocketCacheSize() * 4;
            if (_m_lSendBackMsgTotalSize + 100 >= clientSocketSizeMax)
            {
                //判断是否需要打印处理
                if (_m_bNeedPrintProtocol)
                {
                    _m_bNeedPrintProtocol = false;
                    ALServerLog.Info("_m_lSendBackMsgTotalSize:" + _m_lSendBackMsgTotalSize + " >=cache_size*4 " + clientSocketSizeMax);
                    _printSendBackMsgList();
                }

                //输出用户在此时设置为无效，会引起客户端重登处理
                ALServerLog.Sys("Ignore Send back msg! for AccId: " + getUid() + "   sessionId: " + getGCSessionId());
                //此时需要返回一个特殊消息，告知客户端服务器网络消息出现堆积
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(getGCSessionId());
                if (null != gcListener)
                {
                    //补充发送切除的消息，保证消息都能发送到客户端，避免消息遗漏。
                    gcListener.send(_msg.getMsgRealBytes());

                    //补充发送一个无效消息
                    gcListener.send(NPGS2GCWriter_001_BasicOp.make_010_OnMsgInvalid(getGCSessionId()));
                }

                return null;
            } else
            {
                //当消息恢复正常则继续可以打印处理
                _m_bNeedPrintProtocol = true;
            }

            //添加到队列
            _m_lSendBackMsg.add(_msg);
            _m_lSendBackMsgTotalSize += _msg.getFullSize();
            //修改序列号为当前消息序号
            _msg.setMsgSendSerialize(_m_iListFirstSendedMsgSerialize + _m_lSendBackMsg.size());

            return _msg;
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 确认发送了的消息的数量
     */
    public void checkSendedMsg(int _sendedMsgCount)
    {
        //根据当前第一个消息以及序号从队列删除消息对象
        _lock();

        try
        {
            if (!_m_bIsEnable)
                return;

            while (_m_iListFirstSendedMsgSerialize < _sendedMsgCount)
            {
                //从消息队列删除第一个消息
                if (!_m_lSendBackMsg.isEmpty())
                {
                    _ANPGSMsgItem firstMsg = _m_lSendBackMsg.removeFirst();
                    //移除缓存则删除缓存消息队列长度统计
                    if (null != firstMsg)
                        _m_lSendBackMsgTotalSize -= firstMsg.getFullSize();

                    //累加序列号
                    _m_iListFirstSendedMsgSerialize++;
                } else
                {
                    System.out.println("chk sended er: " + _m_iListFirstSendedMsgSerialize + " - " + _sendedMsgCount);
                    _m_iListFirstSendedMsgSerialize = _sendedMsgCount;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 检测当前消息数量并获取当前需要重新发送的消息
     */
    public ArrayList<_ANPGSMsgItem> checkReconnectSendBackMsg(int _sendedMsgCount)
    {
        //根据当前第一个消息以及序号从队列删除消息对象
        _lock();

        try
        {
            if (!_m_bIsEnable)
                return null;

            while (_m_iListFirstSendedMsgSerialize < _sendedMsgCount)
            {
                //从消息队列删除第一个消息
                if (!_m_lSendBackMsg.isEmpty())
                {
                    _ANPGSMsgItem firstMsg = _m_lSendBackMsg.removeFirst();
                    //移除缓存则删除缓存消息队列长度统计
                    if (null != firstMsg)
                        _m_lSendBackMsgTotalSize -= firstMsg.getFullSize();

                    //累加序列号
                    _m_iListFirstSendedMsgSerialize++;
                } else
                {
                    System.out.println("get need sended er: " + _m_iListFirstSendedMsgSerialize + " - " + _sendedMsgCount);

                    //累加序列号
                    _m_iListFirstSendedMsgSerialize = _sendedMsgCount;
                }
            }

            if (_m_lSendBackMsg.isEmpty())
                return null;

            ArrayList<_ANPGSMsgItem> list = new ArrayList<_ANPGSMsgItem>();
            //将所有消息放到队列中
            list.addAll(_m_lSendBackMsg);

            return list;
        } finally
        {
            //设置已经检测
            _m_bIsCheckReconnect = true;

            _unlock();
        }
    }

    /******************
     * 尝试从对应消息开始，获取之后的未发送消息。一般在客户端初次进入初始化的时候，reconnect为false导致消息可能出现断层
     * 此处理将保证消息会被全部发送
     * @param _sendedMsgCount
     * @return
     */
    public ArrayList<_ANPGSMsgItem> tryReSendReconnectSendBackMsg(int _sendedMsgCount)
    {
        //根据当前第一个消息以及序号从队列删除消息对象
        _lock();

        try
        {
            if (!_m_bIsEnable)
                return null;

            int tmpFirstSerialize = _m_iListFirstSendedMsgSerialize;
            int sendStartIdx = 0;
            while (tmpFirstSerialize < _sendedMsgCount)
            {
                //从消息队列删除第一个消息
                if (sendStartIdx < _m_lSendBackMsg.size())
                {
                    //累加序列号
                    tmpFirstSerialize++;
                    sendStartIdx++;
                } else
                {
                    return null;
                }
            }

            if (sendStartIdx >= _m_lSendBackMsg.size())
                return null;

            ArrayList<_ANPGSMsgItem> list = new ArrayList<_ANPGSMsgItem>();
            for (int i = sendStartIdx; i < _m_lSendBackMsg.size(); i++)
            {
                //将消息返回，准备发送
                list.add(_m_lSendBackMsg.get(i));
            }

            return list;
        } finally
        {
            _unlock();
        }
    }

    /**************
     * 累加收到的消息数量
     */
    public int checkClientMsgSerialize(int _clientMsgSerialize)
    {
        _lock();

        try
        {
            if (_clientMsgSerialize <= _m_iReceivedMsgCount)
                return 0;

            if (_clientMsgSerialize - _m_iReceivedMsgCount != 1)
                return -1;

            _m_iReceivedMsgCount++;

            return _m_iReceivedMsgCount;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 釋放本消息处理对象数据
     */
    public void dispose()
    {
        _lock();

        try
        {
            if (!_m_bIsEnable)
                return;

            //只有不在线用户才能设置为无效
            setDisable();

            //非在线用户清空缓存
            _m_lSendBackMsg.clear();
            _m_lSendBackMsgTotalSize = 0;
        } finally
        {
            _unlock();
        }
    }

    /******
     * 打印发送协议列表。
     */
    private static class ProtoStat
    {
        public int count;
        public int size;

        public void add(int _size)
        {
            count++;
            size += _size;
        }

        public ProtoStat(int _size)
        {
            count = 1;
            size = _size;
        }
    }

    private void _printSendBackMsgList()
    {
        try
        {
            Map<Integer, ProtoStat> idStatMap = new HashMap<>();
            for (_ANPGSMsgItem msg : _m_lSendBackMsg)
            {
                byte maindId = msg.getMainOrder();
                byte subId = msg.getSubOrder();
                int key = maindId * 1000 + subId;
                ProtoStat stat = idStatMap.get(key);
                if (null == stat)
                {
                    stat = new ProtoStat(msg.getFullSize());
                    idStatMap.put(key, stat);
                } else
                {
                    stat.add(msg.getFullSize());
                }

            }
            for (Entry<Integer, ProtoStat> enrty : idStatMap.entrySet())
            {
                CommLog.info("[{}-{}] = count:{} total size:{}", enrty.getKey() / 1000, enrty.getKey() % 1000, enrty.getValue().count, enrty.getValue().size);
            }
        } catch (Throwable e)
        {
            CommLog.error("", e);
        }

    }
}
