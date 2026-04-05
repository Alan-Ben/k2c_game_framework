package NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr;

import Common.ChildObj.Adult_Info;
import Common.ChildObj.Adult_ToMeApplyBaseInfo;
import Common.ChildObj.Adult_ToMeApplyInfo;
import CommonEnum.ESpecAttrType;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerAdultToMeApplyBO;

import java.nio.ByteBuffer;

public class ToMeMarryApplyInfo 
{
	//子嗣组件对象
	private ChildComponent _m_comp;
	
	//数据对象
	private PlayerAdultToMeApplyBO _m_bo;
	
	public ToMeMarryApplyInfo(ChildComponent _comp, PlayerAdultToMeApplyBO _bo)
	{
		_m_comp = _comp;
		
		_m_bo = _bo;
	}
	
	//玩家数据
	public ChildComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//bo数据
	public PlayerAdultToMeApplyBO getBo() {return _m_bo;}
	//请求子嗣的数据
	public long getApplyCid() {return _m_bo.getApplyCid();}
	public String getApplyCname() {return _m_bo.getApplyCname();}
	public long getApplyAdultId() {return _m_bo.getApplyAdultId();}
	public long getApplyAdultInitResId() {return _m_bo.getInitResId();}
	public long getApplyQuality() {return _m_bo.getQuality();}
	public ESpecAttrType getApplyAttrType() {return ESpecAttrType.ESpecAttrType_FromInt(_m_bo.getAttrType());}
	public long getApplyCareer() {return _m_bo.getCareer();}
	public boolean getIsGiftde() {return _m_bo.getIsGiftde();}
	public String getApplyName() {return _m_bo.getName();}
	public long getBonus() {return _m_bo.getBonus();}
	//获取毕业奖励
	public final NPCommon_ItemInfo getMarriedItem()
	{
		NPCommon_ItemInfo item = new NPCommon_ItemInfo();
		if(null != _m_bo.getMarriedItem())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getMarriedItem());
			item.readPackage(buff);
		}
		
		return item;
	}
	//请求截至时间（毫秒）
	public int getApplyExpiredTs() {return _m_bo.getApplyExpiredTs();}
	
	/**
	 * 构造请求子嗣数据
	 * @return
	 */
	public Adult_Info toApplyAdult()
	{
		Adult_Info proto = new Adult_Info();
		proto.setId(getApplyAdultId());
		proto.setInitResId(getApplyAdultInitResId());
		proto.setQuality(getApplyQuality());
		proto.setAttrType(getApplyAttrType());
		proto.setCareerId(getApplyCareer());
		proto.setIsGiftde(getIsGiftde());
		proto.setName(getApplyName());
		proto.setIsGiftde(getIsGiftde());
		proto.setBonus(getBonus());
		
		return proto;
	}
	
	/**
	 * 简要数据
	 * @return
	 */
	public Adult_ToMeApplyBaseInfo toBaseProto()
	{
		Adult_ToMeApplyBaseInfo proto = new Adult_ToMeApplyBaseInfo();
		proto.setApplyAdultId(getApplyAdultId());
		proto.setExpiredTs(getApplyExpiredTs());
		
		return proto;
	}

	/**
	 * 详细数据
	 * @return
	 */
	public Adult_ToMeApplyInfo toProto()
	{
		Adult_ToMeApplyInfo proto = new Adult_ToMeApplyInfo();
		proto.setApplyCid(getApplyCid());
		proto.setApplyAdult(toApplyAdult());
		
		return proto;
	}
	
	/**
	 * 检查请求是否超时检查
	 * @return
	 */
	public boolean isExpired()
	{
		return CommonFunc.getNowTimeSec() > getApplyExpiredTs();
	}
	
	/**
	 * 销毁数据
	 */
	public void discard()
	{
		_m_bo.del(getComp().getUSServer().getBM());
	}
}
