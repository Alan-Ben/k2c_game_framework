package NPUSServer.Guild.MarsHelp;

import Common.ServerObj.ServerObj_GuildAutoDealedInfo;
import Common.ServerObj.ServerObj_GuildAutoDealedList;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * 每日帮助记录数据管理
 * @author mj
 *
 */
public class GuildMarsAutoHelpDealedRecordMgr 
{
	//公会数据对象
    private GuildInfo _m_guildInfo;
	//每日帮助记录数据列表
    private ArrayList<GuildMarsAutoHelpDealedRecordInfo> _m_alAutoDealedList;
    //保存数据LazyDealer，短时间内会有大量数据变动，延迟500毫秒执行
    private LazyTaskDealer _m_saveRecordDealer;
    
    public GuildMarsAutoHelpDealedRecordMgr(GuildInfo _guild)
    {
    	_m_guildInfo = _guild;

    	_m_alAutoDealedList = new ArrayList<>();
    	
    	_m_saveRecordDealer = new LazyTaskDealer(() -> _save(), 500);
    	
    	//加载数据
    	if(null != _m_guildInfo.getBo().getMarsHelpDailyRecord())
    	{
    		ServerObj_GuildAutoDealedList listObj = new ServerObj_GuildAutoDealedList();
    		ByteBuffer buff = ByteBuffer.wrap(_m_guildInfo.getBo().getMarsHelpDailyRecord());
    		listObj.readPackage(buff);
    		
    		for(int i = 0; i < listObj.getList().size(); i++)
    		{
    			ServerObj_GuildAutoDealedInfo obj = listObj.getList().get(i);
    			if(null == obj)
    				continue;
    			
    			GuildMarsAutoHelpDealedRecordInfo info = new GuildMarsAutoHelpDealedRecordInfo(
    					obj.getCid(), obj.getLastDate(), obj.getCount());
    			_m_alAutoDealedList.add(info);
    		}
    		//更新数据，移除重置数据
    		doLazySave();
    	}
    }

    private void _lock() {_m_guildInfo.getMarsHelpMgr()._lock();}
    private void _unlock() {_m_guildInfo.getMarsHelpMgr()._unlock();}
    
    public void doLazySave() {_m_saveRecordDealer.setNeedDeal();}
    
    private void _save()
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alAutoDealedList.isEmpty())
    			return;
    		
    		ServerObj_GuildAutoDealedList listObj = null;
    		for(int i = 0; i < _m_alAutoDealedList.size(); i++)
    		{
    			GuildMarsAutoHelpDealedRecordInfo info = _m_alAutoDealedList.get(i);
    			if(null == info)
    				continue;
    			
    			//已经重置的数据无需再次保存
    			if(info.getCount() <= 0)
    				continue;
    			
    			if(null == listObj)
    			{
    				listObj = new ServerObj_GuildAutoDealedList();
    			}
    			
    			ServerObj_GuildAutoDealedInfo obj = new ServerObj_GuildAutoDealedInfo();
    			obj.setCid(info.getCid());
    			obj.setLastDate(info.getLastDate());
    			obj.setCount(info.getCount());
    			
    			listObj.addList(obj);
    		}
    		
    		if(null != listObj)
    		{
    			_m_guildInfo.getBo().saveMarsHelpDailyRecord(_m_guildInfo.getGuildMgr().getServer().getBM(), 
    					CommonFunc.ByteBfferToBytes(listObj.makePackage()));
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    public GuildMarsAutoHelpDealedRecordInfo lookup(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alAutoDealedList.size(); i++)
    		{
    			GuildMarsAutoHelpDealedRecordInfo info = _m_alAutoDealedList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getCid() == _cid)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    public int getCount(long _cid)
    {
    	GuildMarsAutoHelpDealedRecordInfo info = lookup(_cid);
    	return null == info ? 0 : info.getCount();
    }
    
    public GuildMarsAutoHelpDealedRecordInfo ensure(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		GuildMarsAutoHelpDealedRecordInfo info = lookup(_cid);
    		if(null == info)
    		{
    			info = new GuildMarsAutoHelpDealedRecordInfo(_cid);
    			_m_alAutoDealedList.add(info);
    		}
    		
    		return info;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 设置数量
     * @param _cid
     * @param _count
     */
    public void setCount(long _cid, int _count)
    {
    	_lock();
    	
    	try
    	{
    		//更新玩家数据
    		GuildMarsAutoHelpDealedRecordInfo info = ensure(_cid);
    		info._refresh();
    		info._setCount(_count);
    		
    		//保存数据
    		doLazySave();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 增加数量
     * @param _cid
     * @param _count
     */
    public void incrCount(long _cid, int _count)
    {
    	_lock();
    	
    	try
    	{
    		//更新玩家数据
    		GuildMarsAutoHelpDealedRecordInfo info = ensure(_cid);
    		info._refresh();
    		info._setCount(info.getCount() + _count);
    		
    		//保存数据
    		doLazySave();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 移除玩家
     * @param _cid
     */
    public void remove(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		boolean needSave = false;
    		for(int i = 0; i < _m_alAutoDealedList.size(); i++)
    		{
    			GuildMarsAutoHelpDealedRecordInfo info = _m_alAutoDealedList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getCid() == _cid)
    			{
    				_m_alAutoDealedList.remove(i);
    				needSave = true;
    				break;
    			}
    		}
    		
    		if(needSave)
    		{
        		//保存数据
        		doLazySave();
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
