package NPUSServer.NPUSUserMgr.UserComp.HeroComp.BusinessSkill;

import Common.HeroObj.Hero_BusinessSkillInfo;
import CommonEnum.EBonusPropertyType;
import GS2GC.p013_HeroOp.GS2GC_013_054_OnHeroBusinessSkillChg;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Hero.RefHeroBusinessSkill;
import NPGameRes.Refs.Hero.RefHeroBusinessSkillUpgrade;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.BuildingConditionDealerMgr;
import USDB.Bo.PlayerHeroBusinessSkillBO;
import USLOGDB.Bo.LogHeroSkillBO;

/*********
 * 大臣经营技能数据管理对象
 *
 * 本技能默认1级，如不升级则不在数据库留数据
 */
public class HeroBusinessSkillInfo
{
    private HeroBusinessSkillMgr _m_mgr;
    private RefHeroBusinessSkill _m_ref;

    private PlayerHeroBusinessSkillBO _m_bo;

    public HeroBusinessSkillInfo(HeroBusinessSkillMgr _mgr, RefHeroBusinessSkill _ref)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;

        _m_bo = null;
    }

    public int getSkillLevel()
    {
        return _m_bo == null ? 1 : _m_bo.getLevel();
    }

    public RefHeroBusinessSkill getRef()
    {
        return _m_ref;
    }

    public long getSkillId()
    {
        return _m_ref.id;
    }

    /************
     * 初始化数据库读取数据
     * @param _bo
     */
    protected void _initBo(PlayerHeroBusinessSkillBO _bo)
    {
        if (null == _bo)
            return;

        _m_bo = _bo;
    }

    /**
     * 设置等级
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(NPPlayerContext _context)
    {
        //检查是否已经达到技能等级上限
        if (!_m_ref.can_upgrade)
            return HeroErr.HERO_BUSINESS_SKILL_LEVEL_REACH_MAX;

        //获取当前等级配置
        RefHeroBusinessSkillUpgrade refUpgrade = RefHeroBusinessSkillUpgrade.getMgr().lookupLevelRef(_m_ref.upgrade_cost_group, getSkillLevel());
        if (refUpgrade == null)
            return CommErr.REF_NOT_FOUND;

        //获取等级配置
        int targetLevel = getSkillLevel() + 1;
        if (targetLevel > _m_ref.business_skill_lvl_max)
            return HeroErr.HERO_BUSINESS_SKILL_LEVEL_REACH_MAX;

        //获取下一级数据
        RefHeroBusinessSkillUpgrade refNextUpgrade = RefHeroBusinessSkillUpgrade.getMgr().lookupLevelRef(_m_ref.upgrade_cost_group, targetLevel);
        if (refNextUpgrade == null)
            return CommErr.REF_NOT_FOUND;

        //消耗道具
        if (!refUpgrade.upgrade_cost_item.isEmpty() && !_m_mgr.getHeroInfo().getUserdata().spendItem(refUpgrade.upgrade_cost_item, _context))
            return CommErr.CONSUME_FAIL;

        //记录变更前的等级
        int oriLevel = getSkillLevel();

        //设置等级
        _setLevel(targetLevel, _context);

        //记录MJ日志
        MJEventLog.logFellowAptitudeSkill(
            _m_mgr.getHeroInfo().getUserdata(),
            _m_mgr.getHeroInfo().getHeroId(),
            _m_mgr.getHeroInfo().getLevel(),
            2, // 培养类型：2=经营技能
            _context.getContextId(),
            oriLevel,
            targetLevel
        );

        _m_mgr.getHeroInfo().getUserdata().getRecordComponent().addRecord(ENPPlayerRecordParam.HERO_BUSINESS_SKILL_UPGRADE_TIMES, 1, _context);

        return Result.SUCC;
    }

    /**
     * GM命令设置经营技能等级
     *
     * @param _level 目标等级
     * @param _context 上下文
     * @return 结果
     */
    public Result gmSetLevel(int _level, NPPlayerContext _context)
    {
        // 验证等级范围
        if (_level < 1 || _level > _m_ref.business_skill_lvl_max)
            return HeroErr.HERO_BUSINESS_SKILL_LEVEL_REACH_MAX;

        // 验证等级配表是否存在
        RefHeroBusinessSkillUpgrade refUpgrade = RefHeroBusinessSkillUpgrade.getMgr().lookupLevelRef(_m_ref.upgrade_cost_group, _level);
        if (refUpgrade == null)
            return CommErr.REF_NOT_FOUND;

        // 设置等级
        _setLevel(_level, _context);

        return Result.SUCC;
    }

    /**
     * 设置等级
     * @param _level   目标等级
     * @param _context
     */
    private void _setLevel(int _level, NPPlayerContext _context)
    {
        BM bmObj = _m_mgr.getHeroInfo().getComp().getUSServer().getBM();

        //判断是否有Bo，如无Bo则生成一个，如有Bo则直接保存
        int oriLevel = 1;
        if (null == _m_bo)
        {
            _m_bo = new PlayerHeroBusinessSkillBO();
            _m_bo.setCid(bmObj, _m_mgr.getHeroInfo().getCid());
            _m_bo.setHeroId(bmObj, _m_mgr.getHeroInfo().getHeroId());
            _m_bo.setSkillId(bmObj, _m_ref.id);
            _m_bo.setLevel(bmObj, _level);
            _m_bo.insert(bmObj);
        } else
        {
            oriLevel = _m_bo.getLevel();
            _m_bo.saveLevel(bmObj, _level);
        }

        onSkillChg();

        //如果大臣有被放置到建筑上则需要更新属性加成
        BuildingInfo buildingInfo = _m_mgr.getHeroInfo().getBuildingInfo();
        if (buildingInfo != null)
        {
            //检查是否在作用范围内
            if (!checkInLimit(buildingInfo))
                return;

            //变更等级差的加成
            buildingInfo.getBonusPropertyContainer().addModifier(_m_ref.bonus_prop_modifier_per_level, _level - oriLevel);
        }

        //通知联盟技能变化
        _m_mgr.getHeroInfo().getUserdata().getGuildComponent().onHeroBusinessSkillChg(_m_mgr.getHeroInfo());

        //日志数据
        LogHeroSkillBO logBo = new LogHeroSkillBO();
        logBo.setCid(bmObj, _m_bo.getCid());
        logBo.setHeroId(bmObj, _m_bo.getHeroId());
        logBo.setSkillId(bmObj, _m_bo.getSkillId());
        logBo.setOriLvl(bmObj, oriLevel);
        logBo.setCurLvl(bmObj, getSkillLevel());
        CommLogDB.log(bmObj, logBo, _context);
    }

    public void onSkillChg()
    {
        //推送消息
        GS2GC_013_054_OnHeroBusinessSkillChg proto = new GS2GC_013_054_OnHeroBusinessSkillChg();
        proto.setHeroId(_m_mgr.getHeroInfo().getHeroId());
        proto.setBusinessSkillInfo(toProto());
        _m_mgr.getHeroInfo().getComp().getUserData().sendMsgToGC(proto);
    }

    /**
     * 构造数据结构
     * @return 数据
     */
    public Hero_BusinessSkillInfo toProto()
    {
        Hero_BusinessSkillInfo proto = new Hero_BusinessSkillInfo();
        proto.setBusinessSkillId(getSkillId());
        proto.setLevel(getSkillLevel());
        return proto;
    }

    /**
     * 检查是否在作用范围内
     * @param _buildingInfo
     * @return
     */
    public boolean checkInLimit(BuildingInfo _buildingInfo)
    {
        if (null == _buildingInfo)
            return false;

        return BuildingConditionDealerMgr.IsEnable(_m_ref.limit_range, _buildingInfo, null);
    }

    /**
     * 放置到建筑
     * @param _buildingInfo
     */
    public void onPlaceToBuilding(BuildingInfo _buildingInfo)
    {
        if (null == _buildingInfo)
            return;

        //检查是否在作用范围内
        if (!checkInLimit(_buildingInfo))
            return;

        _buildingInfo.getBonusPropertyContainer().addModifier(_m_ref.bonus_prop_modifier);
        _buildingInfo.getBonusPropertyContainer().addModifier(_m_ref.bonus_prop_modifier_per_level, getSkillLevel() - 1);
    }

    /**
     * 从建筑上移除
     * @param _buildingInfo
     */
    public void onRemoveFormBuilding(BuildingInfo _buildingInfo)
    {
        if (null == _buildingInfo)
            return;

        //检查是否在作用范围内
        if (!checkInLimit(_buildingInfo))
            return;

        _buildingInfo.getBonusPropertyContainer().removeModifier(_m_ref.bonus_prop_modifier);
        _buildingInfo.getBonusPropertyContainer().removeModifier(_m_ref.bonus_prop_modifier_per_level, getSkillLevel() - 1);
    }

    /**
     * 获取属性加成
     * @param _type
     * @return
     */
    public long getAttrProp(EBonusPropertyType _type)
    {
        return _m_ref.bonus_prop_modifier.getPropValue(_type) + _m_ref.bonus_prop_modifier_per_level.getPropValue(_type) * (getSkillLevel() - 1);
    }
}
