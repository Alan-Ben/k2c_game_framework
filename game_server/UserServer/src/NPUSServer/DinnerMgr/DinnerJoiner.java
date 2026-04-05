package NPUSServer.DinnerMgr;

import Common.DinnerEnum.EDinnerJoinerType;
import Common.DinnerObj.Dinner_Joiner;
import Common.DinnerObj.Dinner_ResultGuestInfo;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsDinnerJoinerBO;

/**
 * 赴宴玩家数据
 *
 */
public class DinnerJoiner 
{
	//归属宴会数据
	private DinnerInfo _m_diDinnerInfo;
	//参与宴会玩家数据
	private UsDinnerJoinerBO _m_bo;
	
	//赴宴玩家配置
	private RefDinnerJoinCost _m_ref;
	
	public DinnerJoiner(DinnerInfo _dinner, UsDinnerJoinerBO _bo)
	{
		_m_diDinnerInfo = _dinner;
		
		_m_bo = _bo;
		
		_m_ref = RefDinnerJoinCost.getMgr().get(_m_bo.getCostId());
		if(null == _m_ref)
		{
			USLog.error(getUSServer(), "Dinner:{} Cost:{} Joiner:{}-{} can not get ref.", getDinnerId(), getCostId(), getJoinerType(), getJoinerId());
		}
	}
	public DinnerJoiner(DinnerInfo _dinner, UsDinnerJoinerBO _bo, RefDinnerJoinCost _ref)
	{
		_m_diDinnerInfo = _dinner;
		
		_m_bo = _bo;
		
		_m_ref = _ref;
	}
	
	//宴会数据
	public DinnerInfo getDinner() {return _m_diDinnerInfo;}
	public long getDinnerId() {return _m_diDinnerInfo.getDinnerId();}
    //US服务器
	public NPUserServer getUSServer() {return _m_diDinnerInfo.getUSServer();}
	//赴宴玩家数据
	public UsDinnerJoinerBO getBo() {return _m_bo;}
	public long getCostId() {return _m_bo.getCostId();}
	public EDinnerJoinerType getJoinerType() {return EDinnerJoinerType.EDinnerJoinerType_FromInt(_m_bo.getJoinerType());}
	public long getJoinerId() {return _m_bo.getJoinerId();}
	public int getJoinTs() {return _m_bo.getJoinTs();}
	public long getCoin() {return _m_bo.getGainCoin();}
	public long getScore() {return _m_bo.getGainScore();}
	//配置数据
	public RefDinnerJoinCost getCostRef() {return _m_ref;}
	public long getBaseCoin() {return null == _m_ref ? 0 : _m_ref.join_gain_coin;}
	
	/**
	 * 构造协议对象
	 * @return
	 */
	public Dinner_Joiner toProto()
	{
		Dinner_Joiner proto = new Dinner_Joiner();
		proto.setJoinerType(getJoinerType());
		proto.setJoinerId(getJoinerId());
		proto.setJoinTimeMs(getJoinTs() * 1000L);
		proto.setCostId(getCostId());
		proto.setScore(getScore());
		return proto;
	}
	
	/**
	 * 宴会结算中的宾客记录
	 * @return
	 */
	public Dinner_ResultGuestInfo toResultGuestProto()
	{
		Dinner_ResultGuestInfo proto = new Dinner_ResultGuestInfo();
		proto.setJoinerType(getJoinerType());
		proto.setJoinerId(getJoinerId());
		proto.setCostId(getCostId());
		proto.setCoin(getBaseCoin());
		proto.setScore(getScore());
		
		return proto;
	}
	
	/**
	 * 计算开宴玩家收获的宴会币
	 * @return
	 */
	public long calOwnerCoin()
	{
		if(null == _m_ref)
			return getCoin();
		
		return (long) Math.ceil(1.0f * getCoin() * _m_ref.banquet_host_factor / 10000f);
	}
}
