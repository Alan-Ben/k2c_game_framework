package NPCommon.Util;

import NPCommon.Log.CommLog;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

public class JsonElementExtrator
{
    public enum EJsonElementType
    {
        INT,
        LONG,
        FLOAT,
        DOUBLE,
        BOOL,
        SHORT,
        STRING,
        ARRAY,
        OBJECT,
    }

    private static JsonElementExtrator _instance = new JsonElementExtrator();

    public static JsonElementExtrator getInstance()
    {
        return _instance;
    }

    private Object[] _m_extraList = new Object[EJsonElementType.values().length];

    private JsonElementExtrator()
    {
        _m_extraList[EJsonElementType.LONG.ordinal()] = new JsonExtratorType<Long>()
        {
            @Override
            public Long elementToValue(JsonElement element)
            {
                return element.getAsLong();
            }
        };

        _m_extraList[EJsonElementType.INT.ordinal()] = new JsonExtratorType<Integer>()
        {

            @Override
            public Integer elementToValue(JsonElement element)
            {
                return element.getAsInt();
            }
        };

        _m_extraList[EJsonElementType.BOOL.ordinal()] = new JsonExtratorType<Boolean>()
        {

            @Override
            public Boolean elementToValue(JsonElement element)
            {
                return element.getAsBoolean();
            }
        };

        _m_extraList[EJsonElementType.FLOAT.ordinal()] = new JsonExtratorType<Float>()
        {

            @Override
            public Float elementToValue(JsonElement element)
            {
                return element.getAsFloat();
            }
        };

        _m_extraList[EJsonElementType.DOUBLE.ordinal()] = new JsonExtratorType<Double>()
        {

            @Override
            public Double elementToValue(JsonElement element)
            {
                return element.getAsDouble();
            }
        };
        _m_extraList[EJsonElementType.STRING.ordinal()] = new JsonExtratorType<String>()
        {

            @Override
            public String elementToValue(JsonElement element)
            {
                return element.getAsString();
            }
        };

        _m_extraList[EJsonElementType.SHORT.ordinal()] = new JsonExtratorType<Short>()
        {

            @Override
            public Short elementToValue(JsonElement element)
            {
                return element.getAsShort();
            }
        };

        _m_extraList[EJsonElementType.ARRAY.ordinal()] = new JsonExtratorType<JsonArray>()
        {

            @Override
            public JsonArray elementToValue(JsonElement element)
            {
                return element.getAsJsonArray();
            }
        };
        _m_extraList[EJsonElementType.OBJECT.ordinal()] = new JsonExtratorType<JsonObject>()
        {

            @Override
            public JsonObject elementToValue(JsonElement element)
            {
                return element.getAsJsonObject();
            }
        };
    }

    @SuppressWarnings("unchecked")
    public <T> JsonExtratorType<T> getExtrator(EJsonElementType eElementType)
    {
        return (JsonExtratorType<T>) _m_extraList[eElementType.ordinal()];
    }

    public static abstract class JsonExtratorType<T>
    {
        public T getValue(JsonObject obj, String _name, T _default)
        {
            if (null == obj)
            {
                CommLog.error("parse json field:{},json obj is null", _name);
                return _default;
            }
            JsonElement element = obj.get(_name);
            if (null == element)
            {
                CommLog.error("json do not has field:{},json:{}", _name, obj.toString());
                return _default;
            }
            //处理Json传值为null的情况
            if (element.isJsonNull())
            {
                CommLog.error("parse json field:{},json obj is null", _name);
                return _default;
            }
            try
            {
                return elementToValue(element);
            } catch (Exception e)
            {
                CommLog.error("parse json field:{} value:{} convert failed! json:{}", _name, element.toString(), obj.toString());
                return _default;
            }

        }

        public abstract T elementToValue(JsonElement element);

    }
}
