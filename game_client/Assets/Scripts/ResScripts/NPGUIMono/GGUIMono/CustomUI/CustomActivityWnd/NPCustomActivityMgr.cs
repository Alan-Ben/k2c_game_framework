using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;


namespace GOE
{
	public class NPCustomActivityMgr : WCGSingleton<NPCustomActivityMgr>
	{
	    private Action<NPGGUIMonoActivityItemWnd> wndActivatedEvent; // 按钮显示事件
	    private Action<NPGGUIMonoActivityItemWnd> wndDisactivatedEvent; // 按钮隐藏事件


	    // 触发激活事件
	    public void OnWndActivated(NPGGUIMonoActivityItemWnd matchItem)
	    {
	        if (null == matchItem)
	            return;

	        if(wndActivatedEvent != null)
	        {
	            wndActivatedEvent(matchItem);
	        }
	    }

	    // 触发反激活事件
	    public void OnWndInactivated(NPGGUIMonoActivityItemWnd matchItem)
	    {
	        if (null == matchItem)
	            return;

	        if (wndDisactivatedEvent != null)
	        {
	            wndDisactivatedEvent(matchItem);
	        }
	    }

	    // 注册按钮激活事件
	    public void SetWndActivatedEvent(Action<NPGGUIMonoActivityItemWnd> ac)
	    {
	        if (ac == null)
	        {
	            return;
	        }

	        wndActivatedEvent = ac;
	    }

	    // 注册按钮隐藏事件
	    public void SetWndInactivatedEvent(Action<NPGGUIMonoActivityItemWnd> ac)
	    {
	        if(ac == null)
	        {
	            return;
	        }

	        wndDisactivatedEvent = ac;
	    }
	}
}