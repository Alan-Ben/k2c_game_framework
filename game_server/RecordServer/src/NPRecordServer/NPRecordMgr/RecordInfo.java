package NPRecordServer.NPRecordMgr;

import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import RCSDB.Bo.NpLoginServerInfoBO;


/**
 * @description: 玩家登录服务器信息
 * @author: ricci
 * @date: 2022-06-27 11:50:16
 */
public class RecordInfo
{
    /**
     * 登录记录数据
     */
    private NpLoginServerInfoBO _m_bo;

    /**
     * 隶属列表
     */
    private RecordInfoList _m_list;

    public RecordInfo(RecordInfoList _list, NpLoginServerInfoBO _bo)
    {
        _m_list = _list;
        _m_bo = _bo;
    }

    //region get&&set
    public long getCid()
    {
        return _m_bo.getCid();
    }

    public NpLoginServerInfoBO getBo()
    {
        return _m_bo;
    }

    public RecordInfoList getList()
    {
        return _m_list;
    }

    public NP_SYS_PlayerJoinedUSInfo makeProto()
    {
        NP_SYS_PlayerJoinedUSInfo proto = new NP_SYS_PlayerJoinedUSInfo();
        proto.getServerItem().setServerLogicId(_m_bo.getLastLoginServerId());
        proto.setCid(getCid());
        proto.setLastLoginTimeMs(getBo().getLastLoginTimeMs());
        return proto;
    }
    //endregion


    @Override
    public String toString()
    {
        return "\nRecordInfo{" +
                "accountId " + _m_bo.getAccountId() +
                "cid " + _m_bo.getCid() +
                "serverType " + _m_bo.getLastLoginServerId() +
                "loginTime " + _m_bo.getLastLoginTimeMs() +
                '}' + "\n";
    }
}
