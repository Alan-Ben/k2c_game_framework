package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.Skill;

import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.TreasureHuntCompositeInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

public class TreasureHuntCompositeAdvancedSkillInfo extends _ATreasureHuntSkillInfo
{
    private TreasureHuntCompositeInfo _m_compositeInfo;

    public TreasureHuntCompositeAdvancedSkillInfo(TreasureHuntCompositeInfo _compositeInfo, RefTreasureHuntSkill _skillRef, int _level)
    {
        super(_skillRef, _level);
        _m_compositeInfo = _compositeInfo;
        initSkill();
    }

    @Override
    protected TreasureHuntComponent getComp()
    {
        return _m_compositeInfo.getMgr().getComp();
    }

    @Override
    protected boolean canActive()
    {
        return _m_compositeInfo.hadCollectAdvancedOre();
    }

    @Override
    protected boolean hasUpgradeCost(long _upgradeCostNum)
    {
        return false;
    }

    @Override
    protected boolean consumeUpgradeCost(long _upgradeCostNum, NPPlayerContext _context)
    {
        return false;
    }

    @Override
    protected void saveLevelToDB(int _level)
    {
        getUserData().lockUser();
        try
        {
            _m_compositeInfo.getBo().saveIsAdvancedSkillActive(_m_compositeInfo.getBMObj(), _level > 0);

            // 通知客户端更新信息
            _m_compositeInfo.getMgr().getComp().getUserData().sendMsgToGC(
                    US2GCWriter_036_TreasureHuntOp.make_056_OnTreasureCompositeChg(_m_compositeInfo.makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
