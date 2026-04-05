package NPCommon.Log;

public class CommLog
{
    protected static final CoreLogger LOG = new CoreLogger();

    public static void regAppender(_ILogAppender _appender)
    {
        LOG.regAppender(_appender);
    }

    public static void debug(String msg)
    {
        LOG.debug(msg);
    }

    public static void debug(String format, Object arg)
    {
        LOG.debug(format, arg);
    }

    public static void debug(String format, Object arg1, Object arg2)
    {
        LOG.debug(format, arg1, arg2);
    }

    public static void debug(String format, Object... arguments)
    {
        LOG.debug(format, arguments);
    }

    public static void debug(String msg, Throwable t)
    {
        LOG.debug(msg, t);
    }

    public static void info(String msg)
    {
        LOG.info(msg);
    }

    public static void info(String format, Object arg)
    {
        LOG.info(format, arg);
    }

    public static void info(String format, Object arg1, Object arg2)
    {
        LOG.info(format, arg1, arg2);
    }

    public static void info(String format, Object... arguments)
    {
        LOG.info(format, arguments);
    }

    public static void info(String msg, Throwable t)
    {
        LOG.info(msg, t);
    }

    public static void warn(String msg)
    {
        LOG.warn(msg);
    }

    public static void warn(String format, Object arg)
    {
        LOG.warn(format, arg);
    }

    public static void warn(String format, Object... arguments)
    {
        LOG.warn(format, arguments);
    }

    public static void warn(String format, Object arg1, Object arg2)
    {
        LOG.warn(format, arg1, arg2);
    }

    public static void warn(String msg, Throwable t)
    {
        LOG.warn(msg, t);
    }


    public static void error(String msg)
    {
        LOG.error(msg);
    }

    public static void error(String format, Object arg)
    {
        LOG.error(format, arg);
    }

    public static void error(String format, Object arg1, Object arg2)
    {
        LOG.error(format, arg1, arg2);
    }

    public static void error(String format, Object... arguments)
    {
        LOG.error(format, arguments);
    }

    public static void error(String msg, Throwable t)
    {
        LOG.error(msg, t);
    }

    public static void fatal(String msg)
    {
        LOG.fatal(msg);
    }

    public static void fatal(String format, Object arg)
    {
        LOG.fatal(format, arg);
    }

    public static void fatal(String format, Object... arguments)
    {
        LOG.fatal(format, arguments);
    }

    public static void fatal(String format, Object arg1, Object arg2)
    {
        LOG.fatal(format, arg1, arg2);
    }

    public static void fatal(String msg, Throwable t)
    {
        LOG.fatal(msg, t);
    }

    //sys
    public static void sys(String msg)
    {
        LOG.sys(msg);
    }

    public static void sys(String format, Object arg)
    {
        LOG.sys(format, arg);
    }

    public static void sys(String format, Object... arguments)
    {
        LOG.sys(format, arguments);
    }

    public static void sys(String format, Object arg1, Object arg2)
    {
        LOG.sys(format, arg1, arg2);
    }

    public static void sys(String msg, Throwable t)
    {
        LOG.fatal(msg, t);
    }

}