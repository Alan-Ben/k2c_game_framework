package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildRecalMemberBuilding_2C_Req;
import ALLRPC.US.Guild.GuildRecalMemberBuilding_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;

import java.nio.ByteBuffer;

@ARpc(comment =" 公会成员重新计算建筑赚速")
public class GuildRecalMemberBuilding_2C extends _AGuildBroadCastRPC<GuildRecalMemberBuilding_2C_Req, GuildRecalMemberBuilding_2C_Return>
{
	//临时读取数据对象，避免重复创建再读取
	private ByteBuffer _m_tmpBuf = null;
	//设置发送的Cid对象
	@Override
	public GuildRecalMemberBuilding_2C cloneRPCForCid(long _cid)
	{
		GuildRecalMemberBuilding_2C rpc = new GuildRecalMemberBuilding_2C();
		if(null == _m_tmpBuf)
			_m_tmpBuf = this.req().makePackage();

		//重置到开始位置
		_m_tmpBuf.position(0);

		//直接读取
		rpc.req().readPackage(_m_tmpBuf);

		//设置Cid
		rpc.req().setCid(_cid);

		return rpc;
	}

	@Override
	protected GuildRecalMemberBuilding_2C_Req createRequest()
	{
		return new GuildRecalMemberBuilding_2C_Req();
	}

	@Override
	protected GuildRecalMemberBuilding_2C_Return createResponse()
	{	
		return new GuildRecalMemberBuilding_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildRecalMemberBuilding_2C.ordinal();
	}
}
