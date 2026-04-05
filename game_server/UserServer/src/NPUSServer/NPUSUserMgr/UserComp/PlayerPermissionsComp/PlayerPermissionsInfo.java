package NPUSServer.NPUSUserMgr.UserComp.PlayerPermissionsComp;

import NPGameRes.Refs.RefPlayerPermissions;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class PlayerPermissionsInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//玩家权限配置
	private RefPlayerPermissions _m_ref;
	
	//指针计数，外部增加引用+1，外部减少引用-1，0：失效
	private int _m_iCounter;
	
	public PlayerPermissionsInfo(NPUSUserData _userData, RefPlayerPermissions _ref)
	{
		_m_udUserData = _userData;
		_m_ref = _ref;
	}

    public NPUSUserData getUserData() {return _m_udUserData;}
	
	public RefPlayerPermissions getRef() {return _m_ref;}
	
	public long getId() {return _m_ref.id;}
	
	public int getCounter() {return _m_iCounter;}
	
	/**
	 * 检查是否生效
	 * @return
	 */
	public boolean isEffect()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_iCounter > 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加计数
	 * @return - 是否发生状态改变
	 */
	public boolean addCounter()
	{
		getUserData().lockUser();
		
		try
		{
			boolean preEffect = isEffect();
			
			_m_iCounter++;

			//状态发生改变
			return preEffect != isEffect();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 减少计数
	 * @return - 是否发生状态改变
	 */
	public boolean reduCounter()
	{
		getUserData().lockUser();
		
		try
		{
			boolean preEffect = isEffect();
			
			_m_iCounter--;
			_m_iCounter = Math.max(_m_iCounter, 0);
			
			//状态发生改变
			return preEffect != isEffect();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
