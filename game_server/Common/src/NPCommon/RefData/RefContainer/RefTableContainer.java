package NPCommon.RefData.RefContainer;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.Util.CommonFunc;

import java.util.Collection;
import java.util.Comparator;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.CopyOnWriteArrayList;

/******************
 * 配表管理器的部分实现抽象类,单一id对应一条记录，使用map 存储
 * @author scott
 *
 * @param <K>
 * @param <T>
 */
public class RefTableContainer<T extends RefBase> extends RefContainerBase<T>
{
    private ConcurrentHashMap<Long, T> _m_hmObjMap;
    private CopyOnWriteArrayList<T> _m_lObjList;

    public RefTableContainer()
    {
        _m_hmObjMap = new ConcurrentHashMap<>();
        _m_lObjList = new CopyOnWriteArrayList<>();
    }

    public List<T> getList()
    {
        return _m_lObjList;
    }
    
    public T getRnd()
    {
    	if(_m_lObjList.isEmpty())
    		return null;
    	
    	int idx = CommonFunc.randomInt(_m_lObjList.size() - 1);
    	return _m_lObjList.get(idx);
    }

    /**
     * 返回第一个元素
     * @return T
     */
    public T first()
    {
        if (_m_lObjList.isEmpty())
            return null;
        return _m_lObjList.get(0);
    }

    public Collection<T> values()
    {
        return _m_hmObjMap.values();
    }

    public void copyValues(Collection<T> _recCol)
    {
        if (null == _recCol)
            return;

        _recCol.addAll(_m_hmObjMap.values());
    }

    @Override
    public boolean isEmpty()
    {
        return _m_hmObjMap.isEmpty();
    }

    @Override
    public void initPut(T _value)
    {
        if (null == _value)
            return;

        _m_hmObjMap.put(_value.Id(), _value);
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

        //遍历数据放入map数据集
        for(T t : _value)
        {
            if (null == t)
                continue;
            _m_hmObjMap.put(t.Id(), t);
        }

        //创建临时队列，将数据都拷贝到临时队列。避免在线程访问的时候出现冲突
        CopyOnWriteArrayList<T> tmpList = new CopyOnWriteArrayList<T>(_m_lObjList);
        tmpList.addAll(_value);

        //对列表进行排序
        tmpList.sort(Comparator.comparingLong(RefBase::Id));

        _m_lObjList = tmpList;
    }

    @Override
    public T get(long key)
    {
        return _m_hmObjMap.get(key);
    }

    /**
     * 数据都初始化完成之后调用的函数
     */
    @Override
    public final void onLoaded()
    {
        _onTableLoaded();
    }

    @Override
    public void internalSortRefs()
    {
        _m_lObjList.sort(Comparator.comparingLong(RefBase::Id));
    }

    /********
     * 在本对象内重载处理
     *
     * @author alzq.z
     * @time 2019年4月5日 下午11:15:42
     */
    protected void _onTableLoaded()
    {
    }
}
