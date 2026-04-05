package ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.Writer;

import ToSCS_RB.p001_ShareCodeOp.ToSCS_RB_001_001_GenShareCode;
import ToSCS_RB.p001_ShareCodeOp.ToSCS_RB_001_002_GetShareData;

public class ToSCS_RB_Writer_001_ShareCodeOp
{
    public static ToSCS_RB_001_001_GenShareCode make_001_GenShareCode(long _serial)
    {
        ToSCS_RB_001_001_GenShareCode protocol = new ToSCS_RB_001_001_GenShareCode();
        protocol.setSerial(_serial);
        return protocol;
    }

    public static ToSCS_RB_001_002_GetShareData make_002_GetShareData(byte[] _data)
    {
        ToSCS_RB_001_002_GetShareData protocol = new ToSCS_RB_001_002_GetShareData();
        protocol.setData(_data);
        return protocol;
    }
    

}
