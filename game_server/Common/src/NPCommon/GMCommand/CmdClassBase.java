package NPCommon.GMCommand;

import NPCommon.Context._IContext;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.HotLoad._IInitOnClassLoadedable;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.CommonFunc;

import java.lang.reflect.Method;


/********
 * GM命令类的基类
 */
public abstract class CmdClassBase implements _IInitOnClassLoadedable
{
    private CmdExecutorBase _m_executor; //执行者
    private _IContext _m_Context; //执行的上下文
    private _IRunCallBack _m_runCallBack; //异步执行回调
    private long _m_lStartMs; //命令开始执行时间

    public void setExecutor(CmdExecutorBase _executor)
    {
        _m_executor = _executor;
    }

    public CmdExecutorBase getExecutor()
    {
        return _m_executor;
    }

    public void setContext(_IContext _context)
    {
        _m_Context = _context;
    }

    public void setCallBack(_IRunCallBack _callBack)
    {
        _m_runCallBack = _callBack;
    }

    protected _IContext getIContext()
    {
        return _m_Context;
    }

    public CmdClassBase()
    {
        _m_lStartMs = CommonFunc.getNowTimeMS();
    }

    public synchronized _IRunCallBack takeCallBack()
    {
        if (null != _m_runCallBack)
        {
            _IRunCallBack callBack = _m_runCallBack;
            _m_runCallBack = null;
            return callBack;
        } else
        {
            CmdClassBase theCmd = this;
            return new _IRunCallBack()
            {
                @Override
                public void onRunOver(boolean _bSucc, String _msg)
                {
                    long nowMs = CommonFunc.getNowTimeMS();
                    CommLog.error("expired callback cost:" + (nowMs - _m_lStartMs) + " ms for cmd:" + theCmd.getClass().getSimpleName() + " result:" + _bSucc + " msg:" + _msg);
                }
            };
        }
    }

    @Override
    public Result initOnClassLoaded()
    {
        Class<?> clazz = this.getClass();
        ACommander aCommander = clazz.getAnnotation(ACommander.class);
        if (aCommander == null)
        {
            return Result.failed("[ERROR]no NPCommon.GMCommand.Annotation.ACommander defined");
        }
        GMCommander commander = new GMCommander(aCommander);
        for (Method m : clazz.getMethods())
        {
            ACommand aCommand = m.getAnnotation(ACommand.class);
            if (aCommand == null)
            {
                continue;
            }
            GMCommand gMCommand = new GMCommand(aCommand, m, clazz);
            commander.addCommand(gMCommand);
        }
        GmCommandMgr.getInstance().addCommander(commander);
        return Result.succ(String.format("[OK] new command:%s register", commander.getName()));
    }

}
