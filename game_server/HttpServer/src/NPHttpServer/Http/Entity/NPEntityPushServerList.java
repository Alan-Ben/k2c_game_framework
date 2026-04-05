package NPHttpServer.Http.Entity;

import NPCommon.NP_SYS_ServerItem;

import java.util.ArrayList;

/**
 * @description: 后台推送的服务器列表数据
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityPushServerList
{
    /**
     * 服务器版本标签
     */
    private String serverTag;
    /**
     * 服务器列表信息
     */
    private ArrayList<NP_SYS_ServerItem> serverList;

    public NPEntityPushServerList()
    {
        this.serverTag = "";
        this.serverList = new ArrayList<>();
    }

    public String getServerTag()
    {
        return serverTag;
    }

    public void setServerTag(String serverTag)
    {
        this.serverTag = serverTag;
    }

    public ArrayList<NP_SYS_ServerItem> getServerList()
    {
        return serverList;
    }

    public void setServerList(ArrayList<NP_SYS_ServerItem> serverList)
    {
        this.serverList = serverList;
    }

    public void addServerList(NP_SYS_ServerItem _serverItem)
    {
        serverList.add(_serverItem);
    }
}
