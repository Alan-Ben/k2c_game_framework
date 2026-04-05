package NPUSServer.GMCommand;

import NPCommon.GMCommand.CmdClassBase;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;


public abstract class UsCmdBase extends CmdClassBase
{
	 public NPUserServer getUserServer()
    {
        if (getExecutor() instanceof UsCmdServerExecutor)
        {
            return ((UsCmdServerExecutor) getExecutor()).getUsServer();
        }else if (getExecutor() instanceof UsCmdPlayerExecutor)
        {
            return ((UsCmdPlayerExecutor) getExecutor()).getUserData().getUSServer();
        }else
        {
            return null;
        }
    }
	
    public NPUSUserData getOwner()
    {
        if (getExecutor() instanceof UsCmdPlayerExecutor)
        {
            return ((UsCmdPlayerExecutor) getExecutor()).getUserData();
        }
        return null;
    }

    public NPPlayerContext getContext()
    {
        if (getIContext() instanceof NPPlayerContext)
        {
            return (NPPlayerContext) getIContext();
        }
        return null;
    }

}
