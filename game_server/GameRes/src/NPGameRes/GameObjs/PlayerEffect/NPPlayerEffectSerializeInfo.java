package NPGameRes.GameObjs.PlayerEffect;

import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerEffectType;

import java.util.ArrayList;

public class NPPlayerEffectSerializeInfo
{
    public ENPPlayerEffectType effect_type;//效果类型
    public String effect_info_str;

    /**
     * 是否初始化过
     */
    private boolean _m_bHasInit = false;
    /**
     * 对应的具体数据
     */
    private _ANPPlayerEffectInfo _m_eiEffectInfo;

    public _ANPPlayerEffectInfo effectInfo()
    {
        if (!_m_bHasInit)
        {
            _m_bHasInit = true;
            //还未初始化则进行初始化
            try
            {
                _m_eiEffectInfo = _ANPPlayerEffectInfo.readEffectInfo(effect_type, effect_info_str);
                if (_m_eiEffectInfo == null)
                    throw new Exception("效果没有读取成功: " + effect_type + ":" + effect_info_str);
            } catch (Exception e)
            {
                e.printStackTrace();
            }
        }

        return _m_eiEffectInfo;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static ArrayList<NPPlayerEffectSerializeInfo> readEffectList(String _str)
    {
        if (null == _str || _str == "")
            return null;

        ArrayList<NPPlayerEffectSerializeInfo> list = new ArrayList<NPPlayerEffectSerializeInfo>();

        String[] effectStrs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < effectStrs.length; i++)
        {
            String infoStr = effectStrs[i];
            String[] strs = CommonFunc.charSplit(infoStr, new char[]{':'}, 2);

            if (strs.length < 1)
                continue;

            //创建对象
            NPPlayerEffectSerializeInfo obj = new NPPlayerEffectSerializeInfo();
            obj.effect_type = ENPPlayerEffectType.valueOf(strs[0].toUpperCase());
            obj.effect_info_str = strs.length == 2 ? strs[1] : null;

            list.add(obj);
        }

        return list;
    }

    public static NPPlayerEffectSerializeInfo readEffect(String _str)
    {
        if (null == _str || _str == "")
            return null;

        String[] strs = CommonFunc.charSplit(_str, new char[]{':'}, 2);

        if (strs.length < 1)
            return null;

        //创建对象
        NPPlayerEffectSerializeInfo obj = new NPPlayerEffectSerializeInfo();
        obj.effect_type = ENPPlayerEffectType.valueOf(strs[0].toUpperCase());
        obj.effect_info_str = strs.length == 2 ? strs[1] : null;

        return obj;
    }
}
