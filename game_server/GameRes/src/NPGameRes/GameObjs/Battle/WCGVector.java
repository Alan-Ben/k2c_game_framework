package NPGameRes.GameObjs.Battle;

import NPCommon.Util.Mathf;
import ResCommon.Allocator._IResAllocator;

public class WCGVector
{
    public static WCGVector createZero()
    {
        return new WCGVector(0, 0);
    }

    public static WCGVector createForward()
    {
        return new WCGVector(0, 1);
    }

    private WCGFloatValue _m_fX;

    private WCGFloatValue _m_fZ;

    public WCGFloatValue x()
    {
        return _m_fX;
    }

    public WCGFloatValue z()
    {
        return _m_fZ;
    }

    public void setx(WCGFloatValue value)
    {
        _m_fX.setValue(value);
    }

    public void setz(WCGFloatValue value)
    {
        _m_fZ.setValue(value);
    }

    public float xV()
    {
        return _m_fX.fV();
    }

    public float zV()
    {
        return _m_fZ.fV();
    }

    public int xIV()
    {
        return _m_fX.iV();
    }

    public int zIV()
    {
        return _m_fZ.iV();
    }

    public WCGVector()
    {
        _m_fX = new WCGFloatValue(0);
        _m_fZ = new WCGFloatValue(0);
    }

    public WCGVector(int _x, int _z)
    {
        _m_fX = new WCGFloatValue(_x);
        _m_fZ = new WCGFloatValue(_z);
    }

    public WCGVector(WCGFloatValue _x, WCGFloatValue _z)
    {
        _m_fX = new WCGFloatValue(_x);
        _m_fZ = new WCGFloatValue(_z);
    }

    public WCGVector(DLMapPos _pos)
    {
        _m_fX = new WCGFloatValue(_pos.x);
        _m_fZ = new WCGFloatValue(_pos.z);
    }

    public WCGVector(WCGVector _pos)
    {
        _m_fX = new WCGFloatValue(_pos._m_fX);
        _m_fZ = new WCGFloatValue(_pos._m_fZ);
    }

    public WCGVector normalized(_IResAllocator _alloc)
    {
        return Normalize(this, _alloc);
    }

    /**
     * 设置为对应的坐标
     */
    public void setValue(WCGVector _vec)
    {
        _m_fX.setValue(_vec._m_fX);
        _m_fZ.setValue(_vec._m_fZ);
    }

    public void setValue(DLMapPos _pos)
    {
        if (null == _pos)
            return;

        _m_fX.setValue(_pos.x);
        _m_fZ.setValue(_pos.z);
    }

    public void setValue(WCGFloatValue _x, WCGFloatValue _z)
    {
        _m_fX.setValue(_x);
        _m_fZ.setValue(_z);
    }

    public void setValue(int _x, WCGFloatValue _z)
    {
        _m_fX.setValue(_x);
        _m_fZ.setValue(_z);
    }

    public void setValue(WCGFloatValue _x, int _z)
    {
        _m_fX.setValue(_x);
        _m_fZ.setValue(_z);
    }

    public void setValue(int _x, int _z)
    {
        _m_fX.setValue(_x);
        _m_fZ.setValue(_z);
    }

    public static WCGVector sub(WCGVector lhs, WCGVector rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(lhs._m_fX.sub(rhs._m_fX, _alloc), lhs._m_fZ.sub(rhs._m_fZ, _alloc));
    }

    public static WCGVector sub(WCGVector lhs, DLMapPos rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(lhs._m_fX.sub(rhs.x, _alloc), lhs._m_fZ.sub(rhs.z, _alloc));
    }

    public static WCGVector plus(WCGVector lhs, WCGVector rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(lhs._m_fX.add(rhs._m_fX, _alloc), lhs._m_fZ.add(rhs._m_fZ, _alloc));
    }

    public static WCGVector mul(WCGVector lhs, WCGFloatValue rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(lhs._m_fX.mul(rhs, _alloc), lhs._m_fZ.mul(rhs, _alloc));
    }

    public static WCGVector div(WCGVector lhs, WCGFloatValue rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(lhs._m_fX.div(rhs, _alloc), lhs._m_fZ.div(rhs, _alloc));
    }


    public WCGVector add(DLMapPos rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(_m_fX.add(rhs.x, _alloc), _m_fZ.add(rhs.z, _alloc));
    }

    public WCGVector plus(WCGVector rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(this._m_fX.add(rhs._m_fX, _alloc), this._m_fZ.add(rhs._m_fZ, _alloc));
    }

    public WCGVector add(WCGVector rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(_m_fX.add(rhs.x(), _alloc), _m_fZ.add(rhs.z(), _alloc));
    }

    public WCGVector sub(WCGVector rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(this._m_fX.sub(rhs._m_fX, _alloc), this._m_fZ.sub(rhs._m_fZ, _alloc));
    }

    public WCGVector mul(WCGFloatValue rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(_m_fX.mul(rhs, _alloc), _m_fZ.mul(rhs, _alloc));
    }

    public WCGVector div(WCGFloatValue rhs, _IResAllocator _alloc)
    {
        return _alloc.newVector(this._m_fX.div(rhs, _alloc), this._m_fZ.div(rhs, _alloc));
    }

    public WCGVector div(int _div, _IResAllocator _alloc)
    {
        return _alloc.newVector(this._m_fX.div(_div, _alloc), this._m_fZ.div(_div, _alloc));
    }

    public static WCGVector Normalize(WCGVector _vector, _IResAllocator _alloc)
    {
        WCGFloatValue distance =
                _alloc.newFloatValue(Mathf.Sqrt(_vector._m_fX.mul(_vector._m_fX, _alloc).add(_vector._m_fZ.mul(_vector._m_fZ, _alloc), _alloc).fV()));
        return _alloc.newVector(_vector._m_fX.div(distance, _alloc), _vector._m_fZ.div(distance, _alloc));
    }

    public static WCGVector Normalize(DLMapPos _vector, _IResAllocator _alloc)
    {
        WCGFloatValue distance = _alloc.newFloatValue(Mathf.Sqrt(_vector.x * _vector.x + _vector.z * _vector.z));
        return _alloc.newVector(distance.dived(_vector.x, _alloc), distance.dived(_vector.z, _alloc));
    }

    //从某个点到另一个点   指定百分比的一个点坐标
    //一条线段上指定_percent百分比的一个点坐标
    public static WCGVector Percent(DLMapPos _begin, DLMapPos _end, WCGFloatValue _percent, _IResAllocator _alloc)
    {
        _percent = _percent.Clamp01(_alloc);
        return _alloc.newVector(_percent.mul(_end.x - _begin.x, _alloc).add(_begin.x, _alloc), _percent.mul(_end.z - _begin.z, _alloc).add(_begin.z, _alloc));
    }

    public static WCGVector Percent(WCGVector _begin, DLMapPos _end, WCGFloatValue _percent, _IResAllocator _alloc)
    {
        _percent = _percent.Clamp01(_alloc);
        return _alloc.newVector(_percent.mul(_begin._m_fX.subed(_end.x, _alloc), _alloc).add(_begin._m_fX, _alloc), _percent.mul(_begin._m_fZ.subed(_end.z, _alloc), _alloc).add(_begin._m_fZ, _alloc));
    }

    public static WCGFloatValue Distance(WCGVector _vec, _IResAllocator _alloc)
    {
        return _alloc.newFloatValue(Mathf.Sqrt(_vec._m_fX.mul(_vec._m_fX, _alloc).add(_vec._m_fZ.mul(_vec._m_fZ, _alloc), _alloc).fV()));
    }

    public static WCGFloatValue SqrDistance(WCGVector _vec, _IResAllocator _alloc)
    {
        return _vec._m_fX.mul(_vec._m_fX, _alloc).add(_vec._m_fZ.mul(_vec._m_fZ, _alloc), _alloc);
    }

    //计算两点距离的平方
    public static WCGFloatValue SqrDistance(WCGVector _begin, WCGVector _end, _IResAllocator _alloc)
    {
        return _begin._m_fX.sub(_end._m_fX, _alloc).pow2(_alloc).add(_begin._m_fZ.sub(_end._m_fZ, _alloc).pow2(_alloc), _alloc);
    }

    public static WCGFloatValue SqrDistance(WCGVector _begin, DLMapPos _end, _IResAllocator _alloc)
    {
        return _begin._m_fX.subed(_end.x, _alloc).pow2(_alloc).add(_begin._m_fZ.subed(_end.z, _alloc).pow2(_alloc), _alloc);
    }

    //计算两点距离的平方
    public static WCGFloatValue SqrDistance(DLMapPos _begin, WCGVector _end, _IResAllocator _alloc)
    {
        return _end._m_fX.subed(_begin.x, _alloc).pow2(_alloc).add(_end._m_fZ.subed(_begin.z, _alloc).pow2(_alloc), _alloc);
    }

    public static int SqrDistance(int _srcX, int _srcY, int _destX, int _destY)
    {
        return (_srcX - _destX) * (_srcX - _destX) + (_srcY - _destY) * (_srcY - _destY);
    }

    public static WCGFloatValue Dot(WCGVector _pointA, WCGVector _pointB, _IResAllocator _alloc)
    {
        return _pointA._m_fX.mul(_pointB._m_fX, _alloc).add(_pointA._m_fZ.mul(_pointB._m_fZ, _alloc), _alloc);
    }

    public static WCGFloatValue Dot(WCGVector _pointA, DLMapPos _pointB, _IResAllocator _alloc)
    {
        return _pointA._m_fX.mul(_pointB.x, _alloc).add(_pointA._m_fZ.mul(_pointB.z, _alloc), _alloc);
    }

    public static int Dot(DLMapPos _pointA, DLMapPos _pointB)
    {
        return _pointA.x * _pointB.x + _pointA.z * _pointB.z;
    }

    public static WCGVector CrossPoint(WCGVector _pointA, WCGVector _pointB, DLMapPos _crossPoint, _IResAllocator _alloc)
    {
        return CrossPoint(_pointA._m_fX, _pointA._m_fZ, _pointB._m_fX, _pointB._m_fZ, _crossPoint.x, _crossPoint.z, _alloc);
    }

    public static WCGVector CrossPoint(WCGFloatValue _ax, WCGFloatValue _ay, WCGFloatValue _bx, WCGFloatValue _by, int _cx, int _cy, _IResAllocator _alloc)
    {
        WCGVector cross = _alloc.newVector(0, 0);
        if (_bx.equal(_ax))
        {
            cross.setValue(_bx, _cy);
            return cross;
        }

        if (_by.equal(_ay))
        {
            cross.setValue(_cx, _by);
            return cross;
        }

        WCGFloatValue K1 = _by.sub(_ay, _alloc).div(_bx.sub(_ax, _alloc), _alloc);

        WCGFloatValue K2 = K1.dived(-1, _alloc);

        cross._m_fX.setValue((_by.sub(_cy, _alloc).sub(K1.mul(_bx, _alloc).sub(K2.mul(_cx, _alloc), _alloc), _alloc)).div(K2.sub(K1, _alloc), _alloc));
        cross._m_fZ.setValue(K1.mul(cross._m_fX.sub(_bx, _alloc), _alloc).add(_by, _alloc));

        return cross;
    }

    public static WCGVector CrossPoint(DLMapPos _pointA, WCGVector _pointB, DLMapPos _crossPoint, _IResAllocator _alloc)
    {
        return CrossPoint(_pointA.x, _pointA.z, _pointB._m_fX, _pointB._m_fZ, _crossPoint.x, _crossPoint.z, _alloc);
    }

    public static WCGVector CrossPoint(int _ax, int _ay, WCGFloatValue _bx, WCGFloatValue _by, int _cx, int _cy, _IResAllocator _alloc)
    {
        WCGVector cross = _alloc.newVector();
        if (_bx.equal(_ax))
        {
            cross.setValue(_bx, _cy);
            return cross;
        }

        if (_by.equal(_ay))
        {
            cross.setValue(_cx, _by);
            return cross;
        }

        WCGFloatValue K1 = _by.sub(_ay, _alloc).div(_bx.sub(_ax, _alloc), _alloc);

        WCGFloatValue K2 = K1.dived(-1, _alloc);

        cross._m_fX.setValue((_by.sub(_cy, _alloc).sub(K1.mul(_bx, _alloc).sub(K2.mul(_cx, _alloc), _alloc), _alloc)).div(K2.sub(K1, _alloc), _alloc));
        cross._m_fZ.setValue(K1.mul(cross._m_fX.sub(_bx, _alloc), _alloc).add(_by, _alloc));

        return cross;
    }

    public static WCGVector CrossPoint(DLMapPos _pointA, DLMapPos _pointB, DLMapPos _crossPoint, _IResAllocator _alloc)
    {
        return CrossPoint(_pointA.x, _pointA.z, _pointB.x, _pointB.z, _crossPoint.x, _crossPoint.z, _alloc);
    }

    public static WCGVector CrossPoint(int _ax, int _ay, int _bx, int _by, int _cx, int _cy, _IResAllocator _alloc)
    {
        WCGVector cross = _alloc.newVector();
        if (_bx == _ax)
        {
            cross.setValue(_bx, _cy);
            return cross;
        }

        if (_by == _ay)
        {
            cross.setValue(_cx, _by);
            return cross;
        }

        WCGFloatValue K1 = _alloc.newFloatValue(_by).sub(_ay, _alloc).div(_alloc.newFloatValue(_bx).sub(_ax, _alloc), _alloc);

        WCGFloatValue K2 = K1.dived(-1, _alloc);

        cross._m_fX.setValue((_alloc.newFloatValue(_by).sub(_cy, _alloc).sub(K1.mul(_bx, _alloc).sub(K2.mul(_cx, _alloc), _alloc), _alloc)).div(K2.sub(K1, _alloc), _alloc));
        cross._m_fZ.setValue(K1.mul(cross._m_fX.sub(_bx, _alloc), _alloc).add(_by, _alloc));

        return cross;
    }

    public static WCGVector ClampMagnitude(WCGVector _vec, WCGFloatValue _length, _IResAllocator _alloc)
    {
        if (SqrDistance(_vec, _alloc).v() <= _length.pow2(_alloc).v())
            return _vec;

        WCGVector vec = _vec.normalized(_alloc);
        vec._m_fX.setValue(vec._m_fX.mul(_length, _alloc));
        vec._m_fZ.setValue(vec._m_fZ.mul(_length, _alloc));
        return vec;
    }

    public String toString()
    {
        return String.format("(%d,%d)", _m_fX.fV(), _m_fZ.fV());
    }

    public DLMapPos ToMapPos()
    {
        return new DLMapPos(_m_fX.iV(), _m_fZ.iV());
    }


    public Vector3 ToVector3(_IDLBasicMapData _map)
    {
        return new Vector3(_m_fX.fV(), _map.getPointHeight(_m_fX.iV(), _m_fZ.iV()) * 0.01f, _m_fZ.fV());
    }

    public Vector3 ToVector3(float _y)
    {
        return new Vector3(_m_fX.fV(), _y, _m_fZ.fV());
    }

    public Vector2 ToVector2()
    {
        return new Vector2(_m_fX.fV(), _m_fZ.fV());
    }

//    //从带入的字符串内读取信息
//    public static List<WCGVector> readPosList (String _str) {
//        List<WCGVector> posList = new ArrayList<WCGVector>();
//        String[] subStrArray = WCGCommonFunc.charSplit(_str, ';');
//        for (int i = 0; i < subStrArray.length; i++) {
//            String subStr = subStrArray[i].trim();
//            if (subStr==null || subStr.isEmpty()) {
//                continue;
//            }
//
//            String[] valueStr = WCGCommonFunc.charSplit(subStr,':');
//            if (valueStr.length < 2) {
//                CommLog.error("read property info err! : " + valueStr);
//                continue;
//            }
//            posList.add(new WCGVector(new WCGFloatValue(Float.parseFloat(valueStr[0])), new WCGFloatValue(Float.parseFloat(valueStr[1]))));
//        }
//        return posList;
//    }

    // override object.Equals
    public boolean Equals(Object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null)
        {
            return false;
        }
        if (!(obj instanceof WCGVector))
        {
            return false;
        }

        WCGVector vec = (WCGVector) obj;
        return _m_fX.equal(vec._m_fX) && _m_fZ.equal(vec._m_fZ);
    }

}
