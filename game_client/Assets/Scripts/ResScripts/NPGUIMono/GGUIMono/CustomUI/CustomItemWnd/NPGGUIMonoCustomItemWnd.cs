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
    /// 自定义物品附加窗口的脚本对象
    /// </summary>
    public class NPGGUIMonoCustomItemWnd : _AALBasicUIWndMono
	{
		[ALHeader("显示的物品配置")]
	    public NPCommonItem item;//物品
	    [ALHeader("数量，若>=0 直接显示，<0取玩家身上的数量，每次窗口显隐会刷新数量,")]
	    public long count;//数量，若<0取玩家身上的数量, >=0 直接显示
	    [ALHeader("数量对应的翻译key，如不填写则使用默认数字")]
	    public string transKey = string.Empty;//数量对应的翻译key，如不填写则使用默认数字
	    //展示奖励的脚本对象
	    public NPGGUIMonoCommonItem itemMono;//这种动态加载
	    //是否需要检测
	    private bool _m_bNeedCheck = false;
		//数量显示
	    private long _m_showCount = 0;
	    
#if NP_GAME
		//当前显示的物品数据
	    private CommonItemData _m_itemData;
	    
	    //加载控制对象
	    private NPGGUIWndPrefabItemWnd _m_wItemWnd;
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
	            //若<0取玩家身上的数量, >=0 直接显示
	            if(count < 0)
	            {
		            _m_showCount = GCommon.getItemCount(item);
	            }
	            else
	            {
		            _m_showCount = count;
	            }

	            if(null == _m_wItemWnd && null != itemMono)
	            {
		            _m_itemData = new CommonItemData(item, _m_showCount);
	                _m_wItemWnd = new NPGGUIWndPrefabItemWnd(itemMono);
	            }

	            if (null != _m_wItemWnd && null != _m_itemData)
	            {
		            _m_itemData.setCount(_m_showCount);
		            _m_wItemWnd.showWnd(_m_itemData);
	            }
#endif
	        }
	        else
	        {
#if NP_GAME
	            if(null != _m_wItemWnd)
	                _m_wItemWnd.discard();
	            _m_wItemWnd = null;
#endif
	        }
	    }
	}
}