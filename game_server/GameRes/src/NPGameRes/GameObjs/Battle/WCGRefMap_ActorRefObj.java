package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefActor;

@SuppressWarnings("serial")
public class WCGRefMap_ActorRefObj extends _AWCGBaseMapRefCore<NPActorRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        for (RefActor ref : RefActor.getMgr().getList())
        {
            NPActorRefObj obj = new NPActorRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }

}
