package NPCommon.HotLoad;


import JavaAgent.JavaAgentMain;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.CommClass;
import com.sun.tools.attach.VirtualMachine;

import java.io.File;
import java.io.FileInputStream;
import java.lang.instrument.ClassDefinition;
import java.lang.instrument.Instrumentation;
import java.lang.management.ManagementFactory;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.Set;

/********
 * 代码热更辅助类，可以处理单个Class的加载和重定义，jar包的加载
 */
public class HotLoadHelper
{
    private static String jarPath;
    private static VirtualMachine vm;
    private static String pid;

    static
    {
        try
        {
            jarPath = getJarPath();
            CommLog.info("java agent:jarPath:{}", jarPath);
            // 当前进程pid
            String name = ManagementFactory.getRuntimeMXBean().getName();
            pid = name.split("@")[0];
            CommLog.info("当前进程pid：{}", pid);
        } catch (Exception e)
        {
            CommLog.error("HotLoadHelper initialize failed", e);
        }

    }

    /**
     * 获取jar包路径
     * @return
     */
    public static String getJarPath()
    {
//        URL url = WCGVersion.class.getProtectionDomain().getCodeSource().getLocation();
//        String filePath = null;
//        try {
//            filePath = URLDecoder.decode(url.getPath(), "utf-8");// 转化为utf-8编码
//        } catch (Exception e) {
//            e.printStackTrace();
//        }
//        if (filePath.endsWith(".jar")) {// 可执行jar包运行的结果里包含".jar"
//            // 截取路径中的jar包名
//            filePath = filePath.substring(0, filePath.lastIndexOf("/") + 1);
//        }
//
//        File file = new File(filePath);
//
//        filePath = file.getAbsolutePath();//得到windows下的正确路径

        File file = new File("../JavaAgent");

        return file.getAbsolutePath();
    }


    public static Result init()
    {
        // 虚拟机加载
        try
        {
            CommLog.info("start attach to pid:{} ...", pid);
            vm = VirtualMachine.attach(pid);

            String agentPath = jarPath + "/JavaAgent.jar";
            CommLog.info("start load agent:{}...", agentPath);
            vm.loadAgent(agentPath);

            Instrumentation instrumentation = JavaAgentMain.getInstrumentation();
            if (null == instrumentation)
            {
                return Result.failed("initInstrumentation must not be null");

            }
            return Result.succ("vm init ok");
        } catch (Throwable e)
        {
            CommLog.error("init VirtualMachine.attach failed", e);
            return Result.failed("init VirtualMachine.attach failed:"+e.getMessage());
        }

    }

    private static boolean destroy()
    {
        if (vm != null)
        {
            try
            {
                vm.detach();
                vm = null;
                return true;
            } catch (Throwable e)
            {
                CommLog.error("vm destory faild", e);
                return false;
            }
        } else
        {
            return true;
        }
    }

    public static boolean checkEvn()
    {
        Result result = init();
        if (!result.isSucc())
        {
            CommLog.error("HotLoadHelper init failed:{}", result.getMsg());
            return false;
        }
        return destroy();

    }

    /**
     * 重新加载类
     * @param classArr
     * @throws Exception
     */
    public static Result hotLoadClass(String root, String[] classArr)
    {
        boolean isFinalSuccess = false;

        StringBuilder sb = new StringBuilder();
        Result initResult = init();
        if (!initResult.isSucc())
            return initResult;
        try
        {
            // 1.整理需要重定义的类
            boolean isAllSuc = true;
            int succCount = 0;
            int failedCount = 0;
            for (String className : classArr)
            {
                if (null == className || className.trim().isEmpty())
                    continue;
                Result result = loadOrRedefineClass(root, className);
                if (!result.isSucc())
                {
                    sb.append(result.getMsg()).append("\n");
                    failedCount++;
                } else
                {
                    succCount++;
                }
                //有一个加载错误，则标记本次加载失败
                if (isAllSuc && !result.isSucc())
                    isAllSuc = false;
            }
            sb.append("succ:" + succCount + " failed:" + failedCount + " ");

            //全部处理完成则热更版本号，通过监控来确认是否更新成功
            if (isAllSuc)
            {
                String oldVer = NPVersion.getString();
                Result hotLoadVerResult = loadOrRedefineClass(root, "NPCommon.NPVersion");
                if (hotLoadVerResult.isSucc())
                {
                    sb.append("ver:" + oldVer + " --> " + NPVersion.getString());
                    isFinalSuccess = true;
                } else
                {
                    sb.append(hotLoadVerResult.getMsg()).append("\n");
                    sb.append("ver update failed:" + NPVersion.getString());
                }
            }
        } catch (Throwable e)
        {
            sb.append("[HOT]hotLoadClass failed:").append(e.getMessage());
            return Result.failed(CommErr.SYS_ERR.getCode(),sb.toString());
        } finally
        {
            destroy();
        }
        if (isFinalSuccess)
        {
            return Result.succ(sb.toString());
        }
        else
            return Result.failed(CommErr.SYS_ERR.getCode(),sb.toString());
    }


    /********
     * 从文件中加载Class
     * @param root 文件所在根路径
     * @param className //类名，例如 MGCommon.HotLoad.HotLoadHelper 不带.class 后缀
     * @return
     */
    public static Result loadOrRedefineClass(String root, String className)
    {
        className = className.trim();
        String classPath = root + className.replace(".", "/") + ".class";
        byte[] bytesFromFile;
        try
        {
            FileInputStream is = new FileInputStream(classPath);
            bytesFromFile = new byte[is.available()];
            is.read(bytesFromFile);
            is.close();
        } catch (Exception e)
        {
            String msg = String.format("[HOT]read file:%s failed,exception:%s", classPath, e.getMessage());
            CommLog.error(msg, e);
            return Result.failed(CommErr.FILE_READ_ERR.getCode(),msg);

        }
        return loadOrRedefineClass(bytesFromFile, className);

    }

    /****
     * 从字节数组中加载类
     * @param bytesFromFile 要加载的字节数组
     * @param className  //类名，例如 MGCommon.HotLoad.HotLoadHelper 不带.class 后缀
     * @return
     */
    public static Result loadOrRedefineClass(byte[] bytesFromFile, String className)
    {
        className = className.trim();
        Class<?> c = null;
        try
        {
            c = Class.forName(className, true, ClassLoader.getSystemClassLoader());
        } catch (Throwable e)
        {
        }
        if (c != null)
        { //类已经存在了，使用虚拟机重新定义类的方法
            CommLog.info("[HOT]about to redefined class:" + className);
            try
            {
                ClassDefinition classDefinition = new ClassDefinition(c, bytesFromFile);
                JavaAgentMain.getInstrumentation().redefineClasses(classDefinition);
            } catch (Exception e)
            {
                String err = String.format("[HOT]redefine class:%s failed!,exception:%s", className, e.getMessage());
                return Result.failed(err).log();
            }
            Result result = tryExecuteClass(c);
            if(!result.isSucc())
            {
                String err = String.format("[HOT]redefine class:%s succ but, execute failed!,err:%s", className, result.getMsg());
                return Result.failed(err).log();
            }
            String msg = String.format("[HOT]class %s redefine ok!, result str: %s", className, result.getMsg());
            return Result.succ(msg).log();

        } else
        {//类不存在，使用App Class Loader 加载新的类，把字节码转化为Class

            //使用系统类加载对象进行加载
            ClassLoader loader = ClassLoader.getSystemClassLoader();
            try
            {
                byte[] arr = new byte[0];
                Method m = ClassLoader.class.getDeclaredMethod("defineClass", String.class, arr.getClass(), int.class, int.class);
                //在访问私有方法前设置访问操作(不设置直接调用会报错)
                m.setAccessible(true);
                m.invoke(loader, className, bytesFromFile, 0, bytesFromFile.length);
            } catch (Exception e)
            {
                String msg = String.format("[HOT]load class:%s ,load failed!,class loader:%s ClassLoader.defineClass() invoke exception:%s", className, loader.getClass().getSimpleName(), e.getMessage());
                CommLog.info(msg, e);
                return Result.failed(msg);
            }
            try
            {
                Class<?> clazz = null;
                try
                {
                    clazz = Class.forName(className, true, loader);
                } catch (Throwable e)
                {
                }

                if (null != clazz)
                {//类加载成功
                    Result exeResult =tryExecuteClass(clazz);//执行加载后处理函数
                    if(!exeResult.isSucc())
                    {
                        String err = String.format("[HOT]load class:%s succ but ,execute failed!,err:%s", className, exeResult.getMsg());
                        return Result.failed(err).log();
                    }
                } else
                {
                    String msg = String.format("[HOT]load class:%s over,but class.forName() got null", className);
                    return Result.failed(msg).log();
                }
            } catch (Exception e)
            {
                String msg = String.format("[HOT]load class:%s Class.forName() caught exception:%s!,", className, e.getMessage());
                return Result.failed(msg).log();
            }
            String msg = String.format("[HOT]class %s load ok!", className);
            return Result.succ(msg).log();
        }

    }

    private static Result tryExecuteClass(Class<?> c)
    {
        if (!_IInitOnClassLoadedable.class.isAssignableFrom(c)) //没有实现接口
            return Result.succ("");
        if (Modifier.isAbstract(c.getModifiers()) || Modifier.isInterface(c.getModifiers())) //抽象类和接口不能实例化
            return Result.succ("");

        try
        {
            Object obj = c.newInstance();
            return ((_IInitOnClassLoadedable) obj).initOnClassLoaded();
        } catch (Throwable e)
        {
            CommLog.error("",e);
            return Result.failed(e.getMessage());
        }
    }

    public static Result loadJar(String _jarFilePathName,Set<Class<?>> _classSet)
    {
        try
        {
            String filePath = "./" + _jarFilePathName;
            File jarFile = new File(filePath);
            if (!jarFile.exists())
            {
                return Result.failed(filePath + " not exists.").log();
            }
            //URL url = new URL("jar:file:/http://hostname/my.jar!/");
            String jarUrl = String.format("jar:file:%s!/", filePath);
            URL url = new URL(jarUrl);
            ClassLoader sysLoader = ClassLoader.getSystemClassLoader();

            if (!(sysLoader instanceof URLClassLoader))
            {
                String err = String.format("[HOT] Class loader:%s is not a URLClassLoader!", sysLoader.getClass().getName());
                return Result.failed(err).log();
            }
            URLClassLoader classLoader = (URLClassLoader) sysLoader;
            //classLoader.addURL(url); protected 方法无法调用，使用反射调用
            try
            {
                Method m = URLClassLoader.class.getDeclaredMethod("addURL", URL.class);
                //在访问私有方法前设置访问操作(不设置直接调用会报错)
                m.setAccessible(true);
                m.invoke(classLoader, url);
            } catch (Throwable e)
            {
                String msg = String.format("[HOT]class loader:%s add jar url:%s  invoke failed!%s", sysLoader.getClass().getName(), url,e.getMessage());
                return Result.failed(msg).log();
            }

            //加载所有类，jar包被异常也能正常运行
            Set<Class<?>> classSet = CommClass.getClassByPackFromJar(url, "");
            _classSet.addAll(classSet);
            return Result.succ("ok");
        }catch (Throwable e)
        {
            return Result.failed(e.getMessage()).log();
        }
    }

    public static void execClassMethod(String _className, String _methodName, _IRunCallBack _handler)
    {
        try
        {
            ClassLoader sysLoader = ClassLoader.getSystemClassLoader();
            Class<?> clazz;
            try
            {
                clazz = sysLoader.loadClass(_className);
            } catch (Exception e)
            {
                CommLog.error("load class:{} failed", _className, e);
                _handler.onRunOver(false, "[ERROR]load class " + _className + " failed:" + e.toString());
                return;
            }
            Method method;
            try
            {
                Class<?>[] paramClassList = new Class[]{_IRunCallBack.class};
                method = clazz.getDeclaredMethod(_methodName, paramClassList);
            } catch (Exception e)
            {
                CommLog.error(" class:{} method:{} reflect find failed", _className, _methodName, e);
                _handler.onRunOver(false, "[ERROR]method: " + _methodName + " get failed:" + e.toString());
                return;
            }
            try
            {
                Object[] params = new Object[]{_handler};
                method.invoke(null, params);
            } catch (InvocationTargetException e)
            {
                CommLog.error("method: " + _methodName + " invoke failed:", _methodName, e.getTargetException());
                _handler.onRunOver(false, "[ERROR]execJarMethod: " + _className + "." + _methodName + "  failed:" + e.getTargetException().toString());
                return;
            } catch (Exception e)
            {
                CommLog.error(" class:{} method:{} invoke failed", _className, _methodName, e);
                _handler.onRunOver(false, "[ERROR]method: " + _methodName + " invoke failed:" + e.toString());
                return;
            }
            CommLog.info("invoke class:{} method:{} success!", _className, _methodName);

        } catch (Exception e)
        {
            _handler.onRunOver(false, "[ERROR]execJarMethod: " + _className + "." + _methodName + "  failed:" + e.toString());
        }
    }
}