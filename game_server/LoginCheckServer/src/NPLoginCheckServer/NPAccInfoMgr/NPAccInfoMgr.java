package NPLoginCheckServer.NPAccInfoMgr;

import ALMySqlCommon.ALMySqlDBConditionObj;
import LCSDB.Bo.AccountBO;
import NPCommon.DB._ASelectCallback;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.NPLoginCheckServer;

import java.util.HashMap;
import java.util.List;
import java.util.concurrent.locks.ReentrantLock;

public class NPAccInfoMgr
{
    public static interface _ILoadAccountCallBack
    {
        public void onLoad(boolean isSucc, boolean _isExists, AccountBO _bo);
    }

    private static NPAccInfoMgr _g_instance = new NPAccInfoMgr();

    public static NPAccInfoMgr getInstance()
    {
        return _g_instance;
    }

    /**
     * 帐号名与对应帐号信息的映射表
     */
    private HashMap<String, AccountBO> _m_hmAccMap;

    /**
     * 锁对象
     */
    private ReentrantLock _m_mutex;

    protected NPAccInfoMgr()
    {
        _m_hmAccMap = new HashMap<String, AccountBO>();

        _m_mutex = new ReentrantLock();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /***************
     * 获取对应的帐号信息
     * @param _accName
     * @return
     */
    public AccountBO getCachedAccInfoByName(String _accName)
    {
        _lock();

        try
        {
            return _m_hmAccMap.get(_accName.toLowerCase());
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 添加一个数据对象
     * @param _bo
     */
    public boolean addAccInfo(AccountBO _bo)
    {
        _lock();

        try
        {
            if (_m_hmAccMap.containsKey(_bo.getAccName().toLowerCase()))
                return false;

            _m_hmAccMap.put(_bo.getAccName().toLowerCase(), _bo);
            return true;
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 根据用户登录名获取对应的账号信息
     *
     * @author alzq.z
     * @time 2019年3月27日 下午8:31:26
     */
    public void getAccInfoByName(String _accName, final _ILoadAccountCallBack _callBack)
    {
        AccountBO accInfo = getCachedAccInfoByName(_accName);
        if (null != accInfo)
        {
            _callBack.onLoad(true, true, accInfo);
        } else
        {
            NPLoginCheckServer.getInstance().getBM().getBM(AccountBO.class).findAll("accName", _accName, new _ASelectCallback<List<AccountBO>>()
            {
                @Override
                public void dealFail()
                {
                    _callBack.onLoad(false, false, null);
                }

                @Override
                public void dealSuc(List<AccountBO> accList)
                {
                    if (accList.isEmpty())
                    {
                        _callBack.onLoad(true, false, null);
                    } else
                    {
                        AccountBO bo = accList.get(0);
                        //添加到数据集
                        if (!addAccInfo(bo))
                        {
                            //失败的处理
                            _callBack.onLoad(true, false, null);
                        } else
                        {
                            _callBack.onLoad(true, true, bo);
                        }
                    }
                }
            });

        }
    }

    public boolean InitLoadFromDB()
    {
        List<AccountBO> bos = null;
        try
        {
            ALMySqlDBConditionObj conditon = new ALMySqlDBConditionObj();
            conditon.addAndLardgeThan("lastLoginTime", CommonFunc.getNowTimeSec() - 3600 * 24 * 3);//只读取三天内登录的玩家.
            bos = NPLoginCheckServer.getInstance().getBM().getBM(AccountBO.class).s_findAll(conditon);
            if (null == bos)
            {
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("WCGAccInfoMgr  InitLoadFromDB failed:", e);
            return false;
        }

        _lock();

        try
        {
            //将数据放入缓存
            for (AccountBO bo : bos)
            {
                _m_hmAccMap.put(bo.getAccName().toLowerCase(), bo);
            }
        } finally
        {
            _unlock();
        }

        CommLog.sys("Init Cached Account record num " + bos.size());
        return true;
    }

    /**
     * 通过命令移除登录账号信息
     * @param _acc
     * @return
     */
    public boolean cmdRemoveAccountInfo(String _acc)
    {
        _lock();
        try
        {
            AccountBO accountBO = _m_hmAccMap.remove(_acc);
            if (accountBO == null)
                return false;

            accountBO.del(NPLoginCheckServer.getInstance().getBM());
            return true;
        } finally
        {
            _unlock();
        }
    }
}
