package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness;

import Common.ConsortObj.Consort_BusinessSkill;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import CommonEnum.ESpecAttrType;
import NPCommon.DB.BM.BM;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;
import NPGameRes.Refs.Consort.RefConsortBusinessSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_BUSINESS_SKILL_UPGRADE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerConsortBusinessSkillBO;

/**
 * 家人经营技能数据
 * @author mj
 *
 */
public class ConsortBusinessSkillInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//技能配置
	private final RefConsortBusinessSkill _m_refBusinessSkill;
	
	//属性加成
	private int _m_iProAdd;
	//普通领悟次数
	private int _m_iNormalOpCount;
	//高级领悟次数
	private int _m_iAdvanceOpCount;
	
	//数据ID
	private PlayerConsortBusinessSkillBO _m_boBusinessSkill;
	
	//bonus属性数据
	private PlayerBonusPropertyModifier _m_mBonusPropertyModifier;
	
	public ConsortBusinessSkillInfo(ConsortInfo _consort, RefConsortBusinessSkill _skillRef)
	{
		_m_ciConsort = _consort;
		
		_m_refBusinessSkill = _skillRef;
		//初始化赋值1%
		_m_iProAdd = 100;
		
		_m_mBonusPropertyModifier = new PlayerBonusPropertyModifier();
		_m_mBonusPropertyModifier.addProperty(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _m_iProAdd);

		if (_m_refBusinessSkill.property == ESpecAttrType.NONE)
		{
			for (ESpecAttrType type : ESpecAttrType.ESpecAttrType_Values)
			{
				//加载到全局属性容器对象
				_m_ciConsort.getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
						type.ordinal(), _m_mBonusPropertyModifier);
			}
		}else
		{
			//加载到全局属性容器对象
			_m_ciConsort.getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
					_m_refBusinessSkill.property.ordinal(), _m_mBonusPropertyModifier);
		}
	}

	public ConsortBusinessSkillInfo(ConsortInfo _consort, RefConsortBusinessSkill _skillRef, PlayerConsortBusinessSkillBO _bo)
	{
		_m_ciConsort = _consort;

		_m_refBusinessSkill = _skillRef;

		_m_boBusinessSkill = _bo;
		_m_iProAdd = _m_boBusinessSkill.getProAdd();
		_m_iNormalOpCount = _m_boBusinessSkill.getNormalOpCount();
		_m_iAdvanceOpCount = _m_boBusinessSkill.getAdvanceOpCount();

		_m_mBonusPropertyModifier = new PlayerBonusPropertyModifier();
		_m_mBonusPropertyModifier.addProperty(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _m_iProAdd);

		if (_m_refBusinessSkill.property == ESpecAttrType.NONE)
		{
			for (ESpecAttrType type : ESpecAttrType.ESpecAttrType_Values)
			{
				//加载到全局属性容器对象
				_m_ciConsort.getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
						type.ordinal(), _m_mBonusPropertyModifier);
			}
		} else
		{
			//加载到全局属性容器对象
			_m_ciConsort.getUserData().getBonusMgr().addModifierToFilter(EBonusFilterType.BUILDING_ATTR,
					_m_refBusinessSkill.property.ordinal(), _m_mBonusPropertyModifier);
		}
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}

	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	//获取技能配表数据
	public RefConsortBusinessSkill getRef() {return _m_refBusinessSkill;}
	public long getSkillId() {return _m_refBusinessSkill.id;}
	public ESpecAttrType getAttrType() {return _m_refBusinessSkill.property;}
	
	//属性加成数值
	public int getProAdd() {return _m_iProAdd;}
	//普通领悟次数
	public int getNormalOpCount() {return _m_iNormalOpCount;}
	//高级领悟次数
	public int getAdvanceOpCount(){return _m_iAdvanceOpCount;}
	//领悟次数总和
	public int getOpCount() {return _m_iNormalOpCount + _m_iAdvanceOpCount;}
	
	//Bo数据
	public PlayerConsortBusinessSkillBO getBo() {return _m_boBusinessSkill;}
	
	//bonus属性数据
	public PlayerBonusPropertyModifier getBonusPropertyModifier() {return _m_mBonusPropertyModifier;}
	
	/**
	 * 加成变化处理
	 * @param _preProAdd
	 * @param _curProAdd
	 */
	protected void _onProAddChg(int _preProAdd, int _curProAdd) 
	{
		if(_preProAdd == _curProAdd)
			return;
		
		//原属性容器
		PlayerBonusPropertyModifier preModifier = _m_mBonusPropertyModifier.duplicate();
		//修改当前属性容器
		_m_mBonusPropertyModifier.rmvProperty(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _preProAdd);
		_m_mBonusPropertyModifier.addProperty(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, _curProAdd);

		if (_m_refBusinessSkill.property == ESpecAttrType.NONE)
		{
			for (ESpecAttrType type : ESpecAttrType.ESpecAttrType_Values)
			{
				//全局属性容器对象进行替换
				_m_ciConsort.getUserData().getBonusMgr().replaceModifierToFilter(EBonusFilterType.BUILDING_ATTR,
						type.ordinal(), preModifier, _m_mBonusPropertyModifier);
			}
		}else
		{
			//全局属性容器对象进行替换
			_m_ciConsort.getUserData().getBonusMgr().replaceModifierToFilter(EBonusFilterType.BUILDING_ATTR,
					_m_refBusinessSkill.property.ordinal(), preModifier, _m_mBonusPropertyModifier);
		}
	}
	
	/***
	 * 构造协议数据
	 * @return
	 */
	public Consort_BusinessSkill toProto()
	{
		Consort_BusinessSkill proto = new Consort_BusinessSkill();
		proto.setSkillId(getSkillId());
		proto.setProAdd(_m_iProAdd);
		proto.setNormalOpCount(_m_iNormalOpCount);
		proto.setAdvanceOpCount(_m_iAdvanceOpCount);
		
		return proto;
	}
	
	/*********
	 * 设置加成数值
	 * @param _proAdd
	 * @param _context
	 */
	public void setProAdd(int _proAdd, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			int preProAdd = _m_iProAdd;
			
			_m_iProAdd = _proAdd;
			//保存数据
			_save();
			
			//变更属性
			_onProAddChg(preProAdd, _m_iProAdd);
			//触发建筑重新计算产出速度
			getUserData().getBuildingComponent().recalAllBuilding();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_058_OnBusinessSkillChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置操作次数
	 * @param _isAdvance
	 * @param _count
	 */
	public void setOpCount(boolean _isAdvance, int _count)
	{
		getUserData().lockUser();
		
		try
		{
			if(_isAdvance)
				_m_iAdvanceOpCount = _count;
			else
				_m_iNormalOpCount = _count;
			
			_save();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加操作次数
	 * @param _isAdvance
	 * @param _context
	 */
	public void incrOpCount(boolean _isAdvance, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_isAdvance)
				_m_iAdvanceOpCount++;
			else
				_m_iNormalOpCount++;
			
			_save();

			_m_ciConsort.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_BUSINESS_SKILL_UPGRADE_TIMES, 1, _context);

			_m_ciConsort.getUserData().onLogicEvent(new Event_P_CONSORT_BUSINESS_SKILL_UPGRADE(_context));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 保存数据
	 */
	private void _save()
	{
		BM bmObj = getUSServer().getBM();
		
		if(null == _m_boBusinessSkill)
		{
			PlayerConsortBusinessSkillBO bo = new PlayerConsortBusinessSkillBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, getConsortId());
			bo.setSkillId(bmObj, getSkillId());
			bo.setProAdd(bmObj, _m_iProAdd);
			bo.setNormalOpCount(bmObj, _m_iNormalOpCount);
			bo.setAdvanceOpCount(bmObj, _m_iAdvanceOpCount);
			bo.insert(bmObj);
			
			_m_boBusinessSkill = bo;
		}
		else
		{
			_m_boBusinessSkill.setProAdd(bmObj, _m_iProAdd);
			_m_boBusinessSkill.setNormalOpCount(bmObj, _m_iNormalOpCount);
			_m_boBusinessSkill.setAdvanceOpCount(bmObj, _m_iAdvanceOpCount);
			_m_boBusinessSkill.saveAll(bmObj);
		}
	}
}
