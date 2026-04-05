package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p004_PlayerOp.GC2GS_004_002_ReqGmCommand;
import GS2GC.p004_PlayerOp.GS2GC_004_002_RetGmCommand;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_004_RetCheckIsInWhiteList;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.ServerGMUtil;
import NPEnum.ENPGameEvent;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GMCommand.UsCmdPlayerExecutor;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.USLog;
import NPUSServer.UserServerConf;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

public class MsgDealer_GC2GS_004_002_ReqGmCommand extends NPUserMsgDealer<GC2GS_004_002_ReqGmCommand>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_002_ReqGmCommand _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        if (UserServerConf.getInstance().getOpenGM())
        {
            runGM(_commiter, _msg.getCommand(), userData);
        } else
        {
            getUSServer().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(),
                    NPEnum.ENPSingleServerType.HTTP.ordinal(), Np2HS_R_Writer_001_HSOP.make_001_004_ReqCheckIsInWhiteList(userData.getUid()), new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2HS_RB_001_004_RetCheckIsInWhiteList();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _retProto)
                        {
                            NP2HS_RB_001_004_RetCheckIsInWhiteList ret = (NP2HS_RB_001_004_RetCheckIsInWhiteList) _retProto;

                            if (!ret.getIsInWhiteList())
                            {
                                GS2GC_004_002_RetGmCommand proto = new GS2GC_004_002_RetGmCommand();
                                proto.setResult("对不起，您没有使用GM权限，请联系管理员进行分配 cid: " + userData.getCid());
                                USLog.info(userData.getUSServer(), "run GM not open error cid:{}", userData.getCid());
                                _commiter.commitSucRes(proto);
                                return;
                            }

                            runGM(_commiter, _msg.getCommand(), userData);
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            GS2GC_004_002_RetGmCommand proto = new GS2GC_004_002_RetGmCommand();
                            proto.setResult("服务器内部通信检查白名单失败 或 检查UsConf是否配置开启GM命令");
                            CommLog.info("run GM not open error cid:{}", userData.getCid());
                            _commiter.commitSucRes(proto);
                        }
                    });
        }
    }

    /**
     * 执行GM命令
     * @param _commiter committer
     * @param _command  命令
     * @param _userData 玩家数据
     */
    private static void runGM(_ANPUSUserBasicMsgItem _commiter, String _command, NPUSUserData _userData)
    {
        //输出服务器日志
        USLog.sys("Cid: " + _userData.getCid() + " Deal Gm CMD: " + _command);
        if (ServerGMUtil.routeGmCommand(_commiter.getUserData().getUSServer(), _command, new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _bSucc, String _result)
            {
                GS2GC_004_002_RetGmCommand proto = new GS2GC_004_002_RetGmCommand();
                proto.setIsSucc(_bSucc);
                if (_result.length() < 10 * 1024)
                {
                    proto.setResult(_result);
                } else
                {//超出10k文本，压缩发送给客户端
                    byte[] zipedString = CommonFunc.zipString(_result);
                    proto.setZipedResult(zipedString);
                }
                _commiter.commitSucRes(proto);
            }
        }))
        {
            //已经路由到其它服务器执行
        } else
        { //本服执行
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
            UsCmdPlayerExecutor execContext = new UsCmdPlayerExecutor(_userData);
            GmCommandMgr.getInstance().run(execContext, _command, context, (_bSucc, _result) ->
            {

                GS2GC_004_002_RetGmCommand proto = new GS2GC_004_002_RetGmCommand();
                proto.setIsSucc(_bSucc);
                if (_result.length() < 10 * 1024)
                {
                    proto.setResult(_result);
                } else
                {//超出10k文本，压缩发送给客户端
                    byte[] zipedString = CommonFunc.zipString(_result);
                    proto.setZipedResult(zipedString);
                }
                _commiter.commitSucRes(proto);
            });
        }
    }
}
