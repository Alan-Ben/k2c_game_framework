package NPCommon.Util;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

public class JsonUtil
{
    public static long getLong(JsonObject obj, String _name, long _default)
    {
        return (long) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.LONG).getValue(obj, _name, _default);
    }

    public static long getLong(JsonObject obj, String _name)
    {
        return getLong(obj, _name, 0);
    }

    public static int getInt(JsonObject obj, String _name, int _default)
    {
        return (int) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.INT).getValue(obj, _name, _default);
    }

    public static int getInt(JsonObject obj, String _name)
    {
        return getInt(obj, _name, 0);
    }

    public static float getFloat(JsonObject obj, String _name, float _default)
    {
        return (float) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.FLOAT).getValue(obj, _name, _default);
    }

    public static float getFloat(JsonObject obj, String _name)
    {
        return getFloat(obj, _name, 0.0f);
    }

    public static double getDouble(JsonObject obj, String _name, double _default)
    {
        return (double) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.DOUBLE).getValue(obj, _name, _default);
    }

    public static double getDouble(JsonObject obj, String _name)
    {
        return getDouble(obj, _name, 0.0d);
    }

    public static boolean getBool(JsonObject obj, String _name, boolean _default)
    {
        return (boolean) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.BOOL).getValue(obj, _name, _default);
    }

    public static boolean getBool(JsonObject obj, String _name)
    {
        return getBool(obj, _name, false);
    }


    public static String getString(JsonObject obj, String _name, String _default)
    {
        return (String) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.STRING).getValue(obj, _name, _default);
    }

    public static String getString(JsonObject obj, String _name)
    {
        return getString(obj, _name, "");
    }

    public static short getShort(JsonObject obj, String _name, short _default)
    {
        return (short) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.SHORT).getValue(obj, _name, _default);
    }

    public static short getShort(JsonObject obj, String _name)
    {
        return getShort(obj, _name, (short) 0);
    }

    public static JsonArray getJsonArray(JsonObject obj, String _name, JsonArray _default)
    {
        return (JsonArray) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.ARRAY).getValue(obj, _name, _default);
    }

    public static JsonArray getJsonArray(JsonObject obj, String _name)
    {
        return getJsonArray(obj, _name, null);
    }

    public static JsonObject getJsonObj(JsonObject obj, String _name, JsonObject _default)
    {
        return (JsonObject) JsonElementExtrator.getInstance().getExtrator(JsonElementExtrator.EJsonElementType.OBJECT).getValue(obj, _name, _default);
    }

    public static JsonObject getJsonObj(JsonObject obj, String _name)
    {
        return getJsonObj(obj, _name, null);
    }

}
