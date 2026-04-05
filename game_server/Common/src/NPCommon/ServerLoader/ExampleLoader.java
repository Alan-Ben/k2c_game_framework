package NPCommon.ServerLoader;

import NPCommon.SerialLoader._IAsyncSingleLoadHandler;
import NPCommon.SerialLoader._IAsyncSingleLoader;

public class ExampleLoader implements _IAsyncSingleLoader
{
    @Override
    public void asyncLoad(_IAsyncSingleLoadHandler _callback)
    {
        _callback.onSingleLoadOver(true);

    }
}
