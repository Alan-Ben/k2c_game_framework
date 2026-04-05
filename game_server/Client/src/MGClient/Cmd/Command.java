package MGClient.Cmd;

import java.lang.reflect.Method;
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class Command
{
    private String name;
    private String comment;
    private Method cmd;
    private Object excuter;

    public Command(MGClient.Cmd.Annotation.Command annotation, Method m, Object excuter)
    {
        this.name = annotation.command().trim();
        if (this.name.isEmpty())
        {
            this.name = m.getName().toLowerCase();
        }
        this.comment = annotation.comment();
        this.cmd = m;
        this.excuter = excuter;
    }

    public String run(String[] args) throws Exception
    {
        Object[] params = parseParams(args);

        if (cmd.getReturnType() == null)
        {
            cmd.invoke(excuter, params);
            return "";
        }
        Object rtn = cmd.invoke(excuter, params);
        if (rtn == null)
        {
            return "";
        } else
        {
            return rtn.toString();
        }
    }

    private Object[] parseParams(String[] args) throws Exception
    {
        Class<?>[] type = cmd.getParameterTypes();
        if (args.length != type.length)
        {
            throw new Exception("[" + name + "] needs " + type.length + " params but provide " + args.length + " params");
        }
        Object[] params = new Object[cmd.getParameterTypes().length];
        for (int i = 0; i < params.length; ++i)
        {
            params[i] = parseParam(type[i], args[i]);
        }
        return params;
    }

    @SuppressWarnings({"unchecked","rawtypes"})
    private Object parseParam(Class<?> clazz, String param) throws Exception
    {
        if (List.class.isAssignableFrom(clazz))
        {
            List<Object> list;
            if (clazz.isAssignableFrom(List.class))
            {
                list = new ArrayList<>();
            } else
            {
                list = (List<Object>) clazz.newInstance();
            }
            // Type gt = clazz.getGenericSuperclass(); // 得到泛型类型
            // ParameterizedType pt = (ParameterizedType) gt;

            Type[] genericParameterTypes = cmd.getGenericParameterTypes();
            Class parameterArgClass = null;
            for (Type genericParameterType : genericParameterTypes)
            {
                if (genericParameterType instanceof ParameterizedType)
                {
                    ParameterizedType aType = (ParameterizedType) genericParameterType;
                    Type[] parameterArgTypes = aType.getActualTypeArguments();
                    for (Type parameterArgType : parameterArgTypes)
                    {
                        parameterArgClass = (Class) parameterArgType;
                        System.out.println("parameterArgClass = " + parameterArgClass);
                    }
                }
            }

            String[] subparams = param.split(";");
            for (String p : subparams)
            {
                list.add(parseParam(parameterArgClass, p));
            }
            return list;
        } else if (clazz.isArray())
        {
            String[] subparams = param.split(";");
            Object[] array = new Object[subparams.length];
            for (int i = 0; i < subparams.length; ++i)
            {
                array[i] = parseParam(clazz.getComponentType(), subparams[i]);
            }
            return Arrays.copyOf(array, subparams.length, Object[].class);
        } else if (clazz == int.class || clazz == Integer.class)
        {
            return Integer.parseInt(param.trim());
        } else if (clazz == long.class || clazz == Long.class)
        {
            return Long.parseLong(param.trim());
        } else if (clazz == float.class || clazz == Float.class)
        {
            return Float.valueOf(param);
        } else if (clazz == double.class || clazz == Double.class)
        {
            return Double.valueOf(param);
        } else if (clazz == boolean.class || clazz == Boolean.class)
        {
            return Boolean.valueOf(param);
        } else if (clazz == String.class)
        {
            return param;
        } else if (clazz.isEnum())
        {
            return Enum.valueOf((Class) clazz, param);
        } else
        {
            throw new Exception("can not instance " + clazz.getSimpleName() + " define for command:" + name);
        }
    }

    public String toString()
    {
        return name + " " + getParamString() + " " + comment;
    }

    private String getParamString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (Class<?> parameter : cmd.getParameterTypes())
        {
            stringBuilder.append(parameter.getSimpleName().toLowerCase()).append(" ");
        }
        return stringBuilder.toString();
    }

    public String getName()
    {
        return name;
    }
}
