package NPUSServer.Guild.MarsHelp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * 可以自动互助的玩家列表数据管理
 * @author mj
 *
 */
public class GuildMarsAutoHelpPlayerMgr 
{
	//公会数据对象
    private GuildInfo _m_guildInfo;
	//可以自动帮助的玩家数据列表
    private ArrayList<GuildMarsAutoHelpPlayer> _m_alAutoPlayerList;
    
    public GuildMarsAutoHelpPlayerMgr(GuildInfo _guild)
    {
    	_m_guildInfo = _guild;
    	
    	_m_alAutoPlayerList = new ArrayList<>();
    }
    
    private void _lock() {_m_guildInfo.getMarsHelpMgr()._lock();}
    private void _unlock() {_m_guildInfo.getMarsHelpMgr()._unlock();}
    
    /**
     * 排序，截至时间从大到小
     */
    private void _sort()
    {
		_m_alAutoPlayerList.sort(new Comparator<GuildMarsAutoHelpPlayer>() 
		{
			@Override
			public int compare(GuildMarsAutoHelpPlayer o1, GuildMarsAutoHelpPlayer o2) 
			{
				//-1表示无限期
				if(o1.getEndMs() == -1)
					return 1;
				else if(o2.getEndMs() == -1)
					return 1;
				
				return Long.compare(o2.getEndMs(), o1.getEndMs());
			}
		});
    }
    
    /**
     * 获取可以自动帮助的盟友数据列表（需要检查有效性）
     * 本方法只限内部使用
     * @return
     */
    protected ArrayList<GuildMarsAutoHelpPlayer> _getEffectAutoPlayerList() 
    {
    	checkExpired();
    	
		return _m_alAutoPlayerList;
	}
    
    /**
     * 检查过期时间
     */
    public void checkExpired()
    {
    	_lock();
    	
    	try
    	{
    		long nowMS = CommonFunc.getNowTimeMS();
    		
    		for(int i = _m_alAutoPlayerList.size() - 1; i >= 0; i--)
    		{
    			GuildMarsAutoHelpPlayer player = _m_alAutoPlayerList.get(i);
    			if(null == player)
    				continue;
    			
    			if(player.getEndMs() == -1 || player.getEndMs() > nowMS)
    				break;
    			
    			_m_alAutoPlayerList.remove(i);
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 查找数据
     * @param _cid
     * @return
     */
    public GuildMarsAutoHelpPlayer lookup(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAutoPlayerList.size(); i++)
    		{
    			GuildMarsAutoHelpPlayer player = _m_alAutoPlayerList.get(i);
    			if(null == player)
    				continue;
    			
    			if(player.getCid() == _cid)
    				return player;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造数据
     * @param _cid
     * @param _endMs
     * @return
     */
    public GuildMarsAutoHelpPlayer ensure(long _cid, long _endMs)
    {
    	_lock();
    	
    	try
    	{
    		//清空过期数据
    		checkExpired();
    		
    		//处理玩家数据
    		GuildMarsAutoHelpPlayer player = lookup(_cid);
    		if(null == player)
    		{
    			player = new GuildMarsAutoHelpPlayer(_cid, _endMs);
    			_m_alAutoPlayerList.add(player);
    			
    			//排序，截至时间从大到小
    			_sort();
    			
    			//对所有求助发起帮助
    			ALSynTaskManager.getInstance().regTask(()->
    			{
    				NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_MARS_HELP_AUTO);
    				int dealedCount = _m_guildInfo.getMarsHelpMgr().cmdDealHelp(_cid, true);
    				//数据计入离线数据，用于玩家获取互助奖励
    				if(dealedCount > 0)
    				{
    					//更新玩家记录
    					_m_guildInfo.getMarsHelpMgr().getAutoHelpDealedRecordMgr().incrCount(_cid, dealedCount);

						//发送rpc给予互助奖励
						_m_guildInfo.autoHelpDealReward(_cid, dealedCount);
    				}
    			});
    		}
    		else
    		{
    			player.setEndMs(_endMs);
    			
    			//排序，截至时间从大到小
    			_sort();
    		}
    		
    		return player;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 移除可以自动帮助的玩家
     * @param _cid
     */
    public void remove(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAutoPlayerList.size(); i++)
    		{
    			GuildMarsAutoHelpPlayer player = _m_alAutoPlayerList.get(i);
    			if(null == player)
    				continue;
    			
    			if(player.getCid() == _cid)
    			{
    				_m_alAutoPlayerList.remove(i);
    				break;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    @Override
    public String toString()
    {
    	_lock();
    	
    	try
    	{
    		StringBuilder sb = new StringBuilder();
    		sb.append("\nautoPlayerSize:").append(_m_alAutoPlayerList.size());
    		for(int i = 0; i < _m_alAutoPlayerList.size(); i++)
    		{
    			GuildMarsAutoHelpPlayer player = _m_alAutoPlayerList.get(i);
    			if(null == player)
    				continue;
    			
    			sb.append("\ncid:").append(player.getCid()).append(", end:").append(CommonFunc.getTimeStringMs(player.getEndMs()));
    		}
    		
    		return sb.toString();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
