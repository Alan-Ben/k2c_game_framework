package NPCommon.GMCommand;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Context._IContext;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.RunResult;

import java.lang.reflect.Method;
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


/******
 * 描述单个执行方法
 */
public class GMCommand
{
    private String _m_name; //方法名
    private String _m_sComment; //注释
    private Method _m_cmdMethod; //反射的方法
    private Class<?> _m_cmdClass;//主体类

    public GMCommand(ACommand _annotation, Method _m, Class<?> _clazz)
    {
        this._m_name = _annotation.command().trim();
        if (this._m_name.isEmpty())
        {
            this._m_name = _m.getName().toLowerCase();
        }
        this._m_sComment = _annotation.comment();
        this._m_cmdMethod = _m;
        this._m_cmdClass = _clazz;
    }

    /******
     * 执行方法
     * @param _executor
     * @param _handler
     * @param _context
     * @param args
     * @throws Exception
     */
    public void run(CmdExecutorBase _executor, _IRunCallBack _handler, _IContext _context, String[] args) throws Exception
    {
        Object[] params;
        try
        {
            params = parseParams(args);
        } catch (Exception e)
        {
            _handler.onRunOver(false, e.toString());
            return;
        }

        CmdClassBase cmdObj;
        try
        {
            cmdObj = (CmdClassBase) _m_cmdClass.newInstance();
        } catch (Exception e)
        {
            CommLog.error("", e);
            _handler.onRunOver(false, "[ERROR]run failed on create executor Instance," + e.getMessage());
            return;
        }
        cmdObj.setExecutor(_executor);
        cmdObj.setContext(_context);
        cmdObj.setCallBack(_handler);
        Object retObj = _m_cmdMethod.invoke(cmdObj, params);
        if (retObj != null)
        { //执行结果有返回值，说明是同步调用的GM命令，直接取结果，支持String 和 RunResult 类型
            if (retObj instanceof RunResult)
            {
                RunResult result = (RunResult) retObj;
                _handler.onRunOver(result.isSucc(), result.getMsg());
                return;
            } else
            {
                String result = retObj.toString();
                _handler.onRunOver(true, result);
                return;
            }

        } else
        {//这是一个异步调用的GM命令，注册监控任务确定有调用回调

            ALSynTaskManager.getInstance().regTask(() ->
            {
                _IRunCallBack callBack = cmdObj.takeCallBack();
                if (callBack == _handler)//3秒后还未执行异步，执行回调
                {
                    _handler.onRunOver(false, "gm command exec run 3 sec not return!");
                }
            }, 3000);
        }
    }

    /*****
     * 解析字符串输入参数为具体参数列表
     * @param args
     * @return
     * @throws Exception
     */
    private Object[] parseParams(String[] args) throws Exception
    {
        Class<?>[] type = _m_cmdMethod.getParameterTypes();
        if (args.length != type.length)
        {
            throw new Exception("[" + _m_name + "] needs " + type.length + " params but provide " + args.length + " params");
        }
        Object[] params = new Object[_m_cmdMethod.getParameterTypes().length];
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

            Type[] genericParameterTypes = _m_cmdMethod.getGenericParameterTypes();
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
            return Float.valueOf(param.trim());
        } else if (clazz == double.class || clazz == Double.class)
        {
            return Double.valueOf(param.trim());
        } else if (clazz == boolean.class || clazz == Boolean.class)
        {
            return Boolean.valueOf(param.trim());
        } else if (clazz == String.class)
        {
            return param;
        } else if (clazz.isEnum())
        {
            return Enum.valueOf((Class) clazz, param.toUpperCase().trim());
        } else
        {
            throw new Exception("can not instance " + clazz.getSimpleName() + " define for command:" + _m_name);
        }
    }

    public String toString()
    {
        return _m_name + " " + getParamString() + " | " + _m_sComment;
    }

    private String getParamString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (Class<?> parameter : _m_cmdMethod.getParameterTypes())
        {
            stringBuilder.append(parameter.getSimpleName().toLowerCase()).append(" ");
        }
        return stringBuilder.toString();
    }

    public String getName()
    {
        return _m_name;
    }
}
