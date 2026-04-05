package NPHttpServer.Http.Entity;

import java.util.HashSet;
import java.util.Set;

/**
 * @description: 封账号UID
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityBanUidList
{
    /**
     * 账号uid
     */
    private Set<String> uidSet;

    public NPEntityBanUidList()
    {
        uidSet = new HashSet<>();
    }

    /**
     * 封禁时间
     */
    private int hours;

    public Set<String> getUidSet()
    {
        return uidSet;
    }

    public void addUid(String uid)
    {
        uidSet.add(uid);
    }

    public int getHours()
    {
        return hours;
    }

    public void setHours(int hours)
    {
        this.hours = hours;
    }

    public long getTimeMs()
    {
        return hours * 3600000L;
    }
}
