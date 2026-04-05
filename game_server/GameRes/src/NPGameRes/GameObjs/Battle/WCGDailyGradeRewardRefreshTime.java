package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGDailyGradeRewardRefreshTime implements _IParseFromStringable
{
    public int refreshHour;
    public int refreshMinute;

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] subStrs = CommonFunc.charSplit(sValue, ':');
        if (subStrs.length > 2)
            return false;

        refreshHour = Integer.parseInt(subStrs[0].trim());

        if (2 == subStrs.length)
            refreshMinute = Integer.parseInt(subStrs[1].trim());
        else
            refreshMinute = 0;

        return true;
    }
}
