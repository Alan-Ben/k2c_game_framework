package NPServerProtocolWriter.NP2LBS.Request;

import WCGPS2LBS.p001_BasicOp.WCGPS2LBS_001_001_LogServerChanged;

public class WCGPS2LBS_Writer_001_BasicOp
{
    public static WCGPS2LBS_001_001_LogServerChanged make_001_AddLogServer(int _serverTypeID)
    {
        WCGPS2LBS_001_001_LogServerChanged protocol = new WCGPS2LBS_001_001_LogServerChanged();
        protocol.setServerTypeId(_serverTypeID);
        protocol.setAddOrRemoved(true);
        return protocol;
    }

    public static WCGPS2LBS_001_001_LogServerChanged make_001_RemoveLogServer(int _serverTypeID)
    {
        WCGPS2LBS_001_001_LogServerChanged protocol = new WCGPS2LBS_001_001_LogServerChanged();
        protocol.setServerTypeId(_serverTypeID);
        protocol.setAddOrRemoved(false);
        return protocol;
    }

}
