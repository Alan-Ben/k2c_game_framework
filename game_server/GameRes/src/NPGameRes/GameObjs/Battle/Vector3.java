package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

public class Vector3 implements _IParseFromStringable
{

    public float x;
    public float y;
    public float z;
    public static Vector3 zero = new Vector3(0, 0, 0);
    public static Vector3 forward = new Vector3(0, 0, 1);

    public Vector3(float _x, float _y, float _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }

    public Vector3(Vector3 _pos)
    {
        x = _pos.x;
        y = _pos.y;
        z = _pos.z;
    }

    public Vector3()
    {
        x = y = z = 0.f;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        String[] valueStr = sValue.split("[:;]");
        if (valueStr.length < 3)
        {
            CommLog.error("read property info err2! : " + valueStr);
            return false;
        }
        this.x = Float.parseFloat(valueStr[0]);
        this.y = Float.parseFloat(valueStr[1]);
        this.z = Float.parseFloat(valueStr[2]);
        return true;
    }

    public static Vector3 Lerp(Vector3 _begionPos, Vector3 _endPos, float f)
    {
        return new Vector3(_begionPos.x + (_endPos.x - _begionPos.x) * f, _begionPos.y + (_endPos.y - _begionPos.y) * f, _begionPos.z + (_endPos.z - _begionPos.z) * f);
    }

    public float magnitude()
    {
        return (float) Math.sqrt(x * x + y * y + z * z);
    }

    public Vector3 sub(Vector3 v)
    {
        return new Vector3(x - v.x, y - v.y, z - v.z);
    }

    public Vector3 div(float v)
    {
        return new Vector3(x / v, y / v, z / v);
    }

    public Vector3 multi(float f)
    {
        return new Vector3(x * f, y * f, z * f);
    }

    public Vector3 plus(Vector3 v)
    {
        return new Vector3(x + v.x, y + v.y, z + v.z);
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        return (a.sub(b)).magnitude();
    }

    /**
     * 平面距离
     */
    public static float DistanceXZ(Vector3 a, Vector3 b)
    {
        return (a.sub(b)).magnitudeXZ();
    }

    private float magnitudeXZ()
    {
        return (float) Math.sqrt(x * x + z * z);
    }

    public void Set(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}
