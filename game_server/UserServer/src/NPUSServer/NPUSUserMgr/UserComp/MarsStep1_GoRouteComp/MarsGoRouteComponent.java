package NPUSServer.NPUSUserMgr.UserComp.MarsStep1_GoRouteComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.EUsParam;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGTripleLong;
import NPEnum.ENPFunctionType;
import NPGameRes.Refs.Mars.RefMarsGoRoute;
import NPGameRes.Refs.RefFuncUnlock;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.MarsGoRouteMsgMgr.MarsGoRouteMsgMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsGoRouteBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 火星 - 前往火星
 * @author mj
 *
 */
public class MarsGoRouteComponent extends _ANPUserComponent
{
	//当前数据ID
	private long _m_lId;
	//当前阶段
	private int _m_iStage;
	//第一阶段到达时间，即前往火星开启时间（毫秒）
	private long _m_lArrivedMs;
	//当前阶段开始时间（毫秒）
	private long _m_lStageStartMs;
	
	//是否已经发送登录成功跑马灯
	private boolean _m_bSendDoneMarquee;
	//当前阶段是否已发送过留言（阶段变更时清空）
	private boolean _m_bHasSentStageMsg;
	
    public MarsGoRouteComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MARS_GO_ROUTE);
    }
    
    public int getStage() {return _m_iStage;}
    public long getArrivedMs() {return _m_lArrivedMs;}
    public long getStageStartMs() {return _m_lStageStartMs;}
    
    @Override
    protected void _init()
    {
		getUSServer().getBM().getBM(PlayerMarsGoRouteBO.class).findOne("cid", getUserData().getCid(), 
				new _ASelectCallback<PlayerMarsGoRouteBO>() 
		{
			@Override
			public void dealSuc(PlayerMarsGoRouteBO _bo) 
			{
				_m_lId = _bo.getId();
				_m_iStage = _bo.getStage();
				_m_lArrivedMs = _bo.getArrivedMs();
				_m_lStageStartMs = _bo.getStageStartMs();
				_m_bSendDoneMarquee = _bo.getSendDoneMarquee();
				_m_bHasSentStageMsg = _bo.getHasSentStageMsg();
				
				setInited();
			}

			@Override
			public void dealFail() 
			{
                if (getHasErr())
                {
                    USLog.error(getUSServer(), "MarsGoRouteComponent _init fail cid:{}", getUserData().getCid());
                    getUserData().setDataLoadFail();
                    return;
                }
				
                setInited();
			}
		});
	}

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }
    
    private void _save()
    {
		BM bmObj = getUSServer().getBM();
		
    	if(0 == _m_lId)
    	{
    		PlayerMarsGoRouteBO bo = new PlayerMarsGoRouteBO();
    		bo.setCid(bmObj, getCid());
    		bo.setStage(bmObj, _m_iStage);
    		bo.setArrivedMs(bmObj, _m_lArrivedMs);
    		bo.setStageStartMs(bmObj, _m_lStageStartMs);
    		bo.setSendDoneMarquee(bmObj, _m_bSendDoneMarquee);
    		bo.setHasSentStageMsg(bmObj, _m_bHasSentStageMsg);
    		bo.insert(bmObj);
    		
    		_m_lId = bo.getId();
    	}
    	else
    	{
    		ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("stage", _m_iStage);
            updateValue.addValueObj("arrivedMs", _m_lArrivedMs);
            updateValue.addValueObj("stageStartMs", _m_lStageStartMs);
            updateValue.addValueObj("sendDoneMarquee", _m_bSendDoneMarquee ? 1 : 0);
            updateValue.addValueObj("hasSentStageMsg", _m_bHasSentStageMsg ? 1 : 0);

            bmObj.getBM(PlayerMarsGoRouteBO.class).update("id", _m_lId, updateValue);
    	}
    }
    
    /**
     * 阶段是否全部完成
     * @return
     */
    public boolean isAllDone()
    {
    	return getUserData().getPlayerComponent().getBo().getIsMarsGoRouteDone();
    }
    
    /**
     * 设置是否全部完成
     * @param _context
     */
    public void setAllDone(NPPlayerContext _context)
    {
    	getUserData().getPlayerComponent().getBo().saveIsMarsGoRouteDone(getUSServer().getBM(), true);

    	//累计全服抵达人数
    	getUSServer().onMarsGoRouteArrived();

    	//推送数据
    	getUserData().sendMsgToGC(US2GCWriter_038_MarsOp.make_051_OnGoToAllStageDone());
    }
    
    /**
     * 启动前往火星
     * @param _context
     * @return
     */
    public Result startStage(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		RefFuncUnlock refFuncUnlock = RefFuncUnlock.getMgr().get(ENPFunctionType.MARS.ordinal());
            if (refFuncUnlock != null
                    && !NPPlayerConditionDealerMgr.IsEnable(refFuncUnlock.simple_unlock_id, getUserData(), null))
            {
            	return MarsErr.MARS_NOT_UNLOCK;
            }
    		
    		return doneStage(1, _context);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 到达指定阶段
     * @param _stage
     * @param _context
     * @return
     */
    public Result doneStage(int _stage, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//检查当前阶段，只能是当前阶段+1
    		if(_stage != _m_iStage + 1)
    			return MarsErr.MARS_GO_ROUTE_NEXT_ERROR;
    		
    		//检查当前阶段的完成条件
            //计算当前阶段所需时长
            long needSecs = 0;
    		if(_m_iStage > 0)
    		{
    			RefMarsGoRoute preRef = RefGeneral.Ref().getMarsGoRouteStageMapMgr().getLevelData(_m_iStage);
        		if(null == preRef)
        			return CommErr.REF_NOT_FOUND;
        		if(!NPPlayerConditionDealerMgr.IsEnable(preRef.done_simple_unlock_id, getUserData(), null))
        			return CommErr.CONDITION_NOT_ENABLE;

                needSecs = preRef.continue_secs;
    		}
    		
    		//当前阶段配置
    		RefMarsGoRoute ref = RefGeneral.Ref().getMarsGoRouteStageMapMgr().getLevelData(_stage);
    		if(null == ref)
    			return CommErr.REF_NOT_FOUND;

            //根据全服抵达人数动态减少时长
            long reducePer = _getArriveReducePer();
            if (reducePer > 0)
                needSecs = needSecs * (10000 - reducePer) / 10000;

    		long secs = CommonFunc.getNowTimeMS() - _m_lStageStartMs;
    		if(secs < needSecs)
    			return MarsErr.MARS_GO_ROUTE_NEXT_SECS_NOT_FULL;
    		
    		//更新数据（阶段变更时清空本阶段留言标识）
    		_m_iStage = _stage;
    		_m_bHasSentStageMsg = false;
    		if(1 == _m_iStage)
    		{
    			// 第一阶段为出发事件，记录实际操作时间作为起点
    			_m_lStageStartMs = CommonFunc.getNowTimeMS();
    			_m_lArrivedMs = _m_lStageStartMs;
    		}
    		else
    		{
    			// 后续阶段：从上一阶段开始时间加上所需时长推算，防止累积多余时间
    			_m_lStageStartMs = _m_lStageStartMs + needSecs * 1000;
    		}
    		_save();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_038_MarsOp.make_050_OnGoToStageArrived(_m_iStage, _m_lArrivedMs, _m_lStageStartMs));
    		
    		//领取奖励
    		getUserData().gainItemList(ref.arrive_item_list, _context);
    		
    		//检查是否完成火星完成
    		RefMarsGoRoute nextRef = RefGeneral.Ref().getMarsGoRouteStageMapMgr().getLevelData(_m_iStage + 1);
    		if(null == nextRef)
    		{
                setAllDone(_context);
    		}
    		
    		return Result.SUCC;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 直接设置前往火星 阶段（GM命令专用）
     * @param _stage
     * @param _context
     */
    public void cmdSetStage(int _stage, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//更新数据
    		_m_iStage = _stage;
    		_save();
    		
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_038_MarsOp.make_050_OnGoToStageArrived(_m_iStage, _m_lArrivedMs, _m_lStageStartMs));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 同时偏移开启时间和当前阶段开始时间（GM命令专用）
     * @param _offsecs 偏移秒数
     * @param _context
     */
    public void cmdSetStartMs(int _offsecs, NPPlayerContext _context)
    {
    	getUserData().lockUser();

    	try
    	{
    		//同时更新两个时间字段
    		_m_lArrivedMs += _offsecs * 1000L;
    		_m_lStageStartMs += _offsecs * 1000L;
    		_save();

    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_038_MarsOp.make_050_OnGoToStageArrived(_m_iStage, _m_lArrivedMs, _m_lStageStartMs));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 发送登录成功跑马灯
     */
    public void sendDoneMarquee()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		//已经发送
    		if(_m_bSendDoneMarquee)
    			return;
    		
    		_m_bSendDoneMarquee = true;
    		_save();
    		
    		//发送跑马灯
    		ArrayList<String> params = new ArrayList<>();
    		params.add(getUserData().getPlayerComponent().getName());
    		getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().mars_go_to_finish_marquee_id, params);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 当前阶段是否已发送过留言
     */
    public boolean isHasSentStageMsg() {return _m_bHasSentStageMsg;}

    /**
     * 发送阶段留言
     * 每个阶段只能发送一次，阶段变更后可再次发送
     *
     * @param _stage 客户端当前阶段，需与服务器一致
     * @param _content 留言内容
     * @return 操作结果
     */
    public Result sendStageMsg(int _stage, String _content)
    {
        getUserData().lockUser();

        try
        {
            //未开始则不允许留言
            if(_m_iStage <= 0)
                return MarsErr.MARS_GO_ROUTE_STAGE_NOT_ALLOW_MSG;

            //校验客户端阶段与服务器阶段一致
            if(_stage != _m_iStage)
                return MarsErr.MARS_GO_ROUTE_STAGE_NOT_ALLOW_MSG;

            //本阶段已发送过
            if(_m_bHasSentStageMsg)
                return MarsErr.MARS_GO_ROUTE_ALREADY_SENT_STAGE_MSG;

            _m_bHasSentStageMsg = true;
            _save();

            //记录到服务器留言管理器
            MarsGoRouteMsgMgr msgMgr = getUSServer().getMarsGoRouteMsgMgr();
            msgMgr.addMsg(_m_iStage, getCid(), getUserData().getPlayerComponent().getName(), _content);

            return Result.SUCC;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 根据全服累计抵达人数查询时长减少万分比，无匹配则返回0
     * first=-1表示无下限，second=-1表示无上限
     */
    private long _getArriveReducePer()
    {
        long arriveCount = getUSServer().getUSParams().getParam(EUsParam.MARS_GO_ROUTE_ARRIVE_COUNT);
        List<WCGTripleLong> reduceList = RefGeneral.Ref().mars_go_route_arrive_reduce_list;
        for (WCGTripleLong item : reduceList)
        {
            boolean lowerOk = item.getFirst() == -1 || arriveCount >= item.getFirst();
            boolean upperOk = item.getSecond() == -1 || arriveCount <= item.getSecond();
            if (lowerOk && upperOk)
                return item.getThree();
        }
        return 0;
    }
}
