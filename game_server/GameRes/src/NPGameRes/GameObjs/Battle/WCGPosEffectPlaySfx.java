package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;


public class WCGPosEffectPlaySfx extends _AWCGPosEffectInfo
{
    private long _m_lSfxId;

    public long SfxID()
    {
        return _m_lSfxId;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.PLAY_SFX;
    }

    public static WCGPosEffectPlaySfx readVariable(String _str)
    {
        WCGPosEffectPlaySfx effectObj = new WCGPosEffectPlaySfx();
        //解析字符串

        try
        {
            effectObj._m_lSfxId = Long.parseLong(_str);

            return effectObj;
        } catch (Exception e)
        {
            CommLog.error("效果配置错误 - play_sfx example: enum:sfx_id: Error Str: " + _str);
            return null;
        }
    }
}