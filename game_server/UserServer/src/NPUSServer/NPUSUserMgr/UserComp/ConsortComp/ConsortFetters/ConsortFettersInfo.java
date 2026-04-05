package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortFetters;

import Common.ConsortObj.Consort_Fetters;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Consort.RefConsortFettersLvl;
import NPGameRes.Refs.Consort.RefConsortFettersSkill;
import NPGameRes.Refs.Consort.RefConsortFettersSkillLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

/**
 * 家人羁绊技能数据
 * @author mj
 *
 */
public class ConsortFettersInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//羁绊等级配置
	private RefConsortFettersLvl _m_refFettersLvl;
	
	//羁绊技能数据
	private ConsortFettersSkillInfo _m_siFettersSkill;
	
	public ConsortFettersInfo(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		//初始化羁绊等级
		_m_refFettersLvl = RefConsortFettersLvl.getMgr().get(_consort.getBo().getFettersLvl());
		if(null == _m_refFettersLvl)
		{
			USLog.error(getUSServer(), "player:{} consort:{} init bo fetters lvl:{} ref fail, not find ref."
					, getUserData().getCid(), _m_ciConsort.getConsortId(), _consort.getBo().getFettersLvl());
		}
		
		//初始化羁绊技能等级
		if(null != _m_refFettersLvl && _m_ciConsort.getRef().consort_fetters_skill_id > 0)
		{
			RefConsortFettersSkill skillRef = RefConsortFettersSkill.getMgr().get(_m_ciConsort.getRef().consort_fetters_skill_id);
			if(null == skillRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init default fetters skill:{} ref fail, not find ref."
						, getUserData().getCid(), _m_ciConsort.getConsortId(), _m_ciConsort.getRef().consort_fetters_skill_id);
				return;
			}
			
			RefConsortFettersSkillLvl skillLvlRef = skillRef.getLevelMapMgr().getLevelData(getSkillLvl());
			if(null == skillLvlRef) //等级配表即时不存在，羁绊技能依然要保留内存对象，表示该玩家正常数据拥有羁绊技能
			{
				USLog.error(getUSServer(), "player:{} consort:{} init default fetters skill:{} lvl:{} ref fail, not find ref."
						, getUserData().getCid(), _m_ciConsort.getConsortId(), _m_ciConsort.getRef().consort_fetters_skill_id, _m_refFettersLvl.consort_fetters_skill_lvl);
			}
			
			_m_siFettersSkill = new ConsortFettersSkillInfo(_m_ciConsort, skillRef, skillLvlRef);
		}	
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//羁绊等级配置
	public RefConsortFettersLvl getLvlRef() {return _m_refFettersLvl;}
	public int getLvl() {return null == _m_refFettersLvl ? 0 : _m_refFettersLvl.lvl;}
	public int getStudyBonus() {return null == _m_refFettersLvl ? 0 : _m_refFettersLvl.study_bonus;}
	//羁绊技能等级数据，来自羁绊等级配表
	public int getSkillLvl() {return null == _m_refFettersLvl ? 0 : _m_refFettersLvl.consort_fetters_skill_lvl;}
	
	//羁绊技能数据
	public ConsortFettersSkillInfo getSkill() {return _m_siFettersSkill;}
	
	/**
	 * 构造羁绊协议数据
	 * @return
	 */
	public Consort_Fetters toProto()
	{
		Consort_Fetters proto = new Consort_Fetters();
		proto.setLvl(getLvl());
		
		return proto;
	}
	
	/**
	 * 移除羁绊技能全局属性加成（GM删除妃子时调用）
	 */
	public void cmdDiscardBonus()
	{
		if (null == _m_siFettersSkill || null == _m_siFettersSkill.getLvlRef()) return;
		getUserData().getBonusMgr().removeBonus(_m_siFettersSkill.getLvlRef().add_bonus);
		getUserData().getConsortComponent().getPlayerPropertyContainer().removeModifier(_m_siFettersSkill.getLvlRef().add_player);
	}

	/**
	 * 设置羁绊等级
	 * @param _lvlRef
	 * @param _context
	 */
	public void setLvl(RefConsortFettersLvl _lvlRef, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			if(getLvl() == _lvlRef.lvl)
				return;

			//记录变更前的值
			int oriLvl = getLvl();

			_m_refFettersLvl = _lvlRef;

			BM bmObj = getUSServer().getBM();
			_m_ciConsort.getBo().setFettersLvl(bmObj, getLvl());
			_m_ciConsort.getBo().saveAll(bmObj);

			//更新技能数据
			_updateSkill();

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_057_OnFettersChg(this));

			//记录MJ日志
			MJEventLog.logConsortAttribute(
				getUserData(),
				getConsortId(),
				4, // 培养类型：4=羁绊升级
				_context.getContextId(),
				oriLvl,
				getLvl()
			);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***
	 * 更新羁绊技能等级
	 */
	protected void _updateSkill() 
	{
		//该家人无配置羁绊技能
		if(null == _m_siFettersSkill)
			return;
		
		RefConsortFettersSkillLvl skillLvlRef = _m_siFettersSkill.getRef().getLevelMapMgr().getLevelData(getSkillLvl());
		if(null == skillLvlRef) //等级配表即时不存在，羁绊技能依然要保留内存对象，表示该玩家正常数据拥有羁绊技能
		{
			USLog.error(getUSServer(), "player:{} consort:{} check fetters skill:{} lvl:{} ref fail, not find ref."
					, getUserData().getCid(), _m_ciConsort.getConsortId(), _m_ciConsort.getRef().consort_fetters_skill_id, _m_refFettersLvl.consort_fetters_skill_lvl);
			return;
		}
		
		_m_siFettersSkill.setLvl(skillLvlRef);
	}
}
