package NPUSServer.NPUSUserMgr.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/****************
 * 玩家数据在线状态变更处理任务
 * @author Administrator
 *
 */
public class NPSynUserDataOnlineChgTask implements _IALSynTask
{
    private NPUSUserData _m_udUserData;
    //对应序列号
    private long _m_lUserSerialize;
    //在线状态
    private boolean _m_bOnlineState;

    public NPSynUserDataOnlineChgTask(NPUSUserData _userData, boolean _onlineState)
    {
        _m_udUserData = _userData;
        _m_lUserSerialize = _m_udUserData.getSerialize();
        _m_bOnlineState = _onlineState;
    }

    @Override
    public void run()
    {
        if (null == _m_udUserData)
            return;

        //判断状态是否一致
        if (_m_bOnlineState != _m_udUserData.isOnline())
            return;

        //处理变更
        _m_udUserData.dealOnlineStateChg(_m_lUserSerialize, _m_bOnlineState);
    }

}
