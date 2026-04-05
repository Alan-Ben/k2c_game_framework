using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GOE
{
	/// <summary>
	/// 自定义UI的基类窗口，汇总了检测机制，重载后实现_onCustomUIEnable和_onCustomUIDisable 保证显示和隐藏的正常处理及资源释放即可
	/// </summary>
	public abstract class _ANPGGUIMonoCustomBasicWnd : _AALBasicUIWndMono
	{
		//当前逻辑上是否显隐，保证显隐函数是相对的
		private bool _m_curActiveInHierarchy;
	    //是否需要检测
	    protected bool _m_bNeedCheck = false;

	    //有效和无效的时候分别注册和注销显示对象
	    protected void OnEnable()
	    {
	        //判断是否已经准备检测，避免当帧过多检测
	        if(_m_bNeedCheck)
	            return;
        
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    protected void OnDisable()
	    {
	        //判断是否已经准备检测，避免当帧过多检测
	        if(_m_bNeedCheck)
	            return;
        
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    protected void OnDestroy()
	    {
	        _m_bNeedCheck = true;
	        //直接检测
	        _check();
	    }

	    protected void _check()
	    {
	        if (!_m_bNeedCheck || null == this || null == gameObject)
	        {
#if UNITY_EDITOR
	            if (_m_bNeedCheck)
	                UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
	            return;
	        }

	        _m_bNeedCheck = false;

	        //当前需要显示，之前不显示，执行显示函数
	        if (gameObject.activeInHierarchy && !_m_curActiveInHierarchy)
	        {
		        _m_curActiveInHierarchy = true;
	            _onCustomUIEnable();
	        }
	        //当前不需要显示，之前是显示的，才执行不显示函数
	        else if(!gameObject.activeInHierarchy && _m_curActiveInHierarchy)
	        {
		        _m_curActiveInHierarchy = false;
	            _onCustomUIDisable();
	        }
	    }

	    /// <summary>
	    /// 在窗口显示的时候调用的函数
	    /// </summary>
	    protected abstract void _onCustomUIEnable();

	    /// <summary>
	    /// 在窗口无效的时候调用的函数
	    /// </summary>
	    protected abstract void _onCustomUIDisable();
	}
}