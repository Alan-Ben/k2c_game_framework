package NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp;

import Common.GuildObj.Guild_MarsHelpInfo;
import Common.MarsObj.Mars_Technology;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPGameRes.Refs.Mars.RefMarsTechnology;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsHelp.GuildMarsHelpInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsTechBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 火星 - 前往火星
 * @author mj
 *
 */
public class MarsTechComponent extends _ANPUserComponent
{
	//火星科技数据对象
	private ArrayList<MarsTechInfo> _m_alTechList;
	
    public MarsTechComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_TECH);
        
        _m_alTechList = new ArrayList<>();
    }
    
    @Override
    protected void _init()
    {
        _initAllRef();
    	
        getUSServer().getBM().getBM(PlayerMarsTechBO.class).findAll("cid", getCid(), 
        		new _ASelectCallback<List<PlayerMarsTechBO>>()
        {
            @Override
            public void dealFail()
            {
            	getUserData().setDataLoadFail();
            }
            
            @Override
            public void dealSuc(List<PlayerMarsTechBO> _list)
            {
            	_initFromBoList(_list);
            }
        });
    }
    
    private void _initAllRef()
    {
    	List<RefMarsTechnology> refList = RefMarsTechnology.getMgr().getList();
    	for(int i = 0; i < refList.size(); i++)
    	{
    		RefMarsTechnology ref = refList.get(i);
    		if(null == ref)
    			continue;
    		
    		MarsTechInfo info = new MarsTechInfo(getUserData(), ref);
    		_m_alTechList.add(info);
    	}
    }
    
    private void _initFromBoList(List<PlayerMarsTechBO> _list)
    {
    	for(int i = 0; i < _list.size(); i++)
    	{
    		PlayerMarsTechBO bo = _list.get(i);
    		if(null == bo)
    			continue;
    		
    		MarsTechInfo info = lookup(bo.getTechId());
    		if(null == info)
    		{
    			USLog.error(getUSServer(), "player:{} tech:{} mars tech init load bo fail, not find info.", getCid(), bo.getTechId());
    			continue;
    		}
    		
    		info._loadBo(bo);
    	}
    	
    	setInited();
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    	//对所有科技进行初始化
    	for(int i = 0; i < _m_alTechList.size(); i++)
    	{
    		MarsTechInfo info = _m_alTechList.get(i);
    		if(null == info)
    			continue;
    		
    		info._onInited();
    	}
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }
    
    /**
     * 获取正在升级的数量
     * @return
     */
    public int getUpgradingSum()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		int sum = 0;
    		for(int i = 0; i < _m_alTechList.size(); i++)
    		{
    			MarsTechInfo info = _m_alTechList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.isUpgrading())
    				sum++;
    		}
    		
    		return sum;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 计算所有科技的火星实力
     * @return
     */
    public long getMarsPowerSum(StringBuilder _sb)
    {
    	getUserData().lockUser();
    	
    	try
    	{
			if(null != _sb)
			{
				_sb.append("\n---------------- cal tech mars power ----------------");
			}
			
    		int value = 0;
    		for(int i = 0; i < _m_alTechList.size(); i++)
    		{
    			MarsTechInfo info = _m_alTechList.get(i);
    			if(null == info)
    				continue;

    			long power = info.getMarsPower();
				if(null != _sb)
				{
					_sb.append("\ntech:").append(info.getTechId()).append(", power:").append(power);
				}
				
    			value += power;
    		}
    		
    		return value;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 查找指定的火星科技数据
     * @param _techId
     * @return
     */
    public MarsTechInfo lookup(long _techId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTechList.size(); i++)
    		{
    			MarsTechInfo info = _m_alTechList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getTechId() == _techId)
    			{
    				return info;
    			}
    		}
    		
    		return null;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 检查指定parent是否解锁
     * @param _parent
     * @return
     */
    public boolean checkUnlockParent(ArrayList<Long> _idList)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _idList.size(); i++)
    		{
    			MarsTechInfo info = lookup(_idList.get(i));
    			if(null == info)
    				return false;
    			
    			if(!info.isUnlock())
    				return false;
    		}
    		
    		return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造科技数据协议
     * @param _list
     */
    public void makeProto(ArrayList<Mars_Technology> _list)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for(int i = 0; i < _m_alTechList.size(); i++)
    		{
    			MarsTechInfo info = _m_alTechList.get(i);
    			if(null == info)
    				continue;
    			
    			_list.add(info.toProto());
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

	/**
	 * 构造玩家自身发起的求助数据
	 * @param _guild
	 * @param _list
	 */
	public void makeSendGuildHelpProto(GuildInfo _guild, ArrayList<Guild_MarsHelpInfo> _list) 
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alTechList.size(); i++)
			{
				MarsTechInfo info = _m_alTechList.get(i);
    			if(null == info)
    				continue;
				
				if(info.getGuildHelpId() <= 0)
					continue;
				
				GuildMarsHelpInfo help = _guild.getMarsHelpMgr().lookup(info.getGuildHelpId());
				if(null != help)
				{
					_list.add(help.toProto());
				}
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			sb.append("\ntechSize:").append(_m_alTechList.size());
			for(int i = 0; i < _m_alTechList.size(); i++)
			{
				MarsTechInfo info = _m_alTechList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n----------------------------");
				sb.append(info.toString());
			}
			
			return sb.toString();
		}
    	finally
    	{
    		getUserData().unlockUser();
    	}
	}
}
