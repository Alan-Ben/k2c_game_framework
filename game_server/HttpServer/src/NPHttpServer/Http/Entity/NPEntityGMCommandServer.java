package NPHttpServer.Http.Entity;

import java.util.HashSet;
import java.util.Set;

/**
 * @description: 设置白名单
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityGMCommandServer
{
    //用户ID
    private Set<Integer> serverIdList;
    private String command;

    public NPEntityGMCommandServer()
    {
        serverIdList = new HashSet<>();
    }

    public Set<Integer> getServerIdList()
    {
        return serverIdList;
    }

    public void addServerId(int _serverId)
    {
        serverIdList.add(_serverId);
    }

    public String getCommand()
    {
        return command;
    }

    public void setCommand(String _command)
    {
        command = _command;
    }
}
