package NPUSServer.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

/**
 * @description: 产生服务器事件，需要触发所有玩家onLogicEvent
 * @author: ricci
 * @date: 2022-09-16 17:58:32
 */
public class USServerServerEventBroadCastAllUser implements _IALSynTask
{
    private NPUserServer _m_server;
    /**
     * 全局事件
     */
    private _ALogicEventBase _m_event;

    public USServerServerEventBroadCastAllUser(NPUserServer _server, _ALogicEventBase _event)
    {
        _m_server = _server;
        this._m_event = _event;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        for (NPUSUserData userData : getUSServer().getUsUserMgr().getAllCacheUserData())
        {
            if (userData == null)
            {
                continue;
            }
            userData.onLogicEvent(_m_event);
        }
    }
}
