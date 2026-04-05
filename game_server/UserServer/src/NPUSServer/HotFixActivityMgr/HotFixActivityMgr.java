package NPUSServer.HotFixActivityMgr;

import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.HotLoad.HotLoadHelper;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CommFile;
import NPUSServer.HotActivity.Annotation.HotActivityJar;

import java.io.File;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/*****
 * Us 上热更活动Jar包管理器，防止重复加载相同的Jar包，以包名称来唯一标识
 */
public class HotFixActivityMgr
{
    private static final HotFixActivityMgr _instance = new HotFixActivityMgr();

    public static HotFixActivityMgr getInstance() { return _instance;}

    //是否已经加载所有
    private boolean _m_bHasLoadAll = false;
    private Result _m_rLoadRes = Result.SUCC;

    private final Set<String> _m_alIgnoreJarSet = new HashSet<>();//忽略的jar包列表，有些Jar包被废弃后，加入此列表
    private final Set<String> _m_LoadedJarSet = new HashSet<>();//已加载的jar包列表，避免重复加载

    public int getLoadedJarSize() {return _m_LoadedJarSet.size();}

    private HotFixActivityMgr()
    {
        _m_alIgnoreJarSet.add("xxxxx");//忽略测试活动
    }

    /*****
     * 同步加载所有activities目录下的活动jar包，一般在服务器启动的时候调用
     * @return
     */
    public synchronized Result loadAll()
    {
        //已经加载过，直接返回加载结果
        if(_m_bHasLoadAll)
            return _m_rLoadRes;

        //标记已经加载
        _m_bHasLoadAll = true;

        //获取所有的jar包
        List<File> fileList = new ArrayList<>();
        CommFile.getFileListByPath("./activities", fileList, "jar", false);
        if (fileList.isEmpty())
        {
            _m_rLoadRes = Result.succ("no activities in ./activities");

            return _m_rLoadRes;
        }

        //输出检测到的jar包列表
        StringBuilder sb = new StringBuilder("./activities/ Detected hot jar file list:\n");
        for (File file : fileList)
        {
            sb.append(file.getName());
            sb.append("\n");
        }
        CommLog.info(sb.toString());

        sb = new StringBuilder();
        int errCount = 0;
        int ignoreCount=0;
        int succCount=0;
        //逐个加载
        for (File file : fileList)
        {
            String name = file.getName().substring(0, file.getName().length() - 4);
            String lowerName = name.toLowerCase();
            //跳过忽略的jar包
            if (_m_alIgnoreJarSet.contains(lowerName))
            {
                CommLog.info("Ignore jar:{}", file.getName());
                ignoreCount++;
                continue;
            }
            //加载jar包
            Result result = loadOneSync(file);
            sb.append(String.format("load activity:%s result:%s;", file.getName(), result.toString()));
            if (!result.isSucc())
            {
                errCount++;
            }
            else
            {
                succCount++;
            }
        }

        CommLog.error("load activity jar succ count:{}  failed count:{}  ignore count:{}",succCount, errCount,ignoreCount);
        if (errCount > 0)
            _m_rLoadRes = Result.failed(sb.toString());
         else
            _m_rLoadRes = Result.succ(sb.toString());

         return _m_rLoadRes;
    }

    /*****
     * 异步加载单个热更Jar包，服务器运行中调用
     * @param _jarFile
     * @param _handler
     */
    public void loadOneAsync(File _jarFile, _ICallBackResult _handler)
    {
        ResultOne<Class<?>> resultOne = _loadJarMainClass(_jarFile);
        if (!resultOne.isSucc())
        {
            _handler.onRunOver(resultOne.getResult());
            return;
        }
        Class<?> mainClazz = resultOne.getData();

        Method method;
        String methodName = "runAsync";
        Class<?>[] paramClassList = new Class[]{_ICallBackResult.class};
        try
        {
            method = mainClazz.getDeclaredMethod(methodName, paramClassList);
        } catch (NoSuchMethodException e)
        {
            CommLog.error("", e);
            _handler.onRunOver(Result.failed("not found method:" + methodName + " " + e.getMessage()));
            return;
        }
        Object[] params = new Object[]{_handler};
        try
        {
            method.invoke(null, params);
        } catch (Throwable e)
        {
            CommLog.error("", e);
            _handler.onRunOver(Result.failed("method:" + methodName + " invoke failed! " + e.getMessage()));
            return;
        }
        String name = _jarFile.getName().substring(0, _jarFile.getName().length() - 4);
        String lowerName = name.toLowerCase();
        _m_LoadedJarSet.add(lowerName);
        CommLog.info("invoke class:{} method:{} success!", mainClazz.getName(), methodName);
    }

    /*******
     * 同步加载单个热更Jar包
     * @param _jarFile
     * @return
     */
    public Result loadOneSync(File _jarFile)
    {
        //加载主类
        ResultOne<Class<?>> resultOne = _loadJarMainClass(_jarFile);
        if (!resultOne.isSucc())
        {
            return resultOne.getResult();
        }

        //调用主类的runSync方法
        Class<?> mainClazz = resultOne.getData();
        Method method;
        String methodName = "runSync";
        try
        {
            method = mainClazz.getDeclaredMethod(methodName, (Class<?>[]) null);
        } catch (NoSuchMethodException e)
        {
            CommLog.error("", e);
            return Result.failed("not found method:" + methodName + " " + e.getMessage());
        }

        Object invoke;
        try
        {
            invoke = method.invoke(null);
        } catch (Throwable e)
        {
            CommLog.error("", e);
            return Result.failed("method:" + methodName + " invoke failed! " + e.getMessage());
        }

        if (invoke instanceof Result)
        {
            Result result = (Result) invoke;
            if (result.isSucc())
            {
                String name = _jarFile.getName().substring(0, _jarFile.getName().length() - 4);
                String lowerName = name.toLowerCase();
                _m_LoadedJarSet.add(lowerName);
                CommLog.info("invoke class:{} method:{} success!", mainClazz.getName(), methodName);
            }
            return result;
        } else
        {
            return Result.failed("invoke " + methodName + " return invalid type:" + invoke.getClass().getName());
        }

    }

    /**
     * 加载Jar包的主类。
     * @param _jarFile 要加载的Jar文件
     * @return 加载的Jar包的主类
     */
    private ResultOne<Class<?>> _loadJarMainClass(File _jarFile)
    {
        String name = _jarFile.getName().substring(0, _jarFile.getName().length() - 4);
        String lowerName = name.toLowerCase();
        //已加载直接返回
        if (_m_LoadedJarSet.contains(lowerName))
        {
            return ResultOne.failed("duplicated load jar:" + name);
        }
        //在忽略列表中，直接返回
        if (_m_alIgnoreJarSet.contains(lowerName))
        {
            String msg = String.format("Loading jar file:%s  ignored!", _jarFile.getName());
            return ResultOne.failed(msg);
        }
        //加载Jar包
        Set<Class<?>> classSet = new HashSet<>();
        Result result = HotLoadHelper.loadJar("./activities/" + _jarFile.getName(), classSet);
        if (!result.isSucc())
        {
            return ResultOne.failed(result);
        }
        //查找主类
        Class<?> mainClazz = null;
        for (Class<?> clazz : classSet)
        {
            HotActivityJar annotation = clazz.getAnnotation(HotActivityJar.class);
            if (null != annotation)
            {
                mainClazz = clazz;
                break;
            }
        }
        if (null == mainClazz)
        {
            return ResultOne.failed("Not found main class in jar:" + _jarFile.getName());
        }
        return ResultOne.succ(mainClazz);
    }

}
