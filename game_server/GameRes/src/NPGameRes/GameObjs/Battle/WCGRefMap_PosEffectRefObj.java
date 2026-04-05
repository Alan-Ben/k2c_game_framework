package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefPosEffect;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_PosEffectRefObj
        extends _AWCGBaseMapRefCore<WCGPosEffectRefObj>
{
    public void initData()
    {
        this.clear();
        ArrayList<RefPosEffect> list = new ArrayList<RefPosEffect>();
        RefPosEffect.getMgr().copyValues(list);

        for (RefPosEffect ref : list)
        {
            WCGPosEffectRefObj obj = new WCGPosEffectRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }
}
