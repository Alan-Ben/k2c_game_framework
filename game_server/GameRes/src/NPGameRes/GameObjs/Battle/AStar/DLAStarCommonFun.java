package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.GameObjs.Battle.DLMapPos;

public class DLAStarCommonFun
{
    /*********************
     * 计算某点到终点的权重附加系数
     *
     * @author alzq.z
     * @time Jul 11, 2013 12:00:27 AM
     */
    public static int manhattan(DLMapPos _pos, DLMapPos _destinationPos)
    {
        return manhattan(_pos.x, _pos.z, _destinationPos);
    }

    public static int manhattan(int _x, int _z, DLMapPos _destinationPos)
    {
        int res = 0;

        int dx = Math.abs(_x - _destinationPos.x);
        int dz = Math.abs(_z - _destinationPos.z);

        if (1 == dx && 1 == dz)
        {
            return 2;
        } else if (0 == dz)
        {
            res = dx * dx;
        } else if (0 == dx)
        {
            res = dz * dz;
        } else
        {
            res = ((dx * dx) + (dz * dz));
        }

        return res;
    }


    /*****************
     * 将2个short转化为int对象
     *
     * @author alzq.z
     * @time Feb 19, 2013 11:30:30 AM
     */
    public static int mergeShort(short _s1, short _s2)
    {
        return (((int) _s1) << 16) | (Short) _s2;
    }

    public static int mergeShortNum(int _s1, int _s2)
    {
        return (_s1 << 16) | _s2;
    }
}

