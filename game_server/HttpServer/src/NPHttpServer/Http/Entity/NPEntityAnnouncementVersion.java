package NPHttpServer.Http.Entity;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 公告版本通知
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityAnnouncementVersion
{
    //服务器列表
    private List<Integer> _m_usIdList;
    //版本
    private String _m_version = "";

    public NPEntityAnnouncementVersion()
    {
        _m_usIdList = new ArrayList<>();
    }

    public List<Integer> getUsIdList()
    {
        return _m_usIdList;
    }

    public void setUsIdList(List<Integer> _usIdList)
    {
        _m_usIdList = _usIdList;
    }

    public void addUsId(int _usId)
    {
        _m_usIdList.add(_usId);
    }

    public String getVersion()
    {
        return _m_version;
    }

    public void setVersion(String _version)
    {
        _m_version = _version;
    }
}
