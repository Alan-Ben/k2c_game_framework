package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortHalo;

import NPGameRes.Refs.Consort.RefConsortHaloSkill;
import NPGameRes.Refs.Consort.RefConsortHaloSkillLvl;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserServer;

/***
 * 家人星辉技能
 * @author mj
 *
 */
public class ConsortHaloSkillInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//配置数据
	private RefConsortHaloSkill _m_refHaloSkill;
	private RefConsortHaloSkillLvl _m_refHaloSkillLvl;
	
	public ConsortHaloSkillInfo(ConsortInfo _consort, RefConsortHaloSkill _ref, RefConsortHaloSkillLvl _lvlRef)
	{
		_m_ciConsort = _consort;
		
		_m_refHaloSkill = _ref;
		_m_refHaloSkillLvl = _lvlRef;
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//配置数据
	public RefConsortHaloSkill getRef() {return _m_refHaloSkill;}
	public RefConsortHaloSkillLvl getLvlRef() {return _m_refHaloSkillLvl;}
}
