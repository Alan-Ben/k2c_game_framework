using System;
using System.Collections.Generic;

using UnityEngine;


namespace GOE
{
	public abstract class _AMonoMobClickGo : _AMonoOutCombatClick
	{
	    //点击触发效果函数
	    protected override void _onClick()
	    {
#if NP_GAME
	        //点击主城对象，此时需要使用本对象展示菜单等信息
	        //NPCityOpMgr.instance.setSelectGo(this);
#endif

	        //处理点击操作
	       // _dealCityGoClickFunc();
	    }

	    //当取消选中时的处理
	    public abstract void onDisSelect(_AMonoMobClickGo _nextSelect);
	    //当选中时的处理
	    public abstract void onSelect();

	    //处理不同对象的点击效果
	    protected abstract void _dealMobGoClickFunc();
	}
}