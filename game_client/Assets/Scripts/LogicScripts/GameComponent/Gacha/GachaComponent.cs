using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 抽卡组件
    /// </summary>
    public class GachaComponent : _ANPBasicPlayerComponent
    {
        private List<GachaPoolInfo> _m_lPoolInfoList;//卡池信息列表
        private bool _m_bHasCommonItemChgRefreshRedTipTask;//是否有刷新红点任务
        [NotNull] private ObjectPool<NPCommonItem> _m_commonItemPool = new ObjectPool<NPCommonItem>(() => new NPCommonItem(), null, null, null, false, 1);//通用道具对象池
        [NotNull] private List<NPCommonItem> _m_lChgItemList = new List<NPCommonItem>();//变化的道具列表
        
        private int _m_freeDrawRedTipRefreshTaskSerialize = 0;//免费抽卡红点刷新任务序列号，用于取消过期定时任务
        private long _m_freeDrawNextRefreshTimeMs = 0;//免费抽卡红点下次刷新时间戳（ms）
        
        public GachaComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GACHA; } }
        public override ENPPlayerCompType[] dependCompList { get { return new ENPPlayerCompType[]{ ENPPlayerCompType.COMMON_ACTIVITY }; } }
        
        public override bool canPreInit { get { return true; } }
        public override void presendInitProtocol()
        {
            _reqGachaInit();
        }

        protected override void _dealInit()
        {
            if (_m_lPoolInfoList == null)
                _m_lPoolInfoList = new List<GachaPoolInfo>();
            _m_lPoolInfoList.Clear();

            foreach (var gachaPoolRefObj in GRefdataCoreMgr.instance.gachaPoolRefCore.refList)
            {
                if(gachaPoolRefObj == null)
                    continue;

                if (gachaPoolRefObj.activity_id <= 0)
                {
                    _m_lPoolInfoList.Add(new GachaPoolInfo(gachaPoolRefObj));
                    continue;
                }
                
                _ABaseActivityInfo aBaseActivity = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(gachaPoolRefObj.activity_id);

                if (null != aBaseActivity && aBaseActivity.isPlaying)
                    _m_lPoolInfoList.Add(new GachaPoolInfo(gachaPoolRefObj));
            }
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);
        }

        protected override void _onInitDone()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();

            // 所有组件初始化完成后, 刷新红点
            _refreshRedTip();
        }

        protected override void _onInitFail()
        {
            ALLog.Error("GachaComponent init Fail!!!");
        }

        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);
            
            _m_lPoolInfoList?.Clear();
            _m_lPoolInfoList = null;

            _m_bHasCommonItemChgRefreshRedTipTask = false;
            _pushBackAllCommonItem();
            _m_commonItemPool.Clear();

            // 使序列号失效，取消所有待执行的免费抽卡红点刷新任务
            _m_freeDrawRedTipRefreshTaskSerialize = ALSerializeOpMgr.next();
            _m_freeDrawNextRefreshTimeMs = 0;
        }

        /// <summary>
        /// 获取卡池信息
        /// </summary>
        /// <param name="_poolId"></param>
        /// <returns></returns>
        public GachaPoolInfo getGachaPoolInfo(long _poolId)
        {
            if (_m_lPoolInfoList == null || _m_lPoolInfoList.Count <= 0)
                return null;

            GachaPoolInfo poolInfo = null;
            for (int i = 0; i < _m_lPoolInfoList.Count; i++)
            {
                poolInfo = _m_lPoolInfoList[i];
                if (poolInfo != null && poolInfo.poolId == _poolId)
                    return _m_lPoolInfoList[i];
            }

            return null;
        }
        
        /// <summary>
        /// 更新或添加卡池信息
        /// </summary>
        /// <param name="_serverInfo"></param>
        /// <returns></returns>
        public GachaPoolInfo updataOrAddGachaPoolInfo(Common.GachaObj.Gacha_PoolInfo _serverInfo)
        {
            if (_serverInfo == null)
                return null;

            GachaPoolInfo poolInfo = getGachaPoolInfo(_serverInfo.getPoolId());
            if (poolInfo == null)
            {
                poolInfo = new GachaPoolInfo(_serverInfo);
                if (_m_lPoolInfoList == null)
                    _m_lPoolInfoList = new List<GachaPoolInfo>();
                _m_lPoolInfoList.Add(poolInfo);
            }
            else
            {
                poolInfo.update(_serverInfo);
            }

            return poolInfo;
        }

        #region 红点

        /// <summary>
        /// 创建CommonItem变化刷新红点任务
        /// </summary>
        private void _createCommonItemChgRefreshRedTipTask()
        {
            if (_m_bHasCommonItemChgRefreshRedTipTask)
                return;

            _m_bHasCommonItemChgRefreshRedTipTask = true;
            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                if (!_m_bHasCommonItemChgRefreshRedTipTask)
                    return;
                
                _m_bHasCommonItemChgRefreshRedTipTask = false;
                
                _refreshCommonItemChgRedTipTask();//刷新物品变化相关红点
                
            }, 1f);//延迟一会刷新红点, 防止频繁刷新
        }
        
        /// <summary>
        /// 刷新物品变化相关红点任务
        /// </summary>
        private void _refreshCommonItemChgRedTipTask()
        {
            // 根据_m_lChgItemList刷新红点
            if (_m_lChgItemList.Count > 0)
            {
                foreach (var poolRefObj in GRefdataCoreMgr.instance.gachaPoolRefCore.refList)
                {
                    if (poolRefObj == null)
                        continue;
                    
                    // 若十连抽需要的道具不在变化列表中, 则跳过
                    if (poolRefObj.ten_roll_cost != null && !_m_lChgItemList.Contains(poolRefObj.ten_roll_cost.item))
                        continue;
                    
                    _refreshCanTenRollRedTip(poolRefObj);
                }
                
                _pushBackAllCommonItem();
            }
        }
        
        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (_m_lPoolInfoList == null)
                return;

            _refreshCanTenRollRedTip();
            _refreshCanDrawCumulativeRewardRedTip();
            _refreshCanFreeDrawRedTip();
        }

        /// <summary>
        /// 刷新可十抽红点
        /// </summary>
        private void _refreshCanTenRollRedTip()
        {
            foreach (var poolRefObj in GRefdataCoreMgr.instance.gachaPoolRefCore.refList)
            {
                _refreshCanTenRollRedTip(poolRefObj);
            }
        }
        
        /// <summary>
        /// 刷新可十抽红点
        /// </summary>
        /// <param name="_poolRefObj"></param>
        private void _refreshCanTenRollRedTip(GachaPoolRefObj _poolRefObj)
        {
            if (_poolRefObj == null)
                return;

            // 检查十连抽消耗是否足够
            bool canTenRoll = _poolRefObj.ten_roll_cost != null && GCommon.isItemEnough(_poolRefObj.ten_roll_cost, false);
            RedTipMgr.instance.setCountByRefRedTipId(_poolRefObj.can_ten_roll_red_tip_id, canTenRoll ? 1 : 0);
        }

        private void _refreshCanDrawCumulativeRewardRedTip()
        {
            if(_m_lPoolInfoList == null)
                return;

            foreach (var poolInfo in _m_lPoolInfoList)
            {
                _refreshCanDrawCumulativeRewardRedTip(poolInfo);
            }
        }
        
        /// <summary>
        /// 刷新可领取累计抽卡次数奖励红点
        /// </summary>
        /// <param name="_poolInfo"></param>
        private void _refreshCanDrawCumulativeRewardRedTip(GachaPoolInfo _poolInfo)
        {
            if (_poolInfo == null || _poolInfo.poolRefObj == null)
                return;

            GachaPoolRefObj poolRefObj = _poolInfo.poolRefObj;
            if (poolRefObj.can_draw_cumulative_reward_red_tip_id <= 0)
                return;

            // 检查累计抽卡次数是否达到领取条件
            bool canDraw = poolRefObj.cumulative_reward_need_num > 0 && 
                          _poolInfo.cumulativeRewardTimes >= poolRefObj.cumulative_reward_need_num;
            RedTipMgr.instance.setCountByRefRedTipId(poolRefObj.can_draw_cumulative_reward_red_tip_id, canDraw ? 1 : 0);
        }
        
        /// <summary>
        /// 刷新可免费抽卡红点（遍历所有卡池），并安排下次倒计时刷新任务
        /// </summary>
        private void _refreshCanFreeDrawRedTip()
        {
            long miniNextRefreshTimeMs = 0;
            foreach (var poolRefObj in GRefdataCoreMgr.instance.gachaPoolRefCore.refList)
            {
                long tempTimeMs = _refreshCanFreeDrawRedTip(poolRefObj);
                // 收集最早的下次恢复时间
                if (tempTimeMs > 0 && (miniNextRefreshTimeMs == 0 || tempTimeMs < miniNextRefreshTimeMs))
                    miniNextRefreshTimeMs = tempTimeMs;
            }

            // 若新的下次刷新时间更早（或尚未安排），则重新安排定时任务
            if (miniNextRefreshTimeMs > 0 && (_m_freeDrawNextRefreshTimeMs == 0 || _m_freeDrawNextRefreshTimeMs > miniNextRefreshTimeMs))
            {
                _m_freeDrawNextRefreshTimeMs = miniNextRefreshTimeMs;
                _addFreeDrawRedTipNextRefreshTask();
            }
        }

        /// <summary>
        /// 刷新单个卡池可免费抽卡红点，返回下次CD恢复时间戳（次数为0且未达上限时返回，否则返回0）
        /// </summary>
        /// <param name="_poolRefObj"></param>
        /// <returns>下次恢复时间戳(ms)，无需安排刷新时返回0</returns>
        private long _refreshCanFreeDrawRedTip(GachaPoolRefObj _poolRefObj)
        {
            if (_poolRefObj == null || _poolRefObj.can_free_draw_red_tip_id <= 0)
                return 0;

            if (_poolRefObj.fixed_cd_id <= 0)
            {
                RedTipMgr.instance.setCountByRefRedTipId(_poolRefObj.can_free_draw_red_tip_id, 0);
                return 0;
            }

            // FixedCD次数大于0时，可免费抽卡，显示红点
            NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_poolRefObj.fixed_cd_id);
            bool canFreeDraw = cdInfo != null && cdInfo.getCount() > 0;
            RedTipMgr.instance.setCountByRefRedTipId(_poolRefObj.can_free_draw_red_tip_id, canFreeDraw ? 1 : 0);

            // 次数为0且未达上限时，返回下次恢复时间，由外部安排定时刷新
            if (!canFreeDraw && cdInfo != null && cdInfo.getCount() < cdInfo.getMaxCount())
                return cdInfo.getNextCalcTimeTagMs();

            return 0;
        }

        /// <summary>
        /// 安排免费抽卡红点的下次定时刷新任务
        /// </summary>
        private void _addFreeDrawRedTipNextRefreshTask()
        {
            // 更新序列号，使上一个待执行的定时任务失效
            int refreshSerialize = _m_freeDrawRedTipRefreshTaskSerialize = ALSerializeOpMgr.next();
            if (_m_freeDrawNextRefreshTimeMs <= 0)
                return;

            long leftTimeMs = _m_freeDrawNextRefreshTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                // 序列号不匹配说明任务已过期，直接丢弃
                if (refreshSerialize != _m_freeDrawRedTipRefreshTaskSerialize)
                    return;

                _m_freeDrawNextRefreshTimeMs = 0;
                _refreshCanFreeDrawRedTip();
            }, leftTimeMs / 1000f + 0.5f);//多0.5秒保证CD已触发
        }

        /// <summary>
        /// 放回所有通用道具对象
        /// </summary>
        private void _pushBackAllCommonItem()
        {
            foreach (var item in _m_lChgItemList)
            {
                _m_commonItemPool.Release(item);
            }
            _m_lChgItemList.Clear();
        }

        #endregion
        
        #region GC2GS

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqGachaInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_047_ReqGachaInit());
        }
        
        /// <summary>
        /// 请求抽卡
        /// </summary>
        public void reqGachaRoll(long _poolId, bool _isTen, Action<GS2GC_007_026_RetGachaRoll> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_026_ReqGachaRoll(_poolId, _isTen), 
                new CommonRequestCallbackProtocolDealer<GS2GC_007_026_RetGachaRoll>((_info) =>
                {
                    // 招募成功
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求自身抽卡记录
        /// </summary>
        public void reqGachaRollRecord(long _poolId, Action<GS2GC_007_027_RetGachaRollRecord> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_027_ReqGachaRollRecord(_poolId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_007_027_RetGachaRollRecord>((_info) =>
                {
                    // 招募成功
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求抽卡公屏记录
        /// <param name="_poolId"></param>
        /// <param name="_dbId">抽卡记录数据id 查询这个id之后的数据</param>
        /// </summary>
        public void reqGachaPublicRollRecord(long _poolId, long _dbId, Action<GS2GC_007_029_RetGachaPublicRollRecord> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_029_ReqGachaPublicRollRecord(_poolId, _dbId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_007_029_RetGachaPublicRollRecord>((_info) =>
                {
                    // 招募成功
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        /// <summary>
        /// 请求领取抽卡累计奖励
        /// </summary>
        /// <param name="_poolId"></param>
        /// <param name="_dealDone"></param>
        /// <param name="_dealFail"></param>
        public void reqDrawGachaCumulativeReward(long _poolId, Action<GS2GC_007_030_RetDrawGachaCumulativeReward> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_030_ReqDrawGachaCumulativeReward(_poolId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_007_030_RetDrawGachaCumulativeReward>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
        
        #endregion

        #region GS2GC

        /// <summary>
        /// 初始化协议回包
        /// </summary>
        public void retGachaInit(GS2GC_002_047_RetGachaInit _msg)
        {
            if (_msg == null)
            {
                Debug.LogError($"[GachaComponent retGachaInit] _msg is null");
                setInitDone();
                return;
            }

            if (_m_lPoolInfoList == null)
                _m_lPoolInfoList = new List<GachaPoolInfo>();

            if (_msg.getPoolList() != null)
            {
                GachaPoolInfo gachaPoolInfo = null;
                foreach (var item in _msg.getPoolList())
                {
                    if (item != null)
                    {
                        updataOrAddGachaPoolInfo(item);
                    }
                }
            }
            
            setInitDone();
        }

        /// <summary>
        /// 抽卡卡池信息变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGachaPoolChg(GS2GC_007_074_OnGachaPoolChg _msg)
        {
            if(_msg == null || !isInited)
                return;
            
            Common.GachaObj.Gacha_PoolInfo serverInfo = _msg.getPoolInfo();
            if(serverInfo == null)
                return;
            
            GachaPoolInfo poolInfo = getGachaPoolInfo(serverInfo.getPoolId());
            if(poolInfo == null)
            {
                poolInfo = new GachaPoolInfo(serverInfo);
                
                if(_m_lPoolInfoList == null)
                    _m_lPoolInfoList = new List<GachaPoolInfo>();
                _m_lPoolInfoList.Add(poolInfo);
            }
            else
            {
                poolInfo.update(serverInfo);
            }
            
            WinMsg.SendMsg(WinMsgType.ON_GACHA_POOL_INFO_CHG, poolInfo.poolId);
            
            // 卡池信息变更后刷新红点
            _refreshCanDrawCumulativeRewardRedTip(poolInfo);
        }

        #endregion

        #region 消息监听

        /// <summary>
        /// FixedCD数量变化（用于刷新免费抽卡红点）
        /// 注：倒计时自然恢复不发此消息，依赖定时任务处理；此处仅处理服务器推送导致的数量变化
        /// </summary>
        /// <param name="_objs">参数: _objs[0] 为 long(fixedCdRefId)</param>
        private void _onFixedCdCountChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is long _fixedCdId))
                return;

            // 检查是否有卡池使用该FixedCD，若有则全量刷新并重新安排定时任务
            foreach (var poolRefObj in GRefdataCoreMgr.instance.gachaPoolRefCore.refList)
            {
                if (poolRefObj != null && poolRefObj.fixed_cd_id == _fixedCdId)
                {
                    _refreshCanFreeDrawRedTip();
                    return;
                }
            }
        }

        /// <summary>
        /// 通用道具数量变化
        /// </summary>
        /// <param name="_objs">参数: _objs[0] 为 ENPItemType, _objs[1] 为 long(itemId)</param>
        private void _onCommonItemChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 2 || !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId))
                return;

            NPCommonItem chgCommonItem = _m_commonItemPool.Get();
            if (chgCommonItem == null)
                return;
            
            chgCommonItem.itemType = itemType;
            chgCommonItem.itemId = subId;
            _m_lChgItemList.Add(chgCommonItem);
            
            _createCommonItemChgRefreshRedTipTask();
        }

        #endregion
    }
}