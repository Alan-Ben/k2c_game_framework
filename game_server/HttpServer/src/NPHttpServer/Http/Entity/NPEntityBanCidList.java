package NPHttpServer.Http.Entity;

import java.util.HashSet;
import java.util.Set;

/**
 * @description: 封禁列表
 * @author: ricci
 * @date: 2023-04-08 16:22:54
 */
public class NPEntityBanCidList
{
    //cid列表
    private Set<Long> cidSet;
    //小时数
    private int hours;

    public NPEntityBanCidList()
    {
        cidSet = new HashSet<>();
    }

    public void addBanCid(long _cid)
    {
        cidSet.add(_cid);
    }

    public Set<Long> getBanCidList()
    {
        return cidSet;
    }

    public int getHours()
    {
        return hours;
    }

    public void setHours(int _hours)
    {
        hours = _hours;
    }

    public long getTimeMs()
    {
        return hours * 3600000L;
    }
}
