package CrossTeamServer.CrossTeam;

import ALBasicServer.ALBasicMutex.MutexManager;
import CTSDB.Bo.CrossGroupBO;
import CTSDB.Bo.CrossTeamApplyBO;
import CTSDB.Bo.CrossTeamBO;
import CTSDB.Bo.CrossTeamMemberBO;
import Common.Common_LongList;
import CrossTeamServer.CrossTeamServer;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

import java.util.HashMap;
import java.util.List;

/**
 * 跨服分组管理，根据分组ID区分不同的分组
 */
public class CrossGroupMgr
{
    //单例对象
    private static final CrossGroupMgr _g_instance = new CrossGroupMgr();
    public static CrossGroupMgr getInstance() { return _g_instance; }

    //分组数据集合
    private HashMap<Long, CrossGroup> _m_hmCrossGroupMap;

    //锁对象
    private MutexManager _m_mutex;

    public CrossGroupMgr()
    {
        _m_hmCrossGroupMap = new HashMap<>();

        _m_mutex = new MutexManager();
    }

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 初始化数据库数据
     * @return
     */
    public boolean initFromDB()
    {
        //分组数据
        List<CrossGroupBO> boList = CrossTeamServer.getInstance().getBM().getBM(CrossGroupBO.class).s_findAll();
        if(null == boList)
        {
            return false;
        }
        //逐个创建实例对象
        for(int i = 0; i < boList.size(); i++)
        {
            CrossGroupBO bo = boList.get(i);
            if (null == bo)
                continue;

            //检查重复的分组ID
            if(_m_hmCrossGroupMap.containsKey(bo.getGroupId()))
            {
                CommLog.error("CrossGroupMgr::initFromDB duplicate groupId={}", bo.getGroupId());
                continue;
            }

            CrossGroup group = new CrossGroup(bo);
            //若分组内没有US服务器ID，则删除该分组数据
            if(group.isUsIdSetEmpty())
            {
                // group 尚未加入 map，_discard() 清理 DB 即可
                group._discard();
                continue;
            }

            _m_hmCrossGroupMap.put(group.getGroupId(), group);
        }

        //队伍数据
        List<CrossTeamBO> teamBoList = CrossTeamServer.getInstance().getBM().getBM(CrossTeamBO.class).s_findAll();
        if(null == teamBoList)
        {
            return false;
        }
        for(int i = 0; i < teamBoList.size(); i++)
        {
            CrossTeamBO teamBo = teamBoList.get(i);
            if(null == teamBo)
                continue;

            CrossGroup group = lookupByTeamId(teamBo.getTeamId());
            if(null == group)
                continue;

            group.getTeamMgr()._initFromDB(teamBo);
        }

        //队伍成员数据
        List<CrossTeamMemberBO> memberBoList = CrossTeamServer.getInstance().getBM().getBM(CrossTeamMemberBO.class).s_findAll();
        if(null == memberBoList)
        {
            return false;
        }
        for(int i = 0; i < memberBoList.size(); i++)
        {
            CrossTeamMemberBO memberBo = memberBoList.get(i);
            if(null == memberBo)
                continue;

            CrossGroup group = lookupByTeamId(memberBo.getTeamId());
            if(null == group)
                continue;

            CrossTeamInfo team = group.getTeamMgr().lookup(memberBo.getTeamId());
            if(null == team)
                continue;

            team.getMemberMgr()._initMemberFromDB(memberBo);

            //添加玩家-队伍映射
            group.getTeamMgr().addPlayerTeamMap(memberBo.getMemberCid(), team);
        }

        //队伍申请数据
        List<CrossTeamApplyBO> applyBoList = CrossTeamServer.getInstance().getBM().getBM(CrossTeamApplyBO.class).s_findAll();
        if(null == applyBoList)
        {
            return false;
        }
        for(int i = 0; i < applyBoList.size(); i++)
        {
            CrossTeamApplyBO applyBo = applyBoList.get(i);
            if(null == applyBo)
                continue;

            CrossGroup group = lookupByTeamId(applyBo.getTeamId());
            if(null == group)
                continue;

            CrossTeamInfo team = group.getTeamMgr().lookup(applyBo.getTeamId());
            if(null == team)
                continue;

            team.getApplyMgr()._initApplyBo(applyBo);

            group.getPlayerApplyMgr().addApply(applyBo);
        }

        //分组数据加载完成后处理
        for(CrossGroup group : _m_hmCrossGroupMap.values())
        {
            if(null == group)
                continue;

            group._onInited();
        }

        return true;
    }

    /**
     * 查找分组数据
     * @param _groupId
     * @return
     */
    public CrossGroup lookup(long _groupId)
    {
        _lock();

        try
        {
            return _m_hmCrossGroupMap.get(_groupId);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 查找分组数据（通过队伍ID）
     * @param _teamId
     * @return
     */
    public CrossGroup lookupByTeamId(long _teamId)
    {
        _lock();

        try
        {
            long groupId = CommonFunc.parseActivityTeamGroupId(_teamId);

            return _m_hmCrossGroupMap.get(groupId);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 确保分组存在，若不存在则创建
     * @param _groupId
     * @param _usId US服务器ID
     * @return
     */
    public CrossGroup ensure(long _groupId, long _usId)
    {
        _lock();

        try
        {
            // 持锁状态下直接访问 map，避免 lookup() 重复加锁导致死锁
            CrossGroup group = _m_hmCrossGroupMap.get(_groupId);
            if(null == group)
            {
                Common_LongList usIdListObj = new Common_LongList();
                usIdListObj.getValueList().add(_usId);

                CrossGroupBO bo = new CrossGroupBO();
                bo.setGroupId(CrossTeamServer.getInstance().getBM(), _groupId);
                bo.setJoinerList(CrossTeamServer.getInstance().getBM(), CommonFunc.ByteBfferToBytes(usIdListObj.makePackage()));
                bo.insert(CrossTeamServer.getInstance().getBM());

                group = new CrossGroup(bo, _usId);
                _m_hmCrossGroupMap.put(_groupId, group);
            }

            group.addUsId(_usId);

            return group;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除分组内US服务器ID
     * @param _groupId
     * @param _usId US服务器ID
     */
    public void removeUsId(long _groupId, long _usId)
    {
        _lock();

        try
        {
            // 持锁状态下直接访问 map，避免 lookup() 重复加锁导致死锁
            CrossGroup group = _m_hmCrossGroupMap.get(_groupId);
            if(null == group)
                return;

            group.removeUsId(_usId);

            //若分组内已经没有US服务器ID，则删除分组数据
            if(group.isUsIdSetEmpty())
            {
                _m_hmCrossGroupMap.remove(_groupId);
                group._discard();
            }
        }
        finally
        {
            _unlock();
        }
    }
}
