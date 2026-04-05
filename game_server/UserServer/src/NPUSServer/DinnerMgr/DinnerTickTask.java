package NPUSServer.DinnerMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;

/**
 * 宴会池的TICK
 * @author mj
 *
 */
public class DinnerTickTask implements _IALSynTask
{
	//宴会池
	private DinnerPool _m_dpDinnerPool;
	//上下文对象
	private NPPlayerContext _m_ctxContext;

	public DinnerTickTask(DinnerPool _dinnerPool, NPPlayerContext _context)
	{
		_m_dpDinnerPool = _dinnerPool;
		
		_m_ctxContext = _context;
	}

	@Override
	public void run() 
	{
		_m_dpDinnerPool.tick(CommonFunc.getNowTimeSec(), _m_ctxContext);
		
		ALSynTaskManager.getInstance().regTask(this, 1000);
	}
}
