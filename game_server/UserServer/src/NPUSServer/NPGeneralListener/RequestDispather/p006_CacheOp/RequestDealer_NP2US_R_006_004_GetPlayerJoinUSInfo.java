package NPUSServer.NPGeneralListener.RequestDispather.p006_CacheOp;

import NP2US_R.p006_CacheOp.NP2US_R_006_004_GetPlayerJoinUSInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_006_CacheOp;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2US_R_006_004_GetPlayerJoinUSInfo extends _ABasicGeneralRequestDealer<NP2US_R_006_004_GetPlayerJoinUSInfo>
{
    public RequestDealer_NP2US_R_006_004_GetPlayerJoinUSInfo(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_006_004_GetPlayerJoinUSInfo _msg)
    {
        PlayerCacheFunc.getData(getUSServer(), _msg.getCid(), new HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache>() {
            @Override
            public void handle(Boolean _isExist, UserOfflineTmpDataInfo_PlayerCache _cacheInfo) {
                if (!_isExist) {
                    _committer.commitFailRes(CommErr.PLAYER_CACHE_ERR.getCode());
                    return ;
                }

                if(null != _cacheInfo)
                {
                    _committer.commitSucRes(NP2US_RB_Writer_006_CacheOp.make_004_GetPlayerJoinUSInfo(_cacheInfo.makeJoinUSInfo()));
                }
                else
                {
                    _committer.commitFailRes(CommErr.PLAYER_CACHE_ERR.getCode());
                }
            }
        });
    }
}
