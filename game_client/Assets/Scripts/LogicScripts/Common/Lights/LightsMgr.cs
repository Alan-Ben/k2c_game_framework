
using System;
using ALPackage;

namespace GOE
{
	/// <summary>
	/// 屏幕灯光效果的管理类，在需要使用的地方进行开启和关闭
	/// </summary>
	public class LightsMgr
	{
	    private static LightsMgr _g_instance = new LightsMgr();
	    public static LightsMgr instance
	    {
	        get
	        {
	            if (null == _g_instance)
	                _g_instance = new LightsMgr();
	            return _g_instance;
	        }
	    }

		//当前使用的主灯光配置索引，注意：主灯光配置只允许替换，不允许关闭
		private GLightGoIndex _m_iCurLightMainLightIndex;

		//当前灯管使用的附加配置索引，当附加配置关闭的时候会自动恢复到主灯光配置
		private GLightGoIndex _m_iCurAdditionLightIndex;
		//当前的附加灯光序列号，避免误删
		private long _m_lCurAdditionLightSerialize;

	    private LightsMgr()
	    {
			_m_iCurLightMainLightIndex = null;
			_m_iCurAdditionLightIndex = null;
			_m_lCurAdditionLightSerialize = 0;
        }

	    /// <summary>
	    /// 开启环境灯光配置
	    /// </summary>
	    public void openMainLight(GLightGoIndex _index, Action _doneDelegate)
	    {
		    //无效数据不处理
		    if (!_index.isValid())
		    {
			    _doneDelegate?.Invoke();
			    return;
		    }
		    
			_m_iCurLightMainLightIndex = _index;

			//当附加灯光配置有效的时候不做主灯光处理
			if(_m_iCurAdditionLightIndex != null)
			{
				_doneDelegate?.Invoke();
                return;
            }

			//调用内部管理器开启灯光
			LightsInnerController.instance.openLight(_index, _doneDelegate);
        }
	    
		/// <summary>
		/// 开启一个附加灯光配置
		/// </summary>
		/// <param name="_index"></param>
		public long openAdditionLight(GLightGoIndex _index, Action _doneDelegate)
		{
			//无效数据不处理
			if (!_index.isValid())
			{
				_doneDelegate?.Invoke();
				return 0;
			}
			
			_m_iCurAdditionLightIndex = _index;

			//调用内部管理器开启灯光
			LightsInnerController.instance.openLight(_m_iCurAdditionLightIndex, _doneDelegate);
			_m_lCurAdditionLightSerialize = ALSerializeOpMgr.next();

			return _m_lCurAdditionLightSerialize;
        }

		/// <summary>
		/// 关闭当前的附加灯光配置
		/// </summary>
		/// <param name="_index"></param>
		public void closeAdditionLight(long _lightSerialize)
		{
			if(_m_lCurAdditionLightSerialize != _lightSerialize || _lightSerialize == 0)
                return;

			//关闭附加灯光配置
			_m_iCurAdditionLightIndex = null;
			_m_lCurAdditionLightSerialize = 0;
			//开启主灯光配置
			LightsInnerController.instance.openLight(_m_iCurLightMainLightIndex);
        }
    }
}