using UnityEngine;
using System;
using System.Text;

using ALPackage;


namespace GOE
{
	/// <summary>
	/// 监控并输出异常初始化过程的监控对象
	/// </summary>
	public class GameInitMonitor : _IALProcessMonitor
	{
	    private string _m_sMonitorTag;
	    private long _m_lMonitorMaxTimeMS;

	    public GameInitMonitor(string _monitorTag, long _monitorMaxTimeMS)
	    {
	        _m_sMonitorTag = _monitorTag;
	        _m_lMonitorMaxTimeMS = _monitorMaxTimeMS;
	    }

	    public long monitorTimeMS(string _processTag)
	    {
	        //不对主要进程进行监控
	        if (_processTag.Equals("main", StringComparison.OrdinalIgnoreCase))
	            return 0;

	        //其他过程超过300毫秒需要监控
	        return _m_lMonitorMaxTimeMS;
	    }

	    public void onErr(long _processTimeMS, string _processTag, string _exInfo, Exception _ex)
	    {
	        ALLog.Error($"[{_m_sMonitorTag}] - [{_processTag}] onErr");
	        ALLog.Error(_ex.ToString());
	    }

	    public void onTimeout(long _processTimeMS, string _processTag, string _exInfo)
	    {
	        ALLog.Error($"[{_m_sMonitorTag}] - [{_processTag}] onTimeout - [{_processTimeMS}]ms");
	    }

	    public void onTimeoutDone(long _processTimeMS, string _processTag, string _exInfo)
	    {
	        ALLog.Error($"[{_m_sMonitorTag}] - [{_processTag}] onTimeoutDone - [{_processTimeMS}]ms");
	    }

	    /// <summary>
	    /// 在某个过程因为失败卡住的时候调用的事件函数
	    /// </summary>
	    /// <param name="_process"></param>
	    public void onProcessFailStop(_AALProcess _process)
	    {
		    string failTag = String.Empty;
		    if (_process != null)
		    {
			    failTag = _process.processTag;
		    }

		    //发送埋点-登入初始化过程
		    GCommon.sendStepReport(TraceConst.INIT_LOGIN_FAIL.setMarkParam(_m_sMonitorTag, failTag));
		    
		    //提示重登
		    NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_login_fail_str, failTag),//是否返回登入界面切换服务器？
			    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
			    () =>
			    {
				    Application.Quit();
			    },
			    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
			    () =>
			    {
				    //这里直接重试登录
				    Game.instance.reloginByDefault();
			    });
	    }
	    /// <summary>
	    /// 在根节点的过程被终止时触发的函数
	    /// </summary>
	    public void onRootProecssStop()
	    {

	    }
	    /// <summary>
	    /// 在根节点的过程完成时执行的处理，此函数必定触发
	    /// </summary>
	    public void onRootProecssDone()
	    {

	    }
	}
}