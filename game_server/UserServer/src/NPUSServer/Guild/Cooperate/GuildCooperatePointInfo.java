package NPUSServer.Guild.Cooperate;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo;
import Common.GuildCooperateObj.GuildCooperate_RewardPointInfo;
import Common.GuildCooperateObj.GuildCooperate_RewardPointPos;
import Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout;
import CommonEnum.ESpecAttrType;
import NPCommon.ErrMain.GuildCooperateErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildCooperateArea;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import USDB.Bo.GuildCooperatePointInfoBO;

import java.util.ArrayList;
import java.util.List;

public class GuildCooperatePointInfo
{
    private GuildCooperateInfo _m_cooperateInfo;

    private long _m_dbId;
    private GuildCooperate_RewardPointPos _m_rewardPointPos;
    private long _m_posId;
    //是否击败
    private boolean _m_isDefeat;
    private long _m_leaderCid;
    private boolean _m_hadDrawGuildReward;

    //是否解锁
    private boolean _m_isUnlock;

    private RefGuildCooperateArea _m_areaRef;
    //属性据点属性类型列表
    private List<ESpecAttrType> _m_attrList;
    //已造成据点伤害值
    private List<Long> _m_damageList;

    public GuildCooperatePointInfo(GuildCooperateInfo _info, RefGuildCooperateArea _areaRef, ServerObj_GuildCooperatePropertyPointLayout _layout)
    {
        _m_cooperateInfo = _info;
        _m_areaRef = _areaRef;
        _m_attrList = _layout.getAttrType();
        _m_damageList = new ArrayList<>();
        for (int i = 0; i < _m_attrList.size(); i++)
            _m_damageList.add(0L);

        _m_rewardPointPos = new GuildCooperate_RewardPointPos(_layout.getAreaId(), _layout.getRewardPointIndex());
        _m_posId = _layout.getPosId();
    }

    /**
     * 初始化据点信息
     * @param _bo
     */
    protected void _initPointBo(GuildCooperatePointInfoBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_isDefeat = _bo.getIsUnlock();
        _m_hadDrawGuildReward = _bo.getHadDrawGuildReward();

        _m_damageList = CommonFunc.listLongFromString(_bo.getDamageList());
        //检查长度是否匹配，不匹配需要报错，且把数量补充到正确数量
        if (_m_damageList.size() != _m_attrList.size())
        {
            USLog.error(_m_cooperateInfo.getUSServer(), "GuildCooperatePointInfo _initPointBo attrList and damageList size not match, dbId:{} attrList:{} damageList:{}",
                    _m_dbId, CommonFunc.toStringList(_m_attrList), CommonFunc.toStringList(_m_damageList));

            //补充长度
            while (_m_damageList.size() < _m_attrList.size())
                _m_damageList.add(0L);
        }

    }

    public long getAreaId()
    {
        return _m_rewardPointPos.getAreaId();
    }

    public int getIndex()
    {
        return _m_rewardPointPos.getIndex();
    }

    public boolean isDefeat()
    {
        return _m_isDefeat;
    }

    public void setIsUnlock(boolean _isUnlock)
    {
        _m_isUnlock = _isUnlock;
    }

    public boolean hadDrawGuildReward()
    {
        return _m_hadDrawGuildReward;
    }

    public RefGuildCooperateArea getAreaRef()
    {
        return _m_areaRef;
    }

    public GuildCooperate_RewardPointPos getPosInfo()
    {
        return _m_rewardPointPos;
    }

    protected void _lock()
    {
        _m_cooperateInfo._lock();
    }

    protected void _unlock()
    {
        _m_cooperateInfo._unlock();
    }

    /**
     * 属性据点受到伤害
     * @param _propertyPointIndex
     * @param _damage
     * @param _context
     * @return
     */
    public Result chgPropPointHadDamage(int _propertyPointIndex, long _damage, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (_propertyPointIndex < 0 || _propertyPointIndex >= _m_attrList.size())
                return GuildCooperateErr.PROPERTY_POINT_NOT_FOUND;

            long currentDamage = _m_damageList.get(_propertyPointIndex);
            long maxHp = getPropertyPointMaxHp();

            if (currentDamage >= maxHp)
                return GuildCooperateErr.PROPERTY_POINT_ALREADY_DEFEATED;

            _m_damageList.set(_propertyPointIndex, _damage);

            // 先推送属性据点信息变化
            _m_cooperateInfo.getGuildInfo().getMemberMgr().broadcastMsg(
                    US2GCWriter_032_GuildOp.make_076_OnPropertyPointInfoChange(getPosInfo(), makePropertyPointProto(_propertyPointIndex)));

            // 检查据点是否完全被攻破并解锁
            checkAndUnlockIfComplete();

            // 同步数据库
            if (makeSureBoInsert())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("damage_list", CommonFunc.list2String(_m_damageList));
                updateValue.addValueObj("is_unlock", _m_isDefeat ? 1 : 0);
                updateValue.addValueObj("leader_cid", _m_leaderCid);
                _m_cooperateInfo.getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).update("id", _m_dbId, updateValue);
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    public static class AttackResult
    {
        public boolean isDefeat;
        public long actualDamage;
        public GuildCooperate_PropertyPointInfo newPropertyPointInfo;
        public long leaderCid;

        public AttackResult(boolean _isDefeat, long _actualDamage, GuildCooperate_PropertyPointInfo _propertyPointInfo, long _leaderCid)
        {
            isDefeat = _isDefeat;
            actualDamage = _actualDamage;
            newPropertyPointInfo = _propertyPointInfo;
            leaderCid = _leaderCid;
        }
    }

    /**
     * 进攻属性据点
     * @param _playerName
     * @param _index        进攻属性据点索引
     * @param _heroAttrType 进攻大臣属性类型
     * @param _power        进攻大臣战力
     * @return Result
     */
    public ResultOne<AttackResult> attack(String _playerName, int _index, ESpecAttrType _heroAttrType, long _power)
    {
        _lock();
        try
        {
            boolean isDefeat;
            long actualDamage;

            // 参数验证
            if (_index < 0 || _index >= _m_attrList.size())
                return ResultOne.failed(GuildCooperateErr.PROPERTY_POINT_NOT_FOUND);

            if (!_m_isUnlock)
                return ResultOne.failed(GuildCooperateErr.AREA_NOT_UNLOCKED);

            if (_m_isDefeat)
                return ResultOne.failed(GuildCooperateErr.REWARD_POINT_ALREADY_DEFEATED);

            // 计算属性匹配伤害
            ESpecAttrType targetAttrType = _m_attrList.get(_index);
            long finalDamage = calculateAttributeMatchDamage(_power, _heroAttrType, targetAttrType);

            // 获取该属性据点的血量上限
            long maxHp = getPropertyPointMaxHp();

            // 更新累计伤害（不超过血量上限）
            long currentDamage = _m_damageList.get(_index);

            if (maxHp <= currentDamage)
                return ResultOne.failed(GuildCooperateErr.PROPERTY_POINT_ALREADY_DEFEATED);

            long newDamage = Math.min(currentDamage + finalDamage, maxHp);
            actualDamage = newDamage - currentDamage;

            _m_damageList.set(_index, newDamage);

            // 记录攻击日志和更新排行榜
            _m_cooperateInfo.recordAttackLog(_playerName, _m_posId, targetAttrType, actualDamage);

            // 检查据点是否完全被攻破并解锁
            isDefeat = checkAndUnlockIfComplete();

            // 同步数据库
            if (makeSureBoInsert())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("damage_list", CommonFunc.list2String(_m_damageList));
                updateValue.addValueObj("is_unlock", _m_isDefeat ? 1 : 0);
                updateValue.addValueObj("leader_cid", _m_leaderCid);
                _m_cooperateInfo.getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).update("id", _m_dbId, updateValue);
            }

            return ResultOne.succ(new AttackResult(isDefeat, actualDamage, makePropertyPointProto(_index), _m_leaderCid));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 计算属性匹配伤害加成
     */
    private long calculateAttributeMatchDamage(long _baseDamage, ESpecAttrType _attackAttrType, ESpecAttrType _targetAttrType)
    {
        // 属性匹配时给予加成
        if (_attackAttrType == _targetAttrType)
            return (long) (_baseDamage * (10000 + RefGeneral.Ref().guild_cooperate_dispatch_same_property_add_per) / 10000.0);

        return _baseDamage;
    }

    /**
     * 获取指定属性据点的血量上限
     */
    public long getPropertyPointMaxHp()
    {
        _lock();
        try
        {
            // 奖励据点索引无效时，返回一个非常大的值，表示该据点无法被攻破
            if (getIndex() < 0 || getIndex() >= _m_areaRef.property_point_hp.size())
                return Long.MAX_VALUE;

            return _m_areaRef.property_point_hp.get(getIndex());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查所有属性据点是否攻破，如果是则解锁奖励据点
     */
    private boolean checkAndUnlockIfComplete()
    {
        _lock();
        try
        {
            if (_m_isDefeat)
                return false;

            long maxHp = getPropertyPointMaxHp();
            // 检查所有属性据点是否都达到血量上限
            for (int i = 0; i < _m_attrList.size(); i++)
            {
                long currentDamage = _m_damageList.get(i);

                if (currentDamage < maxHp)
                    return false;
            }

            // 所有据点都攻破了，解锁奖励据点
            _m_isDefeat = true;
            _m_leaderCid = _m_cooperateInfo.getGuildInfo().getMemberMgr().getLeaderId();

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保BO插入数据库
     * @return true表示已存在，false表示刚插入
     */
    private boolean makeSureBoInsert()
    {
        _lock();
        try
        {
            if (_m_dbId != 0)
                return true;

            GuildCooperatePointInfoBO bo = new GuildCooperatePointInfoBO();
            bo.setGuildId(_m_cooperateInfo.getUSServer().getBM(), _m_cooperateInfo.getGuildInfo().getGuildId());
            bo.setAreaId(_m_cooperateInfo.getUSServer().getBM(), getAreaId());
            bo.setRewardPointIndex(_m_cooperateInfo.getUSServer().getBM(), getIndex());
            bo.setIsUnlock(_m_cooperateInfo.getUSServer().getBM(), _m_isDefeat);
            bo.setLeaderCid(_m_cooperateInfo.getUSServer().getBM(), _m_leaderCid);
            bo.setHadDrawGuildReward(_m_cooperateInfo.getUSServer().getBM(), _m_hadDrawGuildReward);
            bo.setDamageList(_m_cooperateInfo.getUSServer().getBM(), CommonFunc.list2String(_m_damageList));

            bo.insert(_m_cooperateInfo.getUSServer().getBM());

            _m_dbId = bo.getId();

            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 领取公会财富奖励
     */
    public void drawGuildWealthReward(long _cid, NPPlayerContext _context)
    {
        _lock();
        try{
            if (!_m_isDefeat || _m_leaderCid != _cid || _m_hadDrawGuildReward)
                return;

            _m_hadDrawGuildReward = true;

            // 更新数据库状态
            if (makeSureBoInsert())
            {
                // 如果记录已存在，只更新领取状态字段
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("had_draw_guild_reward", _m_hadDrawGuildReward ? 1 : 0);
                _m_cooperateInfo.getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).update("id", _m_dbId, updateValue);
            }
        }finally
        {
            _unlock();
        }

        // 发放奖励
        _m_cooperateInfo.getGuildInfo().gainWealth(_m_areaRef.reward_point_guild_wealth_reward, _context);
    }

    /**
     * 构造属性据点信息协议对象
     * @param _index 属性据点索引
     */
    public GuildCooperate_PropertyPointInfo makePropertyPointProto(int _index)
    {
        _lock();
        try
        {
            if (_index < 0 || _index >= _m_attrList.size())
                return null;

            GuildCooperate_PropertyPointInfo proto = new GuildCooperate_PropertyPointInfo();
            proto.setIndex(_index);
            proto.setAttr(_m_attrList.get(_index));
            proto.setHadAttackHp(_m_damageList.get(_index));
            proto.setTotalHp(getPropertyPointMaxHp());
            return proto;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造所有属性据点信息协议对象列表
     * @return
     */
    public List<GuildCooperate_PropertyPointInfo> makeAllPropertyPointProto()
    {
        List<GuildCooperate_PropertyPointInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (int i = 0; i < _m_attrList.size(); i++)
            {
                GuildCooperate_PropertyPointInfo proto = new GuildCooperate_PropertyPointInfo();
                proto.setIndex(i);
                proto.setAttr(_m_attrList.get(i));
                if (_m_isDefeat) {
                    proto.setHadAttackHp(getPropertyPointMaxHp());
                } else {
                    proto.setHadAttackHp(_m_damageList.get(i));
                }
                proto.setTotalHp(getPropertyPointMaxHp());
                list.add(proto);
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    public GuildCooperate_RewardPointInfo makeProto()
    {
        _lock();
        try{
            GuildCooperate_RewardPointInfo proto = new GuildCooperate_RewardPointInfo();
            proto.setPos(_m_rewardPointPos);
            proto.setIsUnlock(_m_isUnlock);
            proto.setLeaderCid(_m_leaderCid);
            proto.setPosId(_m_posId);
            proto.getPropertyPointList().addAll(makeAllPropertyPointProto());
            return proto;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 强制击败奖励据点（GM命令使用）
     *
     * 执行流程：
     * 1. 检查据点是否已被击败
     * 2. 将所有属性据点的伤害设置为满血
     * 3. 设置据点为击败状态
     * 4. 更新数据库
     *
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result forceDefeat(NPPlayerContext _context)
    {
        _lock();
        try
        {
            // 检查是否已击败
            if (_m_isDefeat)
                return GuildCooperateErr.REWARD_POINT_ALREADY_DEFEATED;

            // 将所有属性据点伤害设置为满血
            long maxHp = getPropertyPointMaxHp();
            for (int i = 0; i < _m_damageList.size(); i++)
            {
                if (_m_damageList.get(i) >= maxHp)
                    continue;

                _m_damageList.set(i, maxHp);

                // 先推送属性据点信息变化
                _m_cooperateInfo.getGuildInfo().getMemberMgr().broadcastMsg(
                        US2GCWriter_032_GuildOp.make_076_OnPropertyPointInfoChange(getPosInfo(), makePropertyPointProto(i)));
            }

            // 设置为击败状态
            _m_isDefeat = true;
            _m_leaderCid = _m_cooperateInfo.getGuildInfo().getMemberMgr().getLeaderId();

            // 同步数据库
            if (makeSureBoInsert())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("damage_list", CommonFunc.list2String(_m_damageList));
                updateValue.addValueObj("is_unlock", _m_isDefeat ? 1 : 0);
                updateValue.addValueObj("leader_cid", _m_leaderCid);
                _m_cooperateInfo.getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).update("id", _m_dbId, updateValue);
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }
}
