package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import NP2US_R.p010_MarsMineOp.NP2US_R_010_004_ReqLeaveMarsMine;
import NPCommon.ErrMain.Result.Result;
import NPServerProtocolWriter.NP2US.RequestBack.NP2US_RB_Writer_010_MarsMineOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 离开火星矿请求处理器
 *
 * 功能：处理跨服离开火星矿的请求
 */
public class NPUSGeneralRequest_010_004_ReqLeaveMarsMine
	extends _ABasicGeneralRequestDealer<NP2US_R_010_004_ReqLeaveMarsMine>
{
	public NPUSGeneralRequest_010_004_ReqLeaveMarsMine(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                NP2US_R_010_004_ReqLeaveMarsMine _msg)
	{
		//处理离开火星矿
		Result result = getUSServer().getMarsMineCore().leaveMine(
			_msg.getInstanceId(),
			_msg.getCid(),
			_msg.getTeamId()
		);

		if (!result.isSucc())
		{
			//操作失败
			_commiter.commitFailRes(result.getCode());
			return;
		}

		//返回成功
		_commiter.commitSucRes(NP2US_RB_Writer_010_MarsMineOp.make_004_RetLeaveMarsMine());
	}
}
