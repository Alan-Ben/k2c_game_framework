package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefDungeon;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_DungeonRefObj extends _AWCGBaseMapRefCore<NPDungeonRefObj>
{

    public void initData()
    {
        this.clear();
        ArrayList<RefDungeon> list = new ArrayList<RefDungeon>();
        RefDungeon.getMgr().copyValues(list);

        for (RefDungeon ref : list)
        {
            NPDungeonRefObj obj = new NPDungeonRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }

}
