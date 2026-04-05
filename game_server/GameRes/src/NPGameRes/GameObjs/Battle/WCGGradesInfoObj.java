package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class WCGGradesInfoObj implements _IParseFromStringable
{
    public long grades = 1; // 段位
    public int starhoner = 3; // 星耀
    public long legendscore = 100;//传说积分

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] subStrs = CommonFunc.charSplit(sValue, ':');
        if (subStrs.length < 3)
        {
            return false;
        }
        this.grades = Long.parseLong(subStrs[0].trim());
        this.starhoner = Integer.parseInt(subStrs[1].trim());
        this.legendscore = Long.parseLong(subStrs[2].trim());
        return true;
    }
}
