package NPUSServer.UserIndexMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPUSServer.GeneralV.UsID;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsCidBO;

import java.util.Hashtable;
import java.util.List;

/**************************
 * 玩家UId对应Cid的索引管理器
 * @author mj
 *
 */
public class UserIndexMgr
{
    private NPUserServer _m_server;

    //uid映射到cid的索引表
    private Hashtable<String, Long> _m_htUserIdToCidTable;

    //cid映射到uid的索引表
    private Hashtable<Long, String> _m_htCidToUserIdTable;

    private MutexAtom _m_mutex;

    public UserIndexMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_htUserIdToCidTable = new Hashtable<String, Long>();
        _m_htCidToUserIdTable = new Hashtable<Long, String>();

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

    /****************
     * 从内存索引查询cid对应的uid
     *
     * @param _cid 玩家cid
     * @return 对应的uid，如果不存在返回null
     *
     * 线程安全：通过_m_mutex保护
     */
    public String lookupCidLinkedUid(long _cid)
    {
        _lock();
        try
        {
            return _m_htCidToUserIdTable.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 查询已经存在的用户cid
     * @param _uid
     */
    public long lookupUserCid(String _uid)
    {
        //尝试从当前数据集获取，有数据则直接处理
        _lock();

        try
        {
            Long uid = _m_htUserIdToCidTable.get(_uid);
            if (uid == null)
            {
                return 0;
            }
            return uid;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询uid对应的cid
     *
     * 执行流程：
     * 1. 先从内存索引查询
     * 2. 如果内存中不存在，从数据库查询
     * 3. 通过回调返回查询结果
     *
     * @param _uid      uid
     * @param _callback 回调
     *
     * 线程安全：通过_m_mutex保护
     */
    public void lookupUidLinkedCid(String _uid, _ICallBackResultT<Long> _callback)
    {
        //尝试从当前数据集获取，有数据则直接处理
        _lock();
        try
        {
            long cid = lookupUserCid(_uid);
            if (cid == 0)
            {
                lookupUidLinkedCidFromDb(_uid, _callback);
            } else
            {
                _callback.onRunOver(Result.SUCC, cid);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询cid对应的uid（带回调的异步查询版本）
     *
     * 执行流程：
     * 1. 先从内存索引查询
     * 2. 如果内存中不存在，从数据库查询
     * 3. 通过回调返回查询结果
     *
     * @param _cid      玩家cid
     * @param _callback 回调，返回对应的uid
     *
     * 线程安全：通过_m_mutex保护
     */
    public void lookupCidLinkedUid(long _cid, _ICallBackResultT<String> _callback)
    {
        //尝试从内存索引获取
        _lock();
        try
        {
            String uid = lookupCidLinkedUid(_cid);
            if (uid == null)
            {
                //内存中不存在，从数据库查询
                lookupCidLinkedUidFromDb(_cid, _callback);
            } else
            {
                //内存中存在，直接返回
                _callback.onRunOver(Result.SUCC, uid);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 从数据库查询uid对应的cid
     * @param _uid      uid
     * @param _callback 回调
     */
    public void lookupUidLinkedCidFromDb(String _uid, _ICallBackResultT<Long> _callback)
    {
        getUSServer().getBM().getBM(UsCidBO.class).findAll("uid", _uid, new _ASelectCallback<List<UsCidBO>>()
        {
            @Override
            public void dealSuc(List<UsCidBO> _obj)
            {
                if (_obj == null || _obj.isEmpty())
                {
                    _callback.onRunOver(CommErr.PLAYER_NOT_FOUND, 0L);
                } else
                {
                    _callback.onRunOver(Result.SUCC, _obj.get(0).getCid());
                }
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(CommErr.PLAYER_NOT_FOUND, 0L);
            }
        });
    }

    /**
     * 查询玩家uid对应的cid（玩家uid）
     * @param _cid      cid
     * @param _callback 回调
     */
    public void lookupCidLinkedUidFromDb(long _cid, _ICallBackResultT<String> _callback)
    {
        getUSServer().getBM().getBM(UsCidBO.class).findAll("cid", _cid, new _ASelectCallback<List<UsCidBO>>()
        {
            @Override
            public void dealSuc(List<UsCidBO> _obj)
            {
                if (_obj == null || _obj.isEmpty())
                {
                    _callback.onRunOver(CommErr.PLAYER_NOT_FOUND, "");
                } else
                {
                    _callback.onRunOver(Result.SUCC, _obj.get(0).getUid());
                }
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(CommErr.PLAYER_NOT_FOUND, "");
            }
        });
    }

    /****************
     * 获取用户的cid，并调用回调处理
     * @param _uid
     * @param _callback
     */
    public void getOrCreateUserCid(String _uid, _AUserIndexCallback _callback)
    {
        //无回调则不处理
        if (null == _callback)
            return;

        long cid = 0;
        //尝试从当前数据集获取，有数据则直接处理
        _lock();

        try
        {
            cid = lookupUserCid(_uid);
        } finally
        {
            _unlock();
        }

        //判断是否有有效数据
        if (0 != cid)
        {
            //调用回调并返回
            _callback.getCidForUid(_uid, cid);
            return;
        }

        //从数据库获取或创建信息
        _getOrCreateCidFromDB(_uid, _callback);
    }

    /****************
     * 从数据库取出cid，如果没有记录则尝试创建，并调用回调
     * @param _uid
     * @param _callback
     */
    protected void _getOrCreateCidFromDB(String _uid, _AUserIndexCallback _callback)
    {
        getUSServer().getBM().getBM(UsCidBO.class).findAll("uid", _uid, new _ASelectCallback<List<UsCidBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_server, "_getCidFromDB deal fail uid:{}", _uid);
                //获取失败此时表示数据库有问题，不能继续操作
                _callback.getCidForUid(_uid, 0);
            }

            @Override
            public void dealSuc(List<UsCidBO> _list)
            {
                long cid = 0;
                do
                {
                    //先判断本地是否有对应cid，无才创建新的。确保数据不会重复创建
                    //判断是否有数据，无数据则需要处理
                    if (null == _list)
                    {
                        //null为非法数据
                        break;
                    }

                    //判断列表数据是否为空，如空创建
                    if (_list.isEmpty())
                    {
                        cid = _tryAddNewCid(_uid);
                    } else
                    {
                        _lock();
                        try
                        {
                            //此时要再检查一次是否有数据，避免重复操作
                            cid = lookupUserCid(_uid);
                            if (0 != cid)
                            {
                                break;
                            }

                            //取出数据放入
                            UsCidBO bo = _list.get(0);
                            cid = bo.getCid();

                            //注册，并处理
                            _m_htUserIdToCidTable.put(_uid, cid);
                            _m_htCidToUserIdTable.put(cid, _uid);

                        } finally
                        {
                            _unlock();
                        }
                    }

                } while (false);

                //处理回调
                _callback.getCidForUid(_uid, cid);
            }
        });
    }

    /*******************
     * 尝试添加新的cid
     * @param _uid
     */
    private long _tryAddNewCid(String _uid)
    {
        long cid = 0;

        do
        {
            _lock();

            try
            {
                //先判断本地是否有对应cid，无才创建新的。确保数据不会重复创建
                cid = lookupUserCid(_uid);
                if (0 != cid)
                {
                    break;
                }

                //创建新cid
                cid = UsID.makeCid(getUSServer());

                //插入数据
                UsCidBO newBO = new UsCidBO();
                newBO.setUid(getUSServer().getBM(), _uid);
                newBO.setCid(getUSServer().getBM(), cid);
                newBO.insert(getUSServer().getBM());

                //注册，并处理
                _m_htUserIdToCidTable.put(_uid, cid);
                _m_htCidToUserIdTable.put(cid, _uid);

            } finally
            {
                _unlock();
            }
        } while (false);

        return cid;
    }

    /**
     * 解除玩家uid与cid的关联（玩家uid）
     *
     * 执行流程：
     * 1. 查询uid对应的cid
     * 2. 从双向索引中移除映射关系
     * 3. 从数据库中删除关联记录
     *
     * @param _uid 玩家uid
     *
     * 线程安全：通过_m_mutex保护
     */
    public void cmdUnlinkUid(String _uid)
    {
        _lock();
        try
        {
            Long cid = _m_htUserIdToCidTable.get(_uid);
            _m_htUserIdToCidTable.remove(_uid);
            if (cid != null)
            {
                _m_htCidToUserIdTable.remove(cid);
            }
            getUSServer().getBM().getBM(UsCidBO.class).delAll("uid", _uid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 将uid和cid关联（玩家uid, cid）
     *
     * 执行流程：
     * 1. 先解除原有关联
     * 2. 建立新的数据库记录
     * 3. 更新双向索引
     *
     * @param _uid uid
     * @param _cid cid
     *
     * 线程安全：通过_m_mutex保护
     */
    public void cmdLinkUidWithCid(String _uid, long _cid)
    {
        _lock();
        try
        {
            //先解除原有关联
            cmdUnlinkUid(_uid);

            //再建立新关联
            BM bmObj = getUSServer().getBM();
            UsCidBO bo = new UsCidBO();
            bo.setUid(bmObj, _uid);
            bo.setCid(bmObj, _cid);
            bo.insert(bmObj);
            _m_htUserIdToCidTable.put(_uid, _cid);
            _m_htCidToUserIdTable.put(_cid, _uid);
        } finally
        {
            _unlock();
        }
    }
}
