package NPCommon.Util.Delegate;

public class GlobalADelegateFour<T1, T2, T3, T4> extends ADelegateFour<T1, T2, T3, T4>
{

    public GlobalADelegateFour(Object _parent)
    {
        super(_parent);
        setGlobal(true);
    }
}
