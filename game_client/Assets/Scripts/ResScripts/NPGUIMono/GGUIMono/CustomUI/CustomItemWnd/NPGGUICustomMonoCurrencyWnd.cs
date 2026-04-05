using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

#if NP_GAME
using GOE;
#endif


namespace GOE
{
    /// <summary>
    /// 玩家自定义的货币信息显示窗口
    /// </summary>
    public class NPGGUICustomMonoCurrencyWnd : _AALBasicUIWndMono
	{
	    //货币类型
	    public CommonEnum.ECurrency currencyType;

	    //货币图标
	    public Image currencyIcon;
	    //货币数量
	    public Text currencyCount;
	    [ALHeader("是否显示大数字")]
	    public bool isNeedLargeString;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;
#if NP_GAME
	    //本窗口的管理类
	    private NPGGUIWndCustomCurrencyWnd _m_cwCurrencyWnd;
#endif

	    //有效和无效的时候分别注册和注销显示对象
	    private void OnEnable()
	    {
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDisable()
	    {
	        // _m_bNeedCheck = true;
	        //
	        // //到管理对象中进行处理
	        // ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDestroy()
	    {
	        _m_bNeedCheck = true;
	        //直接检测
	        _check();
	    }

	    protected void _check()
	    {
	        if(!_m_bNeedCheck || null == this || null == gameObject)
	        {
#if UNITY_EDITOR
	            if(_m_bNeedCheck)
	                UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
	            return;
	        }

	        _m_bNeedCheck = false;

	        if(gameObject.activeInHierarchy)
	        {
#if NP_GAME
	            //创建窗口管理对象并注册
	            if(null == _m_cwCurrencyWnd)
	                _m_cwCurrencyWnd = new NPGGUIWndCustomCurrencyWnd(this);

	            _m_cwCurrencyWnd.showWnd();
#endif
	        }
	        else
	        {
#if NP_GAME
	            //销毁管理对象
	            if(null != _m_cwCurrencyWnd)
	                _m_cwCurrencyWnd.discard();
	            _m_cwCurrencyWnd = null;
#endif
	        }
	    }
	}
}