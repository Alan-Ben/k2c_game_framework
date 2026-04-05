package ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.p001_ShareCodeOp;

import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.Writer.ToSCS_RB_Writer_001_ShareCodeOp;
import ShareCodeCenter.ShareDataMgr._AShareDataMgr;
import ToSCS_R.p001_ShareCodeOp.ToSCS_R_001_001_GenShareCode;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_ToSCS_R_001_001_GenShareCode extends NPRequestDealer<ToSCS_R_001_001_GenShareCode>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToSCS_R_001_001_GenShareCode _msg)
    {
        _AShareDataMgr<?> shareDataMgr = _AShareDataMgr.getShareDataMgr(_msg.getType());
        if (shareDataMgr == null)
        {
            _receiver.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        _receiver.commitSucRes(ToSCS_RB_Writer_001_ShareCodeOp.make_001_GenShareCode(shareDataMgr.createData(_msg.getData())));
    }
}
