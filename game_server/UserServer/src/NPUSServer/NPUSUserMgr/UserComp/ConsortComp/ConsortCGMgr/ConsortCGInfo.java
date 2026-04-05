package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCGMgr;

import Common.ConsortObj.Consort_CGInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Consort.RefConsortCg;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortCgBO;

public class ConsortCGInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//bo数据
	private PlayerConsortCgBO _m_boCgBO;
	
	//配置数据
	private RefConsortCg _m_refCg;
	
	/**
	 * 不带配表数据
	 * @param _userData
	 * @param _bo
	 */
	public ConsortCGInfo(NPUSUserData _userData, PlayerConsortCgBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_boCgBO = _bo;
		
		RefConsortCg ref = RefConsortCg.getMgr().get(_m_boCgBO.getCgId());
		if(null == ref)
		{
			USLog.error(getUSServer(), "player:{} init cg:{} ref fail, not find ref.", getUserData().getCid(), getCgId());
		}
		else
		{
			_m_refCg = ref;
		}
	}
	/**
	 * 带配表数据
	 * @param _userData
	 * @param _bo
	 * @param _ref
	 */
	public ConsortCGInfo(NPUSUserData _userData, PlayerConsortCgBO _bo, RefConsortCg _ref)
	{
		_m_udUserData = _userData;
		
		_m_boCgBO = _bo;
		
		_m_refCg = _ref;
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_udUserData;}
	//服务器数据对象
	public NPUserServer getUSServer() {return _m_udUserData.getUSServer();}
	
	//bo数据
	public PlayerConsortCgBO getBo() {return _m_boCgBO;}
	public long getCgId() {return _m_boCgBO.getCgId();}
	public boolean isRewarded() {return _m_boCgBO.getRewarded();}
	
	//配置数据
	public RefConsortCg getRef() {return _m_refCg;}
	public long getRelationConsortId() {return null == _m_refCg ? 0 : _m_refCg.consort_id;}//关联家人ID
	public int getAdd() {return null == _m_refCg ? 0 : _m_refCg.add;}//加护点加成万分比
	
	/**
	 * 构造数据协议对象
	 * @return
	 */
	public Consort_CGInfo toProto()
	{
		Consort_CGInfo proto = new Consort_CGInfo();
		proto.setCgId(getCgId());
		proto.setRewarded(isRewarded());
		
		return proto;
	}
	
	/**
	 * 设置领取奖励标志
	 * @param _context
	 */
	public void setRewarded(NPPlayerContext _context)
	{
		BM bmObj = getUSServer().getBM();
		
		_m_boCgBO.setRewarded(bmObj, true);
		_m_boCgBO.saveAll(bmObj);

		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_061_OnCgChg(this));
	}
}
