package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefMap;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_MapRefObj extends _AWCGBaseMapRefCore<WCGMapRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        ArrayList<RefMap> list = new ArrayList<RefMap>();
        RefMap.getMgr().copyValues(list);

        for (RefMap ref : list)
        {
            WCGMapRefObj obj = new WCGMapRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }

}
