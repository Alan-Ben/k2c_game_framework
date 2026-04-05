package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_063_ReqGuildInit;
import GS2GC.p002_InitOp.GS2GC_002_063_RetGuildInit;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_063_ReqGuildInit extends NPUserMsgDealer<GC2GS_002_063_ReqGuildInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_063_ReqGuildInit _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //构造US本地信息
        GS2GC_002_063_RetGuildInit usGuildInfo = US2GCWriter_002_InitOp.make_063_RetGuildInit_OnlyUS(userData);

        //检查玩家是否已经加入联盟
        if (userData.getGuildComponent().getGuildId() <= 0)
        {
            _commiter.commitSucRes(usGuildInfo);
            return;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _commiter,
                userData.getCid(),
                userData.getGuildComponent().getGuildId(),
                _msg,
                usGuildInfo,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    //此处失败需要直接返回，避免卡死加载
                    @Override
                    public void onRunOver(int _retValue, _ANPUSUserBasicMsgItem _commiter) {
                        _commiter.commitSucRes(usGuildInfo);
                    }
                }
        );
    }
}
