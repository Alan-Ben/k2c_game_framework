package GameLogicServer.Common;

import GameLogicServer.GameLogicServer;

public class GLS_ID
{
    public static long makeInstanceId(long _groupId)
    {
        //_groupId*100 + 2位服务器id
        return _groupId * 100 + GameLogicServer.getInstance().getServerTypeId();
    }
}
