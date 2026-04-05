package NPUSServer.CrossGameSys.CallbackDealer;

import java.nio.ByteBuffer;

@FunctionalInterface
public interface _ICallBack_GCCreateInstanceHandle
{
    void onRunOver(int _errCode, ByteBuffer _ret);
}
