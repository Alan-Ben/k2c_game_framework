package NPUSServer.HotActivity;


import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Dispather._IAutoRegistMsgHandler;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.GMCommand;
import NPCommon.GMCommand.GMCommander;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.Promise.Promise;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.NPRefDataMgr;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefLoaderMgr;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSMain;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserServer;

import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.net.JarURLConnection;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.*;
import java.util.jar.JarEntry;
import java.util.jar.JarFile;

/**********
 * 热更模块的主类的基类，完成活动相关功能的初始化，每个热更Jar必须包含一个此类的子类
 */
public abstract class _AHotActivityMain implements _IHandlerHolder
{
    private final List<_AActivityInitializer> _m_alInitializers = new ArrayList<>();
    private boolean _m_bHasRegistFactory = false;

    /*****
     * 注册一个活动初始化对象，活动初始化对象负责初始化活动的逻辑，包括注册活动工厂类，事件处理，通用排行榜等
     * @param _inInitializer
     */
    public void registActivity(_AActivityInitializer _inInitializer)
    {
        for (_AActivityInitializer initializer : _m_alInitializers)
        {
            if (initializer.getClass().getSimpleName().compareTo(_inInitializer.getClass().getSimpleName()) == 0)
                return;
        }
        _m_alInitializers.add(_inInitializer);

    }

    /************
     * 返回当前Jar包指定pakage里指定接口或抽象类的所有子类列表，当前jar包的名称设定为与Main所在的包名一致，大小写一致
     * @param c
     * @param packageName
     * @return
     */
    @SuppressWarnings("unchecked")
    public <T> List<Class<? extends T>> getPackClassByInterface(Class<? extends T> c, String packageName)
    {
        List<Class<? extends T>> returnClassList = new ArrayList<>(); // 返回结果

        // 如果不是一个接口，则不做处理
        if (c.isInterface() || Modifier.isAbstract(c.getModifiers()))
        {
            Set<Class<?>> allClass = getClassByPack(packageName); // 获得当前包下以及子包下的所有类
            for (Class<?> cs : allClass)
            {
                // 判断是不是相同接口
                if (!c.isAssignableFrom(cs))
                {
                    continue;
                }
                // 不要抽象类
                if (Modifier.isAbstract(cs.getModifiers()))
                {
                    continue;
                }
                // 本身不加进去
                if (c.equals(cs))
                {
                    continue;
                }
                returnClassList.add((Class<? extends T>) cs);
            }
        }
        return returnClassList;
    }

    /******
     * 返回当前Jar包的JarUrl,设定Main类的Package名就是当前Jar包的名称.
     * @return
     */
    private URL lookupThisJarUrl()
    {
        URLClassLoader loader = (URLClassLoader) this.getClass().getClassLoader();
        URL[] urls = loader.getURLs();
        String thisJarName = this.getClass().getPackage().getName() + ".jar";
        for (URL url : urls)
        {
            if (url.getProtocol().compareTo("jar") != 0)
                continue;

            String strUrl = url.getPath();
            if (strUrl.contains(thisJarName))
            {
                return url;
            }
        }
        CommLog.error("can not get this jar:[{}]'s JarUrl in jar list:[{}] ,maybe Case not match!", thisJarName, CommonFunc.listArray2String(urls));
        return null;
    }

    /*********
     * 返回当前Jar包指定pakage下面所有类.
     * @param pack
     * @return
     */
    private Set<Class<?>> getClassByPack(String pack)
    {
        // 第一个class类的集合
        Set<Class<?>> classes = new LinkedHashSet<>();
        // 是否循环迭代
        boolean recursive = true;

        URL jarUrl = lookupThisJarUrl();
        if (null == jarUrl)
        {
            return classes;
        }

        String packageName = pack;
        String packageDirName = packageName.replace('.', '/');
        JarFile jar;
        try
        {
            jar = ((JarURLConnection) jarUrl.openConnection()).getJarFile();
        } catch (Exception e)
        {
            CommLog.error("open jar url failed:{}", jarUrl, e);
            return classes;
        }
        Enumeration<JarEntry> entries = jar.entries();
        // 同样的进行循环迭代
        while (entries.hasMoreElements())
        {
            // 获取jar里的一个实体 可以是目录 和一些jar包里的其他文件 如META-INF等文件
            JarEntry entry = entries.nextElement();
            String name = entry.getName();

            // 如果是以/开头的
            if (name.charAt(0) == '/')
            {
                // 获取后面的字符串
                name = name.substring(1);
            }
            // 如果前半部分和定义的包名相同
            if (name.startsWith(packageDirName))
            {
                int idx = name.lastIndexOf('/');
                // 如果以"/"结尾 是一个包
                if (idx != -1)
                {
                    // 获取包名 把"/"替换成"."
                    packageName = name.substring(0, idx).replace('/', '.');
                }
                // 如果可以迭代下去 并且是一个包
                if ((idx != -1) || recursive)
                {
                    // 如果是一个.class文件 而且不是目录
                    if (name.endsWith(".class") && !entry.isDirectory())
                    {
                        // 去掉后面的".class" 获取真正的类名
                        String className = name.substring(packageName.length() + 1, name.length() - 6);
                        try
                        {
                            // 添加到classes
                            classes.add(this.getClass().getClassLoader().loadClass(packageName + '.' + className));
                        } catch (ClassNotFoundException e)
                        {
                            // log
                            // .error("添加用户自定义视图类错误 找不到此类的.class文件");
                            CommLog.error("ClassNotFoundException:{}", className, e);
                        }
                    }
                }
            }
        }

        return classes;
    }

    /****
     * 初始化数据库表，可以异步执行也可以同步执行
     * 主表
     * 日志表
     * 操作日志表
     * @param _handler
     */
    private void _initTablesAsync(NPUserServer _server, _ICallBackResult _handler)
    {
        List<Class<? extends BaseBO>> boClassList = getPackClassByInterface(BaseBO.class, getBoPackPath());
        List<Class<? extends BaseBO>> logBoClassList = getPackClassByInterface(BaseBO.class, getLogBoPackPath());
        logBoClassList.addAll(getPackClassByInterface(BaseBO.class, getOptLogBoPackPath()));

        Promise promise = new Promise();
        promise.then(1, (p) ->
        {
            _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(_server.getDBInitializer().getDBTag());
            WCGDBVersionChecker checker = new WCGDBVersionChecker(_server.getBM(), tmpObj.getDB());
            checker.checkAndUpdateVersionAsync(_server.getBM(), boClassList, (_bSucc, _msg) ->
            {
                if (!_bSucc)
                {
                    p.err(_msg);
                }
                p.commit(1);
            });

        }).then(2, (p) ->
        {
            _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(_server.getLogDBInitializer().getDBTag());
            WCGDBVersionChecker checker = new WCGDBVersionChecker(_server.getBM(), tmpObj.getDB());
            checker.checkAndUpdateVersionAsync(_server.getBM(), logBoClassList, (_bSucc, _msg) ->
            {
                if (!_bSucc)
                {
                    p.err(_msg);
                }
                p.commit(2);
            });
        }).over(p ->
        {
            if (p.isSucc())
            {
                _handler.onRunOver(Result.SUCC);
            } else
            {
                _handler.onRunOver(Result.failed(p.getErrMsgs()));
            }
        });
    }

    private Result _initTablesSync()
    {
        List<Class<? extends BaseBO>> boClassList = getPackClassByInterface(BaseBO.class, getBoPackPath());
        List<Class<? extends BaseBO>> logBoClassList = getPackClassByInterface(BaseBO.class, getLogBoPackPath());
        logBoClassList.addAll(getPackClassByInterface(BaseBO.class, getOptLogBoPackPath()));

        for (int i = 0; i < NPUSMain.GetServerCount(); i++)
        {
            NPUserServer userServer = NPUSMain.GetServer(i);

            do
            {
                _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(userServer.getDBInitializer().getDBTag());
                WCGDBVersionChecker checker = new WCGDBVersionChecker(userServer.getBM(), tmpObj.getDB());
                if (!checker.checkAndUpdateVersion(userServer.getBM(), boClassList))
                {
                    return Result.failed("us mainDB table check failed");
                }
            } while (false);

            do
            {
                _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(userServer.getLogDBInitializer().getDBTag());
                WCGDBVersionChecker checker = new WCGDBVersionChecker(userServer.getBM(), tmpObj.getDB());
                if (!checker.checkAndUpdateVersion(userServer.getBM(), logBoClassList))
                {
                    return Result.failed("us logDB table check failed");
                }

            } while (false);
        }

        return Result.SUCC;
    }


    /******
     * 初始化整个活动jar包，初始化包括一下内容：
     * 1、初始化数据库表
     * 2、注册活动的事件类型
     * 3、初始化活动的配表
     * 4、注册活动的GM命令
     * 5、对所有的活动初始化对象进行初始化
     * @param _handler //执行后的回调
     * @return
     */
    public void initAsync(_ICallBackResult _handler)
    {
        ALProcess process = ALProcess.CreateProcess("_AHotActivityMain.initAsync");

        //遍历所有的服务器，初始化数据库表
        for (int i = 0; i < NPUSMain.GetServerCount(); i++)
        {
            NPUserServer server = NPUSMain.GetServer(i);

            //初始化数据库表
            process.addResDelegateProcess(_action -> _initTablesAsync(server, (_result) ->
            {
                if (!_result.isSucc())
                {
                    _action.dealAction(false);
                    if (null != _handler)
                    {
                        _handler.onRunOver(_result);
                    }
                } else
                {
                    _action.dealAction(true);
                }
            }), null, false);
        }

        process.addActionProcess(p ->
        {
            Result result = _initCommon();
            if (!result.isSucc() && null != _handler)
            {
                _handler.onRunOver(result);
            }
        });

        process.dealProcess();
    }

    public Result initSync()
    {
        //初始化玩家数据表
        Result result = _initTablesSync();
        if (!result.isSucc())
        {
            return result;
        }

        return _initCommon();
    }

    public Result _initCommon()
    {
        //注册活动类型工厂
        if (!_m_bHasRegistFactory)
        {
            registActivityFactory();
            _m_bHasRegistFactory = true;
        }

        //注册热更活动中的游戏事件
        if (!_initEvents())
        {
            return Result.failed("_initEvents failed");
        }

        //初始化配表
        if (!_initRefs())
        {
            return Result.failed("_initRefs failed");
        }

        //逐个服务器注册消息处理协议，类和接口管理用一个，但是消息的处理需要每个服务器各自注册处理
        for (int i = 0; i < NPUSMain.GetServerCount(); i++)
        {
            if (!_initMsgDealers(NPUSMain.GetServer(i)))
            {
                return Result.failed("server:[" + NPUSMain.GetServer(i).getServerTypeId() + "] _initMsgDealers failed ");
            }
        }

        //初始化GM命令
        _initGmCommands();
        boolean bSucc = true;

        //使用初始化对象进行初始化
        for (_AActivityInitializer initializer : _m_alInitializers)
        {
            if (!initializer.init())
            {
                bSucc = false;
            }
        }
        if (!bSucc)
        {
            return Result.failed("_AActivityInitializer init failed");
        }
        _initListeners();

        return Result.SUCC;
    }

    @SuppressWarnings({"rawtypes"})
    public boolean _initMsgDealers(NPUserServer _server)
    {
        List<Class<?>> clazzs = getPackClassByInterface(_IAutoRegistMsgHandler.class, getMsgDealerPackPath());
        for (Class<?> clazz : clazzs)
        {
            try
            {
                NPUserMsgDealer handler = (NPUserMsgDealer) clazz.newInstance();

                //设置服务器对象
                handler._initUSServer(_server);
                _server.getUSMsgDispatcher().regSubDealer(handler);
            } catch (Throwable e)
            {
                CommLog.error("auto regist msg handler:{} err", clazz.getSimpleName(), e);
                return false;
            }
        }
        return true;

    }


    /******
     * 初始化GM命令，GM命令路径由getGMCommandPackPath()返回
     */
    private void _initGmCommands()
    {
        List<Class<? extends UsCmdBase>> cmdClassList = getPackClassByInterface(UsCmdBase.class, getGMCommandPackPath());
        for (Class<? extends UsCmdBase> commandClazz : cmdClassList)
        {
            if (null == commandClazz)
                continue;

            ACommander aCommander = commandClazz.getAnnotation(ACommander.class);
            if (aCommander == null)
            {
                continue;
            }

            try
            {
                @SuppressWarnings("unused")
                UsCmdBase excuter = commandClazz.newInstance();
            } catch (Exception e)
            {
                CommLog.error("", e);
                continue;
            }
            GMCommander commander = new GMCommander(aCommander);
            for (Method m : commandClazz.getMethods())
            {
                ACommand aCommand = m.getAnnotation(ACommand.class);
                if (aCommand == null)
                {
                    continue;
                }
                GMCommand gMCommand = new GMCommand(aCommand, m, commandClazz);
                commander.addCommand(gMCommand);
            }
            GmCommandMgr.getInstance().addCommander(commander);
        }
    }

    /******
     * 初始事件，事件的路径由getEventPackPath()返回
     */
    @SuppressWarnings({"unchecked"})
    private boolean _initEvents()
    {
        if (getEventPackPath() == null || getEventPackPath().isEmpty())
            return true;

        //检索对应包名下的所有类
        List<Class<? extends _ALogicEventBase>> clazzs = getPackClassByInterface(_ALogicEventBase.class, getEventPackPath());
        if (clazzs.isEmpty())
            return true;

        //逐个类进行说明文件判断，并比对父子类
        //注册所有event的子类对象
        for (Class<?> clazz : clazzs)
        {
            EventDesc desc = clazz.getAnnotation(EventDesc.class);
            if (desc == null)
                continue;

            if (!_ALogicEventBase.class.isAssignableFrom(clazz))
                continue;

            //注册子类对象
            if (!EventMetaMgr_Refdata.getInstance().registMetaByDesc(desc, (Class<? extends _ALogicEventBase>) clazz))
                return false;
        }

        return true;
    }


    /********
     * 初始化配表，配表的路径由getRefPackPath()返回
     * @param <T>
     * @return
     */
    @SuppressWarnings("rawtypes")
    public boolean _initRefs()
    {
        List<Class<? extends RefBase>> classes = getPackClassByInterface(RefBase.class, getRefPackPath());
        if (!NPRefDataMgr.getInstance().load(classes))
            return false;// 默认加载

        List<Class<? extends _IHotRefTableLoader>> hotRefClasses = getPackClassByInterface(_IHotRefTableLoader.class, getRefPackPath());
        hotRefClasses.forEach(clazz ->
        {
            try
            {
                _IHotRefTableLoader loader = clazz.newInstance();
                ActivityHotRefLoaderMgr.getInstance().registerLoader(loader);
            } catch (Throwable e)
            {
                CommLog.error("auto regist hot ref table loader:{} err", clazz.getSimpleName(), e);
            }
        });

        return true;
    }

    /******
     * 添加需要监听的全局事件
     */
    private void _initListeners()
    {


    }

    protected abstract String getRefPackPath(); //需要子类返回配表包路径

    protected abstract String getGMCommandPackPath();//需要子类返回GM命令包路径

    protected abstract String getEventPackPath();//需要子类返回事件类型包路径

    protected abstract String getBoPackPath();//需要子类返回数据库表的包路径

    protected abstract String getLogBoPackPath();//需要子类返回数据库日志表的包路径

    protected abstract String getOptLogBoPackPath();

    protected abstract String getMsgDealerPackPath();//需要子类返回消息处理协议包路径

    protected abstract void registActivityFactory();//注册类工厂

}
