package NPCommon.Property;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;


public class _TNPBasicPropertyInfoObj<E extends Enum<E>>
{
    /**
     * 属性类型
     */
    public E type;
    /**
     * 属性具体值
     */
    public long value;

    public _TNPBasicPropertyInfoObj()
    {
        value = 0;
    }

    /******************
     * 从带入的字符串内读取属性加成信息
     *
     * @author alzq.z
     * @time Aug 27, 2013 10:57:11 PM
     */
    public void readStr(String _str, String _fieldName, Class<E> _enumClass)
    {
        if (_str.isEmpty())
            return;

        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length < 2)
            return;

        try
        {
            type = Enum.valueOf(_enumClass, strs[0].toUpperCase().trim());
            value = Long.parseLong(strs[1].trim());
        } catch (Exception e)
        {
            CommLog.error("read property info err4! : " + _str + "\t\t_fieldName:  " + _fieldName, e);
        }
    }

    /****************
     * 拷贝数据
     * @return
     */
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

    @Override
    public String toString()
    {
        return String.format("%s:%d", type, value);
    }
}
