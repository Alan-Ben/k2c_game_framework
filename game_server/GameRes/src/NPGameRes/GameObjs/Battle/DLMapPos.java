package NPGameRes.GameObjs.Battle;

import Common.Common_FloatVec;
import Common.Common_MapPos;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Mathf;
import NPGameRes.GameObjs.Battle.AStar.DLAStarCommonFun;
import NPGameRes.GameObjs.Battle.AStar.DLAstarPos;
import ResCommon.Allocator._IResAllocator;

import java.util.ArrayList;
import java.util.List;

public class DLMapPos implements _IParseFromStringable
{
    private static DLMapPos _g_zero = new DLMapPos(0, 0);

    public static DLMapPos zero()
    {
        return _g_zero;
    }

    public static DLMapPos createZero()
    {
        return new DLMapPos();
    }

    public static WCGFloatValue Distance(DLMapPos _vec, _IResAllocator _alloc)
    {
        return _alloc.newFloatValue(Mathf.Sqrt(_vec.x * _vec.x + _vec.z * _vec.z));
    }

    //计算两点的距离
    public static WCGFloatValue Distance(DLMapPos _begin, DLMapPos _end, _IResAllocator _alloc)
    {
        return _alloc.newFloatValue(Mathf.Sqrt((_begin.x - _end.x) * (_begin.x - _end.x) + (_begin.z - _end.z) * (_begin.z - _end.z)));
    }

    //计算两点的距离
    public static float Distance(DLMapPos _begin, Vector3 _end)
    {
        return Mathf.Sqrt((_begin.x - _end.x) * (_begin.x - _end.x) + (_begin.z - _end.z) * (_begin.z - _end.z));
    }

    public static long SqrDistance(DLMapPos _vec)
    {
        return _vec.x * _vec.x + _vec.z * _vec.z;
    }

    //计算两点距离的平方
    public static long SqrDistance(DLMapPos _begin, DLMapPos _end)
    {
        return (_begin.x - _end.x) * (_begin.x - _end.x) + (_begin.z - _end.z) * (_begin.z - _end.z);
    }

    public static long SqrDistance(int _x, int _z, DLMapPos _end)
    {
        return (_x - _end.x) * (_x - _end.x) + (_z - _end.z) * (_z - _end.z);
    }

    //计算两点距离的平方
    public static float SqrDistance(DLMapPos _begin, Vector3 _end)
    {
        return (_begin.x - _end.x) * (_begin.x - _end.x) + (_begin.z - _end.z) * (_begin.z - _end.z);
    }

    public static float SqrDistance(int _x, int _z, Vector3 _end)
    {
        return (_x - _end.x) * (_x - _end.x) + (_z - _end.z) * (_z - _end.z);
    }

    public static int SqrDistance(int _x, int _z, int _eX, int _eZ)
    {
        return (_x - _eX) * (_x - _eX) + (_z - _eX) * (_z - _eZ);
    }

    //从某个点到另一个点   指定百分比的一个点坐标
    //一条线段上指定_percent百分比的一个点坐标

    public static Vector3 Percent(Vector3 _begin, DLMapPos _end, float _percent, _IDLBasicMapData _map)
    {
        _percent = Mathf.Clamp01(_percent);

        Vector3 pos = new Vector3();
        pos.Set(_begin.x + (_end.x - _begin.x) * _percent
                , 0f
                , _begin.z + (_end.z - _begin.z) * _percent);
        pos.y = _map.getPosHeight(pos.x, pos.z, _end.x, _end.z);

        return pos;
    }

    //从带入的字符串内读取信息
    public static DLMapPos readPos(String _str)
    {
        if (_str == null || _str.isEmpty())
        {
            return DLMapPos.createZero();
        }

        String[] valueStr = CommonFunc.charSplit(_str, ':');
        if (valueStr.length < 2)
        {
            CommLog.error("read property info err1! : " + _str);
            return DLMapPos.createZero();
        }

        return new DLMapPos(Integer.parseInt(valueStr[0]), Integer.parseInt(valueStr[1].trim()));
    }

    public static List<DLMapPos> readPosList(String _str)
    {
        List<DLMapPos> posList = new ArrayList<DLMapPos>();
        String[] subStrArray = CommonFunc.charSplit(_str, ';');
        for (int i = 0; i < subStrArray.length; i++)
        {
            String subStr = subStrArray[i];
            if (subStr == null || subStr.isEmpty())
            {
                continue;
            }

            String[] valueStr = CommonFunc.charSplit(subStr, ':');
            if (valueStr.length < 2)
            {
                CommLog.error("read property info err1! : " + _str);
                continue;
            }
            posList.add(new DLMapPos(Integer.parseInt(valueStr[0]), Integer.parseInt(valueStr[1].trim())));
        }
        return posList;
    }

    public int x;
    public int z;

    public DLMapPos()
    {

        x = 0;
        z = 0;
    }

    public DLMapPos(Vector2 _vector)
    {
        x = DLCommon.roundToInt(_vector.x);
        z = DLCommon.roundToInt(_vector.y);
    }

    public DLMapPos(Vector3 _vector)
    {
        x = DLCommon.roundToInt(_vector.x);
        z = DLCommon.roundToInt(_vector.z);
    }

    public DLMapPos(WCGVector _vector)
    {
        x = _vector.xIV();
        z = _vector.zIV();
    }

    public DLMapPos(DLMapPos _pos)
    {
        x = _pos.x;
        z = _pos.z;
    }

    public DLMapPos(DLAstarPos _pos)
    {
        x = _pos.x;
        z = _pos.z;
    }

    public DLMapPos(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public DLMapPos(float _x, float _z)
    {
        x = DLCommon.toInt(_x);
        z = DLCommon.toInt(_z);
    }

    public DLMapPos(List<Integer> _posArr)
    {
        x = _posArr.get(0);
        z = _posArr.get(1);
    }

    public WCGVector normalized(_IResAllocator _alloc)
    {
        return WCGVector.Normalize(this, _alloc);
    }

    /********************
     * 设置值
     *
     * @author alzq.z
     * @time Jun 24, 2013 11:07:10 PM
     */
    public void set(DLMapPos _vec)
    {
        x = _vec.x;
        z = _vec.z;
    }

    public void set(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    /***************
     * 获取位置标记
     *
     * @author alzq.z
     * @time Jul 11, 2013 12:54:50 AM
     */
    public int getPositionTag()
    {
        return DLAStarCommonFun.mergeShortNum(x, z);
    }

    public DLMapPos clone()
    {
        return new DLMapPos(this);
    }

    public void clear()
    {
        x = 0;
        z = 0;
    }


    public DLMapPos sub(DLMapPos rhs)
    {
        if (null == rhs)
            return this.clone();

        return new DLMapPos(this.x - rhs.x, this.z - rhs.z);
    }

    public void sub(DLMapPos rhs, DLMapPos _outPos)
    {
        _outPos.set(this.x - rhs.x, this.z - rhs.z);
    }

    public WCGVector sub(WCGVector rhs, _IResAllocator _alloc)
    {
        if (null == rhs)
            return _alloc.newVector(this.x, this.z);

        return _alloc.newVector(rhs.x().subed(this.x, _alloc), rhs.z().subed(this.z, _alloc));
    }

    public DLMapPos plus(DLMapPos rhs)
    {
        if (null == rhs)
            return this.clone();

        return new DLMapPos(this.x + rhs.x, this.z + rhs.z);
    }

    public DLMapPos multi(int rhs)
    {
        return new DLMapPos(this.x * rhs, this.z * rhs);
    }

    public DLMapPos div(int rhs)
    {
        return new DLMapPos(this.x / rhs, this.z / rhs);
    }

    public static boolean isEqual(DLMapPos lhs, DLMapPos rhs)
    {
        if (lhs == null && rhs == null)
            return true;
        if (lhs == null || rhs == null)
            return false;
        if (lhs.x == rhs.x && lhs.z == rhs.z)
            return true;
        return false;
    }

    public boolean equals(int _x, int _z)
    {
        if (x == _x && z == _z)
            return true;

        return false;
    }


    /*************
     * 新的比对函数
     **/
    public boolean equals(DLAstarPos _pos)
    {
        if (_pos == null)
            return false;

        if (x == _pos.x && z == _pos.z)
            return true;

        return false;
    }

    public boolean equals(DLMapPos _pos)
    {
        if (null == _pos)
            return false;
        if (x == _pos.x && z == _pos.z)
            return true;
        return false;
    }

    public Vector3 getMapVector(_IDLBasicMapData _map)
    {
        return new Vector3(x, _map.getShowPointHeight(x, z) * 0.01f, z);
    }

    public Vector3 ToVector3(_IDLBasicMapData _map)
    {
        return new Vector3(x, _map.getShowPointHeight(x, z) * 0.01f, z);
    }

    public Vector3 ToVector3(float _y)
    {
        return new Vector3(x, _y, z);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, z);
    }

    @Override
    public String toString()
    {
        return String.format("(%d,%d)", x, z);
    }

    public Common_FloatVec toFloatVec()
    {
        Common_FloatVec forward = new Common_FloatVec();
        forward.setX(this.x);
        forward.setZ(this.z);
        return forward;
    }

    public Common_MapPos toMapPos()
    {
        Common_MapPos forward = new Common_MapPos();
        forward.setX(this.x);
        forward.setZ(this.z);
        return forward;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.isEmpty())
            return true;
        String[] valueStr = CommonFunc.charSplit(sValue, ':');
        if (valueStr.length < 2)
        {
            CommLog.error("read property info err1! : " + sValue);
            return false;
        }
        try
        {
            set(Integer.parseInt(valueStr[0].trim()), Integer.parseInt(valueStr[1].trim()));
        } catch (Exception e)
        {
            CommLog.error("parse DLMapPos failed  for str:{} ", sValue);
            return false;
        }
        return true;

    }

}
