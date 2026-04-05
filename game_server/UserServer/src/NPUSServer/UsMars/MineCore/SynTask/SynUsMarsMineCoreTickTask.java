package NPUSServer.UsMars.MineCore.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPUSServer.UsMars.MineCore.UsMarsMineCore;

/**
 * 矿场部分的tick执行任务
 */
public class SynUsMarsMineCoreTickTask implements _IALSynTask
{
	private UsMarsMineCore _m_mpMineCore;
	//本Tick的序列号，避免意外情况重开后，多任务执行的问题
	private long _m_lTickSerialize;
	
	public SynUsMarsMineCoreTickTask(UsMarsMineCore _core, long _tickSerialize)
	{
		_m_mpMineCore = _core;
		_m_lTickSerialize = _tickSerialize;
	}

	@Override
	public void run() 
	{
		//执行Tick处理
		try {
			_m_mpMineCore.tick(_m_lTickSerialize, CommonFunc.getNowTimeMS());
		}
		catch (Exception _ex)
		{
			ALServerLog.Error("Mars Mine Core tick get error: " + _ex.getMessage());
		}

		//延时注册
		ALSynTaskManager.getInstance().regTask(this, 1000);
	}
}
