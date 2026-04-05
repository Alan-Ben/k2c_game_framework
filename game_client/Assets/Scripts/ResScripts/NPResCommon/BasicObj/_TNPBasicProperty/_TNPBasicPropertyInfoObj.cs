using System;
using ALPackage;

namespace GOE
{
	/****************
	 * 记录属性信息的具体对象
	 **/
	[System.Serializable]
	public class _TNPBasicPropertyInfoObj<E> where E : Enum
	{
	    /** 属性类型 */
	    public E type;
	    /** 属性具体值 */
	    public long value;

	    /******************
	     * 从带入的字符串内读取属性加成信息
	     * 
	     * @author alzq.z
	     * @time   Aug 27, 2013 10:57:11 PM
	     */
	    public static _TNPBasicPropertyInfoObj<E> readPropertyInfoObj(string _str, string _fieldName = "unknow")
	    {
	        if (string.IsNullOrEmpty(_str)) {
	            UnityEngine.Debug.LogError(string.Format("read excel field({0}) info err!, str is null or empty!", _fieldName));
	            return null;
	        }

	        _TNPBasicPropertyInfoObj<E> obj = new _TNPBasicPropertyInfoObj<E>();

	        string[] strs = _str.Split(':');
	        if (strs.Length < 2)
	        {
	            UnityEngine.Debug.LogError("read property info err! : " + _str);
	            return null;
	        }

	        obj.type = (E)ALCommon.EnumParse(typeof(E), strs[0], true);
	        obj.value = long.Parse(strs[1]);

	        return obj;
	    }
	    
	    /// <summary>
	    /// 拷贝数据
	    /// </summary>
	    /// <returns></returns>
	    public _TNPBasicPropertyInfoObj<E> duplicate()
	    {
		    _TNPBasicPropertyInfoObj<E> ret = new _TNPBasicPropertyInfoObj<E>();
		    ret.type = type;
		    ret.value = value;
		    return ret;
	    }
	    public _TNPBasicPropertyInfoObj<E> duplicate(int _stack)
	    {
		    _TNPBasicPropertyInfoObj<E> ret = new _TNPBasicPropertyInfoObj<E>();
		    ret.type = type;
		    ret.value = value * _stack;
		    return ret;
	    }
	}
}