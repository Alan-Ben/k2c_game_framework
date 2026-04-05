package NPGameRes.GameObjs.Battle;

import NPCommon.Util.Mathf;

import java.util.List;


public class DLCommon
{

    public static int roundToInt(float _f)
    {
        return Mathf.RoundToInt(_f);
    }

    public static int toInt(float _f)
    {
        if (_f > 0)
            return (int) (_f + 0.001f);
        else
            return (int) (_f - 0.001f);

    }

    public static boolean checkPointInPolygon(DLMapPos _checkPoint, List<DLMapPos> _pointList)
    {

        int crossings = 0;
        for (int i = 0; i < _pointList.size() - 1; i++)
        {
            DLMapPos pointA = _pointList.get(i);
            DLMapPos pointB = _pointList.get(i + 1);

            if (pointB.x == pointA.x)//线段垂直,直接判断是否在线上
            {
                if (_checkPoint.x == pointA.x)//x重合
                {
                    //直接判断是否在线上
                    if ((_checkPoint.z >= pointA.z && _checkPoint.z <= pointB.z)
                            || (_checkPoint.z >= pointB.z && _checkPoint.z <= pointA.z))
                    {
                        return true;
                    } else
                    {
                        crossings += 2;
                    }
                } else
                {
                    continue;
                }

            } else
            {

                double slope = (pointB.z - pointA.z) * 1.f / (pointB.x - pointA.x);

                boolean cond1 = (pointA.x <= _checkPoint.x) && (_checkPoint.x < pointB.x);
                boolean cond2 = (pointB.x <= _checkPoint.x) && (_checkPoint.x < pointA.x);
                boolean above = ((slope * (_checkPoint.x - pointA.x) + pointA.z) - _checkPoint.z) > 0.0001f;

                if ((cond1 || cond2) && above) crossings++;
            }

        }
        return ((crossings % 2) != 0);
    }
}