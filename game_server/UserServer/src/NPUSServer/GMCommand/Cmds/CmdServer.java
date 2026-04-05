package NPUSServer.GMCommand.Cmds;

import GS2GC.p007_CommOp.GS2GC_007_053_OnDebugMsg;
import NPCommon.Enum.EUsParam;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.ServerGMUtil;
import NPCommon.Util.StringFunc;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSMain;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.UserServerConf;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends UsCmdBase
{
    @ACommand(comment = "打印服务器参数")
    public String params()
    {
        return getUserServer().getUSParams().toString();
    }

    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("Pid:%d\n", CommonFunc.getPid()));
        sb.append(String.format("UsTypeId:%d\n", getUserServer().getServerTypeId()));

        sb.append("Us Params:\n");
        sb.append(getUserServer().getUSParams().toString());
        String sInfo = sb.toString();
        CommLog.info(sInfo);
        return sInfo;

    }

    @ACommand(comment = "发送调试信息到客户端(cid,base64编码后的调试消息)")
    public String sendMsg2Client(long _cid, String _encodeMsg)
    {
        NPUSUserData userData = getUserServer().getUsUserMgr().lookupCacheUserData(_cid);
        if (null == userData)
        {
            return "no online debugger player for cid:" + _cid;
        }

        String msg;
        try
        {
            msg = StringFunc.decodeString(_encodeMsg);
        } catch (Exception e)
        {
            msg = _encodeMsg;
        }
        GS2GC_007_053_OnDebugMsg proto = new GS2GC_007_053_OnDebugMsg();
        proto.setMsg(msg);
        userData.sendMsgToGC(proto);
        return "send debug to " + _cid + " msg:" + msg;
    }

    @ACommand(comment = "设置自己为调试玩家")
    public void debugSelf()
    {
        ServerGMUtil.routeGmCommand(getUserServer(), "cs server setDebugger " + getOwner().getCid(), new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String s)
            {
                takeCallBack().onRunOver(_isSucc, s);
            }
        });
    }

    @ACommand(comment = "列表服务器参数")
    public String listUsParam(String _param, long _value)
    {
        return getUserServer().getUSParams().toString();
    }

    @ACommand(comment = "设置服务器参数[枚举名称][数值]")
    public String setUsParam(String _param, long _value)
    {
        EUsParam param = EUsParam.valueOf(_param.toUpperCase());
        if (null == param)
            return "fail, not find param, " + _param;

        getUserServer().getUSParams().setParam(param, _value);
        return "ok";
    }

    @ACommand(comment = "获取服务器参数[枚举名称]")
    public String getUsParam(String _param)
    {
        EUsParam param = EUsParam.valueOf(_param.toUpperCase());
        if (null == param)
            return "fail, not find param, " + _param;

        return "ok, param:" + _param + " : " + getUserServer().getUSParams().getParam(param);
    }

    @ACommand(comment = "封禁玩家 [cid] [时间]")
    public String freeze(long cid, long timeSec)
    {
        getUserServer().getPlayerFreezeMgr().freeze(cid, timeSec * 1000L);
        //如果玩家在线将玩家强制下线
        getUserServer().getUsUserMgr().forceKickUser(cid);
        return "ok";
    }

    @ACommand(comment = "封禁玩家信息")
    public String freezeInfo()
    {
        return getUserServer().getPlayerFreezeMgr().toString();
    }

    @ACommand(comment = "设置玩家离线等待卸载的时长，单位：秒，影响全部玩家")
    public String setPlayerUnloadSecs(int _secs)
    {
        UserServerConf.getInstance().setUserDataExpiredTimeSec(_secs);

        return "ok";
    }

    @ACommand(comment = "设置最大同时登录玩家人数,设置为-1所有人登录都会排队")
    public String setMaxUserLoadCount(int _maxLoadUserCount)
    {
        UserServerConf.getInstance().setMaxLoadUserData(_maxLoadUserCount);
        return "ok";
    }

    @ACommand(comment = "清除玩家缓存")
    public String offloadCache(int _usId)
    {
        NPUserServer userServer = NPUSMain.GetServerByTypeId(_usId);
        if (userServer == null)
            return "fail, not find server, " + _usId;

        userServer.getUsUserMgr().unloadExpiredUserData(true);
        return "ok";
    }

    @ACommand(comment = "是否在线（玩家cid）")
    public String isOn(long _cid)
    {
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        NPUserServer userServer = NPUSMain.GetServerByTypeId(usId);
        if (userServer == null)
            return "fail, not find server, " + usId;

        NPUSUserData userData = userServer.getUsUserMgr().lookupCacheUserData(_cid);
        if (null == userData)
            return "off, no cache";

        return userData.isOnline() ? " on" : "off, has cache";
    }
}
