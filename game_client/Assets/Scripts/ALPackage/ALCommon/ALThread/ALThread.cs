using System.Threading;
using System;
using ALPackage;

/// <summary>
/// 统一的线程管理对象，如果退出可以统一退出
/// </summary>
public class ALThread
{
    //处理函数
    private Action _m_dThreadAction;
    //线程对象
    private Thread _m_tThread;

    public ALThread(Action _threadAction)
    {
        _m_dThreadAction = _threadAction;
        _m_tThread = null;
    }

    /// <summary>
    /// 开启线程，需要在前后分别注册和注销本对象
    /// 便于统一关闭所有线程
    /// </summary>
    public void startThread()
    {
        if(null == _m_dThreadAction)
            return ;

        _m_tThread = new Thread(_runThread);
        //开启线程
        _m_tThread.Start();
    }

    /// <summary>
    /// 释放线程相关资源
    /// </summary>
    public void discard()
    {
        if(null == _m_tThread)
            return;

        _m_tThread.Abort();
        _m_tThread = null;
    }

    /// <summary>
    /// 开启线程的线程体
    /// </summary>
    protected void _runThread()
    {
        //注册到管理器
        ALThreadMgr.instance.regThread(this);

        //执行线程体
        try
        {
            if(null != _m_dThreadAction)
                _m_dThreadAction();
        }
        catch(Exception _ex)
        {
            UnityEngine.Debug.LogError($"[ALThread._runThread] ERROR : 线程执行异常:{ALCommon.ToReadableString(_m_dThreadAction)}\n{_ex}");
        }

        //从管理器注销
        ALThreadMgr.instance.unregThread(this);
    }

    public override string ToString()
    {
        return ALCommon.ToReadableString(_m_dThreadAction);
    }
}