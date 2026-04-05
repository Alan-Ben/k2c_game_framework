package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "账号相关", name = "account")
public class CmdAccount extends UsCmdBase
{
    @ACommand(comment = "解除玩家uid与cid的关联(玩家uid)")
    public String unlinkUid(String _uid)
    {
        getUserServer().getUserIdxMgr().cmdUnlinkUid(_uid);
        return "done, please check db";
    }

    @ACommand(comment = "查询玩家uid对应的cid(玩家uid)")
    public void lookupUserCid(String _uid)
    {
        getUserServer().getUserIdxMgr().lookupUidLinkedCid(_uid, new _ICallBackResultT<Long>()
        {
            @Override
            public void onRunOver(Result _result, Long _cid)
            {
                if (_result.isSucc())
                {
                    takeCallBack().onRunOver(true, "uid:" + _uid + " linked cid:" + _cid);
                }
                else
                {
                    takeCallBack().onRunOver(true, "uid:" + _uid + " not linked cid");
                }
            }
        });
    }

    @ACommand(comment = "查询玩家cid对应的uid(玩家cid)")
    public void lookupUserUid(long _cid)
    {
        getUserServer().getUserIdxMgr().lookupCidLinkedUidFromDb(_cid, new _ICallBackResultT<String>()
        {
            @Override
            public void onRunOver(Result _result, String _uid)
            {
                if (_result.isSucc())
                {
                    takeCallBack().onRunOver(true, "cid:" + _cid + " linked uid:" + _uid);
                }
                else
                {
                    takeCallBack().onRunOver(true, "cid:" + _cid + " not linked uid");
                }
            }
        });
    }

    @ACommand(comment = "将uid和cid关联(玩家uid,cid)")
    public String linkUidWithCid(String _uid, long _cid)
    {

        getUserServer().getUserIdxMgr().cmdLinkUidWithCid(_uid, _cid);
        return "done, please check db";
    }
}
