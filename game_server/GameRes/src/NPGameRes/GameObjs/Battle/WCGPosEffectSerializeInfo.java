package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;

import java.util.ArrayList;
import java.util.List;

public class WCGPosEffectSerializeInfo
{
    public EWCGPosEffectType effect_type;//效果类型
    public String effect_info_str;

    /**
     * 是否初始化过
     */
    private boolean _m_bHasInit = false;
    /**
     * 对应的具体数据
     */

    private _AWCGPosEffectInfo _m_eiEffectInfo;

    public _AWCGPosEffectInfo effectInfo()
    {
        if (!_m_bHasInit)
        {

            //还未初始化则进行初始化
            _m_eiEffectInfo = _AWCGPosEffectInfo.readEffectInfo(effect_type, effect_info_str);
            if (_m_eiEffectInfo == null)
            {
                CommLog.error("_AWCGPosEffectInfo 效果没有读取成功: " + effect_type + ":" + effect_info_str);
            }
            _m_bHasInit = true;
        }

        return _m_eiEffectInfo;

    }

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGPosEffectSerializeInfo readEffect(String _str)
    {
        int splitPos = _str.indexOf(':');
        if (0 >= splitPos)
            return null;

        WCGPosEffectSerializeInfo obj = new WCGPosEffectSerializeInfo();
        //读取第一个字段：条件类型
        String typeStr = _str.substring(0, splitPos);
        //读取类型枚举
        obj.effect_type = EWCGPosEffectType.valueOf(typeStr.toUpperCase().trim());
        obj.effect_info_str = _str.substring(splitPos + 1);

        return obj;
    }

    public static List<WCGPosEffectSerializeInfo> readEffectList(String _str)
    {
        List<WCGPosEffectSerializeInfo> list = new ArrayList<WCGPosEffectSerializeInfo>();

        String[] effectStrs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < effectStrs.length; i++)
        {
            String infoStr = effectStrs[i];
            String[] strs = CommonFunc.charSplit(infoStr, ':', 2);

            if (strs.length < 2)
                continue;

            //创建对象
            WCGPosEffectSerializeInfo obj = new WCGPosEffectSerializeInfo();
            obj.effect_type = EWCGPosEffectType.valueOf(strs[0].toUpperCase().trim());
            obj.effect_info_str = strs[1];
            list.add(obj);
        }

        return list;
    }
}
