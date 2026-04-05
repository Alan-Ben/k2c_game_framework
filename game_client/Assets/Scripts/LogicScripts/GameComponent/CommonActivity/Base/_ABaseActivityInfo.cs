using System;
using Common.ActivityEnum;
using Common.ActivityObj;
using Common.CommonFuncObj;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 活动信息数据
    /// </summary>
    public abstract class _ABaseActivityInfo
    {
        private bool _m_isInit;//是否初始化
        
        private long _m_lInstanceId;//活动实例id
        private long _m_usGroupId;//活动分组id
        private long _m_lActivityId;//活动id
        private long _m_lStartTimeMs;//活动开始时间戳
        private long _m_lEndTimeMs;//活动结束时间戳
        private long _m_lSettleTimeMs;//活动结算时间戳
        private long _m_lCloseTimeMs;//活动关闭时间戳
        private EActivityState _m_eActivityState;//活动当前状态
        private List<int> _m_lUSIdList;//参与跨服的usId列表 空列表则为单服活动
        private List<ActivityRankRushInfo> _m_lRankRushInfoList;//冲榜数据列表
        private List<ActivityStepRewardInfo> _m_lStepRewardInfoList;//阶段奖励数据列表
        private ActivityShopInfo _m_exchangeActivityShopInfo;//活动兑换商店信息
        private ActivityCrystalGiftPackInfo _m_crystalGiftPackInfo;//活动钻石礼包信息
        
        /// <summary>
        /// 活动实例id
        /// </summary>
        public long instanceId { get { return _m_lInstanceId; } }
        /// <summary>
        /// 活动分组id
        /// </summary>
        public long usGroupId { get { return _m_usGroupId; } }
        /// <summary>
        /// 活动id
        /// </summary>
        public long activityId { get { return _m_lActivityId; } }
        /// <summary>
        /// 活动开始时间戳
        /// </summary>
        public long startTimeMs { get { return _m_lStartTimeMs; } }
        /// <summary>
        /// 活动结束时间戳
        /// </summary>
        public long endTimeMs { get { return _m_lEndTimeMs; } }
        /// <summary>
        /// 活动结算时间戳
        /// </summary>
        public long settleTimeMs { get { return _m_lSettleTimeMs; } }
        /// <summary>
        /// 活动关闭时间戳
        /// </summary>
        public long closeTimeMs { get { return _m_lCloseTimeMs; } }
        /// <summary>
        /// 活动当前状态
        /// </summary>
        public EActivityState activityState { get { return _m_eActivityState; } }
        /// <summary>
        /// 参与跨服的usId列表 空列表则为单服活动
        /// </summary>
        public List<int> usIdList { get { return _m_lUSIdList; } }
        /// <summary>
        /// 冲榜数据列表
        /// </summary>
        public List<ActivityRankRushInfo> rankRushInfoList { get { return _m_lRankRushInfoList; } }
        /// <summary>
        /// 阶段奖励数据列表
        /// </summary>
        public List<ActivityStepRewardInfo> stepRewardInfoList { get { return _m_lStepRewardInfoList; } }
        /// <summary>
        /// 兑换商店数据
        /// </summary>
        public ActivityShopInfo exchangeActivityShopInfo { get { return _m_exchangeActivityShopInfo; } }
        /// <summary>
        /// 活动钻石礼包信息
        /// </summary>
        public ActivityCrystalGiftPackInfo crystalGiftPackInfo { get { return _m_crystalGiftPackInfo; } }
        /// <summary>
        /// 活动是否开启
        /// </summary>
        public bool isEnable { get { return _m_eActivityState is EActivityState.PLAYING or EActivityState.SETTLING or EActivityState.REWARDING; } }
        /// <summary>
        /// 是否进行中，不包括零将其
        /// </summary>
        public bool isPlaying { get { return _m_eActivityState == EActivityState.PLAYING; } }
        /// <summary>
        /// 是否是跨服活动
        /// </summary>
        public bool isCross { get { return _m_lUSIdList != null && _m_lUSIdList.Count > 1; } }

        public void init(Activity_Info _activityInfo)
        {
            if(_m_isInit)
                return;
            _m_isInit = true;
            
            updateActivityInfo(_activityInfo);

            _onInit();
        }
        
        public void discard()
        {
            if(!_m_isInit)
                return;
            _m_isInit = false;
            
            _onDiscard();
            
            _m_lInstanceId = 0;
            _m_usGroupId = 0;
            _m_lActivityId = 0;
            _m_lStartTimeMs = 0;
            _m_lEndTimeMs = 0;
            _m_lSettleTimeMs = 0;
            _m_lCloseTimeMs = 0;
            _m_eActivityState = EActivityState.CLOSED;
            _m_lUSIdList = null;
            _m_lRankRushInfoList = null;
            _m_lStepRewardInfoList = null;
            _m_exchangeActivityShopInfo = null;
            _m_crystalGiftPackInfo = null;
        }
        

        /// <summary>
        /// 更新活动信息
        /// </summary>
        /// <param name="_activityInfo"></param>
        public void updateActivityInfo(Activity_Info _activityInfo)
        {
            if (_activityInfo == null)
                return;

            _m_lInstanceId = _activityInfo.getInstanceId();
            _m_usGroupId = _activityInfo.getUsGroupId();
            _m_lActivityId = _activityInfo.getActivityId();
            _m_lStartTimeMs = _activityInfo.getStartTimeMs();
            _m_lEndTimeMs = _activityInfo.getEndTimeMs();
            _m_lSettleTimeMs = _activityInfo.getSettleTimeMs();
            _m_lCloseTimeMs = _activityInfo.getCloseTimeMs();
            _m_eActivityState = _activityInfo.getState();
            _m_lUSIdList = _activityInfo.getUsIdList();

            if (_m_lStartTimeMs > _m_lEndTimeMs || _m_lStartTimeMs > _m_lCloseTimeMs || _m_lEndTimeMs > _m_lCloseTimeMs)
            {
                Debug.LogError($"[CommonActivityInfo updateActivityInfo] _m_lInstanceId:{_m_lInstanceId} _m_lActivityId:{_m_lActivityId} 活动时间错误");
            }

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_lActivityId);
            if (activityMainRef == null)
                return;

            //设置兑换商店
            if(activityMainRef.exchange_shop_id > 0)
                _m_exchangeActivityShopInfo = new ActivityShopInfo(_m_lInstanceId, activityMainRef.exchange_shop_id);

            //设置钻石礼包
            if (activityMainRef.crystal_gift_pack_group_id > 0)
                _m_crystalGiftPackInfo = new ActivityCrystalGiftPackInfo(_m_lInstanceId, activityMainRef.crystal_gift_pack_group_id);


            //更新冲榜信息
            _updateRankRushList();
            //更新阶段奖励信息
            _updateStepRewardList();
        }

        /// <summary>
        /// 更新活动玩家信息
        /// </summary>
        /// <param name="_activityPlayerData"></param>
        public void upateActivityPlayerData(Activity_PlayerData _activityPlayerData)
        {
            if (_activityPlayerData == null)
                return;

            //更新兑换商店信息
            updateShopInfo(_activityPlayerData.getShopInfo());
            //更新钻石礼包信息
            updateCrystalGiftPackInfo(_activityPlayerData.getCrystalGiftPackInfo());
        }

        /// <summary>
        /// 更新阶段奖励信息
        /// </summary>
        /// <param name="_stepRewardInfo"></param>
        public void updateStepRewardInfo(Activity_StepRewardInfo _stepRewardInfo)
        {
            if (_stepRewardInfo == null || _m_lStepRewardInfoList == null)
                return;

            ActivityStepRewardInfo stepRewardInfo = getStepRewardInfo(_stepRewardInfo.getStepRewardId());
            if(stepRewardInfo != null)
                stepRewardInfo.updateInfo(_stepRewardInfo);
        }

        /// <summary>
        /// 更新兑换商店信息
        /// </summary>
        /// <param name="_shopInfo"></param>
        public void updateShopInfo(Activity_ShopInfo _shopInfo)
        {
            if (_shopInfo == null)
                return;

            _m_exchangeActivityShopInfo?.updateInfo(_shopInfo);
        }

        /// <summary>
        /// 更新钻石礼包信息
        /// </summary>
        /// <param name="_giftPackInfo"></param>
        public void updateCrystalGiftPackInfo(CrystalGiftPack_Info _giftPackInfo)
        {
            if (_giftPackInfo == null)
                return;

            _m_crystalGiftPackInfo?.updateInfo(_giftPackInfo);
        }

        /// <summary>
        /// 更新活动状态
        /// </summary>
        /// <param name="_state"></param>
        public void updateState(EActivityState _state)
        {
            if(_m_eActivityState == _state)
                return;
            
            _m_eActivityState = _state;
            
            _updateRankRushList();
            _updateStepRewardList();

            if (_m_eActivityState == EActivityState.PLAYING)
            {
                _onActivityStart();
            }
            else if (_m_eActivityState == EActivityState.REWARDING)
            {
                _onActivityEnd();
            }
            else if (_m_eActivityState == EActivityState.CLOSED)
            {
                _onActivityClosed();
            }
        }

        public ActivityStepRewardInfo getStepRewardInfo(long _stepRewardSetId)
        {
            if (_m_lStepRewardInfoList == null)
                return null;

            foreach (var stepRewardInfo in _m_lStepRewardInfoList)
            {
                if (stepRewardInfo != null && stepRewardInfo.stepRewardSetId == _stepRewardSetId)
                {
                    return stepRewardInfo;
                }
            }

            return null;
        }
        
        //更新冲榜信息
        private void _updateRankRushList()
        {
            if (isEnable)
            {
                if (_m_lRankRushInfoList == null)
                    _m_lRankRushInfoList = new List<ActivityRankRushInfo>();

                List<ActivityRankRushRefObj> rankRushRefList = new List<ActivityRankRushRefObj>();
                GRefdataCoreMgr.instance.getRankRushRefListByActivityId(_m_lActivityId, rankRushRefList);
                for (int i = 0; i < rankRushRefList.Count; i++)
                {
                    //查找是否已经存在冲榜信息
                    bool isFind = false;
                    for (int j = 0; j < _m_lRankRushInfoList.Count; j++)
                    {
                        if(_m_lRankRushInfoList[j]?.activityRankRushRefObj?.id == rankRushRefList[i]?.id)
                        {
                            isFind = true;
                            break;
                        }
                    }
                    //如果没有找到，则添加新的冲榜信息
                    if (!isFind)
                    {
                        _m_lRankRushInfoList.Add(new ActivityRankRushInfo(this, rankRushRefList[i]));
                    }
                }
            }
            else
            {
                //活动结束，清空冲榜信息
                _m_lRankRushInfoList?.Clear();
            }

            //刷新红点
            NPPlayer.instance.commonActivityComp.refreshRankRushRewardRedTip();
        }

        //更新阶段奖励信息
        private void _updateStepRewardList()
        {
            if (isEnable)
            {
                if (_m_lStepRewardInfoList == null)
                    _m_lStepRewardInfoList = new List<ActivityStepRewardInfo>();

                GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_lActivityId);
                if (activityMainRef != null && activityMainRef.step_reward_set_id_list != null)
                {
                    for (int i = 0; i < activityMainRef.step_reward_set_id_list.Count; i++)
                    {
                        long stepRewardSetId = activityMainRef.step_reward_set_id_list[i];
                        //查找是否已经存在阶段奖励信息
                        bool isFind = false;
                        for (int j = 0; j < _m_lStepRewardInfoList.Count; j++)
                        {
                            if (_m_lStepRewardInfoList[j]?.stepRewardSetId == stepRewardSetId)
                            {
                                isFind = true;
                                break;
                            }
                        }
                        //如果没有找到，则添加新的阶段奖励信息
                        if (!isFind)
                        {
                            _m_lStepRewardInfoList.Add(new ActivityStepRewardInfo(_m_lInstanceId, stepRewardSetId));
                        }
                    }
                }
            }
            else
            {
                //活动结束，清空阶段奖励信息
                _m_lStepRewardInfoList?.Clear();
            }

            //刷新红点
            NPPlayer.instance.commonActivityComp.refreshStepRewardRedTip();
        }
        
        //初始化子对象
        public void subInit(Action _doneDelegate)
        {
            _onSubDataInit(_doneDelegate);
        }
        
        protected abstract void _onActivityStart();

        protected abstract void _onActivityEnd();

        protected abstract void _onActivityClosed();
        
        //子对象数据初始化
        protected abstract void _onInit();
        
        //子对象数据初始化
        protected abstract void _onSubDataInit(Action _doneDelegate);
        //销毁
        protected abstract void _onDiscard();
    }
}