package NPUSServer.NPUSUserMgr.UserComp.MarsComp;

import Common.GuildEnum.EGuildMarsHelpObjType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

import java.util.ArrayList;

public class GuildMarsHelpMgr 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//求助目标对象列表
	private ArrayList<_IGuildMarsHelp> _m_alGuildMarsHelpList;
	
	public GuildMarsHelpMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alGuildMarsHelpList = new ArrayList<>();
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}

    private void _lock() {getUserData().lockUser();}
    private void _unlock() {getUserData().unlockUser();}
    
    /**
     * 注册目标数据
     * @param _obj
     */
    public void regMarsHelpObj(_IGuildMarsHelp _obj)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alGuildMarsHelpList.contains(_obj))
    			return;
    		
    		_m_alGuildMarsHelpList.add(_obj);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 注销求助目标数据
     * @param _obj
     */
    public void unregMarsHelpObj(_IGuildMarsHelp _obj)
    {
    	_lock();
    	
    	try
    	{
    		_m_alGuildMarsHelpList.remove(_obj);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
	
	/**
	 * 获取对应帮助对象数据
	 * @param _objType
	 * @param _objId
	 * @return
	 */
	public _IGuildMarsHelp lookupGuildHelp(EGuildMarsHelpObjType _objType, long _objId)
	{
		getUserData().lockUser();
		
		try
		{
    		for(int i = 0; i < _m_alGuildMarsHelpList.size(); i++)
    		{
    			_IGuildMarsHelp tmpObj = _m_alGuildMarsHelpList.get(i);
    			if(null == tmpObj)
    				continue;
    			
    			if(tmpObj.getObjType() == _objType && tmpObj.getObjId() == _objId)
    				return tmpObj;
    		}
    		
    		return null;
    	}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取玩家发起请求的数据队列
	 * @param _reclist
	 */
	public void getHelpIdList(ArrayList<Long> _reclist)
	{
		if(null == _reclist)
			return ;

		getUserData().lockUser();
		
		try
		{
    		for(int i = 0; i < _m_alGuildMarsHelpList.size(); i++)
    		{
    			_IGuildMarsHelp tmpObj = _m_alGuildMarsHelpList.get(i);
    			if(null == tmpObj)
    				continue;
    			
    			long helpId = tmpObj.getHelpId();
    			if(helpId <= 0)
    				continue;

				_reclist.add(helpId);
    		}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 发送求助数据
	 * @param _objType
	 * @param _objId
	 * @param _helpId
	 * @return
	 */
	public boolean sendGuildHelp(EGuildMarsHelpObjType _objType, long _objId, long _helpId)
	{
		getUserData().lockUser();
		
		try
		{
			_IGuildMarsHelp obj = lookupGuildHelp(_objType, _objId);
			if(null == obj)
				return false;
			
			if(!obj.canSendHelp(_objType))
				return false;

	        //更新helpObj的关联数据
			obj.setSendHelp(_helpId);
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_042_GuildRelatedOp.make_050_OnMyMarsHelpChg(obj));
			
			return true;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 被帮助成功的数据处理
	 * @param _objType
	 * @param _objId
	 * @param _helpId
	 * @param _helpSecs
	 * @return
	 */
	public _IGuildMarsHelp beDealedGuildHelpSuc(EGuildMarsHelpObjType _objType, long _objId, long _helpId, int _helpSecs)
	{
		getUserData().lockUser();
		
		try
		{
			_IGuildMarsHelp obj = lookupGuildHelp(_objType, _objId);
			if(null == obj)
				return null;
			
			//更新互助时长
			obj.updateHelpSecs(_helpId, _helpSecs);
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_042_GuildRelatedOp.make_050_OnMyMarsHelpChg(obj));
			
			return obj;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 重置帮助数据并移除联盟里相关的求助数据
	 * @param _obj
	 */
	public void unsetHelp(_IGuildMarsHelp _obj)
	{
		getUserData().lockUser();
		
		try
		{
			long helpId = _obj.getHelpId();
			if(helpId <= 0)
				return;
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_042_GuildRelatedOp.make_054_OnMyMarsHelpDel(helpId));
			
			//移除联盟相关数据
			_m_udUserData.getGuildComponent().sendRmvMarsHelpRPC(helpId, true);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
