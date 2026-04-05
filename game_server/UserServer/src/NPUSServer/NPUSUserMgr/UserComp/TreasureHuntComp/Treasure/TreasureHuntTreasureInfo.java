package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure;

import Common.TreasureHuntObj.TreasureHunt_TreasureInfo;
import MJLog.MJEventLog;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasure;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasureOutput;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import USDB.Bo.PlayerTreasureHuntTreasureBO;

public class TreasureHuntTreasureInfo
{
    private TreasureHuntTreasureMgr _m_mgr;
    private PlayerTreasureHuntTreasureBO _m_bo;
    private RefTreasureHuntTreasure _m_ref;

    private TreasureHuntTreasureSkillInfo _m_skillInfo;

    public TreasureHuntTreasureInfo(TreasureHuntTreasureMgr _mgr, PlayerTreasureHuntTreasureBO _bo, RefTreasureHuntTreasure _ref)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_ref = _ref;

        // 初始化技能信息
        RefTreasureHuntSkill skillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.skill_id);
        if (skillRef != null)
            _m_skillInfo = new TreasureHuntTreasureSkillInfo(this, skillRef, _bo.getSkillLevel());
    }

    public long getTreasureId()
    {
        return _m_bo.getTreasureId();
    }

    public TreasureHuntTreasureMgr getMgr()
    {
        return _m_mgr;
    }

    public PlayerTreasureHuntTreasureBO getBo()
    {
        return _m_bo;
    }

    public RefTreasureHuntTreasure getRef()
    {
        return _m_ref;
    }

    private NPUSUserData getUserData()
    {
        return _m_mgr.getComp().getUserData();
    }

    /**
     * 激活技能
     * @return
     */
    public Result activeSkill(NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _m_skillInfo;
        if (skillInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_SKILL_NOT_EXIST;

        int beforeLevel = skillInfo.getLevel();
        Result result = skillInfo.activeSkill(_context);
        if (!result.isSucc())
            return result;

        int newLevel = skillInfo.getLevel();

        // 激活技能成功，解锁产出
        RefTreasureHuntTreasureOutput refOutput = _m_ref.getTreasureOutputRef();
        if (refOutput != null)
            _m_mgr.onTreasureOutputUnlock(refOutput);

        // 构造等级字符串，格式：{normal:1,advanced:0}（奇物都传普通等级）
        String beforeLevelStr = String.format("{normal:%d,advanced:0}", beforeLevel);
        String newLevelStr = String.format("{normal:%d,advanced:0}", newLevel);

        // 记录寻宝图鉴日志
        MJEventLog.logFishItemRecord(
            getUserData(),
            2,  // 2=奇物
            getTreasureId(),
            2,  // 2=否（奇物都传2）
            1,  // 1=解锁
            beforeLevelStr,
            newLevelStr,
            _context.getContextId()
        );

        return Result.SUCC;
    }

    /**
     * 技能升级
     * @return
     */
    public Result upgradeSkill(NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _m_skillInfo;
        if (skillInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_SKILL_NOT_EXIST;

        int beforeLevel = skillInfo.getLevel();

        Result result = skillInfo.upgradeSkill(_context);
        if (!result.isSucc())
            return result;

        int newLevel = skillInfo.getLevel();

        // 构造等级字符串，格式：{normal:1,advanced:0}（奇物都传普通等级）
        String beforeLevelStr = String.format("{normal:%d,advanced:0}", beforeLevel);
        String newLevelStr = String.format("{normal:%d,advanced:0}", newLevel);

        // 记录寻宝图鉴日志
        MJEventLog.logFishItemRecord(
            getUserData(),
            2,  // 2=奇物
            getTreasureId(),
            2,  // 2=否（奇物都传2）
            2,  // 2=升级
            beforeLevelStr,
            newLevelStr,
            _context.getContextId()
        );

        return result;
    }

    /**
     * 获取技能信息
     * @return 技能信息对象
     */
    public TreasureHuntTreasureSkillInfo getSkillInfo()
    {
        return _m_skillInfo;
    }

    public void setSkillLevel(int _level, NPPlayerContext _context)
    {
        if (_m_skillInfo != null)
        {
            _m_skillInfo.setLevel(_level);
        }
    }

    /**
     * 构造信息
     * @return
     */
    public TreasureHunt_TreasureInfo makeProto()
    {
        TreasureHunt_TreasureInfo proto = new TreasureHunt_TreasureInfo();
        proto.setTreasureId(_m_bo.getTreasureId());
        proto.setSkillLevel(_m_bo.getSkillLevel());
        proto.setGainTimeMs(_m_bo.getGainTimeMs());
        return proto;
    }
}
