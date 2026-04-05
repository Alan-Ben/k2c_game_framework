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
	/** 加载prefab信息对象 */
	[System.Serializable]
	public class NPGGUIRemoteEffectPrefabInfo : NPGGUISubPrefabInfo
	{
	    //对应远程效果Id
	    public long remoteEffectId;
	}

	/// <summary>
    /// 自定义远程效果预制体脚本
    /// </summary>
	public class NPGGUIMonoRemoteEffectPrefab : MonoBehaviour
    {
	    //加载信息对象队列
	    public List<NPGGUIRemoteEffectPrefabInfo> loadInfoList;
	    //最多加载几个
	    public int maxLoadCount;

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

#if NP_GAME
	        if(gameObject.activeInHierarchy)
	            //到管理对象中进行处理
	            NPGSubPrefabActionMgr.instance._onRemoteEffectPrefabActive(this);
	        else
	            //到管理对象中进行处理
	            NPGSubPrefabActionMgr.instance._onRemoteEffectPrefabDisactive(this);
#endif
	    }
	}
}