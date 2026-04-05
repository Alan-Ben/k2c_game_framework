package NPUSServer;

import NPCommon.Log.CommLog;

public class USLog extends CommLog
{
    public static void debug(NPUserServer _server, String msg)
    {
        LOG.debug(_server.getServerName() + msg);
    }

    public static void debug(NPUserServer _server, String format, Object arg)
    {
        LOG.debug(_server.getServerName() + format, arg);
    }

    public static void debug(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.debug(_server.getServerName() + format, arg1, arg2);
    }

    public static void debug(NPUserServer _server, String format, Object... arguments)
    {
        LOG.debug(_server.getServerName() + format, arguments);
    }

    public static void debug(NPUserServer _server, String msg, Throwable t)
    {
        LOG.debug(_server.getServerName() + msg, t);
    }

    public static void info(NPUserServer _server, String msg)
    {
        LOG.info(_server.getServerName() + msg);
    }

    public static void info(NPUserServer _server, String format, Object arg)
    {
        LOG.info(_server.getServerName() + format, arg);
    }

    public static void info(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.info(_server.getServerName() + format, arg1, arg2);
    }

    public static void info(NPUserServer _server, String format, Object... arguments)
    {
        LOG.info(_server.getServerName() + format, arguments);
    }

    public static void info(NPUserServer _server, String msg, Throwable t)
    {
        LOG.info(_server.getServerName() + msg, t);
    }

    public static void warn(NPUserServer _server, String msg)
    {
        LOG.warn(_server.getServerName() + msg);
    }

    public static void warn(NPUserServer _server, String format, Object arg)
    {
        LOG.warn(_server.getServerName() + format, arg);
    }

    public static void warn(NPUserServer _server, String format, Object... arguments)
    {
        LOG.warn(_server.getServerName() + format, arguments);
    }

    public static void warn(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.warn(_server.getServerName() + format, arg1, arg2);
    }

    public static void warn(NPUserServer _server, String msg, Throwable t)
    {
        LOG.warn(_server.getServerName() + msg, t);
    }

    public static void error(NPUserServer _server, String msg)
    {
        LOG.error(_server.getServerName() + msg);
    }

    public static void error(NPUserServer _server, String format, Object arg)
    {
        LOG.error(_server.getServerName() + format, arg);
    }

    public static void error(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.error(_server.getServerName() + format, arg1, arg2);
    }

    public static void error(NPUserServer _server, String format, Object... arguments)
    {
        LOG.error(_server.getServerName() + format, arguments);
    }

    public static void error(NPUserServer _server, String msg, Throwable t)
    {
        LOG.error(_server.getServerName() + msg, t);
    }

    public static void fatal(NPUserServer _server, String msg)
    {
        LOG.fatal(_server.getServerName() + msg);
    }

    public static void fatal(NPUserServer _server, String format, Object arg)
    {
        LOG.fatal(_server.getServerName() + format, arg);
    }

    public static void fatal(NPUserServer _server, String format, Object... arguments)
    {
        LOG.fatal(_server.getServerName() + format, arguments);
    }

    public static void fatal(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.fatal(_server.getServerName() + format, arg1, arg2);
    }

    public static void fatal(NPUserServer _server, String msg, Throwable t)
    {
        LOG.fatal(_server.getServerName() + msg, t);
    }

    //sys
    public static void sys(NPUserServer _server, String msg)
    {
        LOG.sys(_server.getServerName() + msg);
    }

    public static void sys(NPUserServer _server, String format, Object arg)
    {
        LOG.sys(_server.getServerName() + format, arg);
    }

    public static void sys(NPUserServer _server, String format, Object... arguments)
    {
        LOG.sys(_server.getServerName() + format, arguments);
    }

    public static void sys(NPUserServer _server, String format, Object arg1, Object arg2)
    {
        LOG.sys(_server.getServerName() + format, arg1, arg2);
    }

    public static void sys(NPUserServer _server, String msg, Throwable t)
    {
        LOG.fatal(_server.getServerName() + msg, t);
    }
}
