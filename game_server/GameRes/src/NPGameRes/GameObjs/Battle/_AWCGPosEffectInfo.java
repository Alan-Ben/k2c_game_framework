package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGPosEffectType;

public abstract class _AWCGPosEffectInfo
{
    /************
     * 效果类型
     **/
    public abstract EWCGPosEffectType effectType();

    /***********************
     * 获取本对象的实际条件数据对象
     */
    public static _AWCGPosEffectInfo readEffectInfo(EWCGPosEffectType _effectType, String _infoStr)
    {
        if (_effectType == EWCGPosEffectType.TRIGGER_EFFECT)
            return WCGPosEffectDoTrigger.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.SUMMON)
            return WCGPosEffectSummon.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.SPECIAL)
            return WCGPosEffectSpecial.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.ROUND_MOVE)
            return WCGPosEffectRoundMove.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.TELEPORT)
            return WCGPosEffectTeleport.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.PLAY_SFX)
            return WCGPosEffectPlaySfx.readVariable(_infoStr);
        if (_effectType == EWCGPosEffectType.TIME_LAND)
            return WCGPosEffectTimeLand.readVariable(_infoStr);
        return null;
    }
}
