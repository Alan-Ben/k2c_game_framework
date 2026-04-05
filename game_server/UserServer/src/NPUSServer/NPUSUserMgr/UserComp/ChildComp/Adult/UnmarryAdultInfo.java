package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ChildEnum.EAdultStatus;
import Common.ChildObj.Adult_UnmarriedInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.MatchAdultMgr.MatchAdultItem;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USDB.Bo.PlayerAdultBO;
import USDB.Bo.PlayerAdultMarryApplyBO;

/**
 * 成年未婚子嗣数据
 * @author mj
 *
 */
public class UnmarryAdultInfo extends _AAdultInfo
{
	//用于联姻池的匹配ID（性别匹配）
	private int _m_iMatchId;
	//联姻池匹配数据
	private MatchAdultItem _m_miMatchItem;
	
	//发起指定联姻请求
	private MarryApplyInfo _m_aiMarryApply;
	
	public UnmarryAdultInfo(ChildComponent _comp, PlayerAdultBO _bo)
	{
		super(_comp, _bo);
		
		_initMatch();
	}
	
	//初始化匹配ID
	private void _initMatch()
	{
//		RefChildInitRes ref = RefChildInitRes.getMgr().get(getInitResId());
//		if(null == ref)
//		{
//			_m_iMatchId = -1;
//		}
//		else
//		{
//			_m_iMatchId = ref.sex.ordinal();
//		}
		//【GOB-3679】学徒联谊时去掉性别限制
		_m_iMatchId = -1;
	}
	//获取联姻池匹配ID（性别匹配）
	public int getMatchId() {return _m_iMatchId;}
	//获取申请截至时间
	public int getApplyExpiredTs()
	{
		getUserData().lockUser();
		
		try
		{
			//指定联姻申请
			if(null != _m_aiMarryApply)
			{
				return _m_aiMarryApply.getApplyExpiredTs();
			}
			
			//联姻池申请
			if(null != _m_miMatchItem)
			{
				return _m_miMatchItem.getApplyExpiredTs();
			}
			
			return 0;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

    /**
     * 允许的最低联姻收益
     * @return
     */
    public long getApplyMinBonus()
    {
        getUserData().lockUser();

        try
        {
            //联姻池申请
            if(null != _m_miMatchItem)
            {
                return _m_miMatchItem.getMinBonus();
            }

            return 0;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }
	
	//发起指定联姻的数据
	public MarryApplyInfo getMarryApply() {return _m_aiMarryApply;}
	//获取指定联姻请求目标玩家CID
	public long getMarryApplyTargetCid() 
	{
		getUserData().lockUser();
		
		try
		{
			return null == _m_aiMarryApply ? 0 : _m_aiMarryApply.getTargetCid();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造未婚子嗣数据
	 * @return
	 */
	public Adult_UnmarriedInfo toUnmarriedProto()
	{
		Adult_UnmarriedInfo proto = new Adult_UnmarriedInfo();
		proto.setAdult(toProto());
		proto.setStatus(getStatus());
		proto.setExpiredTs(getApplyExpiredTs());
        proto.setMinAllowBonus(getApplyMinBonus());
		
		return proto;
	}
	
	/**
	 * 设置指定联姻数据
	 * @param _bInited
	 * @param _apply
	 * @param _context
	 */
	public void setMarryApply(boolean _bInited, MarryApplyInfo _apply, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			EAdultStatus preStatus = getStatus();
			
			_m_aiMarryApply = _apply;
			
			//更新状态：指定联姻请求中
			setStatus(EAdultStatus.APPLY_PLAYER);

			//推送数据
			if(!_bInited)
			{
				getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));

				//日志
				_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 转变为指定联姻状态
	 * @param _targetCid
	 * @param _context
	 * @return
	 */
	public MarryApplyInfo transToMarryApply(long _targetCid, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(!isIdle())
				return null;
			
			//构造请求数据
			BM bmObj = getUSServer().getBM();
			
			PlayerAdultMarryApplyBO bo = new PlayerAdultMarryApplyBO();
			bo.setCid(bmObj, getBo().getCid());
			bo.setAdultId(bmObj, getAdultId());
			bo.setTargetCid(bmObj, _targetCid);
			bo.setApplyExpiredTs(bmObj, CommonFunc.getNowTimeSec() + RefGeneral.Ref().marry_apply_expired_S);
			bo.insert(bmObj);
			
			MarryApplyInfo apply = new MarryApplyInfo(this, bo);
			
			//挂载请求数据
			setMarryApply(false, apply, _context);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));
			
			return apply;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查并移除指定联姻请求数据
	 * @param _targetCid
	 * @param _context
	 */
	public void checkAndDelMarryApply(long _targetCid, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//检查请求数据
			if(null == _m_aiMarryApply)
				return;
			
			//检查是否指定玩家发起的请求，_targetCid=0时不做检查
			if(_targetCid > 0 && _m_aiMarryApply.getTargetCid() != _targetCid)
				return;
			
			EAdultStatus preStatus = getStatus();
			
			//发起异步请求，移除目标玩家数据
			MarryApplyInfo apply = _m_aiMarryApply;
			ALSynTaskManager.getInstance().regTask(()->
			{
				AdultMarrySystem.SendCancelPlayerApply(apply, _context);
			});
			
			//销毁请求数据
			_m_aiMarryApply._discard();
			_m_aiMarryApply = null;
			
			//更新子嗣状态
			setStatus(EAdultStatus.NONE);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));
			
			//日志
			_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置进入联姻池中
	 * @param _bInited
	 * @param _matchItem
	 */
	public void setMatchItem(boolean _bInited, MatchAdultItem _matchItem, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			EAdultStatus preStatus = getStatus();
			
			_m_miMatchItem = _matchItem;
			
			//更新状态：联姻池中
			setStatus(EAdultStatus.APPLY_SERVER);

			//推送数据
			if(!_bInited)
			{
				getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));

				//日志
				_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 移除联姻池状态
	 */
	public void unsetMatchItem()
	{
		getUserData().lockUser();
		
		try
		{
			if(null == _m_miMatchItem)
				return;

			EAdultStatus preStatus = getStatus();
			
			_m_miMatchItem = null;
			
			//更新状态：空闲状态
			setStatus(EAdultStatus.NONE);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));

			//日志
			_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), null);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 设置子嗣空闲状态（移除所有请求数据）
	 * @param _bInited
	 * @param _context
	 */
	public void setIdle(boolean _bInited, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			EAdultStatus preStatus = getStatus();
			
			//移除个人申请数据
			if(null != _m_aiMarryApply)
			{
				_m_aiMarryApply._discard();
				_m_aiMarryApply = null;
			}
			
			//移除全服申请数据
			if(null != _m_miMatchItem)
			{
				_m_miMatchItem = null;
				ALSynTaskManager.getInstance().regTask(()->
				{
					getUSServer().getMatchAdultPool().removeItem(getAdultId(), _context);
				});
			}
			
			setStatus(EAdultStatus.NONE);

			//推送数据
			if(!_bInited)
			{
				getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));

				//日志
				_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 设置子嗣空闲状态
	 * @param _apply
	 * @param _context
	 */
	public void setApplyIdle(MarryApplyInfo _apply, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			EAdultStatus preStatus = getStatus();

			//移除个人申请数据
			if(_apply != _m_aiMarryApply || preStatus != EAdultStatus.APPLY_PLAYER)
				return;

			_m_aiMarryApply._discard();
			_m_aiMarryApply = null;


			setStatus(EAdultStatus.NONE);

			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_059_OnUnmarriedAdultStatusChg(this));

			//日志
			_AAdultInfo.logAdultStatusChg(this, preStatus, getStatus(), getApplyExpiredTs(), _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}


	/**
	 * 是否过期 true-过期
	 * @return
	 */
	protected boolean _isExpired() 
	{
		getUserData().lockUser();
		
		try
		{
			int expiredTs = getApplyExpiredTs();
			
			return expiredTs > 0 && CommonFunc.getNowTimeSec() >= expiredTs;
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
}
