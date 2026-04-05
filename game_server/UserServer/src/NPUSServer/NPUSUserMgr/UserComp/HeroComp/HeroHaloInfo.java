package NPUSServer.NPUSUserMgr.UserComp.HeroComp;

import Common.HeroObj.Hero_HaloInfo;
import GS2GC.p013_HeroOp.GS2GC_013_061_OnHeroHaloChg;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.Refs.Hero.RefHeroHalo;
import NPGameRes.Refs.Hero.RefHeroHaloLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerHeroHaloBO;

/*************
 * 大臣星辉数据类
 */
public class HeroHaloInfo
{
    private HeroInfo _m_heroInfo;
    private RefHeroHalo _m_ref;

    private PlayerHeroHaloBO _m_bo;
    private RefHeroHaloLevel _m_levelRef;

    public HeroHaloInfo(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
        _m_ref = RefHeroHalo.getMgr().get(_m_heroInfo.getRef().halo_id);
        if (_m_ref == null)
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo setBo ref not found, boId:{}", _m_heroInfo.getRef().halo_id);
            return;
        }

        _m_bo = null;
        _m_levelRef = null;
    }

    public HeroInfo getHeroInfo()
    {
        return _m_heroInfo;
    }

    public RefHeroHalo getHaloRef()
    {
        return _m_ref;
    }

    public long getLevel()
    {
        return _m_levelRef == null ? 0 : _m_levelRef.level;
    }

    public NPUSUserData getUserdata()
    {
        return _m_heroInfo.getUserdata();
    }

    /**
     * 初始化光环默认套系技能加成
     * 无论光环是否激活，都将默认加成应用到套系上
     */
    public void initDefaultSuitBonus(boolean _isInit)
    {
        if (null == _m_ref)
            return;

        if (null == _m_ref.halo_suit_skill_extra_level || _m_ref.halo_suit_skill_extra_level.isEmpty())
            return;

        if (null == getHeroInfo().getSuitInfo())
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo.initDefaultSuitBonus - suitInfo not found, suitId:{}", getHeroInfo().getRef().suit_id);
            return;
        }

        //将默认套系技能加成累加到套系数据上
        for (WCGPairLong skillInfoPair : _m_ref.halo_suit_skill_extra_level)
        {
            //0级的不做累加处理
            if (skillInfoPair.second() == 0)
                continue;

            getHeroInfo().getSuitInfo().addSkillLevel(skillInfoPair.first(), (int) skillInfoPair.second());
        }

        //套件变更推送
        if (!_isInit)
            getHeroInfo().getSuitInfo().onSuitChg();
    }

    /**
     * 设置数据对象
     * @param _bo
     */
    protected void _initBo(PlayerHeroHaloBO _bo)
    {
        if (null == _bo)
            return;

        if (_m_bo != null)
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo setBo already set, boId:{}", _m_bo.getId());
            return;
        }

        //设置数据对象
        _m_bo = _bo;
        //设置等级数据
        _m_levelRef = _m_ref.getLevelMapMgr().getLevelData(_bo.getLevel());
        if (_m_levelRef == null)
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo setBo refHaloLevel not found, boId:{}", _bo.getId());
            return;
        }

        //向大臣增加属性
        getHeroInfo().getPropertyContainer().addModifier(_m_levelRef.self_attr_prop_modifier);

        //向套件数据中初始化增加等级
        if (null != getHeroInfo().getSuitInfo())
        {
            //这个累加技能
            for (WCGPairLong skillInfoPair : _m_levelRef.halo_suit_skill_level_list)
            {
                //0级的不做累加处理
                if (skillInfoPair.second() == 0)
                    continue;

                getHeroInfo().getSuitInfo().addSkillLevel(skillInfoPair.first(), (int) skillInfoPair.second());
            }
        } else
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo setBo suitInfo not found, suitId:{}", getHeroInfo().getRef().suit_id);
        }
    }

    /**
     * 解锁光环
     * @param _context
     * @return
     */
    public Result unlock(NPPlayerContext _context)
    {
        if (_m_bo != null)
            return HeroErr.HERO_HALO_UNLOCK_REPEAT;

        //无光环数据直接返回
        if (null == _m_ref)
            return CommErr.REF_NOT_FOUND;

        RefHeroHaloLevel refHaloLevel = _m_ref.getLevelMapMgr().getLevelData(0);
        if (refHaloLevel == null)
            return CommErr.REF_NOT_FOUND;

        //构建Bo数据插入
        _m_bo = new PlayerHeroHaloBO();
        _m_bo.setCid(getUserdata().getUSServer().getBM(), getUserdata().getCid());
        _m_bo.setHeroId(getUserdata().getUSServer().getBM(), _m_heroInfo.getHeroId());
        _m_bo.setLevel(getUserdata().getUSServer().getBM(), refHaloLevel.level);
        _m_bo.insert(getUserdata().getUSServer().getBM());

        _m_levelRef = refHaloLevel;

        //向大臣增加属性
        getHeroInfo().getPropertyContainer().addModifier(_m_levelRef.self_attr_prop_modifier);

        //向套件数据中初始化增加等级
        if (null != getHeroInfo().getSuitInfo())
        {
            //这个累加技能
            for (WCGPairLong skillInfoPair : _m_levelRef.halo_suit_skill_level_list)
            {
                //0级的不做累加处理
                if (skillInfoPair.second() == 0)
                    continue;

                getHeroInfo().getSuitInfo().addSkillLevel(skillInfoPair.first(), (int) skillInfoPair.second());
            }
        } else
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo unlock suitInfo not found, suitId:{}", getHeroInfo().getRef().suit_id);
        }

        //推送变更
        onHaloChg();

        //套件变更推送
        getHeroInfo().getSuitInfo().onSuitChg();

        return Result.SUCC;
    }

    /**
     * 升级光环
     * @param _context 上下文
     * @return 结果
     */
    public Result upgrade(NPPlayerContext _context)
    {
        if (_m_bo == null)
            return HeroErr.HERO_HALO_NOT_UNLOCK;

        //查询是否有下一级，如无则不处理
        RefHeroHaloLevel nextLevelRef = _m_ref.getLevelMapMgr().getLevelData(_m_levelRef.level + 1);
        if (null == nextLevelRef)
            return CommErr.REF_NOT_FOUND;

        //消耗道具
        if (!_m_levelRef.upgrade_cost.isEmpty() && !getUserdata().spendItem(_m_levelRef.upgrade_cost, _context))
            return CommErr.CONSUME_FAIL;

        //设置为下一等级
        return _setLevel(nextLevelRef, _context);
    }

    /**
     * 设置等级
     * @param _levelRef 目标等级数据
     * @param _context  上下文
     */
    protected Result _setLevel(RefHeroHaloLevel _levelRef, NPPlayerContext _context)
    {
        //查找目标等级配置
        if (_levelRef == null)
            return CommErr.REF_NOT_FOUND;

        //等级数据一致则不做后续处理
        if (_m_levelRef == _levelRef)
            return Result.SUCC;

        //记录前等级数据
        RefHeroHaloLevel preLevelRef = _m_levelRef;

        //设置数据
        _m_levelRef = _levelRef;
        _m_bo.saveLevel(getUserdata().getUSServer().getBM(), _levelRef.level);

        //先移除旧技能，再添加新技能
        //向大臣增加属性
        getHeroInfo().getPropertyContainer().replaceModifier(null == preLevelRef ? null : preLevelRef.self_attr_prop_modifier
                , _m_levelRef.self_attr_prop_modifier);

        //推送变更
        onHaloChg();

        //向套件数据中移除旧技能，再添加新技能
        if (null != getHeroInfo().getSuitInfo())
        {
            //移除旧技能
            if (null != preLevelRef)
            {
                for (WCGPairLong skillInfoPair : preLevelRef.halo_suit_skill_level_list)
                {
                    //0级的不做累加处理
                    if (skillInfoPair.second() == 0)
                        continue;

                    getHeroInfo().getSuitInfo().removeSkillLevel(skillInfoPair.first(), (int) skillInfoPair.second());
                }
            }

            //添加新技能
            if (null != _m_levelRef)
            {
                for (WCGPairLong skillInfoPair : _m_levelRef.halo_suit_skill_level_list)
                {
                    //0级的不做累加处理
                    if (skillInfoPair.second() == 0)
                        continue;

                    getHeroInfo().getSuitInfo().addSkillLevel(skillInfoPair.first(), (int) skillInfoPair.second());
                }
            }

            //套件变更推送
            getHeroInfo().getSuitInfo().onSuitChg();
        } else
        {
            USLog.error(getUserdata().getUSServer(), "HeroHaloInfo setBo suitInfo not found, suitId:{}", getHeroInfo().getRef().suit_id);
        }

        return Result.SUCC;
    }

    public void onHaloChg()
    {
        GS2GC_013_061_OnHeroHaloChg proto = new GS2GC_013_061_OnHeroHaloChg();
        proto.setHaloInfo(toProto());
        getUserdata().sendMsgToGC(proto);
    }

    public Hero_HaloInfo toProto()
    {
        Hero_HaloInfo haloInfo = new Hero_HaloInfo();

        haloInfo.setHeroId(_m_heroInfo.getHeroId());
        haloInfo.setIsUnlock(null != _m_bo);
        haloInfo.setLevel(null == _m_bo ? 0 : _m_bo.getLevel());
        return haloInfo;
    }
}
