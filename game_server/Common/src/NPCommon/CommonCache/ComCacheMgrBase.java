package NPCommon.CommonCache;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;

import java.util.List;


public abstract class ComCacheMgrBase<K, T extends ComCachedDataBase, L extends ComCacheLoaderBase<K, T>> implements _IALSynTask
{

    private TimeKeyLinkList<K, L> _m_timeKeyLink = new TimeKeyLinkList<>();//按时间顺序排序的队列


    public int getLoaderMapSize()
    {
        return _m_timeKeyLink.size();
    }    //返回loader数量

    public abstract int getUnloadCheckSpanSec(); //定义检测卸载的间隔时长，每隔多少时间检测一次

    protected abstract int getLoaderExpiredSec();//定义loader过期多少秒后需要被卸载

    protected abstract L createLoader(K _key); // 创建一个Loader

    protected ComCacheMgrBase()
    {
        ALSynTaskManager.getInstance().regTask(this, getUnloadCheckSpanSec() * 1000);
    }

    @Override
    public void run()
    {
        _unloadExpiredLoader();
        ALSynTaskManager.getInstance().regTask(this, getUnloadCheckSpanSec() * 1000);

    }

    /*****
     * 卸载过期的加载器
     */
    private void _unloadExpiredLoader()
    {
        //表示不设置超时时间
        if (-1 == getLoaderExpiredSec())
            return;

        if (_m_timeKeyLink.isEmpty())
            return;

        List<L> expiredLoaders = _m_timeKeyLink.popFirstList(getLoaderExpiredSec());
        if (expiredLoaders == null || expiredLoaders.isEmpty())
            return;
        for (L loader : expiredLoaders)
        {
            //移除后的回调处理
            loader.callbackOnRemoved();
        }


        CommLog.info("[CACHE]:{} unload:{},remain size:{}", getClass().getSimpleName(), expiredLoaders.size(), _m_timeKeyLink.size());
    }

    /*******
     * 自定Key，异步返回加载后的Loader
     * @param _key
     * @param _dealer
     */
    public void getData(K _key, HandlerTwo<Boolean, T> _dealer)
    {
        L loader = ensureLoader(_key);
        loader.asyncLoad(_dealer);
    }

    /******
     * 查找Loader，没有的话创建一个
     * @param _key
     * @return
     */
    public L ensureLoader(K _key)
    {
        L loader = _m_timeKeyLink.computeIfAbsent(_key, k -> createLoader(_key));
        _m_timeKeyLink.update(loader);
        return loader;
    }

    /******
     * 直接查找数据
     * @param _key
     * @return
     */
    public T lookupData(K _key)
    {
        L loader = ensureLoader(_key);
        return loader.getCacheData();

    }

    /**
     * 按键值移除数据
     * @param _key
     */
    public void removeByKey(K _key)
    {
        _m_timeKeyLink.removeByKey(_key);
    }
}
