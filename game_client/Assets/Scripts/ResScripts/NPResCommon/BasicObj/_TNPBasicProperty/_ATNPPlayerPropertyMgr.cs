using System;
using System.Collections.Generic;
using System.Text;

namespace GOE
{
	/****************
	 * 总的属性管理对象
	 **/
	public abstract class _ATNPPlayerPropertyMgr<E, M, C>
	    where E : Enum
	    where M : _ATNPBasicPropertyModifier<E, M>
	    where C : _ATNPBasicPropertyContainer<E, M, C>
	{
	    /** 属性具体容器 */
	    private long[] _m_lPropertyValueList;

	    /** 属性变更记录对象 */
	    private NPBasicPropertyChgRecorder _m_crChgRecorder;

	    /** 本管理对象下属的容器对象 */
	    private List<C> _m_hsChildContainer;

	    /** 监听回调 */
	    private Action<E, long> _m_dPropertyChgDelegate;

        /** 全部属性变更完成回调 */
		private Action _m_aOnAllPropertyChgDone;

	    /** 枚举信息对象 */
	    protected _TBaseEnumObj<E> _m_eoEnumObj;

	    public _ATNPPlayerPropertyMgr()
	    {
	        _m_eoEnumObj = _TBaseEnumObj<E>.g_enumObj;

	        _m_lPropertyValueList = new long[_m_eoEnumObj.enumLength];
	        _m_hsChildContainer = new List<C>();

	        _m_crChgRecorder = new NPBasicPropertyChgRecorder(_m_eoEnumObj.enumLength);

	        //初始化所有属性
	        for (int i = 0; i < _m_eoEnumObj.enumLength; i++)
	        {
	            _m_lPropertyValueList[i] = 0;
	        }

	        _m_dPropertyChgDelegate = default(Action<E, long>);
            _m_aOnAllPropertyChgDone = default(Action);
	    }
	    public Action<E, long> propertyChgDelegate { get { return _m_dPropertyChgDelegate; } set { _m_dPropertyChgDelegate = value; } }
	    public Action onAllPropertyChgDone { get { return _m_aOnAllPropertyChgDone; } set { _m_aOnAllPropertyChgDone = value; } }

	    /*******************
	     * 注册子容器对象
	     * 
	     * @author alzq.z
	     * @time   May 9, 2013 12:04:55 AM
	     */
	    public void regPropertyContainer(C _container)
	    {
	        if (!_m_hsChildContainer.Contains(_container))
	        {
	            _m_hsChildContainer.Add(_container);

	            //设置修改对象
	            _container._setChgRecorder(_m_crChgRecorder);
	        }
	    }

	    /*******************
	     * 初始化所有属性计算
	     * 
	     * @author alzq.z
	     * @time   May 9, 2013 12:06:54 AM
	     */
	    public void initProperties()
	    {
	        for (int i = 0; i < _m_eoEnumObj.enumLength; i++)
	        {
	            long value = _calculationAllContainerProperty(i);

	            //设置属性值
	            _setValue(i, value);
	        }
	    }

	    /*******************
	     * 计算所有属性值
	     * 
	     * @author alzq.z
	     * @time   May 9, 2013 12:06:54 AM
	     */
	    public void calculateChgProperties()
	    {
	        //获取修改属性类型
	        int chgPropertyType = _m_crChgRecorder.popChgProperty();
			//是否有属性变更
            bool isPropertyChg = false;

	        while (chgPropertyType >= 0)
	        {
	            long preValue = getValue(chgPropertyType);

	            long newValue = _calculationAllContainerProperty(chgPropertyType);

	            if (preValue != newValue)
                {
					//属性有变更
                    isPropertyChg = true;
					//设置新的属性值
					_setValue(chgPropertyType, newValue);
	                //回调属性变化
	                onPropertyChg(chgPropertyType, newValue);
	            }

				//获取下一个修改属性类型
				chgPropertyType = _m_crChgRecorder.popChgProperty();
	        }

            if (isPropertyChg && _m_aOnAllPropertyChgDone != null)
                _m_aOnAllPropertyChgDone();
        }

	    /*************
	     * 获取对应的属性
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:37:11 AM
	     */
	    public long getValue(E _type)
	    {
	        return _m_lPropertyValueList[_m_eoEnumObj.toInt(_type)];
	    }
	    public long getValue(int _type)
	    {
	        return _m_lPropertyValueList[_type];
	    }

		/// <summary>
		/// 获取子容器列表详情字符串
		/// </summary>
		/// <returns></returns>
        public string getChildContainerDetailString()
        {
            if (_m_hsChildContainer == null)
                return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_hsChildContainer.Count; i++)
            {
				if(_m_hsChildContainer[i] == null)
					continue;

                sb.AppendLine(_m_hsChildContainer[i].getName());
                sb.Append(_m_hsChildContainer[i].ToString());
            }

            return sb.ToString();
        }

		/****************
	     * 计算所有容器对应的属性
	     * 
	     * @author alzq.z
	     * @time   May 9, 2013 12:09:37 AM
	     */
		protected long _calculationAllContainerProperty(int _type)
	    {
	        long value = 0;

	        for (int i = 0; i < _m_hsChildContainer.Count; i++)
	        {
	            value += _m_hsChildContainer[i].getValue(_type);
	        }

	        return value;
	    }

	    /***************
	     * 设置对应值
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:38:19 AM
	     */
	    protected void _setValue(E _type, long _value)
	    {
	        _m_lPropertyValueList[_m_eoEnumObj.toInt(_type)] = _value;
	    }
	    protected void _setValue(int _type, long _value)
	    {
	        _m_lPropertyValueList[_type] = _value;
	    }

	    /****************
	     * 当属性变更时调用的事件函数
	     * 
	     * @author alzq.z
	     * @time   May 9, 2013 12:32:01 AM
	     */
	    protected void onPropertyChg(int _type, long _newValue)
	    {
	        if (null != _m_dPropertyChgDelegate)
	            _m_dPropertyChgDelegate(_m_eoEnumObj.getEnum(_type), _newValue);
	    }
	    protected void onPropertyChg(E _type, long _newValue)
	    {
	        if (null != _m_dPropertyChgDelegate)
	            _m_dPropertyChgDelegate(_type, _newValue);
	    }

	    public override string ToString() {
	        List<string> strs = new List<string>();
	        for (int i = 0; i < _m_eoEnumObj.enumLength; i++) {
	            strs.Add((_m_eoEnumObj.getEnum(i)).ToString() + ":" + _m_lPropertyValueList[i].ToString());
	        }
	        return string.Join("\n", strs.ToArray());
	    }
	}
}