package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

/**
 * 火星居民任务
 * @author mj
 *
 */
public class MarsPeopleTickTask implements _IALSynTask
{
	private NPUserServer _m_usUserServer;
	private long _m_lCid;
	
	public MarsPeopleTickTask(NPUserServer _userServer, long _cid)
	{
		_m_usUserServer = _userServer;
		_m_lCid = _cid;
	}

	@Override
	public void run() 
	{
		NPUSUserData userData = _m_usUserServer.getUsUserMgr().lookupCacheUserData(_m_lCid);
		if(null == userData)
			return;
		
		//火星居民tick处理
		userData.getMarsPeopleComponent().dealTick(NPPlayerContext.createNew(ENPGameEvent.MARS_PEOPLE_TICK));
		
		//10分钟后再次检查
		ALSynTaskManager.getInstance().regTask(this, 600000);
	}
}
