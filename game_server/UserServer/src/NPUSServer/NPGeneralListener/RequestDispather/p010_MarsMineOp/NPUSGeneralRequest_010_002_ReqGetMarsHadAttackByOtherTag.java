package NPUSServer.NPGeneralListener.RequestDispather.p010_MarsMineOp;

import NP2US_R.p010_MarsMineOp.NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag;
import NPCommon.ErrMain.MarsErr;
import NPServerProtocolWriter.NP2US.RequestBack.NP2US_RB_Writer_010_MarsMineOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.UsMars.MineCore.UsMarsMineObj;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 查询火星矿是否被其他公会攻击过请求处理器
 *
 * 功能：处理跨服查询火星矿攻击记录的请求
 */
public class NPUSGeneralRequest_010_002_ReqGetMarsHadAttackByOtherTag
	extends _ABasicGeneralRequestDealer<NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag>
{
	public NPUSGeneralRequest_010_002_ReqGetMarsHadAttackByOtherTag(NPUserServer _server)
	{
		super(_server);
	}

	@Override
	protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag _msg)
	{
		//查找矿数据
		UsMarsMineObj usMine = getUSServer().getMarsMineCore().lookupData(_msg.getInstanceId());

		if (null == usMine)
		{
			//矿不存在
			_commiter.commitFailRes(MarsErr.MARS_MINE_NOT_FOUND.getCode());
			return;
		}

		//查询是否被其他公会攻击过
		boolean hasAttacked = usMine.hasOtherGuildAttacked(_msg.getGuildId());
		_commiter.commitSucRes(NP2US_RB_Writer_010_MarsMineOp.make_002_RetGetMarsHadAttackByOtherTag(hasAttacked));
	}
}
