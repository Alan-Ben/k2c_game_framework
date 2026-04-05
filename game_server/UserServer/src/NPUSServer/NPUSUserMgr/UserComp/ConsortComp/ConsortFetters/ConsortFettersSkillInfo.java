package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortFetters;

import NPGameRes.Refs.Consort.RefConsortFettersSkill;
import NPGameRes.Refs.Consort.RefConsortFettersSkillLvl;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserServer;

/**
 * 家人羁绊数据
 * @author mj
 *
 */
public class ConsortFettersSkillInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//技能配置数据
	private final RefConsortFettersSkill _m_refSkill;
	//等级配置数据
	private RefConsortFettersSkillLvl _m_refSkillLvl;
	
	public ConsortFettersSkillInfo(ConsortInfo _consort, RefConsortFettersSkill _ref, RefConsortFettersSkillLvl _lvlRef)
	{
		_m_ciConsort = _consort;
		
		_m_refSkill = _ref;
		_m_refSkillLvl = _lvlRef;
		
		//放入属性容器
		if(null != _m_refSkillLvl)
		{
			getUserData().getBonusMgr().addBonus(_m_refSkillLvl.add_bonus);
			
			//玩家属性部分
			getUserData().getConsortComponent().getPlayerPropertyContainer().addModifier(_m_refSkillLvl.add_player);
		}
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//技能相关数据
	public RefConsortFettersSkill getRef() {return _m_refSkill;}
	public RefConsortFettersSkillLvl getLvlRef() {return _m_refSkillLvl;}
	public long getLvl() {return null == _m_refSkillLvl ? 0 : _m_refSkillLvl.lvl;}
	
	/**
	 * 更新羁绊技能等级配置
	 * @param _lvlRef
	 */
	public void setLvl(RefConsortFettersSkillLvl _lvlRef)
	{
		RefConsortFettersSkillLvl preLvlRef = _m_refSkillLvl;
		
		_m_refSkillLvl = _lvlRef;
		
		//全局属性
		getUserData().getBonusMgr().replaceBonus(null == preLvlRef ? null : preLvlRef.add_bonus, _m_refSkillLvl.add_bonus);
		//玩家属性
		getUserData().getConsortComponent().getPlayerPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.add_player, _m_refSkillLvl.add_player);
	}
}
