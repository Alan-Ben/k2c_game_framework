using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


namespace GOE
{
    /// <summary>
    /// 自定义的活动界面列表
    /// </summary>
    public class NPGGUIMonoCustomActivity : _AALBasicUIWndMono
	{
	    //活动对象的队列
	    public List<NPGGUIMonoActivityItemWnd> activityItemList;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;

	    //有效和无效的时候分别注册和注销显示对象
	    private void OnEnable()
	    {
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDisable()
	    {
	        _m_bNeedCheck = true;

	        //到管理对象中进行处理
	        ALCommonActionMonoTask.addNextFrameTask(_check);
	    }

	    private void OnDestroy()
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

	        if (gameObject.activeInHierarchy)
	        {
	            //注册到管理对象
	            for (int i = 0; i < activityItemList.Count; i++)
	            {
	                NPCustomActivityMgr.instance.OnWndActivated(activityItemList[i]);
	            }
	        }
	        else
	        {
	            //从管理对象注销并销毁相关显示信息
	            for (int i = 0; i < activityItemList.Count; i++)
	            {
	                NPCustomActivityMgr.instance.OnWndInactivated(activityItemList[i]);
	            }
	        }
	    }
	}
}