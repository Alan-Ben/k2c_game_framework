package NPPlatServer.NPGeneralListener.RequsetDispather;

import NP2PS_R.p100_SysOp.NP2PS_R_100_001_GetAreaSerial;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.PSErr;
import NPPlatServer.NPGeneralListener.Writer.NP2PS_RB_Writer_100_SysOp;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaMgr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPPSGeneral_100_RequestDispather extends NPRequestDispatcher
{
    public static void init(NPPSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_100_001_GetAreaSerial>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_100_001_GetAreaSerial _msg)
            {
                NPPlatAreaInfo area = NPPlatAreaMgr.getInstance().lookupAreaInfoByName(_msg.getAreaTag().trim());
                if (area == null)
                {
                    _committer.commitFailRes(PSErr.PS_NO_AREA.getCode());
                } else
                {
                    _committer.commitSucRes(NP2PS_RB_Writer_100_SysOp.make_001_RetAreaSerialSuc(area.getAreaSerialize()));
                }
            }
        });
    }
}
