package NPUSServer.NPUserMsgDispather.p007_CommOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p007_CommOp.GC2GS_007_005_ReqGenerateShareCode;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ShareCode.ShareCodeUtil;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.NPUserMsgDispather.p007_CommOp.ShareDataCheck.ShareDataCheckerMgr;
import ToSCS_R.p001_ShareCodeOp.ToSCS_R_001_001_GenShareCode;
import ToSCS_RB.p001_ShareCodeOp.ToSCS_RB_001_001_GenShareCode;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class MsgDealer_GC2GS_007_005_ReqGenerateShareCode extends NPUserMsgDealer<GC2GS_007_005_ReqGenerateShareCode>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_005_ReqGenerateShareCode _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //对传入数据的校验
        ResultOne<?> resultOne = ShareDataCheckerMgr.getInstance().doCheck(userData, _msg.getType().ordinal(), _msg.getData());
        if (!resultOne.isSucc())
        {
            _commiter.commitFailRes(resultOne.getCode());
            return;
        }

        //校验数据类型，获取数据
        if (!(resultOne.getData() instanceof _IALProtocolStructure))
        {
            _commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        _IALProtocolStructure transGetData = (_IALProtocolStructure) resultOne.getData();

        //转发到分享码服务器获取id
        ToSCS_R_001_001_GenShareCode proto = new ToSCS_R_001_001_GenShareCode();
        proto.setType(_msg.getType().ordinal());
        proto.setData(transGetData.makePackage().array());
        userData.getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SHARE_CODE.ordinal(), proto,
                new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new ToSCS_RB_001_001_GenShareCode();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {
                ToSCS_RB_001_001_GenShareCode retMsg = (ToSCS_RB_001_001_GenShareCode) _retMsg;
                String shareCode = ShareCodeUtil.generateCode(_msg.getType().ordinal(), retMsg.getSerial());
                _commiter.commitSucRes(US2GCWriter_007_CommOp.make_005_RetGenerateShareCode(shareCode));
            }

            @Override
            public void dealFail(int _errCode)
            {
                _commiter.commitFailRes(_errCode);
            }
        });
    }
}
