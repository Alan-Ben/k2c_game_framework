package NPCommon.Util;

import NP2HS_R.p002_HsClientOp.NP2HS_R_002_001_ReqExecGmSuper;

public class ProtoUtil
{
    public static String getString(NP2HS_R_002_001_ReqExecGmSuper _msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("[serverType]=%d\n", _msg.getServerType()));
        sb.append(String.format("[serverTypeIdList]=%s\n", CommonFunc.list2String(_msg.getServerIdList(), ';')));
        sb.append(String.format("[isAll]=%s\n", String.valueOf(_msg.getIsAll())));
        sb.append(String.format("[cmd]=%s\n", _msg.getGmComamnd()));
        sb.append(String.format("[isAllRef]=%s\n", String.valueOf(_msg.getIsAllRef())));
        return sb.toString();
    }
}
