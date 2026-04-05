package MGClient.Common;

public class LoginStat
{
    public static LoginStat getInstance()
    {
        return _instance;
    }

    private static LoginStat _instance = new LoginStat();

    private long[] _m_arrStatData = new long[ELoginStat.values().length];


    public synchronized void incStat(ELoginStat eStat)
    {
        _m_arrStatData[eStat.ordinal()]++;
    }

    @Override
    public synchronized String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (ELoginStat eStat : ELoginStat.values())
        {
            sb.append(String.format("[%s]=%d \n", eStat.toString(), _m_arrStatData[eStat.ordinal()]));
        }
        return sb.toString();
    }

    public void clear()
    {
        for (ELoginStat eStat : ELoginStat.values())
        {
            _m_arrStatData[eStat.ordinal()] = 0;
        }
    }
}
