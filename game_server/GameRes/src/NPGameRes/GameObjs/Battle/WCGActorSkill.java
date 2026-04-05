package NPGameRes.GameObjs.Battle;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class WCGActorSkill implements _IParseFromStringable
{
    public long skillId;
    public int level;

    public static ArrayList<WCGActorSkill> readSkillInfoList(String _str)
    {
        ArrayList<WCGActorSkill> skillInfoList = new ArrayList<WCGActorSkill>();
        if (_str.isEmpty())
        {
            //CommLog.error("read excel WCGActorSkill info err!, str is null or empty!" );
            return skillInfoList;
        }
        String[] strs = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < strs.length; i++)
        {
            String[] subStrs = CommonFunc.charSplit(strs[i], ':');
            if (subStrs.length == 0)
            {
                continue;
            }

            WCGActorSkill actorSkillInfo = new WCGActorSkill();
            actorSkillInfo.skillId = Long.parseLong(subStrs[0].trim());

            if (subStrs.length > 1)
                actorSkillInfo.level = Integer.parseInt(subStrs[1].trim());
            else
                actorSkillInfo.level = 1;

            skillInfoList.add(actorSkillInfo);
        }
        return skillInfoList;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] subStrs = CommonFunc.charSplit(sValue, ':');
        if (subStrs.length == 0)
        {
            return false;
        }
        if (subStrs.length == 1)
        {
            this.skillId = Long.parseLong(subStrs[0].trim());
            this.level = 1;
            return true;
        }
        this.skillId = Long.parseLong(subStrs[0].trim());
        this.level = Integer.parseInt(subStrs[1].trim());
        return true;

    }
}
