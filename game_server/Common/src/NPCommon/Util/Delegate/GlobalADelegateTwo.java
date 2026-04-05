package NPCommon.Util.Delegate;

public class GlobalADelegateTwo<T1, T2> extends ADelegateTwo<T1, T2>
{

    public GlobalADelegateTwo(Object _parent)
    {
        super(_parent);
        setGlobal(true);
    }
}
