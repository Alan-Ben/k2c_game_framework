package NPCommon.RefData;

public class RefForceReloadRefInfo
{
    private String _m_sRefName;

    public RefForceReloadRefInfo(String _name)
    {
        _m_sRefName = _name;
    }

    public boolean isSame(String _name)
    {
        return _m_sRefName.equalsIgnoreCase(_name);
    }
}
