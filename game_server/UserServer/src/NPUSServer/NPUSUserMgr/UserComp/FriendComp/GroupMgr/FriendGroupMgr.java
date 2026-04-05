package NPUSServer.NPUSUserMgr.UserComp.FriendComp.GroupMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.FriendObj.Friend_CustomGroupInfo;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.FriendErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerFriendGroupBO;
import USDB.Bo.PlayerFriendMgrBO;

import java.nio.ByteBuffer;
import java.util.*;

public class FriendGroupMgr
{
    private FriendComponent _m_comp;
    //好友分组列表
    private List<FriendGroupInfo> _m_groupList;
    //数据锁
    private MutexObject _m_mutex;
    //分组顺序列表
    private List<Long> _m_groupOrderList;

    public FriendGroupMgr(FriendComponent _comp)
    {
        _m_comp = _comp;
        _m_groupList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    /**
     * 锁与解锁
     */
    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 初始化玩家的分组顺序
     * @param _bo 玩家好友管理器数据
     */
    public void initGroupOrderFromDb(PlayerFriendMgrBO _bo)
    {
        if (_bo.getOrderList() != null)
        {
            Common_LongList longList = new Common_LongList();
            longList.readPackage(ByteBuffer.wrap(_bo.getOrderList()));

            _m_groupOrderList = longList.getValueList();
        }
    }

    /**
     * 加载玩家的分组信息
     */
    public void initFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerFriendGroupBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerFriendGroupBO>>()
        {
            @Override
            public void dealSuc(List<PlayerFriendGroupBO> _boList)
            {
                for (PlayerFriendGroupBO bo : _boList)
                {
                    FriendGroupInfo group = new FriendGroupInfo(getUserData().getUSServer(), bo);
                    _m_groupList.add(group);
                }

                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "Can not load PlayerFriendGroupBO Data[cid:" + getUserData().getCid() + "]");
                _handler.onRunOver(false);
            }
        });
    }

    /**
     * 查询分组信息
     * @param _dbId
     */
    public FriendGroupInfo lookupGroup(long _dbId)
    {
        _lock();
        try
        {
            for (FriendGroupInfo group : _m_groupList)
            {
                if (group.getDbId() == _dbId)
                {
                    return group;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 变更分组顺序
     * @param _groupOrderList 排序列表
     */
    public Result chgGroupOrderList(List<Long> _groupOrderList)
    {
        _lock();
        try
        {
            //去重
            Set<Long> groupIdSet = new HashSet<>(_groupOrderList);

            //检查是否有重复的id
            if (groupIdSet.size() != _groupOrderList.size())
                return CommErr.PARAM_ERROR;

            //检查分组列表id列表是否符合玩家的分组列表
            groupIdSet.remove(0L);
            for (FriendGroupInfo groupInfo : _m_groupList)
            {
                groupIdSet.remove(groupInfo.getDbId());
            }

            if (!groupIdSet.isEmpty())
                return CommErr.PARAM_ERROR;

            //替换排序列表
            _m_groupOrderList = _groupOrderList;
            _saveGroupOrderList();

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 保存分组顺序数据
     */
    private void _saveGroupOrderList()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        Common_LongList longList = new Common_LongList();
        longList.getValueList().addAll(_m_groupOrderList);
        updateValue.addValueObj("order_list", longList.makePackage().array());
        _m_comp.getUSServer().getBM().getBM(PlayerFriendMgrBO.class).update("id", _m_comp.getMgrBoDbId(), updateValue);

        //通知客户端
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_055_OnFriendGroupOrderListChg(_m_groupOrderList));
    }

    /**
     * 添加分组
     * @param _name
     */
    public FriendGroupInfo addGroup(String _name)
    {
        _lock();
        try
        {
            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerFriendGroupBO bo = new PlayerFriendGroupBO();
            bo.setCid(bmObj, getUserData().getCid());
            bo.setName(bmObj, _name);
            bo.insert(bmObj);

            FriendGroupInfo group = new FriendGroupInfo(getUserData().getUSServer(), bo);
            _m_groupList.add(group);

            //加入到排序列表
            _m_groupOrderList.add(group.getDbId());
            _saveGroupOrderList();

            //通知客户端
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_056_OnFriendGroupCreate(bo.getId(), _name));

            return group;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 删除分组
     * @param _dbId
     */
    public Result removeGroup(long _dbId)
    {
        _lock();
        try
        {
            FriendGroupInfo group = lookupGroup(_dbId);
            if (group == null)
                return FriendErr.FRIEND_GROUP_NOT_FOUND;

            _m_groupList.remove(group);
            group.discard();

            //从排序列表中删除
            _m_groupOrderList.remove(group.getDbId());
            _saveGroupOrderList();

            //通知客户端
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_057_OnFriendGroupDelete(_dbId));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 变更分组名称
     * @param _dbId 分组id
     * @param _name 名称
     */
    public Result chgGroupName(long _dbId, String _name)
    {
        _lock();
        try
        {
            FriendGroupInfo group = lookupGroup(_dbId);
            if (group == null)
                return FriendErr.FRIEND_GROUP_NOT_FOUND;

            group.changeName(_name);

            //通知客户端
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_059_OnFriendGroupNameChg(_dbId, _name));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 当好友被删除的时候
     * @param _friendCid 好友CID
     */
    public void onFriendRemove(long _friendCid)
    {
        _lock();
        try
        {
            for (FriendGroupInfo group : _m_groupList)
            {
                if (group.hasFriend(_friendCid))
                {
                    group._removeFriend(_friendCid);
                    group._saveFriendList();
                    return;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 变更玩家的分组
     * @param _targetGroupId
     * @param _cidList
     */
    public Result changeGroup(long _targetGroupId, List<Long> _cidList)
    {
        _lock();
        try
        {
            //如果是移动到默认分组
            if (_targetGroupId == 0)
            {
                //需要进行保存的分组
                Set<FriendGroupInfo> needSaveGroup = new HashSet<>();

                //先遍历cid列表，把已存在的cid从原分组中删除
                for (Long cid : _cidList)
                {
                    //找到原分组
                    FriendGroupInfo oldGroup = null;
                    for (FriendGroupInfo group : _m_groupList)
                    {
                        if (group.hasFriend(cid))
                        {
                            oldGroup = group;
                            break;
                        }
                    }

                    //如果原分组不为空，就把cid从原分组中删除
                    if (oldGroup != null)
                    {
                        oldGroup._removeFriend(cid);
                        needSaveGroup.add(oldGroup);
                    }
                }

                //保存所有需要保存的分组
                for (FriendGroupInfo group : needSaveGroup)
                {
                    group._saveFriendList();
                }
            } else
            {
                FriendGroupInfo targetGroup = lookupGroup(_targetGroupId);
                if (targetGroup == null)
                    return FriendErr.FRIEND_GROUP_NOT_FOUND;

                //需要进行保存的分组
                Set<FriendGroupInfo> needSaveGroup = new HashSet<>();

                //先遍历cid列表，把已存在的cid从原分组中删除
                for (Long cid : _cidList)
                {
                    //找到原分组
                    FriendGroupInfo oldGroup = null;
                    for (FriendGroupInfo group : _m_groupList)
                    {
                        //跳过目标分组
                        if (group == targetGroup)
                            continue;

                        if (group.hasFriend(cid))
                        {
                            oldGroup = group;
                            break;
                        }
                    }

                    //如果原分组不为空，就把cid从原分组中删除
                    if (oldGroup != null)
                    {
                        oldGroup._removeFriend(cid);
                        needSaveGroup.add(oldGroup);
                    }
                }

                //再把cid添加到目标分组中
                targetGroup._addFriend(_cidList);
                needSaveGroup.add(targetGroup);

                //保存所有需要保存的分组
                for (FriendGroupInfo group : needSaveGroup)
                {
                    group._saveFriendList();
                }
            }
        } finally
        {
            _unlock();
        }

        //通知客户端
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_058_OnFriendBelongGroupChg(_targetGroupId, _cidList));

        return Result.SUCC;
    }

    /**
     * 构造初始化数据
     * @param _groupList 分组列表
     * @param _orderList 排序列表
     */
    public void makeProto(List<Friend_CustomGroupInfo> _groupList, ArrayList<Long> _orderList)
    {
        _lock();
        try
        {
            _orderList.addAll(_m_groupOrderList);
            for (FriendGroupInfo groupInfo : _m_groupList)
            {
                _groupList.add(groupInfo.toProto());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查看排序列表
     * @return
     */
    public String getGroupOrderList()
    {
        return Arrays.toString(_m_groupOrderList.toArray());
    }

    /**
     * gm命令修改排序列表
     * @param _orderList 排序列表
     */
    public void cmdSetGroupOrderList(List<Long> _orderList)
    {
        _m_groupOrderList = _orderList;
        _saveGroupOrderList();
    }

    /**
     * 获取自定义分组数量
     * @return
     */
    public int getCustomGroupNum()
    {
        _lock();
        try{
            return _m_groupList.size();
        }finally
        {
            _unlock();
        }
    }
}
