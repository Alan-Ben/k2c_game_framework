package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child;

import Common.ChildObj.Child_Info;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import CommonEnum.EChildSexType;
import CommonEnum.ESpecAttrType;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Child.RefChildAttr;
import NPGameRes.Refs.Child.RefChildCareer;
import NPGameRes.Refs.Child.RefChildInitRes;
import NPGameRes.Refs.Child.RefChildQuality;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerChildBO;

public class ChildInfo 
{
	//子嗣组件对象
	private ChildMgr _m_mgrChild;
	
	//子嗣Bo数据
	private PlayerChildBO _m_bo;
	
	//配表数据
	private RefChildInitRes _m_refInitRes;//初始化形象配置
	private RefChildQuality _m_refQuality;//子嗣品质配置
	private RefChildAttr _m_refAttr;//子嗣相性配置
	private RefChildCareer _m_refCareer;//子嗣职业配置
	
	public ChildInfo(ChildMgr _mgr, PlayerChildBO _bo)
	{
		_m_mgrChild = _mgr;
		
		_m_bo = _bo;
		
		_m_refInitRes = RefChildInitRes.getMgr().get(_m_bo.getInitResId());
		_m_refQuality = RefChildQuality.getMgr().get(_m_bo.getQuality());
		_m_refAttr = RefChildAttr.getMgr().get(_m_bo.getAttrType());
		_m_refCareer = RefChildCareer.getMgr().get(_m_bo.getCareer());
	}
	public ChildInfo(ChildMgr _mgr, PlayerChildBO _bo
			, RefChildInitRes _initResRef
			, RefChildQuality _qualityRef
			, RefChildAttr _attrRef
			, RefChildCareer _careerRef)
	{
		_m_mgrChild = _mgr;
		
		_m_bo = _bo;
		
		_m_refInitRes = _initResRef;
		_m_refQuality = _qualityRef;
		_m_refAttr = _attrRef;
		_m_refCareer = _careerRef;
	}
	
	//玩家数据
	public ChildMgr getChildMgr() {return _m_mgrChild;}
	public ChildComponent getComp() {return _m_mgrChild.getComp();}
	public NPUSUserData getUserData() {return _m_mgrChild.getComp().getUserData();}
	public NPUserServer getUserServer() {return _m_mgrChild.getComp().getUserData().getUSServer();}
	
	//bo数据
	public PlayerChildBO getBo() {return _m_bo;}
	//子嗣实例ID
	public long getChildId() {return _m_bo.getId();}
	//关联家人ID
	public long getConsortId() {return _m_bo.getConsortId();}
	//初始亲密度
	public long getInitIntimacy() {return _m_bo.getInitIntimacy();}
	//训练房ID
	public long getSeatId() {return _m_bo.getSeatId();}
	//是否卷王
	public boolean isGiftde() {return _m_bo.getIsGiftde();}
	//教学经验加成（万分比）
	public int getInitStudyBonus() {return _m_bo.getInitStudyBonus();}
	//子嗣名称
	public String getName() {return _m_bo.getName();}
	//子嗣等级
	public int getLvl() {return _m_bo.getLvl();}
	//创建时间
	public int getCreatedAt() {return _m_bo.getCreatedAt();}
	
	//子嗣基础收益
	public long getBaseBonus() {return _m_bo.getBaseBonus();}
	//子嗣上课收益
	public long getTrainBonus() {return _m_bo.getTrainBonus();}

	//形象配置
	public long getInitResId() {return _m_bo.getInitResId();}//从数据库获取
	public RefChildInitRes getInitResRef() {return _m_refInitRes;}
	public EChildSexType getSex() {return null == _m_refInitRes ? EChildSexType.NONE : _m_refInitRes.sex;}
	//品质配置
	public long getQuality() {return _m_bo.getQuality();}//从数据库获取
	public RefChildQuality getQualityRef() {return _m_refQuality;}
	public int getMaxLvl() {return null == _m_refQuality ? 0 : _m_refQuality.maxLvl;}
	//相性配置
	public int getAttrV() {return _m_bo.getAttrType();}//从数据库获取
	public RefChildAttr getAttrRef() {return _m_refAttr;}
	public ESpecAttrType getAttr() {return null == _m_refAttr ? ESpecAttrType.NONE : _m_refAttr.type;}
	//职业配置
	public long getCareer() {return _m_bo.getCareer();}//从数据库获取
	public RefChildCareer getCareerRef() {return _m_refCareer;}
	
	/**
	 * 获取子嗣全局数据
	 * @param _type
	 * @return
	 */
	public long getBonusPropertyValue(EBonusPropertyType _type)
	{
		return getUserData().getBonusMgr().getTotalPropertyBonus(_type)
				+ getUserData().getBonusMgr().getFilterPropertyBonus(_type, EBonusFilterType.STUDENT_SEX, getSex().ordinal())
                + getUserData().getBonusMgr().getFilterPropertyBonus(_type, EBonusFilterType.STUDENT_ATTR, getAttr().ordinal());
	}
	
	/**
	 * 构造数据
	 * @return
	 */
	public Child_Info toProto()
	{
		Child_Info proto = new Child_Info();
		proto.setId(getChildId());
		proto.setConsortId(getConsortId());
		proto.setInitIntimacy(getInitIntimacy());
		proto.setInitResId(getInitResId());
		proto.setQuality(getQuality());
		proto.setAttrType(getAttr());
		proto.setCareerId(getCareer());
		proto.setSeatId(getSeatId());
		proto.setIsGiftde(isGiftde());
		proto.setName(getName());
		proto.setLvl(getLvl());
		proto.setBaseBonus(getBaseBonus());
		proto.setBonus(getTrainBonus());
		proto.setInitStudyBonus(getInitStudyBonus());
		
		return proto;
	}
	
	/**
	 * 设置名称
	 * @param _name
	 * @param _context
	 */
	public void setName(String _name, NPPlayerContext _context)
	{
		BM bmObj = getUserServer().getBM();
		
		getBo().setName(bmObj, _name);
		getBo().saveAll(bmObj);
		
		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_051_OnChildNameChg(this));

		//mj日志
		mjLog(2);
	}
	
	/**
	 * 设置子嗣等级
	 * @param _lvl
	 * @param _context
	 */
	public void setLvl(int _lvl, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(getLvl() == _lvl)
				return;
			
			//保存数据
			BM bmObj = getUserServer().getBM();
			getBo().setLvl(bmObj, _lvl);
			getBo().saveAll(bmObj);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_052_OnChildLvlChg(this));

			//mj日志
			RefChildQuality qualityRef = getQualityRef();
			if(null != qualityRef && qualityRef.step_lvl.contains(_lvl))
			{
				mjLog(3);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	/**
	 * 子嗣升级（等级+1）
	 * @param _context
	 */
	public void incrLvl(NPPlayerContext _context)
	{
		setLvl(getLvl() + 1, _context);

		getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.CHILD_TRAIN_TIMES, 1, _context);
	}
	
	/**
	 * 设置上课收益
	 * @param _value
	 * @param _context
	 */
	public void setTrainBonus(long _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(getTrainBonus() == _value)
				return;
			
			//保存数据
			BM bmObj = getUserServer().getBM();
			getBo().setTrainBonus(bmObj, _value);
			getBo().saveAll(bmObj);
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_062_OnChildBonusChg(this));
			
			//计算产出速度
			getChildMgr().recalBonus();
			
			//单个子嗣收益最高记录
			getUserData().getRecordComponent().ensureRecord(ENPPlayerRecordParam.MAX_EARNINGS_CHILD, getTrainBonus(), ENCounterDealType.SET_GT, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加上课收益
	 * @param _addValue
	 * @param _context
	 */
	public void incrTrainBonus(long _addValue, NPPlayerContext _context)
	{
		setTrainBonus(getTrainBonus() + _addValue, _context);
	}
	
	/**
	 * 销毁数据
	 */
	protected	void _discard()
	{
		getBo().del(getComp().getUSServer().getBM());
	}
	
	/**
	 * MJ日志
	 * @param _stateId 1 = 获取、2 = 取名、3 = 升级、4 = 成年、5 = 已婚、6 = 流放
	 */
	public void mjLog(int _stateId)
	{
		MJEventLog.logChild(getUserData(), getChildId(), getInitResId(), getTrainBonus(), getConsortId(), getLvl(), _stateId, getSex().ordinal(), getQuality(), getCreatedAt());
	}
}
