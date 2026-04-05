package NPUSServer.NPUserMsgDispather.p007_CommOp.ShareDataCheck;

import ALBasicProtocolPack._IALProtocolStructure;
import CommonEnum.EShareCodeType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.nio.ByteBuffer;

public abstract class _AShareDataChecker<F extends _IALProtocolStructure, T extends _IALProtocolStructure>
{
    public abstract EShareCodeType getType();

    public abstract F buildMsgInstance();

    public F parseMsg(byte[] _msg)
    {
        F msg = buildMsgInstance();
        msg.readPackage(ByteBuffer.wrap(_msg));
        return msg;
    }

    public ResultOne<T> checkAndTranMsg(NPUSUserData _userdata, byte[] _msg)
    {
        F msg;
        try
        {
            msg = parseMsg(_msg);
        } catch (Exception e)
        {
            USLog.error(_userdata.getUSServer(), "_AShareDataChecker parseMsg fail, cid:{}", _userdata.getCid(), e);
            return new ResultOne<>(CommErr.PARAM_ERROR, null);
        }

        return _checkAndTranMsg(_userdata, msg);
    }

    protected abstract ResultOne<T> _checkAndTranMsg(NPUSUserData _userdata, F _msg);
}
