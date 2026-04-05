package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure;

import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

public class TreasureHuntTreasureSkillInfo extends _ATreasureHuntSkillInfo
{
    private TreasureHuntTreasureInfo _m_treasureInfo;

    public TreasureHuntTreasureSkillInfo(TreasureHuntTreasureInfo _treasureInfo, RefTreasureHuntSkill _skillRef, int _level)
    {
        super(_skillRef, _level);
        _m_treasureInfo = _treasureInfo;
        initSkill();
    }

    @Override
    protected TreasureHuntComponent getComp()
    {
        return _m_treasureInfo.getMgr().getComp();
    }

    @Override
    protected boolean canActive()
    {
        return true;
    }

    @Override
    protected boolean hasUpgradeCost(long _upgradeCostNum)
    {
        return getComp().getUserData().hasItem(_m_treasureInfo.getRef().upgrade_cost, _upgradeCostNum);
    }

    @Override
    protected boolean consumeUpgradeCost(long _upgradeCostNum, NPPlayerContext _context)
    {
        return getComp().getUserData().spendItem(_m_treasureInfo.getRef().upgrade_cost, _upgradeCostNum, _context);
    }

    @Override
    protected void saveLevelToDB(int _level)
    {
        getUserData().lockUser();
        try
        {
            _m_treasureInfo.getBo().saveSkillLevel(getComp().getUSServer().getBM(), _level);

            // 通知客户端更新信息
            _m_treasureInfo.getMgr().getComp().getUserData().sendMsgToGC(
                    US2GCWriter_036_TreasureHuntOp.make_052_OnTreasureHuntTreasureLevelChg(_m_treasureInfo.getTreasureId(), _level));
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
