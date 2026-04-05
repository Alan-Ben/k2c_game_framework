package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless;

import Common.ConsortObj.Consort_BlessSkill;
import NPCommon.DB.BM.BM;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Consort.RefConsortBlessSkill;
import NPGameRes.Refs.Consort.RefConsortBlessSkillLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortBlessSkillBO;

/**
 * 家人加护技能数据
 * @author mj
 *
 */
public class ConsortBlessSkillInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//加护技能配置
	private RefConsortBlessSkill _m_refBlessSkill;
	//加护技能等级配置
	private RefConsortBlessSkillLvl _m_refBlessSkillLvl;
	
	//bo数据对象
	private PlayerConsortBlessSkillBO _m_boBlessSkill;
	
	public ConsortBlessSkillInfo(ConsortInfo _consort, RefConsortBlessSkill _ref, RefConsortBlessSkillLvl _lvlRef)
	{
		_m_ciConsort = _consort;
		
		_m_refBlessSkill = _ref;
		_m_refBlessSkillLvl = _lvlRef;
		
		//家人对伙伴的属性数据
		if(null != _m_refBlessSkillLvl)
		{
			_m_ciConsort.getPropertyContainer().addModifier(_m_refBlessSkillLvl.add);
		}
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//配置对象
	public RefConsortBlessSkill getRef() {return _m_refBlessSkill;}
	public long getSkillId() {return _m_refBlessSkill.bless_skill_id;}
	
	public RefConsortBlessSkillLvl getLvlRef() {return _m_refBlessSkillLvl;}
	public int getLvl() {return null == _m_refBlessSkillLvl ? 0 : _m_refBlessSkillLvl.lvl;}
	
	protected void _setBo(PlayerConsortBlessSkillBO _bo) 
	{
		_m_boBlessSkill = _bo;
		
		if(_m_boBlessSkill.getSkillLvl() > 0)
		{
			RefConsortBlessSkillLvl lvlRef = _m_refBlessSkill.getLevelMapMgr().getLevelData(_m_boBlessSkill.getSkillLvl());
			if(null == lvlRef)
			{
				USLog.error(getUSServer(), "player:{} consort:{} init bo bless skill:{} lvl:{} fail, not find ref."
						, getUserData().getCid(), getConsortId(), getSkillId(), _m_boBlessSkill.getSkillLvl());
			}
			else
			{
				RefConsortBlessSkillLvl preLvlRef = _m_refBlessSkillLvl;
				_m_refBlessSkillLvl = lvlRef;
				
				_m_ciConsort.getPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.add, _m_refBlessSkillLvl.add);
			}
		}
	}
	
	/***
	 * 构造数据协议
	 * @return
	 */
	public Consort_BlessSkill toProto()
	{
		Consort_BlessSkill proto = new Consort_BlessSkill();
		proto.setSkillId(getSkillId());
		proto.setLvl(getLvl());
		
		return proto;
	}
	
	/**
	 * 设置加护技能等级
	 * @param _lvlRef
	 * @param _context
	 */
	public void setLvl(RefConsortBlessSkillLvl _lvlRef, NPPlayerContext _context)
	{
		RefConsortBlessSkillLvl preLvlRef = _m_refBlessSkillLvl;
		//更新等级配置
		_m_refBlessSkillLvl = _lvlRef;
		
		//更新bo数据
		_save();
		
		//更新属性
		_m_ciConsort.getPropertyContainer().replaceModifier(null == preLvlRef ? null : preLvlRef.add, _m_refBlessSkillLvl.add);
		
		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_059_OnBlessSkillChg(this));

		_m_ciConsort.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_BLESSING_SKILL_UPGRADE_TIMES, 1, _context);
	}
	
	/**
	 * 保存数据
	 */
	private void _save()
	{
		BM bmObj = getUSServer().getBM();
		
		if(null == _m_boBlessSkill)
		{
			PlayerConsortBlessSkillBO bo = new PlayerConsortBlessSkillBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, getConsortId());
			bo.setSkillId(bmObj, getSkillId());
			bo.setSkillLvl(bmObj, getLvl());
			bo.insert(bmObj);
			
			_m_boBlessSkill = bo;
		}
		else
		{
			_m_boBlessSkill.setSkillLvl(bmObj, _m_refBlessSkillLvl.lvl);
			_m_boBlessSkill.saveAll(bmObj);
		}
	}
}
