package NPCommonServer.CachedShareData;

import NPCommon.CommonCache.ComCacheMgrBase;
import NPCommon.Util.Delegate._IHandlerHolder;

public class CachedShareDataMgr extends ComCacheMgrBase<Long, CachedShareData, CachedShareDataLoader> implements _IHandlerHolder
{
    private int _m_type;

    public CachedShareDataMgr(int _type)
    {
        _m_type = _type;
    }

    @Override
    public int getUnloadCheckSpanSec()
    {
        return 10;
    }

    @Override
    protected int getLoaderExpiredSec()
    {
        return 10 * 60;
    }

    @Override
    protected CachedShareDataLoader createLoader(Long _key)
    {
        return new CachedShareDataLoader(_m_type, _key);
    }
}
