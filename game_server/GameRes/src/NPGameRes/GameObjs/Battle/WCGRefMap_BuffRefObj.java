package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefBuff;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_BuffRefObj extends _AWCGBaseMapRefCore<NPActorBuffRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        ArrayList<RefBuff> list = new ArrayList<RefBuff>();
        RefBuff.getMgr().copyValues(list);

        for (RefBuff refBuff : list)
        {
            NPActorBuffRefObj obj = new NPActorBuffRefObj();
            obj.adapt(refBuff);
            this.add(obj);
        }

    }

}
