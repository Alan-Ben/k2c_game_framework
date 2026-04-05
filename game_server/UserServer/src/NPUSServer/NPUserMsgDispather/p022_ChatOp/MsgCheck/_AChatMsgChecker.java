package NPUSServer.NPUserMsgDispather.p022_ChatOp.MsgCheck;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPChatMsgType;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.nio.ByteBuffer;

public abstract class _AChatMsgChecker<FROM extends _IALProtocolStructure, TO extends _IALProtocolStructure>
{
    public abstract ENPChatMsgType getType();

    public abstract FROM buildMsgInstance();

    public FROM parseMsg(byte[] _msg)
    {
        FROM msg = buildMsgInstance();
        msg.readPackage(ByteBuffer.wrap(_msg));
        return msg;
    }

    public ResultOne<byte[]> checkMsg(NPUSUserData _userdata, byte[] _msg)
    {
        FROM msg = parseMsg(_msg);

        //区分结果
        ResultOne<TO> checkResult = _checkMsg(_userdata, msg);
        if (!checkResult.isSucc())
            return new ResultOne<>(checkResult.getCode(), checkResult.getMsg(), null);

        return new ResultOne<>(checkResult.getCode(), checkResult.getMsg(), checkResult.getData().makePackage().array());
    }

    protected abstract ResultOne<TO> _checkMsg(NPUSUserData _userdata, FROM _msg);
}
