package NPUSServer.NPUSUserMgr.UserComp.HeroComp.BusinessSkill;

import Common.HeroObj.Hero_BusinessSkillInfo;
import CommonEnum.EBonusPropertyType;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.Refs.Hero.RefHeroBusinessSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.BuildingConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.ConditionDealer.SimpleItem.SimpleAttrBuildingConditionMgr;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import USDB.Bo.PlayerHeroBusinessSkillBO;
import USLOGDB.Bo.LogHeroSkillBO;

import java.util.ArrayList;
import java.util.List;

public class HeroBusinessSkillMgr
{
    private HeroInfo _m_heroInfo;
    private List<HeroBusinessSkillInfo> _m_skillList;

    public HeroBusinessSkillMgr(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
        _m_skillList = new ArrayList<>();

        //调用初始化技能数据
        _initSkillInfo();
    }

    /**************
     * 带入大臣之后将根据大臣数据直接进行相关技能数据的初始化
     * 此初始化为单纯内存的初始化
     */
    private void _initSkillInfo()
    {
        //数据非法判断
        if (null == _m_heroInfo || null == _m_heroInfo.getRef())
            return;

        //初始化经营技能内存数据
        if (_m_heroInfo.getRef().business_skill_id != 0)
        {
            //初始化技能数据
            RefHeroBusinessSkill refSkill = RefHeroBusinessSkill.getMgr().get(_m_heroInfo.getRef().business_skill_id);
            if (null != refSkill)
            {
                _m_skillList.add(new HeroBusinessSkillInfo(this, refSkill));
            }else
            {
                CommLog.error("HeroBusinessSkillMgr _initSkillInfo refSkill not found, skillId:{}", _m_heroInfo.getRef().business_skill_id);
            }
        }

        //初始化可能的额外解锁数据
        for (WCGPairLong starSkillPair : _m_heroInfo.getRef().extra_business_skill_id_list)
        {
            //第一个数据为等级，等级超过则解锁，否则不解锁
            if (starSkillPair.first() > _m_heroInfo.getLevel())
                continue;

            //如果已经存在对应技能则不添加
            if (null != lookupSkillInfo(starSkillPair.second()))
                continue;

            //添加基础技能数据
            //逐个初始化经营技能内存数据
            RefHeroBusinessSkill refSkill = RefHeroBusinessSkill.getMgr().get(starSkillPair.second());
            //数据非法判断
            if (null == refSkill)
            {
                CommLog.error("HeroBusinessSkillMgr _initExtraSkillInfo refSkill not found, skillId:{}", starSkillPair.second());
                continue;
            }

            //添加到数据集
            _m_skillList.add(new HeroBusinessSkillInfo(this, refSkill));
        }
    }

    public HeroInfo getHeroInfo()
    {
        return _m_heroInfo;
    }

    /**
     * 数据库初始化技能数据
     * @param _skillBo
     */
    public void initSkill(PlayerHeroBusinessSkillBO _skillBo)
    {
        //从已有数据检查是否有匹配数据
        HeroBusinessSkillInfo skillInfo = lookupSkillInfo(_skillBo.getSkillId());
        //无数据则报错
        if (null == skillInfo)
        {
            CommLog.error("HeroBusinessSkillMgr initSkill skillInfo not found, skillId:{}", _skillBo.getSkillId());
            return;
        }

        //初始化Bo数据
        skillInfo._initBo(_skillBo);
    }

    /**
     * 查找技能
     * @param _skillId
     * @return
     */
    public HeroBusinessSkillInfo lookupSkillInfo(long _skillId)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroBusinessSkillInfo skillInfo : _m_skillList)
            {
                if (skillInfo.getSkillId() == _skillId)
                    return skillInfo;
            }
            return null;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 检查解锁额外技能
     * 需要在以下场景调用：
     * 1、大臣等级变化
     */
    public void checkUnlockExtraBusinessSkill(NPPlayerContext _context)
    {
        int level = _m_heroInfo.getLevel();

        List<WCGPairLong> starSkillPairList = _m_heroInfo.getRef().extra_business_skill_id_list;
        for (WCGPairLong starSkillPair : starSkillPairList)
        {
            if (starSkillPair.first() > level)
                continue;

            //检查是否有对应数据，如无则添加
            HeroBusinessSkillInfo skillInfo = lookupSkillInfo(starSkillPair.second());
            if (null != skillInfo)
                continue;

            //添加一个解锁技能
            _unlockSkill(starSkillPair.second(), _context);
        }
    }

    /**
     * 解锁技能
     * @param _skillId 技能id
     * @param _context 上下文
     */
    private HeroBusinessSkillInfo _unlockSkill(long _skillId, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            if (lookupSkillInfo(_skillId) != null)
                return null;

            RefHeroBusinessSkill refSkill = RefHeroBusinessSkill.getMgr().get(_skillId);
            if (refSkill == null)
                return null;

            HeroBusinessSkillInfo skillInfo = new HeroBusinessSkillInfo(this, refSkill);
            _m_skillList.add(skillInfo);

            //调用技能变化函数
            skillInfo.onSkillChg();

            //如果大臣已经放置到建筑上，则调用技能放置函数
            BuildingInfo buildingInfo = _m_heroInfo.getBuildingInfo();
            if (buildingInfo != null)
            {
                skillInfo.onPlaceToBuilding(buildingInfo);
            }

            //通知联盟技能变化
            _m_heroInfo.getUserdata().getGuildComponent().onHeroBusinessSkillChg(_m_heroInfo);

            BM bmObj = getHeroInfo().getComp().getUSServer().getBM();
            //日志数据
            LogHeroSkillBO logBo = new LogHeroSkillBO();
            logBo.setCid(bmObj, getHeroInfo().getCid());
            logBo.setHeroId(bmObj, getHeroInfo().getHeroId());
            logBo.setSkillId(bmObj, skillInfo.getSkillId());
            logBo.setCurLvl(bmObj, skillInfo.getSkillLevel());
            CommLogDB.log(bmObj, logBo, _context);

            return skillInfo;
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /************
     * 为对应Id的技能升级
     * @param _skillId
     * @param _context
     * @return
     */
    public Result upgrade(long _skillId, NPPlayerContext _context)
    {
        _m_heroInfo.lock();
        try
        {
            HeroBusinessSkillInfo skillInfo = lookupSkillInfo(_skillId);
            if (skillInfo == null)
                return HeroErr.HERO_SKILL_NOT_EXIST;

            return skillInfo.upgrade(_context);
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 放置到建筑处理
     * @param _buildingInfo
     */
    public void onPlaceToBuilding(BuildingInfo _buildingInfo)
    {
        for (HeroBusinessSkillInfo skillInfo : _m_skillList)
        {
            skillInfo.onPlaceToBuilding(_buildingInfo);
        }
    }

    /**
     * 从建筑上移除处理
     * @param _buildingInfo
     */
    public void onRemoveFormBuilding(BuildingInfo _buildingInfo)
    {
        for (HeroBusinessSkillInfo skillInfo : _m_skillList)
        {
            skillInfo.onRemoveFormBuilding(_buildingInfo);
        }
    }

    /**
     * 获取派遣到联盟的相性加成值
     * @return
     */
    public int calGuildDispatchAddValue()
    {
        _m_heroInfo.lock();
        try{
            int addValue = 0;
            for (HeroBusinessSkillInfo skillInfo : _m_skillList)
            {
                boolean isEnable = BuildingConditionDealerMgr.IsEnable(
                        skillInfo.getRef().limit_range,
                        SimpleAttrBuildingConditionMgr.getInstance().getItem(_m_heroInfo.getRef().spec_attr_type),
                        null);
                if (!isEnable)
                    continue;

                addValue += (int) skillInfo.getAttrProp(EBonusPropertyType.BUILDING_PROFIT_ADD_PER);
            }
            return addValue;
        }finally
        {
            _m_heroInfo.unlock();
        }
    }

    /**
     * 填充经营技能数据
     * @param _skillList
     */
    public void fillBusinessSkillProto(ArrayList<Hero_BusinessSkillInfo> _skillList)
    {
        _m_heroInfo.lock();
        try
        {
            for (HeroBusinessSkillInfo skillInfo : _m_skillList)
            {
                _skillList.add(skillInfo.toProto());
            }
        } finally
        {
            _m_heroInfo.unlock();
        }
    }

    @Override
    public String toString()
    {
        _m_heroInfo.lock();
        try
        {
            StringBuilder sb = new StringBuilder();
            for (HeroBusinessSkillInfo skillInfo : _m_skillList)
            {
                sb.append(skillInfo.getSkillId()).append(":").append(skillInfo.getSkillLevel()).append("\n");
            }
            return sb.toString();
        } finally
        {
            _m_heroInfo.unlock();
        }
    }
	
	/**
	 * 截面数据（仅供上级截面方法使用）
	 * @return
	 */
	public String _sectionLog()
	{
		_m_heroInfo.lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			for(int i = 0; i < _m_skillList.size(); i++)
			{
				HeroBusinessSkillInfo info = _m_skillList.get(i);
				if(null == info)
					continue;
				
				sb.append(info.getSkillId()).append(":").append(info.getSkillLevel()).append(";");
			}
			
			return sb.toString();
		}
		finally
        {
            _m_heroInfo.unlock();
        }
	}
}
