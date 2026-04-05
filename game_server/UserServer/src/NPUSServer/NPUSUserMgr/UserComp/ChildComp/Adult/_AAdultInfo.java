package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult;

import Common.ChildEnum.EAdultStatus;
import Common.ChildObj.Adult_Info;
import CommonEnum.ESpecAttrType;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerAdultBO;
import USLOGDB.Bo.LogAdultMarriedAdultBO;
import USLOGDB.Bo.LogAdultStatusChgBO;

import java.nio.ByteBuffer;

public class _AAdultInfo 
{
	//子嗣组件对象
	private ChildComponent _m_comp;
	
	//bo数据
	private PlayerAdultBO _m_bo;
	
	//子嗣状态
	private EAdultStatus _m_eStatus;
	
	protected _AAdultInfo(ChildComponent _comp, PlayerAdultBO _bo)
	{
		_m_comp = _comp;
		
		_m_bo = _bo;
		
		_m_eStatus = EAdultStatus.NONE;
	}
	
	//玩家数据
	public final ChildComponent getComp() {return _m_comp;}
	public final NPUSUserData getUserData() {return _m_comp.getUserData();}
	public final NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//子嗣数据
	public final PlayerAdultBO getBo() {return _m_bo;}
	public final long getAdultId() {return _m_bo.getId();}
	public final long getConsortId() {return _m_bo.getConsortId();}
	public final long getInitIntimacy() {return _m_bo.getInitIntimacy();}
	public final long getInitResId() {return _m_bo.getInitResId();}
	public final long getQuality() {return _m_bo.getQuality();}
	public final ESpecAttrType getAttrType() {return ESpecAttrType.ESpecAttrType_FromInt(_m_bo.getAttrType());}
	public final long getCareer() {return _m_bo.getCareer();}
	public final boolean getIsGiftde() {return _m_bo.getIsGiftde();}
	public final int getInitStudyBonus() {return _m_bo.getInitStudyBonus();}
	public int getGraduateTs() {return _m_bo.getGraduateTs();}
	public final String getName() {return _m_bo.getName();}
	public long getBonus() {return _m_bo.getBonus();}
	
	//状态数据
	public final EAdultStatus getStatus()
	{
		return _m_eStatus;
	}
	//设置状态数据
	public final void setStatus(EAdultStatus _status)
	{
		_m_eStatus = _status;

		if(_status == EAdultStatus.MARRIED)
		{
			//已婚状态，检查Bo数据，并修正
			if(!_m_bo.getIsMarried())
				_m_bo.saveIsMarried(getUSServer().getBM(), true);
		}
	}
	
	//是否空闲
	public final boolean isIdle() {return EAdultStatus.NONE == _m_eStatus;}
	//是否结婚
	public final boolean isMarried() {return EAdultStatus.MARRIED == _m_eStatus;}
	//是否发起个人联姻
	public final boolean isApplyPerson() {return EAdultStatus.APPLY_PLAYER == _m_eStatus;}
	//是否发起全服联姻
	public final boolean isApplyServer() {return EAdultStatus.APPLY_SERVER == _m_eStatus;}
	//获取毕业奖励
	public final NPCommon_ItemInfo getGraduateItem()
	{
		NPCommon_ItemInfo item = new NPCommon_ItemInfo();
		if(null != _m_bo.getGraduateItem())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getGraduateItem());
			item.readPackage(buff);
		}
		
		return item;
	}
	
	/**
	 * 构造数据协议对象
	 * @return
	 */
	public final Adult_Info toProto()
	{
		Adult_Info proto = new Adult_Info();
		proto.setId(getAdultId());
		proto.setConsortId(getConsortId());
		proto.setInitResId(getInitResId());
		proto.setQuality(getQuality());
		proto.setAttrType(getAttrType());
		proto.setCareerId(getCareer());
		proto.setIsGiftde(getIsGiftde());
		proto.setInitStudyBonus(getInitStudyBonus());
		proto.setName(getName());
		proto.setBonus(getBonus());
		proto.setGraduateTs(getGraduateTs());
		
		return proto;
	}
	
	/**
	 * 销毁数据
	 */
	protected final void _discard()
	{
		_m_bo.del(getUSServer().getBM());
	}

	/**
	 * MJ日志
	 * @param _stateId 1 = 获取、2 = 取名、3 = 升级、4 = 成年、5 = 已婚、6 = 流放
	 */
	public void mjLog(int _stateId)
	{
		MJEventLog.logChild(getUserData(), getAdultId(), getInitResId(), 0, getConsortId(), 0, _stateId, 0, getQuality(), getGraduateTs());
	}
	
	/**
	 * 子嗣变化状态日志
	 * 
	 * @param _adult
	 * @param _preStatus
	 * @param _newStatus
	 * @param _expiredTs
	 * @param _context
	 */
	public static void logAdultStatusChg(_AAdultInfo _adult, EAdultStatus _preStatus, EAdultStatus _newStatus, int _expiredTs, NPPlayerContext _context)
	{
		//无变化无需记录
		if(_preStatus == _newStatus)
			return;
		
		BM bmObj = _adult.getUSServer().getBM();
		
		LogAdultStatusChgBO logBo = new LogAdultStatusChgBO();
		logBo.setCid(bmObj, _adult.getUserData().getCid());
		logBo.setPreStatus(bmObj, _preStatus.ordinal());
		logBo.setNewStatus(bmObj, _newStatus.ordinal());
		logBo.setExpiredTs(bmObj, _expiredTs);
		
		CommLogDB.log(bmObj, logBo, _context);
	}
	
	/**
	 * 已婚子嗣日志
	 * 
	 * @param _marriedAdult
	 * @param _context
	 */
	public static void logMarriedAdult(MarriedAdultInfo _marriedAdult, NPPlayerContext _context)
	{
		BM bmObj = _marriedAdult.getUSServer().getBM();
		
		LogAdultMarriedAdultBO logBo = new LogAdultMarriedAdultBO();
		logBo.setCid(bmObj, _marriedAdult.getUserData().getCid());
		logBo.setAdultId(bmObj, _marriedAdult.getAdultId());
		logBo.setMarriedCid(bmObj, _marriedAdult.getMarriedCid());
		logBo.setMarriedAdultId(bmObj, _marriedAdult.getMarriedAdultId());
		logBo.setMarriedBonus(bmObj, _marriedAdult.getMarriedBonus());
		
		CommLogDB.log(bmObj, logBo, _context);
	}
}
