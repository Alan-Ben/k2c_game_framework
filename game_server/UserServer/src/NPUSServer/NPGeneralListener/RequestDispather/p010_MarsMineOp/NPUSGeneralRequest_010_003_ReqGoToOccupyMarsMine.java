package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import NP2US_R.p010_MarsMineOp.NP2US_R_010_003_ReqGoToOccupyMarsMine;
import NPCommon.ErrMain.Result.Result;
import NPServerProtocolWriter.NP2US.RequestBack.NP2US_RB_Writer_010_MarsMineOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 前往占领火星矿请求处理器
 *
 * 功能：处理跨服添加火星矿攻击行动的请求
 */
public class NPUSGeneralRequest_010_003_ReqGoToOccupyMarsMine
	extends _ABasicGeneralRequestDealer<NP2US_R_010_003_ReqGoToOccupyMarsMine>
{
	public NPUSGeneralRequest_010_003_ReqGoToOccupyMarsMine(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                NP2US_R_010_003_ReqGoToOccupyMarsMine _msg)
	{
		//添加攻击行动
		Result result = getUSServer().getMarsActionCore().getMineActionMgr().addAttackAction(
			_msg.getInstanceId(),
			_msg.getIsOtherTeamForward(),
			_msg.getOccupyPlayer(),
			_msg.getStartCollectMs(),
			_msg.getCollectSpeed()
		);

		if (!result.isSucc())
		{
			//操作失败
			_commiter.commitFailRes(result.getCode());
			return;
		}

		//返回成功
		_commiter.commitSucRes(NP2US_RB_Writer_010_MarsMineOp.make_003_RetGoToOccupyMarsMine());
	}
}
