package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefMapMobRef;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_MobRefObj extends _AWCGBaseMapRefCore<NPMobRefObj>
{

    public void initData()
    {
        this.clear();
        ArrayList<RefMapMobRef> list = new ArrayList<RefMapMobRef>();
        RefMapMobRef.getMgr().copyValues(list);

        for (RefMapMobRef refMobRef : list)
        {
            NPMobRefObj obj = new NPMobRefObj();
            obj.adapt(refMobRef);
            this.add(obj);
        }
    }

}
