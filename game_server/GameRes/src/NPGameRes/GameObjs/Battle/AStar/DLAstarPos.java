package NPGameRes.GameObjs.Battle.AStar;


public class DLAstarPos
{
    public int x;
    public int z;

    public DLAstarPos()
    {
        x = 0;
        z = 0;
    }

    public DLAstarPos(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    /********************
     * 设置值
     *
     * @author alzq.z
     * @time Jun 24, 2013 11:07:10 PM
     */
    public void set(int _x, int _y)
    {
        x = _x;
        z = _y;
    }

    public void clear()
    {
        x = 0;
        z = 0;
    }

    /*************
     * 新的比对函数
     **/
    public boolean equals(int _x, int _z)
    {
        if (x == _x && z == _z)
            return true;

        return false;
    }

    @Override
    public String toString()
    {
        return String.format("(%d,%d)", x, z);
    }
}