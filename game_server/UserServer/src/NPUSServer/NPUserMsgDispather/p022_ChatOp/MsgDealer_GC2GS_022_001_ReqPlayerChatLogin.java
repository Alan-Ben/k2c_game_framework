package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p022_ChatOp.GC2GS_022_001_ReqPlayerChatLogin;
import NP2IS_RB.p001_ISOp.NP2IS_RB_001_003_RetRegChatUser;
import NPCommon.Log.CommLog;
import NPServerProtocolWriter.NP2IS.Request.Np2IS_R_Writer_001_ISOp;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/*************
 * 获取聊天服务器登录相关信息
 */
public class MsgDealer_GC2GS_022_001_ReqPlayerChatLogin extends NPUserMsgDealer<GC2GS_022_001_ReqPlayerChatLogin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_001_ReqPlayerChatLogin _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //向IS请求创建聊天用户
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.INTERFACE.ordinal(),
        		Np2IS_R_Writer_001_ISOp.make_003_ReqRegChatUser(userData.getCid(), userData.getAreaTag()), 
        		new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2IS_RB_001_003_RetRegChatUser();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2IS_RB_001_003_RetRegChatUser ret = (NP2IS_RB_001_003_RetRegChatUser) _retProto;
                        
                        ChatUserInfo chatUser = userData.getUSServer().getChatUserMgr().regChatUser(userData, ret.getChatUid());
                        userData.setChatUserSerial(chatUser);
                        
                        _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_001_RetPlayerChatLogin(ret.getSystemId(), ret.getSystemTag(), ret.getIp(), ret.getPort(), ret.getCheckCode()));
                    
                        CommLog.info("player:{} reg chat chat user suc.", userData.getCid());
                    }

                    @Override
                    public void dealFail(int _err)
                    {
                        _commiter.commitFailRes(_err);
                    }
                });
    }
}
