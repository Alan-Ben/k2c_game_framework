package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefMapMob;

import java.util.ArrayList;
import java.util.List;

public class WCGMobInstanceRef
{
    public long mob_instance_id;
    public long mob_id;
    public int group_id;//所属组别
    public DLMapPos pos;
    public List<DLMapPos> patrol_pos = new ArrayList<DLMapPos>();//巡逻点

    public void adapt(RefMapMob ref)
    {
        this.mob_instance_id = ref.mob_instance_id;
        this.mob_id = ref.mob_id;
        this.group_id = ref.group_id;
        this.pos = new DLMapPos(ref.x, ref.y);
        this.patrol_pos = DLMapPos.readPosList(ref.patrol_pos_s);
    }
}