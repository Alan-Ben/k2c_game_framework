package NPUSServer.QueueMgr;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.GeneralV.EGeneralVType;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_002_GSOp;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UserIndexMgr._AUserIndexCallback;
import NPUSServer.UserServerConf;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.LinkedList;

/*********************
 * US服务器进入的队列管理对象
 * 用于管理可以进入服务器的用户进入处理操作
 * @author mj
 *
 */
public class QueueMgr
{
    private NPUserServer _m_server;

    //序列号创建值
    private long _m_lSerializeMaker;

    //检测序列号
    private long _m_lCheckSerialize;
    //最后一次检测的时间戳
    private long _m_lLastCheckTimeTagMS;

    //排队中的队列，队列节点不会删除，只会无效
    private LinkedList<QueueInfo> _m_lQueueList;

    //操作锁对象
    private MutexAtom _m_mutex;

    public QueueMgr(NPUserServer _server)
    {
        _m_server = _server;

        _m_lSerializeMaker = 1;
        _m_lCheckSerialize = ALSerializeMaker.makeNewSerialize();
        _m_lLastCheckTimeTagMS = 0;

        _m_lQueueList = new LinkedList<QueueInfo>();

        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }
    protected void _unlock()
    {
        _m_mutex.unlock();
    }


    public NPUserServer getUSServer(){return _m_server;}
    public long getCheckSerialize()
    {
        return _m_lCheckSerialize;
    }

    /************
     * 获取第一个排队的队列
     * @return
     */
    public long getFirstIndex()
    {
        _lock();

        try
        {
            if (null == _m_lQueueList || _m_lQueueList.isEmpty())
                return -1;

            return _m_lQueueList.getFirst().getQueueIndex();
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 尝试加载用户数据
     * @param _uid
     * @param _gsId
     * @param _customData
     */
    public void tryLoadUserData(String _uid, long _gsSessionId, int _gsId, long _gsInfoSerialize, String _customData, _IWCGBasicRequestCommiter _committer)
    {
        //尝试获取用户数据，如果已经存在用户数据则刷新用户下线时间标记
        long preCid = getUSServer().getUserIdxMgr().lookupUserCid(_uid);
        if (0 != preCid)
        {
            //已经存在cid，查询是否存在UserData，使用尝试设置离线处理，可以保证数据不会被销毁
            NPUSUserData preUserData = getUSServer().getUsUserMgr().changeUserOffline(preCid, 0);

            if (null != preUserData)
            {
                //如果存在数据则直接返回成功
                _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_005_RetEnterUSInfo(0));

                //由于已经存在data对象，所以数据肯定在加载或者已经加载完毕
                //因此这里只需要直接走回正常加载处理的commiter处理即可
                preUserData.setDataLoadRequestCommiter(new QueueInfoLoadCommiter(getUSServer(), _gsSessionId, _gsId, _gsInfoSerialize, preCid));

                return;
            }
        }
        //如果没有cid则需要进行注册处理
        else
        {
            //注册人数控制
            if (getUSServer().getGeneralVMgr().getVObj(EGeneralVType.CID).getV() > UserServerConf.getInstance().getMaxUserCount()) {
                USLog.info(_m_server, "Us register user touch floor num:{},max:{}"
                        , getUSServer().getGeneralVMgr().getVObj(EGeneralVType.CID).getV()
                        , UserServerConf.getInstance().getMaxUserCount());
            }
        }

        //获取当前内存用户数据量，如果在合法范围则直接加载
        if (getUSServer().getUsUserMgr().getAllCacheUserDataCount() > UserServerConf.getInstance().getMaxLoadUserData())
        {
            //放入队列
            QueueInfo info = addUserQueue(_uid, _gsSessionId, _gsId, _gsInfoSerialize, _customData);

            //返回信息
            _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_005_RetEnterUSInfo(info.getQueueIndex()));
            return;
        }

        //返回信息
        _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_005_RetEnterUSInfo(0));

        //直接开始加载处理
        getUSServer().getUserIdxMgr().getOrCreateUserCid(_uid, new _AUserIndexCallback()
        {
            @Override
            public void getCidForUid(String _uid, long _cid)
            {
                if (_cid == 0)
                {
                    //不合法cid
                    USLog.error(_m_server, "get cid for uid:{} err!!!", _uid);
                    return;
                }
                //加载用户数据
                getUSServer().getUsUserMgr().initUserData(_cid, _uid, _customData
                        , new QueueInfoLoadCommiter(getUSServer(), _gsSessionId, _gsId, _gsInfoSerialize, _cid));

            }
        });
    }

    /*****************
     * 添加一个用户的队列节点，队列节点不会删除，只会无效
     * @param _uid
     * @param _gsId
     * @param _customData
     * @return
     */
    public QueueInfo addUserQueue(String _uid, long _gsSessionId, int _gsId, long _gsInfoSerialize, String _customData)
    {
        //判断是否需要开启检测任务
        boolean needStartCheck = false;

        _lock();

        try
        {
            //排队强退时，玩家在还没有过号的时间段上线，就继续排队
            QueueInfo info = lookup(_uid);
            if (info == null)
            {
                info = new QueueInfo(_makeNewIndex(), _uid, _gsSessionId, _gsId, _gsInfoSerialize, _customData);
            } else
            {
                //更新 info 数据
                info.updateAndEnable(_gsSessionId, _gsId, _gsInfoSerialize, _customData);
            }


            //判断队列是否为空，如果为空需要创建
            if (_m_lQueueList.isEmpty())
            {
                needStartCheck = true;
            } else
            {
                long nowTimeMS = ALBasicCommonFun.getNowTimeMS();
                if (nowTimeMS - _m_lLastCheckTimeTagMS > 5000)
                {
                    //超出5秒没检测则需要重新开启
                    needStartCheck = true;
                }
            }

            //添加到队列
            _m_lQueueList.add(info);

            return info;
        } finally
        {
            _unlock();

            //如果需要开启则开启检测任务
            if (needStartCheck)
                _startCheckQueue();
        }
    }

    /**
     * 查找指定uid的登录队列信息
     * @param _uid 用户标识
     * @return QueueInfo
     */
    private QueueInfo lookup(String _uid)
    {
        _lock();
        try
        {
            for (QueueInfo queueInfo : _m_lQueueList)
            {
                if (queueInfo == null)
                {
                    continue;
                }
                if (queueInfo.getUid().equals(_uid))
                {
                    return queueInfo;
                }
            }
        } finally
        {
            _unlock();
        }
        return null;
    }


    /*****************
     * 退出当前队列
     * @return
     */
    public Result quitQueue(long _queueIndex, long _infoSerialize)
    {
        if (_queueIndex < 0)
            return CommErr.DATA_STATE_ERR;

        _lock();

        try
        {
            //根据当前第一个索引，使用下标直接获取
            long firstIndex = getFirstIndex();
            if (-1 == firstIndex)
                return CommErr.DATA_STATE_ERR;

            //判断数据是否合法
            if (_queueIndex < firstIndex || _m_lQueueList.size() <= _queueIndex - firstIndex)
            {
                //数据数量不够
                return CommErr.SYS_ERR;
            }

            //根据下标取数据
            QueueInfo queueInfo = _m_lQueueList.get((int) (_queueIndex - firstIndex));
            //如果数据无效
            if (null == queueInfo)
                return CommErr.SYS_ERR;

            //判断ticket是否一致
            if (queueInfo.getGSInfoSerialize() != _infoSerialize)
            {
                USLog.error(getUSServer(), "Queue Info serialize: " + queueInfo.getGSInfoSerialize() + " not equals  [" + _infoSerialize + "]");
                return CommErr.DATA_STATE_ERR;
            }

            //取消对象状态
            queueInfo.setDisable();

            //返回成功
            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 创建新的索引
     * @return
     */
    protected long _makeNewIndex()
    {
        _lock();

        try
        {
            return _m_lSerializeMaker++;
        } finally
        {
            _unlock();
        }
    }

    /*********************
     * 开始检测队列任务
     */
    protected void _startCheckQueue()
    {
        //设置新的操作序列号
        long newCheckSerialize = ALSerializeMaker.makeNewSerialize();
        //设置变量
        _m_lCheckSerialize = newCheckSerialize;

        //开启任务处理，每秒处理
        ALSynTaskManager.getInstance().regTask(new SynCheckQueueTask(getUSServer(), newCheckSerialize), 1000);
    }

    /******************
     * 检测对应操作序列号的处理
     * 返回是否继续检测
     * @param _checkSerialize
     */
    protected boolean _checkQueue(long _checkSerialize)
    {
        if (_checkSerialize != _m_lCheckSerialize)
            return false;

        //服务器未开启不做处理，同时返回需要继续
        if(!getUSServer().isServerReady())
            return true;

        //获取当前内存用户数据量，如果在合法范围则直接加载
        while (getUSServer().getUsUserMgr().getAllCacheUserDataCount() < UserServerConf.getInstance().getMaxLoadUserData())
        {
            QueueInfo dealInfo = null;

            _lock();

            try
            {
                do
                {
                    //判断队列是否有效
                    if (_m_lQueueList.isEmpty())
                        return false;

                    dealInfo = _m_lQueueList.pollFirst();

                } while (null != dealInfo && !dealInfo.isEnable());//数据无效则需要取下一个
            } finally
            {
                _unlock();
            }

            //判断数据是否有效，有效则进行加载处理
            if (null != dealInfo && dealInfo.isEnable())
            {
                final QueueInfo curInfo = dealInfo;
                //开始检测，并处理
                getUSServer().getUserIdxMgr().getOrCreateUserCid(dealInfo.getUid(), new _AUserIndexCallback()
                {
                    @Override
                    public void getCidForUid(String _uid, long _cid)
                    {
                        if (_cid == 0)
                        {
                            //不合法cid，数据库没有数据会产生这个值
                            USLog.error(_m_server, "get cid for uid:{} err!!!", _uid);
                            return;
                        }
                        //加载用户数据
                        getUSServer().getUsUserMgr().initUserData(_cid, _uid, curInfo.getCustomData(),
                                new QueueInfoLoadCommiter(getUSServer(), curInfo.getGSSessionId(), curInfo.getGSId(), curInfo.getGSInfoSerialize(), _cid));
                    }
                });
            }

            return false;
        }

        //到这里需要返回需要继续检测
        return true;
    }
}
