package ${handlerPackage};
import ${reqPackage}.${reqProtoName};
import ${retPackage}.${retProtoName};
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
public class  ${handlerFileName} extends NPUserMsgDealer<${reqProtoName}>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, ${reqProtoName} _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        ${retProtoName} retMsg = new ${retProtoName}();

        _commiter.commitSucRes(retMsg);
    }
}