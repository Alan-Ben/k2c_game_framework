package MGClient;

public class GlobalStorage
{
    private static GlobalStorage _instance = new GlobalStorage();

    public static GlobalStorage getInstance()
    {
        return _instance;
    }

    private int _m_iRecentServerId = 1;

    public int getRecentServerId()
    {
        return _m_iRecentServerId;
    }

    public void saveRecentServerId(int usServerId)
    {
        _m_iRecentServerId = usServerId;
    }
}
