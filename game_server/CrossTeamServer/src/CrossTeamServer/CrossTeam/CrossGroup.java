package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexAtom;
import CTSDB.Bo.CrossGroupBO;
import Common.Common_LongList;
import CrossTeamServer.CrossTeamServer;
import NPCommon.Util.CommonFunc;

import java.nio.ByteBuffer;
import java.util.HashSet;

/**
 * 分组数据，管理该分组内US列表，队伍管理对象
 */
public class CrossGroup
{
    //数据库对象
    private CrossGroupBO _m_bo;

    // US服务器ID集合
    private HashSet<Long> _m_usIdSet;

    //队伍管理器
    private CrossTeamMgr _m_mgrTeamMgr;

    //玩家队伍申请管理器
    private CrossTeamPlayerApplyMgr _m_mgrPlayerApplyMgr;

    //锁对象
    private MutexAtom _m_mutex;

    public CrossGroup(CrossGroupBO _bo)
    {
        _m_bo = _bo;

        _m_usIdSet = new HashSet<>();

        if(null != _m_bo.getJoinerList())
        {
            ByteBuffer buff = ByteBuffer.wrap(_m_bo.getJoinerList());
            Common_LongList usListObj = new Common_LongList();
            usListObj.readPackage(buff);

            _m_usIdSet.addAll(usListObj.getValueList());
        }

        _m_mgrTeamMgr = new CrossTeamMgr(this);

        _m_mgrPlayerApplyMgr = new CrossTeamPlayerApplyMgr(this);

        _m_mutex = new MutexAtom();
    }
    public CrossGroup(CrossGroupBO _bo, long _usId)
    {
        _m_bo = _bo;

        _m_usIdSet = new HashSet<>();
        _m_usIdSet.add(_usId);

        _m_mgrTeamMgr = new CrossTeamMgr(this);
        _m_mgrPlayerApplyMgr = new CrossTeamPlayerApplyMgr(this);

        _m_mutex = new MutexAtom();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public CrossGroupBO getBo() {return _m_bo;}
    //分组ID
    public long getGroupId() {return _m_bo.getGroupId();}
    //队伍最大idx
    public int getTeamMaxIdx() {return _m_bo.getTeamMaxIdx();}

    public CrossTeamMgr getTeamMgr() {return _m_mgrTeamMgr;}

    public CrossTeamPlayerApplyMgr getPlayerApplyMgr() {return _m_mgrPlayerApplyMgr;}

    /**
     * 分组数据初始化完成后调用，对队伍数据进行初始化
     */
    protected void _onInited()
    {
        _m_mgrTeamMgr._onInited();
    }

    /**
     * 保存US服务器ID集合到数据库
     */
    private void _saveUsIdSet()
    {
        Common_LongList usIdListObj = new Common_LongList();
        usIdListObj.getValueList().addAll(_m_usIdSet);

        _m_bo.setJoinerList(CrossTeamServer.getInstance().getBM(), CommonFunc.ByteBfferToBytes(usIdListObj.makePackage()));
    }

    /**
     * 增加US服务器ID，线程安全
     * @param _usId US服务器ID
     */
    public void addUsId(long _usId)
    {
        _lock();

        try
        {
            if(!_m_usIdSet.add(_usId))
                return;

            _saveUsIdSet();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除US服务器ID，线程安全
     * @param _usId US服务器ID
     */
    public void removeUsId(long _usId)
    {
        _lock();

        try
        {
            if(!_m_usIdSet.remove(_usId))
                return;

            _saveUsIdSet();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 判断US服务器ID集合是否为空，线程安全
     * @return 集合为空返回true
     */
    public boolean isUsIdSetEmpty()
    {
        _lock();

        try
        {
            return _m_usIdSet.isEmpty();
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取当前队伍idx，在之前队伍idx+1
     * @return
     */
    public int getCurTeamIdx()
    {
        _lock();

        try
        {
            int curTeamIdx = _m_bo.getTeamMaxIdx() + 1;
            _m_bo.saveTeamMaxIdx(CrossTeamServer.getInstance().getBM(), curTeamIdx);

            return curTeamIdx;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 销毁分组数据
     */
    protected void _discard()
    {
        //销毁基础数据
        _m_bo.del(CrossTeamServer.getInstance().getBM());

        //注销所有队伍的聊天房间
        _m_mgrTeamMgr._onDiscard();

        //销毁所有队伍申请数据
        _m_mgrPlayerApplyMgr._onDiscard();
    }
}
