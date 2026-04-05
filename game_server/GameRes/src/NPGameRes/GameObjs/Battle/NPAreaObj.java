package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefMapArea;

import java.util.List;

public class NPAreaObj
{
    public int area_id;// 建造 区域id
    public int groupId;// 所属组别
    public DLMapPos pos_a;
    public DLMapPos pos_b;
    public List<DLMapPos> point_list;

    public void adapt(RefMapArea refArea)
    {
        this.area_id = refArea.area_id;
        this.groupId = refArea.group_id;
        this.pos_a = new DLMapPos(refArea.pos_a_x, refArea.pos_a_y);
        this.pos_b = new DLMapPos(refArea.pos_b_x, refArea.pos_b_y);
        this.point_list = DLMapPos.readPosList(refArea.point_list);
    }
}