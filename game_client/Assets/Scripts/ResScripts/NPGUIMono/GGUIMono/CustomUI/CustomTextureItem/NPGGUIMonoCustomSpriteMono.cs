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
    /// 自定义Sprite脚本
    /// </summary>
	public class NPGGUIMonoCustomSpriteMono : MonoBehaviour
	{
	    //图片索引
	    public NPGSpriteIndex spriteIndex;
	    //加载对象
	    public Image spriteUIObj;

	    //是否需要检测
	    private bool _m_bNeedCheck = false;

#if NP_GAME
	    //加载控制对象
	    private GGuiWndSprite _m_wtSpriteWnd;
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
#if NP_GAME
	            if (null == _m_wtSpriteWnd && null != spriteUIObj)
	            {
	                _m_wtSpriteWnd = new GGuiWndSprite(spriteUIObj);
	                _m_wtSpriteWnd.setTexture(spriteIndex);
	            }
#endif
	        }
	        else
	        {
#if NP_GAME
	            if (null != _m_wtSpriteWnd)
	                _m_wtSpriteWnd.discard();
	            _m_wtSpriteWnd = null;
#endif
	        }
	    }
	}
}