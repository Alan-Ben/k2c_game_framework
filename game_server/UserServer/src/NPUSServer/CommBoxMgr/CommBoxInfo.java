package NPUSServer.CommBoxMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.NpChatObj.NPCommon_ChatContent_CommBox;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPBoxChatStatus;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.NPUserServer;
import USDB.Bo.UsBoxBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class CommBoxInfo
{
    private NPUserServer _m_server;
    private long _m_lInstanceId;//宝箱实例ID
    private long _m_lSenderCid;//发送玩家CID
    private long _m_lCreatedAt;//创建时间
    private RefBoxComm _m_refBox;//宝箱配置数据
    private Common_LongList _m_clGainedCid;//领取宝箱的玩家列表
    private long _m_lExpiredMs;//过期时间

    private MutexAtom _m_mutex;

    public CommBoxInfo(NPUserServer _server, UsBoxBO _bo, RefBoxComm _ref)
    {
        _m_server = _server;
        _m_lInstanceId = _bo.getInstanceId();
        _m_lSenderCid = _bo.getSenderCid();
        _m_lCreatedAt = _bo.getCreatedAt();

        _m_clGainedCid = new Common_LongList();
        if (null != _bo.getGainedCidList())
        {
            ByteBuffer cidListBuff = ByteBuffer.wrap(_bo.getGainedCidList());
            _m_clGainedCid.readPackage(cidListBuff);
        }

        _m_refBox = _ref;

        _m_mutex = new MutexAtom();

        //设置过期时间
        _m_lExpiredMs = _m_lCreatedAt + _m_refBox.expire_secs * 1000;
    }

    public NPUserServer getUSServer(){return _m_server;}
    public long getInstanceId()
    {
        return _m_lInstanceId;
    }
    public RefBoxComm getRef()
    {
        return _m_refBox;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /*****
     * 获取所有已领取玩家CID列表
     * @return
     */
    public ArrayList<Long> getGainedCidList()
    {
        _lock();

        try
        {
            return new ArrayList<>(_m_clGainedCid.getValueList());
        } finally
        {
            _unlock();
        }
    }

    /****
     * 已经过期检查
     * @return
     */
    public boolean isExpired()
    {
        return CommonFunc.getNowTimeMS() > _m_lExpiredMs;
    }

    /**
     * 构造聊天数据对象
     * @return
     */
    public NPCommon_ChatContent_CommBox toChatProto()
    {
        NPCommon_ChatContent_CommBox proto = new NPCommon_ChatContent_CommBox();
        proto.setInstanceId(_m_lInstanceId);
        proto.setRefId(_m_refBox.id);
        proto.setSenderCid(_m_lSenderCid);

        return proto;
    }

    /**
     * 检查宝箱状态
     * @param _cid
     * @return
     */
    public ENPBoxChatStatus checkStatus(long _cid)
    {
        _lock();

        try
        {
            //检查宝箱被领取的上限
            if (_m_clGainedCid.getValueList().size() >= _m_refBox.box_limit)
                return ENPBoxChatStatus.IS_EMPTY;

            //如果这个宝箱发送者不能领，查看宝箱状态的人是发送者
            if (!_m_refBox.is_sender_gain && _cid == _m_lSenderCid)
                return ENPBoxChatStatus.NONE;

            //检查是否领取
            if (_m_clGainedCid.getValueList().contains(_cid))
                return ENPBoxChatStatus.IS_GAINED;

            return ENPBoxChatStatus.NONE;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置领取宝箱奖励
     * @return
     */
    public boolean setGainedCid(long _cid)
    {
        _lock();

        try
        {
            //检查领取资格
            if (ENPBoxChatStatus.NONE != checkStatus(_cid))
                return false;

            _m_clGainedCid.getValueList().add(_cid);
            //跟新数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("gainedCidList", _m_clGainedCid.makePackage());
            getUSServer().getBM().getBM(UsBoxBO.class).update("instanceId", _m_lInstanceId, updateValue);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /****
     * 移除数据
     */
    protected void del()
    {
        getUSServer().getBM().getBM(UsBoxBO.class).delAll("instanceId", _m_lInstanceId);
    }

    /****
     * GM命令移除已领取玩家
     * @param _cid
     */
    public void clearGainedCid(long _cid)
    {
        _lock();

        try
        {
            if (_cid > 0)
            {
                _m_clGainedCid.getValueList().remove(Long.valueOf(_cid));
            } else
            {
                _m_clGainedCid.getValueList().clear();
            }

            //跟新数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("gainedCidList", _m_clGainedCid.makePackage());
            getUSServer().getBM().getBM(UsBoxBO.class).update("instanceId", _m_lInstanceId, updateValue);
        } finally
        {
            _unlock();
        }
    }
}
