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
    /// 卡牌大图对象加载
    /// </summary>
    public class NPGGUIMonoCustomVersionUpReward : _AALBasicUIWndMono
	{
	    public NPGGUIMonoCommonItemContainer commonItemGrid;
	    public List<GameObject> notGetRewardShowList;
	    public List<GameObject> aleardyGetRewardShowList;

	    public GameObject getRewardBtn;

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
	            return;

	        _m_bNeedCheck = false;

#if NP_GAME
	        if(gameObject.activeInHierarchy)
	            //到管理对象中进行处理
	            NPGGUIVersionUpRewardWndMgr.instance.DealWndActivated(this);
	        else
	            //到管理对象中进行处理
	            NPGGUIVersionUpRewardWndMgr.instance.DealWndInactivated(this);
#endif
	    }
	}
}