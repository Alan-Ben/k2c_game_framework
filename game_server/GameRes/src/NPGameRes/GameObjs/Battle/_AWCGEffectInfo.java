package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.Function.FuncTwo;
import WCGCommon.Enum.NPEnum.EWCGEffectType;

public abstract class _AWCGEffectInfo
{

    /************
     * 效果类型
     **/
    public abstract EWCGEffectType effectType();

    /*************
     * 相关读取的设定函数
     **/
    private static FuncTwo<_AWCGEffectInfo, EWCGEffectType, String> _g_fReadEffectInfoFunc;

    public static void regReadFunc(FuncTwo<_AWCGEffectInfo, EWCGEffectType, String> _readFunc)
    {
        _g_fReadEffectInfoFunc = _readFunc;
    }

    /***********************
     * 获取本对象的实际条件数据对象
     */
    public static _AWCGEffectInfo readEffectInfo(EWCGEffectType _effectType, String _infoStr)
    {
        if (null == _g_fReadEffectInfoFunc)
            return null;

        _AWCGEffectInfo ret = _g_fReadEffectInfoFunc.call(_effectType, _infoStr);
        if (null == ret)
        {
            if (EWCGEffectType.PLAY_SFX != _effectType)
            {
                CommLog.error("_AWCGEffectInfo 效果没有读取成功: " + _effectType + ":" + _infoStr);
            }
        }
        return ret;

    }
}