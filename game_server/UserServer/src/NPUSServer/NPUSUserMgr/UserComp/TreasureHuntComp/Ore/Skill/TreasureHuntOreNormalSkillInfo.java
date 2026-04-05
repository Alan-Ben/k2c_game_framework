package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.Skill;

import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.TreasureHuntOreInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

public class TreasureHuntOreNormalSkillInfo extends _ATreasureHuntSkillInfo
{
    private TreasureHuntOreInfo _m_oreInfo;

    public TreasureHuntOreNormalSkillInfo(TreasureHuntOreInfo _oreInfo, RefTreasureHuntSkill _skillRef, int _level)
    {
        super(_skillRef, _level);
        _m_oreInfo = _oreInfo;
        initSkill();
    }

    @Override
    protected TreasureHuntComponent getComp()
    {
        return _m_oreInfo.getMgr().getComp();
    }

    @Override
    protected boolean canActive()
    {
        return true;
    }

    @Override
    protected boolean hasUpgradeCost(long _upgradeCostNum)
    {
        getUserData().lockUser();
        try
        {
            return _m_oreInfo.getBo().getNormalSkillPoint() >= _upgradeCostNum;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    protected boolean consumeUpgradeCost(long _upgradeCostNum, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (!hasUpgradeCost(_upgradeCostNum))
                return false;

            _m_oreInfo.getBo().saveNormalSkillPoint(_m_oreInfo.getBMObj(), (int) (_m_oreInfo.getBo().getNormalSkillPoint() - _upgradeCostNum));

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    protected void saveLevelToDB(int _level)
    {
        getUserData().lockUser();
        try
        {
            _m_oreInfo.getBo().saveNormalSkillLevel(_m_oreInfo.getBMObj(), _level);

            // 通知客户端更新信息
            _m_oreInfo.getMgr().getComp().getUserData().sendMsgToGC(
                    US2GCWriter_036_TreasureHuntOp.make_055_OnTreasureHuntOreSkillChg(_m_oreInfo.getOreId(), true, _m_oreInfo.makeNormalSkillInfo()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    protected void _onUpgradeSucc(NPPlayerContext _context)
    {
        getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TREASURE_HUNT_UPGRADE_ORE_NORMAL_SKILL_TIMES, 1, _context);
    }
}
