package NPUSServer.Guild.MarsHelp;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildEnum.EGuildMarsHelpObjType;
import Common.GuildObj.Guild_MarsHelpShowInfo;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_041_001_AddMarsHelp;
import NPCommon.DB.BM.BM;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import USDB.Bo.GuildMarsHelpBO;

import java.util.ArrayList;

/**
 * 公会-火星互助管理对象
 * @author mj
 *
 */
public class GuildMarsHelpMgr 
{
	//公会数据对象
    private GuildInfo _m_guildInfo;
    
    //互助数据表
    private ArrayList<GuildMarsHelpInfo> _m_alMarsHepList;
    
    //自动互助玩家管理对象
    private GuildMarsAutoHelpPlayerMgr _m_mgrAutoHelpPlayerMgr;
    //自动互助每日帮助数据管理对象
    private GuildMarsAutoHelpDealedRecordMgr _m_mgrAutoHelpDealedRecordMgr;
    
    //互助数据容器锁
    private MutexAtom _m_helpMutex;

    public GuildMarsHelpMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        
        _m_alMarsHepList = new ArrayList<>();
        
        _m_mgrAutoHelpPlayerMgr = new GuildMarsAutoHelpPlayerMgr(_guildInfo);
        _m_mgrAutoHelpDealedRecordMgr = new GuildMarsAutoHelpDealedRecordMgr(_guildInfo);
        
        _m_helpMutex = new MutexAtom();
    }
    
    public GuildInfo getGuildInfo() {return _m_guildInfo;}
    public BM getBM() {return _m_guildInfo.getGuildMgr().getServer().getBM();}
    
    public GuildMarsAutoHelpPlayerMgr getAutoHelpPlayerMgr() {return _m_mgrAutoHelpPlayerMgr;}
    public GuildMarsAutoHelpDealedRecordMgr getAutoHelpDealedRecordMgr() {return _m_mgrAutoHelpDealedRecordMgr;}

    protected void _lock() {_m_helpMutex.lock();}
    protected void _unlock() {_m_helpMutex.unlock();}
    
    public void _initFromMarsHelpBo(GuildMarsHelpBO _bo)
    {
    	GuildMarsHelpInfo info = new GuildMarsHelpInfo(_m_guildInfo, _bo);
    	
    	//超过求助上限，该求助数据可以移除
    	if(info.isHelpLimit())
	    {
    		_bo.del(getBM());
    		return;
	    }
    	
    	_m_alMarsHepList.add(info);
    }
    
    /**
     * 只移除内存对象（内部使用）
     * @param _info
     */
    protected void _removeHelp(GuildMarsHelpInfo _info) 
    {
    	_m_alMarsHepList.remove(_info);
	}
    
    /**
     * 查找指定的求助数据
     * @param _id
     * @return
     */
    public GuildMarsHelpInfo lookup(long _id)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alMarsHepList.size(); i++)
    		{
    			GuildMarsHelpInfo info = _m_alMarsHepList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getId() == _id)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 移除指定求助数据
     * @param _id
     * @param _push
     */
    public void removeHelp(long _id, boolean _push)
    {
    	GuildMarsHelpInfo info = null;
    	
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alMarsHepList.size(); i++)
    		{
    			GuildMarsHelpInfo tmpInfo = _m_alMarsHepList.get(i);
    			if(null == tmpInfo)
    				continue;
    			
    			if(tmpInfo.getId() == _id)
    			{
    				_m_alMarsHepList.remove(i);
    				tmpInfo._del();
    				
    				 info = tmpInfo;
    				 
    				 break;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    	
    	//推送移除求助数据
    	if(_push && null != info)
    	{
    		pushMarsHelpDel(info);
    	}
    }
    
    /**
     * 推送新增求助数据
     * @param _info
     */
    public void pushMarsHelpAdd(GuildMarsHelpInfo _info)
    {
    	_m_guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_042_GuildRelatedOp.make_051_OnCanDealMarsHelpAdd(_info.getId()), 
				m -> (m.getCid() != _info.getSenderCid()));
    }
    
    /**
     * 推送移除求助数据
     * @param _excludeCid
     * @param _id
     */
    public void pushMarsHelpDel(GuildMarsHelpInfo _info)
    {
    	_m_guildInfo.getMemberMgr().broadcastMsg(US2GCWriter_042_GuildRelatedOp.make_052_OnCanDealMarsHelpDel(_info.getId()), 
				m -> (m.getCid() != _info.getSenderCid() && !_info._hasDealedPlayer(m.getCid())));
    }
    
    /**
     * 移除玩家数据，不做推送，客户端在移除联盟玩家时自行移除
     * @param _cid
     */
    public void removePlayerHelp(long _cid)
    {
    	_lock();
    	
    	try
    	{
    		//移除求助内存数据
    		for(int i = _m_alMarsHepList.size() - 1; i >= 0; i--)
    		{
    			GuildMarsHelpInfo info = _m_alMarsHepList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getSenderCid() == _cid)
    			{
    				_m_alMarsHepList.remove(i);
    				
    				//推送移除消息
    				ALSynTaskManager.getInstance().regTask(() -> 
    				{
    					pushMarsHelpDel(info);
    				});
    			}
    		}
    		//移除bo数据
    		getGuildInfo().getGuildMgr().getServer().getBM().getBM(GuildMarsHelpBO.class).delAll("sender_cid", _cid);
    		
    		//移除可以自动求助的玩家数据
    		_m_mgrAutoHelpPlayerMgr.remove(_cid);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 销毁数据
     */
    public void discard()
    {
    	_lock();
    	
    	try
    	{
    		_m_alMarsHepList.clear();
    		
    		getGuildInfo().getGuildMgr().getServer().getBM().getBM(GuildMarsHelpBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造玩家可以帮助的求助数据ID列表
     * @param _cid
     * @param _list
     */
    public void makePlayerCanDealMarsHelpIdProto(long _cid, ArrayList<Long> _list)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alMarsHepList.size(); i++)
    		{
    			GuildMarsHelpInfo info = _m_alMarsHepList.get(i);
    			if(null == info)
    				continue;
    			
    			//过滤求助数据：自身求助 + 已经求助
    			if(info.getSenderCid() == _cid || info.hasDealedPlayer(_cid))
    				continue;
    			
    			_list.add(info.getId());
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 构造玩家可以帮助的求助数据列表
     * @param _cid
     * @param _list
     */
    public void makePlayerCanDealMarsHelpProto(long _cid, ArrayList<Guild_MarsHelpShowInfo> _list)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alMarsHepList.size(); i++)
    		{
    			GuildMarsHelpInfo info = _m_alMarsHepList.get(i);
    			if(null == info)
    				continue;
    			
    			//过滤求助数据：自身求助 + 已经求助
    			if(info.getSenderCid() == _cid || info.hasDealedPlayer(_cid))
    				continue;
    			
    			_list.add(info.toShowProto());
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 增加帮助
     * @param _cid
     * @param _objType
     * @return
     */
    public GuildMarsHelpInfo cmdAddHelp(long _cid, EGuildMarsHelpObjType _objType, GuildOp_041_001_AddMarsHelp _addInfo)
    {
    	_lock();
    	
    	try
    	{
    		//增加数据
    		GuildMarsHelpBO bo = new GuildMarsHelpBO();
    		bo.setGuildId(getBM(), getGuildInfo().getGuildId());
    		bo.setSenderCid(getBM(), _cid);
    		bo.setObjType(getBM(), _objType.ordinal());
    		bo.setObjId(getBM(), _addInfo.getUsHelpDBId());
    		bo.setDealLimit(getBM(), _addInfo.getDealLimit());
    		bo.setDealSecs(getBM(), _addInfo.getDealSecs());
    		//额外数据
			bo.setExt(getBM(), _addInfo.getHelpExData());

    		bo.insert(getBM());
    		
    		GuildMarsHelpInfo info = new GuildMarsHelpInfo(_m_guildInfo, bo);
    		_m_alMarsHepList.add(info);
    		
    		return info;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 帮助所有可以处理的求助数据
     * @param _cid
     * @param _isAuto
     * @return
     */
    public int cmdDealHelp(long _cid, boolean _isAuto)
    {
    	_lock();
    	
    	try
    	{
    		int dealCount = 0;
    		for(int i = _m_alMarsHepList.size() - 1; i >= 0; i--)
    		{
    			GuildMarsHelpInfo info = _m_alMarsHepList.get(i);
    			if(null == info)
    				continue;
    			
    			//过滤求助数据：自身求助 + 已帮助
    			if(info.getSenderCid() == _cid || info.hasDealedPlayer(_cid))
    				continue;
    			
    			//增加处理玩家CID
    			if(_isAuto)
    			{
    				info._addAutoDealedCid(_cid);
    			}
    			else
    			{
    				info._addDealedCid(_cid);
    			}
    			dealCount++;
    			
    			//帮助达到上限，移除该帮助数据
    			if(info.isHelpLimit()) //已经达到上限，需要移除
    			{
    				_m_alMarsHepList.remove(i);
    				info._del();
    				
    				//推送所有玩家移除数据
    				ALSynTaskManager.getInstance().regTask(() -> 
    				{
    					pushMarsHelpDel(info);
    				});
    			}
    		}
    		
    		//返回处理数量
    		return dealCount;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
