using System;
using System.Collections.Generic;

/// <summary>
/// 线程管理对象，统一注册和注销。在退出程序时统一退出所有线程
/// </summary>
public class ALThreadMgr
{
    private static ALThreadMgr _g_instance = new ALThreadMgr();
    public static ALThreadMgr instance
    {
        get
        {
            if(null == _g_instance)
                _g_instance = new ALThreadMgr();

            return _g_instance;
        }
    }

    //线程存储队列
    private List<ALThread> _m_lThreadList;

    public ALThreadMgr()
    {
        _m_lThreadList = new List<ALThread>();
    }

    /// <summary>
    /// 注册和注销线程
    /// </summary>
    /// <param name="_thread"></param>
    public void regThread(ALThread _thread)
    {
        if(null == _thread)
            return;

        lock(_m_lThreadList)
        {
            _m_lThreadList.Add(_thread);
        }
    }
    public void unregThread(ALThread _thread)
    {
        if(null == _thread)
            return;

        lock(_m_lThreadList)
        {
            _m_lThreadList.Remove(_thread);
        }
    }

    /// <summary>
    /// 关闭所有线程
    /// </summary>
    public void clearAllThread()
    {
        lock(_m_lThreadList)
        {
            for(int i = 0; i < _m_lThreadList.Count; i++)
            {
                _m_lThreadList[0].discard();
            }
        }
    }
}