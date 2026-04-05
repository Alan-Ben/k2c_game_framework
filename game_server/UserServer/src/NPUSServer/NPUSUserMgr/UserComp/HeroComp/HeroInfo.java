package NPUSServer.NPUSUserMgr.UserComp.HeroComp;

import Common.HeroObj.Hero_Info;
import Common.HeroObj.Hero_PlaceInfo;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import GS2GC.p013_HeroOp.*;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.Enum.EUsParam;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Delegate.ADelegateNone;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommon.Util.Pair.WCGPair;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPConditionDealerData;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyContainer;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.Hero.RefHeroLevel;
import NPGameRes.Refs.Hero.RefHeroStar;
import NPGameRes.Refs.Hero.RefHeroStep;
import NPUSServer.Cache.Hero.PlayerHeroCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_HERO_UPGRADE;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.Common.HeroPropertyChgDealer;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.BusinessSkill.HeroBusinessSkillMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.ConditionDealer.HeroConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.LazyDealer.HeroRecalPowerTask;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.Skin.HeroSkinMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.StarSkill.HeroStarSkillMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr.HeroSuitInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.Talent.HeroTalentSkillMgr;
import USDB.Bo.PlayerHeroBO;
import USDB.Bo.PlayerHeroBusinessSkillBO;
import USDB.Bo.PlayerHeroHaloBO;
import USDB.Bo.PlayerHeroTalentSkillBO;
import USLOGDB.Bo.LogHeroLevelBO;
import USLOGDB.Bo.LogHeroStepBO;

import java.util.HashMap;
import java.util.List;

/***********
 * 大臣数据存储对象
 */
public class HeroInfo implements _IHandlerHolder, _ITNPConditionDealerData
{
    private HeroComponent _m_comp;
    private PlayerHeroBO _m_bo;
    private RefHero _m_ref;

    //大臣的光环数据存储对象
    private HeroHaloInfo _m_haloInfo;
    //大臣归属的套件数据对象
    private HeroSuitInfo _m_suitInfo;
    //经营技能部分，只作用于派遣驻扎的金币产出
    private HeroBusinessSkillMgr _m_businessSkillMgr;
    //资质技能部分，只作用于资质点数
    private HeroTalentSkillMgr _m_talentSkillMgr;
    private HeroStarSkillMgr _m_starSkillMgr;
    private HeroSkinMgr _m_skinMgr;

    //属性容器
    private PlayerAttrPropertyContainer _m_attrPropertyContainer;

    //当前大臣配置信息 以下三个引用都可能为空
    private RefHeroLevel _m_levelRef;
    private RefHeroStep _m_stepRef;
    private RefHeroStar _m_starRef;

    //大臣关键数据的状态存储，用于在不同的操作中判断是否要触发外围其他处理
    private long _m_lPower;     //实力值
    //大臣实力变更触发器
    private ADelegateNone _m_powerChgDelegate;

    //大臣身上的Lazy处理类对象
    private LazyTaskDealer _m_lazyPowerCalr;

    //驻扎的建筑对象
    private BuildingInfo _m_buildingInfo;

    //穿戴的藏品对象
    private EquipInfo _m_equipInfo;

    public HeroInfo(HeroComponent _comp, RefHero _ref, PlayerHeroBO _bo, boolean _isInit)
    {
        _m_comp = _comp;
        _m_bo = _bo;
        _m_ref = _ref;

        _m_levelRef = RefHeroLevel.getMgr().get(_bo.getLevel());
        if (null == _m_levelRef)
            CommLog.error("HeroInfo::HeroInfo, hero level ref is null, heroId=" + _bo.getHeroId() + " level=" + _bo.getLevel());

        _m_stepRef = RefHeroStep.getMgr().get(_bo.getStep());
        if (null == _m_stepRef)
            CommLog.error("HeroInfo::HeroInfo, hero step ref is null, heroId=" + _bo.getHeroId() + " step=" + _bo.getStep());

        if (_m_ref.hero_star_group_id != 0)
        {
            _m_starRef = RefHeroStar.getByGroupAndStar(_m_ref.hero_star_group_id, _bo.getStar());
            if (null == _m_starRef)
                CommLog.error("HeroInfo::HeroInfo, hero star ref is null, heroId=" + _bo.getHeroId() + " star=" + _bo.getStar());
        }

        _m_attrPropertyContainer = new PlayerAttrPropertyContainer();
        _m_lPower = 0;
        _m_powerChgDelegate = new ADelegateNone(this);

        //300毫秒间隔执行
        _m_lazyPowerCalr = new LazyTaskDealer(new HeroRecalPowerTask(this), 100);

        //判断大臣是否有星辉，有才构造数据
        if (_ref.halo_id != 0)
            _m_haloInfo = new HeroHaloInfo(this);
        else
            _m_haloInfo = null;

        _m_businessSkillMgr = new HeroBusinessSkillMgr(this);
        _m_talentSkillMgr = new HeroTalentSkillMgr(this);
        _m_starSkillMgr = new HeroStarSkillMgr(this);
        _m_skinMgr = new HeroSkinMgr(this);

        //获取套件对象
        _m_suitInfo = _m_comp.getSuitMgr().lookupSuitInfo(_m_ref.suit_id);

        //初始化光环默认套系技能加成（无论是否激活都生效）
        if (null != _m_haloInfo)
            _m_haloInfo.initDefaultSuitBonus(_isInit);

        //驻扎的建筑对象
        _m_buildingInfo = getUserdata().getBuildingComponent().lookupBuilding(_bo.getBuildingId());

        //加上星级的加成
        _m_attrPropertyContainer.addModifier(_m_starRef == null ? null : _m_starRef.self_attr_prop_modifier);
    }

    public void lock()
    {
        _m_comp._lock();
    }

    public void unlock()
    {
        _m_comp._unlock();
    }

    public long getPower()
    {
        return _m_lPower;
    }

    public HeroSuitInfo getSuitInfo()
    {
        return _m_suitInfo;
    }

    public ADelegateNone getPowerChgDelegate()
    {
        return _m_powerChgDelegate;
    }

    /**
     * 获得新大臣时调用的新大臣初始化函数
     * @param _context 上下文
     */
    public void initNewHero(NPPlayerContext _context)
    {
        //注册属性容器的监听处理对象
        _m_attrPropertyContainer.setOnPropertyChg(new HeroPropertyChgDealer(this));
    }

    /**********
     * 重新计算的处理函数，会调用对应的处理类延期计算
     */
    public void _initCalHero()
    {
        //重新计算实力
        long newPower = HeroCalculator.calPower(this, null);

        //设置大臣新数据
        updatePower(newPower);

        //更新数值到玩家组件
        getComp().replaceHeroPower(0, newPower, true);

        //注册属性容器的监听处理对象
        _m_attrPropertyContainer.setOnPropertyChg(new HeroPropertyChgDealer(this));
    }

    /**********
     * 重新计算的处理函数，会调用对应的处理类延期计算
     */
    public void recalHero()
    {
        _m_lazyPowerCalr.setNeedDeal();
    }

    public NPUSUserData getUserdata()
    {
        return _m_comp.getUserData();
    }

    public PlayerHeroBO getBo()
    {
        return _m_bo;
    }

    public HeroHaloInfo getHaloInfo()
    {
        return _m_haloInfo;
    }

    public HeroBusinessSkillMgr getBusinessSkillMgr()
    {
        return _m_businessSkillMgr;
    }

    public HeroTalentSkillMgr getTalentSkillMgr()
    {
        return _m_talentSkillMgr;
    }

    public HeroStarSkillMgr getStarSkillMgr()
    {
        return _m_starSkillMgr;
    }

    public HeroSkinMgr getSkinMgr()
    {
        return _m_skinMgr;
    }

    public RefHero getRef()
    {
        return _m_ref;
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }

    public long getHeroId()
    {
        return _m_bo.getHeroId();
    }

    public int getLevel()
    {
        return _m_bo.getLevel();
    }

    public int getStar()
    {
        return _m_bo.getStar();
    }

    public int getStep()
    {
        return _m_bo.getStep();
    }

    public long getSkinId()
    {
        return _m_bo.getSkinId();
    }

    public long getItemAddPower()
    {
        return _m_bo.getExtAddPower();
    }

    public long getArenaAddPower()
    {
        return _m_bo.getArenaAddPower();
    }

    public long getTravelAddPower()
    {
        return _m_bo.getTravelAddPower();
    }

    public RefHeroLevel getHeroLevelRef()
    {
        return _m_levelRef;
    }

    public RefHeroStep getHeroStepRef()
    {
        return _m_stepRef;
    }

    public RefHeroStar getHeroStarRef()
    {
        return _m_starRef;
    }

    public BuildingInfo getBuildingInfo()
    {
        return _m_buildingInfo;
    }

    public HeroComponent getComp()
    {
        return _m_comp;
    }

    public EquipInfo getEquip()
    {
        return _m_equipInfo;
    }

    public long getEquipPowerPer()
    {
        getUserdata().lockUser();
        try{
            if (_m_equipInfo == null)
                return 0;

            return _m_equipInfo.getAddPowerPerValue();
        }finally
        {
            getUserdata().unlockUser();
        }
    }

    public long getEquipTalent()
    {
        getUserdata().lockUser();
        try{
            if (_m_equipInfo == null)
                return 0;

            return _m_equipInfo.getAddTalentPoint();
        }finally
        {
            getUserdata().unlockUser();
        }
    }

    public PlayerAttrPropertyContainer getPropertyContainer()
    {
        return _m_attrPropertyContainer;
    }

    /**
     * 放置藏品到大臣身上
     * @param _equipInfo
     * @param _isInit
     */
    public void setEquip(EquipInfo _equipInfo, boolean _isInit)
    {
        getUserdata().lockUser();
        try{
            _m_equipInfo = _equipInfo;
            if (!_isInit)
                recalHero();
        }finally
        {
            getUserdata().unlockUser();
        }
    }

    /*************
     * 更新大臣实力值
     * @param _power
     */
    public void updatePower(long _power)
    {
        getUserdata().lockUser();
        try
        {
            _m_lPower = _power;

            //更新到缓存
            PlayerHeroCacheFunc.updatePower(getUserdata(), this);
            //更新到联盟
            getUserdata().getGuildComponent().onHeroInfoChg(this);
        } finally
        {
            getUserdata().unlockUser();
        }
    }

    /***********
     * 获取本大臣有效的玩家身上的所有加成数据
     * @param _propertyType
     * @return
     */
    public long getPlayerBonusAddValue(EBonusPropertyType _propertyType)
    {
        //获取玩家身上加成
        return getUserdata().getBonusMgr().getTotalPropertyBonus(_propertyType)
                + getUserdata().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.HERO_ATTR, getRef().spec_attr_type.ordinal())
                + getUserdata().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.HERO_ID, getHeroId())
                + getUserdata().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.QUALITY, getRef().quality.ordinal());
    }

    /**
     * 提升大臣星级
     * @param _context 上下文
     * @return 结果
     */
    public Result improveStar(NPPlayerContext _context)
    {
        lock();
        try
        {
            //判断是否可以升星
            if (!_m_ref.can_star_upgrade)
                return HeroErr.HERO_STAR_REACH_MAX;

            if (_m_starRef == null)
                return CommErr.REF_NOT_FOUND;

            //查找下一等级配置（根据升星组ID和星级查找）
            RefHeroStar refNextStar = RefHeroStar.getByGroupAndStar(_m_ref.hero_star_group_id, getStar() + 1);
            if (refNextStar == null)
                return HeroErr.HERO_STAR_REACH_MAX;

            //判断条件是否满足
            if (!NPPlayerConditionDealerMgr.IsEnable(_m_starRef.upgrade_player_condition, getUserdata(), null))
                return CommErr.CONDITION_NOT_ENABLE;

            //检查是否满足大臣条件
            if (!HeroConditionDealerMgr.IsEnable(_m_starRef.upgrade_condition, this, null))
                return CommErr.CONDITION_NOT_ENABLE;

            //检查消耗
            List<NPCommonCostItem> upgradeCost = _m_starRef.upgrade_cost;
            boolean hasItem = _m_comp.getUserData().hasCostItemList(upgradeCost);
            if (!hasItem)
                return CommErr.CONSUME_FAIL;
            //尝试消耗
            boolean isSuccess = _m_comp.getUserData().spendCostItemList(upgradeCost, _context);
            if (!isSuccess)
                return CommErr.CONSUME_FAIL;

            getBo().saveStar(_m_comp.getUSServer().getBM(), refNextStar.star);

            RefHeroStar preStarRef = _m_starRef;

            _m_starRef = refNextStar;

            //推送变更
            onStarChg(_context);

            //重新计算大臣属性
            _m_attrPropertyContainer.replaceModifier(preStarRef == null ? null : preStarRef.self_attr_prop_modifier, _m_starRef.self_attr_prop_modifier);

            //检查星级技能解锁
            getStarSkillMgr().onStarLevelChg(getStar(), _context);
            //检测资质技能的处理
            getTalentSkillMgr().checkUnlockExtraSkill(_context);
            
            //检查入驻队伍，发起计算队伍实力
            getUserdata().getMarsExploreComponent().getTeamMgr().onHeroPeropertyChg(getHeroId());

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 提升大臣阶段
     * @param _context 上下文
     * @return 结果
     */
    public Result improveStep(NPPlayerContext _context)
    {
        lock();
        try
        {
            //当前的阶段配置
            RefHeroStep heroStepRef = getHeroStepRef();
            if (heroStepRef == null)
                return CommErr.REF_NOT_FOUND;

            //检查是否已经达到升阶条件
            if (getLevel() < heroStepRef.level_limit)
                return HeroErr.HERO_LEVEL_NOT_ENOUGH;


            //检查消耗
            boolean hasItem = _m_comp.getUserData().hasCostItemList(heroStepRef.cost_item_list);
            if (!hasItem)
                return CommErr.CONSUME_FAIL;

            //尝试消耗
            boolean isSuccess = _m_comp.getUserData().spendCostItemList(heroStepRef.cost_item_list, _context);
            if (!isSuccess)
                return CommErr.CONSUME_FAIL;

            RefHeroStep refNextStep = RefHeroStep.getMgr().get(heroStepRef.step + 1);
            if (refNextStep == null)
                return HeroErr.HERO_STEP_IS_MAX;

            BM bmObj = _m_comp.getUSServer().getBM();
            int oriStep = getBo().getStep();
            getBo().saveStep(bmObj, refNextStep.step);

            _m_stepRef = refNextStep;

            //推送变更
            onStepChg(_context);

            //提升自动升级天赋技能等级
            _m_talentSkillMgr.onStepChg();

            //数据日志
            LogHeroStepBO logBo = new LogHeroStepBO();
            logBo.setCid(bmObj, getCid());
            logBo.setHeroId(bmObj, getHeroId());
            logBo.setOriStep(bmObj, oriStep);
            logBo.setCurStep(bmObj, getBo().getStep());
            CommLogDB.log(bmObj, logBo, _context);

            getUserdata().getRecordComponent().addRecord(ENPPlayerRecordParam.HERO_IMPROVE_STEP_TIMES, 1, _context);

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 设置大臣阶段
     * @param _step    阶段
     * @param _context 上下文
     */
    public Result gmSetStep(int _step, NPPlayerContext _context)
    {
        lock();
        try
        {
            RefHeroStep refNextStep = RefHeroStep.getMgr().get(_step);
            if (refNextStep == null)
                return CommErr.REF_NOT_FOUND;

            BM bmObj = _m_comp.getUSServer().getBM();
            int oriStep = getBo().getStep();

            getBo().saveStep(bmObj, _step);

            onStepChg(_context);

            //数据日志
            LogHeroStepBO logBo = new LogHeroStepBO();
            logBo.setCid(bmObj, getCid());
            logBo.setHeroId(bmObj, getHeroId());
            logBo.setOriStep(bmObj, oriStep);
            logBo.setCurStep(bmObj, getBo().getStep());
            CommLogDB.log(bmObj, logBo, _context);

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 大臣星级变更推送
     */
    private void onStarChg(NPPlayerContext _context)
    {
        //推送变更
        GS2GC_013_056_OnHeroStarChg proto = new GS2GC_013_056_OnHeroStarChg();
        proto.setHeroId(_m_bo.getHeroId());
        proto.setStar(_m_bo.getStar());
        _m_comp.getUserData().sendMsgToGC(proto);
    }

    /**
     * 大臣阶段变更推送
     */
    private void onStepChg(NPPlayerContext _context)
    {
        //推送变更
        GS2GC_013_058_OnHeroStepChg proto = new GS2GC_013_058_OnHeroStepChg();
        proto.setHeroId(_m_bo.getHeroId());
        proto.setStep(_m_bo.getStep());
        _m_comp.getUserData().sendMsgToGC(proto);
    }

    /**
     * 升级操作
     * @param _isTen   是否升十级
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(boolean _isTen, NPPlayerContext _context)
    {
        lock();
        try
        {
            //当前的阶段配置
            RefHeroStep heroStepRef = getHeroStepRef();
            if (heroStepRef == null)
                return CommErr.REF_NOT_FOUND;

            int heroLevelLimit = heroStepRef.level_limit;
            int curLevel = getLevel();

            //判断等级是否达到当前等级上限
            if (heroLevelLimit <= curLevel)
                return HeroErr.HERO_REACH_CURRENT_STEP_LIMIT;

            //玩家当前的银币数量
            long expCount = _m_comp.getUserData().getCurrencyComponent().getItemCount(ECurrency.HERO_EXP.ordinal());
            if (expCount <= 0)
                return CommErr.ITEM_NOT_ENOUGH;

            //计算目标等级,限制到上限范围
            int targetLevel = Math.min(_isTen ? (curLevel + 10) : (curLevel + 1), heroLevelLimit);

            //计算升级到目标等级所需要的经验
            WCGPair<RefHeroLevel, Long> calResult = RefHeroLevel.getMgr().calculateLevelUpNeedExp(curLevel, targetLevel, expCount);
            if (calResult.first == null)
                return HeroErr.HERO_UPGRADE_FAIL;

            //尝试消耗银币
            boolean isSuccess = _m_comp.getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), calResult.second, _context);
            if (!isSuccess)
                return CommErr.CONSUME_FAIL;

            //记录变更前的等级
            int oriLevel = getLevel();

            //增加经验
            _setLevel(calResult.first, _context);

            //记录MJ日志
            MJEventLog.logFellowAptitudeSkill(
                _m_comp.getUserData(),
                getHeroId(),
                calResult.first.level,
                3, // 培养类型：3=等级
                _context.getContextId(),
                oriLevel,
                calResult.first.level
            );

            //检查解锁额外经营技能
            _m_businessSkillMgr.checkUnlockExtraBusinessSkill(_context);

            //更新到联盟
            getUserdata().getGuildComponent().onHeroInfoChg(this);

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 设置等级
     * @param _level
     * @param _context
     */
    public Result gmSetLevel(int _level, NPPlayerContext _context)
    {
        lock();
        try
        {
            RefHeroLevel refHeroLevel = RefHeroLevel.getMgr().get(_level);
            if (refHeroLevel == null)
                return CommErr.REF_NOT_FOUND;

            _setLevel(refHeroLevel, _context);

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 设置等级
     * @param _levelRef   等级
     * @param _context 上下文
     */
    private void _setLevel(RefHeroLevel _levelRef, NPPlayerContext _context)
    {
        lock();
        try
        {
            int oriLevel = getBo().getLevel();
            //保存等级数据
            getBo().saveLevel(_m_comp.getUSServer().getBM(), _levelRef.level);
            //更新等级配置
            _m_levelRef = _levelRef;

            //添加大臣计算处理
            recalHero();

            //触发升级事件
            int levelGap = _levelRef.level - oriLevel;
            if (levelGap > 0)
            {
                Event_P_HERO_UPGRADE event = new Event_P_HERO_UPGRADE(_context, getHeroId(), levelGap);
                getUserdata().onLogicEvent(event);
            }

            //推送变更
            GS2GC_013_050_OnHeroLevelChg proto = new GS2GC_013_050_OnHeroLevelChg();
            proto.setHeroId(_m_bo.getHeroId());
            proto.setLevel(_m_bo.getLevel());
            _m_comp.getUserData().sendMsgToGC(proto);

            //数据日志
            LogHeroLevelBO logBo = new LogHeroLevelBO();
            logBo.setCid(_m_comp.getUSServer().getBM(), getUserdata().getCid());
            logBo.setHeroId(_m_comp.getUSServer().getBM(), getHeroId());
            logBo.setOriLvl(_m_comp.getUSServer().getBM(), oriLevel);
            logBo.setCurLvl(_m_comp.getUSServer().getBM(), getBo().getLevel());
            CommLogDB.log(_m_comp.getUSServer().getBM(), logBo, _context);

            //更新到缓存
            PlayerHeroCacheFunc.updateLevel(getUserdata(), this);

        } finally
        {
            unlock();
        }
    }

    /**
     * 更换大臣皮肤
     * @param _skinId  皮肤id
     * @param _context 上下文
     * @return 结果
     */
    public Result chgSkin(long _skinId, NPPlayerContext _context)
    {
        lock();
        try
        {
            boolean hasSkin = getSkinMgr().hasSkin(_skinId);
            if (!hasSkin)
                return HeroErr.HERO_SKIN_NOT_EXIST;

            _m_bo.saveSkinId(_m_comp.getUSServer().getBM(), _skinId);

            GS2GC_013_059_OnHeroWearSkinChg pushProto = new GS2GC_013_059_OnHeroWearSkinChg();
            pushProto.setHeroId(getHeroId());
            pushProto.setSkinId(getSkinId());
            getUserdata().sendMsgToGC(pushProto);

            //更新到缓存
            PlayerHeroCacheFunc.updateSkinId(getUserdata(), this);

            //更新到联盟
            getUserdata().getGuildComponent().onHeroInfoChg(this);

            return Result.SUCC;
        } finally
        {
            unlock();
        }
    }

    /**
     * 放置大臣到对应建筑上
     * @param _buildingId
     * @param _context
     * @return
     */
    public Result placeToBuilding(long _buildingId, NPPlayerContext _context)
    {
        //检查是否已经驻扎，如果已经驻扎，则需要替换下当前驻扎建筑数据
        if (_m_buildingInfo != null)
        {
        	//检查是否同建筑
        	if(_m_buildingInfo.getBuildingId() == _buildingId)
        		return BuildingErr.BUILDING_EXISTED;
        	
        	//从建筑上移除大臣
        	removeFormBuilding(_context);
        }

        //检查建筑是否存在
        BuildingInfo buildingInfo = getUserdata().getBuildingComponent().lookupBuilding(_buildingId);
        if (buildingInfo == null)
            return BuildingErr.BUILDING_NOT_EXIST;

        //检查建筑是否已经满了
        if (buildingInfo.isFullHero())
            return BuildingErr.BUSINESS_HERO_FULL;

        //检查大臣是否可以驻扎到该属性建筑上
        if (!_m_ref.settled_building_attr_type_list.contains(buildingInfo.getAttr()))
            return HeroErr.HERO_CANT_PLACE_TO_THIS_ATTR_BUILDING;

        _m_bo.setBuildingId(_m_comp.getUSServer().getBM(), _buildingId);
        _m_bo.setSerial(_m_comp.getUSServer().getBM(), getUserdata().getUSServer().getUSParams().incParam(EUsParam.HERO_PLACE_SERIAL, 1));
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        _m_buildingInfo = buildingInfo;

        //放置到建筑上处理
        buildingInfo.placeHero(this);
        _onPlaceToBuilding();

        //驻扎建筑变更推送
        onHeroPlaceBuildingChg(_context);

        getUserdata().getRecordComponent().addRecord(ENPPlayerRecordParam.HERO_PLACE_TO_BUILDING_TIME, 1, _context);

        return Result.SUCC;
    }

    /**
     * 驻扎建筑变更推送
     * @param _context
     */
    public void onHeroPlaceBuildingChg(NPPlayerContext _context)
    {
        GS2GC_013_062_OnHeroPlaceBuildingChg proto = new GS2GC_013_062_OnHeroPlaceBuildingChg();
        proto.setHeroId(getHeroId());
        proto.setPlaceInfo(new Hero_PlaceInfo(getBo().getBuildingId(),_m_bo.getSerial()));
        _m_comp.getUserData().sendMsgToGC(proto);
    }

    /**
     * 放置到建筑上处理
     */
    protected void _onPlaceToBuilding()
    {
        if (_m_buildingInfo == null)
            return;

        getBusinessSkillMgr().onPlaceToBuilding(_m_buildingInfo);
    }

    /**
     * 从建筑上移除大臣
     */
    public Result removeFormBuilding(NPPlayerContext _context)
    {
        if (_m_buildingInfo == null)
            return HeroErr.HERO_NOT_PLACE;

        BuildingInfo buildingInfo = _m_buildingInfo;
        buildingInfo.removeHero(this);

        getBusinessSkillMgr().onRemoveFormBuilding(_m_buildingInfo);
        _m_buildingInfo = null;

        _m_bo.setBuildingId(_m_comp.getUSServer().getBM(), 0);
        _m_bo.setSerial(_m_comp.getUSServer().getBM(), 0);
        _m_bo.saveAllMarked(_m_comp.getUSServer().getBM());

        //驻扎建筑变更推送
        onHeroPlaceBuildingChg(_context);

        return Result.SUCC;
    }

    /**
     * 增减额外加成实力值
     * @param _addValue 变化值
     * @param _context 上下文
     */
    public void addItemAddPower(long _addValue, NPPlayerContext _context)
    {
        getUserdata().lockUser();
        try
        {
            _m_bo.saveExtAddPower(_m_comp.getUSServer().getBM(), getItemAddPower() + _addValue);

            //更新属性
            recalHero();

            //推送变更
            _m_comp.getUserData().sendMsgToGC(new GS2GC_013_063_OnHeroItemAddPowerChg(getHeroId(), getItemAddPower()));
        } finally
        {
            getUserdata().unlockUser();
        }
    }

    /**
     * GM命令设置道具加成实力
     *
     * @param _power 目标实力值
     * @param _context 上下文
     */
    public void gmSetItemAddPower(long _power, NPPlayerContext _context)
    {
        getUserdata().lockUser();
        try
        {
            _m_bo.saveExtAddPower(_m_comp.getUSServer().getBM(), _power);

            //更新属性
            recalHero();

            //推送变更
            _m_comp.getUserData().sendMsgToGC(new GS2GC_013_063_OnHeroItemAddPowerChg(getHeroId(), getItemAddPower()));
        } finally
        {
            getUserdata().unlockUser();
        }
    }

    /**
     * 获取额外加成实力值
     * @param _addValue
     */
    public void addTravelAddPower(long _addValue, NPPlayerContext _context)
    {
        getUserdata().lockUser();
        try
        {
            _m_bo.saveTravelAddPower(_m_comp.getUSServer().getBM(), getTravelAddPower() + _addValue);

            //更新属性
            recalHero();

            //推送变更
            _m_comp.getUserData().sendMsgToGC(new GS2GC_013_069_OnHeroTravelAddPowerChg(getHeroId(), getTravelAddPower()));
        } finally
        {
            getUserdata().unlockUser();
        }
    }

    /**
     * 获取竞技场加成实力值
     * @param _addValue
     */
    public void addArenaAddPower(long _addValue, NPPlayerContext _context)
    {
        getUserdata().lockUser();
        try
        {
            _m_bo.saveArenaAddPower(_m_comp.getUSServer().getBM(), getArenaAddPower() + _addValue);

            //更新属性
            recalHero();

            //推送变更
            _m_comp.getUserData().sendMsgToGC(new GS2GC_013_064_OnHeroArenaAddPowerChg(getHeroId(), getArenaAddPower()));
        } finally
        {
            getUserdata().unlockUser();
        }
    }

    /**
     * 构造大臣的数据
     * @return
     */
    public Hero_Info toProto()
    {
        Hero_Info proto = new Hero_Info();
        proto.setHeroId(_m_bo.getHeroId());
        proto.setSkinId(_m_bo.getSkinId());
        proto.setLevel(_m_bo.getLevel());
        proto.setStep(_m_bo.getStep());
        proto.setStar(_m_bo.getStar());
        proto.setPlaceInfo(new Hero_PlaceInfo(_m_bo.getBuildingId(), _m_bo.getSerial()));
        //皮肤信息
        getSkinMgr().fillSkinProto(proto.getSkinList());
        //技能信息
        _m_businessSkillMgr.fillBusinessSkillProto(proto.getBusinessSkillList());
        //资质技能信息
        _m_talentSkillMgr.fillTalentSkillProto(proto.getTalentSkillList());
        //光环信息
        if (null != _m_haloInfo)
            proto.setHaloInfo(_m_haloInfo.toProto());
        proto.setItemAddPower(_m_bo.getExtAddPower());
        proto.setArenaAddPower(_m_bo.getArenaAddPower());
        proto.setTravelAddPower(_m_bo.getTravelAddPower());
        return proto;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("大臣id:").append(getHeroId()).append("\n");
        sb.append("等级:").append(getLevel()).append("\n");
        sb.append("阶段:").append(getBo().getStep()).append("\n");
        sb.append("吃书属性:").append("\n");
//        sb.append("   武力:").append(getBookAttrValue(EBasicAttrType.STR)).append("\n");
//        sb.append("   智力:").append(getBookAttrValue(EBasicAttrType.INT)).append("\n");
//        sb.append("   政治:").append(getBookAttrValue(EBasicAttrType.POL)).append("\n");
//        sb.append("   统帅:").append(getBookAttrValue(EBasicAttrType.LEAD)).append("\n");
        sb.append("皮肤:").append("\n").append(getSkinMgr()).append("\n");
        sb.append("技能:").append("\n").append(getBusinessSkillMgr()).append("\n");
//        sb.append("资质技能:").append("\n").append(getTalentSkillMgr()).append("\n");
//        sb.append("光环:").append("\n").append(getHaloMgr()).append("\n");
        return sb.toString();
    }

    /**
     * 销毁大臣相关数据
     */
    public void dispose()
    {
        HashMap<String, Object> conditions = new HashMap<>();
        conditions.put("cid", getCid());
        conditions.put("hero_id", getHeroId());

        getSkinMgr().removeAll();

        BM bmObj = _m_comp.getUSServer().getBM();

        bmObj.getBM(PlayerHeroBO.class).delAll(conditions);
        bmObj.getBM(PlayerHeroBusinessSkillBO.class).delAll(conditions);
        bmObj.getBM(PlayerHeroTalentSkillBO.class).delAll(conditions);
        bmObj.getBM(PlayerHeroHaloBO.class).delAll(conditions);
    }
}
