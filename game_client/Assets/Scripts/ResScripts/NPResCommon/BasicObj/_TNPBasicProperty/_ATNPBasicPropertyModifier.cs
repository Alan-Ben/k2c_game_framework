using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
	/************************
	 * 属性加成信息对象
	 **/
	[System.Serializable]
	public abstract class _ATNPBasicPropertyModifier<E, M>
	    where E : Enum
	    where M : _ATNPBasicPropertyModifier<E, M>
	{
	    [NotNull] public List<_TNPBasicPropertyInfoObj<E>> _m_lPropertyObjList = new List<_TNPBasicPropertyInfoObj<E>>();

		/// <summary>
		/// 客户端用于显示处理的获取第一个数据的属性对象
		/// </summary>
		public _TNPBasicPropertyInfoObj<E> firstPropInfoObj { get { return  _m_lPropertyObjList.Count <= 0 ? null : _m_lPropertyObjList[0]; } }
		public List<_TNPBasicPropertyInfoObj<E>> propertyObjList { get { return _m_lPropertyObjList; } }

		/**********
		 * 是否为空
		 * @return
		 */
		public bool isEmpty()
		{
			if (null == _m_lPropertyObjList || _m_lPropertyObjList.Count <= 0)
				return true;

			return false;
		}

		/// <summary>
		/// 添加一个属性加成
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_value"></param>
		public void addProperty(E _type, long _value)
	    {
		    if(0 == _value)
			    return ;
        
		    _TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);
        
		    if(null == infoObj)
		    {
			    infoObj = new _TNPBasicPropertyInfoObj<E>();
			    infoObj.type = _type;
			    infoObj.value = _value;

				_m_lPropertyObjList.Add(infoObj);
		    }
		    else
		    {
			    infoObj.value += _value;
		    }
		}

		/// <summary>
		/// 设置一个属性加成
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_value"></param>
		public void setProperty(E _type, long _value)
	    {
		    _TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);
        
		    if(null == infoObj)
		    {
				if (0 == _value)
					return;

			    infoObj = new _TNPBasicPropertyInfoObj<E>();
			    infoObj.type = _type;
			    infoObj.value = _value;

				_m_lPropertyObjList.Add(infoObj);
		    }
		    else
		    {
			    infoObj.value = _value;

				//判断是否为0，是则删除数据
				if (0 == infoObj.value)
					_m_lPropertyObjList.Remove(infoObj);
			}
		}

		/****************
		 * 删除一个属性加成
		 * @param _type
		 * @param _value
		 */
		public void rmvProperty(E _type, long _value)
		{
			if (0 == _value)
				return;

			_TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);

			if (null == infoObj)
			{
				infoObj = new _TNPBasicPropertyInfoObj<E>();
				infoObj.type = _type;
				infoObj.value = -_value;

				_m_lPropertyObjList.Add(infoObj);
			}
			else
			{
				infoObj.value -= _value;

				//判断是否为0，是则删除数据
				if (0 == infoObj.value)
					_m_lPropertyObjList.Remove(infoObj);
			}
		}

		/**************
		 * 清空属性列表
		 */
		public void clear()
		{
			_m_lPropertyObjList.Clear();
		}

		/// <summary>
		/// 获取对应属性的值
		/// </summary>
		/// <param name="_propertyType"></param>
		/// <returns></returns>
		public long getPropValue(E _propertyType)
	    {
			long v = 0;

            _TNPBasicPropertyInfoObj<E> infoObj = null;
            for (int i = 0; i < _m_lPropertyObjList.Count; i++)
            {
                infoObj = _m_lPropertyObjList[i];
                if (null == infoObj)
                    continue;

                if (!infoObj.type.Equals(_propertyType))
                    continue;
                v += infoObj.value;
            }

            return v;
		}

	    /// <summary>
	    /// 单个数据的读取函数
	    /// </summary>
	    /// <param name="_str"></param>
	    public void ParseFromString(string _str)
	    {
	        readStr(_str, string.Empty);
	    }

	    /******************
	     * 从带入的字符串内读取属性加成信息
	     * 
	     * @author alzq.z
	     * @time   Aug 27, 2013 10:57:11 PM
	     */
	    public void readStr(string _str, string _fieldName = "")
	    {
	        if(string.IsNullOrEmpty(_str))
	            return ;

	        string[] strs = _str.Split(';');
	        for(int i = 0; i < strs.Length; i++)
	        {
	            string itemStr = strs[i];
	            //解析属性对象信息
	            _TNPBasicPropertyInfoObj<E> infoObj = _TNPBasicPropertyInfoObj<E>.readPropertyInfoObj(itemStr, _fieldName);
	            if(null == infoObj || infoObj.value == 0)
	                continue;

				//加入数据集
				_m_lPropertyObjList.Add(infoObj);
	        }
	    }
	    
	    /// <summary>
	    /// 深度拷贝复制一个对象
	    /// </summary>
	    /// <returns></returns>
	    public M duplicate()
	    {
		    M ret = _createModifier();
		    foreach (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj in _m_lPropertyObjList)
		    {
			    ret._m_lPropertyObjList.Add(npPropertyInfoObj.duplicate());
		    }
		    
		    return ret;
	    }
	    public M duplicate(int _stack)
	    {
		    M ret = _createModifier();
		    foreach (_TNPBasicPropertyInfoObj<E> npPropertyInfoObj in _m_lPropertyObjList)
		    {
			    ret._m_lPropertyObjList.Add(npPropertyInfoObj.duplicate(_stack));
		    }
		    return ret;
	    }
	    
	    //将给定modify合并到自己身上
	    public _ATNPBasicPropertyModifier<E, M> addModifier(M _m) {
		    foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m._m_lPropertyObjList) {
	            _TNPBasicPropertyInfoObj<E> obj1 = __lookup(obj2.type);

				//累加属性
				addProperty(obj2.type, obj2.value);
			}
	        return this;
		}

        /**
		 * 将给定modify合并到自己身上
		 * @return this
		 */
        public _ATNPBasicPropertyModifier<E, M> addModifier(M _m, long _stack)
        {
            foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m._m_lPropertyObjList)
            {
                //累加属性
                addProperty(obj2.type, obj2.value * _stack);
            }
            return this;
        }

		public _ATNPBasicPropertyModifier<E, M> removeModifier(M _m)
		{
			foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m._m_lPropertyObjList)
			{
				//移除属性
				rmvProperty(obj2.type, obj2.value);
			}
			return this;
		}

        /**
		 * 将给定modify从自己身上移除
		 * @return this
		 */
        public _ATNPBasicPropertyModifier<E, M> removeModifier(M _m, long _stack)
        {
            foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m._m_lPropertyObjList)
            {
                //移除属性
                rmvProperty(obj2.type, obj2.value * _stack);
            }

            return this;
        }

		//将给定万分比加成到自己身上
		public _ATNPBasicPropertyModifier<E, M> multipleM(long _multiple) {
		    foreach (_TNPBasicPropertyInfoObj<E> obj in _m_lPropertyObjList) {
	            obj.value = obj.value * _multiple / 10000;
	        }
	        return this;
		}

		/// <summary>
		/// 将给定万分比加成到固定属性上
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_multiple"></param>
		/// <returns></returns>
		public _ATNPBasicPropertyModifier<E, M> multipleM(E _type, long _multiple)
		{
			_TNPBasicPropertyInfoObj<E> infoObj = __lookup(_type);

			if (null == infoObj)
				return this;

			infoObj.value = infoObj.value * _multiple / 10000;

			return this;
		}

        public List<_TNPBasicPropertyInfoObj<E>> getObjList()
        {
            return new List<_TNPBasicPropertyInfoObj<E>>(_m_lPropertyObjList);
        }

		//两个Modifier相加
		public static _ATNPBasicPropertyModifier<E, M> plus(M _m1, M _m2)
	    {
	        M ret = _m1.duplicate();
	        foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m2._m_lPropertyObjList)
	        {
	            _TNPBasicPropertyInfoObj<E> obj1 =  ret.__lookup(obj2.type);
	            if(null != obj1)
	            {
	                obj1.value += obj2.value;
	            }
	            else
	            {
	                ret._m_lPropertyObjList.Add(obj2.duplicate());
	            }
	        }
	        return ret;
	    }
	    public static _ATNPBasicPropertyModifier<E, M> plus(M _m1, int _m1Stack, M _m2, int _m2Stack)
	    {
	        M ret = _m1.duplicate(_m1Stack);
	        foreach (_TNPBasicPropertyInfoObj<E> obj2 in _m2._m_lPropertyObjList)
	        {
	            _TNPBasicPropertyInfoObj<E> obj1 =  ret.__lookup(obj2.type);
	            if(null != obj1)
	            {
	                obj1.value += (obj2.value * _m2Stack);
	            }
	            else
	            {
	                ret._m_lPropertyObjList.Add(obj2.duplicate(_m2Stack));
	            }
	        }
	        return ret;
	    }

	    //将第二个modifier作为万分比加成，作用于前面的属性上
	    public static _ATNPBasicPropertyModifier<E, M> mulPercent(M _modifier, M _percent)
	    {
	        M ret = _modifier.duplicate();
	        foreach (_TNPBasicPropertyInfoObj<E> obj2 in _percent._m_lPropertyObjList)
	        {
	            //只有在检索到对应值的时候才能加成，否则不做处理
	            _TNPBasicPropertyInfoObj<E> obj1 =  ret.__lookup(obj2.type);
	            if(null != obj1)
	            {
	                obj1.value = obj1.value * obj2.value / 10000L;
	            }
	        }
	        return ret;
	    }
	    
	    //查找某个属性
	    public _TNPBasicPropertyInfoObj<E> lookup(E _type)
	    {
		    return __lookup(_type);
	    }
	    
	    //查找某个属性
	    protected  _TNPBasicPropertyInfoObj<E> __lookup(E _type)
	    {
		    foreach (_TNPBasicPropertyInfoObj<E> tnpBasicPropertyInfoObj in _m_lPropertyObjList)
		    {
			    if (tnpBasicPropertyInfoObj.type.Equals(_type))
			    {
				    return tnpBasicPropertyInfoObj;
			    }
		    }
		    return null;
	    }
	    
	    /// <summary>
	    /// 创建一个编辑器对象
	    /// </summary>
	    /// <returns></returns>
	    protected abstract M _createModifier();

	    public abstract string ToString();
	}
}