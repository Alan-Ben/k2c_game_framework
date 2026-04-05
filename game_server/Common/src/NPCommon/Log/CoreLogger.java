package NPCommon.Log;

import ALServerLog.ALServerLog;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.util.ArrayList;

public class CoreLogger
{
    private static final int LOG_LEVEL_TRACE = 0;
    private static final int LOG_LEVEL_DEBUG = 10;
    private static final int LOG_LEVEL_INFO = 20;
    private static final int LOG_LEVEL_SYS = 30;
    private static final int LOG_LEVEL_WARN = 40;
    private static final int LOG_LEVEL_ERROR = 50;
    private static final int LOG_LEVEL_FATAL = 60;


    private ArrayList<_ILogAppender> _m_sAppenders = new ArrayList<>();

    public void regAppender(_ILogAppender _appender)
    {
        _m_sAppenders.add(_appender);
    }

    private void formatAndLog(int level, String format, Object arg1, Object arg2)
    {
        FormattingTuple tp = MessageFormatter.format(format, arg1, arg2);
        log(level, tp.getMessage(), tp.getThrowable());
    }

    /**
     * For formatted messages, first substitute arguments and then log.
     * @param level
     * @param format
     * @param arguments a list of 3 ore more arguments
     */
    private void formatAndLog(int level, String format, Object... arguments)
    {
        FormattingTuple tp = MessageFormatter.arrayFormat(format, arguments);
        log(level, tp.getMessage(), tp.getThrowable());
    }


    /**
     * A simple implementation which logs messages of level DEBUG according
     * to the format outlined above.
     */
    public void debug(String msg)
    {
        log(LOG_LEVEL_DEBUG, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * DEBUG according to the format outlined above.
     */
    public void debug(String format, Object param1)
    {
        formatAndLog(LOG_LEVEL_DEBUG, format, param1, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * DEBUG according to the format outlined above.
     */
    public void debug(String format, Object param1, Object param2)
    {
        formatAndLog(LOG_LEVEL_DEBUG, format, param1, param2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * DEBUG according to the format outlined above.
     */
    public void debug(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_DEBUG, format, argArray);
    }

    /**
     * Log a message of level DEBUG, including an exception.
     */
    public void debug(String msg, Throwable t)
    {
        log(LOG_LEVEL_DEBUG, msg, t);
    }

    /**
     * A simple implementation which logs messages of level INFO according
     * to the format outlined above.
     */
    public void info(String msg)
    {
        log(LOG_LEVEL_INFO, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * INFO according to the format outlined above.
     */
    public void info(String format, Object arg)
    {
        formatAndLog(LOG_LEVEL_INFO, format, arg, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * INFO according to the format outlined above.
     */
    public void info(String format, Object arg1, Object arg2)
    {
        formatAndLog(LOG_LEVEL_INFO, format, arg1, arg2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * INFO according to the format outlined above.
     */
    public void info(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_INFO, format, argArray);
    }

    /**
     * Log a message of level INFO, including an exception.
     */
    public void info(String msg, Throwable t)
    {
        log(LOG_LEVEL_INFO, msg, t);
    }

    //warn

    /**
     * A simple implementation which always logs messages of level WARN according
     * to the format outlined above.
     */
    public void warn(String msg)
    {
        log(LOG_LEVEL_WARN, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * WARN according to the format outlined above.
     */
    public void warn(String format, Object arg)
    {
        formatAndLog(LOG_LEVEL_WARN, format, arg, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * WARN according to the format outlined above.
     */
    public void warn(String format, Object arg1, Object arg2)
    {
        formatAndLog(LOG_LEVEL_WARN, format, arg1, arg2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * WARN according to the format outlined above.
     */
    public void warn(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_WARN, format, argArray);
    }

    /**
     * Log a message of level WARN, including an exception.
     */
    public void warn(String msg, Throwable t)
    {
        log(LOG_LEVEL_WARN, msg, t);
    }

    //error

    /**
     * A simple implementation which always logs messages of level ERROR according
     * to the format outlined above.
     */
    public void error(String msg)
    {
        log(LOG_LEVEL_ERROR, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void error(String format, Object arg)
    {
        formatAndLog(LOG_LEVEL_ERROR, format, arg, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void error(String format, Object arg1, Object arg2)
    {
        formatAndLog(LOG_LEVEL_ERROR, format, arg1, arg2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void error(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_ERROR, format, argArray);
    }

    /**
     * Log a message of level ERROR, including an exception.
     */
    public void error(String msg, Throwable t)
    {
        log(LOG_LEVEL_ERROR, msg, t);
    }


    ////fatal

    /**
     * A simple implementation which always logs messages of level ERROR according
     * to the format outlined above.
     */
    public void fatal(String msg)
    {
        log(LOG_LEVEL_FATAL, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void fatal(String format, Object arg)
    {
        formatAndLog(LOG_LEVEL_FATAL, format, arg, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void fatal(String format, Object arg1, Object arg2)
    {
        formatAndLog(LOG_LEVEL_FATAL, format, arg1, arg2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void fatal(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_FATAL, format, argArray);
    }

    /**
     * Log a message of level ERROR, including an exception.
     */
    public void fatal(String msg, Throwable t)
    {
        log(LOG_LEVEL_FATAL, msg, t);
    }

    ////////sys

    /**
     * A simple implementation which always logs messages of level ERROR according
     * to the format outlined above.
     */
    public void sys(String msg)
    {
        log(LOG_LEVEL_SYS, msg, null);
    }

    /**
     * Perform single parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void sys(String format, Object arg)
    {
        formatAndLog(LOG_LEVEL_SYS, format, arg, null);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void sys(String format, Object arg1, Object arg2)
    {
        formatAndLog(LOG_LEVEL_SYS, format, arg1, arg2);
    }

    /**
     * Perform double parameter substitution before logging the message of level
     * ERROR according to the format outlined above.
     */
    public void sys(String format, Object... argArray)
    {
        formatAndLog(LOG_LEVEL_SYS, format, argArray);
    }

    /**
     * Log a message of level ERROR, including an exception.
     */
    public void sys(String msg, Throwable t)
    {
        log(LOG_LEVEL_SYS, msg, t);
    }

    private void log(int level, String message, Throwable t)
    {

        StringBuilder buf = new StringBuilder(32);
        buf.append(message);
        if (null != t)
        {
            buf.append(" Exception:");
            buf.append(t.getMessage());
            buf.append('\n');

            StringWriter sw = new StringWriter();
            PrintWriter pw = new PrintWriter(sw);
            t.printStackTrace(pw);
            buf.append(sw.toString());
        }
        String sLog = buf.toString();

        switch (level)
        {
            case LOG_LEVEL_TRACE:
                break;
            case LOG_LEVEL_DEBUG:
                ALServerLog.Debug(sLog);
                break;
            case LOG_LEVEL_INFO:
                ALServerLog.Info(sLog);
                break;
            case LOG_LEVEL_SYS:
                ALServerLog.Sys(sLog);
                break;
            case LOG_LEVEL_WARN:
                ALServerLog.Warning(sLog);
                break;
            case LOG_LEVEL_ERROR:
                ALServerLog.Error(sLog);
                break;
            case LOG_LEVEL_FATAL:
                ALServerLog.Fatal(sLog);
                break;
        }
        for (_ILogAppender appender : _m_sAppenders)
        {
            appender.append(sLog);
        }

    }
}