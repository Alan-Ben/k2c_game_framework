package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGPosEffectSpecialType;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;

public class WCGPosEffectSpecial extends _AWCGPosEffectInfo
{
    /**
     * 位置触发效果的特殊效果的类型
     */
    private EWCGPosEffectSpecialType _m_ePosEffectSpecialType;

    public EWCGPosEffectSpecialType PosEffectSpecialType()
    {
        return _m_ePosEffectSpecialType;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.SPECIAL;
    }

    public static WCGPosEffectSpecial readVariable(String _str)
    {
        WCGPosEffectSpecial effectObj = new WCGPosEffectSpecial();
        effectObj._m_ePosEffectSpecialType = EWCGPosEffectSpecialType.valueOf(_str.toUpperCase().trim());


        return effectObj;
    }

}
