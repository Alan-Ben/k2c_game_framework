package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefSkillLevel;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_SkillRefObj extends _AWCGBaseMapRefCore<NPActorSkillRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        ArrayList<RefSkillLevel> list = new ArrayList<RefSkillLevel>();
        RefSkillLevel.getMgr().copyValues(list);

        for (RefSkillLevel ref : list)
        {
            NPActorSkillRefObj obj = this.get(ref.id);
            if (null == obj)
            {
                obj = new NPActorSkillRefObj();
                obj.id = ref.id;
                this.put(ref.id, obj);
            }
            WCGSkillLevelObj levelObj = new WCGSkillLevelObj();
            levelObj.adapt(ref);
            obj.skillLevelList.add(levelObj);
        }
    }

}
