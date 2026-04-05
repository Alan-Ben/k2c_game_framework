package NPCommon.Util.Delegate;


import NPCommon.Util.CommonFunc;

import java.lang.ref.WeakReference;

/****
 * 监听入口的基类
 */
public abstract class HandlerEntryBase
{
    private int _m_addTimeSec; //加入delegate的时间
    private WeakReference<_IHandlerHolder> _m_holder;//handler被谁持有

    public _IHandlerHolder getHolder()
    {
        return _m_holder.get();
    }

    public void setHolder(_IHandlerHolder _holder)
    {
        _m_holder = new WeakReference<>(_holder);
    }

    public int getAddTimeSec()
    {
        return _m_addTimeSec;
    }

    public void setAddTime(int _addTimeSec)
    {
        _m_addTimeSec = _addTimeSec;
    }

    public abstract HandlerBase getHandler();

    @Override
    public String toString()
    {
        return String.format("addTime[%s]%d |handler:[%s] | holder:%s"
                , CommonFunc.getTimeStringMs(getAddTimeSec() * 1000L)
                , getAddTimeSec()
                , getHandler().toString()
                , getHolder() == null ? "null" : getHolder().toString());

    }

}
