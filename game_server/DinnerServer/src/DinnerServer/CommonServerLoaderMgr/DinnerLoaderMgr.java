package DinnerServer.CommonServerLoaderMgr;

import NPCommon.SerialLoader._AAsyncSerialLoadMgr;
import NPCommon.ServerLoader.ExampleLoader;

public class DinnerLoaderMgr extends _AAsyncSerialLoadMgr
{
    public DinnerLoaderMgr()
    {
        registLoader(new ExampleLoader());
    }
}
