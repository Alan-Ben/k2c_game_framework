using System;
using System.Collections.Generic;

using ALPackage;
using GOE;
using NPEnum;

/****************
 * 记录属性信息的具体对象
 **/
[System.Serializable]
public class NPPlayerPropertyInfoObj
{
    /** 属性类型 */
    public ENPPlayerPropertyType type;
    /** 属性具体值 */
    public long value;

    /******************
     * 从带入的字符串内读取属性加成信息
     * 
     * @author alzq.z
     * @time   Aug 27, 2013 10:57:11 PM
     */
    public static NPPlayerPropertyInfoObj readPropertyInfoObj(string _str, string _fieldName = "unknow")
    {
        if (string.IsNullOrEmpty(_str)) {
            UnityEngine.Debug.LogError(string.Format("read excel field({0}) info err!, str is null or empty!", _fieldName));
            return null;
        }

        NPPlayerPropertyInfoObj obj = new NPPlayerPropertyInfoObj();

        string[] strs = _str.Split(':');
        if (strs.Length < 2)
        {
            UnityEngine.Debug.LogError("read property info err! : " + _str);
            return null;
        }

        obj.type = (ENPPlayerPropertyType)ALCommon.EnumParse(typeof(ENPPlayerPropertyType), strs[0], true);
        obj.value = long.Parse(strs[1]);

        return obj;
    }
    
    
    
    /****************
   * 拷贝数据
   * @return
   */
    public NPPlayerPropertyInfoObj duplicate()
    {
        NPPlayerPropertyInfoObj ret = new NPPlayerPropertyInfoObj();
        ret.type = type;
        ret.value = value;
        return ret;
    }
    
    public NPPlayerPropertyInfoObj duplicate(int _stack)
    {
        NPPlayerPropertyInfoObj ret = new NPPlayerPropertyInfoObj();
        ret.type = type;
        ret.value = value * _stack;
        return ret;
    }
}
