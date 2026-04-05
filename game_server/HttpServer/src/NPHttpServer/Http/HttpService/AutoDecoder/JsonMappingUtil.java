package NPHttpServer.Http.HttpService.AutoDecoder;

import NPCommon.Log.CommLog;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.lang.reflect.Field;
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * @description: 基于注解的JSON映射工具类
 * 核心特性：
 * 1. 自动反射映射JSON到Java对象
 * 2. 支持嵌套对象和集合类型
 * 3. 字段缓存提升性能
 * 4. 详细的错误处理和日志
 * 5. 类型安全转换
 * @author: claude
 * // * @date: 2025-08-06
 */
public class JsonMappingUtil
{

    // 字段缓存，避免重复反射
    private static final Map<Class<?>, List<FieldInfo>> FIELD_CACHE = new ConcurrentHashMap<>();

    /**
     * 从JSON字符串映射到Java对象
     * @param jsonStr JSON字符串
     * @param clazz   目标类型
     * @param <T>     泛型类型
     * @return 映射后的对象
     */
    public static <T> T fromJson(String jsonStr, Class<T> clazz)
    {
        if (jsonStr == null || jsonStr.trim().isEmpty())
        {
            CommLog.error("[JsonMapping] 输入JSON字符串为空");
            return null;
        }

        try
        {
            JsonElement jsonElement = new JsonParser().parse(jsonStr);
            if (!jsonElement.isJsonObject())
            {
                CommLog.error("[JsonMapping] JSON根元素不是对象类型");
                return null;
            }

            return fromJsonObject(jsonElement.getAsJsonObject(), clazz);
        } catch (Exception e)
        {
            CommLog.error("[JsonMapping] JSON解析失败: {}", e.getMessage());
            return null;
        }
    }

    /**
     * 从JsonObject映射到Java对象
     * @param jsonObj JSON对象
     * @param clazz   目标类型
     * @param <T>     泛型类型
     * @return 映射后的对象
     */
    public static <T> T fromJsonObject(JsonObject jsonObj, Class<T> clazz)
    {
        if (jsonObj == null || clazz == null)
        {
            CommLog.error("[JsonMapping] JsonObject或目标类型为null");
            return null;
        }

        try
        {
            // 创建目标对象实例
            T instance = clazz.getDeclaredConstructor().newInstance();

            // 获取字段映射信息
            List<FieldInfo> fieldInfos = getFieldInfos(clazz);

            // 逐个字段进行映射
            for (FieldInfo fieldInfo : fieldInfos)
            {
                try
                {
                    mapField(jsonObj, instance, fieldInfo);
                } catch (Exception e)
                {
                    if (fieldInfo.required)
                    {
                        CommLog.error("[JsonMapping] 必填字段映射失败: {}, 原因: {}", fieldInfo.jsonFieldName, e.getMessage());
                        return null;
                    } else
                    {
                        CommLog.warn("[JsonMapping] 可选字段映射失败: {}, 原因: {}", fieldInfo.jsonFieldName, e.getMessage());
                    }
                }
            }

            return instance;
        } catch (Exception e)
        {
            CommLog.error("[JsonMapping] 对象创建失败: {}", clazz.getName());
            return null;
        }
    }

    /**
     * 映射单个字段
     */
    private static void mapField(JsonObject jsonObj, Object instance, FieldInfo fieldInfo) throws Exception
    {
        Field field = fieldInfo.field;
        String jsonFieldName = fieldInfo.jsonFieldName;

        // 检查JSON中是否存在该字段
        if (!jsonObj.has(jsonFieldName))
        {
            if (fieldInfo.required)
            {
                throw new IllegalArgumentException("缺少必填字段: " + jsonFieldName);
            }

            // 使用默认值
            if (!fieldInfo.defaultValue.isEmpty())
            {
                setFieldValue(instance, field, fieldInfo.defaultValue, field.getType());
            }
            return;
        }

        JsonElement jsonElement = jsonObj.get(jsonFieldName);
        if (jsonElement.isJsonNull())
        {
            if (fieldInfo.required)
            {
                throw new IllegalArgumentException("必填字段为null: " + jsonFieldName);
            }
            return;
        }

        // 根据字段类型进行映射
        Object value = convertJsonElementToValue(jsonElement, field.getType(), field.getGenericType());
        if (value != null)
        {
            field.setAccessible(true);
            field.set(instance, value);
        }
    }

    /**
     * 转换JsonElement到Java值
     */
    private static Object convertJsonElementToValue(JsonElement element, Class<?> fieldType, Type genericType) throws Exception
    {
        if (element.isJsonNull())
        {
            return null;
        }

        // 基本类型处理
        if (fieldType == String.class)
        {
            return element.getAsString();
        } else if (fieldType == int.class || fieldType == Integer.class)
        {
            return element.getAsInt();
        } else if (fieldType == long.class || fieldType == Long.class)
        {
            return element.getAsLong();
        } else if (fieldType == boolean.class || fieldType == Boolean.class)
        {
            return element.getAsBoolean();
        } else if (fieldType == float.class || fieldType == Float.class)
        {
            return element.getAsFloat();
        } else if (fieldType == double.class || fieldType == Double.class)
        {
            return element.getAsDouble();
        } else if (fieldType == short.class || fieldType == Short.class)
        {
            return element.getAsShort();
        } else if (fieldType == byte.class || fieldType == Byte.class)
        {
            return element.getAsByte();
        }

        // List类型处理
        else if (List.class.isAssignableFrom(fieldType) && element.isJsonArray())
        {
            return convertJsonArrayToList(element.getAsJsonArray(), genericType);
        }

        // 自定义对象类型处理
        else if (element.isJsonObject())
        {
            return fromJsonObject(element.getAsJsonObject(), fieldType);
        }

        // 其他类型尝试字符串转换
        else if (element.isJsonPrimitive())
        {
            return convertStringToType(element.getAsString(), fieldType);
        }

        CommLog.warn("[JsonMapping] 不支持的字段类型: {}", fieldType.getName());
        return null;
    }

    /**
     * 转换JsonArray到List
     */
    private static List<?> convertJsonArrayToList(JsonArray jsonArray, Type genericType) throws Exception
    {
        if (!(genericType instanceof ParameterizedType))
        {
            CommLog.warn("[JsonMapping] List字段缺少泛型信息");
            return new ArrayList<>();
        }

        ParameterizedType parameterizedType = (ParameterizedType) genericType;
        Type[] actualTypeArguments = parameterizedType.getActualTypeArguments();
        if (actualTypeArguments.length == 0)
        {
            return new ArrayList<>();
        }

        Class<?> itemType = (Class<?>) actualTypeArguments[0];
        List<Object> list = new ArrayList<>();

        for (JsonElement element : jsonArray)
        {
            Object item = convertJsonElementToValue(element, itemType, itemType);
            if (item != null)
            {
                list.add(item);
            }
        }

        return list;
    }

    /**
     * 字符串转换为指定类型
     */
    private static Object convertStringToType(String str, Class<?> type)
    {
        if (str == null || str.trim().isEmpty())
        {
            return null;
        }

        try
        {
            if (type == String.class)
            {
                return str;
            } else if (type == int.class || type == Integer.class)
            {
                return Integer.parseInt(str.trim());
            } else if (type == long.class || type == Long.class)
            {
                return Long.parseLong(str.trim());
            } else if (type == boolean.class || type == Boolean.class)
            {
                return Boolean.parseBoolean(str.trim());
            } else if (type == float.class || type == Float.class)
            {
                return Float.parseFloat(str.trim());
            } else if (type == double.class || type == Double.class)
            {
                return Double.parseDouble(str.trim());
            }
        } catch (NumberFormatException e)
        {
            CommLog.warn("[JsonMapping] 字符串转换失败: {} -> {}", str, type.getSimpleName());
        }

        return null;
    }

    /**
     * 设置字段默认值
     */
    private static void setFieldValue(Object instance, Field field, String defaultValue, Class<?> fieldType) throws Exception
    {
        if (defaultValue.isEmpty())
        {
            return;
        }

        Object value = convertStringToType(defaultValue, fieldType);
        if (value != null)
        {
            field.setAccessible(true);
            field.set(instance, value);
        }
    }

    /**
     * 获取类的字段映射信息（带缓存）
     */
    private static List<FieldInfo> getFieldInfos(Class<?> clazz)
    {
        return FIELD_CACHE.computeIfAbsent(clazz, JsonMappingUtil::extractFieldInfos);
    }

    /**
     * 提取类的字段映射信息
     */
    private static List<FieldInfo> extractFieldInfos(Class<?> clazz)
    {
        List<FieldInfo> fieldInfos = new ArrayList<>();
        Field[] fields = clazz.getDeclaredFields();

        for (Field field : fields)
        {
            JsonField annotation = field.getAnnotation(JsonField.class);
            if (annotation == null || annotation.ignore())
            {
                continue; // 跳过没有注解或标记为忽略的字段
            }

            FieldInfo info = new FieldInfo();
            info.field = field;
            info.jsonFieldName = annotation.value().isEmpty() ? field.getName() : annotation.value();
            info.required = annotation.required();
            info.defaultValue = annotation.defaultValue();
            info.description = annotation.description();

            fieldInfos.add(info);
        }

        CommLog.debug("[JsonMapping] 类 {} 缓存字段映射信息: {} 个字段", clazz.getSimpleName(), fieldInfos.size());
        return fieldInfos;
    }

    /**
     * 字段映射信息
     */
    private static class FieldInfo
    {
        Field field;
        String jsonFieldName;
        boolean required;
        String defaultValue;
        String description;
    }
}