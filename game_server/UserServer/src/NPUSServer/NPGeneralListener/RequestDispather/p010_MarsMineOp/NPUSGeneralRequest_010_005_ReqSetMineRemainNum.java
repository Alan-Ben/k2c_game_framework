package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import NP2US_R.p010_MarsMineOp.NP2US_R_010_005_ReqSetMineRemainNum;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPServerProtocolWriter.NP2US.RequestBack.NP2US_RB_Writer_010_MarsMineOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 设置火星矿剩余资源数量请求处理器（GM命令）
 *
 * 功能：处理跨服设置火星矿剩余资源数量的请求
 */
public class NPUSGeneralRequest_010_005_ReqSetMineRemainNum
	extends _ABasicGeneralRequestDealer<NP2US_R_010_005_ReqSetMineRemainNum>
{
	public NPUSGeneralRequest_010_005_ReqSetMineRemainNum(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                NP2US_R_010_005_ReqSetMineRemainNum _msg)
	{
		//查找矿数据
		UsMarsMineObj usMine = getUSServer().getMarsMineCore().lookupData(_msg.getInstanceId());

		if (null == usMine)
		{
			//矿不存在
			_commiter.commitFailRes(MarsErr.MARS_MINE_NOT_FOUND.getCode());
			return;
		}

		//设置剩余资源数量
		Result result = usMine.gmSetRemainNum(_msg.getRemainNum());

		if (!result.isSucc())
		{
			//操作失败
			_commiter.commitFailRes(result.getCode());
			return;
		}

		//返回成功
		_commiter.commitSucRes(NP2US_RB_Writer_010_MarsMineOp.make_005_RetSetMineRemainNum());
	}
}
