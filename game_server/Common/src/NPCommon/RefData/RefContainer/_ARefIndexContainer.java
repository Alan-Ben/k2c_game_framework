package NPCommon.RefData.RefContainer;

import NPCommon.RefData.Ref.RefBase;

import java.util.ArrayList;
import java.util.List;

/******************
 * 配表管理器的部分实现抽象类
 * @author scott
 *
 * @param <K>
 * @param <T>
 */
public abstract class _ARefIndexContainer<T extends RefBase> extends RefContainerBase<T>
{
    private ArrayList<T> _m_lObjList;

    public _ARefIndexContainer()
    {
        int maxCount = _getIndexCount();
        _m_lObjList = new ArrayList<T>(_getIndexCount());
        for (int i = 0; i < maxCount; i++)
            _m_lObjList.add(null);
    }

    public ArrayList<T> getRefList()
    {
        return _m_lObjList;
    }

    @Override
    public boolean isEmpty()
    {
        for (int i = 0; i < _m_lObjList.size(); i++)
        {
            if (null != _m_lObjList.get(i))
                return false;
        }

        return true;
    }

    @Override
    public void initPut(T _value)
    {
        if (null == _value)
            return;

        _m_lObjList.set((int) _value.Id(), _value);
    }

    @Override
    public void put(List<T> _value)
    {
        if (null == _value)
            return;

        for (T t : _value)
        {
            _m_lObjList.set((int) t.Id(), t);
        }
    }

    @Override
    public T get(long _key)
    {
        if (_key >= _m_lObjList.size())
            return null;

        return _m_lObjList.get((int) _key);
    }

    /*************
     * 数据都初始化完成之后调用的函数
     *
     * @author alzq.z
     * @time 2019年4月3日 下午10:34:18
     */
    @Override
    public void onLoaded()
    {

    }

    @Override
    public void internalSortRefs()
    {

    }

    /*************
     * 获取队列数量上限
     *
     * @author alzq.z
     * @time 2019年4月5日 下午1:28:56
     */
    protected abstract int _getIndexCount();
}
