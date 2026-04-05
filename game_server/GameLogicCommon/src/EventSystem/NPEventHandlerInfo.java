package EventSystem;

import NPCommon.Util.Delegate.ADelegateTwo;
import NPCommon.Util.Delegate.HandlerEntryBase;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.LogicEvent._ALogicEventBase;

/******
 * 事件管理对象，包括 注册监听对象，注销监听对象，执行响应处理
 * 根据事件类型进行的管理，本对象后续可能扩展不同主体的回调处理
 * @author mark
 *
 */
public class NPEventHandlerInfo<T>
{
    //回调管理对象，本对象内部有对回调数据集进行多线程管理。所以这里不需要额外加锁进行管理
    private ADelegateTwo<_ALogicEventBase, T> _m_dDelegate;

    protected NPEventHandlerInfo()
    {
        _m_dDelegate = new ADelegateTwo<>(this);
    }

    /**
     * 注册回调
     * @param _holder 发起监听对象
     * @param _handler 回调信息
     * @return 监听注册信息
     */
    protected HandlerEntryBase _regDelegate(_IHandlerHolder _holder, HandlerTwo<_ALogicEventBase, T> _handler)
    {
        return _m_dDelegate.addHandler(_holder, _handler);
    }

    /**
     * 注销回调
     * @param _holder 发起监听对象
     */
    protected void _unregDelegate(_IHandlerHolder _holder)
    {
        _m_dDelegate.clear(_holder);
    }

    /**
     * 处理事件
     * @param _evt 事件信息
     * @param _t1 触发事件的对象
     */
    protected void _handle(_ALogicEventBase _evt, T _t1)
    {
        if (null == _evt)
            return;

        //处理事件
        _m_dDelegate.onEvent(_evt, _t1);
    }

    /**
     * 注销处理对象
     * @param _handlerEntry 监听注册信息
     * @return 是否成功移除
     */
    protected boolean unregHandler(HandlerEntryBase _handlerEntry)
    {
        return _m_dDelegate.removeEntry(_handlerEntry);
    }
}
