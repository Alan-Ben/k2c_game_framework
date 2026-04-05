package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.Battle.Condition.WCGControlStateRefObj;
import NPGameRes.Refs.Battle.RefControlState;

@SuppressWarnings("serial")
public class WCGRefMap_ControlStateObj extends _AWCGBaseMapRefCore<WCGControlStateRefObj>
{

    public void initData()
    {
        this.clear();
        for (RefControlState ref : RefControlState.getMgr().getRefList())
        {
            if (null == ref)
                continue;

            WCGControlStateRefObj obj = new WCGControlStateRefObj();
            obj.adapt(ref);
            add(obj);
        }

    }

}
