using System;
using System.Collections.Generic;

using UnityEngine;


namespace GOE
{
	/// <summary>
	/// 基础的TD里点击反馈mono
	/// </summary>
	public abstract class _AMonoOutCombatClick : MonoBehaviour
	{
	    public void onClick()
	    {
	        //触发点击效果
	        _onClick();
	    }
	    //长按的处理
	    public void onHolding()
	    {
	        //触发效果
	        _onHolding();
	    }
	    
	    //按下按钮的时候的处理
	    public void onPress()
	    {
		    //按下按钮的时候的处理
		    _onPress();
	    }
	    
	    //弹起按钮的时候的处理
	    public void onUnPress()
	    {
		    //弹起按钮的时候的处理
		    _onUnPress();
	    }

	    //点击触发效果函数
	    protected abstract void _onClick();

	    //长按触发效果函数
	    protected abstract void _onHolding();

	    //按下按钮的时候的处理
	    protected abstract void _onPress();
	    
	    //弹起按钮的时候的处理
	    protected abstract void _onUnPress();
	}
}