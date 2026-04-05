package <%ClassPackage%>;

import <%ProtoPackage%>.<%ClassName%>_Req;
import <%ProtoPackage%>.<%ClassName%>_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="<%ClassComment%>")
public class <%ClassName%> extends _ARPCBase<<%ClassName%>_Req, <%ClassName%>_Return>
{

	@Override
	protected <%ClassName%>_Req createRequest()
	{
		return new <%ClassName%>_Req();
	}

	@Override
	protected <%ClassName%>_Return createResponse()
	{	
		return new <%ClassName%>_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.<%ClassName%>.ordinal();
	}
}
