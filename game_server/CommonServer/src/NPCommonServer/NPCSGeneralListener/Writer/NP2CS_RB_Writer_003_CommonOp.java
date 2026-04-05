package NPCommonServer.NPCSGeneralListener.Writer;

import NP2CS_RB.p003_CommonOp.ToCS_RB_003_001_GetShareData;

public class NP2CS_RB_Writer_003_CommonOp
{
    public static ToCS_RB_003_001_GetShareData make_001_RetSpaceSerialize(byte[] _data)
    {
        ToCS_RB_003_001_GetShareData protocol = new ToCS_RB_003_001_GetShareData();
        protocol.setData(_data);
        return protocol;
    }
}
