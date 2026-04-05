package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless;

import Common.ConsortObj.Consort_BlessSkill;
import NPGameRes.Refs.Consort.RefConsortBlessSkill;
import NPGameRes.Refs.Consort.RefConsortBlessSkillLvl;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortBlessSkillBO;

import java.util.ArrayList;

/**
 * 家人加护技能管理
 * @author mj
 *
 */
public class ConsortBlessSkillMgr 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//技能数据列表
	private ArrayList<ConsortBlessSkillInfo> _m_alBlessSkillList;

	public ConsortBlessSkillMgr(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		_m_alBlessSkillList = new ArrayList<>();
		
		//初始化技能配表
		_initDefault();
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	/***
	 * 初始化默认数据
	 */
	private void _initDefault()
	{
		for(int i = 0; i < _m_ciConsort.getRef().blessSkillList.size(); i++)
		{
			RefConsortBlessSkill skillRef = _m_ciConsort.getRef().blessSkillList.get(i);
			if(null == skillRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init default bless skill:{} fail, not find ref."
						, getUserData().getCid(), getConsortId(), _m_ciConsort.getRef().bless_skill_id_list.get(i));
				continue;
			}
			
			//加护技能默认解锁，从1级开始
			RefConsortBlessSkillLvl skillLvlRef = skillRef.getLevelMapMgr().getLevelData(1);
			if(null == skillLvlRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init default bless skill:{} lvl:{} fail, not find ref."
						, getUserData().getCid(), getConsortId(), _m_ciConsort.getRef().bless_skill_id_list.get(i), 1);
				continue;
			}
			
			ConsortBlessSkillInfo skill = new ConsortBlessSkillInfo(_m_ciConsort, skillRef, skillLvlRef);
			_m_alBlessSkillList.add(skill);
		}
	}
	
	/**
	 * 初始化bo数据
	 * @param _bo
	 */
	public void _initBo(PlayerConsortBlessSkillBO _bo)
	{
		ConsortBlessSkillInfo skill = lookup(_bo.getSkillId());
		if(null == skill)
		{
			USLog.error(getUSServer(), "player:{} consort:{} init bless bo skill:{} fail, not find skill."
					, getUserData().getCid(), getConsortId(), _bo.getSkillId());
			return;
		}
		
		skill._setBo(_bo);
	}
	
	/***********
	 * 构造数据协议对象
	 * @param _list
	 */
	public void makeProto(ArrayList<Consort_BlessSkill> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alBlessSkillList.size(); i++)
			{
				ConsortBlessSkillInfo skill = _m_alBlessSkillList.get(i);
				if(null == skill)
					continue;
				
				_list.add(skill.toProto());
			}
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定加护技能
	 * @param _skillId
	 * @return
	 */
	public ConsortBlessSkillInfo lookup(long _skillId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alBlessSkillList.size(); i++)
			{
				ConsortBlessSkillInfo skill = _m_alBlessSkillList.get(i);
				if(null == skill)
					continue;
				
				if(skill.getSkillId() == _skillId)
					return skill;
			}
			
			return null;
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 截面数据（仅供上级截面方法使用）
	 * @return
	 */
	public String _sectionLog()
	{
		StringBuilder sb = new StringBuilder();
		
		for(int i = 0; i < _m_alBlessSkillList.size(); i++)
		{
			ConsortBlessSkillInfo info = _m_alBlessSkillList.get(i);
			if(null == info)
				continue;
			
			sb.append(info.getSkillId()).append(":").append(info.getLvl()).append(";");
		}
		
		return sb.toString();
	}
}
