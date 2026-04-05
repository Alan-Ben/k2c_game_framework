package NPUSServer.NPUSUserMgr.UserComp.EquipComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.HeroObj.Equip_BaseInfo;
import Common.HeroObj.Equip_Info;
import Common.HeroObj.Equip_SkillInfo;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Equip.RefEquip;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_EQUIP_UPGRADE;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import USDB.Bo.PlayerEquipBO;
import USDB.Bo.PlayerEquipSkillBO;

import java.util.ArrayList;
import java.util.List;

public class EquipInfo
{
    private EquipComponent _m_comp;
    private long _m_dbId;
    private RefEquip _m_ref;
    private int _m_level;
    private int _m_awakenLevel;
    private long _m_wearHeroId;
    private List<EquipSkillInfo> _m_skillList;
    private boolean _m_isLocked;

    private HeroInfo _m_heroInfo;

    //技能加成值
    private int _m_avaSkillAddValue;

    public EquipInfo(EquipComponent _comp, PlayerEquipBO _bo, RefEquip _ref)
    {
        _m_comp = _comp;
        _m_dbId = _bo.getId();
        _m_ref = _ref;
        _m_level = _bo.getLevel();
        _m_awakenLevel = _bo.getAwakenLevel();
        _m_wearHeroId = _bo.getWearHeroId();
        _m_skillList = new ArrayList<>();
        _m_isLocked = _bo.getIsLocked();

        _m_heroInfo = _m_comp.getUserData().getHeroComponent().lookupHero(_m_wearHeroId);
    }

    public EquipComponent getComp()
    {
        return _m_comp;
    }

    public RefEquip getRef()
    {
        return _m_ref;
    }

    public int getLevel()
    {
        return _m_level;
    }

    public int getAwakenLevel()
    {
        return _m_awakenLevel;
    }

    public long getWearHeroId()
    {
        return _m_wearHeroId;
    }

    public long getEquipId()
    {
        return getRef().id;
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public boolean isLocked()
    {
        return _m_isLocked;
    }

    public boolean isWeared()
    {
        return _m_wearHeroId != 0;
    }
    
    public HeroInfo getWearedHero()
    {
    	return _m_heroInfo;
    }

    /**
     * 获取技能总数
     */
    public int getCanUnlockSkillCount()
    {
        return _m_ref.initial_skill_num + _m_ref.getAwakenSkillNum(_m_awakenLevel);
    }

    /**
     * 获取资质点数
     */
    public int getAddTalentPoint()
    {
        return _m_ref.initial_talent + _m_ref.upgrade_increase_talent * (_m_level - 1);
    }

    /**
     * 初始化技能
     * @param _bo
     */
    protected void _initSkill(PlayerEquipSkillBO _bo)
    {
        _m_skillList.add(new EquipSkillInfo(_bo));

        //更新技能加成值
        _m_avaSkillAddValue += _bo.getValue();
    }

    /**
     * 检查解锁技能
     */
    protected void _initNewCheckUnlockSkill()
    {
        //需要解锁的技能数量
        int needUnlockNum = getCanUnlockSkillCount() - _m_skillList.size();
        if (needUnlockNum <= 0)
            return;

        for (int i = 0; i < needUnlockNum; i++)
        {
            //随机技能属性
            int value = _randomSkillValue(false);

            PlayerEquipSkillBO bo = new PlayerEquipSkillBO();
            bo.setCid(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getCid());
            bo.setEquipDbId(_m_comp.getUSServer().getBM(), _m_dbId);
            bo.setIndex(_m_comp.getUSServer().getBM(), _m_skillList.size());
            bo.setValue(_m_comp.getUSServer().getBM(), value);
            bo.insert(_m_comp.getUSServer().getBM());

            _m_skillList.add(new EquipSkillInfo(bo));

            //更新技能加成值
            _m_avaSkillAddValue += value;
        }
    }

    /**
     * 查找技能
     * @param _index
     * @return
     */
    public EquipSkillInfo lookSkillInfo(int _index)
    {
        for (EquipSkillInfo skillInfo : _m_skillList)
        {
            if (skillInfo.getIndex() == _index)
                return skillInfo;
        }
        return null;
    }

    /**
     * 随机技能数值
     * @return
     */
    protected int _randomSkillValue(boolean _isAdvance)
    {
        //获取随机组id
        int addProGroupId = _isAdvance ? RefGeneral.Ref().equip_advance_add_pro_group_id : RefGeneral.Ref().equip_normal_add_pro_group_id;
        //随机技能加成值
        return RefGeneral.Ref().randomProAdd(addProGroupId);
    }

    /**
     * 升级
     * @return
     */
    public Result upgrade(boolean _isTen, NPPlayerContext _context)
    {
        //计算上限
        int levelLimit = (int) (_m_ref.level_limit
                + _m_comp.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.EQUIP_LEVEL_LIMIT_ADD));

        //检查是否满级
        if (levelLimit != 0 && _m_level >= levelLimit)
            return HeroErr.EQUIP_LEVEL_MAX;

        //还可以升级的次数
        int limit = levelLimit - _m_level;
        int canUpgradeNum = _isTen ? Math.min(limit, 10) : 1;
        //区分十连和单次
        boolean consumeSuc = _m_comp.getUserData().spendItem(_m_ref.cost_item.getItemType(),
                _m_ref.cost_item.getItemId(), _m_ref.cost_item.getCount() * canUpgradeNum, _context);
        if (!consumeSuc)
            return CommErr.ITEM_NOT_ENOUGH;

        //记录升级前的状态
        int initialLevel = _m_level;
        int initialAptitude = getAddTalentPoint();

        //保存数据
        _setLevel(_m_level + canUpgradeNum);

        //记录升级后的状态
        int finalLevel = _m_level;
        int finalAptitude = getAddTalentPoint();

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        //如果有穿戴英雄，更新属性
        if (_m_heroInfo != null)
            _m_heroInfo.recalHero();

        _m_comp.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.EQUIP_UPGRADE_TIMES, canUpgradeNum, _context);

        _m_comp.getUserData().onLogicEvent(new Event_P_EQUIP_UPGRADE(_context, getEquipId(), canUpgradeNum));

        //记录日志
        MJEventLog.logArtifactUpdate(
                _m_comp.getUserData(),
                getEquipId(),
                _m_dbId,
                _m_wearHeroId,
                initialLevel,
                initialAptitude,
                finalLevel,
                finalAptitude
        );

        return Result.SUCC;
    }

    /**
     * 修改等级
     * @param _newLevel
     */
    private void _setLevel(int _newLevel)
    {
        _m_level = _newLevel;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("level", _m_level);
        _m_comp.getUSServer().getBM().getBM(PlayerEquipBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 修改穿戴英雄
     * @return
     */
    public Result chgWearHero(long _heroId, NPPlayerContext _context)
    {
        //检查是否有变化
        if (_heroId == _m_wearHeroId)
        {
            //同步状态推送
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));
            return Result.SUCC;
        }

        //查询目标大臣
        HeroInfo heroInfo = _m_comp.getUserData().getHeroComponent().lookupHero(_heroId);
        if (heroInfo == null)
            return HeroErr.HERO_NOT_FOUND;

        //检查是否已经穿戴，如果已经穿戴，先卸下
        EquipInfo heroOriEquip = heroInfo.getEquip();
        if (heroOriEquip != null)
            heroOriEquip.unWearHero(_context);

        //原大臣
        HeroInfo preHero = _m_heroInfo;

        //穿戴
        _m_heroInfo = heroInfo;
        _setWearHero(_heroId);

        //原大臣卸下
        if (preHero != null)
            preHero.setEquip(null, false);

        //新大臣穿戴
        _m_heroInfo.setEquip(this, false);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        _m_comp.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.HERO_WEAR_EQUIP_TIMES, 1, _context);

        return Result.SUCC;
    }

    /**
     * 修改穿戴英雄
     * @param _heroId 英雄id
     */
    private void _setWearHero(long _heroId)
    {
        _m_wearHeroId = _heroId;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("wear_hero_id", _m_wearHeroId);
        _m_comp.getUSServer().getBM().getBM(PlayerEquipBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 从穿戴英雄卸下
     */
    public Result unWearHero(NPPlayerContext _context)
    {
        //检查是否有英雄穿戴
        if (_m_wearHeroId == 0)
            return HeroErr.EQUIP_IS_NOT_BEEN_WEAR;

        //卸下
        HeroInfo preHero = _m_heroInfo;
        _m_heroInfo = null;
        _setWearHero(0);

        //移除属性加成
        if (preHero != null)
            preHero.setEquip(null, false);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        return Result.SUCC;
    }

    /**
     * 放置到大臣
     */
    public void onPlaceToHero()
    {
        _m_heroInfo = _m_comp.getUserData().getHeroComponent().lookupHero(_m_wearHeroId);
        if (_m_heroInfo != null)
            _m_heroInfo.setEquip(this, true);
    }

    /**
     * 获取属性加成
     * @return
     */
    public int getAddPowerPerValue()
    {
        return _m_avaSkillAddValue;
    }

    /**
     * 觉醒升级
     * @return
     */
    public Result awakenUpgrade(NPPlayerContext _context)
    {

        return Result.SUCC;
    }

    /**
     * 重塑技能
     * @param _index 位置
     */
    public Result reshapeSkill(int _index, boolean _isAdvance, NPPlayerContext _context)
    {
        //获取技能
        EquipSkillInfo skillInfo = lookSkillInfo(_index);
        if (skillInfo == null)
            return HeroErr.EQUIP_SKILL_NOT_FOUND;

        int oriAddValue = skillInfo.getValue();

        Result result = skillInfo.reshapeSkill(this, _isAdvance, _context);
        if (!result.isSucc())
            return result;

        int curAddValue = skillInfo.getValue();
        //如果技能加成值变化，需要更新技能加成值
        if (curAddValue != oriAddValue)
        {
            //更新技能加成值
            _m_avaSkillAddValue += (curAddValue - oriAddValue);

            //推送变更
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

            //如果有穿戴英雄，更新属性
            if (_m_heroInfo != null)
                _m_heroInfo.recalHero();
        }

        return Result.SUCC;
    }

    /**
     * 分解藏品
     */
    public void disassemble(NPItemCostCollector_nosafe _totalReturnItemList,
                            List<NPCommonCostItem> _logReturnItemList, NPPlayerContext _context)
    {
        //获取分解获得的道具
        _totalReturnItemList.addItemList(_m_ref.disassemble_get_item_list);
        _logReturnItemList.addAll(_m_ref.disassemble_get_item_list);

        //获得升级消耗的道具
        int upgradeTimes = _m_level - 1;
        if (upgradeTimes > 0)
        {
            NPCommonCostItem costItem = CommonFunc.itemMultiple(_m_ref.cost_item, upgradeTimes);

            _totalReturnItemList.addItem(costItem);
            _logReturnItemList.add(costItem);
        }

        //删除藏品
        _m_comp.getUSServer().getBM().getBM(PlayerEquipBO.class).delAll("id", _m_dbId);
        //删除技能数据
        _m_comp.getUSServer().getBM().getBM(PlayerEquipSkillBO.class).delAll("equip_db_id", _m_dbId);
    }

    /**
     * 修改锁定状态
     * @param _isLock
     * @param _context
     * @return
     */
    public Result chgLockState(boolean _isLock, NPPlayerContext _context)
    {
        //检查是否有变化
        if (_isLock == _m_isLocked)
        {
            //同步状态推送
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));
            return Result.SUCC;
        }

        _m_isLocked = _isLock;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("is_locked", _m_isLocked ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerEquipBO.class).update("id", _m_dbId, updateValue);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        return Result.SUCC;
    }

    /**
     * 构造基础信息
     * @return
     */
    public Equip_BaseInfo makeBaseInfo()
    {
        Equip_BaseInfo proto = new Equip_BaseInfo();
        proto.setDbId(_m_dbId);
        proto.setEquipId(getEquipId());
        proto.setLevel(_m_level);
        proto.setAwakenLevel(_m_awakenLevel);
        proto.setIsLock(_m_isLocked);
        proto.setWearHeroId(_m_wearHeroId);
        proto.setSkillAddValue(_m_avaSkillAddValue);
        return proto;
    }

    /**
     * 构造信息
     * @return
     */
    public Equip_Info makeDetailInfo()
    {
        Equip_Info proto = new Equip_Info();
        proto.setBaseInfo(makeBaseInfo());
        makeSkillList(proto.getSkillList());
        return proto;
    }

    /**
     * 构造技能列表
     * @return
     */
    public void makeSkillList(ArrayList<Equip_SkillInfo> _skillList)
    {
        _m_comp._lock();
        try
        {
            for (EquipSkillInfo skillInfo : _m_skillList)
            {
                _skillList.add(skillInfo.toProto());
            }
        } finally
        {
            _m_comp._unlock();
        }
    }

    /**
     * GM命令：重置所有技能加成为1%
     *
     * 功能说明：
     * 将藏品所有技能的加成值重置为最低值100（1%）
     *
     * 执行流程：
     * 1. 遍历所有技能
     * 2. 更新每个技能的value、pendingValue为100，reshapeNum为0
     * 3. 重新计算技能总加成值
     * 4. 推送变更消息
     * 5. 如果被穿戴，重新计算大臣属性
     *
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result gmResetAllSkills(NPPlayerContext _context)
    {
        if (_m_skillList.isEmpty())
            return HeroErr.EQUIP_SKILL_NOT_FOUND;

        for (EquipSkillInfo skillInfo : _m_skillList)
        {
            skillInfo.resetSkill(this, _context);
        }

        // 重新计算总加成值
        _m_avaSkillAddValue = 100 * _m_skillList.size();

        // 推送基础信息变更
        _m_comp.getUserData().sendMsgToGC(
            US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        // 如果被穿戴，重新计算大臣属性
        if (_m_heroInfo != null)
            _m_heroInfo.recalHero();

        return Result.SUCC;
    }

    /**
     * GM命令：设置藏品等级
     *
     * 功能说明：
     * 直接设置藏品等级为指定值（绕过升级限制）
     *
     * 执行流程：
     * 1. 验证等级合法性
     * 2. 更新等级到数据库
     * 3. 推送变更消息
     * 4. 如果被穿戴，重新计算大臣属性
     *
     * @param _newLevel 新等级
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result gmSetLevel(int _newLevel, NPPlayerContext _context)
    {
        if (_newLevel < 1)
            return CommErr.PARAM_ERROR;

        // 直接设置等级
        _setLevel(_newLevel);

        // 如果被穿戴，重新计算大臣属性
        if (_m_heroInfo != null)
            _m_heroInfo.recalHero();

        // 推送变更
        _m_comp.getUserData().sendMsgToGC(
                US2GCWriter_013_HeroOp.make_066_OnEquipBaseChg(makeBaseInfo()));

        return Result.SUCC;
    }

    /**
     * GM命令：直接删除藏品
     * 执行流程：
     * 1. 如果被穿戴，先卸下
     * 2. 删除藏品数据库记录
     * 3. 删除技能数据库记录
     * 注意：此方法不会返还道具，仅用于GM命令
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result gmDelete(NPPlayerContext _context)
    {
        // 如果被穿戴，先卸下
        if (isWeared())
        {
            Result result = unWearHero(_context);
            if (!result.isSucc())
            {
                return result;
            }
        }

        // 删除藏品数据
        _m_comp.getUSServer().getBM().getBM(PlayerEquipBO.class).delAll("id", _m_dbId);

        // 删除技能数据
        _m_comp.getUSServer().getBM().getBM(PlayerEquipSkillBO.class).delAll("equip_db_id", _m_dbId);

        return Result.SUCC;
    }
}
