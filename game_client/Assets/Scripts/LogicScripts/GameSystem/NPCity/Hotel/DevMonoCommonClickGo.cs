using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
	/// <summary>
	/// 常规接受点击的模型上挂的脚本，客户端脚本调用
	/// </summary>
	public class DevMonoCommonClickGo : _AMonoOutCombatClick
	{
	    //模型点击选中或者取消选中的回调
	    private Action _m_onSelected;
	    private Action _m_onDisSelected;
	    private bool _m_isAutoDisSelected = false;//是否自动取消选中

	    /// <summary>
	    /// 是否自动取消选中,是的话，调用setSelected后会自动调disSelected
	    /// </summary>
	    public bool isAutoDisSelected { get { return _m_isAutoDisSelected; } }


	    public void setIsAutoDisSelected(bool _isAutoDisSelected)
	    {
	        _m_isAutoDisSelected = _isAutoDisSelected;
	    }

	    /// <summary>
	    /// 设置选择回调
	    /// </summary>
	    /// <param name="_onSelected"></param>
	    public void setSelectedAction(Action _onSelected, Action _onDisSelected)
	    {
	        _m_onSelected = _onSelected;
	        _m_onDisSelected = _onDisSelected;
	    }
    
	    /// <summary>
	    /// 取消选中
	    /// </summary>
	    public void disSelected()
	    {
	        if (null != _m_onDisSelected)
	        {
	            _m_onDisSelected();
	        }
	    }

	    /// <summary>
	    /// 选中操作
	    /// </summary>
	    public void onSelected()
	    {
	        if (null != _m_onSelected)
	        {
	            _m_onSelected();
	        }
	    }

	    protected override void _onClick()
	    {
	        CommonOpMgr.instance.setSelectedGo(this);
	    }

	    protected override void _onHolding()
	    {
        
	    }

	    protected override void _onPress()
	    {
	    }

	    protected override void _onUnPress()
	    {
	    }
	}
}