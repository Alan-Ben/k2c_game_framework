package MGClient.ClientRequestMgr;

import MGClient.ClientPlayer.ClientPlayer;

import java.nio.ByteBuffer;

public interface _IClientRequestHandler
{
    void handleResponse(int errCode, ByteBuffer bytes);

    void setCaller(ClientPlayer _player);
}
