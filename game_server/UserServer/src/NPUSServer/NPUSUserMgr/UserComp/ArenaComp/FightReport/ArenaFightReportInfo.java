package NPUSServer.NPUSUserMgr.UserComp.ArenaComp.FightReport;

import Common.ArenaObj.Arena_BattleReportShow;
import USDB.Bo.PlayerArenaFightReportBO;

public class ArenaFightReportInfo
{
    private ArenaFightReportMgr _m_mgr;

    private long _m_dbId;
    private long _m_opponentCid;
    private int _m_defeatHeroNum;
    private int _m_deductInfluence;
    private long _m_timestamp;
    private boolean _m_isFightBack;

    public ArenaFightReportInfo(ArenaFightReportMgr _mgr, PlayerArenaFightReportBO _bo)
    {
        _m_mgr = _mgr;
        _m_dbId = _bo.getId();
        _m_opponentCid = _bo.getOpponentCid();
        _m_defeatHeroNum = _bo.getDefeatHeroNum();
        _m_deductInfluence = _bo.getDeductInfluence();
        _m_timestamp = _bo.getTimestamp();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public long getOpponentCid()
    {
        return _m_opponentCid;
    }

    public boolean isFightBack()
    {
        return _m_isFightBack;
    }

    public void markFightBack()
    {
        _m_isFightBack = true;

        _m_mgr.getComp().getUSServer().getBM().getBM(PlayerArenaFightReportBO.class).delAll("id", _m_dbId);
    }

    public void discard()
    {
        if (_m_isFightBack)
            return;

        _m_mgr.getComp().getUSServer().getBM().getBM(PlayerArenaFightReportBO.class).delAll("id", _m_dbId);
    }

    public Arena_BattleReportShow makeProto()
    {
        Arena_BattleReportShow proto = new Arena_BattleReportShow();
        proto.setOpponentCid(_m_opponentCid);
        proto.setDefeatHeroNum(_m_defeatHeroNum);
        proto.setDeductinfluence(_m_deductInfluence);
        proto.setTimestamp(_m_timestamp);
        return proto;
    }
}
