package NPUSServer.UserServerLoaderMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_007_RetUSInfoListByTypeId;
import NPCommon.SerialLoader._IAsyncSingleLoadHandler;
import NPCommon.SerialLoader._IAsyncSingleLoader;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UserServerConf;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 获取服务器信息加载任务
 * @author: ricci
 * @date: 2023-03-24 09:57:34
 */
public class UsOnlineStateSyncLoader implements _IAsyncSingleLoader
{
    private NPUserServer _m_server;

    public UsOnlineStateSyncLoader(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void asyncLoad(_IAsyncSingleLoadHandler _callback)
    {
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_007_ReqUSInfoListByTypeId(getUSServer().getServerTypeId())
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_007_RetUSInfoListByTypeId();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_007_RetUSInfoListByTypeId proto = (NP2CS_RB_002_007_RetUSInfoListByTypeId) _proto;

                        getUSServer().setUsState(proto.getServerItem().getOnlineStateTypeId(),
                                proto.getServerItem().getShowStateTypeId(), proto.getServerItem().getStartDate());

                        _callback.onSingleLoadOver(true);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(_m_server, "NPUserServer UsOnlineStateSyncLoader deal fail err:{}", _errCode);
                        //通过配置区分是否需要依赖CS开启,如果不需要的话,直接返回成功
                        _callback.onSingleLoadOver(!UserServerConf.getInstance().getNeedRelyCSOpen());
                    }
                });
    }
}
