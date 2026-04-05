package NPUSServer.NPUSUserMgr.UserComp.ConsortComp;

import Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType;
import Common.ConsortObj.Consort_Info;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import GS2GC.p015_ConsortOp.GS2GC_015_074_OnConsortChatHasAddChg;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.Game.EChildBirthRes;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.PlayerAttrProperty.PlayerAttrPropertyContainer;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Enum.ELogConsortResEnum;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless.ConsortBlessSkillMgr;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness.ConsortBusinessSkillMgr;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortFetters.ConsortFettersInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortHalo.ConsortHaloInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortSkin.ConsortSkinMgr;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTriggeredCallStory.ConsortTriggeredCallStoryInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.LazyDealer.ConsortRecalCharmTask;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.LazyDealer.ConsortRecalIntimacyTask;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortBO;
import USLOGDB.Bo.LogConsortResChgBO;

/**
 * 家人数据
 * @author mj
 *
 */
public class ConsortInfo implements _IHandlerHolder
{
	//玩家数据
	private ConsortComponent _m_comp;
	
	//家人数据Bo
	private PlayerConsortBO _m_boConsort;
	
	//家人配表对象
	private RefConsort _m_refConsort;
	
	//属性数值
	private long _m_lIntimacy;//亲密度
	private long _m_lCharm;//加护力
	
	//家人羁绊数据
	private ConsortFettersInfo _m_fiFettersInfo;
	//家人经营技能管理
	private ConsortBusinessSkillMgr _m_mgrBusinessSkillMgr;
	//家人加护技能管理
	private ConsortBlessSkillMgr _m_mgrBlessSkillInfoMgr;
	//家人皮肤数据管理
	private ConsortSkinMgr _m_mgrSkinMgr;
	//家人星辉数据管理
	private ConsortHaloInfo _m_hiHaloInfo;
	//家人已触发邀约事件数据
	private ConsortTriggeredCallStoryInfo _m_siTriggeredCallStoryInfo;
	
	//对大臣属性加成数据容器（大臣属性）
    private PlayerAttrPropertyContainer _m_attrPropertyContainer;
    
    //相关的LazyDealer
    private LazyTaskDealer _m_lazyIntimacyDealer;//亲密度重新计算任务
    private LazyTaskDealer _m_lazyCharmDealer;//加护力重新计算任务
	
	public ConsortInfo(ConsortComponent _comp, PlayerConsortBO _bo, RefConsort _ref)
	{
		_m_comp = _comp;
		
		_m_boConsort = _bo;
		_m_refConsort = _ref;
		
		_m_attrPropertyContainer = new PlayerAttrPropertyContainer();
		
		//300毫秒间隔执行
		_m_lazyIntimacyDealer = new LazyTaskDealer(new ConsortRecalIntimacyTask(this), 100);
		_m_lazyCharmDealer = new LazyTaskDealer(new ConsortRecalCharmTask(this), 100);

		//管理对象
		_m_fiFettersInfo = new ConsortFettersInfo(this);
		_m_mgrBusinessSkillMgr = new ConsortBusinessSkillMgr(this);
		_m_mgrBlessSkillInfoMgr = new ConsortBlessSkillMgr(this);
		_m_mgrSkinMgr = new ConsortSkinMgr(this);
		_m_hiHaloInfo = new ConsortHaloInfo(this);
		_m_siTriggeredCallStoryInfo = new ConsortTriggeredCallStoryInfo(this);
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return _m_comp.getUSServer();}
	
	//数据对象
	public PlayerConsortBO getBo() {return _m_boConsort;}
	public long getInitIntimacy() {return _m_boConsort.getInitIntimacy();}
	public long getInitCharm() {return _m_boConsort.getInitCharm();}
	public long getCharmPoint() {return _m_boConsort.getCharmPoint();}
	public long getCurSkinId() {return _m_boConsort.getCurSkinId();}
	public long getCharmPointRecord() {return _m_boConsort.getCharmPointRecord();}
	
	//有外部加成的属性数值
	public long getIntimacy() {return _m_lIntimacy;}
    public void setIntimacy(long _intimacy) {_m_lIntimacy = _intimacy;}
    public void recalIntimacy()
    {
    	_m_lazyIntimacyDealer.setNeedDeal();
    }
	
	public long getCharm() {return _m_lCharm;}
    public void setCharm(long _charm) {_m_lCharm = _charm;}
    public void recalCharm()
    {
    	_m_lazyCharmDealer.setNeedDeal();
    }
	
	public long getCharmPointPer() {return getPlayerBonusAddValue(EBonusPropertyType.CHARMPOINT_PER);}
	public long getCharmPointExtraAdd() {return getPlayerBonusAddValue(EBonusPropertyType.CHARMPOINT);}

	//配置对象
	public RefConsort getRef() {return _m_refConsort;}
	public long getConsortId() {return _m_refConsort.id;}
	
	//管理对象
	public ConsortFettersInfo getFettersInfo() {return _m_fiFettersInfo;}
	public ConsortBusinessSkillMgr getBusinessSkillMgr() {return _m_mgrBusinessSkillMgr;}
	public ConsortBlessSkillMgr getBlessSkillInfoMgr() {return _m_mgrBlessSkillInfoMgr;}
	public ConsortSkinMgr getSkinMgr() {return _m_mgrSkinMgr;}
	public ConsortHaloInfo getHaloInfo() {return _m_hiHaloInfo;}
	public ConsortTriggeredCallStoryInfo getTriggeredCallStoryInfo() {return _m_siTriggeredCallStoryInfo;}

	//对大臣属性加成数据容器（大臣属性）
    public PlayerAttrPropertyContainer getPropertyContainer() {return _m_attrPropertyContainer;}

    /**
     * 计算家人属性
     */
    protected void _initCalConsort()
    {
    	//亲密度
    	_m_lIntimacy = ConsortCalculator.calIntimacy(this);
    	//加护力
    	_m_lCharm = ConsortCalculator.calCharm(this);
    }
    
    /**
     * 挂载属性变更处理Dealer
     */
    protected void _initPropertyChgDealer()
    {
    	_m_attrPropertyContainer.setOnPropertyChg(new PropertyChgDealer(this));
    }
    
    /**
     * 保存初始化数值
     */
    protected void _saveInitValue() 
    {
		BM bmObj = getUSServer().getBM();

		_m_boConsort.setInitIntimacy(bmObj, _m_lIntimacy);
		_m_boConsort.setInitCharm(bmObj, _m_lCharm);
		
		_m_boConsort.saveAll(bmObj);
	}
    
    /**
     * 销毁妃子时清理全局属性加成（GM命令专用）
     * 移除星辉/羁绊/经营技能注册到全局BonusMgr的加成
     */
    public void cmdDiscard()
    {
        _m_hiHaloInfo.cmdDiscardBonus();
        _m_fiFettersInfo.cmdDiscardBonus();
        _m_mgrBusinessSkillMgr.cmdDiscardBonus();
    }

    /**
     * 关联伙伴重新计算国力
     */
    public void relationHeroReCal()
    {
    	for(int i = 0; i < _m_refConsort.relation_hero_id_list.size(); i++)
    	{
    		long heroId = _m_refConsort.relation_hero_id_list.get(i);
    		HeroInfo hero = getUserData().getHeroComponent().lookupHero(heroId);
    		if(null == hero)
    			continue;
    		
    		hero.recalHero();
    	}
    }
    
    /***********
     * 获取家人有效的玩家身上的所有加成数据
     * @param _propertyType
     * @return
     */
    public long getPlayerBonusAddValue(EBonusPropertyType _propertyType)
    {
        //获取玩家身上加成
        long playerPowerAddition = getUserData().getBonusMgr().getTotalPropertyBonus(_propertyType)
                + getUserData().getBonusMgr().getFilterPropertyBonus(_propertyType, EBonusFilterType.CONSORT_ID, getConsortId());

        return playerPowerAddition;
    }
	
	/**********
	 * 家人协议
	 * @return
	 */
	public Consort_Info toProto()
	{
		Consort_Info proto = new Consort_Info();
		proto.setConsortId(getConsortId());
		proto.setIntimacy(getIntimacy());
		proto.setCharm(getCharm());
		proto.setCharmPoint(getCharmPoint());
		proto.setCurSkinId(getCurSkinId());
		//羁绊数据
		proto.setFetters(getFettersInfo().toProto());
		//加护技能
		getBlessSkillInfoMgr().makeProto(proto.getBlessSkillList());
		//皮肤数据
		getSkinMgr().makeProto(proto.getSkinList());
		//已触发事件数据
		getTriggeredCallStoryInfo().makeProto(proto.getTriggeredCallStoryIdList());
		//星辉数据
		proto.setIsUnlockHalo(getHaloInfo().isUnlock());
		proto.setHalo(getHaloInfo().toProto());
		//经营技能
		proto.setOpBusinessSkillCount(getBusinessSkillMgr().calOpSkillCount());
		getBusinessSkillMgr().makePropertySumProto(proto.getBusinessSkillPropertySum());
		
		return proto;
	}
	
	/**********
	 * 修改亲密度
     * @param _intimacy
     */
	public void updateIntimacy(long _intimacy, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//记录变更前的值
			int oriIntimacy = (int)_m_boConsort.getIntimacy();

			//更新数据
			BM bmObj = getUSServer().getBM();

			_m_boConsort.saveIntimacy(bmObj, _intimacy);

			//重新计算总亲密度
			recalIntimacy();

			//记录MJ日志
			if (oriIntimacy != (int)_intimacy)
			{
				MJEventLog.logConsortAttribute(
					getUserData(),
					getConsortId(),
					1, // 培养类型：1=亲密度
					_context.getContextId(),
					oriIntimacy,
					(int)_intimacy
				);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***************
	 * 增加自身亲密度数据
	 * @param _intimacy
	 */
	public void incrIntimacy(long _intimacy, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long curlIntimacy = getBo().getIntimacy() + _intimacy;
			updateIntimacy(curlIntimacy, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**********
	 * 修改自身加护力数据
     * @param _charm
     */
	public void updateCharm(long _charm, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			//记录变更前的值
			int oriCharm = (int)_m_boConsort.getCharm();

			//更新数据
			BM bmObj = getUSServer().getBM();

			_m_boConsort.saveCharm(bmObj, _charm);

			//重新计算总加护力
			recalCharm();

			//记录MJ日志
			if (oriCharm != (int)_charm)
			{
				MJEventLog.logConsortAttribute(
					getUserData(),
					getConsortId(),
					3, // 培养类型：3=加护力
					_context.getContextId(),
					oriCharm,
					(int)_charm
				);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***************
	 * 只增加自身加护力数据
	 */
	public void incrCharm(long _charm, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long curCharm = getBo().getCharm() + _charm;
			updateCharm(curCharm, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**********
	 * 修改加护点
     * @param _charmPoint
     */
	public void updateCharmPoint(long _charmPoint, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long preValue = getCharmPoint();
			
			//更新加护点历史记录
			long newcharmPointRecord = getCharmPointRecord();
			if(_charmPoint > newcharmPointRecord)
			{
				newcharmPointRecord = _charmPoint;
			}
			
			//更新数据
			BM bmObj = getUSServer().getBM();
			
			_m_boConsort.setCharmPoint(bmObj, _charmPoint);
			_m_boConsort.setCharmPointRecord(bmObj, newcharmPointRecord);
			_m_boConsort.saveAllMarked(bmObj);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_053_OnConsortCharmPointChg(this));

            //日志
            ConsortInfo.logResChg(this, ELogConsortResEnum.CHARM_POINT, preValue, _charmPoint);

			//记录MJ日志
			if (preValue != _charmPoint)
			{
				MJEventLog.logConsortAttribute(
					getUserData(),
					getConsortId(),
					2, // 培养类型：2=吸引力
					_context.getContextId(),
					(int)preValue,
					(int)_charmPoint
				);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/***************
	 * 增加魅力值
	 */
	public void incrCharmPoint(long _charmPoint, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long curCharmPoint = getCharmPoint() + _charmPoint;
			updateCharmPoint(curCharmPoint, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/****
	 * 扣除加护点
	 * @param _charmPoint
	 * @param _context
	 */
	public void descCharmPoint(long _charmPoint, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long curCharmPoint = getCharmPoint() - _charmPoint;
			updateCharmPoint(curCharmPoint, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
     * 增加家人资源类型获取
     * @param _type
     * @param _value
     * @param _context
     */
	public void incrConsortRes(EBagItemUse_ConsortDrawShowType _type, long _value, NPPlayerContext _context)
	{
		if(EBagItemUse_ConsortDrawShowType.CHARM == _type)
		{
			incrCharm(_value, _context);
		}
		else if(EBagItemUse_ConsortDrawShowType.INTIMACY == _type)
		{
			incrIntimacy(_value, _context);
		}
		else if(EBagItemUse_ConsortDrawShowType.CHARM_POINT == _type)
		{
			incrCharmPoint(_value, _context);
		}
		else
		{
			USLog.error(getUSServer(), "player:{} incr consort:{} res:{} value:{} fail.", getUserData().getCid(), getConsortId(), _type, _value);
		}
	}
	
	/**
	 * 获取家人资源数量
	 * @param _type
	 * @return
	 */
	public long getConsortRes(EBagItemUse_ConsortDrawShowType _type)
	{
		if(EBagItemUse_ConsortDrawShowType.INTIMACY == _type)
		{
			return getIntimacy();
		}
		else if(EBagItemUse_ConsortDrawShowType.CHARM == _type)
		{
			return getCharm();
		}
		else if(EBagItemUse_ConsortDrawShowType.CHARM_POINT == _type)
		{
			return getCharmPoint();
		}
		
		return 0;
	}
	
	/**
	 * 设置当前皮肤
	 * @param _skinId
	 * @param _context
	 */
	public void setCurSkin(long _skinId, NPPlayerContext _context)
	{
		BM bmObj = getUSServer().getBM();
		_m_boConsort.saveCurSkinId(bmObj, _skinId);
		
		//推送数据
		getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_056_OnCurSkinChg(this));
	}
	
	/**
	 * 妃子资源变化日志
	 * 
	 * @param _consort
	 * @param _resType
	 * @param _preValue
	 * @param _newValue
	 */
	public static void logResChg(ConsortInfo _consort, ELogConsortResEnum _resType, long _preValue, long _newValue)
	{		
		BM bmObj = _consort.getUSServer().getBM();
		
		LogConsortResChgBO logBo = new LogConsortResChgBO();
		logBo.setCid(bmObj, _consort.getUserData().getCid());
		logBo.setConsortId(bmObj, _consort.getConsortId());
		logBo.setResType(bmObj, _resType.ordinal());
		logBo.setPreValue(bmObj, _preValue);
		logBo.setNewValue(bmObj, _newValue);
		CommLogDB.log(bmObj, logBo);
	}

	/**
	 * 是否添加聊天好友
	 * @return
	 */
	public boolean getHasAddChatFriend()
	{
		return _m_boConsort.getHasAddChatFriend();
	}

	/**
	 * 是否添加聊天好友
	 * @return
	 */
	public void markHasAddChatFriend()
	{
		_m_boConsort.saveHasAddChatFriend(getUSServer().getBM(), true);

		getUserData().sendMsgToGC(new GS2GC_015_074_OnConsortChatHasAddChg(getConsortId(), true));
	}

    /**
     * 检查在随机邀约中是否获得子嗣
     * @param _hasCg
     * @param _context
     * @return
     */
    public ChildInfo checkRandCallBirthChild(boolean _hasCg, NPPlayerContext _context)
    {
        //GOB-9270 【优化-0】情人随机约会-概率获得学徒增加前置条件 https://www.teambition.com/task/69a5360031a5fbf86ef1fab1
        if(!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().consort_call_gain_child_simple_unlock_id, getUserData(), null))
            return null;

        //检查是否存在未使用的子嗣位
        boolean hasUnusingSeat = getUserData().getChildComponent().getChildMgr().hasUnusingSeat();
        if(!hasUnusingSeat)
            return null;

        int curRecordCount = (int) getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.CONSORT_RND_CALL_TIMES);
        if (curRecordCount == 0) //首次邀约
        {
            return null;
        }
        else if(curRecordCount == 1) //第二次邀约，指定普通子嗣
        {
            return ChildSystem.createChild(this, false, null, _context);
        }
        else //其他情况
        {
            //先检查是否必定卷王子嗣
            int giftedCount = (int) getUserData().getParam(ENPPlayerParam.BIRTH_GIFTDE_COUM);
            if (giftedCount > 0) //如果存在必定卷王子嗣，则无需权重随机
            {
                giftedCount--;
                getUserData().setParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, giftedCount);

                return ChildSystem.createChild(this, true, null, _context);
            }
            else if(!_hasCg)
            {
                EChildBirthRes birthType = RefGeneral.Ref().consortCallRandPerWeight.random();
                if(birthType != EChildBirthRes.NO_CHILD)
                {
                    return ChildSystem.createChild(this, EChildBirthRes.GIFTDE_CHILD == birthType, null, _context);
                }
            }
        }

        return null;
    }
}
