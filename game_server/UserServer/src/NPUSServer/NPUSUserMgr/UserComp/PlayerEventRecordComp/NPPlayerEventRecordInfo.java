package NPUSServer.NPUSUserMgr.UserComp.PlayerEventRecordComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.PlayerEnum.EPlayerEventRecordType;
import Common.PlayerObj.Player_EventRecordInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USDB.Bo.PlayerEventRecordBO;

/**
 * @description: 行为计数器
 * @author: ricci
 * @date: 2022-08-26 15:54:14
 */
public class NPPlayerEventRecordInfo
{
    /**
     * 上级管理器
     */
    private final NPPlayerEventRecordInfoMgr _m_mgr;

    /**
     * 计数器数据
     */
    private long _m_lDbid;
    private EPlayerEventRecordType _m_eType;
    private long _m_lSubId;
    private long _m_lCount;

    public NPPlayerEventRecordInfo(NPPlayerEventRecordInfoMgr _mgr, PlayerEventRecordBO _bo)
    {
        _m_mgr = _mgr;

        _m_lDbid = _bo.getId();
        _m_eType = EPlayerEventRecordType.EPlayerEventRecordType_FromInt(_bo.getRecordTypeId());
        _m_lSubId = _bo.getRecordSubId();
        _m_lCount = _bo.getCount();
    }

    public NPPlayerEventRecordInfoMgr getMgr()
    {
        return _m_mgr;
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    public EPlayerEventRecordType getType()
    {
        return _m_eType;
    }

    public long getSubId()
    {
        return _m_lSubId;
    }

    public long getCount()
    {
        return _m_lCount;
    }

    public void setCount(long _count)
    {
        //相同数据不再处理
        if (_count == _m_lCount)
            return;

        //更新计数
        _m_lCount = _count;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("count", _m_lCount);
        _m_mgr.getComp().getUSServer().getBM().getBM(PlayerEventRecordBO.class).update("id", _m_lDbid, updateValue);

        //推送协议
        getMgr().getComp().getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_065_OnEventRecordChg(toProto()));
    }

    /**
     * 构造数据协议对象
     * @return
     */
    public Player_EventRecordInfo toProto()
    {
        Player_EventRecordInfo proto = new Player_EventRecordInfo();
        proto.setType(_m_eType);
        proto.setSubId(_m_lSubId);
        proto.setCount(_m_lCount);

        return proto;
    }

    @Override
    public String toString()
    {
        return "\n" + _m_eType + "-" + _m_lSubId + ":" + _m_lCount + "}";
    }
}
