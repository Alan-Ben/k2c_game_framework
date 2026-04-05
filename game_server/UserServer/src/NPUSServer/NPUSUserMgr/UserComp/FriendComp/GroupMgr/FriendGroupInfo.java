package NPUSServer.NPUSUserMgr.UserComp.FriendComp.GroupMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_LongList;
import Common.FriendObj.Friend_CustomGroupInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerFriendGroupBO;

import java.nio.ByteBuffer;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class FriendGroupInfo
{
    private NPUserServer _m_server;

    private long _m_dbId;
    //分组名称
    private String _m_name;
    //好友CID列表
    private Set<Long> _m_cidSet;
    //列表锁
    private MutexAtom _m_mutex = new MutexAtom();

    public FriendGroupInfo(NPUserServer _server, PlayerFriendGroupBO _bo)
    {
        _m_server = _server;

        _m_dbId = _bo.getId();
        _m_name = _bo.getName();
        _m_cidSet = new HashSet<>();

        //从数据库中读取好友CID列表
        if (_bo.getFriendCidList() != null)
        {
            Common_LongList longList = new Common_LongList();
            longList.readPackage(ByteBuffer.wrap(_bo.getFriendCidList()));
            _m_cidSet.addAll(longList.getValueList());
        }
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

    public NPUserServer getUSServer() {return _m_server;}
    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 是否包含好友
     * @param _cid 好友CID
     * @return 是否包含
     */
    public boolean hasFriend(long _cid)
    {
        return _m_cidSet.contains(_cid);
    }

    /**
     * 修改分组名称
     */
    public void changeName(String _name)
    {
        _m_name = _name;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("name", _name);
        getUSServer().getBM().getBM(PlayerFriendGroupBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 添加好友CID
     */
    protected void _addFriend(List<Long> _cidList)
    {
        _lock();
        try{
            _m_cidSet.addAll(_cidList);
        }finally{
            _unlock();
        }
    }

    /**
     * 删除好友CID
     */
    protected void _removeFriend(long _cid)
    {
        _lock();
        try{
            _m_cidSet.remove(_cid);
        }finally{
            _unlock();
        }
    }

    /**
     * 好友列表变更写入数据库
     */
    protected void _saveFriendList()
    {
        _lock();
        try{
            Common_LongList longList = new Common_LongList();
            longList.getValueList().addAll(_m_cidSet);

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("friend_cid_list", CommonFunc.ByteBfferToBytes(longList.makePackage()));
            getUSServer().getBM().getBM(PlayerFriendGroupBO.class).update("id", _m_dbId, updateValue);
        }finally{
            _unlock();
        }
    }

    /**
     * 销毁当前分组数据
     */
    public void discard()
    {
        getUSServer().getBM().getBM(PlayerFriendGroupBO.class).delAll("id", _m_dbId);
    }

    public Friend_CustomGroupInfo toProto()
    {
        _lock();
        try{
            Friend_CustomGroupInfo proto = new Friend_CustomGroupInfo();
            proto.setDbId(_m_dbId);
            proto.setName(_m_name);
            proto.getCidList().addAll(_m_cidSet);
            return proto;
        }finally
        {
            _unlock();
        }
    }
}
