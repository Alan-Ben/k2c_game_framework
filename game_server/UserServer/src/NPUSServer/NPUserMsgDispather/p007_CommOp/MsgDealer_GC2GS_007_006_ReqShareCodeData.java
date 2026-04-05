package NPUSServer.NPUserMsgDispather.p007_CommOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p007_CommOp.GC2GS_007_006_ReqShareCodeData;
import NP2CS_R.p003_CommonOp.ToCS_R_003_001_GetShareData;
import NP2CS_RB.p003_CommonOp.ToCS_RB_003_001_GetShareData;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ShareCode.ShareCodeData;
import NPCommon.ShareCode.ShareCodeUtil;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class MsgDealer_GC2GS_007_006_ReqShareCodeData extends NPUserMsgDealer<GC2GS_007_006_ReqShareCodeData>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_006_ReqShareCodeData _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        ResultOne<ShareCodeData> parseResult = ShareCodeUtil.parseCode(_msg.getShareCode());
        if (!parseResult.isSucc())
        {
            _commiter.commitFailRes(parseResult.getCode());
            return;
        }

        ToCS_R_003_001_GetShareData proto = new ToCS_R_003_001_GetShareData();
        proto.setType(parseResult.getData().getType());
        proto.setSerial(parseResult.getData().getSerialNum());
        userData.getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(), proto,
                new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new ToCS_RB_003_001_GetShareData();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {
                ToCS_RB_003_001_GetShareData retMsg = (ToCS_RB_003_001_GetShareData) _retMsg;
                _commiter.commitSucRes(US2GCWriter_007_CommOp.make_006_RetShareCodeData(retMsg.getData()));
            }

            @Override
            public void dealFail(int _errCode)
            {
                _commiter.commitFailRes(_errCode);
            }
        });
    }
}
