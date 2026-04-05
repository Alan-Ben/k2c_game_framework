package NPCommon.CommonCache.Player.Getter.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NP2US_RB.p006_CacheOp.NP2US_RB_006_001_GetPlayerShowInfo;
import NPCommon.CommonCache.GetterBase._ACacheGetterDealer;
import NPCommon.CommonCache.Player.Getter.PlayerCacheGetter;
import NPCommon.CommonCache.Player.Getter._IPlayerCacheGetterEnv;
import NPCommon.CommonCache.Player._ACachedPlayerInfo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_006_CacheOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

public class PlayerCommonShowGetterDealer extends _ACacheGetterDealer<_ACachedPlayerInfo, _IPlayerCacheGetterEnv, PlayerInfo_CommonShow>
{
    public PlayerCommonShowGetterDealer(PlayerCacheGetter _getter)
    {
        super(_getter);
    }

    @Override
    public void getInfo(long _cid, HandlerTwo<Boolean, PlayerInfo_CommonShow> _handler)
    {
        //获取目标玩家所在UsId
        int typeId = CommonFunc.parseServerTypeIdFromCid(_cid);
        //如果是本地服务器，直接获取数据
        if (getServer().getServerType() == EServerType.USER.ordinal() && getServer().getServerTypeId() == typeId)
        {
            getEnv().getData(_cid, new HandlerTwo<Boolean, _ACachedPlayerInfo>()
            {
                @Override
                public void handle(Boolean _suc, _ACachedPlayerInfo _data)
                {
                    if (_suc && _data != null)
                    {
                        PlayerInfo_CommonShow commonShow = _data.makeShowInfo();
                        //玩家的点赞次数
                        commonShow.setBeLikeCount(getEnv().getLikeCount(_cid));
                        _handler.handle(true, commonShow);
                    } else
                    {
                        _handler.handle(false, null);
                    }
                }
            });
        } else
        {
            getServer().sendRequestToBSServer(EServerType.USER.ordinal(), typeId,
                    NP2US_R_Writer_006_CacheOp.make_001_GetPlayerShowInfo(_cid), new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_006_001_GetPlayerShowInfo();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _protocol)
                        {
                            NP2US_RB_006_001_GetPlayerShowInfo ret = (NP2US_RB_006_001_GetPlayerShowInfo) _protocol;
                            _handler.handle(true, ret.getShowInfo());
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            _handler.handle(false, null);
                        }
                    });
        }
    }
}
