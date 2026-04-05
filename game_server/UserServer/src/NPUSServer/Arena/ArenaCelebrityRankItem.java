package NPUSServer.Arena;

import Common.ArenaObj.Arena_CelebrityRankInfo;
import USDB.Bo.ArenaCelebrityRankBO;

public class ArenaCelebrityRankItem
{
    private ArenaCelebrityRankMgr _m_mgr;
    private ArenaCelebrityRankBO _m_bo;

    public ArenaCelebrityRankItem(ArenaCelebrityRankMgr _mgr, ArenaCelebrityRankBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
    }

    public long getDbId()
    {
        return _m_bo.getId();
    }

    public void discard()
    {
        _m_bo.del(_m_mgr.getServer().getBM());
    }

    public Arena_CelebrityRankInfo makeProto()
    {
        Arena_CelebrityRankInfo proto = new Arena_CelebrityRankInfo();
        proto.setDbId(_m_bo.getId());
        proto.setAttackerCid(_m_bo.getAttackerCid());
        proto.setAttackerName(_m_bo.getAttackerName());
        proto.setDefenderName(_m_bo.getDefenderName());
        proto.setDefeatHeroNum(_m_bo.getDefeatHeroNum());
        proto.setIsSelectAttack(_m_bo.getIsSelectAttack());
        proto.setTimeMs(_m_bo.getTimestamp());
        return proto;
    }
}
