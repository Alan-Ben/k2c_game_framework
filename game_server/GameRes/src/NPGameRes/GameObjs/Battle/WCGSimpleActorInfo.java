package NPGameRes.GameObjs.Battle;

public class WCGSimpleActorInfo
{
    public long actorId;
    public int actorLevel;
    public long skinId;

    public WCGSimpleActorInfo()
    {
        actorId = 0;
        actorLevel = 0;
        skinId = 0;
    }

    public WCGSimpleActorInfo(long _actorId, int _level, long _skinId)
    {
        actorId = _actorId;
        actorLevel = _level;
        skinId = _skinId;
    }

}
