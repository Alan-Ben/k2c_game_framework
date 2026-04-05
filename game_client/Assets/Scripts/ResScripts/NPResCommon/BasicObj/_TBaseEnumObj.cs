using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;


namespace GOE
{
	/****************
	 * 记录属性信息的具体对象
	 **/
	public class _TBaseEnumObj<E> where E : Enum
	{
	    /** 构造全局对象方便检索 */
	    public static _TBaseEnumObj<E> g_enumObj = new _TBaseEnumObj<E>();

	    //枚举类存储对象，方便模板类做其他操作
	    private E[] _m_arrEArr;

	    protected _TBaseEnumObj()
	    {
	        Array arr = System.Enum.GetValues(typeof(E));

	        //创建数组
	        _m_arrEArr = new E[arr.Length];
	        for(int i = 0; i < arr.Length; i++)
	        {
	            _m_arrEArr[i] = (E)arr.GetValue(i);
	        }
	    }

	    /***
	     * 返回枚举长度
	     * @return
	     */
	    public int enumLength
	    {
	        get
	        {
	            return _m_arrEArr.Length;
	        }
	    }

	    /***
	     * 指定索引，返回枚举值
	     * @param _index
	     * @return
	     */
	    public E getEnum(int _index)
	    {
	        if (_index < 0 || _index >= enumLength)
	        {
	            return default(E);
	        }

	        return _m_arrEArr[_index];
	    }

	    /// <summary>
	    /// 参考：https://wenku.baidu.com/view/99559714ed06eff9aef8941ea76e58fafab045e6.html
	    /// 直接使用GetHashCode
	    /// </summary>
	    /// <param name="_e"></param>
	    /// <returns></returns>
	    public int toInt(E _e)
	    {
	        return _e.GetHashCode();
	    }
	}
}