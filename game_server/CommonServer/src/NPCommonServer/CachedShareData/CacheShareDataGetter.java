package NPCommonServer.CachedShareData;

import CommonEnum.EShareCodeType;

public class CacheShareDataGetter
{
    private static final CacheShareDataGetter _g_instance = new CacheShareDataGetter();
    public static CacheShareDataGetter getInstance()
    {
        return _g_instance;
    }

    private final CachedShareDataMgr[] _m_cachedMgrList;

    public CacheShareDataGetter()
    {
        _m_cachedMgrList = new CachedShareDataMgr[EShareCodeType.EShareCodeType_Length];
        for (int i = 0; i < EShareCodeType.EShareCodeType_Length; i++)
        {
            _m_cachedMgrList[i] = new CachedShareDataMgr(i);
        }
    }

    public CachedShareDataMgr getCachedMgr(int _type)
    {
        return _m_cachedMgrList[_type];
    }
}
