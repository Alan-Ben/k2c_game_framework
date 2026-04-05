package NPGameRes.CacheSys;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;

import java.util.ArrayList;

/**************
 * 非安全的缓存控制对象
 * @author Administrator
 *
 * @param <TEMP>
 */
public abstract class _AALUnsafeQuickCacheController<T>
{

    //创建的对象缓存池，默认创建最少数量，超出最大数量则删除过多缓存
    private int _m_iMinCacheCount = 10;

    /**
     * 还可使用的缓存队列
     */
    private ArrayList<T> _m_lEnableCacheList;
    private int _m_nextObjIndex = 0;
    private boolean _m_inited = false;
    private boolean _m_bRestElements = false;

    protected _AALUnsafeQuickCacheController(int _minCount, boolean _bResetElements)
    {
        _m_iMinCacheCount = _minCount;
        _m_bRestElements = _bResetElements;
        _m_lEnableCacheList = new ArrayList<T>();
    }

    /****************
     * 带入模板对象进行初始化
     **/
    public void init()
    {
        if (_m_inited)
        {
            //输出错误
            ALServerLog.Error("Init  _AALUnsafeQuickCacheController multiple times!");
            return;
        }

        _m_inited = true;

        //逐个实例化子窗口对象
        for (int i = 0; i < _m_iMinCacheCount; i++)
        {
            //创建控制对象
            T newItem = _createItem();
            if (null == newItem)
                break;

            //先重置对象
            _resetItem(newItem);
            //将对象加入缓存队列
            _m_lEnableCacheList.add(newItem);
        }
    }


    /******************
     * 释放资源
     **/
    public void discard()
    {
        for (int i = 0; i < _m_lEnableCacheList.size(); i++)
        {
            _discardItem(_m_lEnableCacheList.get(i));
        }
        //清空队列
        _m_lEnableCacheList.clear();

    }


    public void resetAll()
    {
        if (_m_bRestElements)
        {
            for (int i = 0; i < _m_nextObjIndex; i++)
            {
                _resetItem(_m_lEnableCacheList.get(i));
            }

        }

        _m_nextObjIndex = 0;
    }

    /***************
     * 取出一个对象名称显示对象
     **/
    public T newItem()
    {
        //判断缓存是否有对象，有则直接返回
        if (_m_nextObjIndex >= _m_lEnableCacheList.size())
        {
            T newItem = _createItem();
            _m_lEnableCacheList.add(newItem);

            int size = _m_lEnableCacheList.size();
            if (size % 100 == 0 && size >= 800)
            {
                CommLog.info("Allocator:{} cahced obj num excceed:{},", this.getClass().getSimpleName(), size, new Exception());
            }
        }
        T ret = _m_lEnableCacheList.get(_m_nextObjIndex);
        _m_nextObjIndex++;
        if (_m_nextObjIndex > _m_lEnableCacheList.size())
        {
            CommLog.fatal("[Cache] List of type:{},Index:{} out of bounds,size:{}!", ret.getClass().getSimpleName(), _m_nextObjIndex, _m_lEnableCacheList.size(), new Exception());
        }
        return ret;
    }

    //根据模板创建对象的函数
    protected abstract T _createItem();

    //设置对象无效
    protected abstract void _resetItem(T _item);

    //设置对象无效
    protected abstract void _discardItem(T _item);

    public int getCahceLength()
    {
        return _m_lEnableCacheList.size();
    }
}
