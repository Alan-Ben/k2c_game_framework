package NPLoginCheckServer.NPSDKLoginMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.LoginCheckServerConf;
import NPLoginCheckServer.NPSDKLoginMgr.SynTask.NPSynSDKEnterAndCheckTask;
import NPLoginCheckServer.NPSDKLoginMgr.SynTask.NPSynSDKEnterTask;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;
import java.util.List;

/**
 * 该管理器为SDK链接登录的管理器
 * <p>
 * 该管理器会设置工作的SDK链接，和备选的SDK链接池
 * 该工作SDK链接在一段时间内会生效，通过_m_lCheckingGapMs进行控制
 * 在生效时间内，会通过NPSynSDKEnterTask进行登录验证；超过生效时间，会通过NPSynSDKEnterAndCheckTask进行登录验证，并选择速度最快的链接作为下一个生效时间的工作链接
 */
public class NPSDKLoginMgr
{
    private static NPSDKLoginMgr _g_instance = new NPSDKLoginMgr();

    public static NPSDKLoginMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = new NPSDKLoginMgr();

        return _g_instance;
    }

    //下次检测链接延迟的时间戳，超过此时间戳会直接调用多链接检查
    private long _m_lNextCheckingMs;
    //检查间隔时间
    private long _m_lCheckingGapMs = 10 * 60 * 1000;
    //锁 用于保证检查标志位 线程安全
    private MutexAtom _m_mutex;

    //正在工作的链接
    private String _m_sWorkingUrl;
    //可以备选的链接列表
    private List<String> _m_canChooseUrlList;

    //登录失败通知器
    private LoginFailInfoLogger _m_loginFailLogger;

    public NPSDKLoginMgr()
    {
        _m_mutex = new MutexAtom();
        _m_canChooseUrlList = new ArrayList<>();

        //添加SDK链接到备选链接列表
        _m_canChooseUrlList.add(LoginCheckServerConf.getInstance().getSDKUrl());
        _m_canChooseUrlList.addAll(LoginCheckServerConf.getInstance().getStandBySDKUrlList());

        //设置默认Url为工作链接
        _m_sWorkingUrl = LoginCheckServerConf.getInstance().getSDKUrl();

        //设置下一次需要检测多链接的时间戳
        _m_lNextCheckingMs = 0;

        _m_loginFailLogger = new LoginFailInfoLogger();

        CommLog.info("NPSDKLoginMgr create _m_sWorkingUrl:{} _m_canChooseUrlList:{}", _m_sWorkingUrl, CommonFunc.list2String(_m_canChooseUrlList, ';'));
    }

    /**
     * 返回SDK备选链接列表，供NPSynSDKEnterAndCheckTask检查任务使用
     * @return
     */
    public List<String> getCanChooseUrlList()
    {
        return _m_canChooseUrlList;
    }

    public LoginFailInfoLogger getLoginFailLogger()
    {
        return _m_loginFailLogger;
    }

    /**
     * 检测是否需要进行多链接测试
     * 根据时间戳进行判断，超过时间戳表示本次需要进行多链接并行的处理。
     * 返回false 表示使用当前确认链接即可
     * @return
     */
    public boolean tryDoChecking()
    {
        //设置标志位
        _m_mutex.lock();
        try
        {
            //重新检查一遍是否可以进行check，只需要根据时间戳进行判断即可
            long nowTimeMS = CommonFunc.getNowTimeMS();
            if (nowTimeMS < _m_lNextCheckingMs)
                return false;

            _m_lNextCheckingMs = nowTimeMS + _m_lCheckingGapMs;
        } finally
        {
            _m_mutex.unlock();
        }

        return true;
    }

    /**
     * 设置工作链接检查成功，并设置新的工作链接，重置标志位和设置下一次需要检查的时间
     * @param _fasterUrl 最快的链接
     */
    public void setFastURLSuc(String _fasterUrl)
    {
        _m_mutex.lock();
        try
        {
            _m_lNextCheckingMs = CommonFunc.getNowTimeMS() + _m_lCheckingGapMs;
            //设置工作链接
            _m_sWorkingUrl = _fasterUrl;

            CommLog.info("NPSDKLoginMgr check sdk url success, fastestUrl:{} nextCheckTimeMs:{}", _fasterUrl, _m_lNextCheckingMs);
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 处理SDK登录请求
     * @param _accName
     * @param _accPass
     * @param _clientIp
     * @param _commiter
     */
    public void dealSDKEnterRequest(String _accName, String _accPass, String _clientIp, _IWCGBasicRequestCommiter _commiter)
    {
        _ILoginOverDealer _overDealer = new _ILoginOverDealer()
        {
            @Override
            public void onLoginSuc()
            {

            }

            @Override
            public void onLoginFail(int _errType)
            {
                _m_loginFailLogger.recordFail();
            }
        };

        //检查当前工作链接是否有效，如果超过有效时间，则设置检查标志位
        if (tryDoChecking())
        {
            //创建链接登录检查任务
            ALSynTaskManager.getInstance().regTask(new NPSynSDKEnterAndCheckTask(_accName, _accPass, _clientIp, _commiter, _overDealer));
            return;
        }

        //创建登录任务
        ALSynTaskManager.getInstance().regTask(new NPSynSDKEnterTask(_accName, _accPass, _clientIp, _commiter, _m_sWorkingUrl, _overDealer));
    }

    /**
     * 设置下次进行多链接检查的时间
     * @param _timeMs
     */
    public void chgNextCheckTimeMs(long _timeMs)
    {
        _m_mutex.lock();
        try
        {
            _m_lNextCheckingMs = _timeMs;

            CommLog.info("NPSDKLoginMgr chgNextCheckTimeMs success _m_lNextCheckingMs:{}", _timeMs);
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 设置进行多链接检查的间隔时间
     * @param _gapTimeMs
     */
    public void chgCheckingGapTimeMs(long _gapTimeMs)
    {
        _m_mutex.lock();
        try
        {
            _m_lCheckingGapMs = _gapTimeMs;

            CommLog.info("NPSDKLoginMgr chgCheckingGapTimeMs success _m_lCheckingGapMs:{}", _gapTimeMs);
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 移除备用SDK链接
     * @param _url
     */
    public void removeStandByUrl(String _url)
    {
        _m_canChooseUrlList.remove(_url);
        CommLog.info(toString());
    }

    /**
     * 增加备用SDK链接
     * @param _url
     */
    public void addStandByUrl(String _url)
    {
        _m_canChooseUrlList.add(_url);
        CommLog.info(toString());
    }

    @Override
    public String toString()
    {
        return "NPSDKLoginMgr{" +
                "_m_lNextCheckingMs=" + _m_lNextCheckingMs +
                ", _m_lCheckingGapMs=" + _m_lCheckingGapMs +
                ", _m_sWorkingUrl='" + _m_sWorkingUrl + '\'' +
                ", _m_canChooseUrlList=" + CommonFunc.list2String(_m_canChooseUrlList, ';') +
                '}';
    }
}
