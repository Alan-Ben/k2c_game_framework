package EventSystem;

import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate._IHandlerHolder;

import java.lang.ref.WeakReference;
import java.util.HashSet;

/*******************
 * 每一个holder关联事件Info的数据管理器
 * @author mj
 *
 */
public class NPHolderHandlerMap<T>
{
    private Class<?> _m_clazz;
    //本对象归属的处理对象,弱引用，get时有可能为空,这里主要是不想干扰外部内存回收机制，避免内存泄漏
    private WeakReference<_IHandlerHolder> _m_hHolder;
    //用来记录这个 holder 在 handlerInfo 里注册了哪些 HandlerEntry
    private HashSet<NPHandlerEntry<T>> _m_alHandlerList;
    //是否手动触发过反注册
    private boolean _m_bOptUnReg;

    protected NPHolderHandlerMap(_IHandlerHolder _holder)
    {
        _m_clazz = _holder.getClass();
        _m_hHolder = new WeakReference<>(_holder);
        _m_alHandlerList = new HashSet<>();
    }

    public _IHandlerHolder getHolder()
    {
        return _m_hHolder.get();
    }

    public boolean isEmpty()
    {

        return _m_alHandlerList.isEmpty();
    }

    protected void setOptUnReg(boolean _optUnReg)
    {
        this._m_bOptUnReg = _optUnReg;
    }

    /**
     * 记录这个 holder 在哪个 delegate 里注册了何种 handler
     * @param _handlerEntry handler身份信息
     */
    protected void _addHandleInfo(NPHandlerEntry<T> _handlerEntry)
    {
        synchronized (this)
        {
            //向数据集中添加数据
            _m_alHandlerList.add(_handlerEntry);
        }
    }

    /******************
     * 清空所有关联管理器中本对象holder的处理
     */
    protected void _clearHandler()
    {
        synchronized (this)
        {
            //全部注销
            for (NPHandlerEntry<T> info : _m_alHandlerList)
            {
                info.disposeHandler();
            }

            _m_alHandlerList.clear();
        }
    }

    /**
     * 移除指定entry
     * @param _handlerEntry
     */
    protected void removeEntry(NPHandlerEntry<T> _handlerEntry)
    {
        synchronized (this)
        {
            _m_alHandlerList.remove(_handlerEntry);
        }
    }

    @Override
    protected void finalize() throws Throwable
    {
        super.finalize();

        //被释放时检测是不是用手动的方式释放的
        if (!_m_bOptUnReg)
        {
            System.out.println(" NPPlayerHolderHandlerMap 有人忘了反注册handler,垃圾回收正在帮你回收了,建议检查逻辑");
            System.out.println("相关处理类：" + _m_clazz.getName());
            for (NPHandlerEntry<T> tnpHandlerEntry : _m_alHandlerList)
            {
                System.out.println("相关eventId：" + tnpHandlerEntry.getEventId());
            }
            try
            {
                //这里只是保险措施，不能依赖 gc 时的反注册。
                _clearHandler();
            } catch (Exception e)
            {
                CommLog.error("", e);
            }
        }
    }

}
