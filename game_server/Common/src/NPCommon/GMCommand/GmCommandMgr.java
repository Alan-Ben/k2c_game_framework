package NPCommon.GMCommand;

import NPCommon.Context._IContext;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.CommClass;

import java.lang.reflect.Method;
import java.util.*;

/**
 * GM命令管理器
 * @author scott
 * @date 2018年11月16日
 */
public class GmCommandMgr
{
    public static GmCommandMgr _instance = new GmCommandMgr();

    public static GmCommandMgr getInstance()
    {
        return _instance;
    }

    protected GmCommandMgr()
    {

    }

    private Map<String, GMCommander> _m_commanderMap = new HashMap<>();//所有的命令列表

    /****
     * 指定包路径,初始化GM命令
     * @param _packPath
     */
    public void init(String _packPath)
    {
        Set<Class<?>> clazzs = CommClass.getClasses(_packPath);
        for (Class<?> clazz : clazzs)
        {
            registGMCommand(clazz);
        }
    }

    /****
     * 注册单个命令类
     * @param _clazz
     */
    public void registGMCommand(Class<?> _clazz)
    {
        ACommander aCommander = _clazz.getAnnotation(ACommander.class);
        if (aCommander == null)
        {
            return;
        }
        try
        {
            @SuppressWarnings("unused")
            CmdClassBase cmdClassObj = (CmdClassBase) _clazz.newInstance();
        } catch (Exception e)
        {
            CommLog.error("", e);
            return;
        }
        GMCommander commander = new GMCommander(aCommander);
        for (Method m : _clazz.getMethods())
        {
            ACommand aCommand = m.getAnnotation(ACommand.class);
            if (aCommand == null)
            {
                continue;
            }
            GMCommand command = new GMCommand(aCommand, m, _clazz);
            commander.addCommand(command);
        }
        _m_commanderMap.put(commander.getName().toLowerCase(), commander);
    }

    public void addCommander(GMCommander commander)
    {
        _m_commanderMap.put(commander.getName().toLowerCase(), commander);
    }

    /*******
     * GM命令执行入口
     * @param _executor
     * @param cmdline
     * @param _context
     * @param _handler
     */
    public void run(CmdExecutorBase _executor, String cmdline, _IContext _context, _IRunCallBack _handler)
    {
        runAsyncCore(_executor, cmdline, _context, new _IRunCallBack()
        {
            @Override
            public void onRunOver(boolean _bSucc, String _msg)
            {
                _handler.onRunOver(_bSucc, _msg);

                //TODO:记录日志以及其它监控数据
            }
        });
    }

    /*******
     * 异步执行GM命令
     * @param _executor
     * @param cmdline
     * @param _context
     * @param _handler
     */
    public void runAsyncCore(CmdExecutorBase _executor, String cmdline, _IContext _context, _IRunCallBack _handler)
    {
        if (cmdline == null)
        {
            _handler.onRunOver(false, "null command");
            return;
        }
        cmdline = cmdline.trim();
        if (cmdline.isEmpty())
        {
            _handler.onRunOver(false, "empty command");
            return;
        }
        if (_executor != null)
        {
            CommLog.info("executor:{} run GM Command:{}", _executor.toString(), cmdline);
        } else
        {
            CommLog.info("Sys run GM Command:{}", cmdline);
        }
        //处理help
        String[] args = cmdline.split("\\s+");
        if (args[0].toLowerCase().equalsIgnoreCase("help"))
        {

            if (args.length == 1)
            {
                _handler.onRunOver(true, getHelp());
                return;

            } else
            {
                String cmd = args[1].toLowerCase();
                GMCommander commander = _m_commanderMap.get(cmd);
                if (commander == null)
                {
                    _handler.onRunOver(true, "help commander[" + cmd + "] not found");
                    return;
                } else
                {
                    _handler.onRunOver(true, commander.getHelp());
                    return;
                }
            }
        }
        //处理helpAll指令
        if (args[0].toLowerCase().equalsIgnoreCase("helpAll"))
        {
            String helpList = getAllHelpList();
            _handler.onRunOver(true, helpList);
            return;
        }

        //实际的command
        GMCommander commander = _m_commanderMap.get(args[0].toLowerCase());
        if (commander == null)
        {//未找到命令入口

            String sError = String.format("commander[%s] not found", args[0]);
            CommLog.error("executing cmd:{} error:{}", cmdline, sError);
            _handler.onRunOver(false, sError);
            return;
        }
        String cmd = "";
        if (args.length >= 2)
        {
            cmd = args[1].toLowerCase();
        }
        try
        {
            if (args.length >= 2)
            {
                commander.run(_executor, _handler, _context, cmd, Arrays.copyOfRange(args, 2, args.length));
            } else
            {
                _handler.onRunOver(true, commander.getHelp());
            }
        } catch (Exception e)
        {
            CommLog.error("Exception while running command:{}", cmdline, e);
            _handler.onRunOver(false, String.format("Exception while running command:%s message:%s", cmd, e.toString()));
        }
    }

    /**
     * 执行指定GM命令
     * @param _executor
     * @param _commandType
     * @param _funcType
     * @param _args
     * @param _context
     * @param _handler
     */
    public void runSpecific(CmdExecutorBase _executor, String _commandType, String _funcType, String[] _args, _IContext _context, _IRunCallBack _handler)
    {
        runSpecificAsyncCore(_executor, _commandType, _funcType, _args, _context, new _IRunCallBack()
        {
            @Override
            public void onRunOver(boolean _bSucc, String _msg)
            {
                _handler.onRunOver(_bSucc, _msg);
                //TODO:记录日志以及其它监控数据
            }
        });
    }

    /**
     * 异步执行指定GM命令
     * @param _executor
     * @param _commandType
     * @param _funcType
     * @param _args
     * @param _context
     * @param _handler
     */
    public void runSpecificAsyncCore(CmdExecutorBase _executor, String _commandType, String _funcType, String[] _args, _IContext _context, _IRunCallBack _handler)
    {
        String cmdline = _commandType + " " + _funcType + " " + Arrays.toString(_args);

        if (_executor != null)
        {
            CommLog.info("executor:{} run GM cmdLine:{}", _executor.toString(), cmdline);
        } else
        {
            CommLog.info("Sys run GM cmdLine:{}", cmdline);
        }

        //格式化参数
        String commandType = _commandType.toLowerCase();
        String funcType = _funcType.toLowerCase();

        //处理helpAll指令
        if (commandType.equalsIgnoreCase("helpAll"))
        {
            String helpList = getAllHelpList();
            _handler.onRunOver(true, helpList);
            return;
        }

        //实际的command
        GMCommander commander = _m_commanderMap.get(commandType);
        if (commander == null)
        {//未找到命令入口

            String sError = String.format("commander[%s] not found", commandType);
            CommLog.error("executing cmd:{} error:{}", cmdline, sError);
            _handler.onRunOver(false, sError);
            return;
        }

        //处理没有传入func的情况, 返回功能列表
        if (funcType.isEmpty())
        {
            _handler.onRunOver(true, commander.getHelp());
            return;
        }

        try
        {
            commander.run(_executor, _handler, _context, funcType, _args);
        } catch (Exception e)
        {
            CommLog.error("Exception while running command:{}", cmdline, e);
            _handler.onRunOver(false, String.format("Exception while running command:%s message:%s", cmdline, e.toString()));
        }
    }

    /****
     * 返回所有命令列表
     * @return
     */
    private String getHelp()
    {
        StringBuilder sBuilder = new StringBuilder();
        for (GMCommander commander : _m_commanderMap.values())
        {
            sBuilder.append(commander.toString()).append("\n");
        }
        return sBuilder.toString();
    }

    /*******
     * 返回所有命令的帮助列表
     * @return
     */
    public String getAllHelpList()
    {
        StringBuilder sBuilder = new StringBuilder("");
        List<GMCommander> commanderList = new ArrayList<>(_m_commanderMap.values());
        Collections.sort(commanderList, (A, B) -> A.getName().compareToIgnoreCase(B.getName()));
        for (GMCommander commander : commanderList)
        {
            sBuilder.append("\n");
            sBuilder.append("======= ").append(commander.toString()).append(" =======").append("\n");
            sBuilder.append(commander.getHelp()).append("\n");
        }
        return sBuilder.toString();
    }

    public void initByClassName(String _className)
    {
        Class<?> c = null;
        try
        {
            c = Class.forName(_className, true, ClassLoader.getSystemClassLoader());
        } catch (Throwable e)
        {
        }
        if (null == c)
            return;
        init(c.getPackage().getName());

    }
}
