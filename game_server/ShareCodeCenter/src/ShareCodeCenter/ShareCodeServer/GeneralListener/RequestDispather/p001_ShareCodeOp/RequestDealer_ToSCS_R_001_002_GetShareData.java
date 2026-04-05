package ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.p001_ShareCodeOp;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.Delegate.HandlerTwo;
import ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.Writer.ToSCS_RB_Writer_001_ShareCodeOp;
import ShareCodeCenter.ShareDataMgr._AShareDataMgr;
import ToSCS_R.p001_ShareCodeOp.ToSCS_R_001_002_GetShareData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_ToSCS_R_001_002_GetShareData extends NPRequestDealer<ToSCS_R_001_002_GetShareData>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToSCS_R_001_002_GetShareData _msg)
    {
        _AShareDataMgr<?> shareDataMgr = _AShareDataMgr.getShareDataMgr(_msg.getType());
        if (shareDataMgr == null)
        {
            _receiver.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        shareDataMgr.loadData(_msg.getSerial(), new HandlerTwo<Boolean, _IALProtocolStructure>()
        {
            @Override
            public void handle(Boolean _isSucc, _IALProtocolStructure _data)
            {
                if (_isSucc)
                {
                    _receiver.commitSucRes(ToSCS_RB_Writer_001_ShareCodeOp.make_002_GetShareData(_data.makePackage().array()));
                }
                else
                {
                    _receiver.commitFailRes(CommErr.SYS_ERR.getCode());
                }
            }
        });
    }
}
