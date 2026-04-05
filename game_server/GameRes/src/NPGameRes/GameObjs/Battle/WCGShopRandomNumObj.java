package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGShopRandomNumObj implements _IParseFromStringable
{

    public long id; // 随机组ID
    public int count; // 随机次数

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] subStrs = CommonFunc.charSplit(sValue, ':');
        if (subStrs.length < 2)
        {
            return false;
        }
        this.id = Long.parseLong(subStrs[0].trim());
        this.count = Integer.parseInt(subStrs[1].trim());
        return true;
    }
}
