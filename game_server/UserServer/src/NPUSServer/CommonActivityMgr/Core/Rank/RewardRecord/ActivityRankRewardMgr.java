package NPUSServer.CommonActivityMgr.Core.Rank.RewardRecord;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GraveObj.GraveObj_NewInfo;
import Common.MailObj.Mail_Data;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ERankType;
import NPGameRes.Refs.Activity.RefActivityRankReward;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.Rank.ActivityRankInfo;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityRankRewardInfoBO;
import USLOGDB.Bo.LogRankRewardBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 活动排行榜的奖励记录管理器
 * 用于记录排行榜的奖励结算信息，记录玩家的领取状态
 * 提供领奖和补发奖励的接口
 */
public class ActivityRankRewardMgr
{
    private ActivityRankInfo _m_rankInfo;
    private List<ActivityRankRewardInfo> _m_hasDrawRewardList;
    private MutexAtom _m_mutex;
    
    //只记录第一名数据
    private ActivityRankRewardInfo _m_firstRankRewardInfo;

    public ActivityRankRewardMgr(ActivityRankInfo _rankInfo)
    {
        _m_rankInfo = _rankInfo;
        _m_hasDrawRewardList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public ActivityRankInfo getRank()
    {
        return _m_rankInfo;
    }

    public NPUserServer getUSServer()
    {
        return _m_rankInfo.getActivity().getUSServer();
    }

    /**
     * 初始化已领取记录
     * @param _bo 记录数据
     */
    public void initRankRewardInfo(ActivityRankRewardInfoBO _bo)
    {
        _createRankRewardInfo(_bo);
    }

    /**
     * 创建排行榜奖励信息
     * @param _bo 数据
     */
    private void _createRankRewardInfo(ActivityRankRewardInfoBO _bo)
    {
       _lock();
       try{
            ActivityRankRewardInfo info = new ActivityRankRewardInfo(this, _bo);
            _m_hasDrawRewardList.add(info);
            
            //记录第一名数据
            if(info.getRank() == 1)
            {
            	_m_firstRankRewardInfo = info;
            }
       }finally
       {
           _unlock();
       }
    }
    
    /**
     * 获取全部可以进入名人堂的数据
     * 
     * 20260104 - 线上bug修复（只有第一名进入名人堂）
     		GOB-8277 【优化-0】每次冲榜，前10有称号，会进入名人堂。但是一次冲榜进10个会触发点赞，这个点赞会获得1300钻。这个点赞数量需要确认应该有问题
    		https://www.teambition.com/task/695777b326b3e24ac7be9adf
     * 
     * @return
     */
    public ArrayList<GraveObj_NewInfo> getGraveTitleRewardList()
    {
    	_lock();
    	
    	try
    	{
    		if(null == _m_firstRankRewardInfo)
    			return null;
    		
    		//检查奖励数据
            List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(_m_rankInfo.getRankId(), _m_firstRankRewardInfo.getRank());
            if(null == rewardList)
            	return null;
            
            //检查是否可以进入名人堂：检查activity_rank_reward配表的title_reward字段是否有配置，并且一个玩家只需要检查一次
            RefActivityRankReward graveRewardRef = null;
            for(int i = 0; i < rewardList.size(); i++)
            {
            	RefActivityRankReward ref = rewardList.get(i);
            	if(null == ref)
            		continue;
            	
            	if(ref.getTitleId() > 0)
            	{
            		graveRewardRef = ref;
            		break;
            	}
            }
            if(null == graveRewardRef)
            	return null;

    		ArrayList<GraveObj_NewInfo> list = new ArrayList<>();
    		list.add(new GraveObj_NewInfo(graveRewardRef.getTitleId(), _m_firstRankRewardInfo.getCid()));
    		
    		return list;
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 查找Cid的排行榜奖励信息
     */
    public ActivityRankRewardInfo lookupRankRewardInfo(long _cid)
    {
        _lock();
        try{
            for (ActivityRankRewardInfo info : _m_hasDrawRewardList)
            {
                if (info.getCid() == _cid)
                    return info;
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 添加排行榜奖励
     * @param _rankList 排行榜排名记录列表
     */
    /**
     * 不再使用，只在 ActivityRankPlayerRewardedMgr 管理已领取奖励的玩家数据
     */
//    public void addRankReward(List<Rank_ItemDump> _rankList)
//    {
//        _lock();
//        try{
//            BM bmObj = _m_rankInfo.getActivity().getUSServer().getBM();
//
//            for (Rank_ItemDump item : _rankList)
//            {
//                if (lookupRankRewardInfo(item.getKey()) != null)
//                {
//                    USLog.warn(_m_rankInfo.getActivity().getUSServer(), "ActivityRankRewardMgr addRankReward cid already exist, activityInstanceId:{} rankId:{} cid:{} groupId:{} rank:{} ",
//                            _m_rankInfo.getActivity().getInstanceId(), _m_rankInfo.getRankId(), item.getKey(), item.getGroupId(), item.getRank());
//                    continue;
//                }
//
//                ActivityRankRewardInfoBO bo = new ActivityRankRewardInfoBO();
//                bo.setActivityInstanceId(bmObj, _m_rankInfo.getActivity().getInstanceId());
//                bo.setRankId(bmObj, _m_rankInfo.getRankId());
//                bo.setCid(bmObj, item.getKey());
//                bo.setGroupId(bmObj, item.getGroupId());
//                bo.setRank(bmObj, item.getRank());
//                bo.setScore(bmObj, item.getScore());
//                bo.insert(bmObj);
//
//                _createRankRewardInfo(bo);
//            }
//        }finally
//        {
//            _unlock();
//        }
//    }

//    /**
//     * 查询排行榜结算信息
//     * @param _cid 玩家id
//     * @return 执行结果
//     */
//    public ResultOne<Activity_RankSettleInfo> getSettleInfo(long _cid)
//    {
//        //查询是否有奖励
//        ActivityRankRewardInfo rewardInfo = lookupRankRewardInfo(_cid);
//        //由于是查询协议，不需要返回错误码
//        if (rewardInfo == null)
//            return new ResultOne<>(Result.SUCC, null);
//
//        return new ResultOne<>(Result.SUCC, rewardInfo.makeSettleInfo());
//    }

    /**
     * 领取奖励
     * @param _userdata 用户数据
     * @param _context  玩家上下文
     * @return
     */
//    public Result drawReward(NPUSUserData _userdata, NPPlayerContext _context)
//    {
//        //查询是否有奖励
//        ActivityRankRewardInfo rewardInfo = lookupRankRewardInfo(_userdata.getCid());
//        if (rewardInfo == null)
//            return ActivityErr.ACTIVITY_RANK_DONT_HAVE_REWARD;
//
//        _lock();
//        try{
//            //检查是否已领取
//            if (rewardInfo.hadDraw())
//                return ActivityErr.ACTIVITY_RANK_REWARD_HAD_DRAW;
//
//            //领取奖励
//            //设置已领取
//            rewardInfo.setHadDraw(true);
//        }finally
//        {
//            _unlock();
//        }
//
//        //查找对应排名的奖励
//        List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(_m_rankInfo.getRankId(), rewardInfo.getRank());
//        if (rewardList.isEmpty())
//            return CommErr.REF_NOT_FOUND;
//
//        //发放奖励
//        for (RefActivityRankReward reward : rewardList)
//        {
//            _userdata.gainItemList(reward.reward_item_list, _context);
//
//            //发放称号
//            if(reward.getTitleId() > 0)
//            {
//            	_userdata.gainItem(ENPItemType.TITLE, reward.getTitleId(), reward.getTitleExpiredSecs(), _context);
//            }
//        }
//        
//        return Result.SUCC;
//    }

    /**
     * 发送未领取的邮件
     * 过度数据，之后 _m_hasDrawRewardList 不会有新数据
     */
    public void sendNotDrawMail()
    {
    	//不存在数据
    	if(_m_hasDrawRewardList.isEmpty())
    		return;
    	
    	RefRank rankRef = getRank().getRankRef();
        if(null == rankRef)
        {
        	USLog.error(getUSServer(), "ActivityRankRewardMgr.sendNotDrawMail fail, not find ref. id:{} rank:{}", 
        			getRank().getRankInstanceId(), getRank().getRankId());
        	return;
        }
    	
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_RANK_REWARD_SEND_MAIL);
        ArrayList<ActivityRankRewardInfo> rankRewardList = new ArrayList<>(_m_hasDrawRewardList);
        
        //先更新所有标记位，标志已完成
        HashMap<String, Object> conds = new HashMap<>();
        conds.put("activity_instance_id", getRank().getActivity().getInstanceId());
        conds.put("rank_id", getRank().getRankId());
        
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_draw", 1);
        getUSServer().getBM().getBM(ActivityRankRewardInfoBO.class).update(conds, updateValue);
        
        //遍历所有奖励信息
        int mailRewardedCount = 0;
        for (ActivityRankRewardInfo rewardInfo : rankRewardList)
        {
        	if(null == rewardInfo)
        		continue;
        	
        	//检查是否已领取（该方法会检查新流程）
        	boolean isRewarded = getRank().isRewarded(rewardInfo.getCid());
        	if(isRewarded)
        	{
            	//设置标记位（只设置标记位）
            	rewardInfo.setHadDrawFlag(true);
        		continue;
        	}
        	
        	//设置标记位（只设置标记位）
        	rewardInfo.setHadDrawFlag(true);
        	mailRewardedCount++;
            
            //查找对应排名的奖励
            List<RefActivityRankReward> rewardList = RefActivityRankReward.getMgr().getRankRewardRefByRank(_m_rankInfo.getRankId(), rewardInfo.getRank());
            if (rewardList.isEmpty())
                continue;
            
            boolean isLeader = getRank().getActivity().getSettleGuildMgr().isLeader(rewardInfo.getCid());
            NPItemCostCollector_nosafe itemCostCollector = new NPItemCostCollector_nosafe();
            //发放奖励
            for (RefActivityRankReward reward : rewardList)
            {
            	if(null == reward)
            		continue;
            	
            	//分发对应奖励
            	if(!isLeader && (ERankType.GUILD == rankRef.rank_type || ERankType.ACTIVITY_TEAM == rankRef.rank_type)) //非联盟/组队队长，发放普通成员奖励
            	{
            		itemCostCollector.addItemList(reward.member_reward_item_list);
            	}
            	else //玩家冲榜/联盟/组队队长，发放队长奖励
            	{
            		itemCostCollector.addItemList(reward.reward_item_list);

                    //发放称号
                    if(reward.getTitleId() > 0)
                    {
                    	itemCostCollector.addItem(ENPItemType.TITLE, reward.getTitleId(), reward.getTitleExpiredSecs());
                    }
            	}
            }

            NPPlayerContext newContext = NPPlayerContext.createNew(context);

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().activity_rank_reward_mail_id);
            mailData.getContentReplace().add(String.valueOf(getRank().getRankRef().name));
            mailData.getContentReplace().add(String.valueOf(rewardInfo.getRank()));
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(itemCostCollector.getItemList()));
            MailSystem.addMail(_m_rankInfo.getActivity().getUSServer(), rewardInfo.getCid(), mailData, newContext);
            
            //日志数据
            LogRankRewardBO logBo = new LogRankRewardBO();
            logBo.setCid(getUSServer().getBM(), rewardInfo.getCid());
            logBo.setVersion(getUSServer().getBM(), 0);
            logBo.setIsMail(getUSServer().getBM(), true);
            logBo.setRankInstance(getUSServer().getBM(), getRank().getRankInstanceId());
            logBo.setRankId(getUSServer().getBM(), getRank().getRankId());
            logBo.setRankType(getUSServer().getBM(), rankRef.rank_type.ordinal());
            logBo.setRank(getUSServer().getBM(), rewardInfo.getRank());
            logBo.setIsLeader(getUSServer().getBM(), isLeader);
            CommLogDB.log(getUSServer().getBM(), logBo, newContext);
        }
        
        USLog.info(getUSServer(), "ActivityRankRewardMgr.sendNotDrawMail mail rewarded id:{} rank:{} count:{}", 
        		getRank().getRankInstanceId(), getRank().getRankId(), mailRewardedCount);
    }

    /**
     * 清空领取记录
     */
    public void clear()
    {
        _lock();
        try{
            _m_hasDrawRewardList.clear();
            _m_firstRankRewardInfo = null;
        }finally
        {
            _unlock();
        }

        HashMap<String, Object> condMap = new HashMap<>();
        condMap.put("activity_instance_id", _m_rankInfo.getActivity().getInstanceId());
        condMap.put("rank_id", _m_rankInfo.getRankId());

        _m_rankInfo.getActivity().getUSServer().getBM().getBM(ActivityRankRewardInfoBO.class).delAll(condMap);
    }
}
