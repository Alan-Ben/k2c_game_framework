package NPCommon.RefData.RefContainer;

import NPCommon.RefData.Ref.RefBase;

import java.util.List;

/**
 * 配表管理器
 * @param <T>
 * @author scott
 */
public abstract class RefContainerBase<T extends RefBase>
{
    private String _m_tableName = "";
    /**
     * 容器中存储对象的实际类型
     */
    private Class<? extends RefBase> _m_refClazz;

    public Class<? extends RefBase> getRefClass()
    {
        return _m_refClazz;
    }

    public void setRefClass(Class<? extends RefBase> _clazz)
    {
        _m_refClazz = _clazz;
    }

    public String getTableName()
    {
        return _m_tableName;
    }

    public void setTableName(String _tableName)
    {
        _m_tableName = _tableName;
    }

    public abstract boolean isEmpty();

    /************
     * 放入数据
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:35:40
     */
    public abstract void initPut(T _value);

    /************
     * 在执行过程中可修改数据的处理函数
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:35:40
     */
    public abstract void put(List<T> _value);

    /**
     * 查询数据
     */
    public abstract T get(long key);

    /**
     * 数据都初始化完成之后调用的函数
     */
    public abstract void onLoaded();

    /**
     * 数据完成后对数据进行排序
     * 仅在服务器初始化时调用，热更配表的排序不调用该方法
     */
    public abstract void internalSortRefs();

    /**
     * 通过一个 refBase 修改另一个,避免使用反射，保证效率
     * @param _newRef 尝试设置的对象
     * @return 返回是否设置成功
     */
    public boolean resetRef(T _newRef)
    {
        T t = get(_newRef.Id());
        if (t == null)
            return false;

        //存在就数据，将新值附加
        t.resetRef(_newRef);

        return true;
    }
}
