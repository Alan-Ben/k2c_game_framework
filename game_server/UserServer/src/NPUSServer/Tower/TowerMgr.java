package NPUSServer.Tower;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.TowerObj.Tower_OpponentInfo;
import Common.TowerObj.Tower_PosInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TowerErr;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Promise.SerialPromise;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.RefWrap;
import NPEnum.ENPGameEvent;
import NPGameRes.GameObjs.Tower.TowerStageRefObj;
import NPGameRes.Refs.Tower.RefTowerChapter;
import NPGameRes.Refs.Tower.RefTowerChapterStageShow;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerTowerDefenceReportBO;
import USDB.Bo.TowerFloorBO;

import java.util.ArrayList;
import java.util.List;

public class TowerMgr
{
    private NPUserServer _m_server;
    private List<TowerFloorInfo> _m_floorList;
    private MutexAtom _m_mutex;

    public TowerMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_floorList = new ArrayList<>();
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

    /**
     * 从数据库初始化塔楼数据
     * @return
     */
    public boolean initFromDB()
    {
        List<TowerFloorBO> towerFloorBoList = _m_server.getBM().getBM(TowerFloorBO.class).s_findAll();
        if (towerFloorBoList == null)
            return false;

        for (TowerFloorBO towerFloorBO : towerFloorBoList)
        {
            TowerFloorInfo floorInfo = new TowerFloorInfo(towerFloorBO);
            _m_floorList.add(floorInfo);
        }
        return true;
    }

    /**
     * 查询指定章节和等级的关卡
     * @param _chapterId
     * @param _chapterLevel
     * @return
     */
    public TowerFloorInfo lookFloor(long _chapterId, int _chapterLevel)
    {
        _lock();
        try
        {
            for (TowerFloorInfo floorInfo : _m_floorList)
            {
                if (floorInfo.getChapterId() == _chapterId && floorInfo.getChapterLevel() == _chapterLevel)
                    return floorInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 攻击
     * @param _targetChapterRef
     * @param _targetStageRefObj
     * @param _chapterId
     * @param _chapterLevel
     * @param _callback
     */
    public void attackFloor(RefTowerChapter _targetChapterRef, TowerStageRefObj _targetStageRefObj,
                            long _chapterId, int _chapterLevel, NPUSUserData _userdata, NPPlayerContext _context, _ICallBackResultT<TowerAttackResult> _callback)
    {
        // 判断是否是PVP章节，pvp章节第一关按pve处理
        if (!_targetChapterRef.if_pve_chapter && _chapterLevel != 1)
        {
            TowerFloorInfo floorInfo = lookFloor(_chapterId, _chapterLevel);
            if (floorInfo != null)
            {
                _m_server.getPlayerCacheGetter().getDealer(PlayerInfo_IconShow.class).getInfo(floorInfo.getCid(),
                        new HandlerTwo<Boolean, PlayerInfo_IconShow>()
                        {
                            @Override
                            public void handle(Boolean _isSucc, PlayerInfo_IconShow _data)
                            {
                                if (!_isSucc)
                                {
                                    _callback.onRunOver(CommErr.PLAYER_CACHE_ERR, null);
                                    return;
                                }

                                if (_data.getTotalPower() > _userdata.getHeroComponent().getTotalPower())
                                {
                                	//增加防守战报
                                    _defenderReport(floorInfo.getCid(), _userdata.getCid(), _chapterId, _chapterLevel, false, _chapterLevel);
                                	
                                    _callback.onRunOver(Result.SUCC, new TowerAttackResult(floorInfo.getCid(), false));
                                    return;
                                }

                                Result result = onPvpAttackSucc(_userdata, floorInfo.getCid(), _chapterId, _chapterLevel, false, _context);
                                if (!result.isSucc())
                                {
                                    _callback.onRunOver(result, null);
                                    return;
                                }

                                _callback.onRunOver(Result.SUCC, new TowerAttackResult(floorInfo.getCid(), true));
                            }
                        });
            } else
            {
                if (_userdata.getHeroComponent().getTotalPower() < _targetStageRefObj.calPower(_chapterLevel))
                {
                    _callback.onRunOver(Result.SUCC, new TowerAttackResult(0, false));
                    return;
                }

                Result result = tryOccupyFloor(_userdata, _chapterId, _chapterLevel, _context);
                if (!result.isSucc())
                {
                    _callback.onRunOver(result, null);
                    return;
                }

                _callback.onRunOver(Result.SUCC, new TowerAttackResult(0, true));
            }
        } else
        {
            if (_userdata.getHeroComponent().getTotalPower() < _targetStageRefObj.calPower(_chapterLevel))
            {
                _callback.onRunOver(Result.SUCC, new TowerAttackResult(0, false));
                return;
            }

            _callback.onRunOver(Result.SUCC, new TowerAttackResult(0, true));
        }
    }

    /**
     * PVP攻击成功后的处理逻辑
     * @param _userdata
     * @param _chapterId
     * @param _chapterLevel
     * @param _context
     * @return
     */
    private Result tryOccupyFloor(NPUSUserData _userdata, long _chapterId, int _chapterLevel, NPPlayerContext _context)
    {
        _lock();
        try
        {
            //判断楼层是否已被占领
            TowerFloorInfo targetFloor = lookFloor(_chapterId, _chapterLevel);
            if (targetFloor != null)
                return TowerErr.TOWER_FLOOR_HAD_BEEN_OCCUPIED;

            //查询玩家是否原来就在楼层上
            TowerFloorInfo oriFloorInfo = lookFloorByCid(_userdata.getCid());
            if (oriFloorInfo != null)
            {
                //如果原来就在楼层上, 则直接更新
                oriFloorInfo.chgPos(_m_server.getBM(), _chapterId, _chapterLevel);
            } else
            {
                TowerFloorBO bo = new TowerFloorBO();
                bo.setCid(_m_server.getBM(), _userdata.getCid());
                bo.setChapterId(_m_server.getBM(), _chapterId);
                bo.setChapterLevel(_m_server.getBM(), _chapterLevel);
                bo.insert(_m_server.getBM());

                TowerFloorInfo floorInfo = new TowerFloorInfo(bo);
                _m_floorList.add(floorInfo);
            }

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据cid查找楼层信息
     * @param _cid
     * @return
     */
    public TowerFloorInfo lookFloorByCid(long _cid)
    {
        _lock();
        try
        {
            for (TowerFloorInfo floorInfo : _m_floorList)
            {
                if (floorInfo.getCid() == _cid)
                    return floorInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * PVP攻击成功后的处理逻辑
     * @param _userdata
     * @param _defenderCid
     * @param _chapterId
     * @param _chapterLevel
     * @param _context
     * @return
     */
    public Result onPvpAttackSucc(NPUSUserData _userdata, long _defenderCid, long _chapterId, int _chapterLevel,boolean _isGm, NPPlayerContext _context)
    {
        _lock();
        try
        {
            TowerFloorInfo targetFloorInfo = lookFloor(_chapterId, _chapterLevel);
            if (targetFloorInfo != null)
            {
                if (targetFloorInfo.getCid() != _defenderCid && !_isGm)
                    return TowerErr.TOWER_FLOOR_HAD_BEEN_OCCUPIED;

                //查询玩家是否原来就在楼层上
                TowerFloorInfo oriFloorInfo = lookFloorByCid(_userdata.getCid());
                if (oriFloorInfo != null)
                {
                    long oriChapterId = oriFloorInfo.getChapterId();
                    int oriChapterLevel = oriFloorInfo.getChapterLevel();

                    //如果原来就在楼层上, 则直接更新
                    oriFloorInfo.chgPos(_m_server.getBM(), _chapterId, _chapterLevel);

                    //如果原来楼层和当前楼层相同, 则交换位置
                    if (oriChapterId == _chapterId)
                    {
                        targetFloorInfo.chgPos(_m_server.getBM(), oriChapterId, oriChapterLevel);
                        
                        //增加防守战报
                        _defenderReport(_defenderCid, _userdata.getCid(), _chapterId, _chapterLevel, true, oriChapterLevel);
                    }else
                    {
                        //如果原来楼层和当前楼层不同, 则删除原来的楼层
                        _m_floorList.remove(targetFloorInfo);
                        targetFloorInfo.discard(_m_server.getBM());
                        
                        //增加防守战报
                        _defenderReport(_defenderCid, _userdata.getCid(), _chapterId, _chapterLevel, true, 1);
                    }
                }

                ALSynTaskManager.getInstance().regTask(() ->
                {
                    NPUSUserData defender = _m_server.getUsUserMgr().lookupCacheUserData(_defenderCid);
                    if (defender == null)
                        return;

                    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TOWER_BEEN_DEFEATED);
                    context.setGuid(_context.getGuid());

                    defender.getTowerComponent().pvpBeenDefeated(_chapterId, _chapterLevel, context);
                });

                return  Result.SUCC;
            }else
            {
                //如果楼层未被占领, 则尝试占领
                return tryOccupyFloor(_userdata, _chapterId, _chapterLevel, _context);
            }
        } finally
        {
            _unlock();
        }
    }
    
    /**
     * 防守玩家战报
     * @param _defenderCid
     * @param _attackerCid
     * @param _chapterId
     * @param _chapterLevel
     * @param _isSucc
     * @param _curChapterLevel
     */
    private void _defenderReport(long _defenderCid, long _attackerCid, long _chapterId, int _chapterLevel, boolean _isSucc, int _curChapterLevel)
    {
    	BM bmObj = _m_server.getBM();
    	
    	PlayerTowerDefenceReportBO bo = new PlayerTowerDefenceReportBO();
    	bo.setCid(bmObj, _defenderCid);
    	bo.setAttackerCid(bmObj, _attackerCid);
    	bo.setChapterId(bmObj, _chapterId);
    	bo.setChapterLevel(bmObj, _chapterLevel);
    	bo.setIsSucc(bmObj, _isSucc);
    	bo.setCurChapterLevel(bmObj, _curChapterLevel);
    	bo.setTimestamp(bmObj, CommonFunc.getNowTimeMS());
    	bo.insert(bmObj);
    	
    	//检查玩家是否在线
    	NPUSUserData userData = _m_server.getUsUserMgr().lookupCacheUserData(_defenderCid);
    	if(null != userData)
    	{
    		userData.safeCall(()->
    		{
    			userData.getTowerComponent().getDefenceReportList().addData(bo);
    		});
    	}
    }

    /**
     * 生成挑战列表
     * @param _stageRefObj
     * @param _chapterId
     * @param _chapterLevel
     * @param _power
     * @param _callback
     */
    public void makeChallengeList(TowerStageRefObj _stageRefObj, long _chapterId, int _chapterLevel, long _power, _ICallBackResultT<List<Tower_OpponentInfo>> _callback)
    {
        calShowLevel(_stageRefObj, _chapterId, _chapterLevel, _power, new _ICallBackT<Integer>()
        {
            @Override
            public void onRunOver(Integer _offset)
            {
                if (_offset <= 0)
                {
                    _callback.onRunOver(CommErr.REF_NOT_FOUND, null);
                    return;
                }

                List<Tower_OpponentInfo> opponentList = new ArrayList<>();

                int eachOffset = (int) Math.ceil(_offset / 4d);
                for (int i = 1; i <= 4; i++)
                {
                    Tower_PosInfo targetPos = TowerUtil.getTowerStageByOffset(_stageRefObj, _chapterLevel, eachOffset * i);
                    if (targetPos == null)
                        continue;

                    TowerFloorInfo towerFloorInfo = lookFloor(targetPos.getChapterId(), targetPos.getLevel());
                    if (towerFloorInfo == null)
                    {
                        opponentList.add(new Tower_OpponentInfo(targetPos, 0));
                        continue;
                    }

                    opponentList.add(new Tower_OpponentInfo(targetPos, towerFloorInfo.getCid()));
                }

                _callback.onRunOver(Result.SUCC, opponentList);
            }
        });
    }

    /**
     * 计算展示的档位
     * @param _stageRefObj
     * @param _chapterId
     * @param _chapterLevel
     * @param _power
     * @param _callback
     */
    public void calShowLevel(TowerStageRefObj _stageRefObj, long _chapterId, int _chapterLevel, long _power, _ICallBackT<Integer> _callback)
    {
        SerialPromise promise = new SerialPromise(null);
        RefWrap<Integer> refWrap = new RefWrap<>(-1);

        List<RefTowerChapterStageShow> refList = RefTowerChapterStageShow.getMgr().getList();
        for (int i = refList.size() - 1; i >= 0; i--)
        {
            //展示配置
            RefTowerChapterStageShow refShow = refList.get(i);

            promise.then(p ->
            {
                Tower_PosInfo targetPos = TowerUtil.getTowerStageByOffset(_stageRefObj, _chapterLevel, refShow.show_lvl);
                if (targetPos == null)
                {
                    p.commit();
                    return;
                }

                //如果是PVP章节
                TowerFloorInfo towerFloorInfo = lookFloor(targetPos.getChapterId(), targetPos.getLevel());

                if (towerFloorInfo == null)
                {
                    if (_power > TowerUtil.calFloorNeedPower(targetPos.getChapterId(), targetPos.getLevel()))
                    {
                        //如果战力更高, 则使用当前档位
                        refWrap.set(refShow.show_lvl);
                        p.breakOut();
                    } else
                    {
                        //如果未被占领
                        p.commit();
                    }
                } else
                {
                    //如果已被占领
                    _m_server.getPlayerCacheGetter().getDealer(PlayerInfo_IconShow.class).getInfo(towerFloorInfo.getCid(),
                            new HandlerTwo<Boolean, PlayerInfo_IconShow>()
                            {
                                @Override
                                public void handle(Boolean _isSucc, PlayerInfo_IconShow _data)
                                {
                                    if (!_isSucc)
                                    {
                                        p.commit();
                                        return;
                                    }

                                    if (_power > _data.getTotalPower())
                                    {
                                        //如果战力更高, 则使用当前档位
                                        refWrap.set(refShow.show_lvl);
                                        p.breakOut();
                                    } else
                                    {
                                        //如果未被占领
                                        p.commit();
                                    }
                                }
                            });
                }
            });
        }

        promise.over(p ->
        {
            if (refWrap.get() <0)
            {
                RefTowerChapterStageShow refFirst = RefTowerChapterStageShow.getMgr().first();
                if (refFirst != null)
                    refWrap.set(refFirst.show_lvl);
            }

            _callback.onRunOver(refWrap.get());
        });
    }

    /**
     * 生成向前查询的列表
     * @param _startChapterId
     * @param _startLevel
     * @param _num
     * @return
     */
    public List<Tower_OpponentInfo> makeForwardList(long _startChapterId, int _startLevel, int _num)
    {
        _lock();
        try
        {
            List<Tower_OpponentInfo> forwardList = new ArrayList<>();
            for (TowerFloorInfo floorInfo : _m_floorList)
            {
                if (floorInfo.getChapterId() == _startChapterId
                        && (floorInfo.getChapterLevel() - _startLevel >= 0 && floorInfo.getChapterLevel() - _startLevel < _num))
                {
                    forwardList.add(floorInfo.makeOpponentInfo());
                }
            }
            return forwardList;
        } finally
        {
            _unlock();
        }
    }
}
