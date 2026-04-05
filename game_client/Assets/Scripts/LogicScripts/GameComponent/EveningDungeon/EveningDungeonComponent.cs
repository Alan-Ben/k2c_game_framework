using System;
using System.Collections.Generic;
using ALPackage;
using Common.DungeonObj;
using GC2GS.p024_DungeonOp;
using GS2GC.p002_InitOp;
using GS2GC.p024_DungeonOp;

namespace GOE
{
    /// <summary>
    /// 晚间副本组件
    /// </summary>
    public class EveningDungeonComponent : _ANPBasicPlayerComponent
    {
        private List<long> _m_lUsedHeroList;//本轮使用的伙伴列表

        // 活动相关时间, 服务器会在活动结束End时推送变更
        private long _m_lPreCloseTimeMs;//上轮关闭时间戳
        private long _m_lPreviewTimeMs;//本轮预告时间戳
        private long _m_lStartTimeMs;// 本轮开始时间戳
        private long _m_lEndTimeMs;//本轮结束时间戳
        
        private EEveningDungeonActivityState _m_activityState;//活动状态

        private ALCommonEnableTaskController _m_tickActionMonoTask;//任务

        private EveningDungeonLocalPushDealer _m_localPushDealer;//晚间活动开始本地推送

        public EveningDungeonComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.EVENING_DUNGEON; } }
        public override ENPPlayerCompType[] dependCompList { get { return new ENPPlayerCompType[] {ENPPlayerCompType.HERO}; } }
        
        public override bool canPreInit { get { return true; } }

        public EEveningDungeonActivityState activityState { get { return _m_activityState; } }
        
        public long preCloseTimeMs { get { return _m_lPreCloseTimeMs; } }
        public long previewTimeMs { get { return _m_lPreviewTimeMs; } }
        public long startTimeMs { get { return _m_lStartTimeMs; } }
        public long endTimeMs { get { return _m_lEndTimeMs; } }
        
        public override void presendInitProtocol()
        {
            reqEveningDungeonInit();//请求初始化数据
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            _m_tickActionMonoTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 1f);
            //注册本地推送
            _m_localPushDealer = new EveningDungeonLocalPushDealer();
            LocalPushMgr.instance.regDealer(_m_localPushDealer);
        }

        protected override void _onInitFail()
        {
            ALLog.Error("EveningDungeonComponent init Fail!!!");
        }

        protected override void _discard()
        {
            _m_tickActionMonoTask.setDisable();
            //注销本地推送
            LocalPushMgr.instance.unRegDealer(_m_localPushDealer);
            _m_localPushDealer = null;
        }

        /// <summary>
        /// 任务执行方法
        /// </summary>
        private void _tick()
        {
            _refreshActivityState();//刷新当前活动状态
            
            WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_SEC_TICK);
        }
        
        /// <summary>
        /// 更新活动时间
        /// </summary>
        private void _updateActivityTime(EveningDungeon_TimeInfo _timeInfo)
        {
            if(_timeInfo == null)
                return;

            _m_lPreCloseTimeMs = _timeInfo.getPreCloseTimeMs();
            _m_lPreviewTimeMs = _timeInfo.getPreviewTimeMs();
            _m_lStartTimeMs = _timeInfo.getStartTimeMs();
            _m_lEndTimeMs = _timeInfo.getEndTimeMs();
            
            // 刷新当前活动状态
            _refreshActivityState();
        }
        
        /// <summary>
        /// 更新本轮使用的伙伴列表
        /// </summary>
        /// <param name="_heroList"></param>
        private void _updateUsedHeroList(List<long> _heroList)
        {
            int preUsedHeroCount = _m_lUsedHeroList?.Count ?? 0;//上一次使用过的伙伴数量

            if (_m_lUsedHeroList == null)
                _m_lUsedHeroList = new List<long>();
            else
                _m_lUsedHeroList.Clear();

            if (_heroList != null && _heroList.Count > 0)
            {
                _m_lUsedHeroList.AddRange(_heroList);
            }

            // 当数量有变化时
            if (_m_lUsedHeroList.Count != preUsedHeroCount)
            {
                _refreshRedTip();
                WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_USED_HERO_CHG);
            }
        }

        /// <summary>
        /// 刷新当前活动状态
        /// </summary>
        private void _refreshActivityState()
        {
            long nowServerTime = FpsAndPingMgr.instance.serverTimeTag;//当前服务器时间
            EEveningDungeonActivityState prevState = _m_activityState;//上一次的活动状态
            
            //刷新当前活动状态
            if(nowServerTime < _m_lPreCloseTimeMs)//若当前时间小于上一轮的关闭时间, 处于上一轮的结束状态
            {
                _m_activityState = EEveningDungeonActivityState.END;
            }
            else if (nowServerTime < _m_lPreviewTimeMs)//若当前时间大于上一轮的关闭时间 且 小于本轮的预览时间, 处于关闭状态
            {
                _m_activityState = EEveningDungeonActivityState.CLOSE;
            }
            else if(nowServerTime < _m_lStartTimeMs)//若当前时间大于本轮的预览时间 且 小于本轮的开始时间, 处于预览状态
            {
                _m_activityState = EEveningDungeonActivityState.PREVIEW;
            }
            else if(nowServerTime < _m_lEndTimeMs)//若当前时间大于本轮的开始时间 且 小于本轮的结束时间, 处于进行中状态
            {
                _m_activityState = EEveningDungeonActivityState.ONGOING;
            }
            else//若大于本轮的结束时间(这里不判断本轮的关闭时间 是因为服务端在活动结束后会推送时间变更, 会进行一次刷新)
            {
                _m_activityState = EEveningDungeonActivityState.END;
            }
            
            // 若活动状态发生变化, 发出对应消息
            if (prevState != _m_activityState)
            {
                switch (_m_activityState)
                {
                    case EEveningDungeonActivityState.PREVIEW:
                        WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_PREVIEW);
                        break;
                    
                    case EEveningDungeonActivityState.ONGOING:
                        WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_START);
                        break;
                    
                    case EEveningDungeonActivityState.END:
                        WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_END);
                        break;
                    
                    case EEveningDungeonActivityState.CLOSE:
                        WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_CLOSE);
                        break;
                }
                
                _refreshRedTip();
                WinMsg.SendMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, prevState, _m_activityState);
            }
        }
        
        /// <summary>
        /// 大臣是否使用过
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public bool getHeroUsed(long _heroId)
        {
            if (_m_lUsedHeroList == null || _m_lUsedHeroList.Count <= 0)
                return false;

            for (int i = 0; i < _m_lUsedHeroList.Count; i++)
            {
                if (_m_lUsedHeroList[i] == _heroId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取所有大臣的战斗信息
        /// </summary>
        /// <returns></returns>
        public List<EveningDungeonHeroFightInfo> getAllHeroFightInfoList()
        {
            List<EveningDungeonHeroFightInfo> heroFightInfoList = new List<EveningDungeonHeroFightInfo>();
            
            NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
            {
                if(_heroInfo == null)
                    return;
                
                heroFightInfoList.Add(new EveningDungeonHeroFightInfo(_heroInfo));
            });

            return heroFightInfoList;
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (_m_activityState != EEveningDungeonActivityState.ONGOING)
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EVENING_DUNGEON_ENTRY, 0);
                return;
            }

            int useHeroCount = _m_lUsedHeroList?.Count ?? 0;//已经使用大臣数量
            int totalHeroCount = NPPlayer.instance.heroComponent.getTotalHeroCount();//大臣总数量
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EVENING_DUNGEON_ENTRY, useHeroCount < totalHeroCount ? 1 : 0);
        }
        
        #region GC2GS

        /// <summary>
        /// 请求晚间副本初始化
        /// </summary>
        public void reqEveningDungeonInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_052_ReqEveningDungeonInit());
        }

        /// <summary>
        /// 请求攻击
        /// </summary>
        public void reqEveningDungeonAttack(long _heroId, Action<bool, GS2GC_024_011_RetEveningDungeonAttack> _reqCallback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_011_ReqEveningDungeonAttack(_heroId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_011_RetEveningDungeonAttack>(_reqCallback));
        }

        /// <summary>
        /// 请求boss信息
        /// </summary>
        public void reqEveningDungeonBossInfo(Action<GS2GC_024_012_RetEveningDungeonBossInfo> _succCallback)
        {
            NPGSClientListener.sendRequest(new GC2GS_024_012_ReqEveningDungeonBossInfo(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_024_012_RetEveningDungeonBossInfo>(_msg =>
                {
                    _succCallback?.Invoke(_msg);
                }, false, false));
        }

        /// <summary>
        /// 请求攻击日志
        /// </summary>
        /// <param name="_serial">请求序列号, 若为-1, 给最新的_needNum条</param>
        /// <param name="_needNum">请求数量, 若为-1, 给最新的一条到指定_serial之间的所有数据</param>
        public void reqEveningDungeonAttackLog(long _serial, int _needNum, Action<GC2GS_024_013_ReqEveningDungeonAttackLog, GS2GC_024_013_RetEveningDungeonAttackLog> _succCallback)
        {
            GC2GS_024_013_ReqEveningDungeonAttackLog reqMsg = new GC2GS_024_013_ReqEveningDungeonAttackLog(_serial, _needNum);
            NPGSClientListener.sendRequest(reqMsg,
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_024_013_RetEveningDungeonAttackLog>(_retMsg =>
                {
                    _succCallback?.Invoke(reqMsg, _retMsg);
                }, false, false));
        }

        /// <summary>
        /// 请求晚间副本排行榜
        /// </summary>
        public void reqEveningDungeonRankList(Action<GS2GC_024_014_RetEveningDungeonRankList> _succCallback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_014_ReqEveningDungeonRankList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_024_014_RetEveningDungeonRankList>(_msg =>
                {
                    _succCallback?.Invoke(_msg);
                }));
        }
        
        /// <summary>
        /// 请求晚间副本尾刀记录
        /// </summary>
        public void reqEveningDungeonDefeatLog(Action<GS2GC_024_015_RetEveningDungeonDefeatLog> _succCallback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_024_015_ReqEveningDungeonDefeatLog(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_024_015_RetEveningDungeonDefeatLog>(_msg =>
                {
                    _succCallback?.Invoke(_msg);
                }));
        }
        
        /// <summary>
        /// 请求晚间副本宝箱是否可以领取
        /// </summary>
        /// <param name="_dbId">宝箱数据库ID</param>
        /// <param name="_expireTs">过期时间戳</param>
        /// <param name="_callback">回调(是否成功, 是否可领取, 剩余领取次数)</param>
        public void reqEveningDungeonBoxCanDraw(long _dbId, long _expireTs, Action<bool, bool, int> _callback)
        {
            // 复用午间副本的宝箱领取检查接口
            NPGSClientListener.sendRequestByLog(new GC2GS.p024_DungeonOp.GC2GS_024_005_ReqMiddayDungeonBoxCanDraw(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_005_RetMiddayDungeonBoxCanDraw>((_isSuc, _msg) =>
                {
                    if(_isSuc && !_msg.getCanDraw())
                        AccountSettingMgr.instance.middayDungeonSaver.setHasInValid(_dbId, _expireTs);

                    _callback?.Invoke(_isSuc, _msg != null && _msg.getCanDraw(), _msg.getRemainDrawCount());
                }, null, false));
        }
        
        /// <summary>
        /// 请求晚间副本领取宝箱
        /// </summary>
        /// <param name="_dbId">宝箱数据库ID</param>
        /// <param name="_expireTs">过期时间戳</param>
        /// <param name="_callback">回调(是否成功)</param>
        public void reqEveningDungeonDrawBox(long _dbId, long _expireTs, Action<bool> _callback)
        {
            // 复用午间副本的宝箱领取接口
            NPGSClientListener.sendRequestByLog(new GC2GS.p024_DungeonOp.GC2GS_024_004_ReqMiddayDungeonDrawBox(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_024_004_RetMiddayDungeonDrawBox>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        AccountSettingMgr.instance.middayDungeonSaver.setHasOpen(_dbId, _expireTs);
                    }
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        #endregion

        #region GS2GC

        /// <summary>
        /// 收到到晚间副本初始化数据回包
        /// </summary>
        /// <param name="_msg"></param>
        public void retEveningDungeonInit(GS2GC_002_052_RetEveningDungeonInit _msg)
        {
            if (_msg == null || _msg.getInfo() == null)
                return;

            // 更新当前轮次开始时间
            _updateActivityTime(_msg.getTimeInfo());
            
            // 更新使用过的伙伴列表
            if (_msg.getInfo().getRoundStartTimeMS() != _m_lStartTimeMs)//若 数据对应的轮次开始时间 和 本轮活动开始时间不同, 说明轮次已经变化, 服务器给的数据无效
            {
                // 若服务器给的开始时间比客户端计算得到的开始时间还早, 说明是新的一轮, 需要清空使用过的伙伴列表
                _updateUsedHeroList(null);
            }
            else
            {
                _updateUsedHeroList(_msg.getInfo().getHadFightHeroList());
            }

            // 若还没初始化完成, 则设置初始化完成
            if(!isInitDone)
                setInitDone();
        }

        /// <summary>
        /// 晚间副本数据变化
        /// </summary>
        /// <param name="_msg"></param>
        public void OnEveningDungeonInfoChg(GS2GC_024_061_OnEveningDungeonInfoChg _msg)
        {
            if(_msg == null || _msg.getInfo() == null)
                return;

            if (_msg.getInfo().getRoundStartTimeMS() != _m_lStartTimeMs)//若 数据对应的轮次开始时间 和 本轮活动开始时间不同, 说明轮次已经变化, 服务器给的数据无效
            {
                _updateUsedHeroList(null);
            }
            else
            {
                _updateUsedHeroList(_msg.getInfo().getHadFightHeroList());
            }
        }

        /// <summary>
        /// 晚间副本活动时间变化推送, 服务器会在活动结束End时推送变更
        /// </summary>
        /// <param name="_msg"></param>
        public void OnEveningDungeonTimeInfoChg(GS2GC_024_062_OnEveningDungeonTimeInfoChg _msg)
        {
            if(_msg == null || _msg.getInfo() == null)
                return;
            
            _updateActivityTime(_msg.getInfo());
        }

        #endregion
    }
}