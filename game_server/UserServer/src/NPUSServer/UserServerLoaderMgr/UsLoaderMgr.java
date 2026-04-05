package NPUSServer.UserServerLoaderMgr;

import NPCommon.SerialLoader._AAsyncSerialLoadMgr;
import NPUSServer.NPUserServer;

public class UsLoaderMgr extends _AAsyncSerialLoadMgr
{
    public UsLoaderMgr(NPUserServer _server)
    {
        super();

        registLoader(new UsOnlineStateSyncLoader(_server));
    }
}
