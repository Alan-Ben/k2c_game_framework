package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;


public class WCGPosEffectTimeLand extends _AWCGPosEffectInfo
{
    //移动时间
    private int _m_iMoveTimeMS;
    //重量
    private int _m_iWeight;

    public int moveTimeMS()
    {
        return _m_iMoveTimeMS;
    }

    public int weight()
    {
        return _m_iWeight;
    }

    protected WCGPosEffectTimeLand()
    {
        _m_iMoveTimeMS = 0;
        _m_iWeight = 0;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.TIME_LAND;
    }

    public static WCGPosEffectTimeLand readVariable(String _str)
    {
        WCGPosEffectTimeLand effectObj = new WCGPosEffectTimeLand();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');

        if (strs.length < 2)
        {
            CommLog.error("Error Format for WCGEffectKnockRound -  example: enum:time:weight Error Str: " + _str);
            return null;
        }

        effectObj._m_iMoveTimeMS = Integer.parseInt(strs[0].trim());
        effectObj._m_iWeight = Integer.parseInt(strs[1].trim());

        return effectObj;
    }
}