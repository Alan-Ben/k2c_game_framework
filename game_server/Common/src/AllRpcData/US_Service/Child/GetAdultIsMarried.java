package AllRpcData.US_Service.Child;

import AllRpcData.ERpcClassName;
import GC2GS.p014_ChildOp.GC2GS_014_030_ReqCidAdultIsMarried;
import GS2GC.p014_ChildOp.GS2GC_014_030_RetCidAdultIsMarried;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment =" 获取联姻池指定子嗣数据")
public class GetAdultIsMarried extends _ARPCBase<GC2GS_014_030_ReqCidAdultIsMarried, GS2GC_014_030_RetCidAdultIsMarried>
{

	@Override
	protected GC2GS_014_030_ReqCidAdultIsMarried createRequest()
	{
		return new GC2GS_014_030_ReqCidAdultIsMarried();
	}

	@Override
	protected GS2GC_014_030_RetCidAdultIsMarried createResponse()
	{	
		return new GS2GC_014_030_RetCidAdultIsMarried();
	}

	public int getClassId()
	{
		return ERpcClassName.GetAdultIsMarried.ordinal();
	}
}
