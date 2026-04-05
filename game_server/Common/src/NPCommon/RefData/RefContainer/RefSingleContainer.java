package NPCommon.RefData.RefContainer;

import NPCommon.RefData.Ref.RefBase;

import java.util.List;

/******************
 * 配表管理器的部分实现抽象类，单条对象一个管理器
 * @author scott
 *
 * @param <T>
 */
public class RefSingleContainer<T extends RefBase> extends RefContainerBase<T>
{
    private T _m_lObj;

    public RefSingleContainer()
    {
        _m_lObj = null;
    }

    public T getRef()
    {
        return _m_lObj;
    }

    @Override
    public boolean isEmpty()
    {
        return null == _m_lObj;
    }

    @Override
    public void initPut(T _ref)
    {
        if (null == _ref)
            return;

        _m_lObj = _ref;
    }

    /************
     * 在执行过程中可修改数据的处理函数
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:35:40
     */
    @Override
    public void put(List<T> _value)
    {
        if (null == _value || _value.isEmpty())
            return;

        _m_lObj = _value.get(0);
    }

    @Override
    public T get(long _key)
    {
        return _m_lObj;
    }

    /**
     * 数据都初始化完成之后调用的函数
     */
    @Override
    public void onLoaded()
    {

    }

    @Override
    public void internalSortRefs()
    {

    }
}
