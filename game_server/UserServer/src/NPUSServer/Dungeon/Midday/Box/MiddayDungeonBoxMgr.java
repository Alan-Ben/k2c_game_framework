package NPUSServer.Dungeon.Midday.Box;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.DungeonEnum.EDungeonBoxType;
import Common.DungeonObj.MiddayDungeon_BoxInfo;
import Common.DungeonObj.MiddayDungeon_DrawRecord;
import Common.NpChatObj.ChatObj_MiddayDungeonBox;
import NPCommon.ErrMain.MiddayDungeonErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonBox;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Dungeon.Midday.MiddayDungeonMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.MiddayDungeonBoxBO;

import java.util.ArrayList;
import java.util.List;

public class MiddayDungeonBoxMgr
{
    private MiddayDungeonMgr _m_mgr;
    private List<MiddayDungeonBoxInfo> _m_boxList;
    private MutexObject _m_mutex;

    public MiddayDungeonBoxMgr(MiddayDungeonMgr _mgr)
    {
        _m_mgr = _mgr;
        _m_boxList = new ArrayList<>();
        _m_mutex = new MutexObject();
    }

    public NPUserServer getServer()
    {
        return _m_mgr.getServer();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        List<MiddayDungeonBoxBO> boList = getServer().getBM().getBM(MiddayDungeonBoxBO.class).s_findAll();
        if (boList == null)
            return false;

        for (MiddayDungeonBoxBO bo : boList)
        {
            RefMiddayDungeonBox ref = RefMiddayDungeonBox.getMgr().get(bo.getBoxRefId());
            if (ref == null)
            {
                USLog.error(getServer(), "MiddayDungeonBoxMgr.init - ref not found: boxRefId={}", bo.getBoxRefId());
                continue;
            }

            MiddayDungeonBoxInfo boxInfo = new MiddayDungeonBoxInfo(this, ref, bo);
            _m_boxList.add(boxInfo);
        }

        // 按过期时间从大到小排序
        _m_boxList.sort((o1, o2) -> Long.compare(o2.getExpiredMs(), o1.getExpiredMs()));

        return true;
    }

    /**
     * 查找宝箱
     * @param _dbId
     * @return
     */
    public MiddayDungeonBoxInfo lookupBox(long _dbId)
    {
        _lock();
        try
        {
            for (MiddayDungeonBoxInfo boxInfo : _m_boxList)
            {
                if (boxInfo.getId() == _dbId)
                    return boxInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加宝箱
     * @param _ref
     * @param _userdata
     * @param _sendTimeMs
     */
    public void addBox(RefMiddayDungeonBox _ref, NPUSUserData _userdata, long _sendTimeMs)
    {
        MiddayDungeonBoxBO bo = new MiddayDungeonBoxBO();
        bo.setBoxRefId(getServer().getBM(), _ref.Id());
        bo.setSenderCid(getServer().getBM(), _userdata.getCid());
        bo.setSenderName(getServer().getBM(), _userdata.getPlayerComponent().getName());
        bo.setExpiredMs(getServer().getBM(), _sendTimeMs + _ref.duration_sec * 1000L);
        bo.insert(getServer().getBM());

        _lock();
        try
        {
            //加入列表头部，保证按过期时间从大到小排序
            _m_boxList.add(0, new MiddayDungeonBoxInfo(this, _ref, bo));
        } finally
        {
            _unlock();
        }

        //发送宝箱到聊天频道
        ChatObj_MiddayDungeonBox proto = new ChatObj_MiddayDungeonBox();
        proto.setBoxId(_ref.Id());
        proto.setDbId(bo.getId());
        proto.setExpiredTimeMs(bo.getExpiredMs());

        ENPChatMsgType msgType =
                _ref.box_type == EDungeonBoxType.MIDDAY ? ENPChatMsgType.MIDDAY_DUNGEON_BOX : ENPChatMsgType.EVENING_DUNGEON_BOX;

        //对全服聊天频道发送消息
        _userdata.getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.US_SERVER.ordinal()
                , 0
                , msgType.ordinal()
                , _userdata.toChatPlayerProto().makePackage()
                , proto.makePackage()
                , null);
    }

    /**
     * 检查宝箱是否可以领取
     *
     * @param _dbId 宝箱数据ID
     * @param _cid 玩家CID
     * @return 是否可以领取
     */
    public boolean checkBoxCanDraw(long _dbId, long _cid)
    {
        MiddayDungeonBoxInfo boxInfo = lookupBox(_dbId);
        if (boxInfo == null)
            return false;

        if (boxInfo.hadDraw(_cid))
            return false;

        if (boxInfo.isEmpty())
            return false;

        if (boxInfo.hadExpired())
            return false;

        return true;
    }

    /**
     * 获取宝箱剩余可领取次数
     *
     * @param _dbId 宝箱数据ID
     * @return 剩余可领取次数，宝箱不存在返回0
     */
    public int getBoxRemainDrawCount(long _dbId)
    {
        MiddayDungeonBoxInfo boxInfo = lookupBox(_dbId);
        if (boxInfo == null)
            return 0;

        return boxInfo.getRemainDrawCount();
    }

    /**
     * 获取宝箱领取记录列表
     *
     * @param _dbId 宝箱数据ID
     * @return 领取记录列表，宝箱不存在返回空列表
     */
    public List<MiddayDungeon_DrawRecord> getBoxDrawRecordList(long _dbId)
    {
        MiddayDungeonBoxInfo boxInfo = lookupBox(_dbId);
        if (boxInfo == null)
            return new ArrayList<>();

        return boxInfo.getDrawRecordList();
    }

    /**
     * 领取宝箱
     * @param _dbId
     * @param _userdata
     * @param _context
     * @return
     */
    public Result drawBox(long _dbId, NPUSUserData _userdata, NPPlayerContext _context)
    {
        checkDelBox();

        //查找宝箱
        MiddayDungeonBoxInfo boxInfo = lookupBox(_dbId);
        if (boxInfo == null)
            return MiddayDungeonErr.BOX_EXPIRED;

        return boxInfo.draw(_userdata, _context);
    }

    /**
     * 检查移除宝箱
     */
    public void checkDelBox()
    {
        _lock();
        try
        {
            List<Long> delBoxIdList = null;

            for (int i = _m_boxList.size() - 1; i >= 0; --i)
            {
                MiddayDungeonBoxInfo boxInfo = _m_boxList.get(i);
                if (!boxInfo.hadExpired())
                    break;

                _m_boxList.remove(i);

                if (delBoxIdList == null)
                    delBoxIdList = new ArrayList<>();
                delBoxIdList.add(boxInfo.getId());
            }

            //从数据库删除宝箱数据
            if (delBoxIdList != null)
            {
                getServer().getBM().getBM(MiddayDungeonBoxBO.class).delAllInList("id", delBoxIdList);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取宝箱列表
     * @return
     */
    public List<MiddayDungeon_BoxInfo> makeAllBoxList(EDungeonBoxType _boxType)
    {
        List<MiddayDungeon_BoxInfo> list = new ArrayList<>();

        _lock();
        try
        {
            for (MiddayDungeonBoxInfo boxInfo : _m_boxList)
            {
                if (boxInfo.getBoxType() != _boxType)
                    continue;

                if (boxInfo.hadExpired())
                    continue;

                list.add(boxInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }

        return list;
    }
}
