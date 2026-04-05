package NPCommon.Util.Delegate;

public class GlobalADelegateOne<T> extends ADelegateOne<T>
{

    public GlobalADelegateOne(Object _parent)
    {
        super(_parent);
        setGlobal(true);
    }
}
