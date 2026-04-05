package NPGameRes.GameObjs.Battle;

public class WCGPathPoint
{
    public WCGVector pos;
    public float radius;// 移动到该点的最小允许范围

    public String toString()
    {
        return String.format("%s : %f", pos.toString(), radius);
    }
}