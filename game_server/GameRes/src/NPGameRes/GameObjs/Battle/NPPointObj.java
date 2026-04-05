package NPGameRes.GameObjs.Battle;

public class NPPointObj
{
    public int point_id;
    public DLMapPos pos;
    public int radius;//移动到该点的最小允许范围

    @Override
    public String toString()
    {
        return String.format("%s : %d", pos.toString(), radius);
    }
}