package NPCommon.Property;

import java.util.Hashtable;

/******
 * 枚举相关的基类，派生于此类的子类，将获得指定枚举的运行时信息。
 * @param <E>
 */
public class _TBaseEnumObj<E extends Enum<E>>
{
    /**
     * 构造全局对象方便检索
     */
    @SuppressWarnings("rawtypes")
    private static Hashtable<Class, _TBaseEnumObj> _g_htBaseEnumTable = new Hashtable<>();

    //根据带入的class获取对应的枚举信息对象
    @SuppressWarnings("unchecked")
    public static <E extends Enum<E>> _TBaseEnumObj<E> getBaseEnum(Class<E> _enumClass)
    {
        return _g_htBaseEnumTable.computeIfAbsent(_enumClass, aClass -> new _TBaseEnumObj<>(_enumClass));
    }

    //枚举类存储对象，方便模板类做其他操作
    private Class<E> _m_cClass;
    private E[] _m_arrEnumArr;

    protected _TBaseEnumObj(Class<E> _enumClass)
    {
        _m_cClass = _enumClass;
        _m_arrEnumArr = _m_cClass.getEnumConstants();
    }

    /****
     * 返还枚举的Class
     * @return
     */
    public Class<E> getEnumClass()
    {
        return _m_cClass;
    }

    /***
     * 返回枚举长度
     * @return
     */
    public int getEnumLength()
    {
        return _m_arrEnumArr.length;
    }

    /***
     * 指定索引，返回枚举值
     * @param _index
     * @return
     */
    public E getEnum(int _index)
    {
        if (_index < 0 || _index >= getEnumLength())
        {
            return null;
        }
        return _m_arrEnumArr[_index];
    }
}
