package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefBullet;

import java.util.ArrayList;

@SuppressWarnings("serial")
public class WCGRefMap_BulletRefObj extends _AWCGBaseMapRefCore<WCGBulletRefObj>
{

    @Override
    public void initData()
    {
        this.clear();
        ArrayList<RefBullet> list = new ArrayList<RefBullet>();
        RefBullet.getMgr().copyValues(list);

        for (RefBullet ref : list)
        {
            WCGBulletRefObj obj = new WCGBulletRefObj();
            obj.adapt(ref);
            this.add(obj);
        }
    }

}
