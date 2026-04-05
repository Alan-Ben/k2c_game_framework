package NPUSServer.NPUSUserMgr.UserComp.RecordComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_Record;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer.NPRecordExtDealerMgr;
import USDB.Bo.PlayerRecordBO;

public class PlayerRecordInfo
{
    private PlayerRecordComponent _m_comp;
    //记录相关数据
    private long _m_lDbid;
    private ENPPlayerRecordParam _m_eType;
    private long _m_lCount;

    protected PlayerRecordInfo(PlayerRecordComponent _comp, PlayerRecordBO _bo)
    {
        _m_comp = _comp;

        _m_lDbid = _bo.getId();
        _m_eType = ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt(_bo.getType());
        _m_lCount = _bo.getCount();
    }

    public PlayerRecordComponent getComp()
    {
        return _m_comp;
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    public ENPPlayerRecordParam getType()
    {
        return _m_eType;
    }

    public long getCount()
    {
        return _m_lCount;
    }

    /**
     * 构造协议对象
     * @return
     */
    public PlayerInfo_Record toProto()
    {
        PlayerInfo_Record proto = new PlayerInfo_Record();
        proto.setType(_m_eType);
        proto.setCount(_m_lCount);

        return proto;
    }

    /**
     * 修改记录计数
     * @param _chgCount
     * @param _dealType
     * @param _context
     */
    public void chgCount(long _chgCount, ENCounterDealType _dealType, NPPlayerContext _context)
    {
        long oriCount = _m_lCount;
        long curCount = 0;

        switch (_dealType)
        {
            case REDUCE:
                curCount = oriCount - _chgCount;
                break;
            case SET:
                curCount = _chgCount;
                break;
            case SET_GT:
                curCount = Math.max(oriCount, _chgCount);
                break;
            case ADD:
            default:
                curCount = oriCount + _chgCount;
                break;
        }

        //相同数据不予处理
        if (curCount == _m_lCount)
            return;

        //更新数据
        _m_lCount = curCount;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("count", _m_lCount);
        _m_comp.getUSServer().getBM().getBM(PlayerRecordBO.class).update("id", _m_lDbid, updateValue);

        //后续操作
        NPRecordExtDealerMgr.getInstance().dealOnCountChg(getComp().getUserData(), _m_eType, oriCount, _m_lCount, _context);
    }
}
