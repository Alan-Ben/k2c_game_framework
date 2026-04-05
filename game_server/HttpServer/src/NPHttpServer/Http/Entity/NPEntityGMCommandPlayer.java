package NPHttpServer.Http.Entity;

import java.util.HashSet;
import java.util.Set;

/**
 * @description: 设置白名单
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityGMCommandPlayer
{
    //用户ID
    private Set<Long> cidList;
    private String command;

    public NPEntityGMCommandPlayer()
    {
        cidList = new HashSet<>();
    }

    public Set<Long> getCidSet()
    {
        return cidList;
    }

    public void addCid(long _cid)
    {
        cidList.add(_cid);
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
