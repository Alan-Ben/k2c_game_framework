package NPServerProtocolWriter.NP2CS.Msg;

import NP2CS.p001_BasicOp.*;

public class NP2CS_Writer_001_BasicOp
{
    public static NP2CS_001_001_AddRoomServer make_001_AddRoomServer(int _serverTypeId)
    {
        NP2CS_001_001_AddRoomServer protocol = new NP2CS_001_001_AddRoomServer();
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_002_RomoveRoomServer make_002_RomoveRoomServer(int _serverTypeId)
    {
        NP2CS_001_002_RomoveRoomServer protocol = new NP2CS_001_002_RomoveRoomServer();
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_003_AddCrossRankServer make_003_AddCrossRankServer(int _serverTypeId)
    {
        NP2CS_001_003_AddCrossRankServer protocol = new NP2CS_001_003_AddCrossRankServer();
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_006_AddCrossGameServer make_006_AddCrossGameServer(int _serverTypeId)
    {
        NP2CS_001_006_AddCrossGameServer protocol = new NP2CS_001_006_AddCrossGameServer();
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_007_RomoveCrossGameServer make_007_RomoveCrossGameServer(int _serverTypeId)
    {
        NP2CS_001_007_RomoveCrossGameServer protocol = new NP2CS_001_007_RomoveCrossGameServer();
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_010_AddHandleServer make_010_AddHandleServer(int _serverType, int _serverTypeId)
    {
        NP2CS_001_010_AddHandleServer protocol = new NP2CS_001_010_AddHandleServer();
        protocol.setServerType(_serverType);
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_001_011_RemoveHandleServer make_011_RemoveHandleServer(int _serverType, int _serverTypeId)
    {
        NP2CS_001_011_RemoveHandleServer protocol = new NP2CS_001_011_RemoveHandleServer();
        protocol.setServerType(_serverType);
        protocol.setServerTypeId(_serverTypeId);
        return protocol;
    }
}
