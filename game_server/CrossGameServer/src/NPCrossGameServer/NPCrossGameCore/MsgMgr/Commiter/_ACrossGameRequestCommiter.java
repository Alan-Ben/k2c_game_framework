package NPCrossGameServer.NPCrossGameCore.MsgMgr.Commiter;

import ALBasicProtocolPack._IALProtocolStructure;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/****************
 * 空间框架中的请求提交对象
 * 根据实际的载体不同，会进行不同的返回处理
 * @author mj
 *
 */
public abstract class _ACrossGameRequestCommiter implements _IWCGBasicRequestCommiter
{
    //实际请求的信息提交对象
    protected _IWCGBasicRequestCommiter _m_cCommiter;

    public _ACrossGameRequestCommiter()
    {
    }

    public _ACrossGameRequestCommiter(_IWCGBasicRequestCommiter _commiter)
    {
        _m_cCommiter = _commiter;
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        if (null == _m_cCommiter)
            return;

        _m_cCommiter.commitFailRes(_errCode);
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        if (null == _m_cCommiter)
            return;

        _m_cCommiter.commitSucRes(_retMsg);
    }
}
