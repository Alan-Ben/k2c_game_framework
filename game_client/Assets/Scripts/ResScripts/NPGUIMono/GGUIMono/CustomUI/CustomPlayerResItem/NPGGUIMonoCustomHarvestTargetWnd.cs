using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;

using UnityEngine.UI;
using System;
using NPEnum;
#if NP_GAME
using GOE;
#endif


namespace GOE
{
    /// <summary>
    /// 资源收集附加窗口的脚本对象
    /// </summary>
    public class NPGGUIMonoCustomHarvestTargetWnd : MonoBehaviour
    {
        [ALHeader("监听相关收集事件的类型，None为不监听")]
        public EHarvestType harvestResType;
        [ALHeader("资源收集的目标对象")]
        public RectTransform harvestTargetUIObj;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;

#if NP_GAME
        private NPGGUISubEmptyHarvestWnd _m_hwHarvestWnd = null;
#endif

        private void Start()
	    {
	    }
    
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
	        if(!_m_bNeedCheck || null == this || null == gameObject)
	        {
#if UNITY_EDITOR
	            if(_m_bNeedCheck)
	                UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
	            return;
	        }

	        _m_bNeedCheck = false;

#if NP_GAME
	        if(gameObject.activeInHierarchy)
	        {
                //如果已有窗口则注销
                if(null != _m_hwHarvestWnd)
                {
                    _m_hwHarvestWnd.discard();
                    _m_hwHarvestWnd = null;
                }

                //创建窗口对象
                _m_hwHarvestWnd = new NPGGUISubEmptyHarvestWnd(harvestResType, harvestTargetUIObj);
                //初始化窗口
                _m_hwHarvestWnd.init();
	        }
	        else
            {
                //如果已有窗口则注销
                if (null != _m_hwHarvestWnd)
                {
                    _m_hwHarvestWnd.discard();
                    _m_hwHarvestWnd = null;
                }
            }
#endif
	    }
	}
}