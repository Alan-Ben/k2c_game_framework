using System.Collections.Generic;
using System;
using System.Linq;
using ALPackage;
using Common.ClientData;
using Common.TravelEnum;
using Common.TravelObj;
using CommonEnum;
using JetBrains.Annotations;
using GS2GC.p002_InitOp;
using GS2GC.p008_TravelOp;
using GS2GC.p021_PlayerInfo;
using NPEnum;

namespace GOE
{
    // 游历模块管理类
    public partial class GPlayerTravelComponent : _ANPRemarkInfoComponent
    {
        //当前处理的事件
        private RedTipDealer _m_redDealer;
        [NotNull] private Travel_ClientData _m_clientData;
        private ALStepCounter _m_initStepCounter;

        [NotNull] private List<_ATravelEventInfo> _m_travelEventList = new List<_ATravelEventInfo>();
        [NotNull] private List<TravelConsortInfo> _m_lTravelConsortInfoList = new List<TravelConsortInfo>();
        
        [NotNull] private Dictionary<long, List<WCGPairIntLong>> _m_NeedShowLikeStageChgDialogDic = new Dictionary<long, List<WCGPairIntLong>>();//需要显示的好感度阶段变化的对话
        [NotNull] private List<long> _m_lNeedShowGainConsortIdList = new List<long>();//需要展示的获取妃子列表
        
        //当前正在处理的事件(这个事件可能不在_m_travelEventList列表中, _m_travelEventList列表中事件会在收到GS2GC_008_051_OnEventDel推送后删除,
        //但是并不代表客户端层面该事件处理完成, 要调用事件的setEventDealDone设置当前事件处理完成)
        private _ATravelEventInfo _m_curDealEvent;

        private bool _m_travelIng;
        
        //构造函数
        public GPlayerTravelComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr, ENPClientDataType.TRAVEL)
        {
            _m_redDealer = new RedTipDealer(this);
            _m_clientData = new Travel_ClientData();
            _m_initStepCounter= new ALStepCounter();
        }

        [NotNull] public List<_ATravelEventInfo> eventList { get => _m_travelEventList; }
        
        public _ATravelEventInfo curDealEvent { get => _m_curDealEvent; }

        /// <summary>
        /// 是否正在游历中
        /// </summary>
        public bool isTraveling
        {
            get { return _m_travelIng; }
            set => _m_travelIng = value;
        }
        
        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.TRAVEL; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion


        #region override 方法
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }


        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _m_initStepCounter.resetAll();
            _m_initStepCounter.chgTotalStepCount(2);
            _m_initStepCounter.regAllDoneDelegate(_m_redDealer.init);
            _reqTravelInitData();
        }

        protected override void _dealInit()
        {
            regDelegate(_m_initStepCounter.addDoneStepCount);
            _sendRequest();
        }

        protected override byte[] _makeData()
        {
            return _m_clientData.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            if (null == _data)
                return;
            _m_clientData.readPackage(_data);
        }

        protected override void _resetRemarkInfo()
        {
            _m_clientData = new Travel_ClientData();
            _saveData();
        }
        
        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_CHANGE, _simulateClickTravelChangeEventChange);//模拟点击游历交换事件交换按钮
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_NOT_CHANGE, _simulateClickTravelChangeEventNotChange);//模拟点击游历交换事件不交换按钮
            WinMsg.RegisterMsg(WinMsgType.TRAVEL_POS_UNLOCK, _onTravelPosUnlock);//游历地点解锁
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GPlayerTravelComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_CHANGE, _simulateClickTravelChangeEventChange);//模拟点击游历交换事件交换按钮
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAVEL_CHANGE_EVENT_NOT_CHANGE, _simulateClickTravelChangeEventNotChange);//模拟点击游历交换事件不交换按钮
            WinMsg.UnregisterMsg(WinMsgType.TRAVEL_POS_UNLOCK, _onTravelPosUnlock);//游历地点解锁

            _m_travelEventList.Clear();
            _m_curDealEvent = null;
            _m_travelIng = false;
            _m_lTravelConsortInfoList.Clear();
            _m_NeedShowLikeStageChgDialogDic.Clear();
            _m_lNeedShowGainConsortIdList.Clear();
            
            _m_redDealer.clear();
        }

        #endregion

        #region 游历事件

        /// <summary>
        /// 获取事件信息
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public _ATravelEventInfo getEventInfo(long _instanceId)
        {
            foreach (var eventInfo in _m_travelEventList)
            {
                if(eventInfo != null && eventInfo.instanceId == _instanceId)
                    return eventInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 设置当前执行的事件
        /// </summary>
        /// <param name="_dealEvent"></param>
        public void setCurDealEvent(_ATravelEventInfo _dealEvent)
        {
            // 若当前还有事件正在处理
            if (_m_curDealEvent != null && !_m_curDealEvent.eventDealDone)
            {
                Debug.LogError_EditorOnly($"[setCurDealEvent] 当前还有事件:{_m_curDealEvent}正在处理, 但是却设置了新的处理事件: {_dealEvent}, 请检查代码调用逻辑");
            }
            
            _m_curDealEvent = _dealEvent;
        }

        public void clearCurDealEvent()
        {
            _m_curDealEvent = null;
        }
        
        public void removeEvent(_ATravelEventInfo _travelEventInfo)
        {
            if(_travelEventInfo == null)
                return;

            _m_travelEventList.Remove(_travelEventInfo);
        }
        
        #endregion

        #region 游历妃子

        /// <summary>
        /// 获取游历获取妃子数据
        /// </summary>
        /// <param name="consortId"></param>
        /// <returns></returns>
        public TravelConsortInfo getTravelConsortInfo(long consortId)
        {
            foreach (var consortInfo in _m_lTravelConsortInfoList)
            {
                if(consortInfo != null && consortInfo.consortId == consortId)
                    return consortInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取妃子游历解锁状态
        /// </summary>
        /// <param name="consortId"></param>
        /// <returns></returns>
        public ETravelConsortUnlockStat getTravelConsortUnlockStat(long consortId)
        {
            // 若已经有该妃子的信息，则表示已经解锁
            if (NPPlayer.instance.consortComp.getConsortInfo(consortId) != null)
                return ETravelConsortUnlockStat.UNLOCK;

            // 若能从travel_consort表中获取到数据, 代表能从游历中解锁 但是还未解锁
            if (GRefdataCoreMgr.instance.travelConsortRefCore.getRef(consortId) != null)
                return ETravelConsortUnlockStat.LOCK_IN_TRAVEL;
            
            // 返回未解锁 且 无法从游历中解锁
            return ETravelConsortUnlockStat.LOCK;
        }

        #endregion
        
        #region 妃子好感度阶段变化

        /// <summary>
        /// 当妃子好感度变化时
        /// </summary>
        /// <param name="_oldLike">旧好感度</param>
        /// <param name="_newLike">新好感度</param>
        private void _onConsortLikeChg(long _consortId, int _oldLike, int _newLike)
        {
            TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_consortId);
            if(travelConsortRefObj == null)
                return;

            if (travelConsortRefObj.like_step_dialog_list != null)
            {
                List<WCGPairIntLong> needShowLikeStageChgDialogList = getNeedShowLikeStageChgList(_consortId, true);
                foreach (WCGPairIntLong likeStepDialog in travelConsortRefObj.like_step_dialog_list)
                {
                    // 遍历的好感度阶段所需好感度 <= 旧的好感度时, 继续遍历
                    if(likeStepDialog == null || likeStepDialog.first() <= _oldLike)
                        continue;
                
                    // 遍历的好感度阶段所需好感度 > 新的好感度时, 直接返回(因为like_step_dialog_list按照好感度升序排序, 所以只要找到第一个大于新好感度的阶段就可以返回)
                    if(likeStepDialog.first() > _newLike)
                        return;
                
                    needShowLikeStageChgDialogList?.Add(likeStepDialog);
                }
            }

            // 当妃子当前好感度 >= 结婚所需好感度时
            if (_newLike >= travelConsortRefObj.marry_need_like && 
                // 且该妃子未展示过获取窗口
                (AccountSettingMgr.instance.accountSetting.alreadyShowGainConsortList == null || !AccountSettingMgr.instance.accountSetting.alreadyShowGainConsortList.Contains(_consortId)) && 
                // 且 _m_lNeedShowGainConsortIdList待展示列表中不包含该妃子
                !_m_lNeedShowGainConsortIdList.Contains(_consortId))
            {
                // 将该妃子加入待展示列表
                _m_lNeedShowGainConsortIdList.Add(_consortId);
            }
        }
        
        /// <summary>
        /// 获取需要显示的好感度阶段变化的列表
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public List<WCGPairIntLong> getNeedShowLikeStageChgList(long _consortId, bool _needCreate = false)
        {
            if (!_m_NeedShowLikeStageChgDialogDic.TryGetValue(_consortId, out List<WCGPairIntLong> resultList) || resultList == null)
            {
                if (_needCreate)
                {
                    resultList = new List<WCGPairIntLong>();
                    _m_NeedShowLikeStageChgDialogDic[_consortId] = resultList;
                }
            }

            return resultList;
        }

        /// <summary>
        /// 获取并移除需要显示的好感度阶段变化的列表
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public List<WCGPairIntLong> getAndRemoveLikeStageChgList(long _consortId)
        {
            _m_NeedShowLikeStageChgDialogDic.Remove(_consortId, out List<WCGPairIntLong> _needShowLikeStageChgDialogList);
            return _needShowLikeStageChgDialogList;
        }

        /// <summary>
        /// 展示所有妃子好感度阶段变化对话
        /// </summary>
        public void showAllConsortLikeStageChgDialog(bool _tryShowConsortGet, Action _allShowDone)
        {
            if(_m_NeedShowLikeStageChgDialogDic.Count <= 0)
            {
                _allShowDone?.Invoke();
                return;
            }
            
            long showConsortId = _m_NeedShowLikeStageChgDialogDic.Keys.First();
            showConsortLikeStageChgDialog(showConsortId, _tryShowConsortGet, () =>
            {
                // 显示完成后, 继续显示下一个妃子的好感度阶段变化对话
                showAllConsortLikeStageChgDialog(_tryShowConsortGet, _allShowDone);
            });
        }
        
        /// <summary>
        /// 显示妃子好感度阶段变化对话
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_dealDone"></param>
        public void showConsortLikeStageChgDialog(long _consortId, bool _tryShowConsortGet, Action _dealDone)
        {
            // 获取需要显示的好感度阶段对话列表, 并从字典中移除
            List<WCGPairIntLong> needShowLikeStageChgDialogList = getAndRemoveLikeStageChgList(_consortId);
            if (needShowLikeStageChgDialogList == null || needShowLikeStageChgDialogList.Count <= 0)
            {
                // 好感度阶段变化对话显示完成后, 若需要尝试显示妃子获取窗口, 尝试显示一下这个妃子的获取窗口
                if (_tryShowConsortGet)
                {
                    tryShowGainConsort(_consortId, _dealDone);
                }
                else
                {
                    _dealDone?.Invoke();
                }
                return;
            }

            GGUIWndConsortLikeStageChangeTip consortLikeStageChangeTipWnd = new GGUIWndConsortLikeStageChangeTip();
            consortLikeStageChangeTipWnd.load();
            
            ALProcess showConsortLikeStageChangeProcess = ALProcess.CreateProcess();
            
            foreach (var needShowLikeStageChgDialog in needShowLikeStageChgDialogList)
            {
                if (needShowLikeStageChgDialog == null)
                    continue;

                showConsortLikeStageChangeProcess
                    // 展示好感度增加tip
                    .addDelegateProcess((_showAddLikeTipDone) =>
                    {
                        GGUIWndTravelResult_MeetConsortAddLikeTip.showMeetConsortAddLikeTipWnd(_showAddLikeTipDone);
                    })
                    // 展示对话
                    .addDelegateProcess((_showAddLikeDialogDone) =>
                    {
                        GCommon.enterDialogueNode(needShowLikeStageChgDialog.second(), _showAddLikeDialogDone, true,
                            false);
                    })
                    // 显示好感度阶段变化提示窗口
                    .addDelegateProcess((_likeStageChangeTipWndShowDone) =>
                    {
                        consortLikeStageChangeTipWnd.regLoadDoneDelegate(() =>
                        {
                            consortLikeStageChangeTipWnd.showWnd();
                            consortLikeStageChangeTipWnd.setData(_consortId, needShowLikeStageChgDialog.first(), () =>
                            {
                                consortLikeStageChangeTipWnd.hideWnd();
                                _likeStageChangeTipWndShowDone?.Invoke();
                            });
                        });
                    });
            }

            showConsortLikeStageChangeProcess.addProcess(() =>
            {
                consortLikeStageChangeTipWnd?.discard();
                consortLikeStageChangeTipWnd = null;

                // 好感度阶段变化对话显示完成后, 若需要尝试显示妃子获取窗口, 尝试显示一下这个妃子的获取窗口
                if (_tryShowConsortGet)
                {
                    tryShowGainConsort(_consortId, _dealDone);
                }
                else
                {
                    _dealDone?.Invoke();
                }
            });
            
            showConsortLikeStageChangeProcess.deal();
        }

        /// <summary>
        /// 尝试进行获取妃子表现
        /// </summary>
        public void tryShowGainConsort(long _consortId, Action _showDone)
        {
            if (_m_lNeedShowGainConsortIdList.Contains(_consortId))
            {
                CommonRewardDealer.dealShowGainConsort(_consortId, ()=>
                {
                    _m_lNeedShowGainConsortIdList.Remove(_consortId);
                    _showDone?.Invoke();
                });
            }
            else
            {
                _showDone?.Invoke();
            }
        }
        
        #endregion

        /// <summary>
        /// 是否需要游历地点展示解锁表现
        /// </summary>
        /// <param name="_travelPosRef"></param>
        /// <returns></returns>
        public bool getNeedShowPosUnlock(TravelPosRefObj _travelPosRef)
        {
            if (_travelPosRef == null)
                return false;

            // 如果没有解锁条件(代表默认解锁), 则不展示解锁表现 https://www.teambition.com/task/69a938a02d74482d3a7fc18c
            if (_travelPosRef.unlock_condition == null || _travelPosRef.unlock_condition.isEmpty)
                return false;
            
            bool isUnlock = _travelPosRef.isUnlock(null);
            return !getIsPosUnlockShowed(_travelPosRef) && isUnlock;
        }
        
        /// <summary>
        /// 是否已经展示过地点解锁表现
        /// </summary>
        /// <param name="_travelPosRef"></param>
        /// <returns></returns>
        public bool getIsPosUnlockShowed(TravelPosRefObj _travelPosRef)
        {
            if (_travelPosRef == null)
                return false;

            return _m_clientData.getUnlockedPosList()?.Contains(_travelPosRef.id) ?? false;
        }
        
        //
        // /// <summary>
        // /// 某个地点的解锁领奖状态
        // /// </summary>
        // /// <param name="_travelPosRef"></param>
        // /// <returns></returns>
        // public EGameCommonUnlockRewardType getPosUnlockRewardStat(TravelPosRefObj _travelPosRef)
        // {
        //     EGameCommonUnlockType stat = (null != _travelPosRef.unlock_condition && _travelPosRef.unlock_condition.IsEnable(null))
        //         ? EGameCommonUnlockType.UNLOCK
        //         : EGameCommonUnlockType.LOCK;
        //
        //     if (stat == EGameCommonUnlockType.UNLOCK && getIsPosUnlockShowed(_travelPosRef))
        //     {
        //         if (_m_rewardedPosList.Contains(_travelPosRef.id))
        //             return EGameCommonUnlockRewardType.UNLOCK_HAS_GET;
        //         return EGameCommonUnlockRewardType.UNLOCK_UN_GET;
        //     }
        //     
        //     return EGameCommonUnlockRewardType.LOCK;
        // }

        #region S2C
        
        public void retTravelInit(GS2GC_002_006_RetTravelInit _msg)
        {
            if (null == _msg)
                return;

            _m_travelEventList.Clear();
            _ATravelEventInfo eventInfo = null;
            if (_msg.getCanDealEventList() != null)
            {
                foreach (Travel_Event serverEventInfo in _msg.getCanDealEventList())
                {
                    if(serverEventInfo  == null)
                        continue;

                    eventInfo = TravelEventUtil.makeTravelEventInfo(serverEventInfo);
                    _m_travelEventList.Add(eventInfo);
                }
            }
            
            _m_lTravelConsortInfoList.Clear();
            TravelConsortInfo travelConsortInfo = null;
            if (_msg.getTravelConsortList() != null)
            {
                foreach (var serverInfo in _msg.getTravelConsortList())
                {
                    if(serverInfo == null)
                        continue;
                    
                    travelConsortInfo = new TravelConsortInfo(serverInfo);
                    _m_lTravelConsortInfoList.Add(travelConsortInfo);
                }
            }
            
            _m_initStepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 增加游历事件
        /// </summary>
        public void onEventAdd(GS2GC_008_050_OnEventAdd _msg)
        {
            if(!isInited || _msg == null || _msg.getEventInfo() == null)
                return;

            _ATravelEventInfo eventInfo = getEventInfo(_msg.getEventInfo().getInstanceId());
            if (eventInfo == null)
            {
                eventInfo = TravelEventUtil.makeTravelEventInfo(_msg.getEventInfo());
                _m_travelEventList.Add(eventInfo);
            }
            else
            {
                eventInfo.updateEventId(_msg.getEventInfo().getEventId());
                eventInfo.updatePosId(_msg.getEventInfo().getPos());
            }
        }
        
        /// <summary>
        /// 删除游历事件
        /// </summary>
        /// <param name="_msg"></param>
        public void onEventDel(GS2GC_008_051_OnEventDel _msg)
        {
            if(!isInited || _msg == null)
                return;

            _m_travelEventList.Remove((_eventInfo) =>
            {
                return _eventInfo != null && _eventInfo.instanceId == _msg.getInstanceId();
            });
        }

        /// <summary>
        /// 游历妃子数据变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onConsortChg(GS2GC_008_052_OnConsortChg _msg)
        {
            if(!isInited || _msg == null || _msg.getConsort() == null)
                return;

            TravelConsortInfo travelConsortInfo = getTravelConsortInfo(_msg.getConsort().getConsortId());
            int oldLike = travelConsortInfo?.like ?? 0;
            
            if (travelConsortInfo == null)
            {
                travelConsortInfo = new TravelConsortInfo(_msg.getConsort());
                _m_lTravelConsortInfoList.Add(travelConsortInfo);
            }
            else
            {
                travelConsortInfo.updateInfo(_msg.getConsort());
            }
         
            if(oldLike != travelConsortInfo.like)
                _onConsortLikeChg(travelConsortInfo.consortId, oldLike, travelConsortInfo.like);
        }
        
        #endregion

        #region C2S

        /// <summary>
        /// 商店组件初始化
        /// </summary>
        private void _reqTravelInitData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_006_ReqTravelInit());
        }

        // /// <summary>
        // /// 领取地点解锁奖励
        // /// </summary>
        // public void reqGainUnlockedPosReward(TravelPosRefObj _travelPosRef, Action<GS2GC_021_019_RetGainUnlockedPosReward> _backAction = null)
        // {
        //     if (getPosUnlockRewardStat(_travelPosRef) != EGameCommonUnlockRewardType.UNLOCK_UN_GET)
        //     {
        //         return;
        //     }
        //     NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_019_ReqGainUnlockedPosReward(_travelPosRef.id),
        //         new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_019_RetGainUnlockedPosReward>((info) =>
        //         {
        //             if (null != _backAction)
        //                 _backAction(info);
        //         }));
        // }

        /// <summary>
        /// 请求单次游历
        /// </summary>
        public void reqStartTravel(Action<GS2GC_008_001_RetStartTravel> _backAction = null, Action _failAction = null)
        {
            if (!GCommon.lazycdEnough(GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id, 1, true))
            {
                _failAction?.Invoke();
                return;
            }
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_001_ReqStartTravel(),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_001_RetStartTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求一键游历
        /// </summary>
        public void reqAkeyTravel(Action<GS2GC_008_002_RetAkeyTravel> _backAction = null, Action _failAction = null)
        {
            if (!GCommon.lazycdEnough(GRefdataCoreMgr.instance.npGeneral.travel_cost_lazycd_id, 1, true))
            {
                _failAction?.Invoke();
                return;
            }
            
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_002_ReqAkeyTravel(),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_002_RetAkeyTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }

        /// <summary>
        /// 请求处理奖励游历事件
        /// </summary>
        public void reqDealRewardTravel(long instanceId, Action<GS2GC_008_003_RetDealRewardTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_003_ReqDealRewardTravel(instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_003_RetDealRewardTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理妃子酒馆游历事件
        /// </summary>
        public void reqDealConsortBarTravel(long instanceId, long consortId, long costId, Action<GS2GC_008_004_RetDealConsortBarTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_004_ReqDealConsortBarTravel(instanceId, consortId, costId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_004_RetDealConsortBarTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理妃子酒馆游历事件
        /// </summary>
        public void reqDealChangeTravel(long instanceId, bool isChange, Action<GS2GC_008_005_RetDealChangeTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_005_ReqDealChangeTravel(instanceId, isChange),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_005_RetDealChangeTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }

        /// <summary>
        /// 处理妃子邀约游历事件
        /// </summary>
        public void reqDealInvitationTravel(long instanceId, long consortId, Action<GS2GC_008_006_RetDealInvitationTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_006_ReqDealInvitationTravel(instanceId, consortId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_006_RetDealInvitationTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理大臣加国力游历事件
        /// </summary>
        public void reqDealAddPowerTravel(long instanceId, long consortId, Action<GS2GC_008_007_RetDealAddPowerTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_007_ReqDealAddPowerTravel(instanceId, consortId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_007_RetDealAddPowerTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理增加妃子好感度游历事件
        /// </summary>
        public void reqDealConsortLikeTravel(long instanceId, Action<GS2GC_008_008_RetDealConsortLikeTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.mak_008_ReqDealConsortLikeTravel(instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_008_RetDealConsortLikeTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理增加妃子亲密度游历事件
        /// </summary>
        public void reqDealConsortIntimacyTravel(long instanceId, Action<GS2GC_008_009_RetDealConsortIntimacyTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.mak_009_ReqDealConsortIntimacyTravel(instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_009_RetDealConsortIntimacyTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }
        
        /// <summary>
        /// 处理增加必生卷王游历事件
        /// </summary>
        public void reqDealGiftedTravel(long instanceId, Action<GS2GC_008_010_RetDealGiftedTravel> _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.mak_010_ReqDealGiftedTravel(instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_008_010_RetDealGiftedTravel>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }, (_errCode) =>
                {
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failAction?.Invoke();
                }));
        }

        /// <summary>
        /// 处理博彩游历事件
        /// </summary>
        public void reqDealGambleTravel(long instanceId, int betAmount, bool isAbandon, Action<bool, GS2GC_008_011_RetDealGamblingTravel> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_008_TravelOp.make_011_ReqDealGamblingTravel(instanceId, betAmount, isAbandon),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_008_011_RetDealGamblingTravel>((_isSucc, _info) =>
                {
                    _backAction?.Invoke(_isSucc, _info);
                }));
        }
        
        #endregion

        #region remarkInfo

        public void setPosUnlockShowed(long _posId, Action _dealDone)
        {
            if (!(_m_clientData.getUnlockedPosList()?.Contains(_posId) ?? false))
            {
                _m_clientData.addUnlockedPosList(_posId);
                _saveData();
                _dealDone?.Invoke();
            }
            _dealDone?.Invoke();
        }

        /// <summary>
        /// 是否第一次进入游历
        /// </summary>
        public bool getIsFirstEnterTravel()
        {
            return !_m_clientData.getHadEnteredTravel();
        }

        /// <summary>
        /// 设置进入过游历
        /// </summary>
        public void setHadEnteredTravel()
        {
            _m_clientData.setHadEnteredTravel(true);
            
            _saveData();
        }

        #endregion
        
        /// <summary>
        /// 模拟点击游历交换事件交换按钮
        /// </summary>
        private void _simulateClickTravelChangeEventChange()
        {
            if(curDealEvent == null || !(curDealEvent is TravelChangeEventInfo _changeEventInfo) || _changeEventInfo == null)
                return;

            _changeEventInfo.needChange = true;
        }
        
        /// <summary>
        /// 模拟点击游历交换事件不交换按钮
        /// </summary>
        private void _simulateClickTravelChangeEventNotChange()
        {
            if(curDealEvent == null || !(curDealEvent is TravelChangeEventInfo _changeEventInfo) || _changeEventInfo == null)
                return;

            _changeEventInfo.needChange = false;
        }

        /// <summary>
        /// 游历地点解锁
        /// </summary>
        /// <param name="_objs"></param>
        private void _onTravelPosUnlock(params object[] _objs)
        {
            if(_objs == null || !(_objs[0] is TravelPosRefObj unlockPosRefObj) || unlockPosRefObj == null)
                return;
            
            NPGUIAddSceneCenterTip.instance.showIconTextTip(unlockPosRefObj.icon, TextTranslate.instance.getLanguage(unlockPosRefObj.name), GRefdataCoreMgr.instance.npGeneral.travel_pos_unlock_center_tip_id);
        }
    }
}
