package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;


public class WCGPosEffectTeleport extends _AWCGPosEffectInfo
{
    //移动位移
    private int _m_iMoveDis;
    //排挤的移动时间
    private int _m_iKnockMoveTimeMS;
    //移动时的重量
    private int _m_iMoveWeight;

    public int moveDis()
    {
        return _m_iMoveDis;
    }

    public int knockMoveTimeMS()
    {
        return _m_iKnockMoveTimeMS;
    }

    public int moveWeight()
    {
        return _m_iMoveWeight;
    }

    protected WCGPosEffectTeleport()
    {
        _m_iMoveDis = 0;
        _m_iKnockMoveTimeMS = 0;
        _m_iMoveWeight = 0;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.TELEPORT;
    }

    public static WCGPosEffectTeleport readVariable(String _str)
    {
        WCGPosEffectTeleport effectObj = new WCGPosEffectTeleport();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':');
        if (strs.length < 2)
        {
            CommLog.error("Error Format for WCGEffectKnockBack -  example: enum:distance:knock_move_time_ms(:weight) Error Str: " + _str);
            return null;
        }

        effectObj._m_iMoveDis = Integer.parseInt(strs[2].trim());
        effectObj._m_iKnockMoveTimeMS = Integer.parseInt(strs[3].trim());
        if (strs.length > 2)
            effectObj._m_iMoveWeight = Integer.parseInt(strs[4].trim());

        return effectObj;
    }
}
