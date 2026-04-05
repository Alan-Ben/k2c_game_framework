package NPUSServer.NPUSUserMgr.UserComp.EquipComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.HeroObj.Equip_BaseInfo;
import Common.PlayerEnum.EPlayerEventRecordType;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;
import NPEnum.*;
import NPGameRes.Refs.Equip.RefEquip;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerEquipBO;
import USDB.Bo.PlayerEquipSkillBO;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class EquipComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    //藏品列表
    private List<EquipInfo> _m_equipList;

    public EquipComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.EQUIP);

        _m_equipList = new ArrayList<>();
    }

    protected void _lock()
    {
        getUserData().lockUser();
    }

    protected void _unlock()
    {
        getUserData().unlockUser();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("init_equip_component");
        //初始化藏品数据
        process.addResDelegateProcess(_action -> _initEquipData(_action::dealAction), "init_equip_data", null, false);
        //初始化藏品技能数据
        process.addResDelegateProcess(_action -> _initEquipSkillData(_action::dealAction), "init_equip_skill_data", null, false);
        //放置藏品到大臣
        process.addResDelegateProcess(_action ->
        {
            _initPlaceEquip();
            _action.dealAction(true);
        }, "init_equip_place", null, false);
        //处理流程
        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "EquipComponent init fail");
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 初始化藏品数据
     * @param _callback
     */
    private void _initEquipData(_ICallBackBool _callback)
    {
        getUSServer().getBM().getBM(PlayerEquipBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerEquipBO>>()
        {
            @Override
            public void dealSuc(List<PlayerEquipBO> _boList)
            {
                for (PlayerEquipBO bo : _boList)
                {
                    RefEquip refEquip = RefEquip.getMgr().get(bo.getEquipId());
                    if (refEquip == null)
                    {
                        USLog.error(getUSServer(), "EquipComponent _initEquipData refEquip not found, cid:{} equipId:{}", getUserData().getCid(), bo.getEquipId());
                        continue;
                    }

                    EquipInfo equipInfo = new EquipInfo(EquipComponent.this, bo, refEquip);
                    _m_equipList.add(equipInfo);
                }
                _callback.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(false);
            }
        });
    }

    /**
     * 初始化藏品数据
     * @param _callback
     */
    private void _initEquipSkillData(_ICallBackBool _callback)
    {
        getUSServer().getBM().getBM(PlayerEquipSkillBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerEquipSkillBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerEquipSkillBO> _boList)
                    {
                        for (PlayerEquipSkillBO bo : _boList)
                        {
                            EquipInfo equipInfo = lookupEquipByDbId(bo.getEquipDbId());
                            if (equipInfo == null)
                            {
                                USLog.error(getUSServer(), "EquipComponent _initEquipSkillData equipInfo not found, cid:{} equipId:{}",
                                        getUserData().getCid(), bo.getEquipDbId());
                                continue;
                            }

                            equipInfo._initSkill(bo);
                        }
                        _callback.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _callback.onRunOver(false);
                    }
                });
    }

    /**
     * 查询藏品
     * @param _equipDbId
     * @return
     */
    public EquipInfo lookupEquipByDbId(long _equipDbId)
    {
        for (EquipInfo equipInfo : _m_equipList)
        {
            if (equipInfo.getDbId() == _equipDbId)
                return equipInfo;
        }
        return null;
    }
    
    /**
     * 获取指定关联伙伴的藏品数据
     * @param _heroId
     * @return
     */
    public EquipInfo lookupEquipByHeroId(long _heroId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		for (EquipInfo equipInfo : _m_equipList)
            {
                if (equipInfo.getWearHeroId() == _heroId)
                    return equipInfo;
            }
            return null;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 初始化放置藏品
     */
    public void _initPlaceEquip()
    {
        //初始化放置藏品
        for (EquipInfo equipInfo : _m_equipList)
        {
            if (equipInfo.getWearHeroId() > 0)
            {
                equipInfo.onPlaceToHero();
            }
        }
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return new ENPPlayerCompType[]{ENPPlayerCompType.HERO};
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {

    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.EQUIP;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        return getEquipCount(_itemId);
    }

    /**
     * 获取藏品数量
     * @param _equipId
     * @return
     */
    private long getEquipCount(long _equipId)
    {
        _lock();
        try
        {
            int count = 0;
            for (EquipInfo equipInfo : _m_equipList)
            {
                if (equipInfo.getEquipId() == _equipId)
                    count++;
            }
            return count;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return getEquipCount(_itemId) >= _count;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        for (long i = 0; i < _count; i++)
        {
            gainEquip(_itemId, true, _context);
        }
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        for (long i = 0; i < _count; i++)
        {
            gainEquip(_itemId, false, _context);
        }
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    /**
     * 获取藏品
     * @param _equipId
     * @param _isInit
     * @param _context
     */
    public Result gainEquip(long _equipId, boolean _isInit, NPPlayerContext _context)
    {
        RefEquip refEquip = RefEquip.getMgr().get(_equipId);
        if (refEquip == null)
        {
            USLog.error(getUSServer(), "EquipComponent _gainEquip refEquip not found, cid:{} equipId:{}", getUserData().getCid(), _equipId);
            return CommErr.REF_NOT_FOUND;
        }

        _lock();
        try
        {
            //判断是否达到上限
            if (refEquip.num_limit != 0 && getEquipCount(_equipId) >= refEquip.num_limit)
            {
                USLog.error(getUSServer(), "EquipComponent _gainEquip equip num reach max, cid:{} equipId:{}", getUserData().getCid(), _equipId);
                return HeroErr.EQUIP_NUM_REACH_LIMIT;
            }

            //构造数据
            PlayerEquipBO bo = new PlayerEquipBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setEquipId(getUSServer().getBM(), _equipId);
            bo.setLevel(getUSServer().getBM(), 1);
            bo.insert(getUSServer().getBM());

            EquipInfo equipInfo = new EquipInfo(this, bo, refEquip);
            _m_equipList.add(equipInfo);

            //检查解锁技能
            equipInfo._initNewCheckUnlockSkill();

            //新增藏品推送
            if (!_isInit)
                getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_065_OnEquipAdd(equipInfo.makeDetailInfo()));

            //首次获得要记录玩家解锁了新的藏品种类
            long oriCount = getUserData().getEventRecordComp().getCount(EPlayerEventRecordType.GAIN_EQUIP.ordinal(), _equipId);
            if (oriCount == 0)
                getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.UNLOCK_NEW_EQUIP_TYPE_COUNT, 1, _context);

            //记录获得藏品事件
            NPSynPlayerEvnetRecordTask.syncRecord(getUserData(), ENCounterDealType.ADD,
                    EPlayerEventRecordType.GAIN_EQUIP.ordinal(), _equipId, 1);

            _context.collectItem(getItemType(), _equipId, 1, false);

            getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.GAIN_EQUIP_TIMES, 1, _context);

            if (refEquip.quality == EQuality.RED && _context.getContextId() != ENPGameEvent.GM_CMD.ordinal())
            {
                ArrayList<String> paramList = new ArrayList<>();
                paramList.add(getUserData().getPlayerComponent().getName());
                paramList.add(refEquip.name);

                getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().marquee_gain_ur_museum_item_marquee_id, paramList);
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 分解藏品
     * @param _equipDbIdList
     */
    public Result disassemble(List<Long> _equipDbIdList, NPPlayerContext _context)
    {
        _lock();
        try
        {
            List<EquipInfo> needDealEquipList = new ArrayList<>();
            for (long equipDbId : _equipDbIdList)
            {
                EquipInfo equipInfo = lookupEquipByDbId(equipDbId);
                if (equipInfo == null)
                    return HeroErr.EQUIP_NOT_FOUND;

                //检查是否锁定
                if (equipInfo.isLocked())
                    return HeroErr.EQUIP_IS_LOCKED;

                //检查是否穿戴
                if (equipInfo.isWeared())
                    return HeroErr.EQUIP_IS_WEARED;

                _m_equipList.remove(equipInfo);
                needDealEquipList.add(equipInfo);
            }

            //检查是否有需要处理的藏品
            if (needDealEquipList.isEmpty())
                return CommErr.PARAM_ERROR;

            //收集分解信息用于日志记录
            JsonArray recycleArray = new JsonArray();

            //分解藏品
            List<Long> equipIdList = new ArrayList<>();
            NPItemCostCollector_nosafe itemCollector = new NPItemCostCollector_nosafe();
            for (EquipInfo equipInfo : needDealEquipList)
            {
                //计算返还道具列表
                List<NPCommonCostItem> logReturnItemList = new ArrayList<>();

                equipInfo.disassemble(itemCollector, logReturnItemList, _context);
                equipIdList.add(equipInfo.getDbId());

                //构造JSON对象
                JsonObject equipObj = new JsonObject();
                equipObj.addProperty("equip_id", equipInfo.getEquipId());
                equipObj.addProperty("equip_level", equipInfo.getLevel());
                equipObj.addProperty("return_items", CommonFunc.list2String(logReturnItemList));

                recycleArray.add(equipObj);
            }

            //发放分解获得的道具
            getUserData().gainItemList(itemCollector.getItemList(), _context);

            //推送消息
            getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_068_OnEquipRemove(equipIdList));

            //记录藏品重塑日志
            MJEventLog.logArtifactRecycle(getUserData(), needDealEquipList.size(), recycleArray.toString());

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取藏品列表
     * @return
     */
    public void makeProtocol(ArrayList<Equip_BaseInfo> _equipList)
    {
        _lock();
        try
        {
            for (EquipInfo equipInfo : _m_equipList)
            {
                _equipList.add(equipInfo.makeBaseInfo());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有藏品信息列表（用于GM命令）
     *
     * 功能说明：
     * 返回当前玩家的所有藏品对象列表的副本
     *
     * 线程安全：通过玩家锁保护
     *
     * @return 藏品信息列表的副本
     */
    public List<EquipInfo> getAllEquipList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_equipList);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * GM命令：从列表中移除藏品
     * @param _equipDbId 藏品数据库ID
     */
    public Result gmRemoveEquip(long _equipDbId,NPPlayerContext _context)
    {
        _lock();
        try
        {
            EquipInfo equipInfo = lookupEquipByDbId(_equipDbId);
            if (equipInfo == null)
                return HeroErr.EQUIP_NOT_FOUND;

            // 调用GM删除方法
            Result result = equipInfo.gmDelete(_context);
            if (!result.isSucc())
                return result;

            _m_equipList.remove(equipInfo);

            //推送消息
            getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_068_OnEquipRemove(Collections.singletonList(_equipDbId)));

            return Result.SUCC;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * GM命令：重置藏品技能为1%
     * @param _equipDbId 藏品数据库ID
     */
    public Result gmResetSkills(long _equipDbId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            EquipInfo equipInfo = lookupEquipByDbId(_equipDbId);
            if (equipInfo == null)
                return HeroErr.EQUIP_NOT_FOUND;

            return equipInfo.gmResetAllSkills(_context);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * GM命令：设置藏品等级
     * @param _equipDbId 藏品数据库ID
     * @param _level 目标等级
     */
    public Result gmSetLevel(long _equipDbId, int _level, NPPlayerContext _context)
    {
        _lock();
        try
        {
            EquipInfo equipInfo = lookupEquipByDbId(_equipDbId);
            if (equipInfo == null)
                return HeroErr.EQUIP_NOT_FOUND;

            return equipInfo.gmSetLevel(_level, _context);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * GM命令：一键下架所有藏品
     */
    public WCGPairInt gmUnWearAll(NPPlayerContext _context)
    {
        _lock();
        try
        {
            int unWearCount = 0;
            int failCount = 0;

            for (EquipInfo equipInfo : _m_equipList)
            {
                if (equipInfo.isWeared())
                {
                    Result result = equipInfo.unWearHero(_context);
                    if (result.isSucc())
                    {
                        unWearCount++;
                    }
                    else
                    {
                        failCount++;
                    }
                }
            }
            return new WCGPairInt(unWearCount, failCount);
        }
        finally
        {
            _unlock();
        }
    }
}
