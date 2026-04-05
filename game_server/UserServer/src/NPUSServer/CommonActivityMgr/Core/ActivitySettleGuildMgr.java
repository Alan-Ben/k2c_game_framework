package NPUSServer.CommonActivityMgr.Core;

import Common.ServerObj.ServerObj_RankSettleGuild;
import Common.ServerObj.ServerObj_RankSettleGuildList;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import USDB.Bo.ActivityBaseBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class ActivitySettleGuildMgr 
{
    //所属活动对象
    private _AActivityBase _m_activity;
    
    //当前结算时联盟对应的盟主
    private HashMap<Long, Long> _m_hmGuildLeaderCidMap;
    //当前结算时所有联盟的盟主CID数据集合
    private HashSet<Long> _m_hsGuildLeaderCidSet;
    
    //活动数据BO
    private ActivityBaseBO _m_bo;
    
    public ActivitySettleGuildMgr(_AActivityBase _activity, ActivityBaseBO _bo)
    {
    	_m_activity = _activity;
    	_m_bo = _bo;
    	
    	_m_hmGuildLeaderCidMap = new HashMap<>();
    	_m_hsGuildLeaderCidSet = new HashSet<>();
    	
    	if(null != _m_bo.getSettledGuildLeadersSet())
    	{
    		ByteBuffer buff = ByteBuffer.wrap(_m_bo.getSettledGuildLeadersSet());
    		ServerObj_RankSettleGuildList objList = new ServerObj_RankSettleGuildList();
    		objList.readPackage(buff);
    		
    		for(int i = 0; i < objList.getList().size(); i++)
    		{
    			ServerObj_RankSettleGuild obj = objList.getList().get(i);
    			if(null == obj)
    				continue;
    			
    			_m_hmGuildLeaderCidMap.put(obj.getGuildId(), obj.getLeaderCid());
    			_m_hsGuildLeaderCidSet.add(obj.getLeaderCid());
    		}
    	}
    }
    
    private void _lock() {_m_activity._activityLock();}
    private void _unlock() {_m_activity._activityUnlock();}
    
    /**
     * 获取并记录此时所有联盟盟主CID集合
     */
    public void dumpAllLeaderCid(ArrayList<GuildInfo> _guildList)
    {
    	_lock();
    	
    	try
    	{
    		_m_hmGuildLeaderCidMap.clear();
    		_m_hsGuildLeaderCidSet.clear();
        	
    		ServerObj_RankSettleGuildList objList = new ServerObj_RankSettleGuildList();
        	if(null != _guildList && !_guildList.isEmpty())
        	{
        		for(int i = 0; i < _guildList.size(); i++)
        		{
        			GuildInfo guild = _guildList.get(i);
        			if(null == guild)
        				continue;
        			
        			ServerObj_RankSettleGuild obj = new ServerObj_RankSettleGuild();
        			obj.setGuildId(guild.getGuildId());
        			obj.setLeaderCid(guild.getLeaderId());
        			objList.addList(obj);
        			
        			_m_hmGuildLeaderCidMap.put(obj.getGuildId(), obj.getLeaderCid());
        			_m_hsGuildLeaderCidSet.add(obj.getLeaderCid());
        		}
        	}
        	
        	_m_bo.saveSettledGuildLeadersSet(_m_activity.getUSServer().getBM(), CommonFunc.ByteBfferToBytes(objList.makePackage()));
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 是否盟主检查
     * @param _cid
     * @return
     */
    public boolean isLeader(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		return _m_hsGuildLeaderCidSet.contains(_cid);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取指定联盟的盟主CID
     * @param _guildId
     * @return
     */
    public long getLeaderCid(long _guildId)
    {
    	_lock();
    	
    	try
    	{
    		Long value = _m_hmGuildLeaderCidMap.get(_guildId);
    		
    		return null == value ? 0 : value;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
