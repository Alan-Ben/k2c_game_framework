package NPGameRes.GameObjs.Battle;

public class WCGSegmentObj //寻路路径的主线段
{
    public long segment_id;//路径ID
    public int groupId;//组别id
    public int point_id;
    public int next_point_id;
    public int research_distance;//重新寻路距离
    public int area_id;//归属区域id 对应group区域id有效时才可寻路
    public boolean is_research_enable;//在重新搜索寻路时本线段是否有效
}
