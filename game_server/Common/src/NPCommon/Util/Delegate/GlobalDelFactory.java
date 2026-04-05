package NPCommon.Util.Delegate;

/****
 * 全局监听的生成器
 */
public class GlobalDelFactory
{
    public static GlobalADelegateNone createNone(Object _parent)
    {
        return new GlobalADelegateNone(_parent);
    }

    public static <T> GlobalADelegateOne<T> createOne(Object _parent)
    {
        return new GlobalADelegateOne<>(_parent);
    }

    public static <T1, T2> GlobalADelegateTwo<T1, T2> createTwo(Object _parent)
    {
        return new GlobalADelegateTwo<>(_parent);
    }

    public static <T1, T2, T3> GlobalADelegateThree<T1, T2, T3> createThree(Object _parent)
    {
        return new GlobalADelegateThree<>(_parent);
    }

    public static <T1, T2, T3, T4> GlobalADelegateFour<T1, T2, T3, T4> createFour(Object _parent)
    {
        return new GlobalADelegateFour<>(_parent);
    }
}
