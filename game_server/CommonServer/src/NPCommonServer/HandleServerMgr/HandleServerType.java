package NPCommonServer.HandleServerMgr;

import CSDB.Bo.HandleServerBO;
import NPCommon.Log.CommLog;
import NPCommonServer.NPCommonServer;
import WCGCommon.Enum.NPEnum;

import java.util.List;

/**
 * 服务器负载类型管理，每个服务器类型对应一个HandleServerMgr对象，管理该类型的服务器负载信息
 * 已经注册的服务器负载机制不进入本机制（CrossRank，Room，CrossGame）
 * @author Administrator
 */
public class HandleServerType
{
    //单例模式
    private static HandleServerType _g_instance = new HandleServerType();
    public static  HandleServerType getInstance()
    {
        return _g_instance;
    }

    private HandleServerMgr[] _m_arrHandleServerMgr;

    public HandleServerType()
    {
        _m_arrHandleServerMgr = new HandleServerMgr[NPEnum.EServerType.values().length];

        regHandleServerMgr(new HandleServerMgr(NPEnum.EServerType.GAME_LOGIC.ordinal(), 0));
    }

    public void regHandleServerMgr(HandleServerMgr _mgr)
    {
        _m_arrHandleServerMgr[_mgr.getServerType()] = _mgr;
    }

    public HandleServerMgr getHandleServerMgr(int _serverType)
    {
        return _m_arrHandleServerMgr[_serverType];
    }

    public boolean sInit()
    {
        //排行榜数据
        List<HandleServerBO> boList = NPCommonServer.getInstance().getBM().getBM(HandleServerBO.class).s_findAll();
        if (null == boList)
        {
            return false;
        }

        for(int i = 0; i < boList.size(); i++)
        {
            HandleServerBO bo = boList.get(i);
            if (null == bo)
                continue;

            HandleServerMgr mgr = getHandleServerMgr(bo.getServerType());
            if(null == mgr)
            {
                CommLog.error("HandleServerType.sInit error, no HandleServerMgr for serverType:{}", bo.getServerType());
                return false;
            }

            HandleServerInfo info = new HandleServerInfo(mgr, bo);
            mgr._initServer(info);
        }

        return true;
    }
}
