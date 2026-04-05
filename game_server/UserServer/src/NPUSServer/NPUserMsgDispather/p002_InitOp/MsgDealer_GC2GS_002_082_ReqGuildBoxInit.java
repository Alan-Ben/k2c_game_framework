package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_082_ReqGuildBoxInit;
import GS2GC.p002_InitOp.GS2GC_002_082_RetGuildBoxInit;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;


public class MsgDealer_GC2GS_002_082_ReqGuildBoxInit extends NPUserMsgDealer<GC2GS_002_082_ReqGuildBoxInit> 
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_082_ReqGuildBoxInit _msg) 
    {
        // 获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查玩家公会，如果玩家公会不存在，则直接清空玩家当前的宝箱数据
        //无公会直接返回
        if(userData.getGuildComponent().getGuildId() <= 0)
        {
            userData.getGuildBoxComponent().dispatchAll(null, userData.getPlayerInitContext());

            _commiter.commitSucRes(new GS2GC_002_082_RetGuildBoxInit());
            return ;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GS2GC_002_082_RetGuildBoxInit>(_commiter) {
                    @Override
                    protected GS2GC_002_082_RetGuildBoxInit _createNewTmpObj() {
                        return new GS2GC_002_082_RetGuildBoxInit();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GS2GC_002_082_RetGuildBoxInit _retMsg) {

                        userData.getGuildBoxComponent().refreshAll();
                        userData.getGuildBoxComponent().makeProto(_retMsg.getBoxList());

                        //返回协议
                        _commiter.commitSucRes(_retMsg);
                    }
                },
                _commiter.getUserData().getCid(),
                _commiter.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null,
                new _ICallBackIntT<_ANPUSUserBasicMsgItem>() {
                    @Override
                    public void onRunOver(int _retValue, _ANPUSUserBasicMsgItem _commiter)
                    {
                        userData.getGuildBoxComponent().dispatchAll(null, userData.getPlayerInitContext());

                        _commiter.commitSucRes(new GS2GC_002_082_RetGuildBoxInit());
                    }
                }
        );
    }
}
