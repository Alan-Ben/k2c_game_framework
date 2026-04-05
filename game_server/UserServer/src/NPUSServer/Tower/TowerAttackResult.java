package NPUSServer.Tower;

public class TowerAttackResult
{
    private long _m_targetCid;
    private boolean _m_isWin;

    public TowerAttackResult(long _targetCid, boolean _isWin)
    {
        _m_targetCid = _targetCid;
        _m_isWin = _isWin;
    }

    public long getTargetCid()
    {
        return _m_targetCid;
    }

    public boolean isWin()
    {
        return _m_isWin;
    }
}
