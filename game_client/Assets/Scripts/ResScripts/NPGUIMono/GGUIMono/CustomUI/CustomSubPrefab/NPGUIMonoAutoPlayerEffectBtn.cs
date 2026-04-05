using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

using GOE;


namespace GOE
{
	/** 加载prefab信息对象 */
	[System.Serializable]
	public class NPGGUIAutoPlayerEffectInfo
	{
	    public string playerEffectStr;//效果字符串

	    public string conditionStr;//点击的判断条件字符串

	    //效果数据
	    private bool _m_bIsInited = false;
	    private List<NPPlayerEffectSerializeInfo> _m_lPlayerEffectList;
	    private NPPlayerConditionGroupObj _m_lCondition;
	    public List<NPPlayerEffectSerializeInfo> playerEffectList
	    {
	        get
	        {
	            if (_m_bIsInited)
	                return _m_lPlayerEffectList;

	            _init();

	            return _m_lPlayerEffectList;
	        }
	    }
	    public NPPlayerConditionGroupObj condition
	    {
	        get
	        {
	            if (_m_bIsInited)
	                return _m_lCondition;

	            _init();

	            return _m_lCondition;
	        }
	    }

	    //初始操作
	    protected void _init()
	    {
	        _m_lPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(playerEffectStr);
	        _m_lCondition = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "custB");
	        _m_bIsInited = true;
	    }
	}

    /// <summary>
    /// 自定义处理效果按钮脚本
    /// </summary>
	public class NPGUIMonoAutoPlayerEffectBtn : MonoBehaviour
    {
	    //自动执行的效果队列
	    public List<NPGGUIAutoPlayerEffectInfo> autoPlayerEffectList;
	    //最多执行几个
	    public int maxCount;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;


	    private void OnEnable()
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

#if NP_GAME
	        if(gameObject.activeInHierarchy)
	            //调用点击处理
	            NPGSubPrefabActionMgr.instance._dealAutoPlayerEffect(this);
#endif
	    }
	}

}