package NPCommonServer.CachedShareData;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.CommonCache.ComCacheLoaderBase;
import NPCommon.CommonCache.ComLoadHandler;
import NPCommonServer.NPCommonServer;
import ToSCS_R.p001_ShareCodeOp.ToSCS_R_001_002_GetShareData;
import ToSCS_RB.p001_ShareCodeOp.ToSCS_RB_001_002_GetShareData;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class CachedShareDataLoader extends ComCacheLoaderBase<Long, CachedShareData>
{
    private int _m_type;

    public CachedShareDataLoader(int _type, Long _key)
    {
        super(_key);

        _m_type = _type;
    }

    @Override
    public boolean _isExpired()
    {
        return false;
    }

    @Override
    public long _getLoadTimeoutMs()
    {
        return 10 * 1000L;
    }

    @Override
    protected void _doAsyncLoad(ComLoadHandler<Long, CachedShareData> handler)
    {
        ToSCS_R_001_002_GetShareData proto = new ToSCS_R_001_002_GetShareData();
        proto.setType(_m_type);
        proto.setSerial(getkey());
        NPCommonServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SHARE_CODE.ordinal(), proto,
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new ToSCS_RB_001_002_GetShareData();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        ToSCS_RB_001_002_GetShareData retMsg = (ToSCS_RB_001_002_GetShareData) _retMsg;
                        //回调处理
                        handler.handle(true, new CachedShareData(retMsg.getData()));
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        //回调处理
                        handler.handle(false, null);
                    }
                });
    }

    @Override
    public void callbackOnRemoved()
    {

    }
}
