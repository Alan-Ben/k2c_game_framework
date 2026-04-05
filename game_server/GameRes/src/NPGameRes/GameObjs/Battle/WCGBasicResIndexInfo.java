package NPGameRes.GameObjs.Battle;

public abstract class WCGBasicResIndexInfo extends ALBasicResIndexInfo
{
    /************
     * 资源加载路径
     **/
    public abstract String assetPath();

    public abstract String objName();


    public static boolean IsEqual(WCGBasicResIndexInfo _a1, WCGBasicResIndexInfo _a2)
    {
        if (_a1 == _a2)
            return true;
        if (_a1 != null && _a2 != null)
            return _a1.mainId == _a2.mainId && _a1.subId == _a2.subId;
        return false;
    }
}