package NPCommon.RefData.RefContainer;

import NPCommon.RefData.Ref.RefBase;

import java.util.Comparator;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

/******************
 * 配表管理器的部分实现抽象类，存储记录列表，id不唯一的配表，使用List存储
 * @author scott
 *
 * @param <K>
 * @param <T>
 */
public class RefListContainer<T extends RefBase> extends RefContainerBase<T>
{
    private CopyOnWriteArrayList<T> _m_lObjList;

    public RefListContainer()
    {
        _m_lObjList = new CopyOnWriteArrayList<>();
    }

    public List<T> getList()
    {
        return _m_lObjList;
    }

    @Override
    public boolean isEmpty()
    {
        return _m_lObjList.isEmpty();
    }

    @Override
    public void initPut(T _value)
    {
        if (null == _value)
            return;

        _m_lObjList.add(_value);
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
        if (null == _value)
            return;

        //创建临时队列，将数据都拷贝到临时队列。避免在线程访问的时候出现冲突
        CopyOnWriteArrayList<T> tmpList = new CopyOnWriteArrayList<T>(_m_lObjList);
        tmpList.addAll(_value);

        //对列表进行排序
        tmpList.sort(Comparator.comparingLong(RefBase::Id));

        _m_lObjList = tmpList;
    }

    @Override
    public T get(long _key)
    {
        T tmp = null;
        for (int i = 0; i < _m_lObjList.size(); i++)
        {
            tmp = _m_lObjList.get(i);
            if (null == tmp)
                continue;

            if (tmp.Id() == _key)
                return tmp;
        }
        return null;
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
        _m_lObjList.sort(Comparator.comparingLong(RefBase::Id));
    }
}
