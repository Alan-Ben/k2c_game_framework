package NPUSServer.NPUSUserMgr.UserComp.ForeverAddComp;

import NPCommon.DB.BM.BM;
import NPCommon.NPCommon_ForeverAddInfo;
import NPGameRes.Refs.RefPlayerForeverAdd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerForeverAddBO;

public class ForeverAddInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//数据bo
	private PlayerForeverAddBO _m_bo;
	
	//配置数据
	private RefPlayerForeverAdd _m_ref;
	
	public ForeverAddInfo(NPUSUserData _userData, PlayerForeverAddBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_m_ref = RefPlayerForeverAdd.getMgr().get(_m_bo.getItemId());
	}
	public ForeverAddInfo(NPUSUserData _userData, PlayerForeverAddBO _bo, RefPlayerForeverAdd _ref)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		_m_ref = _ref;
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
	
	public PlayerForeverAddBO getBo() {return _m_bo;}
	public long getItemId() {return getBo().getItemId();}
	public int getItemCount() {return getBo().getItemCount();}
	
	public RefPlayerForeverAdd getRef() {return _m_ref;}
	
	protected void _onInited() 
	{
		if(null == _m_ref)
		{
			USLog.error(getUSServer(), "player:{} itemId:{} init forever add fail, not find ref.", getUserData().getCid(), getItemId());
			return;
		}
		
		if(getItemCount() > 0)
		{
			getUserData().getForeverAddComponent().getPlayerPropertyContainer().addModifier(_m_ref.player_pro_add, getItemCount());
		}
	}
	
	public NPCommon_ForeverAddInfo toProto()
	{
		NPCommon_ForeverAddInfo proto = new NPCommon_ForeverAddInfo();
		proto.setId(getItemId());
		proto.setCount(getItemCount());
		
		return proto;
	}
	
	/**
	 * 设置数量
	 * @param _count
	 * @param _context
	 */
	public void setCount(int _count, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(null == _m_ref)
				return;
			
			//检查允许最大值
			if(_m_ref.add_limit > 0)
				_count = Math.min(_count, _m_ref.add_limit);
			
			//数据一致，无需处理
			if(_count == getItemCount())
				return;
			
			int preCount = getItemCount();
			
			getBo().saveItemCount(getBM(), _count);
			
			//移除原玩家属性
			if(preCount > 0)
				getUserData().getForeverAddComponent().getPlayerPropertyContainer().removeModifier(_m_ref.player_pro_add, preCount);
			//更新当前玩家属性
			if(getItemCount() > 0)
				getUserData().getForeverAddComponent().getPlayerPropertyContainer().addModifier(_m_ref.player_pro_add, getItemCount());

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_075_OnForeverAddChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
