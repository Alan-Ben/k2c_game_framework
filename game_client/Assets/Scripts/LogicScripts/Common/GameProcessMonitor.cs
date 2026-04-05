using UnityEngine;
using System;
using System.Text;

using ALPackage;


namespace GOE
{
	/// <summary>
	/// 监控并输出异常初始化过程的监控对象
	/// </summary>
	public class GameProcessMonitor : _IALProcessMonitor
	{
	    //对应3个事件的处理函数
	    private Action<_AALProcess> _m_dOnFailStop;
	    private Action _m_dOnStop;
	    private Action _m_dOnSuc;

	    public GameProcessMonitor(Action<_AALProcess> _onFailStop, Action _onStop, Action _onSuc)
	    {
	        _m_dOnFailStop = _onFailStop;
	        _m_dOnStop = _onStop;
	        _m_dOnSuc = _onSuc;
	    }
	    public GameProcessMonitor(Action _onSuc)
	    {
	        _m_dOnFailStop = null;
	        _m_dOnStop = null;
	        _m_dOnSuc = _onSuc;
	    }

	    public long monitorTimeMS(string _processTag)
	    {
	        return 0;
	    }

	    public void onErr(long _processTimeMS, string _processTag, string _exInfo, Exception _ex)
	    {
	        ALLog.Error($"[{_processTag}] onErr");
	        ALLog.Error(_ex.ToString());
	    }

	    public void onTimeout(long _processTimeMS, string _processTag, string _exInfo)
	    {
	        ALLog.Error($"[{_processTag}] onTimeout - [{_processTimeMS}]ms");
	    }

	    public void onTimeoutDone(long _processTimeMS, string _processTag, string _exInfo)
	    {
	        ALLog.Error($"[{_processTag}] onTimeoutDone - [{_processTimeMS}]ms");
	    }
	    /// <summary>
	    /// 在某个过程因为失败卡住的时候调用的事件函数
	    /// </summary>
	    /// <param name="_process"></param>
	    public void onProcessFailStop(_AALProcess _process)
	    {
	        if (null != _m_dOnFailStop)
	            _m_dOnFailStop(_process);
	    }
	    /// <summary>
	    /// 在根节点的过程被终止时触发的函数
	    /// </summary>
	    public void onRootProecssStop()
	    {
	        if (null != _m_dOnStop)
	            _m_dOnStop();
	    }
	    /// <summary>
	    /// 在根节点的过程完成时执行的处理，此函数必定触发
	    /// </summary>
	    public void onRootProecssDone()
	    {
	        if (null != _m_dOnSuc)
	            _m_dOnSuc();
	    }
	}
}