package NPUSServer.NPUSUserMgr.UserComp.PlayerPermissionsComp;

import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.Delegate.ADelegateOne;
import NPGameRes.Refs.RefPlayerPermissions;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家权限数据
 * @author mj
 *
 */
public class PlayerPermissionsComponent extends _ANPUserComponent
{
	//权限列表
	private ArrayList<PlayerPermissionsInfo> _m_alPermissionsList;
    //权限变更触发器
    private ADelegateOne<Long> _m_adPermissionChgDelegate;
	
	public PlayerPermissionsComponent(NPUSUserData _userData) 
	{
		super(_userData, ENPPlayerCompType.PLAYER_PERMISSIONS);
		
		_m_alPermissionsList = new ArrayList<>();
		
		_m_adPermissionChgDelegate = new ADelegateOne<>(this);
	}

    public ADelegateOne<Long> getPermissionChgDelegate() {return _m_adPermissionChgDelegate;}
    
	@Override
	protected void _init() 
	{
		//加载配置上所有数据
		_initAllRef();
		
		setInited();
	}
	
	/**
	 * 加载配置上所有数据
	 */
	private void _initAllRef()
	{
		List<RefPlayerPermissions> refList = RefPlayerPermissions.getMgr().getList();
		for(int i = 0; i < refList.size(); i++)
		{
			RefPlayerPermissions ref = refList.get(i);
			if(null == ref)
				continue;
			
			PlayerPermissionsInfo info = new PlayerPermissionsInfo(getUserData(), ref);
			_m_alPermissionsList.add(info);
		}
	}

	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
	}

	@Override
	public void dispose() 
	{
	}
	
	/**
	 * 构造数据列表 - 生效的权限ID列表
	 * @param _list
	 */
	public void makeProto(ArrayList<Long> _list)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPermissionsList.size(); i++)
			{
				PlayerPermissionsInfo info = _m_alPermissionsList.get(i);
				if(null == info)
					continue;
				
				if(info.isEffect())
				{
					_list.add(info.getId());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找对应的玩家权限数据
	 * @param _id
	 * @return
	 */
	public PlayerPermissionsInfo lookup(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alPermissionsList.size(); i++)
			{
				PlayerPermissionsInfo info = _m_alPermissionsList.get(i);
				if(null == info)
					continue;
				
				if(info.getId() == _id)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查指定权限是否生效
	 * @param _id
	 * @return
	 */
	public boolean checkPermissionEffect(long _id)
	{
		getUserData().lockUser();
		
		try
		{
			PlayerPermissionsInfo info = lookup(_id);
			
			return null != info && info.isEffect();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 批量增加权限计数列表
	 * @param _idList
	 * @param _push
	 */
	public void addPermissionList(ArrayList<Long> _idList, boolean _push)
	{
		getUserData().lockUser();
		
		try
		{
			boolean hasChanged = false;
			for(int i = 0; i < _idList.size(); i++)
			{
				PlayerPermissionsInfo info = lookup(_idList.get(i));
				if(null == info)
					continue;
				
				if(info.addCounter())
				{
					_m_adPermissionChgDelegate.onAsyncEvent(info.getId());
					
					hasChanged = true;
				}
			}
			
			if(_push && hasChanged)
			{
				pushAllEffect();
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 批量扣除权限计数列表
	 * @param _idList
	 * @param _push
	 */
	public void reduPermissionList(ArrayList<Long> _idList, boolean _push)
	{
		getUserData().lockUser();
		
		try
		{
			boolean hasChanged = false;
			for(int i = 0; i < _idList.size(); i++)
			{
				PlayerPermissionsInfo info = lookup(_idList.get(i));
				if(null == info)
					continue;
				
				if(info.reduCounter())
				{
					_m_adPermissionChgDelegate.onAsyncEvent(info.getId());
					
					hasChanged = true;
				}
			}
			
			if(_push && hasChanged)
			{
				pushAllEffect();
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 推送所有生效权限列表
	 */
	public void pushAllEffect()
	{
		getUserData().lockUser();
		
		try
		{
			ArrayList<Long> effectIdList = new ArrayList<>();
			for(int i = 0; i < _m_alPermissionsList.size(); i++)
			{
				PlayerPermissionsInfo info = _m_alPermissionsList.get(i);
				if(null == info)
					continue;
				
				if(info.isEffect())
				{
					effectIdList.add(info.getId());
				}
			}
			
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_062_OnPlayerPermissionsChg(effectIdList));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
