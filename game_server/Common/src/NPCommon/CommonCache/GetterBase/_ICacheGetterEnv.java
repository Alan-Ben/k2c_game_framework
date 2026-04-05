package NPCommon.CommonCache.GetterBase;

import NPCommon.CommonCache.ComCachedDataBase;
import NPCommon.Util.Delegate.HandlerTwo;

public interface _ICacheGetterEnv<T extends ComCachedDataBase>
{
    void getData(long _key, HandlerTwo<Boolean, T> _handler);
}
