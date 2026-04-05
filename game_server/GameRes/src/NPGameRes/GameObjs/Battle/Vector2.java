package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

public class Vector2 implements _IParseFromStringable
{
    public float x;
    public float y;

    public Vector2(float _x, float _y)
    {
        x = _x;
        y = _y;
    }

    public Vector2()
    {
        x = 0;
        y = 0;
    }

    public static double Distance(Vector2 a, Vector2 b)
    {
        float x = a.x - b.x;
        float y = a.y - b.y;
        return Math.sqrt(x * x + y * y);
    }

    public Vector2 normalized()
    {
        float distance = (float) Math.sqrt(x * x + y * y);
        return new Vector2(x / distance, y / distance);
    }

    public float magnitude()
    {
        return (float) Math.sqrt(x * x + y * y);
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
        {
            return true;
        }
        String[] valueStr = sValue.split("[:;]");
        if (valueStr.length < 2)
        {
            CommLog.error("Vector2 read property info err! : " + sValue);
            return false;
        }
        this.x = Float.parseFloat(valueStr[0]);
        this.y = Float.parseFloat(valueStr[1]);
        return true;
    }
}