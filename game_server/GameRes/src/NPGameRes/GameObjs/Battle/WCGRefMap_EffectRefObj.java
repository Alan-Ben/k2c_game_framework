package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefEffect;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_EffectRefObj extends _AWCGBaseMapRefCore<WCGEffectRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        ArrayList<RefEffect> list = new ArrayList<RefEffect>();
        RefEffect.getMgr().copyValues(list);

        for (RefEffect ref : list)
        {
            WCGEffectRefObj obj = new WCGEffectRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }

}
