package NPUSServer.Guild.Cooperate;

import Common.GuildCooperateObj.GuildCooperate_AttackLog;
import CommonEnum.ESpecAttrType;
import USDB.Bo.GuildCooperateAttackLogBO;

public class GuildCooperateLogInfo
{
    private long _m_dbId;
    private long _m_posId;
    private ESpecAttrType _m_attrType;
    private long _m_damageHp;
    private String _m_playerName;
    private long _m_timeMs;

    public GuildCooperateLogInfo(GuildCooperateAttackLogBO _pointBo)
    {
        _m_dbId = _pointBo.getId();
        _m_posId = _pointBo.getPosId();
        _m_attrType = ESpecAttrType.ESpecAttrType_FromInt(_pointBo.getAttrType());
        _m_damageHp = _pointBo.getAttackHp();
        _m_playerName = _pointBo.getPlayerName();
        _m_timeMs = _pointBo.getTimeMs();
    }

    /**
     * 获取数据id
     * @return
     */
    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 构造协议
     * @return
     */
    public GuildCooperate_AttackLog makeProto()
    {
        GuildCooperate_AttackLog log = new GuildCooperate_AttackLog();
        log.setDbId(_m_dbId);
        log.setPosId(_m_posId);
        log.setAttr(_m_attrType);
        log.setAttackHp(_m_damageHp);
        log.setName(_m_playerName);
        log.setTimeMs(_m_timeMs);
        return log;
    }
}
