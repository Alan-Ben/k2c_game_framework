package MGClient.ClientRequestMgr;

import java.nio.ByteBuffer;

public abstract class ClientRequestBase
{
    public abstract void readResponse(ByteBuffer _buf);

    public abstract ByteBuffer getRequestBytes();
}
