package NPUSServer.Dungeon.Midday.Box;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.Common_LongList;
import Common.DungeonEnum.EDungeonBoxType;
import Common.DungeonObj.MiddayDungeon_BoxInfo;
import Common.DungeonObj.MiddayDungeon_DrawRecord;
import Common.DungeonObj.MiddayDungeon_DrawRecordList;
import NPCommon.ErrMain.MiddayDungeonErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Dungeon.Midday.RefMiddayDungeonBox;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.MiddayDungeonBoxBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class MiddayDungeonBoxInfo
{
    private MiddayDungeonBoxMgr _m_mgr;
    private RefMiddayDungeonBox _m_ref;
    private MiddayDungeonBoxBO _m_bo;
    // 宝箱领取记录列表，记录领取的玩家CID和领取时间戳
    private List<MiddayDungeon_DrawRecord> _m_drawRecordList;
    private MutexAtom _m_mutex;

    public MiddayDungeonBoxInfo(MiddayDungeonBoxMgr _mgr, RefMiddayDungeonBox _ref, MiddayDungeonBoxBO _bo)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;
        _m_bo = _bo;
        _m_mutex = new MutexAtom();
        _m_drawRecordList = new ArrayList<>();

        // 加载宝箱领取记录数据，兼容旧数据格式
        if (_m_bo.getHadDrawCidList() != null)
        {
            try
            {
                // 尝试使用新格式读取（MiddayDungeon_DrawRecordList）
                MiddayDungeon_DrawRecordList proto = new MiddayDungeon_DrawRecordList();
                proto.readPackage(ByteBuffer.wrap(_m_bo.getHadDrawCidList()));
                _m_drawRecordList = proto.getRecordList();
            }
            catch (Exception e)
            {
                // 读取新格式失败，尝试使用旧格式读取（Common_LongList）
                try
                {
                    Common_LongList oldProto = new Common_LongList();
                    oldProto.readPackage(ByteBuffer.wrap(_m_bo.getHadDrawCidList()));

                    // 将旧数据转换为新格式，领取时间设为0
                    for (Long cid : oldProto.getValueList())
                    {
                        MiddayDungeon_DrawRecord record = new MiddayDungeon_DrawRecord();
                        record.setCid(cid);
                        record.setDrawTimeMs(0L);
                        _m_drawRecordList.add(record);
                    }

                    USLog.sys(_m_mgr.getServer(), "MiddayDungeonBoxInfo - data format migration: converted old Common_LongList to MiddayDungeon_DrawRecordList, boxId={}, recordCount={}",
                             _m_bo.getId(), _m_drawRecordList.size());
                }
                catch (Exception ex)
                {
                    // 两种格式都读取失败，记录错误并使用空列表
                    USLog.error(_m_mgr.getServer(), "MiddayDungeonBoxInfo - load draw record failed: failed to parse both new and old format, boxId={}, error={}",
                               _m_bo.getId(), ex.getMessage());
                    _m_drawRecordList = new ArrayList<>();
                }
            }
        }
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getId()
    {
        return _m_bo.getId();
    }

    public EDungeonBoxType getBoxType()
    {
        return _m_ref.box_type;
    }

    public long getExpiredMs()
    {
        return _m_bo.getExpiredMs();
    }

    /**
     * 是否已领取
     *
     * @param _cid 玩家CID
     * @return 是否已领取
     */
    public boolean hadDraw(long _cid)
    {
        _lock();
        try{
            for (MiddayDungeon_DrawRecord record : _m_drawRecordList)
            {
                if (record.getCid() == _cid)
                    return true;
            }
            return false;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 是否过期
     * @return
     */
    public boolean hadExpired()
    {
        return _m_bo.getExpiredMs() < CommonFunc.getNowTimeMS();
    }

    /**
     * 是否已领完
     *
     * @return 是否已领完
     */
    public boolean isEmpty()
    {
        _lock();
        try{
            return _m_drawRecordList.size() >= _m_ref.can_draw_limit;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取剩余可领取次数
     *
     * @return 剩余可领取次数
     */
    public int getRemainDrawCount()
    {
        _lock();
        try{
            int remain = _m_ref.can_draw_limit - _m_drawRecordList.size();
            return Math.max(0, remain);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取宝箱领取记录列表
     *
     * @return 领取记录列表
     */
    public List<MiddayDungeon_DrawRecord> getDrawRecordList()
    {
        _lock();
        try{
            return new ArrayList<>(_m_drawRecordList);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 领取宝箱
     *
     * 执行流程：
     * 1. 检查是否已经领取
     * 2. 检查是否过期
     * 3. 检查领取CD
     * 4. 检查是否领完
     * 5. 记录领取信息（玩家CID和领取时间戳）
     * 6. 消耗CD
     * 7. 发放宝箱奖励
     *
     * @param _userdata 玩家数据
     * @param _context 操作上下文
     * @return 领取结果
     *
     * 线程安全：使用玩家级别锁保护数据一致性
     */
    public Result draw(NPUSUserData _userdata, NPPlayerContext _context)
    {
        // 检查是否已经领取
        if (hadDraw(_userdata.getCid()))
            return MiddayDungeonErr.ALREADY_DRAW_BOX;

        // 检查是否过期
        if (hadExpired())
            return MiddayDungeonErr.BOX_EXPIRED;

        // 检查CD
        if (!_userdata.hasItem(ENPItemType.FIXED_CD, _m_ref.draw_box_fixed_cd_id, 1))
            return MiddayDungeonErr.DRAW_BOX_REACH_LIMIT;

        // 检查是否领完了
        _lock();
        try{
            if (isEmpty())
                return MiddayDungeonErr.BOX_EMPTY;

            // 创建领取记录，包含玩家CID和领取时间戳
            MiddayDungeon_DrawRecord record = new MiddayDungeon_DrawRecord();
            record.setCid(_userdata.getCid());
            record.setDrawTimeMs(CommonFunc.getNowTimeMS());
            _m_drawRecordList.add(record);

            // 序列化并保存到数据库
            MiddayDungeon_DrawRecordList listProto = new MiddayDungeon_DrawRecordList();
            listProto.getRecordList().addAll(_m_drawRecordList);
            _m_bo.saveHadDrawCidList(_m_mgr.getServer().getBM(), listProto.makePackage().array());
        }finally
        {
            _unlock();
        }

        // 消耗CD
        boolean consumeSucc = _userdata.spendItem(ENPItemType.FIXED_CD, _m_ref.draw_box_fixed_cd_id, 1, _context);
        if (!consumeSucc)
            return MiddayDungeonErr.DRAW_BOX_REACH_LIMIT;

        // 发放宝箱奖励
        _userdata.gainItemList(_m_ref.item_list, _context);

        return Result.SUCC;
    }

    /**
     * 构造宝箱信息
     * @return
     */
    public MiddayDungeon_BoxInfo makeProto()
    {
        MiddayDungeon_BoxInfo proto = new MiddayDungeon_BoxInfo();
        proto.setDbId(_m_bo.getId());
        proto.setBoxId(_m_bo.getBoxRefId());
        proto.setSenderCid(_m_bo.getSenderCid());
        proto.setSenderName(_m_bo.getSenderName());
        proto.setExpireTimeMS(_m_bo.getExpiredMs());
        return proto;
    }

    public void discard()
    {
        _m_bo.del(_m_mgr.getServer().getBM());
    }
}
