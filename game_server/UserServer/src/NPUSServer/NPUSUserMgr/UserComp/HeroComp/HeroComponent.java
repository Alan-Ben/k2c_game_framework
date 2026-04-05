package NPUSServer.NPUSUserMgr.UserComp.HeroComp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CommonEnum.ESpecAttrType;
import GS2GC.p002_InitOp.GS2GC_002_012_RetHeroInit;
import GS2GC.p013_HeroOp.GS2GC_013_055_OnGainHero;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.*;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.Hero.RefHeroSkin;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Cache.Hero.PlayerHeroCacheFunc;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MAX_POWER_CHG;
import NPUSServer.DinnerMgr.DinnerCheckNpcTask;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr.HeroSuitsMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.*;
import USLOGDB.Bo.LogHeroBO;
import USLOGDB.Bo.LogSectionHeroV2BO;

import java.util.ArrayList;
import java.util.List;

public class HeroComponent extends _ANPUserComponent implements _IUserItemBasicDealer, _IHandlerHolder
{
    //大臣套系的数据管理对象，一般用于管理套系的技能
    private HeroSuitsMgr _m_smSuitsMgr;

    //大臣数据
    private List<HeroInfo> _m_heroList;

    //大臣皮肤的获取处理对象
    private HeroSkinItemDealer _m_skinItemDealer;
    //总实力
    private long _m_totalPower;

    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;

    public HeroComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.HERO);

        _m_smSuitsMgr = new HeroSuitsMgr(this);
        _m_skinItemDealer = new HeroSkinItemDealer(this);
        _m_heroList = new ArrayList<>();
        
        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
    }

    public _IUserItemBasicDealer getSkinItemDealer()
    {
        return _m_skinItemDealer;
    }

    public HeroSuitsMgr getSuitMgr()
    {
        return _m_smSuitsMgr;
    }

    public NPPlayerPropertyContainer getPlayerPropertyContainer() 
    {
    	return _m_pcPlayerPropertyContainer;
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
        final ALProcess process = ALProcess.CreateProcess("hero_comp_init");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: 大臣数据加载
        process.addResDelegateProcess(action -> _initHeroFromDB(action::dealAction), "init_hero",
                () -> USLog.error(getUSServer(), "player:{} load hero bo fail.", getUserData().getCid()), false);
        //步骤2: 大臣技能数据加载
        process.addResDelegateProcess(action -> _initSkillFromDB(action::dealAction), "hero_skill_init",
                () -> USLog.error(getUSServer(), "player:{} load hero skill bo fail.", getUserData().getCid()), false);
        //步骤3: 大臣皮肤数据加载
        process.addResDelegateProcess(action -> _initSkinFromDB(action::dealAction), "hero_skin_init",
                () -> USLog.error(getUSServer(), "player:{} load hero skin bo fail.", getUserData().getCid()), false);
        //步骤4: 大臣资质技能数据加载
        process.addResDelegateProcess(action -> _initTalentSkillFromDB(action::dealAction), "hero_talent_skill_init",
                () -> USLog.error(getUSServer(), "player:{} load hero talent skill bo fail.", getUserData().getCid()), false);
        //步骤5: 大臣光环数据加载
        process.addResDelegateProcess(action -> _initHaloFromDB(action::dealAction), "hero_halo_init",
                () -> USLog.error(getUSServer(), "player:{} load hero halo bo fail.", getUserData().getCid()), false);
        //步骤6: 大臣属性计算
        process.addResDelegateProcess(action -> _initFinalCalProperty(action::dealAction), "hero_attr_property_init",
                () -> USLog.error(getUSServer(), "player:{} load hero attr property bo fail.", getUserData().getCid()), false);

        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    //步骤1:大臣数据加载
    private void _initHeroFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerHeroBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerHeroBO>>()
        {
            @Override
            public void dealFail()
            {
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerHeroBO> _list)
            {
                for (PlayerHeroBO heroBO : _list)
                {
                    RefHero refHero = RefHero.getMgr().get(heroBO.getHeroId());
                    if (refHero == null)
                    {
                        USLog.error(getUSServer(), "HeroComponent _initFromDB refHero not found cid:{} heroId:{}",
                                getUserData().getCid(), heroBO.getHeroId());
                        continue;
                    }

                    HeroInfo heroInfo = new HeroInfo(HeroComponent.this, refHero, heroBO, true);
                    _addHeroToList(heroInfo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //步骤2:大臣技能数据加载
    private void _initSkillFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerHeroBusinessSkillBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerHeroBusinessSkillBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load hero skill bo fail.", getUserData().getCid());

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerHeroBusinessSkillBO> _list)
            {
                for (PlayerHeroBusinessSkillBO skillBO : _list)
                {
                    HeroInfo heroInfo = lookupHero(skillBO.getHeroId());
                    if (heroInfo == null)
                    {
                        USLog.error(getUSServer(), "HeroComponent _initSkillFromDB heroInfo not found cid:{} heroId:{} skillId:{}",
                                getUserData().getCid(), skillBO.getHeroId(), skillBO.getSkillId());
                        continue;
                    }
                    heroInfo.getBusinessSkillMgr().initSkill(skillBO);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //步骤3:大臣皮肤数据加载
    private void _initSkinFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerHeroSkinBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerHeroSkinBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load hero skin bo fail.", getUserData().getCid());

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerHeroSkinBO> _list)
            {
                for (PlayerHeroSkinBO skinBo : _list)
                {
                    RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(skinBo.getSkinId());
                    if (refHeroSkin == null)
                    {
                        USLog.error(getUSServer(), "HeroComponent _initSkinFromDB refHeroSkin not found cid:{} skinId:{}",
                                getUserData().getCid(), skinBo.getSkinId());
                        continue;
                    }
                    HeroInfo heroInfo = lookupHero(refHeroSkin.hero_id);
                    if (heroInfo == null)
                    {
                        USLog.error(getUSServer(), "HeroComponent _initSkinFromDB heroInfo not found cid:{} heroId:{} skinId:{}",
                                getUserData().getCid(), refHeroSkin.hero_id, skinBo.getSkinId());
                        continue;
                    }
                    heroInfo.getSkinMgr().initSkin(skinBo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //步骤4:大臣资质技能数据加载
    //alzq : 大臣皮肤会增加资质技能，所以资质技能处理要在皮肤之后
    private void _initTalentSkillFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerHeroTalentSkillBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerHeroTalentSkillBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load hero talent skill bo fail.", getUserData().getCid());

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerHeroTalentSkillBO> _list)
            {
                for (HeroInfo heroInfo : _m_heroList)
                {
                    heroInfo.getTalentSkillMgr().initCheckSkinSKill();
                }

                //逐个技能Bo放入数据
                for (PlayerHeroTalentSkillBO skillBO : _list)
                {
                    HeroInfo heroInfo = lookupHero(skillBO.getHeroId());
                    if (heroInfo == null)
                    {
                        USLog.error(getUSServer(), "HeroComponent _initTalentSkillFromDB heroInfo not found cid:{} heroId:{} skillId:{}",
                                getUserData().getCid(), skillBO.getHeroId(), skillBO.getTalentSkillId());
                        continue;
                    }

                    heroInfo.getTalentSkillMgr().initTalentSkill(skillBO);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //步骤5:大臣光环数据加载
    private void _initHaloFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerHeroHaloBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerHeroHaloBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "player:{} load hero halo bo fail.", getUserData().getCid());

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerHeroHaloBO> _list)
            {
                for (PlayerHeroHaloBO haloBo : _list)
                {
                    HeroInfo heroInfo = lookupHero(haloBo.getHeroId());
                    if (heroInfo == null)
                    {
                        USLog.error(getUSServer()
                                , "HeroComponent _initHaloFromDB heroInfo not found cid:{} heroId:{} haloId:{}"
                                , getUserData().getCid(), haloBo.getHeroId(), haloBo.getId());
                        continue;
                    }

                    if (heroInfo.getHaloInfo() == null)
                    {
                        USLog.error(getUSServer()
                                , "HeroComponent _initHaloFromDB heroInfo haloInfo is null cid:{} heroId:{} haloId:{}"
                                , getUserData().getCid(), haloBo.getHeroId(), haloBo.getId());
                        continue;
                    }

                    heroInfo.getHaloInfo()._initBo(haloBo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //步骤6:大臣属性计算
    private void _initFinalCalProperty(_ICallBackBool _handler)
    {
        //每个大臣遍历进行最后的处理
        for (HeroInfo heroInfo : _m_heroList)
        {
            if (heroInfo.getBuildingInfo() == null)
                continue;

            //放置大臣到对应的建筑上
            heroInfo.getBuildingInfo().placeHero(heroInfo);
            //后续的属性加成处理
            heroInfo._onPlaceToBuilding();
        }

        _handler.onRunOver(true);
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
    {
        return new ENPPlayerCompType[]{ENPPlayerCompType.BUILDING};
    }

    @Override
    public void onInited()
    {
        //初始化每个套件的变化监听
        getSuitMgr()._initSuitPropertyChgDealer();

    }

    @Override
    public void dispose()
    {

    }

    /**
     * 替换实力
     */
    public void replaceHeroPower(long _prePower, long _newPower, boolean _isInit)
    {
        getUserData().lockUser();
        try{
            _m_totalPower -= _prePower;
            _m_totalPower += _newPower;

            if (!_isInit)
            {
                //记录实力
                PlayerCacheFunc.updateTotalPower(getUserData(), _m_totalPower);


                //记录玩家最大战力
                if (_m_totalPower > getUserData().getPlayerComponent().getParamV(ENPPlayerParam.POWER_MAX_RECORD))
                {
                    long addValue = _m_totalPower - getUserData().getPlayerComponent().getParamV(ENPPlayerParam.POWER_MAX_RECORD);

                    getUserData().getPlayerComponent().setParam(ENPPlayerParam.POWER_MAX_RECORD, _m_totalPower);

                    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MAX_POWER_CHG);
                    getUserData().onLogicEvent(new Event_P_MAX_POWER_CHG(context, _m_totalPower, addValue));

                    //记录历史最高实力
                    PlayerCacheFunc.updateMaxPower(getUserData(), _m_totalPower);
                }
            }
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取总实力
     * @return 总实力
     */
    public long getTotalPower()
    {
        getUserData().lockUser();
        try{
            return _m_totalPower;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 返回大臣数量
     * @return 大臣数量
     */
    public int getHeroNum()
    {
    	_lock();
    	
        try
    	{
        	return _m_heroList.size();
    	}
        finally
        {
        	_unlock();
        }
    }
    /**
     * 获取所有大臣ID列表
     * @return
     */
    public ArrayList<Long> getAllHeroIdList()
    {
        _lock();
        
        try
        {
        	ArrayList<Long> heroIdList = new ArrayList<>();
        	for(int i = 0; i < _m_heroList.size(); i++)
        	{
        		HeroInfo hero = _m_heroList.get(i);
        		if(null == hero)
        			continue;
        		
        		heroIdList.add(hero.getHeroId());
        	}
        	
            return heroIdList;
        } 
        finally
        {
            _unlock();
        }
    }
    
    public ArrayList<HeroInfo> getAllHeroList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_heroList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据大臣id查找大臣
     * @param _heroId
     * @return
     */
    public HeroInfo lookupHero(long _heroId)
    {
        _lock();
        try
        {
            for (HeroInfo heroInfo : _m_heroList)
            {
                if (heroInfo.getHeroId() == _heroId)
                    return heroInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 返回包含指定特长类型的大臣列表
     * @param _attrType 属性类型
     * @return
     */
    public List<HeroInfo> getAttrHeroList(ESpecAttrType _attrType)
    {
        _lock();
        try
        {
            List<HeroInfo> list = new ArrayList<>();
            for (HeroInfo heroInfo : _m_heroList)
            {
                if (heroInfo.getRef().spec_attr_type == _attrType)
                {
                    list.add(heroInfo);
                }
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /************
     * 对所有大臣进行重新计算处理
     */
    public void recalAllHero()
    {
        _lock();
        try
        {
            for (HeroInfo heroInfo : _m_heroList)
            {
                //调用计算处理
                heroInfo.recalHero();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 初始化计算所有大臣
     */
    public void _initCalAllHero()
    {
        _lock();
        try
        {
            for (HeroInfo heroInfo : _m_heroList)
            {
                //调用计算处理
                heroInfo._initCalHero();
            }
        } finally
        {
            _unlock();
        }
    }

    public void recalAllSuitHero(long _suitId)
    {
        _lock();
        try
        {
            for (HeroInfo heroInfo : _m_heroList)
            {
                //匹配套件则调用计算处理
                if (heroInfo.getRef().suit_id == _suitId)
                    heroInfo.recalHero();
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 获得大臣
     * @param _heroId  大臣id
     * @param _context 上下文
     * @return 大臣对象
     */
    protected Result _gainHero(long _heroId, NPPlayerContext _context)
    {
        RefHero refHero = RefHero.getMgr().get(_heroId);
        if (refHero == null)
            return CommErr.REF_NOT_FOUND;

        if (lookupHero(_heroId)!= null)
            return HeroErr.HERO_ALREADY_EXISTED;

        HeroInfo heroInfo = null;

        _lock();
        try
        {
            if (lookupHero(_heroId) != null)
                return HeroErr.HERO_ALREADY_EXISTED;

            BM bmObj = getUSServer().getBM();

            PlayerHeroBO bo = new PlayerHeroBO();
            bo.setCid(bmObj, getUserData().getCid());
            bo.setHeroId(bmObj, _heroId);
            bo.setLevel(bmObj, 1);
            bo.setSkinId(bmObj, refHero.default_skin_id);
            bo.setStep(bmObj, 1);
            bo.insert(bmObj);

            //构造大臣数据
            heroInfo = new HeroInfo(this, refHero, bo, false);
            _addHeroToList(heroInfo);

            //调用初始化大臣处理
            heroInfo.initNewHero(_context);

            //推送消息
            GS2GC_013_055_OnGainHero pushProto = new GS2GC_013_055_OnGainHero();
            pushProto.setHeroInfo(heroInfo.toProto());
            getUserData().sendMsgToGC(pushProto);

            //发起一次计算
            heroInfo.recalHero();
            
            //宴会数据
            ALSynTaskManager.getInstance().regTask(new DinnerCheckNpcTask(getUserData()));

            if (refHero.quality == EQuality.RED && _context.getContextId() != ENPGameEvent.GM_CMD.ordinal())
            {
                ArrayList<String> paramList = new ArrayList<>();
                paramList.add(getUserData().getPlayerComponent().getName());
                paramList.add(refHero.name);

                getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().marquee_gain_ur_hero_marquee_id, paramList);
            }
        } finally
        {
            _unlock();
        }

        // 【GOB-5210】玩家拥有指定数量的大臣，解锁竞技场
        getUserData().getArenaComponent().checkUnlockArena(_context);

        //获取额外奖励，不做表现
        NPPlayerContext newContext = NPPlayerContext.createNew(_context);
        getUserData().gainItemList(refHero.unlock_gain_item_list, newContext);

        //放入数据
        _context.collectItem(getItemType(), _heroId, 1, false);

        //日志
        logItem(_heroId, 0, 1, ELogItem_Type.GAIN, _context);

        //更新到缓存
        PlayerHeroCacheFunc.onGainHero(getUserData(), heroInfo);

        if (_context.getContextId() == ENPGameEvent.RECRUIT.ordinal())
        {
            ArrayList<String> paramList = new ArrayList<>();
            paramList.add(getUserData().getPlayerComponent().getName());
            paramList.add(refHero.name);
            getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().recruit_gain_hero_or_consort_marquee_id, paramList);
        }

        return Result.SUCC;
    }

    private void _addHeroToList(HeroInfo _heroInfo)
    {
        _lock();
        try
        {
            _m_heroList.add(_heroInfo);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造初始化协议
     * @param _proto 协议
     */
    public void makeInitProto(GS2GC_002_012_RetHeroInit _proto)
    {
        _lock();
        try
        {
            for (HeroInfo heroInfo : _m_heroList)
            {
                _proto.getHeroList().add(heroInfo.toProto());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 返回指定等级内的大臣数量
     * @param _lvlRng
     * @return
     */
    public long getHeroNumInLvlRng(WCGIntRange _lvlRng)
    {
        _lock();
        try
        {
            int count = 0;
            for (HeroInfo heroInfo : _m_heroList)
            {
                if (_lvlRng.inRange(heroInfo.getLevel()))
                {
                    count++;
                }
            }
            return count;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.HERO;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        return lookupHero(_itemId) == null ? 0 : 1;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return lookupHero(_itemId) != null;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        Result result = _gainHero(_itemId, _context);
        if (!result.isSucc())
        {
            USLog.error(getUSServer(), "HeroComponent _gainHero fail, cid:{} heroId:{} result:{}",
                    getUserData().getCid(), _itemId, result.toString());
        }
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        Result result = _gainHero(_itemId, _context);
        if (!result.isSucc())
        {
            USLog.error(getUSServer(), "HeroComponent _gainHero fail, cid:{} heroId:{} result:{}",
                    getUserData().getCid(), _itemId, result.toString());
        }
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, NPCommonEnum.ELogItem_Type _logType, _IContext _context)
    {
        LogHeroBO bo = new LogHeroBO();
        bo.setCid(getUSServer().getBM(), getUserData().getCid());
        bo.setHeroId(getUSServer().getBM(), _subId);
        CommLogDB.log(getUSServer().getBM(), bo, _context);
    }

    /**
     * 作弊命令删除大臣
     * @param _heroId 大臣id
     * @return 是否成功
     */
    public boolean cmdRemoveHero(long _heroId)
    {
        _lock();
        try
        {
            //查找对应大臣
            HeroInfo heroInfo = lookupHero(_heroId);
            if (heroInfo == null)
                return false;

            //删除大臣
            _m_heroList.remove(heroInfo);

            //删除相关数据
            heroInfo.dispose();

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取达到指定星级的大臣数量
     * @param _starNum
     * @return
     */
    public int getFitStarHeroNum(int _starNum)
    {
        _lock();
        try
        {
            int num = 0;
            for (HeroInfo heroInfo : _m_heroList)
            {
                if (heroInfo.getStar() >= _starNum)
                    num++;
            }
            return num;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取大臣总等级
     * @return
     */
    public int getTotalHeroLevel()
    {
        _lock();
        try
        {
            int totalLevel = 0;
            for (HeroInfo heroInfo : _m_heroList)
            {
                totalLevel += heroInfo.getLevel();
            }
            return totalLevel;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取大臣在建筑中的数量
     * @return
     */
    public long getHeroInBuildingNum()
    {
        _lock();
        try
        {
            long num = 0;
            for (HeroInfo heroInfo : _m_heroList)
            {
                if (heroInfo.getBuildingInfo() == null)
                    continue;

                num++;
            }
            return num;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取大臣总资质
     * @return
     */
    public long getTotalTalent()
    {
        _lock();
        try
        {
            long totalTalent = 0;
            for (HeroInfo heroInfo : _m_heroList)
            {
                totalTalent += HeroCalculator.calTalent(heroInfo, null);
            }
            return totalTalent;
        } finally
        {
            _unlock();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (HeroInfo heroInfo : _m_heroList)
        {
            sb.append("heroId: ").append(heroInfo.getHeroId()).append(", ")
                    .append("power: ").append(heroInfo.getPower()).append("\n");
        }
        sb.append("totalPower: ").append(getTotalPower());
        sb.append("\ntotalTalent: ").append(getTotalTalent());
        return sb.toString();
    }

	/**
	 * 伙伴截面数据
	 * 
	 * @param _sectionLogType
	 */
	public void logSection(ELogSectionType _sectionLogType)
    {
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			for(int i = 0; i < _m_heroList.size(); i++)
			{
				HeroInfo info = _m_heroList.get(i);
				if(null == info)
					continue;
				
				//获取玩家对应的藏品
				EquipInfo equip = getUserData().getEquipComponent().lookupEquipByHeroId(info.getHeroId());
				
				//日志数据
				LogSectionHeroV2BO logBo = new LogSectionHeroV2BO();
		        //截面日志类型
		        logBo.setSectionType(getUSServer().getBM(), _sectionLogType.ordinal());
		        //玩家
		        logBo.setCid(bmObj, getUserData().getCid());
		        logBo.setHeroId(bmObj, info.getHeroId());
		        logBo.setHeroLvl(bmObj, info.getLevel());
		        logBo.setHeroTalent(bmObj, HeroCalculator.calTalent(info, null));
		        logBo.setHeroPower(bmObj, info.getPower());
		        logBo.setHeroBusinessSkillLvl(bmObj, info.getBusinessSkillMgr()._sectionLog());
		        //藏品
		        if(null != equip)
		        {
		        	logBo.setEquipID(bmObj, equip.getEquipId());
		        	logBo.setEquipLvl(bmObj, equip.getLevel());
		        }
		        
		        CommLogDB.log(bmObj, logBo);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
    }
}
