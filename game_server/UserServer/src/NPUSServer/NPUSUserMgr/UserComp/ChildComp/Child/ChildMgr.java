package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child;

import Common.ChildObj.Child_Info;
import Common.ChildObj.Child_SeatInfo;
import CommonEnum.EBonusPropertyType;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Child.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CHILD;
import NPUSServer.GeneralV.UsID;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.LazyDealer.ChildBonusCalTask;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerChildBO;
import USDB.Bo.PlayerChildSeatBO;
import USLOGDB.Bo.LogChildAddBO;

import java.util.ArrayList;
import java.util.List;

public class ChildMgr 
{
	//子嗣组件对象
	private ChildComponent _m_comp;
	
	//子嗣数据列表
	private ArrayList<ChildInfo> _m_alChildList;
	//训练房数据列表
	private ArrayList<ChildSeatInfo> _m_alSeatList;
	
	//成年子嗣的收益总和
	private long _m_lBonusSum;
	//子嗣收益计算任务
    private LazyTaskDealer _m_lazyCalBonusDealer;
	
	public ChildMgr(ChildComponent _comp)
	{
		_m_comp = _comp;
		
		_m_alChildList = new ArrayList<>();
		_m_alSeatList = new ArrayList<>();
		
		_m_lazyCalBonusDealer = new LazyTaskDealer(new ChildBonusCalTask(this), 100);
		
		_initDefault();
	}

	public ChildComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	public long getBonusSum() {return _m_lBonusSum;}
	public void setBonusSum(long _bonus) {_m_lBonusSum = _bonus;}

	/**
	 * 子嗣收益计算任务
	 */
	public void recalBonus()
    {
		_m_lazyCalBonusDealer.setNeedDeal();
    }
	
	/**
	 * 加载默认数据
	 */
	private void _initDefault()
	{
		//初始化所有席位
		List<RefChildSeat> seatList = RefChildSeat.getMgr().getList();
		for(int i = 0; i < seatList.size(); i++)
		{
			RefChildSeat seatRef = seatList.get(i);
			if(null == seatRef)
				continue;
			
			ChildSeatInfo seat = new ChildSeatInfo(_m_comp, seatRef);
			_m_alSeatList.add(seat);
		}
	}

	/**
	 * 步骤1: 子嗣训练位数据加载
	 * @param _handler
	 */
	public void _initChildSeatFromDB(_ICallBackBool _handler)
	{
        getComp().getUSServer().getBM().getBM(PlayerChildSeatBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerChildSeatBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load child seat bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerChildSeatBO> _list)
            {
            	_initSeatBo(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	//子嗣训练位数据加载
	private void _initSeatBo(List<PlayerChildSeatBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerChildSeatBO bo = _list.get(i);
			if(null == bo)
				continue;

			ChildSeatInfo seat = lookupSeat(bo.getSeatId());
			if(null == seat)
			{
				USLog.error(getUSServer(), "player:{} init child seat:{} bo fail, not find seat info.", getUserData().getCid(), bo.getSeatId());
				continue;
			}
			
			seat._setBo(bo);
		}
	}
	
	/**
	 * 步骤2: 子嗣数据加载
	 * @param _handler
	 */
	public void _initChildFromDB(_ICallBackBool _handler)
	{
        getComp().getUSServer().getBM().getBM(PlayerChildBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerChildBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load child bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerChildBO> _list)
            {
            	_initChildBo(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	//子嗣数据加载
	private void _initChildBo(List<PlayerChildBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerChildBO bo = _list.get(i);
			if(null == bo)
				continue;

			ChildInfo info = new ChildInfo(this, bo);
			_m_alChildList.add(info);
			
			//对应的席位进行标记
			ChildSeatInfo seat = lookupSeat(info.getSeatId());
			if(null == seat)
			{
				CommLog.error("player:{} child:{} load seat:{} fail, not find seat.", getUserData().getCid(), info.getChildId(), info.getSeatId());
			}
			else
			{
				seat.setUsing();
			}
		}
	}
	
	/**
	 * 计算所有子嗣的上课收益总和
	 * @return
	 */
	public long calBonusSum() 
	{
		getUserData().lockUser();
		
		try
		{
			long sum = 0;
			//当前子嗣的收益总和
			for(int i = 0; i < _m_alChildList.size(); i++)
			{
				ChildInfo child = _m_alChildList.get(i);
				if(null == child)
					continue;
				
				sum += child.getTrainBonus();
			}
			
			return sum;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 组件初始化完成调用
	 */
	public void _onInited()
	{
		//计算子嗣属性总和
		_m_lBonusSum = calBonusSum();
		
		//所有训练点初始化处理
		for(int i = 0; i < _m_alSeatList.size(); i++)
		{
			ChildSeatInfo seat = _m_alSeatList.get(i);
			if(null == seat)
				continue;
			
			seat._onInited();
		}
	}

    /**
     * 构造子嗣协议数据列表
     * @param _childList
     */
	public void makeChildProto(ArrayList<Child_Info> _childList)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alChildList.size(); i++)
			{
				ChildInfo child = _m_alChildList.get(i);
				if(null == child)
					continue;
				
				_childList.add(child.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造子嗣训练位数据
	 * @param _seatList
	 */
	public void makeSeatProto(ArrayList<Child_SeatInfo> _seatList)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSeatList.size(); i++)
			{
				ChildSeatInfo seat = _m_alSeatList.get(i);
				if(null == seat)
					continue;
				
				_seatList.add(seat.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/***************
	 * 获取子嗣数量
	 * @return
	 */
	public int getChildCount()
	{
		getUserData().lockUser();
		
		try
		{
			return _m_alChildList.size();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/************
	 * 获取所有子嗣
	 * @return
	 */
	public ArrayList<ChildInfo> getAllChildList()
	{
		getUserData().lockUser();
		
		try
		{
			return new ArrayList<>(_m_alChildList);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定子嗣数据
	 * @param _childId
	 * @return
	 */
	public ChildInfo lookupChild(long _childId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alChildList.size(); i++)
			{
				ChildInfo info = _m_alChildList.get(i);
				if(null == info)
					continue;
				
				if(info.getChildId() == _childId)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定席位
	 * @param _seatId
	 * @return
	 */
	public ChildSeatInfo lookupSeat(long _seatId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSeatList.size(); i++)
			{
				ChildSeatInfo info = _m_alSeatList.get(i);
				if(null == info)
					continue;
				
				if(info.getSeatId() == _seatId)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查是否有尚未使用的训练位
	 * @return
	 */
	public boolean hasUnusingSeat()
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSeatList.size(); i++)
			{
				ChildSeatInfo seat = _m_alSeatList.get(i);
				if(null == seat)
					continue;
				
				//当前的训练位无法解锁，则后续的训练位无需检查
				if(!seat.checkUnlock())
					break;
				
				//尚未使用
				if(!seat.isUsing())
					return true;
			}
			
			return false;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取一个席位
	 * @return
	 */
	public ChildSeatInfo takeSeat()
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSeatList.size(); i++)
			{
				ChildSeatInfo seat = _m_alSeatList.get(i);
				if(null == seat)
					continue;
				
				//当前的训练位无法解锁，则后续的训练位无需检查
				if(!seat.checkUnlock())
					break;
				
				//已经使用
				if(seat.isUsing())
					continue;
				
				//标记当前训练位已使用
				seat.setUsing();
				return seat;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 标记训练位未使用
	 * @param _seatId
	 * @param _context
	 */
	public void unsetSeat(long _seatId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ChildSeatInfo seat = lookupSeat(_seatId);
			if(null == seat)
				return;
			
			seat.unsetUsing();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取新子嗣
	 * @param _consortId
	 * @param _initIntimacy
	 * @param _initResRef
	 * @param _qualityRef
	 * @param _attrRef
	 * @param _careerRef
	 * @param _seatId
	 * @param _isGiftde
	 * @param _baseBonus
	 * @param _initStudyBonus
	 * @param _context
	 * @return
	 */
	public ChildInfo gainChild(long _consortId
			, long _initIntimacy
			, RefChildInitRes _initResRef
			, RefChildQuality _qualityRef
			, RefChildAttr _attrRef
			, RefChildCareer _careerRef
			, long _seatId
			, boolean _isGiftde
			, long _baseBonus
			, int _initStudyBonus
			, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			BM bmObj = getComp().getUSServer().getBM();

			PlayerChildBO bo = new PlayerChildBO();
			bo.setId(UsID.makeChildId(getComp().getUSServer()));
			bo.setCid(bmObj, getUserData().getCid());
			bo.setConsortId(bmObj, _consortId);
			bo.setInitIntimacy(bmObj, _initIntimacy);
			bo.setInitResId(bmObj, _initResRef.id);
			bo.setQuality(bmObj, _qualityRef.id);
			bo.setAttrType(bmObj, _attrRef.type.ordinal());
			bo.setCareer(bmObj, _careerRef.career_id);
			bo.setSeatId(bmObj, _seatId);
			bo.setIsGiftde(bmObj, _isGiftde);
			bo.setBaseBonus(bmObj, _baseBonus);
			bo.setInitStudyBonus(bmObj, _initStudyBonus);
			bo.setCreatedAt(bmObj, CommonFunc.getNowTimeSec());
			bo.insert(bmObj);
			
			ChildInfo info = new ChildInfo(this, bo, _initResRef, _qualityRef, _attrRef, _careerRef);
			_m_alChildList.add(info);
			
			//推送协议
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_050_OnChildAdd(info));
			
			//计算产出速度
			recalBonus();
			
			//子嗣计数
			getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.CHILD_GAIN_NUM, 1, _context);
			
			//单个子嗣收益最高记录
			getUserData().getRecordComponent().ensureRecord(ENPPlayerRecordParam.MAX_EARNINGS_CHILD, info.getTrainBonus(), ENCounterDealType.SET_GT, _context);

			//如果是卷王子嗣 增加计数
			if (_isGiftde)
				getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.GAIN_GIFTED_CHILD_COUNT, 1, _context);

            //获得子嗣事件
            Event_P_GAIN_CHILD evt = new Event_P_GAIN_CHILD(_context, 1);
            getUserData().onLogicEvent(evt);
			
			//日志
			LogChildAddBO logBo = new LogChildAddBO();
			logBo.setCid(bmObj, getUserData().getCid());
			logBo.setChildId(bmObj, info.getChildId());
			logBo.setConsortId(bmObj, info.getConsortId());
			logBo.setInitRes(bmObj, info.getInitResId());
			logBo.setQuality(bmObj, info.getQuality());
			logBo.setAttr(bmObj, info.getAttr().ordinal());
			logBo.setCareer(bmObj, info.getCareer());
			logBo.setSeatId(bmObj, info.getSeatId());
			logBo.setIsGiftde(bmObj, info.isGiftde());
			logBo.setBaseBonus(bmObj, info.getBaseBonus());
			logBo.setInitStudyBonus(bmObj, info.getInitStudyBonus());
			CommLogDB.log(bmObj, logBo, _context);
			
			//mj日志
			info.mjLog(1);
			
			return info;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

    /**
     * 移除子嗣数据（只允许ChildComponent调用）
     * @param _id
     * @param _context
     * @return
     */
	public ChildInfo _delChild(long _id, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ChildInfo info = null;
			for(int i = 0; i < _m_alChildList.size(); i++)
			{
				ChildInfo tmpInfo = _m_alChildList.get(i);
				if(null == tmpInfo)
					continue;
				
				if(tmpInfo.getChildId() == _id)
				{
					info = tmpInfo;
					_m_alChildList.remove(i);
					break;
				}
			}
			
			if(null != info)
			{
				//销毁数据
				info._discard();
				//重置训练位标志位
				unsetSeat(info.getSeatId(), _context);
				//推送数据
				getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_057_OnChildRemove(_id));

				//计算产出速度
				recalBonus();
			}
			
			return info;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 脑力值上限变化时处理
	 * @param _preValue
	 * @param _curVal
	 * @param _context
	 */
	public void onMaxEnergyChg(long _preValue, long _curVal, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSeatList.size(); i++)
			{
				ChildSeatInfo seat = _m_alSeatList.get(i);
				if(null == seat)
					continue;
				
				seat.updateMaxEnergy((int) _curVal, _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 根据训练位移除子嗣（用于GM命令）
	 * @param _seatId
	 * @param _context
	 */
	public void cmdDelChildBySeat(long _seatId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = _m_alChildList.size() - 1; i >= 0; i--)
			{
				ChildInfo child = _m_alChildList.get(i);
				if(null == child)
					continue;
				
				if(0 == _seatId || child.getSeatId() == _seatId)
				{
					_m_alChildList.remove(i);
					//销毁数据
					child._discard();
					//重置训练位标志位
					unsetSeat(child.getSeatId(), _context);
					//推送数据
					getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_057_OnChildRemove(child.getChildId()));
				}
			}
			
			//发起重新计算子嗣收益
			recalBonus();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 获取当前所有子嗣中有名字的子嗣数量
	 * @return
	 */
	public long getNamedChildNum()
	{
		getUserData().lockUser();
		try
		{
			long count = 0;
            for (ChildInfo child : _m_alChildList)
            {
                if (null == child)
                    continue;

                if (!child.getName().isEmpty())
                    count++;
            }
			return count;
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

			sb.append("\nchild size:").append(_m_alChildList.size());

			for(int i = 0; i < _m_alChildList.size(); i++)
			{
				ChildInfo child = _m_alChildList.get(i);
				if(null == child)
					continue;

				sb.append("\nchildId:").append(child.getChildId())
					.append("\n\tconsortId:").append(child.getConsortId())
					.append("\n\tinitIntimacy:").append(child.getInitIntimacy())
					.append("\n\tquality:").append(child.getQuality())
					.append("\n\ttrainAddBonusLevel:").append(child.getQualityRef().maxLvl).append("|").append(CommonFunc.list2String(child.getQualityRef().addBonusStepList))
					.append("\n\tisGiftde:").append(child.isGiftde())
					.append("\n\tbaseBonus:").append(child.getBaseBonus())
					.append("\n\tbonus:").append(child.getTrainBonus())
					.append("\n\tplayerLvl-child_graduate_get_earnings_add:").append(child.getUserData().getPlayerComponent().getCurLevel().child_graduate_get_earnings_add)
					.append("\n\tcareerAdd:").append(child.getCareerRef().career_add)
					.append("\n\tinitStudyBonus:").append(child.getInitStudyBonus())
					.append("\n\tbonus-FIN_STUDY_BONUS:").append(child.getBonusPropertyValue(EBonusPropertyType.FIN_STUDY_BONUS_PER));
					//妃子相关数据
					ConsortInfo consort = child.getUserData().getConsortComponent().lookup(child.getConsortId());
					if(null != consort)
					{
						if(null != consort.getFettersInfo().getLvlRef())
							sb.append("\n\tconsort:").append(child.getConsortId())
								.append(", income_bonus:").append(consort.getFettersInfo().getLvlRef().income_bonus)
								.append(", caretaker_bonus_calculate_coefficient:").append(consort.getFettersInfo().getLvlRef().caretaker_bonus_calculate_coefficient);
						else
							sb.append("\n\tconsort:").append(child.getConsortId()).append(", income_bonus:").append("not find.");
					}
					else
					{
						sb.append("\n\tconsort:").append(child.getConsortId()).append(", not find consort.");
					}
					sb.append("\n\tcalAdultBonus:").append(ChildSystem.calAdultBonus(child));
			}

			return sb.toString();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
