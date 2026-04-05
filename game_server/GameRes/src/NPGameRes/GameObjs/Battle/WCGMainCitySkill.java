package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class WCGMainCitySkill implements _IParseFromStringable
{
    public int city_level;
    public ArrayList<WCGActorSkill> skill_info_list;

    // / <param name="_str">
    // 主城等级:技能ID:技能等级；技能ID:技能等级；技能ID:技能等级|主城等级:技能ID:技能等级.</param>
    public static ArrayList<WCGMainCitySkill> readWCGMainCitySkillList(String _str)
    {
        ArrayList<WCGMainCitySkill> list = new ArrayList<WCGMainCitySkill>();
        if (_str.isEmpty())
            return list;

        String[] strs = CommonFunc.charSplit(_str, '|');
        for (int i = 0; i < strs.length; ++i)
        {
            String substr = strs[i];
            if (substr.isEmpty())
                continue;
            // subStr 主城等级:技能ID:技能等级；技能ID:技能等级；技能ID:技能等级
            WCGMainCitySkill citySkill = new WCGMainCitySkill();
            int idx = substr.indexOf(':');
            if (idx == -1)
                continue;
            citySkill.city_level = Integer.parseInt(substr.substring(0, idx).trim());
            citySkill.skill_info_list = WCGActorSkill.readSkillInfoList(substr.substring(idx + 1));
            list.add(citySkill);
        }
        return list;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue.isEmpty())
            return false;

        int idx = sValue.indexOf(':');
        if (idx == -1)
            return false;
        this.city_level = Integer.parseInt(sValue.substring(0, idx).trim());
        this.skill_info_list = WCGActorSkill.readSkillInfoList(sValue.substring(idx + 1));
        return true;
    }
}