package NPCommonServer.CachedShareData;

import NPCommon.CommonCache.ComCachedDataBase;

public class CachedShareData extends ComCachedDataBase
{
    private byte[] _m_data;

    public CachedShareData(byte[] _data)
    {
        _m_data = _data;
    }

    public byte[] getData()
    {
        return _m_data;
    }
}
