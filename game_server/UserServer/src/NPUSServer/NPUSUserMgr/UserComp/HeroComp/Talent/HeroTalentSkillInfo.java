package NPUSServer.NPUSUserMgr.UserComp.HeroComp.Talent;

import Common.HeroObj.Hero_TalentSkillInfo;
import GS2GC.p013_HeroOp.GS2GC_013_052_OnHeroTalentSkillChg;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Hero.RefHeroTalentSkill;
import NPGameRes.Refs.Hero.RefHeroTalentSkillLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import USDB.Bo.PlayerHeroTalentSkillBO;

import java.util.ArrayList;
import java.util.List;

/*************
 * 大臣资质技能数据管理对象
 */
public class HeroTalentSkillInfo
{
    private HeroTalentSkillMgr _m_mgr;
    private RefHeroTalentSkill _m_ref;

    private PlayerHeroTalentSkillBO _m_bo;

    private int _m_level;//等级
    private RefHeroTalentSkillLevel _m_levelRef;

    public HeroTalentSkillInfo(HeroTalentSkillMgr _mgr, RefHeroTalentSkill _refSkill)
    {
        _m_mgr = _mgr;
        _m_ref = _refSkill;

        _m_bo = null;

        //默认设置为1级数据
        _m_level = 1;
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_m_level);
    }

    public long getTalentSkillId()
    {
        return _m_ref.Id();
    }

    public int getTalentSkillLevel()
    {
        return _m_level;
    }

    public RefHeroTalentSkillLevel getLevelRef()
    {
        return _m_levelRef;
    }

    //获取每级加成的倍率
    public int getLevelBonusStack()
    {
        return null == _m_levelRef ? 0 : getTalentSkillLevel() - _m_levelRef.level;
    }

    /************
     * 初始化数据库读取数据
     * @param _bo
     */
    protected void _initBo(PlayerHeroTalentSkillBO _bo)
    {
        if (null == _bo)
            return;

        _m_bo = _bo;

        //设置等级数据
        _m_level = _bo.getLevel();
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_bo.getLevel());
        if (null == _m_levelRef)
            CommLog.error("HeroTalentSkillInfo _initBo levelRef not found, skillId:{}, level:{}", _bo.getTalentSkillId(), _bo.getLevel());
    }

    /************
     * 设置
     * @param _level
     * @param _needPush
     */
    protected void _setLevelByStep(int _level, boolean _needPush)
    {
        //设置等级数据
        _m_level = _level;
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_m_level);
        if (null == _m_levelRef)
            CommLog.error("HeroTalentSkillInfo _initBo levelRef not found, skillId:{}, level:{}", getTalentSkillId(), _m_level);

        //推送变更
        if (_needPush)
            onSkillChg();
    }

    /**
     * 使用道具升级
     * @param _isTen
     * @param _context 上下文
     * @return 结果
     */
    public Result upgradeUseItem(boolean _isTen, NPPlayerContext _context)
    {
        //检查是否是自动升级的技能
        if (_m_mgr.getHeroInfo().getRef().auto_upgrade_talent_skill_id_list.contains(getTalentSkillId()))
            return HeroErr.HERO_TALENT_SKILL_CANT_MANUAL_UPGREADE;

        //判断是否达到最大等级
        int oriLevel = getTalentSkillLevel();
        if (oriLevel >= _m_ref.level_limit)
            return HeroErr.HERO_TALENT_SKILL_LEVEL_REACH_MAX;

        //消耗列表
        List<NPCommonCostItem> costItemList = new ArrayList<>();
        //新的等级配置
        RefHeroTalentSkillLevel newLevelRef = null;

        int newLevel = 0;
        if (_isTen)
        {
            //遍历查询是否有对应等级配置，能升几级就升几级
            NPItemCostCollector_nosafe tempCostList = new NPItemCostCollector_nosafe();
            for (int i = oriLevel; i < oriLevel + 10; i++)
            {
                if (i >= _m_ref.level_limit)
                    break;

                //查询当前等级对应的起始配置
                RefHeroTalentSkillLevel levelRef = _m_ref.getLevelMapMgr().getLevelData(i);
                if (levelRef == null)
                    break;

                //计算本级消耗并累加到临时列表
                tempCostList.addItem(levelRef.cost);
                // 按等级差计算额外消耗
                int gapLevel = i - levelRef.level;
                if (levelRef.cost_per_level != null && !levelRef.cost_per_level.isEmpty() && gapLevel != 0)
                {
                    tempCostList.addItem(CommonFunc.itemMultiple(levelRef.cost_per_level, gapLevel));
                }

                //检查累计消耗是否足够，不够则停止
                if (!_m_mgr.getHeroInfo().getUserdata().hasCostItemList(tempCostList.getItemList()))
                    break;

                //确认可以升到这一级
                newLevel = i + 1;
                costItemList.clear();
                for (NPCommonCostItem costItem : tempCostList.getItemList())
                {
                    costItemList.add(costItem.duplicate());
                }
                newLevelRef = levelRef;
            }
        } else
        {
            //查询当前等级对应的起始配置
            RefHeroTalentSkillLevel levelRef = _m_ref.getLevelMapMgr().getLevelData(oriLevel);
            if (levelRef != null)
            {
                newLevel = oriLevel + 1;
                costItemList.add(levelRef.cost);
                // 按等级差计算额外消耗
                int gapLevel = oriLevel - levelRef.level;
                if (levelRef.cost_per_level != null && !levelRef.cost_per_level.isEmpty() && gapLevel != 0)
                {
                    costItemList.add(CommonFunc.itemMultiple(levelRef.cost_per_level, gapLevel));
                }
                newLevelRef = levelRef;
            }
        }

        //检查是否已经达到技能等级上限
        if (newLevel == 0)
            return CommErr.REF_NOT_FOUND;

        //消耗升级指定道具
        boolean isSuccess = _m_mgr.getHeroInfo().getUserdata().spendCostItemList(costItemList, _context);
        if (!isSuccess)
            return CommErr.CONSUME_FAIL;

        //保存数据
        BM bmObj = _m_mgr.getHeroInfo().getComp().getUSServer().getBM();
        if (null == _m_bo)
        {
            _m_bo = new PlayerHeroTalentSkillBO();
            _m_bo.setCid(bmObj, _m_mgr.getHeroInfo().getCid());
            _m_bo.setHeroId(bmObj, _m_mgr.getHeroInfo().getHeroId());
            _m_bo.setTalentSkillId(bmObj, _m_ref.id);
            _m_bo.setLevel(bmObj, newLevel);
            _m_bo.insert(bmObj);
        } else
        {
            _m_bo.saveLevel(_m_mgr.getHeroInfo().getComp().getUSServer().getBM(), newLevel);
        }

        //设置数据
        _setLevel(newLevel, newLevelRef);

        //记录MJ日志
        MJEventLog.logFellowAptitudeSkill(
            _m_mgr.getHeroInfo().getUserdata(),
            _m_mgr.getHeroInfo().getHeroId(),
            _m_mgr.getHeroInfo().getLevel(),
            1, // 培养类型：1=资质
            _context.getContextId(),
            oriLevel,
            newLevel
        );

        return Result.SUCC;
    }

    /**
     * 设置等级
     * @param _level
     * @param _newLevelRef
     */
    public void _setLevel(int _level, RefHeroTalentSkillLevel _newLevelRef)
    {
        _m_level = _level;
        _m_levelRef = _newLevelRef;

        //推送变更
        onSkillChg();
    }

    /**
     * 技能变更推送
     */
    public void onSkillChg()
    {
        GS2GC_013_052_OnHeroTalentSkillChg proto = new GS2GC_013_052_OnHeroTalentSkillChg();
        proto.setHeroId(_m_mgr.getHeroInfo().getHeroId());
        proto.setTalentSkillInfo(toProto());
        _m_mgr.getHeroInfo().getComp().getUserData().sendMsgToGC(proto);
    }

    /**
     * 构造技能信息
     * @return 技能信息
     */
    public Hero_TalentSkillInfo toProto()
    {
        Hero_TalentSkillInfo proto = new Hero_TalentSkillInfo();
        proto.setTealentSkillId(getTalentSkillId());
        proto.setLevel(getTalentSkillLevel());
        return proto;
    }

    /**
     * GM命令设置资质技能等级
     *
     * @param _level 目标等级
     * @param _context 上下文
     * @return 结果
     */
    public Result gmSetLevel(int _level, NPPlayerContext _context)
    {
        _m_mgr.getHeroInfo().lock();
        try
        {
            // 验证等级配表是否存在
            RefHeroTalentSkillLevel levelRef = _m_ref.getLevelMapMgr().getLevelData(_level);
            if (levelRef == null)
                return CommErr.REF_NOT_FOUND;

            // 保存数据到数据库
            BM bmObj = _m_mgr.getHeroInfo().getComp().getUSServer().getBM();
            if (null == _m_bo)
            {
                _m_bo = new PlayerHeroTalentSkillBO();
                _m_bo.setCid(bmObj, _m_mgr.getHeroInfo().getCid());
                _m_bo.setHeroId(bmObj, _m_mgr.getHeroInfo().getHeroId());
                _m_bo.setTalentSkillId(bmObj, _m_ref.id);
                _m_bo.setLevel(bmObj, _level);
                _m_bo.insert(bmObj);
            } else
            {
                _m_bo.saveLevel(bmObj, _level);
            }

            // 设置等级数据并推送
            _setLevel(_level, levelRef);

            return Result.SUCC;
        } finally
        {
            _m_mgr.getHeroInfo().unlock();
        }
    }
}
