package NPGameRes.CacheSys;

import ALServerLog.ALServerLog;

import java.util.ArrayList;

/**************
 * 非安全的缓存控制对象
 * @author Administrator
 *
 * @param <TEMP>
 */
public abstract class _AALUnsafeCacheController<T, TEMP>
{
    //模板对象
    private TEMP _m_tTemplateObj;

    //创建的对象缓存池，默认创建最少数量，超出最大数量则删除过多缓存
    private int _m_iMinCacheCount = 10;

    /**
     * 还可使用的缓存队列
     */
    private ArrayList<T> _m_lEnableCacheList;

    protected _AALUnsafeCacheController(int _minCount)
    {
        _m_tTemplateObj = null;

        _m_iMinCacheCount = _minCount;

        _m_lEnableCacheList = new ArrayList<T>();
    }

    /****************
     * 带入模板对象进行初始化
     **/
    public void init(TEMP _template)
    {
        if (null != _m_tTemplateObj)
        {
            //输出错误
            ALServerLog.Error("Init Cache Controller multiple times!");
            return;
        }

        //设置模板对象
        _m_tTemplateObj = _template;
        //创建名称显示对象池
        if (null != _m_tTemplateObj)
        {
            //逐个实例化子窗口对象
            for (int i = 0; i < _m_iMinCacheCount; i++)
            {
                //创建控制对象
                T newItem = _createItem(_m_tTemplateObj);
                if (null == newItem)
                    break;

                //先重置对象
                _resetItem(newItem);
                //将对象加入缓存队列
                _m_lEnableCacheList.add(newItem);
            }
        }

        //调用事件函数
        _onInit(_template);
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
        //重置模板
        _m_tTemplateObj = null;
    }

    /***************
     * 取出一个对象名称显示对象
     **/
    public T popItem()
    {
        //判断缓存是否有对象，有则直接返回
        if (_m_lEnableCacheList.size() > 0)
        {
            //取出最后一个对象
            T firstItem = _m_lEnableCacheList.get(_m_lEnableCacheList.size() - 1);
            _m_lEnableCacheList.remove(_m_lEnableCacheList.size() - 1);
            return firstItem;
        }
        //如无缓存对象则需要创建一个新的名称对象
        T newItem = _createItem(_m_tTemplateObj);
        //先重置对象
        _resetItem(newItem);

        //返回结果
        return newItem;
    }

    /*****************
     * 将名称操作对象放回缓存队列
     **/
    public void pushBackCacheItem(T _item)
    {
        if (null == _item)
            return;

        //设置对象无效
        _resetItem(_item);
        //放入缓存队列
        _m_lEnableCacheList.add(_item);
    }

    //初始化时的事件函数
    protected abstract void _onInit(TEMP _template);

    //根据模板创建对象的函数
    protected abstract T _createItem(TEMP _template);

    //释放创建出来的对象的资源
    protected abstract void _discardItem(T _item);

    //设置对象无效
    protected abstract void _resetItem(T _item);
}
