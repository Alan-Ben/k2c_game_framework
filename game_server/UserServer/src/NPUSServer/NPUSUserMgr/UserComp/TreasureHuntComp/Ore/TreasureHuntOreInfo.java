package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore;

import Common.TreasureHuntObj.TreasureHunt_OreInfo;
import Common.TreasureHuntObj.TreasureHunt_OreNumInfo;
import Common.TreasureHuntObj.TreasureHunt_OreSkillInfo;
import Common.TreasureHuntObj.TreasureHunt_TransOreResult;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntOre;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPGameRes.Refs.TreasureHunt.TreasureHuntMassGradeList.MassGradeItem;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.Skill.TreasureHuntOreAdvancedSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.Skill.TreasureHuntOreNormalSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import USDB.Bo.PlayerTreasureHuntOreBO;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntOreInfo
{
    private TreasureHuntOreMgr _m_mgr;
    private PlayerTreasureHuntOreBO _m_bo;
    private RefTreasureHuntOre _m_ref;
    // 已领取的记录奖励列表
    private List<Integer> _m_hadDrawRecordRewardList;

    private TreasureHuntOreNormalSkillInfo _m_normalSkillInfo;
    private TreasureHuntOreAdvancedSkillInfo _m_advancedSkillInfo;

    public TreasureHuntOreInfo(TreasureHuntOreMgr _mgr, PlayerTreasureHuntOreBO _bo, RefTreasureHuntOre _ref)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_ref = _ref;
        _m_hadDrawRecordRewardList = new ArrayList<>();
        // 读取已领取的记录奖励列表
        _m_hadDrawRecordRewardList.addAll(CommonFunc.listIntFromString(_bo.getHadDrawRecordRewardList()));

        // 初始化技能信息
        RefTreasureHuntSkill normalSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.normal_skill_id);
        if (normalSkillRef != null)
            _m_normalSkillInfo = new TreasureHuntOreNormalSkillInfo(this, normalSkillRef, _bo.getNormalSkillLevel());

        RefTreasureHuntSkill advancedSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.advanced_skill_id);
        if (advancedSkillRef != null)
            _m_advancedSkillInfo = new TreasureHuntOreAdvancedSkillInfo(this, advancedSkillRef, _bo.getAdvancedSkillLevel());
    }

    public RefTreasureHuntOre getRef()
    {
        return _m_ref;
    }

    public long getOreId()
    {
        return _m_bo.getOreId();
    }

    public boolean hadReachAdvanced()
    {
        return _m_bo.getHadReachAdvanced();
    }

    public BM getBMObj()
    {
        return _m_mgr.getComp().getUSServer().getBM();
    }

    public PlayerTreasureHuntOreBO getBo()
    {
        return _m_bo;
    }

    public TreasureHuntOreMgr getMgr()
    {
        return _m_mgr;
    }

    public NPUSUserData getUserData()
    {
        return _m_mgr.getComp().getUserData();
    }

    public int getPendingNum()
    {
        return _m_bo.getNormalPendingNum() + _m_bo.getAdvancedPendingNum();
    }

    /**
     * 领取记录奖励
     * @param _recordIndex 记录索引，从0开始
     * @param _context     玩家上下文
     * @return
     */
    public Result drawRecordReward(int _recordIndex, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否已经领取过该奖励
            if (_m_hadDrawRecordRewardList.contains(_recordIndex))
                return TreasureHuntErr.TREASURE_HUNT_ORE_RECORD_REWARD_HAD_DRAW;

            // 获取当前记录的索引
            int recordIndex = _m_ref.mass_reward_grade_list.getGradeIndexByWeight(_m_bo.getMaxRecord());
            if (recordIndex < _recordIndex)
                return TreasureHuntErr.TREASURE_HUNT_ORE_RECORD_REWARD_NOT_REACHED;

            // 判断是否有奖励
            MassGradeItem gradeItem = _m_ref.mass_reward_grade_list.getGradeByIndex(_recordIndex);
            if (gradeItem == null || gradeItem._m_reward == null)
                return TreasureHuntErr.TREASURE_HUNT_ORE_RECORD_REWARD_NOT_EXIST;

            getUserData().gainItem(gradeItem._m_reward, _context);

            // 添加到已领取列表
            _m_hadDrawRecordRewardList.add(_recordIndex);

            _m_bo.saveHadDrawRecordRewardList(getBMObj(), CommonFunc.list2String(_m_hadDrawRecordRewardList));

            // 通知客户端更新矿石信息
            _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_059_OnTreasureHuntOreHadDrawRecordRewardChg(_m_bo.getOreId(), _m_hadDrawRecordRewardList));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 激活技能
     * @param _isAdvanced 是否是高级技能
     * @return
     */
    public Result activeSkill(boolean _isAdvanced, NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _isAdvanced ? _m_advancedSkillInfo : _m_normalSkillInfo;
        if (skillInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_SKILL_NOT_EXIST;

        int beforeNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int beforeAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        Result result = skillInfo.activeSkill(_context);
        if (!result.isSucc())
            return result;

        int afterNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int afterAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        // 记录激活矿石技能次数
        getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TREASURE_HUNT_ACTIVE_ORE_SKILL_TIMES, 1, _context);

        // 构造等级字符串，格式：{normal:1,advanced:0}
        String beforeLevelStr = String.format("{normal:%d,advanced:%d}", beforeNormalLevel, beforeAdvancedLevel);
        String newLevelStr = String.format("{normal:%d,advanced:%d}", afterNormalLevel, afterAdvancedLevel);

        // 记录寻宝图鉴日志
        MJEventLog.logFishItemRecord(
            getUserData(),
            1,  // 1=矿石
            getOreId(),
            _isAdvanced ? 1 : 2,  // 1=是高级；2=否
            1,  // 1=解锁
            beforeLevelStr,
            newLevelStr,
            _context.getContextId()
        );

        return result;
    }

    /**
     * 技能升级
     * @param _isAdvanced 是否是高级技能
     * @return
     */
    public Result upgradeSkill(boolean _isAdvanced, NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _isAdvanced ? _m_advancedSkillInfo : _m_normalSkillInfo;
        if (skillInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_SKILL_NOT_EXIST;

        int beforeNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int beforeAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        Result result = skillInfo.upgradeSkill(_context);
        if (!result.isSucc())
            return result;

        int afterNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int afterAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        // 构造等级字符串，格式：{normal:1,advanced:0}
        String beforeLevelStr = String.format("{normal:%d,advanced:%d}", beforeNormalLevel, beforeAdvancedLevel);
        String newLevelStr = String.format("{normal:%d,advanced:%d}", afterNormalLevel, afterAdvancedLevel);

        // 记录寻宝图鉴日志
        MJEventLog.logFishItemRecord(
            getUserData(),
            1,  // 1=矿石
            getOreId(),
            _isAdvanced ? 1 : 2,  // 1=是高级；2=否
            2,  // 2=升级
            beforeLevelStr,
            newLevelStr,
            _context.getContextId()
        );

        return result;
    }

    /**
     * 获得新矿石处理
     * @param _weight
     * @param _context
     */
    public boolean onGainNewOre(int _weight, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            boolean isFirstTimeDrawAdvanced = false;

            // 判断是否是高级矿石
            if (!_m_ref.isAdvanced(_weight))
            {
                // 如果不是高级矿石，则增加普通矿石数量
                _m_bo.setNormalPendingNum(getBMObj(), _m_bo.getNormalPendingNum() + 1);
            } else
            {
                //如果是高级矿石，且之前已经获得过高级矿石，则增加高级矿石数量；否则，不增加次数，当做消耗获得激活高级技能资格
                if (_m_bo.getHadReachAdvanced())
                {
                    _m_bo.setAdvancedPendingNum(getBMObj(), _m_bo.getAdvancedPendingNum() + 1);
                } else
                {
                    _m_bo.setHadReachAdvanced(getBMObj(), true);
                    isFirstTimeDrawAdvanced = true;
                }
            }

            boolean reachNewRecord = _weight > _m_bo.getMaxRecord();
            if (reachNewRecord)
            {
                _m_bo.setMaxRecord(getBMObj(), _weight);
                _m_bo.setReachMaxRecordTimeMs(getBMObj(), CommonFunc.getNowTimeMS());
            }
            _m_bo.setTotalGainNum(getBMObj(), _m_bo.getTotalGainNum() + 1);
            _m_bo.saveAllMarked(getBMObj());

            // 通知客户端更新矿石信息
            _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_054_OnTreasureHuntOreNumChg(_m_bo.getOreId(), makeNumInfo()));

            // 如果创造新纪录
            if (reachNewRecord)
                _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_058_OnTreasureHuntOreMaxRecordChg(_m_bo.getOreId(), _weight));

            return isFirstTimeDrawAdvanced;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转换待处理矿石
     * @param _context
     * @return
     */
    public void transPendingOre(List<TreasureHunt_TransOreResult> _resultList, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 检查是否有待处理的普通矿石
            if (_m_bo.getNormalPendingNum() <= 0 && _m_bo.getAdvancedPendingNum() <= 0)
                return;

            boolean isNormalTrans = _m_bo.getNormalPendingNum() > 0;
            int normalTransNum = 0;
            if (isNormalTrans)
            {
                normalTransNum = _m_bo.getNormalPendingNum();
                _resultList.add(new TreasureHunt_TransOreResult(getOreId(), true, normalTransNum));
                _m_bo.setNormalSkillPoint(getBMObj(), _m_bo.getNormalSkillPoint() + normalTransNum);
                _m_bo.setNormalPendingNum(getBMObj(), 0);
            }

            boolean isAdvancedTrans = _m_bo.getAdvancedPendingNum() > 0;
            int advancedTransNum = 0;
            if (isAdvancedTrans)
            {
                advancedTransNum = _m_bo.getAdvancedPendingNum();
                _resultList.add(new TreasureHunt_TransOreResult(getOreId(), false, advancedTransNum));
                _m_bo.setAdvancedSkillPoint(getBMObj(), _m_bo.getAdvancedSkillPoint() + advancedTransNum);
                _m_bo.setAdvancedPendingNum(getBMObj(), 0);
            }
            _m_bo.saveAllMarked(getBMObj());

            // 通知客户端更新矿石信息
            _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_054_OnTreasureHuntOreNumChg(_m_bo.getOreId(), makeNumInfo()));

            // 通知客户端更新技能信息
            if (isNormalTrans)
                _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_055_OnTreasureHuntOreSkillChg(_m_bo.getOreId(), true, makeNormalSkillInfo()));
            if (isAdvancedTrans)
                _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_055_OnTreasureHuntOreSkillChg(_m_bo.getOreId(), false, makeAdvancedSkillInfo()));

            // 记录矿石处理日志（普通和高级矿石处理）
            MJEventLog.logOre(getUserData(), getOreId(), _m_ref.quality.ordinal(),
                    normalTransNum, advancedTransNum);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造矿石数量信息
     * @return
     */
    public TreasureHunt_OreNumInfo makeNumInfo()
    {
        TreasureHunt_OreNumInfo proto = new TreasureHunt_OreNumInfo();
        proto.setTotalGainNum(_m_bo.getTotalGainNum());
        proto.setNormalPendingNum(_m_bo.getNormalPendingNum());
        proto.setAdvancedPendingNum(_m_bo.getAdvancedPendingNum());
        return proto;
    }

    /**
     * 构造普通技能信息
     * @return
     */
    public TreasureHunt_OreSkillInfo makeNormalSkillInfo()
    {
        TreasureHunt_OreSkillInfo proto = new TreasureHunt_OreSkillInfo();
        proto.setSkillLevel(_m_bo.getNormalSkillLevel());
        proto.setSkillPoint(_m_bo.getNormalSkillPoint());
        return proto;
    }

    /**
     * 构造高级技能信息
     * @return
     */
    public TreasureHunt_OreSkillInfo makeAdvancedSkillInfo()
    {
        TreasureHunt_OreSkillInfo proto = new TreasureHunt_OreSkillInfo();
        proto.setSkillLevel(_m_bo.getAdvancedSkillLevel());
        proto.setSkillPoint(_m_bo.getAdvancedSkillPoint());
        return proto;
    }

    /**
     * 获取普通技能信息
     * @return 普通技能信息对象
     */
    public TreasureHuntOreNormalSkillInfo getNormalSkillInfo()
    {
        return _m_normalSkillInfo;
    }

    /**
     * 获取高级技能信息  
     * @return 高级技能信息对象
     */
    public TreasureHuntOreAdvancedSkillInfo getAdvancedSkillInfo()
    {
        return _m_advancedSkillInfo;
    }

    /**
     * 设置技能等级（GM命令用）
     * @param _isAdvanced
     * @param _level
     * @param _context
     */
    public void setSkillLevel(boolean _isAdvanced, int _level, NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _isAdvanced ? _m_advancedSkillInfo : _m_normalSkillInfo;
        if (skillInfo != null)
        {
            skillInfo.setLevel(_level);
        }
    }

    /**
     * 设置技能点数（GM命令用）
     * @param _isAdvanced
     * @param _point
     * @param _context
     */
    public void setSkillPoint(boolean _isAdvanced, int _point, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_isAdvanced)
            {
                _m_bo.saveAdvancedSkillPoint(getBMObj(), _point);

                _m_mgr.getComp().getUserData().sendMsgToGC(
                        US2GCWriter_036_TreasureHuntOp.make_055_OnTreasureHuntOreSkillChg(_m_bo.getOreId(), false, makeAdvancedSkillInfo()));
            } else
            {
                _m_bo.saveNormalSkillPoint(getBMObj(), _point);

                _m_mgr.getComp().getUserData().sendMsgToGC(
                        US2GCWriter_036_TreasureHuntOp.make_055_OnTreasureHuntOreSkillChg(_m_bo.getOreId(), true, makeNormalSkillInfo()));
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重置高级技能（GM命令用）
     * @param _context
     */
    public void resetAdvancedSkill(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 重置高级技能等级为0
            if (_m_advancedSkillInfo != null)
                _m_advancedSkillInfo.setLevel(0);

            // 重置高级技能解锁状态为false
            _m_bo.setMaxRecord(getBMObj(), 1000);
            _m_bo.setHadReachAdvanced(getBMObj(), false);
            _m_bo.saveAllMarked(getBMObj());

            // 通知客户端更新矿石信息
            _m_mgr.getComp().getUserData().sendMsgToGC(
                    US2GCWriter_036_TreasureHuntOp.make_058_OnTreasureHuntOreMaxRecordChg(_m_bo.getOreId(), 1));
        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 构造矿石信息协议对象
     * @return
     */
    public TreasureHunt_OreInfo makeProto()
    {
        TreasureHunt_OreInfo proto = new TreasureHunt_OreInfo();
        proto.setOreId(_m_bo.getOreId());
        proto.setFirstGainTimeMs(_m_bo.getFirstGainTimeMs());
        proto.setMaxRecord(_m_bo.getMaxRecord());
        getUserData().lockUser();
        try
        {
            proto.getHadDrawRecordRewardList().addAll(_m_hadDrawRecordRewardList);
        } finally
        {
            getUserData().unlockUser();
        }
        proto.setNumInfo(makeNumInfo());
        proto.setNormalSkillInfo(makeNormalSkillInfo());
        proto.setAdvancedSkillInfo(makeAdvancedSkillInfo());
        return proto;
    }
}
