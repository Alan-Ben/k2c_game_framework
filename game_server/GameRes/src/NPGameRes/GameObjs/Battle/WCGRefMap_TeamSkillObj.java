package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefTeamSkill;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_TeamSkillObj
        extends _AWCGBaseMapRefCore<WCGTeamSkillRefObj>
{
    public void initData()
    {
        this.clear();
        ArrayList<RefTeamSkill> list = new ArrayList<RefTeamSkill>();
        RefTeamSkill.getMgr().copyValues(list);

        for (RefTeamSkill ref : list)
        {
            WCGTeamSkillRefObj obj = new WCGTeamSkillRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }
}
