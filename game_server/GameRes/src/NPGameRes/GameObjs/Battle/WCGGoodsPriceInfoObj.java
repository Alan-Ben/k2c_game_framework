package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGGoodsPriceInfoObj implements _IParseFromStringable
{
    // 统一物品ID
    public long uniform_item_id = 10001;
    // 兑换数量
    public int num = 9999999;

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] subStrs = CommonFunc.charSplit(sValue, ':');
        if (subStrs.length < 2)
        {
            return false;
        }
        this.uniform_item_id = Long.parseLong(subStrs[0].trim());
        this.num = Integer.parseInt(subStrs[1].trim());
        return true;
    }
}
