package NPServerProtocolWriter.NP2US.Request;

import NP2US_R.p001_BasicOp.NP2US_R_001_002_ChgGeneral;
import NP2US_R.p001_BasicOp.NP2US_R_001_003_ChgRef;

public class NP2US_R_Writer_001_BasicOp
{
    public static NP2US_R_001_002_ChgGeneral make_002_ChgGeneral(String _key, String _value)
    {
        NP2US_R_001_002_ChgGeneral protocol = new NP2US_R_001_002_ChgGeneral();

        protocol.setKey(_key);
        protocol.setValue(_value);

        return protocol;
    }

    public static NP2US_R_001_003_ChgRef make_003_ChgRef(String _tableName, String _id, String _key, String _value)
    {
        NP2US_R_001_003_ChgRef protocol = new NP2US_R_001_003_ChgRef();

        protocol.setTableName(_tableName);
        protocol.setId(_id);
        protocol.setKey(_key);
        protocol.setValue(_value);

        return protocol;
    }
}
