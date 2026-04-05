using System;
using NPEnum;
using System.Collections.Generic;
using GC2GS.p002_InitOp;
using GC2GS.p021_PlayerInfo;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using NPCommon;

namespace GOE
{
    public class PlayerFuncUnlockComponent : _ANPRemarkInfoComponent
    {
        [NotNull]private List<FuncUnlockInfo> _m_dFuncUnlockInfoList;//系统功能解锁数据
        private bool _m_bAlreadyShowNextUnlockBubble;//是否已经展示下一个解锁气泡

        public PlayerFuncUnlockComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr, ENPClientDataType.FUNC_UNLOCK)
        {
            _m_dFuncUnlockInfoList = new List<FuncUnlockInfo>();
        }

        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.FUNC_UNLOCK; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 是否已经展示下一个解锁气泡
        /// </summary>
        public bool alreadyShowNextUnlockBubble { get { return _m_bAlreadyShowNextUnlockBubble; } set { _m_bAlreadyShowNextUnlockBubble = value; } }

        public override void presendInitProtocol()
        {

        }

        protected override void _discard()
        {
            _m_dFuncUnlockInfoList.Clear();
            _m_bAlreadyShowNextUnlockBubble = false;

            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_FUNC_UNLOCK_TIP, checkNewFuncUnlock);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TUTORIAL_ALL_FORCE_DONE, checkNewFuncUnlock);//引导完检查是否还有解锁提示
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_FUNC_UNLOCK_TIP, checkNewFuncUnlock);
            WinMsg.RegisterMsgAct(WinMsgType.ON_TUTORIAL_ALL_FORCE_DONE, checkNewFuncUnlock);//引导完检查是否还有解锁提示
        }

        protected override void _onInitFail()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            //重新计算带有FuncUnlock配置的红点
            RedTipMgr.instance.recalculateRedTipWithFuncUnlock();
        }

        /// <summary>
        /// 构造数据写入存储部分
        /// </summary>
        protected override byte[] _makeData()
        {
            RemarkData.FuncUnlockData data = new RemarkData.FuncUnlockData();

            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if(null == funcUnlockInfo || !funcUnlockInfo.hasShowTip)
                    continue;
                
                data.addFuncUnlockIdList(funcUnlockInfo.funcId);
            }

            return data.makePackage();
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        protected override void _readData(byte[] _data)
        {
            //初始化已经展示解锁列表
            RemarkData.FuncUnlockData data = new RemarkData.FuncUnlockData();
            if (_data != null) 
                data.readPackage(_data);
            List<long> showDoneIdList = data.getFuncUnlockIdList();
            
            //初始化功能解锁列表
            GRefdataCoreMgr.instance.funcUnlockRefCore.dealAllRef((itemRef) =>
            {
                if(null == itemRef)
                    return;

                FuncUnlockInfo funcUnlockInfo = new FuncUnlockInfo(itemRef);
                //设置已经提示过的功能
                if(null != showDoneIdList && showDoneIdList.Contains(itemRef._refId))
                    funcUnlockInfo.setTipShowDone();

                _m_dFuncUnlockInfoList.Add(funcUnlockInfo);
            });

            //按照排序id排序
            _m_dFuncUnlockInfoList.Sort(sortList);

            //初始化已领取奖励
            reqPlayerFuncUnlockInit();
        }

        protected override void _resetRemarkInfo()
        {
            RemarkData.FuncUnlockData data = new RemarkData.FuncUnlockData();
            _saveData();
            _readData(data.makePackage());
        }

        /// <summary>
        /// 功能解锁否完成
        /// </summary>
        /// <returns></returns>
        public bool isFuncUnlock(ENPFunctionType _funcType)
        {
            FuncUnlockInfo funcUnlockInfo = getFuncUnlockInfo(_funcType);
            if (null == funcUnlockInfo)
                return true;

            return funcUnlockInfo.isUnlock;
        }
        
        /// <summary>
        /// 功能解锁表现是否完成
        /// </summary>
        /// <returns></returns>
        public bool isFuncUnlockTipDone(ENPFunctionType _funcType)
        {
            FuncUnlockInfo funcUnlockInfo = getFuncUnlockInfo(_funcType);
            if (null == funcUnlockInfo)
                return true;

            //已经解锁，并且不需要提示，说明提示已经完成了（或者配置了不需要提示）
            return funcUnlockInfo.isUnlock && !funcUnlockInfo.isNeedTip;
        }
        
        /// <summary>
        /// 获取功能解锁信息
        /// </summary>
        /// <param name="_funcType"></param>
        /// <returns></returns>
        public FuncUnlockInfo getFuncUnlockInfo(ENPFunctionType _funcType)
        {
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if(null == funcUnlockInfo)
                    continue;

                if (funcUnlockInfo.funcType == _funcType)
                    return funcUnlockInfo;
            }

            return null;
        }

        /// <summary>
        /// 设置解锁表现显示完成
        /// </summary>
        public void setShowTipDone(ENPFunctionType _funcType)
        {
            FuncUnlockInfo funcUnlockInfo = getFuncUnlockInfo(_funcType);
            if(null == funcUnlockInfo)
                return;
            
            funcUnlockInfo.setTipShowDone();
            
            //必然发送消息保存
            _saveData();

            //发送消息刷新新功能解锁显示
            WinMsg.SendMsg(WinMsgType.ON_NEW_FUNCTION_UNLOCK_SHOW, _funcType);
            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 强制设置全部展示完成
        /// </summary>
        public void forceSetAllShowDone()
        {
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if (null == funcUnlockInfo)
                    continue;

                funcUnlockInfo.setTipShowDone();
            }
            _saveData();
        }

        /// <summary>
        /// 重置解锁表现
        /// </summary>
        /// <param name="_funcType"></param>
        public void resetShowTip(ENPFunctionType _funcType)
        {
            FuncUnlockInfo funcUnlockInfo = getFuncUnlockInfo(_funcType);
            if (null == funcUnlockInfo)
                return;

            funcUnlockInfo.resetShowTip();

            _saveData();
        }

        /// <summary>
        /// 重置所有解锁表现
        /// </summary>
        public void resetAllShowTip()
        {
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if (null == funcUnlockInfo)
                    continue;

                funcUnlockInfo.resetShowTip();
            }

            _saveData();
        }

        /// <summary>
        /// 获取需要提示的tip队列
        /// </summary>
        /// <returns></returns>
        public void getNeedTipFuncUnlock(List<FuncUnlockInfo> _funcUnlockList)
        {
            if (null == _funcUnlockList)
                return;

            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if(null == funcUnlockInfo)
                    continue;

                if (funcUnlockInfo.isNeedTip)
                {
                    _funcUnlockList.Add(funcUnlockInfo);
                }
            }
        }

        /// <summary>
        /// 获取不忽略的信息列表
        /// </summary>
        /// <param name="_funcUnlockList"></param>
        public void getNotIgnoreFuncUnlock(List<FuncUnlockInfo> _funcUnlockList)
        {
            if (null == _funcUnlockList)
                return;

            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if(null == funcUnlockInfo)
                    continue;

                if (funcUnlockInfo.functionUnlockRef != null && !funcUnlockInfo.functionUnlockRef.ignore_function_list_show)
                {
                    _funcUnlockList.Add(funcUnlockInfo);
                }
            }
        }

        /// <summary>
        /// 获取第一个需要展示的功能解锁信息
        /// </summary>
        /// <returns></returns>
        public FuncUnlockInfo getFirstShowInfo()
        {
            FuncUnlockInfo nextInfo = null;
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                //未领取奖励并且不需要忽略的需要展示
                if (null != funcUnlockInfo && !funcUnlockInfo.hasGetReward && funcUnlockInfo.functionUnlockRef != null && !funcUnlockInfo.functionUnlockRef.ignore_function_list_show)
                {
                    if(nextInfo == null)
                        nextInfo = funcUnlockInfo;

                    //已解锁未领奖优先展示
                    if (funcUnlockInfo.isUnlock)
                    {
                        nextInfo = funcUnlockInfo;
                        break;
                    }
                }
            }

            return nextInfo;
        }

        /// <summary>
        /// 检查是否弹出提示
        /// </summary>
        public void checkNewFuncUnlock()
        {
            //检查触发功能解锁
            //只在卧室，主城，华尔街场景弹窗
            if (!(QueueMgr.instance._lastNode is GNodeBuilding || QueueMgr.instance._lastNode is GNodeSpaceStation))
                return;

            //先刷新下红点
            refreshRedTip();

            //处理边引导跳转
            Action<long> dealTutorialEdge = (_id) =>
            {
                //如果正在引导或者正在处理notice，直接返回不触发边引导
                if (Game.instance.isInTutorial || NPUINoticeMgr.instance.isDealing)
                    return;

                NPTutorialEdgeRef tutorialEdgeRef = GRefdataCoreMgr.instance.tutorialEdgeRefCore.getRef(_id);
                if (tutorialEdgeRef != null)
                    NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(tutorialEdgeRef.res_path_id), null);
                else
                {
                    Debug.LogError_EditorOnly($"功能解锁未获取到tutorial edge配置，id:{_id}");
                }
            };

            //需要提示的列表
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if(null == funcUnlockInfo)
                    continue;

                // 是否可以发送通知给服务端
                if (funcUnlockInfo.canNotifyServer)
                {
                    funcUnlockInfo.setHadNotified();
                    reqClientNotifyFuncUnlock(funcUnlockInfo.funcType);
                }
                
                //已经解锁 并且 没展示过 并且 要忽略表现的 直接设置数据为看过,同时发送消息变动
                if (funcUnlockInfo.isUnlock && !funcUnlockInfo.hasShowTip && funcUnlockInfo.ignorePopUnlockTip)
                {
                    funcUnlockInfo.setTipShowDone();
                    WinMsg.SendMsg(WinMsgType.ON_NEW_FUNCTION_UNLOCK_SHOW, funcUnlockInfo.funcType);
                    continue;
                }
                
                if (funcUnlockInfo.isNeedTip && funcUnlockInfo.functionUnlockRef != null)
                {
                    switch (QueueMgr.instance._lastNode)
                    {
                        case GNodeBuilding:

                            //如果是属于主城或者无所谓场景的功能，直接弹解锁提示，否则弹对应的边引导
                            if (funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.BUILDING || funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.ALL)
                                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_FuncUnlockTip(funcUnlockInfo));
                            else if (funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.SPACE_STATION)
                                dealTutorialEdge(GRefdataCoreMgr.instance.npGeneral.building_to_wall_street_tutorial_edge_id);

                            return;
                        case GNodeSpaceStation:

                            //如果是属于华尔街或者无所谓场景的功能，直接弹解锁提示，否则弹对应的边引导
                            if (funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.SPACE_STATION || funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.ALL)
                                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_FuncUnlockTip(funcUnlockInfo));
                            else if (funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.BUILDING)
                                dealTutorialEdge(GRefdataCoreMgr.instance.npGeneral.wall_street_to_building_tutorial_edge_id);

                            return;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            long count = 0;
            for (int i = 0; i < _m_dFuncUnlockInfoList.Count; i++)
            {
                if (_m_dFuncUnlockInfoList[i] != null && _m_dFuncUnlockInfoList[i].canGetReward)
                    count++;
            }

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FUNC_PREVIEW_PAGE, count);
        }

        /// <summary>
        /// 获取下一个会解锁的功能
        /// </summary>
        /// <returns></returns>
        public FuncUnlockInfo getNextUnlockFunction()
        {
            FuncUnlockInfo nextInfo = null;
            for (int i = 0; i < _m_dFuncUnlockInfoList.Count; i++)
            {
                if (_m_dFuncUnlockInfoList[i] != null && 
                    !_m_dFuncUnlockInfoList[i].isUnlock && 
                    _m_dFuncUnlockInfoList[i].functionUnlockRef != null && 
                    !_m_dFuncUnlockInfoList[i].functionUnlockRef.ignore_function_list_show)
                {
                    //按照解锁顺序id排序，获取第一个未解锁的功能
                    if (nextInfo == null || _m_dFuncUnlockInfoList[i].unlockOrderId < nextInfo.unlockOrderId)
                    {
                        nextInfo = _m_dFuncUnlockInfoList[i];
                    }
                }
            }
            return nextInfo;
        }

        //排序：sortId从小到大
        private int sortList(FuncUnlockInfo _a, FuncUnlockInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            return _a.sortId.CompareTo(_b.sortId);
        }

        #region S2C

        /// <summary>
        /// 初始化已领取列表
        /// </summary>
        public void retPlayerFuncUnlockInit(GS2GC_002_029_RetPlayerFuncUnlockInit _msg)
        {
            if (_msg == null)
                return;

            List<NPEnum.ENPFunctionType> hadDrawList = _msg.getHadUnlockTypeList();//已领奖列表
            List<NPEnum.ENPFunctionType> clientNotifiedList = _msg.getClientNotifiedTypeList();//已通知列表
            if (hadDrawList == null)
                return;

            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if (null == funcUnlockInfo)
                    continue;

                if(hadDrawList.Contains(funcUnlockInfo.funcType))
                    funcUnlockInfo.setHadGetReward();
                if(clientNotifiedList != null && clientNotifiedList.Contains(funcUnlockInfo.funcType))
                    funcUnlockInfo.setHadNotified();
            }

            //刷新红点
            refreshRedTip();
        }

        /// <summary>
        /// 已领取奖励推送
        /// </summary>
        public void onFuncUnlockDone(ENPFunctionType funcType)
        {
            foreach (FuncUnlockInfo funcUnlockInfo in _m_dFuncUnlockInfoList)
            {
                if (null == funcUnlockInfo)
                    continue;

                if (funcUnlockInfo.funcType == funcType)
                {
                    funcUnlockInfo.setHadGetReward();
                    break;
                }
            }

            //领取功能解锁奖励
            WinMsg.SendMsg(WinMsgType.ON_FUNC_UNLOCK_GET_REWARD);
            //刷新红点
            refreshRedTip();
            //检查是否需要提示
            checkNewFuncUnlock();
            //刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化已领取列表
        /// </summary>
        public void reqPlayerFuncUnlockInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_029_ReqPlayerFuncUnlockInit());
        }

        /// <summary>
        /// 请求领取解锁奖励
        /// </summary>
        /// <param name="funcType"></param>
        /// <param name="_callback"></param>
        public void reqDoneFuncUnlock(ENPFunctionType funcType, Action<List<NPCommon_ItemInfo>> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_021_024_ReqDoneFuncUnlock(funcType),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_024_RetDoneFuncUnlock>(_msg =>
                {
                    _callback?.Invoke(_msg.getItemList());
                }));
        }
        
        /// <summary>
        /// 请求通知服务端功能已解锁
        /// </summary>
        /// <param name="_funcType"></param>
        public void reqClientNotifyFuncUnlock(ENPFunctionType _funcType)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_021_046_ReqClientNotifyFuncUnlock(_funcType),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_046_RetClientNotifyFuncUnlock>(_msg =>
                {
                }));
        }
        #endregion
    }
}
