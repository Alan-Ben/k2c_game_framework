package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult;

import Common.ChildEnum.EAdultStatus;
import Common.ChildObj.Adult_Info;
import Common.ChildObj.Adult_MarriedInfo;
import CommonEnum.ESpecAttrType;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import USDB.Bo.PlayerAdultBO;
import USDB.Bo.PlayerAdultMarriedBO;

/**
 * 成年已婚子嗣数据
 * @author mj
 *
 */
public class MarriedAdultInfo extends _AAdultInfo
{
	//关联结婚bo数据
	private PlayerAdultMarriedBO _m_marriedBo;
	
	public MarriedAdultInfo(ChildComponent _comp, PlayerAdultBO _bo, PlayerAdultMarriedBO _marriedBo)
	{
		super(_comp, _bo);
		
		_m_marriedBo = _marriedBo;
		
		setStatus(EAdultStatus.MARRIED);
	}

	//结婚子嗣关联数据
	//已婚数据
	public PlayerAdultMarriedBO getMarriedBo() {return _m_marriedBo;}
	public int getMarriedTs() {return getMarriedBo().getMarriedTs();}
	//目标子嗣的数据
	public long getMarriedCid() {return getMarriedBo().getMarriedCid();}
	public long getMarriedAdultId() {return getMarriedBo().getMarriedAdultId();}
	public long getMarriedInitResId() {return getMarriedBo().getInitResId();}
	public long getMarriedQuality() {return getMarriedBo().getQuality();}
	public ESpecAttrType getMarriedAttrType() {return ESpecAttrType.ESpecAttrType_FromInt(getMarriedBo().getAttrType());}
	public long getMarriedCareer() {return getMarriedBo().getCareer();}
	public String getMarriedName() {return getMarriedBo().getName();}
	public long getMarriedBonus() {return getMarriedBo().getBonus();}
	
	/**
	 * 已婚关联子嗣数据
	 * @return
	 */
	public Adult_Info toMarriedRelatedAdultProto()
	{
		Adult_Info proto = new Adult_Info();
		proto.setId(getMarriedAdultId());
		proto.setInitResId(getMarriedInitResId());
		proto.setQuality(getMarriedQuality());
		proto.setAttrType(getMarriedAttrType());
		proto.setCareerId(getMarriedCareer());
		proto.setIsGiftde(getIsGiftde());
		proto.setName(getMarriedName());
		proto.setBonus(getMarriedBonus());
		
		return proto;
	}
	
	/**
	 * 构造已婚子嗣数据
	 * @return
	 */
	public Adult_MarriedInfo toMarriedProto()
	{
		Adult_MarriedInfo proto = new Adult_MarriedInfo();
		proto.setAdult(toProto());
		proto.setMarriedCid(getMarriedCid());
		proto.setMarriedAdult(toMarriedRelatedAdultProto());
		proto.setMarriedTs(getMarriedTs());
		
		return proto;
	}
	
	/**
	 * 销毁数据
	 */
	public void discard()
	{
		//子嗣数据移除
		_discard();
		
		//子嗣关联数据移除
		_m_marriedBo.del(getUSServer().getBM());
		
		//移除子嗣的收益记录在玩家属性里
		long bonus = getBonus() + getMarriedBonus();
		getUserData().incParam(ENPPlayerParam.ADULT_RECORD_BONUS, bonus);
	}
}
