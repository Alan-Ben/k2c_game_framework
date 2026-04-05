package NPUSServer.MatchAdultMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserServer;

public class CheckMatchAdultItemTask implements _IALSynTask
{
	private NPUserServer _m_usUServer;
	private NPPlayerContext _m_ctxContext;
	
	public CheckMatchAdultItemTask(NPUserServer _usServer, NPPlayerContext _context)
	{
		_m_usUServer = _usServer;
		_m_ctxContext = _context;
	}

	@Override
	public void run() 
	{
		//检查联姻池子嗣过期情况
		_m_usUServer.getMatchAdultPool().checkMatchItem(_m_ctxContext);
		
		//每秒检查
		ALSynTaskManager.getInstance().regTask(this, 1000);
	}
}
