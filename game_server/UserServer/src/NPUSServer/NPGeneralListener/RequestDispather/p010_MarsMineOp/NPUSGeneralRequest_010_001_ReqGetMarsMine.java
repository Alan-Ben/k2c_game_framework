package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import Common.ServerObj.ServerObj_MarsMine;
import NP2US_R.p010_MarsMineOp.NP2US_R_010_001_ReqGetMarsMine;
import NPCommon.ErrMain.MarsErr;
import NPServerProtocolWriter.NP2US.RequestBack.NP2US_RB_Writer_010_MarsMineOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 获取火星矿数据请求处理器
 *
 * 功能：处理跨服获取火星矿数据的请求
 */
public class NPUSGeneralRequest_010_001_ReqGetMarsMine
	extends _ABasicGeneralRequestDealer<NP2US_R_010_001_ReqGetMarsMine>
{
	public NPUSGeneralRequest_010_001_ReqGetMarsMine(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                NP2US_R_010_001_ReqGetMarsMine _msg)
	{
		//查找矿数据
		UsMarsMineObj usMine = getUSServer().getMarsMineCore().lookupData(_msg.getInstanceId());

		if (null == usMine)
		{
			//矿不存在
			_commiter.commitFailRes(MarsErr.MARS_MINE_NOT_FOUND.getCode());
			return;
		}

		//转换为协议对象并返回
		ServerObj_MarsMine marsMineData = usMine.toServerProto();
		_commiter.commitSucRes(NP2US_RB_Writer_010_MarsMineOp.make_001_RetGetMarsMine(marsMineData));
	}
}
