package NPUSServer.ServerDeplomacy;

import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.DeplomacyUsBO;

import java.util.ArrayList;
import java.util.List;

/***
 * 服务器外交管理器
 */
public class ServerDeplomacyCore {
    private NPUserServer _m_usServer;
    //建交的Us队列
    private ArrayList<DeplomacyUsBO> _m_lBuildUSIdList;

    public ServerDeplomacyCore(NPUserServer _usServer)
    {
        _m_usServer = _usServer;
        _m_lBuildUSIdList = new ArrayList<DeplomacyUsBO>();
    }

    public NPUserServer getUSServer() {return _m_usServer;}

    /**
     * 初始化外交相关数据，初始化建交Us队列
     */
    public boolean initFromDB()
    {
        List<DeplomacyUsBO> boList = getUSServer().getBM().getBM(DeplomacyUsBO.class).s_findAll();
        if(null == boList)
        {
            USLog.error(_m_usServer, "UserCounterMgr init bo list fail.");
            return false;
        }
        for(int i = 0; i < boList.size(); i++)
        {
            DeplomacyUsBO bo = boList.get(i);
            if(null == bo)
                continue;

            //放入队列
            _m_lBuildUSIdList.add(bo);
        }

        return true;
    }

    /**
     * 是否与_usId建交
     * @param _usId
     * @return
     */
    public boolean isUsInDeplomacy(long _usId)
    {
        for(DeplomacyUsBO bo : _m_lBuildUSIdList)
        {
            if(bo.getUsId() == _usId)
                return true;
        }
        return false;
    }
}
