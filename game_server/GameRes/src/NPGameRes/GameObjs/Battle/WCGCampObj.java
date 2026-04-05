package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefCamp;

import java.util.List;

public class WCGCampObj
{
    public int map_id;//地图ID
    public int camp_id; //势力ID
    public List<Integer> group_list;  //本势力下的Group集合
    public int share_group;   //共享组(目前是单一组状态)
    public DLMapPos end_view_pos;//战斗结束，投降时的聚焦点
    public float end_view_camera_orthographic_size;//战斗结束，摄像头的正交大小

    public void adapt(RefCamp refCamp)
    {
        this.map_id = refCamp.map_id;
        this.camp_id = refCamp.camp_id;
        this.group_list = refCamp.group_list;
        this.share_group = refCamp.share_group;
    }
}