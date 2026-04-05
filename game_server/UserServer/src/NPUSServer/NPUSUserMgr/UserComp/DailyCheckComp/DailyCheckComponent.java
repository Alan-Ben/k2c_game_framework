package NPUSServer.NPUSUserMgr.UserComp.DailyCheckComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.DailyCheckObj.DailyCheck_Info;
import Common.DailyCheckObj.DailyCheck_RewardInfo;
import GS2GC.p021_PlayerInfo.GS2GC_021_068_OnDailyCheckChg;
import GS2GC.p021_PlayerInfo.GS2GC_021_069_OnDailyCheckRewardChg;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.DailyCheckErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.DailyCheck.*;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerDailyCheckBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class DailyCheckComponent extends _ANPUserComponent
{
    private PlayerDailyCheckBO _m_bo;
    private List<Long> _m_dessertList;
    private NPCommon_ItemList _m_checkRewardList;
    //每日登录阶段奖励展示部分列表
    private DailyCheckLoopRewardShowResult _m_rsrRewardShowResult;

    public DailyCheckComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.DAILY_CHECK);

        _m_dessertList = new ArrayList<>();
        _m_checkRewardList = new NPCommon_ItemList();
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("daily_check_comp_init");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: 数据加载
        process.addResDelegateProcess(action -> _initInfo(action::dealAction), "init_info",
                () -> USLog.error(getUSServer(), "player:{} load info bo fail.", getUserData().getCid()), false);

        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "player:{} load daily check comp fail.", getUserData().getCid());
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
     * 数据加载
     * @param _handler 回调
     */
    private void _initInfo(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerDailyCheckBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerDailyCheckBO>()
        {
            @Override
            public void dealSuc(PlayerDailyCheckBO _bo)
            {
                _m_bo = _bo;
                _m_dessertList = new ArrayList<>(CommonFunc.listLongFromString(_m_bo.getDessertList()));
                _m_checkRewardList = new NPCommon_ItemList();
                if (_m_bo.getRewardList() != null)
                    _m_checkRewardList.readPackage(ByteBuffer.wrap(_m_bo.getRewardList()));
                
                //每日登录阶段奖励展示数据
                _m_rsrRewardShowResult = RefDailyCheckLoopReward.getMgr().getGroupList(
                		_m_bo.getRewardedCheckDays() + 1, RefGeneral.Ref().daily_check_step_show_num);
                //奖励数据错误，不影响玩家登录，只影响玩家领取签到阶段奖励
                if(null == _m_rsrRewardShowResult)
                {
                	USLog.error(getUSServer(), "player:{} daily-check-reward cal rewarded-day:{} fail.", getUserData().getCid(), _m_bo.getRewardedCheckDays());
                }
                
                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                boolean hasErr = getHasErr();
                if (hasErr)
                {
                    _handler.onRunOver(false);
                    return;
                }

                PlayerDailyCheckBO bo = new PlayerDailyCheckBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.insert(getUSServer().getBM());
                _m_bo = bo;

                //每日登录阶段奖励展示数据
                _m_rsrRewardShowResult = RefDailyCheckLoopReward.getMgr().getGroupList(
                		_m_bo.getRewardedCheckDays() + 1, RefGeneral.Ref().daily_check_step_show_num);
                //奖励数据错误，不影响玩家登录，只影响玩家领取签到阶段奖励
                if(null == _m_rsrRewardShowResult)
                {
                	USLog.error(getUSServer(), "player:{} daily-check-reward cal rewarded-day:{} fail.", getUserData().getCid(), _m_bo.getRewardedCheckDays());
                }

                _handler.onRunOver(true);
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        tryRefresh();
    }

    @Override
    public void dispose()
    {
    }

    /**
     * 获取玩家距离上次签到的天数
     */
    public int getDaysFromLastCheck(long _curRoundStartTimeMs)
    {
        //如果玩家还没有签到过，则返回1
        if (_m_bo.getLastCheckRoundStartTimeMs() == 0)
            return 1;

        //上一次签到轮的开始时间ms
        long lastCheckRoundTimeMs = _m_bo.getLastCheckRoundStartTimeMs();
        //计算玩家此次登录和上次签到时间相差 的天数
        long gapTimeMs = _curRoundStartTimeMs - lastCheckRoundTimeMs;
        //返回相差的天数
        return (int) (gapTimeMs / (CommonFunc.DAY_SEC * 1000L));
    }

    /**
     * 检查是否可以执行刷新
     * @return 是否可以刷新
     */
    public boolean checkCanRefresh()
    {
        //如果还没到下一次刷新时间，则直接返回失败
        if (_m_bo.getNextRefreshTimeMs() > CommonFunc.getNowTimeMS())
            return false;

        //如果玩家没有签到，且玩家在线期间跨天，这时候玩家的在线时间没超过下一次刷新时间，则不刷新
        if (!_m_bo.getHasCheck() && getUserData().getOnlineTimeMs() < _m_bo.getNextRefreshTimeMs())
            return false;

        return true;
    }

    /**
     * 刷新数据
     */
    public void cmdRefresh()
    {
        //刷新数据
        _refresh();
    }


    /**
     * 尝试刷新数据
     */
    public void tryRefresh()
    {
        //如果还没达成刷新条件，则直接返回
        if (!checkCanRefresh())
            return;

        //刷新数据
        _refresh();
    }

    /**
     * 刷新逻辑
     */
    private void _refresh()
    {
        //检查刷新规则配置
        if (RefGeneral.Ref().daily_check_refresh_clock == null)
        {
            USLog.error(getUSServer(), "DailyCheckComponent _refresh RefGeneral.Ref().daily_check_refresh_clock not found");
            return;
        }

        //获取此轮的开始时间
        long curRoundStartTimeMs = RefGeneral.Ref().daily_check_refresh_clock.getBeforeFreshTimeTagMS(CommonFunc.getNowTimeMS());

        //玩家的所有妃子
        List<ConsortInfo> consortList = getUserData().getConsortComponent().getConsortList();

        //遍历删除不在妃子表中的妃子
        for (int i = consortList.size() - 1; i >= 0; i--)
        {
            ConsortInfo consortInfo = consortList.get(i);
            RefDailyCheckConsort refDailyCheckConsort = RefDailyCheckConsort.getMgr().get(consortInfo.getConsortId());
            if (refDailyCheckConsort == null)
            {
                consortList.remove(i);
            }
        }

        //目标妃子id
        long consortId;

        //从可选的妃子列表里随机一个妃子；如果可选为空，则选择默认妃子
        if (!consortList.isEmpty())
        {
            consortId = CommonFunc.randSelect(consortList).getConsortId();
        }else
        {
            consortId = RefGeneral.Ref().daily_check_default_consort_id;

            if (consortId == 0)
            {
                USLog.error(getUSServer(), "DailyCheckComponent _refresh daily_check_default_consort_id configuration is incorrect, consortId:{}",RefGeneral.Ref().daily_check_default_consort_id);
                return;
            }
        }

        //随机妃子的问候语相关信息
        RefDailyCheckConsort refDailyCheckConsort = RefDailyCheckConsort.getMgr().get(consortId);
        if (refDailyCheckConsort == null)
        {
            USLog.error(getUSServer(), "DailyCheckComponent _refresh consortId:{} refDailyCheckConsort not found", consortId);
            return;
        }

        //计算间隔上次签到的天数
        int daysFromLastCheck = getDaysFromLastCheck(curRoundStartTimeMs);
        //根据间隔时间计算需要展示的甜品数量
        int dessertNum = _calcDessertNum(daysFromLastCheck);
        //根据甜品数量，随机出需要展示的甜品
        List<RefDailyCheckDessert> allDessertList = new ArrayList<>(RefDailyCheckDessert.getMgr().getList());
        //随机打乱顺序
        Collections.shuffle(allDessertList);
        //直接用sublist方法截取需要的数量, 需要注意甜品数量不足的情况，则返回整个列表
        List<RefDailyCheckDessert> dessertList = allDessertList.subList(0, Math.min(dessertNum, allDessertList.size()));
        //此处只需要甜品的id列表
        List<Long> dessertIdList = new ArrayList<>();
        for (RefDailyCheckDessert refDessert : dessertList)
        {
            dessertIdList.add(refDessert.Id());
        }
        //计算下次刷新时间
        long nextFreshTimeTagMS = RefGeneral.Ref().daily_check_refresh_clock.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());

        BM bmObj = getUSServer().getBM();

        _m_bo.setCurRoundStartTimeMs(bmObj, curRoundStartTimeMs);
        _m_bo.setNotCheckDays(bmObj, daysFromLastCheck);
        _m_bo.setConsortId(bmObj, consortId);
        _m_bo.setDessertList(bmObj, CommonFunc.list2String(dessertIdList));
        _m_bo.setHasCheck(bmObj, false);
        _m_bo.setChooseDessertId(bmObj, 0);
        _m_bo.setRewardList(bmObj, null);
        _m_bo.setNextRefreshTimeMs(bmObj, nextFreshTimeTagMS);
        _m_bo.saveAllMarked(bmObj);

        _m_dessertList = dessertIdList;
        _m_checkRewardList.getItemList().clear();

        //刷新成功后，通知客户端
        getUserData().sendMsgToGC(new GS2GC_021_068_OnDailyCheckChg(makeCheckInfo()));
    }

    /**
     * 计算需要展示的甜品数量
     * @param _daysFromLastCheck 距离上次签到的天数
     * @return 需要展示的甜品数量
     */
    private int _calcDessertNum(int _daysFromLastCheck)
    {
        //默认展示的甜品数量 + (如果距离上次签到的天数超过了签到的间隔时间，则额外展示的甜品数量)
        return RefGeneral.Ref().daily_check_dessert_num + ((_daysFromLastCheck >= RefGeneral.Ref().daily_check_timeout) ? RefGeneral.Ref().daily_check_extra_dessert_num : 0);
    }

    /**
     * 玩家签到
     * @param _dessertId 玩家选择的甜品id
     * @param _context 上下文
     * @return 签到结果
     */
    public Result check(long _dessertId, NPPlayerContext _context)
    {
        //检查是否已经初始化完成数据
        if (_m_bo.getCurRoundStartTimeMs() == 0)
            return CommErr.UNKNOW_ERR;

        //检查是否已经签到
        if (_m_bo.getHasCheck())
            return DailyCheckErr.DAILY_CHECK_HAS_CHECK;

        //检查玩家选择的甜品是否合法
        if (!_m_dessertList.contains(_dessertId))
            return DailyCheckErr.DAILY_CHECK_DESSERT_NOT_FOUND;

        BM bmObj = getUSServer().getBM();

        _m_bo.setLastCheckRoundStartTimeMs(bmObj, _m_bo.getCurRoundStartTimeMs());
        _m_bo.setHasCheck(bmObj, true);
        _m_bo.setChooseDessertId(bmObj, _dessertId);

        //领取奖励
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(RefGeneral.Ref().daily_check_once_reward_id);
        if (rewardObj == null)
        {
            USLog.error(getUSServer(), "DailyCheckComponent check rewardObj not found, rewardId:{}", RefGeneral.Ref().daily_check_once_reward_id);
        }else
        {
            getUserData().gainItemList(rewardObj.getItemList(), _context);
        }

        _context.getCollector().fillProtoList(_m_checkRewardList.getItemList());

        _m_bo.setRewardList(bmObj, _m_checkRewardList.makePackage().array());
        _m_bo.setTotalCheckDays(bmObj, _m_bo.getTotalCheckDays() + 1);
        _m_bo.saveAllMarked(bmObj);

        //通知客户端
        getUserData().sendMsgToGC(new GS2GC_021_068_OnDailyCheckChg(makeCheckInfo()));
        //推送消息
        getUserData().sendMsgToGC(new GS2GC_021_069_OnDailyCheckRewardChg(makeCheckRewardInfo()));

        return Result.SUCC;
    }

    /**
     * 领取登录天数阶段奖励
     * @param _context
     * @return
     */
    public ArrayList<DailyCheckLoopRewardShow> drawLoopReward(NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(null == _m_rsrRewardShowResult)
    			return null;
    		
    		ArrayList<DailyCheckLoopRewardShow> newRewardedDays = new ArrayList<>();
    		NPItemCollector gainItemCollect = new NPItemCollector(0);
    		int rewardedDays = 0;
    		for(int i = 0; i < _m_rsrRewardShowResult.getShowList().size(); i++)
    		{
    			DailyCheckLoopRewardShow show = _m_rsrRewardShowResult.getShowList().get(i);
    			if(null == show)
    				continue;
    			
    			//超过总的登录天数，退出
    			if(show.getCurDay() > _m_bo.getTotalCheckDays())
    				break;
    			
    			//已领奖
    			if(show.getCurDay() <= _m_bo.getRewardedCheckDays())
    				continue;
    			
    			//记录领取奖励数据
    			rewardedDays = show.getCurDay();
    			newRewardedDays.add(show);
    			gainItemCollect.addItemList(show.getRef().gain_item_list);
    		}
    		
    		//有领取奖励
    		if(rewardedDays > 0)
    		{
    			_m_bo.saveRewardedCheckDays(getUSServer().getBM(), rewardedDays);
    			
    			//计算下次领取奖励数据
    			int calRewardedDays = rewardedDays + 1;
    			if(calRewardedDays >= _m_rsrRewardShowResult.getCurLastDay())
    			{
    				_m_rsrRewardShowResult = RefDailyCheckLoopReward.getMgr().getGroupList(calRewardedDays, RefGeneral.Ref().daily_check_step_show_num);
        			if(null == _m_rsrRewardShowResult)
        			{
                    	USLog.error(getUSServer(), "player:{} daily-draw-reward cal rewarded-day:{} fail.", getUserData().getCid(), calRewardedDays);
        			}
    			}
    			//推送变化
    			getUserData().sendMsgToGC(new GS2GC_021_069_OnDailyCheckRewardChg(makeCheckRewardInfo()));
    			
    			//领取奖励
    			for(int i = 0; i < newRewardedDays.size(); i++)
    			{
    				DailyCheckLoopRewardShow newRewardedObj = newRewardedDays.get(i);
    				if(null == newRewardedObj)
    					continue;
    				
    				getUserData().gainItemList(gainItemCollect.getAllItemList(), _context);
    			}
    		}
    		
    		return newRewardedDays;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 构造玩家的签到信息结构体
     * @return 玩家的签到信息结构体
     */
    public DailyCheck_Info makeCheckInfo()
    {
        DailyCheck_Info info = new DailyCheck_Info();
        info.setNotCheckDays(_m_bo.getNotCheckDays());
        info.setConsortId(_m_bo.getConsortId());
        info.getDessertList().addAll(_m_dessertList);
        info.setHasCheck(_m_bo.getHasCheck());
        info.setChooseDessertId(_m_bo.getChooseDessertId());
        info.getItemList().addAll(_m_checkRewardList.getItemList());
        info.setNextRefreshTimeMs(_m_bo.getNextRefreshTimeMs());
        return info;
    }

    /**
     * 构造玩家累计签到奖励信息结构体
     * @return 玩家累计签到奖励信息结构体
     */
    public DailyCheck_RewardInfo makeCheckRewardInfo()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		DailyCheck_RewardInfo info = new DailyCheck_RewardInfo();
            info.setTotalCheckDays(_m_bo.getTotalCheckDays());
            info.setRewardedDays(_m_bo.getRewardedCheckDays());
            
            if(null != _m_rsrRewardShowResult)
            {
            	for(int i = 0; i < _m_rsrRewardShowResult.getShowList().size(); i++)
            	{
            		DailyCheckLoopRewardShow show = _m_rsrRewardShowResult.getShowList().get(i);
            		if(null == show)
            			continue;
            		
            		info.addShowInfoList(show.toShowObj());
            	}
            	
            	if(null != _m_rsrRewardShowResult.getPreShow())
            	{
            		info.setPreShowInfo(_m_rsrRewardShowResult.getPreShow().toShowObj());
            	}
            	
            	if(null != _m_rsrRewardShowResult.getNextShow())
            	{
            		info.setNextShowInfo(_m_rsrRewardShowResult.getNextShow().toShowObj());
            	}
            }

            return info;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 修改累计签到天数
     * @param _days 累计签到天数
     */
    public void chgTotalCheckDays(int _days)
    {
        _m_bo.setTotalCheckDays(getUSServer().getBM(), _days);
        _m_bo.saveAllMarked(getUSServer().getBM());

        //推送消息
        getUserData().sendMsgToGC(new GS2GC_021_069_OnDailyCheckRewardChg(makeCheckRewardInfo()));
    }
    
    /**
     * 修改已领取奖励签到天数
     * @param _days
     */
    public void chgRewardedCheckDays(int _days)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		_m_bo.setRewardedCheckDays(getUSServer().getBM(), _days);
            _m_bo.saveAllMarked(getUSServer().getBM());
            
            _m_rsrRewardShowResult = RefDailyCheckLoopReward.getMgr().getGroupList(_days + 1, RefGeneral.Ref().daily_check_step_show_num);
            if(null == _m_rsrRewardShowResult)
            {
            	
            }
            else
            {
                //推送消息
                getUserData().sendMsgToGC(new GS2GC_021_069_OnDailyCheckRewardChg(makeCheckRewardInfo()));
            }
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 修改上次签到时间
     * @param _lastCheckTimeMs 上次签到时间
     */
    public void setLastCheckTimeMs(long _lastCheckTimeMs)
    {
        _m_bo.saveLastCheckRoundStartTimeMs(getUSServer().getBM(), _lastCheckTimeMs);
    }

    /**
     * 获取累计签到天数
     * @return 累计签到天数
     */
    public int getDailyCheckSum()
    {
        return _m_bo.getTotalCheckDays();
    }
}
