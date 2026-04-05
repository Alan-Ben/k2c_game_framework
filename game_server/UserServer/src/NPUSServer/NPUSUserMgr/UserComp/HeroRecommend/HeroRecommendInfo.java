package NPUSServer.NPUSUserMgr.UserComp.HeroRecommend;

import Common.HeroRecommendObj.HeroRecommend_Info;
import NPGameRes.Refs.Hero.RefHeroRecommend;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerHeroRecommendBO;

public class HeroRecommendInfo 
{
	//玩家实例对象
	private NPUSUserData _m_udUserData;
	//大臣推荐事件的基本数据
	private long _m_lInstanceId;
	private long _m_lRefId;
	private long _m_lRndSeed;
	//大臣推荐事件配置
	private RefHeroRecommend _m_hrRef;
	
	public HeroRecommendInfo(NPUSUserData _userData, PlayerHeroRecommendBO _bo, RefHeroRecommend _ref)
	{
		_m_udUserData = _userData;
		
		_m_lInstanceId = _bo.getId();
		_m_lRefId = _bo.getRefId();
		_m_lRndSeed = _bo.getRndSeed();
		
		_m_hrRef = _ref;
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
	
	public long getInstanceId() {return _m_lInstanceId;}
	public long getRefId() {return _m_lRefId;}
	public long getRndSeed() {return _m_lRndSeed;}
	
	public RefHeroRecommend getRef() {return _m_hrRef;}
	
	/**
	 * 构造协议数据
	 * @return
	 */
	public HeroRecommend_Info toProto()
	{
		HeroRecommend_Info proto = new HeroRecommend_Info();
		proto.setInstanceId(_m_lInstanceId);
		proto.setRefId(_m_lRefId);
		proto.setRndSeed(_m_lRndSeed);
		
		return proto;
	}
	
	/**
	 * 检查大臣是否在在池子里
	 * @param _heroId
	 * @return
	 */
	public boolean isHeroInPool(long _heroId)
	{
		return _m_hrRef.hero_pool.contains(_heroId);
	}
	
	/**
	 * 移除数据
	 */
	public void del()
	{
		getUserData().getUSServer().getBM().getBM(PlayerHeroRecommendBO.class).delAll("id", _m_lInstanceId);
	}
}
