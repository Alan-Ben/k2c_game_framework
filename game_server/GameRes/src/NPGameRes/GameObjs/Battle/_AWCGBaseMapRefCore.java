package NPGameRes.GameObjs.Battle;

import java.util.ArrayList;
import java.util.TreeMap;


@SuppressWarnings("serial")
public abstract class _AWCGBaseMapRefCore<T extends _IALBasicRefObj> extends TreeMap<Long, T>
{

    public abstract void initData();

    /*******************
     * 根据id获取对应的数据
     **/
    public T getRef(Long _id)
    {
        return this.get(_id);
    }

    /*******************
     * 获取所有数据对象的队列
     **/
    public ArrayList<T> getAllRefList()
    {
        ArrayList<T> arr = new ArrayList<>(this.values());
        return arr;
    }

    protected void add(T obj)
    {
        this.put(obj._refId(), obj);
    }
}
