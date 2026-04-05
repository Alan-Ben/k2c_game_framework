package NPGameRes.GameObjs.Battle;

import java.util.ArrayList;
import java.util.List;

/**
 * @author scott 地 图 信 息
 */
public class NPActorSkillRefObj extends _IALBasicRefObj
{
    public long id;
    public String icon;//图标
    public ArrayList<WCGSkillLevelObj> skillLevelList = new ArrayList<>();
    public String name;//技能名字(翻译表ID)
    public String desc;//技能描述(翻译表ID)
    public List<Float> desc_args;//技能描述参数


    public WCGSkillLevelObj getLevelInfo(int _lv)
    {
        WCGSkillLevelObj skillInfo = skillLevelList.get(0);
        WCGSkillLevelObj check = null;
        for (int i = 0, max = skillLevelList.size(); i < max; i++)
        {
            check = skillLevelList.get(i);
            if (check.minLevel == _lv)
                return check;

            if (check.minLevel <= _lv && check.minLevel > skillInfo.minLevel)
            {
                skillInfo = check;
                continue;
            }
        }

        return skillInfo;
    }

    @Override
    public long _refId()
    {
        return id;
    }
}
