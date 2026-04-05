package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

public class DecodeInt implements _IParseFromStringable
{
    public int value;

    @Override
    public boolean parseFromString(String _rawStr)
    {
        try
        {
            value = Integer.decode(_rawStr.trim());
        } catch (NumberFormatException e)
        {
            CommLog.error("DecodeInt parseFromString error! rawStr: " + _rawStr, e);
            return false;
        }
        return true;
    }
}