
using System.Collections.Generic;
using System;
using ALPackage;
using Common.ChapterEnum;
using Common.ChapterObj;
using CommonEnum;
using GC2GS.p016_ChapterOp;
using GS2GC.p002_InitOp;
using GS2GC.p016_ChapterOp;
using JetBrains.Annotations;
using NPEnum;
using Unity.Mathematics;

namespace GOE
{
    /// <summary>
    /// 关卡数据组件
    /// </summary>
    public partial class PlayerChapterComponent : _ANPBasicPlayerComponent
    {
        private long _m_curChapterId;
        private int _m_curPointId;
        private int _m_curNodeIndex;
        private ChapterRefObj _m_chapterRefObj;
        private long _m_curChapterEventId;
        private List<long> _m_lHasDrawPlotRewardList;//已经领取的节剧情奖励列表
        
        [NotNull]private ChapterForwardLogicMgr _m_forwardLogicMgr;
        
        [NotNull]private Dictionary<EChapterInspireType, int> _m_inspireInfoDic = new Dictionary<EChapterInspireType, int>();
        
        private PlayerChapterSetting _m_playerChapterSetting;
        
        public PlayerChapterComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_forwardLogicMgr = new ChapterForwardLogicMgr();
            _m_redTipDealer = new RedTipDealer(this);
        }
        
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.Chapter; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }

        public long curChapterId { get { return _m_curChapterId; } }
        public int curPointId { get { return _m_curPointId; } }
        public int curNodeIndex { get { return _m_curNodeIndex; }}
        public int nextPointId { get { return _m_curPointId + 1; } }
        public ChapterRefObj chapterRefObj { get { return _m_chapterRefObj; } }
        [NotNull] public ChapterForwardLogicMgr forwardLogicMgr { get { return _m_forwardLogicMgr; } }
        public long curChapterEventId { get { return _m_curChapterEventId; } }
        public PlayerChapterSetting playerChapterSetting { get { return _m_playerChapterSetting; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqChapterInit();
        }

        protected override void _dealInit()
        {
            for (int i = 0; i < EChapterInspireTypeComparer.g_iEnumCount; i++)
            {
                _m_inspireInfoDic.Add((EChapterInspireType)i , 0);
            }
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            if (_m_forwardLogicMgr != null) 
                _m_forwardLogicMgr.init();

            _m_playerChapterSetting = new PlayerChapterSetting(NPPlayer.instance.playerInfo.CID);
            _m_playerChapterSetting.init();
            
            // 初始化红点管理器
            if (_m_redTipDealer != null)
                _m_redTipDealer.init();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerChapterComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_curChapterId = 0;
            _m_curPointId = 0;
            _m_curNodeIndex = 0;
            _m_chapterRefObj = null;
            _m_inspireInfoDic.Clear();

            if (_m_forwardLogicMgr != null) 
                _m_forwardLogicMgr.discard();

            _m_playerChapterSetting = null;
            
            // 清理红点管理器
            if (_m_redTipDealer != null)
                _m_redTipDealer.clear();
        }
        
        
        /// <summary>
        /// 是否通过指定关卡
        /// </summary>
        public bool chapterIsPass(long _chapterId)
        {
            return _chapterId > _m_curChapterId;
        }
        
        /// <summary>
        /// 获取已经鼓舞的次数
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public int getInspireTimes(EChapterInspireType _type)
        {
            if (_m_inspireInfoDic.TryGetValue(_type, out int times))
                return times;
            return 0;
        }
        
        /// <summary>
        /// 当前是不是boss战
        /// </summary>
        public bool curIsBossPoint()
        {
            ChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(curChapterId);
            if (null == chapterRefObj)
                return false;
            
            //当前是是boss战
            if(chapterRefObj.point_count == NPPlayer.instance.chapterComp.nextPointId)
                return true;
            return false;
        }

        //获取当前战力（经过鼓舞后）
        public long getTotalPower()
        {
            //大臣总战力
            long basePower = NPPlayer.instance.heroComponent.totalPower;
            
            //需要算上鼓舞的加成
            int totalIncreasePer = 0;
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.gold_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.GOLD];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.crystal_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.CRYSTAL];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.item_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.ITEM];
            //总战力
            long totalPower = basePower * (10000 + totalIncreasePer) / 10000;

            return totalPower;
        }
        
        //获取鼓舞加成
        public long getInspirePower()
        {
            //大臣总战力
            long basePower = NPPlayer.instance.heroComponent.totalPower;
            
            //需要算上鼓舞的加成
            int totalIncreasePer = 0;
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.gold_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.GOLD];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.crystal_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.CRYSTAL];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.item_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.ITEM];
            
            return basePower * totalIncreasePer / 10000;
        }
        
        /// <summary>
        /// 获得鼓舞加成比例
        /// </summary>
        /// <returns></returns>
        public float getInspirePowerRate()
        {
            //需要算上鼓舞的加成
            int totalIncreasePer = 0;
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.gold_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.GOLD];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.crystal_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.CRYSTAL];
            totalIncreasePer += GRefdataCoreMgr.instance.npGeneral.item_inspire_increase_power_ratio_per * _m_inspireInfoDic[EChapterInspireType.ITEM];

            return (10000 + totalIncreasePer) / 10000f;
        }
        
        /// <summary>
        /// 计算金币消耗减免比例 金币减免消耗比例=（玩家战力-关卡战力）/关卡战力 * 金币消耗放大倍数
        /// 其中, 金币消耗放大倍数根据[（玩家战力-关卡战力）/关卡战力]的计算结果所在范围进行取值
        /// 返回值乘了10000
        /// </summary>
        public double getGoldCostReduceRate()
        {
            if (null == _m_chapterRefObj)
                return 0;
            
            //前进消耗金币比例范围
            WCGPairInt forwardGoldCostRatioRange = GRefdataCoreMgr.instance.npGeneral.forward_gold_cost_ratio_range;
            //计算目标位置所需战力
            long needPower = _m_chapterRefObj.calNeedPower(nextPointId);
            //计算减免比例
            double reduceRate = math.ceil((double)(NPPlayer.instance.heroComponent.totalPower - needPower) / needPower * 10000);
            //战力比例限制在范围内
            long powerRate = forwardGoldCostRatioRange.limit(reduceRate);

            //金币消耗放大倍数
            int costMultipleRate = GRefdataCoreMgr.instance.getCostMultipleRate((int)powerRate);
            
            return costMultipleRate / 10000d * powerRate;
        }
        
        //写死消耗类型是金币
        [NotNull]private NPCommonCostItem _m_costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (int)ECurrency.SILVER, 0);
        /// <summary>
        /// 计算金币消耗 关卡基础值*递增比例*（1 - 金币消耗减免比例）
        /// </summary>
        public NPCommonCostItem getGoldCost()
        {
            if (null == _m_chapterRefObj || curIsBossPoint())
            {
                _m_costItem.setCount(0);
                return _m_costItem;
            }
            
            //计算金币消耗
            long goldCost = _m_chapterRefObj.calNeedCost(nextPointId);
            //计算减免比例
            double reduceRate = getGoldCostReduceRate();

            //计算最终金币消耗
            long finalCost = (long) math.ceil(goldCost * (1 - reduceRate / 10000d));
            
            //计算减免后的金币消耗
            _m_costItem.setCount(finalCost);

            return _m_costItem;
        }
        
        
        //获取当前金币鼓舞消耗
        public NPCommonCostItem getCurrentInspireCost()
        {
            return getInspireGoldCost(_m_chapterRefObj, _m_inspireInfoDic[EChapterInspireType.GOLD]);
        }
        
        //获取金币鼓舞消耗
        public NPCommonCostItem getInspireGoldCost(ChapterRefObj _ref, int _times)
        {
            long costNum = _ref.gold_inspire_base_value;
            
            TimesPriceRefObj costRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(GRefdataCoreMgr.instance.npGeneral.gold_inspire_calculate_ratio_time_price_id, _times + 1);
            costNum *= costRef.cost_item_formula.CalculateVariableResult(null);

            return new NPCommonCostItem(ENPItemType.CURRENCY, (int) ECurrency.SILVER, costNum);
        }

        /// <summary>
        /// 获取当前node的进度
        /// </summary>
        /// <returns></returns>
        public float getCurNodeFade()
        {
            if (null == _m_chapterRefObj)
                return 0;

            int lastNodePoint = 0;

            CommonIntLongInfo item = null;
            for (int i = 0; i < _m_chapterRefObj.nodeList.Count; i++)
            {
                item = _m_chapterRefObj.nodeList[i];
                if (_m_curPointId >= item.intValue)
                {
                    lastNodePoint = item.intValue;
                    continue;
                }

                return (float)(_m_curPointId - lastNodePoint) / (item.intValue - lastNodePoint);
            }
            
            return 0f;
        }

        public float getTotalFade()
        {
            if (null == _m_chapterRefObj)
                return 0;
            
            return (float)_m_curPointId / _m_chapterRefObj.point_count;
        }
        
        //获取当前node
        public _IChapterNodeStyle getCurNodeStyleRef(out NPGGoIndex _bgGoIndex)
        {
            _bgGoIndex = null;
            if (null == _m_chapterRefObj)
                return null;
            
            return _m_chapterRefObj.getChapterNodeStyleByIndexId(curNodeIndex, out _bgGoIndex);
        }
        
        /// <summary>
        /// 是否已领取剧情奖励
        /// </summary>
        /// <param name="plotId"></param>
        /// <returns></returns>
        public bool hasDrawPlotReward(long plotId)
        {
            if (_m_lHasDrawPlotRewardList == null)
                return false;

            return _m_lHasDrawPlotRewardList.Contains(plotId);
        }

        /// <summary>
        /// 是否有未领取的剧情奖励
        /// </summary>
        /// <returns></returns>
        public bool hasUnclaimedPlotReward()
        {
            if (_m_chapterRefObj == null)
                return false;

            foreach (var stageRefObj in GRefdataCoreMgr.instance.chapterStageRefCore.refList)
            {
                if(stageRefObj == null || stageRefObj.plot_id_list == null || stageRefObj.plot_id_list.Count <= 0
                   // 遍历的阶段的起始章节 > 当前所处章节, 说明当前还未到达遍历的章节, 不检查
                   || stageRefObj.start_chapter_id > _m_chapterRefObj.chapter_id)
                    continue;

                foreach (var plotId in stageRefObj.plot_id_list)
                {
                    ChapterStagePlotRefObj plotRefObj = GRefdataCoreMgr.instance.chapterStagePlotRefCore.getRef(plotId);
                    if (plotRefObj == null)
                        continue;

                    // 检查剧情是否已解锁（解锁条件满足）, 若还未解锁, 不进行判断
                    if (plotRefObj.unlock_condition != null && !plotRefObj.unlock_condition.isNoConditionOrEnable(null))
                        continue;

                    // 若还未领取奖励, 直接返回true
                    if (!hasDrawPlotReward(plotId))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void init(GS2GC.p002_InitOp.GS2GC_002_019_RetChapterInit _msg)
        {
            if (null == _msg)
                return;

            _m_curChapterId = _msg.getPosInfo().getChapterId();
            _m_curPointId = _msg.getPosInfo().getPoint();
            _m_curChapterEventId = _msg.getEventInfo().getEventId();
            _m_chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(_m_curChapterId);
            _m_lHasDrawPlotRewardList = _msg.getHadDrawPlotRewardList();

            if (null != _m_chapterRefObj)
            {
                _m_curNodeIndex = _m_chapterRefObj.getNodeIndexByPoint(_m_curPointId);
            }
            
            foreach (Chapter_SingleInspireInfo chapterSingleInspireInfo in _msg.getInspireInfo().getInspireList())
            {
                if(null == chapterSingleInspireInfo)
                    continue;
                
                _m_inspireInfoDic[chapterSingleInspireInfo.getType()] = chapterSingleInspireInfo.getInspireTimes();
            }
            
            setInitDone();
        }

        
        /// <summary>
        /// 请求关卡初始化
        /// </summary>
        public void reqChapterInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_019_ReqChapterInit());
        }

        /// <summary>
        /// 请求关卡前进
        /// </summary>
        /// <param name="_sucAction"></param>
        public void reqChapterForward(bool _isAKey, Action<GS2GC_016_001_RetChapterForward> _sucAction = null, Action _failAction = null)
        {
            reqChapterForward(_m_curChapterId, nextPointId, _isAKey, _sucAction, _failAction);
        }
        
        /// <summary>
        /// 请求关卡前进
        /// </summary>
        /// <param name="_sucAction"></param>
        public void reqChapterForward(long _chapterId, int _point, bool _isAKey, Action<GS2GC_016_001_RetChapterForward> _sucAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_016_ChapterOp.make_001_ReqChapterForward(_chapterId, _point, _isAKey),
                new CommonRequestCallbackProtocolDealer<GS2GC.p016_ChapterOp.GS2GC_016_001_RetChapterForward>((info) =>
                {
                    if (null == info)
                        return;

                    if (_sucAction != null)
                        _sucAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    
                    if (_failAction != null)
                        _failAction();
                }));
        }
        
        /// <summary>
        /// 请求打boss
        /// </summary>
        public void reqChapterFightBoss(long _chapterId, Action<GS2GC_016_002_RetChapterFightBoss> _sucAction ,  Action<int> _failAction )
        {
            NPGSClientListener.sendRequestByLog(GSWriter_016_ChapterOp.make_002_ReqChapterFightBoss(_chapterId),
                new CommonRequestCallbackProtocolDealer<GS2GC.p016_ChapterOp.GS2GC_016_002_RetChapterFightBoss>((info) =>
                {
                    if (null == info)
                        return;

                    if (_sucAction != null)
                        _sucAction(info);
                },
                    (_errCode) =>
                {
                    if (_failAction != null)
                        _failAction(_errCode);
                }));
        }
        
        /// <summary>
        /// 请求关卡加buff
        /// </summary>
        public void reqChapterFightBossInspire(EChapterInspireType _type, Action _sucAction, Action _failAction, bool _needFailTip)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_016_ChapterOp.make_003_ReqChapterFightBossInspire(_type),
                new CommonRequestCallbackProtocolDealer<GS2GC.p016_ChapterOp.GS2GC_016_003_RetChapterFightBossInspire>((info) =>
                {
                    if (null == info)
                        return;

                    if (_sucAction != null)
                        _sucAction();
                }, (_errCode) =>
                {
                    //上浮提示
                    if(_needFailTip)
                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    
                    if (_failAction != null)
                        _failAction();
                }));
        }

        /// <summary>
        /// 请求处理关卡奖励事件
        /// </summary>
        /// <param name="_optionId"></param>
        /// <param name="_complete"></param>
        public void reqDealChapterRewardEvent(Action<bool, GS2GC_016_004_RetDealChapterRewardEvent> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_016_004_ReqDealChapterRewardEvent(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_016_004_RetDealChapterRewardEvent>(_complete));
        }
        
        /// <summary>
        /// 请求处理关卡选择事件
        /// </summary>
        /// <param name="_optionId"></param>
        /// <param name="_complete"></param>
        public void reqDealChapterChoiceEvent(long _optionId, Action<bool, GS2GC_016_005_RetDealChapterChoiceEvent> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_016_005_ReqDealChapterChoiceEvent(_optionId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_016_005_RetDealChapterChoiceEvent>(_complete));
        }
        
        /// <summary>
        /// 请求处理关卡派遣事件
        /// </summary>
        /// <param name="_heroList"></param>
        /// <param name="_complete"></param>
        public void reqDealChapterDispatchEvent(List<long> _heroList, Action<bool, GS2GC_016_006_RetDealChapterDispatchEvent> _complete)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_016_006_ReqDealChapterDispatchEvent(_heroList), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_016_006_RetDealChapterDispatchEvent>(_complete));
        }

        /// <summary>
        /// 请求领取章剧情奖励
        /// </summary>
        public void reqDrawChapterPlotReward(List<long> plotIdList, Action<bool, GS2GC_016_007_RetDrawChapterPlotReward> _onRet)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_016_007_ReqDrawChapterPlotReward(plotIdList), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_016_007_RetDrawChapterPlotReward>(_onRet));
        }

        /// <summary>
        /// 玩家关卡位置变化
        /// </summary>
        public void onChapterPosChg(GS2GC_016_051_OnChapterPosChg _msg)
        {
            if (null == _msg)
                return;

            _m_curChapterId = _msg.getPosInfo().getChapterId();
            _m_curPointId = _msg.getPosInfo().getPoint();
            _m_chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(_m_curChapterId);

            if (null != _m_chapterRefObj)
            {
                _m_curNodeIndex = _m_chapterRefObj.getNodeIndexByPoint(_m_curPointId);
            }
            
            // 关卡位置变化后刷新剧情入口红点
            if (_m_redTipDealer != null)
                _m_redTipDealer.refreshPlotEntranceRedTip();
            
            WinMsg.SendMsg(WinMsgType.ON_CHAPTER_POS_CHG);
        }
        
        /// <summary>
        /// 作弊修改玩家关卡位置变化
        /// </summary>
        public void onGMChapterPosChg(GS2GC_016_054_OnChapterGmPosChg _msg)
        {
            if (null == _msg)
                return;

            _m_curChapterId = _msg.getPosInfo().getChapterId();
            _m_curPointId = _msg.getPosInfo().getPoint();
            _m_chapterRefObj = GRefdataCoreMgr.instance.chapterRefCore.getRef(_m_curChapterId);
            
            if (null != _m_chapterRefObj)
            {
                _m_curNodeIndex = _m_chapterRefObj.getNodeIndexByPoint(_m_curPointId);
            }
            
            // 关卡位置变化后刷新剧情入口红点
            if (_m_redTipDealer != null)
                _m_redTipDealer.refreshPlotEntranceRedTip();
            
            WinMsg.SendMsg(WinMsgType.ON_CHAPTER_POS_CHG_CHEAT);
            WinMsg.SendMsg(WinMsgType.ON_CHAPTER_POS_CHG);
        }
        
        /// <summary>
        /// 关卡鼓舞信息变更
        /// </summary>
        public void onChapterInspireChg(GS2GC_016_052_OnChapterInspireChg _msg)
        {
            if (null == _msg)
                return;

            foreach (Chapter_SingleInspireInfo chapterSingleInspireInfo in _msg.getInspireInfo().getInspireList())
            {
                if(null == chapterSingleInspireInfo)
                    continue;
                
                _m_inspireInfoDic[chapterSingleInspireInfo.getType()] = chapterSingleInspireInfo.getInspireTimes();
            }
        }
        /// <summary>
        /// 玩家关卡位置变化
        /// </summary>
        public void onChapterEventChg(GS2GC_016_053_OnChapterEventChg _msg)
        {
            if (null == _msg)
                return;
            _m_curChapterEventId = _msg.getEventInfo().getEventId();
        }

        /// <summary>
        /// 关卡 - 章剧情奖励领取
        /// </summary>
        /// <param name="_msg"></param>
        public void OnChapterPlotRewardDraw(GS2GC_016_055_OnChapterPlotRewardDraw _msg)
        {
            if (_msg == null)
                return;

            if (_m_lHasDrawPlotRewardList == null)
                _m_lHasDrawPlotRewardList = new List<long>();
            
            List<long> plotIdList = _msg.getPlotIdList();
            if (plotIdList != null)
            {
                foreach (var plotId in plotIdList)
                {
                    _m_lHasDrawPlotRewardList.Add(plotId);
                }
            }
            
            // 刷新剧情入口红点
            if (_m_redTipDealer != null)
                _m_redTipDealer.refreshPlotEntranceRedTip();
            
            WinMsg.SendMsg(WinMsgType.ON_CHAPTER_PLOT_REWARD_DRAW, plotIdList);
        }
    }
}