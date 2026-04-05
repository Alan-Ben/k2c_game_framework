package NPUSServer.NPGeneralListener.RequestDispather.p006_CacheOp;

import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NP2US_R.p006_CacheOp.NP2US_R_006_001_GetPlayerShowInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_006_CacheOp;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2US_R_006_001_GetPlayerShowInfo extends _ABasicGeneralRequestDealer<NP2US_R_006_001_GetPlayerShowInfo>
{
    public RequestDealer_NP2US_R_006_001_GetPlayerShowInfo(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_006_001_GetPlayerShowInfo _msg)
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
                    PlayerInfo_CommonShow commonShow = _cacheInfo.makeShowInfo();
                    //添加玩家的点赞次数信息
                    commonShow.setBeLikeCount(getUSServer().getCollectLikeMgr().getLikeCount(_msg.getCid()));

                    _committer.commitSucRes(NP2US_RB_Writer_006_CacheOp.make_001_GetPlayerShowInfo(commonShow));
                }
                else
                {
                    _committer.commitFailRes(CommErr.PLAYER_CACHE_ERR.getCode());
                }
            }
        });
    }
}
