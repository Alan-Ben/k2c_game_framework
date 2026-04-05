package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGEffectTargetType;
import WCGCommon.Enum.NPEnum.EWCGEffectType;

import java.util.ArrayList;
import java.util.List;

public class WCGEffectSerializeInfo implements _IParseFromStringable
{

    public EWCGEffectType effect_type;//效果类型
    public EWCGEffectTargetType target_type;//目标类型
    public String effect_info_str;

    /**
     * 是否初始化过
     */

    private boolean _m_bHasInit = false;
    /**
     * 对应的具体数据
     */
    private _AWCGEffectInfo _m_eiEffectInfo;

    public _AWCGEffectInfo effectInfo()
    {
        if (!_m_bHasInit)
        {
            //还未初始化则进行初始化
            _m_eiEffectInfo = _AWCGEffectInfo.readEffectInfo(effect_type, effect_info_str);
            _m_bHasInit = true;

        }

        return _m_eiEffectInfo;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static List<WCGEffectSerializeInfo> readEffectList(String _str)
    {
        List<WCGEffectSerializeInfo> list = new ArrayList<WCGEffectSerializeInfo>();

        String[] effectStrs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < effectStrs.length; i++)
        {
            String infoStr = effectStrs[i];
            String[] strs = CommonFunc.charSplit(infoStr, ':', 3);

            if (strs.length < 3)
                continue;

            //创建对象
            WCGEffectSerializeInfo obj = new WCGEffectSerializeInfo();
            obj.effect_type = EWCGEffectType.valueOf(strs[0].toUpperCase().trim());
            ;
            obj.target_type = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
            obj.effect_info_str = strs[2];

            list.add(obj);
        }

        return list;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGEffectSerializeInfo readEffect(String _str)
    {
        String infoStr = _str;
        String[] strs = CommonFunc.charSplit(infoStr, new char[]{':'}, 3);

        if (strs.length < 3)
            return null;

        //创建对象
        WCGEffectSerializeInfo obj = new WCGEffectSerializeInfo();
        obj.effect_type = EWCGEffectType.valueOf(strs[0].trim().toUpperCase());
        obj.target_type = EWCGEffectTargetType.valueOf(strs[1].trim().toUpperCase());
        obj.effect_info_str = strs[2];

        return obj;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        String infoStr = sValue;
        String[] strs = CommonFunc.charSplit(infoStr, ':', 3);
        if (strs.length < 3)
            return false;
        //创建对象
        this.effect_type = EWCGEffectType.valueOf(strs[0].toUpperCase().trim());
        this.target_type = EWCGEffectTargetType.valueOf(strs[1].toUpperCase().trim());
        this.effect_info_str = strs[2];
        return true;

    }
}
