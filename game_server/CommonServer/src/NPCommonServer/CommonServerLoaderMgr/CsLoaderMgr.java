package NPCommonServer.CommonServerLoaderMgr;

import NPCommon.SerialLoader._AAsyncSerialLoadMgr;
import NPCommon.ServerLoader.ExampleLoader;

public class CsLoaderMgr extends _AAsyncSerialLoadMgr
{
    public CsLoaderMgr()
    {
        registLoader(new ExampleLoader());
    }
}
