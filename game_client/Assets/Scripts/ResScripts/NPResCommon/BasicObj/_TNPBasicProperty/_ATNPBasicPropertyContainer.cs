using System;
using System.Text;
using ALPackage;


namespace GOE
{
	/************************
	 * 属性容器对象
	 **/
	public abstract class _ATNPBasicPropertyContainer<E, M, C>
	    where E : Enum
	    where M : _ATNPBasicPropertyModifier<E, M>
	    where C : _ATNPBasicPropertyContainer<E, M, C>
	{
	    /** 存储所有属性值的队列 */
	    private long[] _m_lPropertyValueList;

	    /** 修改记录对象 */
	    private NPBasicPropertyChgRecorder _m_crChgRecorder;

	    /** 枚举信息对象 */
	    protected _TBaseEnumObj<E> _m_eoEnumObj;

	    public _ATNPBasicPropertyContainer()
	    {
	        //获取枚举信息
	        _m_eoEnumObj = _TBaseEnumObj<E>.g_enumObj;
        
	        _m_lPropertyValueList = new long[_m_eoEnumObj.enumLength];

	        //给属性队列添加对应默认值
	        for (int i = 0; i < _m_eoEnumObj.enumLength; i++)
	        {
	            _m_lPropertyValueList[i] = 0;
	        }

	        _m_crChgRecorder = null;
	    }

	    protected internal void _setChgRecorder(NPBasicPropertyChgRecorder _recorder) { _m_crChgRecorder = _recorder; }

	    /*************
	     * 获取对应属性值
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:37:11 AM
	     */
	    public long getValue(int _type)
	    {
	        return _m_lPropertyValueList[_type];
	    }
	    public long getValue(E _type)
	    {
	        return _m_lPropertyValueList[_m_eoEnumObj.toInt(_type)];
	    }

	    /***************
	     * 修改对应属性的值
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:40:05 AM
	     */
	    public void setValue(int _type, long _value)
	    {
#if UNITY_EDITOR
	        if(_type < 0 || _type >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_type}");
	            return;
	        }
#endif

	        _m_lPropertyValueList[_type] = _value;

	        if (null != _m_crChgRecorder)
	            _m_crChgRecorder.addPropertyChg(_type);
	    }
	    public void setValue(E _type, long _value)
	    {
	        int v = _m_eoEnumObj.toInt(_type);
#if UNITY_EDITOR
	        if (v < 0 || v >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_type}");
	            return;
	        }
#endif

	        _m_lPropertyValueList[v] = _value;

	        if (null != _m_crChgRecorder)
	            _m_crChgRecorder.addPropertyChg(v);
	    }

	    /***************
	     * 修改对应属性的值
	     * 
	     * @author alzq.z
	     * @time   May 8, 2013 1:40:05 AM
	     */
	    public void chgValue(E _type, long _chgValue)
	    {
	        if (0 == _chgValue)
	            return;

	        setValue(_type, _m_lPropertyValueList[_m_eoEnumObj.toInt(_type)] + _chgValue);
	    }

	    /************
	     * 增删附加属性对象
	     * 
	     * @author alzq.z
	     * @time   May 10, 2013 12:39:36 AM
	     */
	    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj)
	    {
	        int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
	        if (v >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_infoObj.type}");
	            return;
	        }
#endif

	        if (null == _infoObj)
	            return;

	        setValue(_infoObj.type, _m_lPropertyValueList[v] + _infoObj.value);
	    }
	    public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj, int _stackNum)
	    {
	        int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
	        if (v >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_infoObj.type}");
	            return;
	        }
#endif

	        if (null == _infoObj)
	            return;

	        setValue(_infoObj.type, _m_lPropertyValueList[v] + (_infoObj.value * _stackNum));
	    }

        public void addValue(_TNPBasicPropertyInfoObj<E> _infoObj, long _stackNum)
		{
			int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
            if (v >= _m_lPropertyValueList.Length)
            {
                ALLog.Error($"Property Type err! value{_infoObj.type}");
                return;
            }
#endif
			if (null == _infoObj)
                return;

            setValue(_infoObj.type, _m_lPropertyValueList[v] + (_infoObj.value * _stackNum));
        }

		public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj)
	    {
	        int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
	        if (v >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_infoObj.type}");
	            return;
	        }
#endif

	        if (null == _infoObj)
	            return;

	        setValue(_infoObj.type, _m_lPropertyValueList[v] - _infoObj.value);
	    }
	    public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj, int _stackNum)
	    {
	        int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
	        if (v >= _m_lPropertyValueList.Length)
	        {
	            ALLog.Error($"Property Type err! value{_infoObj.type}");
	            return;
	        }
#endif

	        if (null == _infoObj)
	            return;

	        setValue(_infoObj.type, _m_lPropertyValueList[v] - (_infoObj.value * _stackNum));
	    }

        public void removeValue(_TNPBasicPropertyInfoObj<E> _infoObj, long _stackNum)
		{
			int v = _m_eoEnumObj.toInt(_infoObj.type);
#if UNITY_EDITOR
            if (v >= _m_lPropertyValueList.Length)
            {
                ALLog.Error($"Property Type err! value{_infoObj.type}");
                return;
            }
#endif

			if (null == _infoObj)
                return;

            setValue(_infoObj.type, _m_lPropertyValueList[v] - (_infoObj.value * _stackNum));
        }

        public void addValue(E _type, long _value)
        {
	        int v = _m_eoEnumObj.toInt(_type);
#if UNITY_EDITOR
	        if (v >= _m_lPropertyValueList.Length)
	        {
		        ALLog.Error($"Property Type err! value{_type}");
		        return;
	        }
#endif

	        setValue(_type, _m_lPropertyValueList[v] + _value);
        }
        
        public void removeValue(E _type, long _value)
		{
			int v = _m_eoEnumObj.toInt(_type);
#if UNITY_EDITOR
			if (v >= _m_lPropertyValueList.Length)
			{
				ALLog.Error($"Property Type err! value{_type}");
				return;
			}
#endif

			setValue(_type, _m_lPropertyValueList[v] - _value);
		}
        
		/*****************
	     * 增加属性奖励对象
	     * 
	     * @author alzq.z
	     * @time   May 10, 2013 12:31:35 AM
	     */
		public void addModifier(M _modifier)
	    {
	        if (null == _modifier)
	            return;

	        //逐项更改属性
	        for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
	        {
	            //更改属性
	            addValue(_modifier._m_lPropertyObjList[i]);
	        }
	    }
	    public void removeModifier(M _modifier)
	    {
	        if (null == _modifier)
	            return;

	        //逐项更改属性
	        for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
	        {
	            //更改属性
	            removeValue(_modifier._m_lPropertyObjList[i]);
	        }
	    }
	    public void addModifier(M _modifier, int _stackNum)
	    {
	        if (null == _modifier || 0 == _stackNum)
	            return;

	        if (1 == _stackNum)
	        {
	            addModifier(_modifier);
	        }
	        else
	        {
	            //逐项更改属性
	            for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
	            {
	                //更改属性
	                addValue(_modifier._m_lPropertyObjList[i], _stackNum);
	            }
	        }
	    }

        public void addModifier(M _modifier, long _stackNum)
        {
            if (null == _modifier || 0 == _stackNum)
                return;

            if (1 == _stackNum)
            {
                addModifier(_modifier);
            }
            else
            {
                //逐项更改属性
                for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
                {
                    //更改属性
                    addValue(_modifier._m_lPropertyObjList[i], _stackNum);
                }
            }
        }

		public void removeModifier(M _modifier, int _stackNum)
	    {
	        if (null == _modifier || 0 == _stackNum)
	            return;

	        if (1 == _stackNum)
	        {
	            removeModifier(_modifier);
	        }
	        else
	        {
	            //逐项更改属性
	            for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
	            {
	                //更改属性
	                removeValue(_modifier._m_lPropertyObjList[i], _stackNum);
	            }
	        }
	    }

        public void removeModifier(M _modifier, long _stackNum)
        {
            if (null == _modifier || 0 == _stackNum)
                return;

            if (1 == _stackNum)
            {
                removeModifier(_modifier);
            }
            else
            {
                //逐项更改属性
                for (int i = 0; i < _modifier._m_lPropertyObjList.Count; i++)
                {
                    //更改属性
                    removeValue(_modifier._m_lPropertyObjList[i], _stackNum);
                }
            }
        }

		public void replaceModifier(M _toRemove, long _toRemoveStack, M _toAdd, long _toAddStack)
        {
            removeModifier(_toRemove, _toRemoveStack);
            addModifier(_toAdd, _toAddStack);
        }

        public void replaceModifier(M _toRemove, M _toAdd)
        {
            removeModifier(_toRemove);
            addModifier(_toAdd);
        }

		//增加属性奖励对象
		public void addContainer(C _container)
	    {
	        if(null == _container)
	            return;

	        //逐项更改属性
	        for(int i = 0; i < _container._m_lPropertyValueList.Length; i++)
	        {
	            //更改属性
	            setValue(i, _m_lPropertyValueList[i] + _container._m_lPropertyValueList[i]);
	        }
	    }
	    public void removeContainer(C _container)
	    {
	        if(null == _container)
	            return;

	        //逐项更改属性
	        for(int i = 0; i < _container._m_lPropertyValueList.Length; i++)
	        {
	            //更改属性
	            setValue(i, _m_lPropertyValueList[i] - _container._m_lPropertyValueList[i]);
	        }
	    }
	    public void addContainer(C _container, int _stackNum)
	    {
	        if(null == _container || 0 == _stackNum)
	            return;

	        if(1 == _stackNum)
	        {
	            addContainer(_container);
	        }
	        else
	        {
	            //逐项更改属性
	            for(int i = 0; i < _container._m_lPropertyValueList.Length; i++)
	            {
	                //更改属性
	                setValue(i, _m_lPropertyValueList[i] + (_container._m_lPropertyValueList[i] * _stackNum));
	            }
	        }
	    }
	    public void removeContainer(C _container, int _stackNum)
	    {
	        if(null == _container || 0 == _stackNum)
	            return;

	        if(1 == _stackNum)
	        {
	            removeContainer(_container);
	        }
	        else
	        {
	            //逐项更改属性
	            for(int i = 0; i < _container._m_lPropertyValueList.Length; i++)
	            {
	                //更改属性
	                setValue(i, _m_lPropertyValueList[i] - (_container._m_lPropertyValueList[i] * _stackNum));
	            }
	        }
	    }

	    //带入的_percentModifier作为万分比系数，将每个对应属性的值做万分比加成
	    public void mulPercent(M _percentModifier)
	    {
	        foreach (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj in _percentModifier._m_lPropertyObjList)
	        {
	            long value =_m_lPropertyValueList[npPropertyInfoObj.type.GetHashCode()];

	            _m_lPropertyValueList[npPropertyInfoObj.type.GetHashCode()] =value * npPropertyInfoObj.value / 10000L;
	        }
	    }


	    /** 清空所有属性 */
	    public void clear()
	    {
	        //逐项更改属性
	        for(int i = 0; i < _m_lPropertyValueList.Length; i++)
	        {
	            //更改属性
	            _m_lPropertyValueList[i] = 0;
	        }
	    }
	    
	    //复制
	    public void clone(C _recObj)
	    {
		    if(null == _recObj)
			    return ;
        
		    for(int i = 0; i < _m_lPropertyValueList.Length; i++)
		    {
			    _recObj._m_lPropertyValueList[i] = _m_lPropertyValueList[i];
		    }
	    }

	    //复制
	    public C duplicate()
	    {
		    C newObj = _createContainer();
		    if(null == newObj)
			    return null;
		    clone(newObj);
		    return newObj;
	    }
	    
	    //将属性加成读入到一个modifier里
	    public void readModifier(M _modifier) {
		    for (int i = 0; i < _m_lPropertyValueList.Length; i++) {
			    _modifier.addProperty(_m_eoEnumObj.getEnum(i), _m_lPropertyValueList[i]);
		    }
	    }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < _m_lPropertyValueList.Length; i++)
            {
                sb.Append($"[{_m_eoEnumObj.getEnum(i)}] = {getValue(i)}").Append("\n");
            }
            return sb.ToString();
        }

		//创建一个容器对象
		public abstract C _createContainer();
	    
	    //返回一个标识名称
	    public abstract String getName();

	}
}