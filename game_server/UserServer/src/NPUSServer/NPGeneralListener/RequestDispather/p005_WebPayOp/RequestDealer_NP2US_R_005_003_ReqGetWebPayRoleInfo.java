package NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp;

import Common.ServerObj.ServerObj_WebPayRoleInfo;
import NP2US_R.p005_WebPayOp.NP2US_R_005_003_ReqGetWebPayRoleInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.Delegate.HandlerTwo;
import NPServerProtocolWriter.NP2US.Response.NP2US_RB_Writer_005_WebPayOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 获取网页支付角色信息请求处理器
 *
 * 处理来自HttpServer的角色信息查询请求
 */
public class RequestDealer_NP2US_R_005_003_ReqGetWebPayRoleInfo extends _ABasicGeneralRequestDealer<NP2US_R_005_003_ReqGetWebPayRoleInfo>
{
	public RequestDealer_NP2US_R_005_003_ReqGetWebPayRoleInfo(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2US_R_005_003_ReqGetWebPayRoleInfo _msg)
	{
        getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _msg.getCid(), new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _showInfo)
            {
                if (!_isSuc)
                {
                    //做失败处理
                    _commiter.commitFailRes(CommErr.PLAYER_CACHE_ERR.getCode());
                    return;
                }

                ServerObj_WebPayRoleInfo roleInfo = new ServerObj_WebPayRoleInfo();
                roleInfo.setCid(_showInfo.getCid());
                roleInfo.setLvl((int) _showInfo.getPlayerLvl());
                roleInfo.setName(_showInfo.getPlayerName());
                roleInfo.setVipLvl((int) _showInfo.getVipLvl());

                getUSServer().getUserIdxMgr().lookupCidLinkedUid(_msg.getCid(), new _ICallBackResultT<String>()
                {
                    @Override
                    public void onRunOver(Result _result, String _uid)
                    {
                        if (_result.isSucc())
                            roleInfo.setUid(_uid);

                        _commiter.commitSucRes(NP2US_RB_Writer_005_WebPayOp.make_003_RetGetWebPayRoleInfo(roleInfo));
                    }
                });
            }
        });
	}
}
