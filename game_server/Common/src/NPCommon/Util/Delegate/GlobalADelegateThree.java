package NPCommon.Util.Delegate;

public class GlobalADelegateThree<T1, T2, T3> extends ADelegateThree<T1, T2, T3>
{

    public GlobalADelegateThree(Object _parent)
    {
        super(_parent);
        setGlobal(true);
    }

}
