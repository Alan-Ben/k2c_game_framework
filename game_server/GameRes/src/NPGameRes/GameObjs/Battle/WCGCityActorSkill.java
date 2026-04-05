package NPGameRes.GameObjs.Battle;

import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;

public class WCGCityActorSkill extends WCGActorSkill
{
    public int idx;//下标，第几个

    /// <param name="_str"> 技能ID:技能等级；技能ID:技能等级；技能ID:技能等级,若有空，这需要;表示</param>
    //不会删除空的对象，保留下标
    public static List<WCGCityActorSkill> readCityActorSkillList(String _str)
    {
        List<WCGCityActorSkill> skillInfoList = new ArrayList<WCGCityActorSkill>();
        if (null == _str || _str.isEmpty())
        {
            return skillInfoList;
        }
        String[] strs = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < strs.length; i++)
        {
            String[] subStrs = CommonFunc.charSplit(strs[i], ':');
            if (subStrs.length < 2)
            {
                continue;
            }
            WCGCityActorSkill actorSkillInfo = new WCGCityActorSkill();
            actorSkillInfo.skillId = Long.parseLong(subStrs[0].trim());
            actorSkillInfo.level = Integer.parseInt(subStrs[1].trim());
            actorSkillInfo.idx = i;
            skillInfoList.add(actorSkillInfo);
        }
        return skillInfoList;
    }
}
