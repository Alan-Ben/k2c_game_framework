package NPUSServer.NPUSUserMgr.UserComp.PrivilegeCardComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.MailObj.Mail_Data;
import Common.PrivilegeCardEnum.EPrivilegeCardType;
import Common.PrivilegeCardObj.PrivilegeCardObj_Info;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.PrivilegeCard.RefPrivilegeCard;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Gold;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerPrivilegeCardBO;

import java.util.ArrayList;

public class PrivilegeCardInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//权益卡类型
	private EPrivilegeCardType _m_eCardType;
	
	//配置数据
	private RefPrivilegeCard _m_ref;
	
	//数据实例ID
	private long _m_lId;
	//生效开始时间（秒）
	private long _m_lStartS;
	//生效结束时间（秒）
	private long _m_lEndS;
	//最后一次领取每日奖励的时间戳（秒）
	private long _m_lLastGainDailyRewardS;
	
	//当前生效标志
	private boolean _m_bEffective;
	
	public PrivilegeCardInfo(NPUSUserData _userData, EPrivilegeCardType _cardType)
	{
		_m_udUserData = _userData;
		
		_m_eCardType = _cardType;
		
		_m_ref = RefPrivilegeCard.getMgr().get(_m_eCardType.ordinal());
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public EPrivilegeCardType getCardType() {return _m_eCardType;}
    
    public RefPrivilegeCard getRef() {return _m_ref;}
    
    public long getId() {return _m_lId;}
    public long getStartS() {return _m_lStartS;}
    public long getEndS() {return _m_lEndS;}
    public long getLastGainDailyRewardS() {return _m_lLastGainDailyRewardS;}
    
    protected void _loadBo(PlayerPrivilegeCardBO _bo) 
    {
    	_m_lId = _bo.getId();
    	_m_lStartS = _bo.getStartS();
    	_m_lEndS = _bo.getEndS();
    	_m_lLastGainDailyRewardS = _bo.getLastGainDailyRewardS();
	}
    
    private void _save()
    {
    	if(0 == _m_lId)
    	{
    		PlayerPrivilegeCardBO bo = new PlayerPrivilegeCardBO();
    		bo.setCid(getBM(), getCid());
    		bo.setCardType(getBM(), _m_eCardType.ordinal());
    		bo.setStartS(getBM(), _m_lStartS);
    		bo.setEndS(getBM(), _m_lEndS);
    		bo.setLastGainDailyRewardS(getBM(), _m_lLastGainDailyRewardS);
    		bo.insert(getBM());
    		
    		_m_lId = bo.getId();
    	}
    	else
    	{
    		ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("startS", _m_lStartS);
            updateValue.addValueObj("endS", _m_lEndS);
            updateValue.addValueObj("lastGainDailyRewardS", _m_lLastGainDailyRewardS);
            
            getBM().getBM(PlayerPrivilegeCardBO.class).update("id", _m_lId, updateValue);
    	}
    }
    
    /**
     * 初始化数据
     */
    protected void _onInited() 
    {
    	//检查是否生效状态，失效状态检查通过tick确认
    	checkEffective(false);
    }
    
    /**
     * 自动结算部分（不超过今天0点）
     * @param _context
     * @param _sb
     */
    public void autoSettle(NPPlayerContext _context, StringBuilder _sb)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//数据配置不存在
    		if(null == _m_ref)
    		{
    			if(null != _sb)
    			{
    				_sb.append("\nauto settle fail, not find ref.");
    			}
    			return;
    		}
    		
    		//权益卡从未生效
    		if(_m_lStartS <= 0)
    		{
    			if(null != _sb)
    			{
    				_sb.append("\nauto settle fail, not start.");
    			}
    			return;
    		}
    		
    		//如果昨天已经领取，无需再次处理
    		int lastDayZeroS = CommonFunc.getZeroClockSecByOffsetDays(1);
    		if(_m_lLastGainDailyRewardS >= lastDayZeroS)
    		{
    			if(null != _sb)
    			{
    				_sb.append("\nauto settle fail, last gain ts:").append(CommonFunc.getTimeString((int) _m_lLastGainDailyRewardS));
    			}
    			return;
    		}
    		
    		//计算开始时间
    		long settleStartS = Math.max(_m_lLastGainDailyRewardS, _m_lStartS);
    		//截至时间不能超过今天0点（当天允许玩家手动领取）
    		long lastDayLimitS =  lastDayZeroS + 86400 - 1;
    		long settleEndS = Math.min(_m_lEndS, lastDayLimitS);
    		//更新最后一次领取时间
    		long preLastGainTs = _m_lLastGainDailyRewardS;
    		_m_lLastGainDailyRewardS = lastDayLimitS;
    		_save();
    		
    		//计算经过的天数，补充上购卡当天的奖励
    		int diffDays = CommonFunc.getDayDiff(settleStartS * 1000, settleEndS * 1000) + 1;
    		if(diffDays <= 0)
    		{
    			if(null != _sb)
    			{
    				_sb.append("\nauto settle fail, diff days:0, startTs:").append(CommonFunc.getTimeString((int) settleStartS))
    					.append(", endTs:").append(CommonFunc.getTimeString((int) settleEndS))
    					.append(", lastGainTs:").append(CommonFunc.getTimeString((int) lastDayLimitS));
    			}
    			return;
    		}
    		
    		//计算附件
    		ArrayList<NPCommonCostItem> itemList = new ArrayList<>();
    		ArrayList<NPCommonCostItem> dailyGainItemList = _m_ref.daily_gain_item_list;
    		for(int i = 0; i < dailyGainItemList.size(); i++)
    		{
    			NPCommonCostItem item = dailyGainItemList.get(i);
    			if(null == item)
    				continue;
    			
    			NPCommonCostItem gainItem = item.duplicate();
    			gainItem.setCount(item.getCount() * diffDays);
    			itemList.add(gainItem);
    		}
			//领取金币奖励
    		long earns = 0;
			SpecialItemDealer_Gold goldDealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
			if(null != goldDealer)
			{
				earns = goldDealer.getEarnings();
				if(earns > 0)
				{
					itemList.add(new NPCommonCostItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), earns * diffDays));
				}
			}
    		
    		//发送邮件
    		Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(_m_ref.mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(itemList));
            MailSystem.addMail(getUSServer(), getUserData().getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE));
    	
			if(null != _sb)
			{
				_sb.append("\nauto settle suc, diff days:").append(diffDays)
					.append(", startTs:").append(CommonFunc.getTimeString((int) settleStartS))
					.append(", endTs:").append(CommonFunc.getTimeString((int) settleEndS))
					.append(", preLastGainTs:").append(CommonFunc.getTimeString((int) preLastGainTs))
					.append(", lastGainTs:").append(CommonFunc.getTimeString((int) lastDayLimitS))
					.append(", earns:").append(earns);
			}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 是否生效
     * @return
     */
    public boolean isEffect()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return null != _m_ref && _m_lEndS > CommonFunc.getNowTimeSec();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 检查权益卡是否生效
     * @param _push
     */
    public void checkEffective(boolean _push)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//已标记生效状态不处理
    		if(_m_bEffective)
    			return;
    		
    		//未生效状态不处理
    		if(!isEffect())
    			return;
    		
    		//更新标记位
    		_m_bEffective = true;
			//推送数据
			if(_push)
			{
				getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_054_OnPrivilegeCardChg(this));
			}

			//增加玩家加成/权限
			if(null != _m_ref)
			{
	    		//增加玩家属性部分
				getUserData().getPrivilegeCardComponent().getPlayerPropertyContainer().addModifier(_m_ref.player_pro);
				//增加玩家bonus属性部分
				getUserData().getBonusMgr().addTotalModifier(_m_ref.bonus_prop_modifier);
				//更新玩家的权限
				getUserData().getPlayerPermissionsComponent().addPermissionList(_m_ref.player_permission_list, _push);
			}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 检查权益卡是否失效
     */
    public void checkInvalid()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//未标记生效状态不处理
    		if(!_m_bEffective)
    			return;
    		
    		//已生效状态不处理
    		if(isEffect())
    			return;
    		
    		//更新标记位
    		_m_bEffective = false;
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_054_OnPrivilegeCardChg(this));
			
			//移除玩家加成/权限
			if(null != _m_ref)
			{
	    		//移除玩家属性
				getUserData().getPrivilegeCardComponent().getPlayerPropertyContainer().removeModifier(_m_ref.player_pro);
				//增加玩家bonus属性部分
				getUserData().getBonusMgr().removeTotalModifier(_m_ref.bonus_prop_modifier);
				//更新玩家的权限
				getUserData().getPlayerPermissionsComponent().reduPermissionList(_m_ref.player_permission_list, true);
			}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 构造权益卡数据
     * @return
     */
    public PrivilegeCardObj_Info toProto()
    {
    	PrivilegeCardObj_Info proto = new PrivilegeCardObj_Info();
    	proto.setCardType(_m_eCardType);
    	proto.setStartS(_m_lStartS);
    	proto.setEndS(_m_lEndS);
    	proto.setLastGainDailyRewardS(_m_lLastGainDailyRewardS);
    	
    	return proto;
    }
    
    /**
     * 增加次数
     * @param _count
     * @param _context
     */
    protected void _addCount(long _count, NPPlayerContext _context) 
    {
    	if(null == _m_ref)
    	{
    		USLog.error(getUSServer(), "player:{} cardType:{} _addCount fail, not find ref.", getCid(), _m_eCardType);
    		return;
    	}
    	
    	//根据当前卡状态是否超时来重新计算
    	if(isEffect()) //尚未失效，则截至时间直接累加
    	{
    		_m_lEndS += _m_ref.effect_days * 86400 * _count;
    	}
    	else //已经失效，重新计算
    	{
    		long nowS = CommonFunc.getNowTimeSec();
    		
    		_m_lStartS = nowS;
    		_m_lEndS = _m_lStartS + _m_ref.effect_days * 86400 * _count;
    	}
		//更新数据
		_save();
	}
	
	/**
	 * 领取权益卡每日奖励
	 * @param _context
	 * @return
	 */
	public Result gainDailyReward(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查激活状态
			if(!isEffect())
				return PlayerErr.PRIVILEGE_CARD_NOT_EFFECT;
			
			int nowS = CommonFunc.getNowTimeSec();
			//检查上次领取奖励时间
			int nowZeroS = CommonFunc.getZeroClockS(nowS);//当前时间的0点时刻
			//今天已经领取奖励
			if(_m_lLastGainDailyRewardS > nowZeroS)
				return PlayerErr.PRIVILEGE_CARD_DAILY_REWARD_GAINED;
			
			//更新上次时间
			_m_lLastGainDailyRewardS = nowS;
			_save();
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_054_OnPrivilegeCardChg(this));
			
			//领取奖励
			getUserData().gainItemList(_m_ref.daily_gain_item_list, _context);
			//领取金币奖励
			SpecialItemDealer_Gold goldDealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
			if(null != goldDealer)
			{
				goldDealer.gainItemBySecs(_m_ref.daily_gain_silver_mins * 60, true, false, _context);
			}
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置权益卡
	 * @param _startS
	 * @param _endS
	 * @param _lastGainS
	 */
	public void cmdSet(int _startS, int _endS, int _lastGainS)
	{
		getUserData().lockUser();
		
		try
		{
			//更新数据
			_m_lStartS = _startS;
			_m_lEndS = _endS;
			_m_lLastGainDailyRewardS = _lastGainS;
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_054_OnPrivilegeCardChg(this));

			//检查是否生效
			checkEffective(true);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public String toString()
	{
		getUserData().lockUser();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			sb.append("\neffect:").append(isEffect());
			if(_m_lStartS > 0)
			{
				sb.append("\nstartS:").append(CommonFunc.getTimeString((int) _m_lStartS));
			}
			if(_m_lEndS > 0)
			{
				sb.append("\nendS:").append(CommonFunc.getTimeString((int) _m_lEndS));
			}
			if(_m_lLastGainDailyRewardS > 0)
			{
				sb.append("\nlastGainS:").append(CommonFunc.getTimeString((int) _m_lLastGainDailyRewardS));
			}
			
			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
