using System;
using System.Collections.Generic;
using Common.TowerObj;
using CommonEnum;
using GC2GS.p023_ArenaOp;
using GS2GC.p002_InitOp;
using GS2GC.p023_ArenaOp;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class TowerComponent : _ANPBasicPlayerComponent
    {
        public TowerComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.TOWER; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        private long _m_curChapterId; // 当前章节
        private int _m_curLevel; // 章节关卡
        private int _m_curEarningAddPer; // 当前塔赚速加成百分比
        
        private TowerPosInfo _m_activatePosInfo; // 当前激活的塔位置
        private TowerPosInfo _m_maxAchievedPosInfo; // 当前最大已达成的塔位置
        [NotNull]private List<long> _m_hadDrawResearchRewardList = new List<long>(); // 已领取研究奖励列表
        
        public long curChapterId => _m_curChapterId; // 当前章节
        public int curLevel => _m_curLevel; // 当前Stage中的索引 1-10
        
        public long towerPassedChapter => GRefdataCoreMgr.instance.getTowerPreChapterId(_m_curChapterId);

        // 当前所在的所用关卡中的关卡数
        public long towerCurTotalLevel => GRefdataCoreMgr.instance.getTowerCurTotalLevel(_m_curChapterId, _m_curLevel);
        // 当前塔赚速加成百分比
        public int towerEarningAddPer => _m_curEarningAddPer; 
        
        public TowerPosInfo activatePosInfo => _m_activatePosInfo; // 当前激活的塔位置
        public TowerPosInfo maxAchievedPosInfo => _m_maxAchievedPosInfo; // 当前最大已达成的塔位置
        
        

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqTowerInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_hadDrawResearchRewardList?.Clear();
        }

        /// <summary>
        /// 是否已领取研究奖励
        /// </summary>
        /// <param name="_chapterId"></param>
        /// <returns></returns>
        public bool hasGetResearchReward(long _chapterId)
        {
            return _m_hadDrawResearchRewardList.Contains(_chapterId);
        }

        /// <summary>
        /// 是否可以领取研究奖励
        /// </summary>
        /// <param name="_chapterId"></param>
        /// <returns></returns>
        public bool canGetResearchReward(long _chapterId)
        {
            if (_m_maxAchievedPosInfo == null)
                return false;
            if (_m_maxAchievedPosInfo.chapterId > _chapterId)
                return true;
            if (_m_maxAchievedPosInfo.chapterId == _chapterId)
            {
                TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapterId);
                // 如果到了章节的最后一关，则返回true
                if (chapterRefObj != null && _m_maxAchievedPosInfo.level == chapterRefObj.level_count)
                    return true;
            }

            return false;
        }

        public ETowerResearchActiveType getLevelActiveState(long _chapterId, int _level)
        {
            if (_m_maxAchievedPosInfo.chapterId < _chapterId ||
                (_m_maxAchievedPosInfo.chapterId == _chapterId &&
                 _level > _m_maxAchievedPosInfo.level))
            {
                return ETowerResearchActiveType.NOT_ACTIVATE;
            }
            else
            {
                if (_m_activatePosInfo.chapterId > _chapterId ||
                    (_m_activatePosInfo.chapterId == _chapterId &&
                     _level <= _m_activatePosInfo.level))
                {
                    return ETowerResearchActiveType.HAS_ACTIVATE;
                }
                else
                {
                    return ETowerResearchActiveType.CAN_ACTIVATE;
                }
            }
        }

        public ETowerResearchActiveType getChapterActiveState(long _chapterId)
        {
            if(_chapterId > _m_maxAchievedPosInfo.chapterId)
            {
                return ETowerResearchActiveType.NOT_ACTIVATE;
            }
            else if (_chapterId == _m_maxAchievedPosInfo.chapterId)
            {
                if (_chapterId < _m_activatePosInfo.chapterId)
                {
                    return ETowerResearchActiveType.HAS_ACTIVATE;
                }
                else if (_chapterId == _m_activatePosInfo.chapterId)
                {
                    // 如果是当前激活得章节，则遍历章节得所有研究，只要有一个研究未激活，则可以激活，否则为不可激活
                    TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapterId);
                    foreach (var researchRef in chapterRefObj.research_list)
                    {
                        if(researchRef != null && _m_activatePosInfo.level < researchRef.level && researchRef.level <=_m_maxAchievedPosInfo.level)
                            return ETowerResearchActiveType.CAN_ACTIVATE;
                    }
                    return ETowerResearchActiveType.HAS_ACTIVATE;
                }
                else // _chapterId > _m_activatePosInfo.chapterId && _chapterId == _m_maxAchievedPosInfo.chapterId
                {
                    TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapterId);
                    foreach (var researchRef in chapterRefObj.research_list)
                    {
                        if(researchRef != null && researchRef.level < _m_maxAchievedPosInfo.level)
                            return ETowerResearchActiveType.CAN_ACTIVATE;
                    }
                    return ETowerResearchActiveType.NOT_ACTIVATE;
                }
            }
            else // _chapterId < _m_maxAchievedPosInfo.chapterId
            {
                if (_chapterId < _m_activatePosInfo.chapterId) //&& _chapterId < _m_maxAchievedPosInfo.chapterId
                {
                    return ETowerResearchActiveType.HAS_ACTIVATE;
                }
                else if (_chapterId == _m_activatePosInfo.chapterId) //&& _chapterId < _m_maxAchievedPosInfo.chapterId
                {
                    // 如果是当前激活得章节，则遍历章节得所有研究，只要有一个研究未激活，则可以激活，否则为不可激活
                    TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_chapterId);
                    foreach (var researchRef in chapterRefObj.research_list)
                    {
                        if(researchRef.level > _m_activatePosInfo.level)
                            return ETowerResearchActiveType.CAN_ACTIVATE;
                    }

                    return ETowerResearchActiveType.HAS_ACTIVATE;
                }
                else // _m_activatePosInfo.chapterId < _chapterId && _chapterId < _m_maxAchievedPosInfo.chapterId
                {
                    return ETowerResearchActiveType.CAN_ACTIVATE;
                }
            }
        }

        private void _refreshRedTip()
        {
            int count = 0;
            
            int canGetRewardCount = 0;
            if (_m_maxAchievedPosInfo != null)
            {
                // 检查是否有未领取的章节研究奖励
                foreach (TowerChapterRefObj chapterRef in GRefdataCoreMgr.instance.towerChapterRefCore.refList)
                {
                    if (_m_maxAchievedPosInfo.chapterId >= chapterRef.id)
                    {
                        if (canGetResearchReward(chapterRef.id) && !_m_hadDrawResearchRewardList.Contains(chapterRef.id))
                            canGetRewardCount++;
                    }
                }
                
                if (canGetRewardCount > 0)
                    count += canGetRewardCount;
                
                // 检查是否有未激活的关卡
                if (_m_activatePosInfo != null)
                {
                    foreach (TowerChapterRefObj chapterRef in GRefdataCoreMgr.instance.towerChapterRefCore.refList)
                    {
                        if(chapterRef == null)
                            continue;
                        if(getChapterActiveState(chapterRef.id) == ETowerResearchActiveType.CAN_ACTIVATE)
                            count++;
                    }
                }
            }
            
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_TOWER_RESEARCH, count);
        }

        //累计激活得迷宫研究加成次数
        public int getActiveResearchCount()
        {
            int activeResearchCount = 0;
            if (_m_activatePosInfo == null)
                return activeResearchCount;
            foreach (TowerChapterRefObj chapterRef in GRefdataCoreMgr.instance.towerChapterRefCore.refList)
            {
                if (chapterRef == null) continue;
                if (_m_activatePosInfo.chapterId > chapterRef.id)
                {
                    if (chapterRef.research_list != null) 
                        activeResearchCount += chapterRef.research_list.Count;
                }
                else if (_m_activatePosInfo.chapterId == chapterRef.id)
                {
                    if (chapterRef.research_list != null)
                        foreach (var researchRef in chapterRef.research_list)
                        {
                            if (researchRef != null && _m_activatePosInfo.level >= researchRef.level)
                                activeResearchCount++;
                        }
                }
            }
            return activeResearchCount;
        }
        
        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqTowerInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_044_ReqTowerInit());
        }
        
        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retTowerInit(GS2GC_002_044_RetTowerInit _info)
        {
            Tower_PosInfo towerInfo = _info.getPosInfo();
            _m_curChapterId = towerInfo.getChapterId();
            _m_curLevel = towerInfo.getLevel();
            _m_curEarningAddPer = _info.getBuildingProfitAddPer();
            _m_activatePosInfo = new TowerPosInfo(_info.getHadActiveResearchPos());
            _m_maxAchievedPosInfo = new TowerPosInfo(_info.getHighestPosHadReach());
            _m_hadDrawResearchRewardList.Clear();
            List<long> hadDrawResearchRewardList = _info.getHadDrawResearchRewardList();
            if(hadDrawResearchRewardList != null)
                _m_hadDrawResearchRewardList.AddRange(hadDrawResearchRewardList);
            _refreshRedTip();
            setInitDone();
        }

        public void reqChallengeTower(long _chapterId, int _level, Action<TowerChallengeResult> _action)
        {
            // 保留旧关卡数据
            long oldChapterId = _m_curChapterId;
            int oldLevel = _m_curLevel;
            NPGSClientListener.sendRequestByLog(new GC2GS_023_021_ReqTowerFight(new Tower_PosInfo(_chapterId, _level)),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_021_RetTowerFight>((_info) =>
                {
                    List<NPCommonCostItem> costItems = new List<NPCommonCostItem>();
                    foreach (NPCommon_ItemInfo itemInfo in _info.getRewardList())
                    {
                        costItems.Add(new NPCommonCostItem(itemInfo));
                    }
                    TowerChallengeResult result = new TowerChallengeResult(_info.getIsDefeat(), _info.getCid(), costItems, oldChapterId,  oldLevel, _chapterId,  _level);
                    _action?.Invoke(result);
                }));
            
        }

        /// <summary>
        /// pvp关卡请求可以挑战的列表
        /// </summary>
        /// <param name="_action"></param>
        public void reqTowerChallengeList(Action<List<Tower_OpponentInfo>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_022_ReqTowerChallengeList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_022_RetTowerChallengeList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        _action.Invoke(_info.getOpponentList());
                    }));
        }

        /// <summary>
        /// 请求pvp关卡信息
        /// </summary>
        /// <param name="_chapterId"></param>
        /// <param name="_level"></param>
        /// <param name="_num"></param>
        /// <param name="_action"></param>
        public void reqPVPLevelInfo(long _chapterId, int _level, int _num, Action<List<Tower_OpponentInfo>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_023_ReqTowerChapterList(_chapterId, _level, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_023_RetTowerChapterList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        _action.Invoke(_info.getOpponentList());
                    }));
        }

        /// <summary>
        /// 请求激活研究
        /// </summary>
        /// <param name="_chapterId"></param>
        /// <param name="_level"></param>
        public void reqTowerResearchActive(long _chapterId, int _level)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_024_ReqTowerResearchActive(new Tower_PosInfo(_chapterId, _level)),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_024_RetTowerResearchActive>(null));
            
        }
        
        /// <summary>
        /// 请求领取章节研究奖励
        /// </summary>
        /// <param name="_chapterId"></param>
        /// <param name="_callBack"></param>
        public void reqTowerDrawResearchReward(long _chapterId, Action _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_025_ReqTowerDrawResearchReward(_chapterId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_023_025_RetTowerDrawResearchReward>(
                    (_suc, _msg) =>
                    {
                        if(_suc)
                            _callBack?.Invoke();
                    } ));
        }
        
        /// <summary>
        /// 请求爬塔战报
        /// </summary>
        /// <param name="_action"></param>
        public void reqTowerReportList(Action<List<Tower_ReportInfo>> _action)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_023_026_ReqTowerReportList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_023_026_RetTowerReportList>(
                    _info =>
                    {
                        if (_action == null || _info == null) return;
                        _action.Invoke(_info.getReportList());
                    }));
        }

        /// <summary>
        /// 当前塔位置发生变化
        /// </summary>
        /// <param name="_msg"></param>
        public void OnTowerPosChg(GS2GC_023_061_OnTowerPosChg _msg)
        {
            Tower_PosInfo towerInfo = _msg.getPosInfo();
            _m_curChapterId = towerInfo.getChapterId();
            _m_curLevel = towerInfo.getLevel();
            WinMsg.SendMsg(WinMsgType.ON_TOWER_CHG);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_msg"></param>
        public void onTowerResearchRewardDraw(GS2GC_023_063_OnTowerResearchRewardDraw _msg)
        {
            if (_msg != null)
                _m_hadDrawResearchRewardList.Add(_msg.getChapterId());
            _refreshRedTip();
        }

        public void onTowerResearchActivePosChg(GS2GC_023_062_OnTowerResearchActivePosChg _msg)
        {
            if(_msg == null)
                return;
            if (_m_activatePosInfo != null) 
                _m_activatePosInfo.updateInfo(_msg.getPos());
            else
                _m_activatePosInfo = new TowerPosInfo(_msg.getPos());
            
            _m_curEarningAddPer = _msg.getBuildingProfitAddPer();
            _refreshRedTip();
            NPPlayer.instance.buildingComp.recalAllBusinessBuilding();
            WinMsg.SendMsg(WinMsgType.ON_TOWER_ACTIVE_POS_CHG);
        }
        
        public void onTowerHighestPosHadReachChg(GS2GC_023_064_OnTowerHighestPosHadReachChg _msg)
        {
            if(_msg == null)
                return;
            if (_m_maxAchievedPosInfo != null) 
                _m_maxAchievedPosInfo.updateInfo(_msg.getPos());
            else
                _m_maxAchievedPosInfo = new TowerPosInfo(_msg.getPos());
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_TOWER_CHG);
        }
        #endregion
    }
}