package NPUSServer.GraveMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;

public class GraveCheckNewInfoEndTask implements _IALSynTask
{
	//归属US服务器对象
	private NPUserServer _m_usUSServer;
	
	public GraveCheckNewInfoEndTask(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;
	}

	@Override
	public void run() 
	{
		_m_usUSServer.getGraveMgr().getGraveNewMgr().checkAllNewInfoEnd();
		
		ALSynTaskManager.getInstance().regTask(this, 1000);
	}
}
