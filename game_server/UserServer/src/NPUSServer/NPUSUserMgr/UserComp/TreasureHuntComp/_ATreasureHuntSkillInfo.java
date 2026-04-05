package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkillLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public abstract class _ATreasureHuntSkillInfo
{
    private RefTreasureHuntSkill _m_skillRef;
    private RefTreasureHuntSkillLevel _m_skillLevelRef;

    private int _m_level;

    public _ATreasureHuntSkillInfo(RefTreasureHuntSkill _skillRef, int _level)
    {
        _m_skillRef = _skillRef;
        _m_level = _level;
        _m_skillLevelRef = _m_skillRef.getSkillLevel(_m_level);
    }

    protected void initSkill()
    {
        // 如果技能已激活，则添加技能属性和奖励
        if (_m_skillLevelRef != null)
        {
            getComp().getPropertyContainer().addModifier(_m_skillLevelRef.add_player);
            getComp().getUserData().getBonusMgr().addBonus(_m_skillLevelRef.union_bonus);
        }
    }

    public NPUSUserData getUserData()
    {
        return getComp().getUserData();
    }

    /**
     * 激活技能
     * @param _context
     * @return
     */
    public Result activeSkill(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_level != 0)
                return TreasureHuntErr.TREASURE_HUNT_SKILL_HAD_ACTIVE;

            int newLevel = 1;

            RefTreasureHuntSkillLevel nextLevelRef = _m_skillRef.getSkillLevel(1);
            if (nextLevelRef == null)
                return CommErr.REF_NOT_FOUND;

            if (!canActive())
                return TreasureHuntErr.TREASURE_HUNT_SKILL_ACTIVE_FAIL;

            _chgLevel(newLevel, nextLevelRef);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 等级变更处理
     * @param newLevel
     * @param nextLevelRef
     */
    private void _chgLevel(int newLevel, RefTreasureHuntSkillLevel nextLevelRef)
    {
        getUserData().lockUser();
        try
        {
            RefTreasureHuntSkillLevel oriLevelRef = _m_skillLevelRef;

            _m_level = newLevel;
            _m_skillLevelRef = nextLevelRef;

            // 替换技能属性和奖励
            getComp().getPropertyContainer().replaceModifier(
                    oriLevelRef == null ? null : oriLevelRef.add_player, _m_skillLevelRef == null ? null : _m_skillLevelRef.add_player);
            getComp().getUserData().getBonusMgr().replaceBonus(
                    oriLevelRef == null ? null : oriLevelRef.union_bonus, _m_skillLevelRef == null ? null : _m_skillLevelRef.union_bonus);

            saveLevelToDB(_m_level);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 升级技能
     * @param _context
     * @return
     */
    public Result upgradeSkill(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_skillLevelRef == null)
                return CommErr.REF_NOT_FOUND;

            int newLevel = _m_level + 1;

            RefTreasureHuntSkillLevel nextLevelRef = _m_skillRef.getSkillLevel(newLevel);
            if (nextLevelRef == null)
                return CommErr.REF_NOT_FOUND;

            if (!hasUpgradeCost(_m_skillLevelRef.upgrade_cost_num))
                return CommErr.ITEM_NOT_ENOUGH;

            if (!consumeUpgradeCost(_m_skillLevelRef.upgrade_cost_num, _context))
                return CommErr.CONSUME_FAIL;

            _chgLevel(newLevel, nextLevelRef);

            _onUpgradeSucc(_context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    protected void _onUpgradeSucc(NPPlayerContext _context)
    {

    }

    protected abstract TreasureHuntComponent getComp();

    protected abstract boolean canActive();

    protected abstract boolean hasUpgradeCost(long _upgradeCostNum);

    protected abstract boolean consumeUpgradeCost(long _upgradeCostNum, NPPlayerContext _context);

    protected abstract void saveLevelToDB(int _level);

    /**
     * 获取技能id
     * @return 技能id
     */
    public long getSkillId()
    {
        return _m_skillRef.Id();
    }

    /**
     * 获取技能等级
     * @return 技能等级
     */
    public int getLevel()
    {
        return _m_level;
    }

    /**
     * 设置技能等级（GM命令使用）
     * @param _level 目标等级
     */
    public void setLevel(int _level)
    {
        getUserData().lockUser();
        try
        {
            RefTreasureHuntSkillLevel newLevelRef = _m_skillRef.getSkillLevel(_level);
            if (newLevelRef == null && _level > 0)
                return;

            _chgLevel(_level, newLevelRef);
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
