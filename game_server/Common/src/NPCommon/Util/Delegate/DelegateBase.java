package NPCommon.Util.Delegate;


import NPCommon.Util.CommFix;
import NPCommon.Util.Delegate.DelegateStat.DelegateStatMgr;

import java.lang.reflect.Field;
import java.util.List;
import java.util.concurrent.atomic.AtomicLong;

/*****
 * 监听Delegate的基类，提供一般操作
 */
public abstract class DelegateBase
{
    private static AtomicLong _g_Id_creator = new AtomicLong(); //生成唯一的Delegate ID
    private long _m_Id; //代理对象的唯一Id
    private Object _m_parent;//父对象
    private boolean _m_bIsGlobal;//是否全局代理对象

    public DelegateBase(Object _parent)
    {
        _m_parent = _parent;
        _m_Id = _g_Id_creator.incrementAndGet();
        DelegateStatMgr.getInstance().regist(this);
    }


    public Object getParent()
    {
        return _m_parent;
    }

    public boolean isGlobal()
    {
        return _m_bIsGlobal;
    }

    public void setGlobal(boolean _isGlobal)
    {
        _m_bIsGlobal = _isGlobal;
    }

    public abstract int getHandlerCount(); //返回Handler数量

    public abstract List<HandlerEntryBase> getEntryList(); //返回调用入口列表

    public abstract int clear(); //清除全部Handler

    public abstract void clear(_IHandlerHolder _holder);//指定Holder，清除所属的handler

    public abstract boolean removeEntry(HandlerEntryBase _entry); //移除指定entry

    public long getId()
    {
        return _m_Id;
    } //返回唯一id


    /******
     * 清除指定的handler，
     * @param _matchName 匹配的名字，null为不判断
     * @param _lessCreateTime 小于指定创建时间，0为不判断
     * @return
     */
    public int clearHandler(String _matchName, int _lessCreateTime)
    {
        if (_matchName.compareToIgnoreCase("null") == 0 && _lessCreateTime == 0)
            return clear();
        List<HandlerEntryBase> entryList = getEntryList();
        int count = 0;
        for (HandlerEntryBase entry : entryList)
        {
            if (entry.getHandler().isMatchName(_matchName) || _matchName.compareToIgnoreCase("null") == 0)
            {
                if (0 == _lessCreateTime || entry.getAddTimeSec() <= _lessCreateTime)
                {
                    if (removeEntry(entry))
                        count++;
                }
            }
        }
        return count;

    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        Object parent = getParent();
        sb.append(getClass().getSimpleName());
        if (null == parent)
        {
            sb.append("@[null]");
        } else
        {
            sb.append("@[" + getVariableName() + "]");
        }
        sb.append(" handlers:[" + getHandlerCount() + "]");

        return sb.toString();
    }

    /*****
     * 如果本Delegate是个对象的成员，返回该成员的名称
     * @return
     */
    public String getVariableName()
    {
        Object parent = getParent();
        if (null == parent)
            return "NULL";

        Field fieldObj = CommFix.getFieldByObj(parent, this);
        if (null == fieldObj)
            return parent.getClass().getSimpleName();
        else
            return parent.getClass().getSimpleName() + "." + fieldObj.getName();
    }
}
