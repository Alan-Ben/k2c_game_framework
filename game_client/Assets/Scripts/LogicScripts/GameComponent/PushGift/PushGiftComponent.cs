using System;
using System.Collections.Generic;
using ALPackage;
using Common.CommonFuncObj;
using Common.PushGiftObj;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 推送礼包组件
    /// </summary>
    public class PushGiftComponent : _ANPBasicPlayerComponent
    {
        //属性
        protected static ENPPlayerCompType[] _g_DependComp = {};
        
        private List<PushGiftGroupInfo> _m_lPushGiftGroupInfoList;//推送礼包组信息列表
        private CommonCountDownInfoMgr<PushGiftPackInfo> _m_PushGiftPackCountDownMgr;//推送礼包倒计时管理器
        
        //构造函数
        public PushGiftComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PUSH_GIFT; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public event Action onCountDownTick;
        public event Action<PushGiftPackInfo> onCountDownFinish;//倒计时完成
        public event Action<PushGiftPackInfo, PushGiftPackInfo> onEarliestFinishCountDownTargetChg;//最早结束倒计时目标变化
        
        public IReadOnlyList<PushGiftGroupInfo> pushGiftGroupInfoList { get { return _m_lPushGiftGroupInfoList; } }
        
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqPushGiftPackList();
        }

        protected override void _dealInit()
        {
            if (_m_PushGiftPackCountDownMgr == null)
            {
                _m_PushGiftPackCountDownMgr = new CommonCountDownInfoMgr<PushGiftPackInfo>(1f);
                _m_PushGiftPackCountDownMgr.onCountDownTick += _onCountDownTick;
                _m_PushGiftPackCountDownMgr.onCountDownFinish += _onCountDownFinish;
                _m_PushGiftPackCountDownMgr.onEarliestFinishCountDownTargetChg += _onEarliestFinishCountDownTargetChg;
            }
            _m_PushGiftPackCountDownMgr.clear();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG, _onGiftPackLeftBuyCountChg);
        }

        public override void onAllCompInited()
        {
            //TODO: 所有组件初始化完成后的处理
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
        }

        //组件销毁时的调用
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG, _onGiftPackLeftBuyCountChg);
            
            onCountDownTick = null;
            onCountDownFinish = null;
            onEarliestFinishCountDownTargetChg = null;

            _clear();
            _m_PushGiftPackCountDownMgr = null;
            _m_lPushGiftGroupInfoList = null;
        }

        #region 倒计时管理器

        private void _onCountDownTick()
        {
            onCountDownTick?.Invoke();
        }

        private void _onCountDownFinish(PushGiftPackInfo _info)
        {
            if(_info == null)
                return;

            _disablePushGiftPack(_info);
            
            onCountDownFinish?.Invoke(_info);
        }
        
        private void _onEarliestFinishCountDownTargetChg(PushGiftPackInfo _oldInfo, PushGiftPackInfo _newInfo)
        {
            onEarliestFinishCountDownTargetChg?.Invoke(_oldInfo, _newInfo);
        }
        
        #endregion

        #region 数据处理

        /// <summary>
        /// 清除数据
        /// </summary>
        private void _clear()
        {
            removeAllGiftPackTriggerPopWndNotice();//移除所有礼包触发弹窗Notice
            _m_lPushGiftGroupInfoList?.Clear();
            _m_PushGiftPackCountDownMgr?.clear();
        }

        /// <summary>
        /// 根据协议数据初始化推送礼包组信息列表
        /// </summary>
        /// <param name="_groupInfoList"></param>
        private void _initPushGiftGroupInfoList(List<PushGift_GroupInfo> _groupInfoList)
        {
            if (_groupInfoList == null)
                return;
            
            if (_m_lPushGiftGroupInfoList == null)
                _m_lPushGiftGroupInfoList = new List<PushGiftGroupInfo>();
            _m_lPushGiftGroupInfoList.Clear();
            
            for (int i = 0; i < _groupInfoList.Count; i++)
            {
                PushGift_GroupInfo groupProto = _groupInfoList[i];
                if (groupProto == null)
                    continue;
                
                // 创建礼包组信息
                PushGiftGroupInfo groupInfo = new PushGiftGroupInfo(groupProto.getGroupId());
                _m_lPushGiftGroupInfoList.Add(groupInfo);
                
                groupInfo.setLastTriggerTimeMs(groupProto.getLastTriggerTimeMs());
                
                // 如果有当前激活的推送礼包，创建并设置
                PushGift_ActivePackInfo packProto = groupProto.getActivePack();
                if (packProto != null)
                {
                    PushGiftPackInfo packInfo = new PushGiftPackInfo(packProto.getPushGiftId());
                    packInfo.setActivateTimeMs(packProto.getActivateTimeMs());
                    packInfo.setRead(packProto.getHasRead());
                    packInfo.isAutoTriggered = packProto.getIsAutoActive();
                    
                    groupInfo.setCurPushGiftPackInfo(packInfo);
                    
                    // 如果推送礼包有效，添加到倒计时管理器
                    if (packInfo.isValid)
                    {
                        _m_PushGiftPackCountDownMgr?.addCountDown(packInfo);
                    }

                    // 若礼包未读，添加触发弹窗通知
                    if (!packInfo.hasRead)
                    {
                        // 因为是初始化回包, 所以这时展示为登录触发弹窗
                        packInfo.showTriggerPopWndNotice(EMainCityPushNoticeTriggerType.LOGIN);
                    }
                }
            }
        }

        private void _onPushGiftPackGroupInfoChg(PushGift_GroupInfo _serverGroupInfo)
        {
            if(_serverGroupInfo == null)
                return;
            
            PushGiftGroupInfo clientGroupInfo = getPushGiftGroupInfoByIdWithCreate(_serverGroupInfo.getGroupId());
            clientGroupInfo.setLastTriggerTimeMs(_serverGroupInfo.getLastTriggerTimeMs());
            
            PushGift_ActivePackInfo serverPackInfo = _serverGroupInfo.getActivePack();
            PushGiftPackInfo preGiftPackInfo = clientGroupInfo.curPushGiftPackInfo;
            bool preGiftPackValid = preGiftPackInfo?.isValid ?? false;//没有礼包视为无效
            bool preGiftPackRead = preGiftPackInfo?.hasRead ?? true;//没有礼包视为已读
            PushGiftPackInfo newGiftPackInfo = null;
            if (serverPackInfo != null)
            {
                // 前一个礼包信息为空或与当前礼包ID不同，视为新礼包
                if (preGiftPackInfo == null || preGiftPackInfo.pushGiftPackId != serverPackInfo.getPushGiftId())
                {
                    newGiftPackInfo = new PushGiftPackInfo(serverPackInfo.getPushGiftId());
                    newGiftPackInfo.setActivateTimeMs(serverPackInfo.getActivateTimeMs());
                    newGiftPackInfo.setRead(serverPackInfo.getHasRead());
                    newGiftPackInfo.isAutoTriggered = serverPackInfo.getIsAutoActive();
                }// 前一个礼包id与需要更新的礼包id相同
                else
                {
                    newGiftPackInfo = preGiftPackInfo;
                    newGiftPackInfo.setActivateTimeMs(serverPackInfo.getActivateTimeMs());
                    newGiftPackInfo.setRead(serverPackInfo.getHasRead());
                    newGiftPackInfo.isAutoTriggered = serverPackInfo.getIsAutoActive();
                }
            }

            if (newGiftPackInfo == null)//若没有新礼包信息
            {
                if (preGiftPackValid) //若前一个礼包信息有效, 表示需要移除推送礼包
                {
                    _disablePushGiftPack(preGiftPackInfo);
                    
                    // 为礼包组设置新的礼包信息
                    clientGroupInfo.setCurPushGiftPackInfo(null);
                }
                else//前一个礼包也无效
                {
                    // 为礼包组设置新的礼包信息
                    clientGroupInfo.setCurPushGiftPackInfo(null);
                }
            }
            else
            {
                if (preGiftPackValid && !newGiftPackInfo.isValid)//若上一个礼包有效, 更新后礼包无效
                {
                    // 使上一个礼包无效
                    _disablePushGiftPack(preGiftPackInfo);
                    
                    // 为礼包组设置新的礼包信息
                    clientGroupInfo.setCurPushGiftPackInfo(newGiftPackInfo);
                }
                else if (!preGiftPackValid && !newGiftPackInfo.isValid)//若上一个礼包无效, 更新后礼包也无效
                {
                    // 为礼包组设置新的礼包信息
                    clientGroupInfo.setCurPushGiftPackInfo(newGiftPackInfo);
                }
                else if (!preGiftPackValid && newGiftPackInfo.isValid)//若上一个礼包无效, 更新后礼包有效, 说明新触发礼包
                {
                    _addNewActivePushGiftPack(clientGroupInfo, newGiftPackInfo);
                }
                else//若上一个礼包有效, 且更新后礼包也有效
                {
                    // 若上一个礼包与新礼包不是同一礼包，视为触发新礼包
                    if (preGiftPackInfo != newGiftPackInfo || preGiftPackInfo.pushGiftPackId != newGiftPackInfo.pushGiftPackId)
                    {
                        // 使上一个礼包无效
                        _disablePushGiftPack(preGiftPackInfo);
                    
                        _addNewActivePushGiftPack(clientGroupInfo, newGiftPackInfo);
                    }
                    else// 与上一礼包是同一个礼包, 说明是单纯的信息变更
                    {
                        // 若前一个礼包已读且当前未读, 则触发弹窗通知
                        if(preGiftPackRead && !newGiftPackInfo.hasRead)
                            newGiftPackInfo.showTriggerPopWndNotice(EMainCityPushNoticeTriggerType.OTHER);
                        
                        WinMsg.SendMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, newGiftPackInfo);
                    }
                }
            }
        }
        
        /// <summary>
        /// 处理无效推送礼包
        /// </summary>
        /// <param name="_pushGiftPackInfo"></param>
        private void _disablePushGiftPack(PushGiftPackInfo _pushGiftPackInfo)
        {
            if (_pushGiftPackInfo == null)
                return;
            
            // 若 该礼包还有效 且 该礼包所属礼包组的当前激活推送礼包是该礼包, 则将所属礼包组的当前激活推送礼包置空
            if(_pushGiftPackInfo.isValid && _pushGiftPackInfo.pushGiftGroupInfo != null && _pushGiftPackInfo.pushGiftGroupInfo.curPushGiftPackInfo == _pushGiftPackInfo)
                _pushGiftPackInfo.pushGiftGroupInfo?.setCurPushGiftPackInfo(null);
            
            // 从倒计时管理器中移除
            _m_PushGiftPackCountDownMgr?.removeCountDown(_pushGiftPackInfo);
            // 移除触发弹窗通知
            _pushGiftPackInfo.removeTriggerPopWndNotice(true);
            
            WinMsg.SendMsg(WinMsgType.PUSH_GIFT_PACK_DISABLE, _pushGiftPackInfo);
        }

        /// <summary>
        /// 添加新推送礼包, 只有当新礼包有效时才处理
        /// </summary>
        /// <param name="_belongGroupInfo">所属礼包组</param>
        /// <param name="_newPushGiftPackInfo">新推送礼包信息</param>
        private void _addNewActivePushGiftPack(PushGiftGroupInfo _belongGroupInfo, PushGiftPackInfo _newPushGiftPackInfo)
        {
            // 所属礼包组信息不能为空
            if(_belongGroupInfo == null)
                return;

            if (_newPushGiftPackInfo == null || !_newPushGiftPackInfo.isValid)//调用方保证新礼包信息有效, 因为这里是添加新有效礼包
            {
                Debug.LogError($"PushGiftComponent::_activeNewPushGiftPack error: _newPushGiftPackInfo is null or invalid!");
                return;
            }
            
            // 注意这边不对_belongGroupInfo中的curPushGiftPackInfo(即_belongGroupInfo的上一个推送礼包)失效处理, 这个方法只负责激活新礼包, 失效处理由调用方负责
            
            _belongGroupInfo.setCurPushGiftPackInfo(_newPushGiftPackInfo);
            
            _m_PushGiftPackCountDownMgr?.addCountDown(_newPushGiftPackInfo);
            
            if(!_newPushGiftPackInfo.hasRead)
                _newPushGiftPackInfo.showTriggerPopWndNotice(EMainCityPushNoticeTriggerType.OTHER);
            
            WinMsg.SendMsg(WinMsgType.TRIGGER_NEW_PUSH_GIFT_PACK, _newPushGiftPackInfo);
        }

        /// <summary>
        /// 请求设置推送礼包为已读
        /// </summary>
        public void reqSetPushGiftPackRead(PushGiftPackInfo _pushGiftPackInfo)
        {
            // 若礼包信息为空或已读, 则不处理
            if(_pushGiftPackInfo == null || _pushGiftPackInfo.hasRead)
                return;
            
            _pushGiftPackInfo.setRead(true);//客户端先直接设置为已读
            if(_pushGiftPackInfo.pushGiftGroupInfo == null)//所属礼包组信息为空, 不处理
                return;
            
            // 向服务端请求标记为已读
            reqMarkPushGiftAsRead(_pushGiftPackInfo.pushGiftGroupInfo.pushGiftGroupId, _pushGiftPackInfo.pushGiftPackId,
                (_isSucc) =>
                {
                    if(!_isSucc)// 若没有请求成功, 则将礼包重新设置为未读
                        _pushGiftPackInfo.setRead(false);
                });
        }
        
        #endregion

        /// <summary>
        /// 通过组ID获取推送礼包组信息
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        public PushGiftGroupInfo getPushGiftGroupInfoById(long _groupId)
        {
            if (_m_lPushGiftGroupInfoList == null)
                return null;
            
            foreach (PushGiftGroupInfo groupInfo in _m_lPushGiftGroupInfoList)
            {
                if (groupInfo != null && groupInfo.pushGiftGroupId == _groupId)
                    return groupInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 通过组ID获取推送礼包组信息, 查找不到则创建
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        [NotNull] public PushGiftGroupInfo getPushGiftGroupInfoByIdWithCreate(long _groupId)
        {
            if(_m_lPushGiftGroupInfoList == null)
                _m_lPushGiftGroupInfoList = new List<PushGiftGroupInfo>();
            
            foreach (PushGiftGroupInfo groupInfo in _m_lPushGiftGroupInfoList)
            {
                if (groupInfo != null && groupInfo.pushGiftGroupId == _groupId)
                    return groupInfo;
            }

            PushGiftGroupInfo newGroupInfo = new PushGiftGroupInfo(_groupId);
            _m_lPushGiftGroupInfoList.Add(newGroupInfo);
            return newGroupInfo;
        }

        /// <summary>
        /// 处理所有推送礼包组信息
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllPushGiftPack(Action<PushGiftGroupInfo> _action)
        {
            if(_m_lPushGiftGroupInfoList == null || _action == null)
                return;

            foreach (var groupInfo in _m_lPushGiftGroupInfoList)
            {
                _action(groupInfo);
            }
        }
        
        /// <summary>
        /// 尝试触发推送礼包
        /// </summary>
        /// <param name="_groupId"></param>
        /// <param name="_tryTriggerDone">尝试触发操作完成, 不管是否真的触发出推送礼包都调用</param>
        /// <returns></returns>
        public void tryTriggerPushGiftPack(long _groupId, bool _isAfterBuyAutoTrigger, Action<PushGiftGroupInfo> _tryTriggerDone = null)
        {
            PushGiftGroupInfo groupinfo = getPushGiftGroupInfoByIdWithCreate(_groupId);
            groupinfo.tryTriggerPushGiftPack(_isAfterBuyAutoTrigger, ()=>
            {
                _tryTriggerDone?.Invoke(groupinfo);
            });
        }
        
        #region 触发礼包弹窗处理

        /// <summary>
        /// 移除所有礼包触发弹窗Notice
        /// </summary>
        public void removeAllGiftPackTriggerPopWndNotice()
        {
            NPUINoticeMgr.instance.removeDealer((_noticeDealer) =>
            {
                if (_noticeDealer != null && (_noticeDealer is NoticeDealer_PushGiftPackTriggerPop _pushGiftPackTriggerPopNotice))
                {
                    PushGiftPackInfo pushGiftPackInfo = _pushGiftPackTriggerPopNotice.pushGiftPackInfo;
                    if(pushGiftPackInfo != null)
                        pushGiftPackInfo.removeTriggerPopWndNotice(false);

                    return true;
                }
                
                return false;
            });
        }

        /// <summary>
        /// 移除可以在指定节点弹出显示的礼包触发弹窗Notice
        /// </summary>
        /// <param name="_nodeTag"></param>
        public void removeGiftPackTriggerPopWndNoticeByShowNodeTag(string _nodeTag)
        {
            if (_m_lPushGiftGroupInfoList == null)
                return;

            NPUINoticeMgr.instance.removeDealer((_noticeDealer) =>
            {
                if (_noticeDealer != null && (_noticeDealer is NoticeDealer_PushGiftPackTriggerPop _pushGiftPackTriggerPopNotice))
                {
                    PushGiftPackInfo pushGiftPackInfo = _pushGiftPackTriggerPopNotice.pushGiftPackInfo;
                    if(pushGiftPackInfo?.pushGiftPackRefObj?.isCanPopShowNode(_nodeTag) ?? false)
                    {
                        pushGiftPackInfo.removeTriggerPopWndNotice(false);
                        return true;
                    }
                }
                
                return false;
            });
        }

        /// <summary>
        /// 展示可以在指定节点弹出显示的礼包触发弹窗Notice
        /// </summary>
        /// <param name="_nodeTag"></param>
        public void showGiftPackTriggerPopWndNoticeByShowNodeTag(string _nodeTag, EMainCityPushNoticeTriggerType _pushNoticeTriggerType)
        {
            if (_m_lPushGiftGroupInfoList == null)
                return;

            foreach (var pushGiftGroupInfo in _m_lPushGiftGroupInfoList)
            {
                if(pushGiftGroupInfo == null || pushGiftGroupInfo.curPushGiftPackInfo == null || 
                   pushGiftGroupInfo.curPushGiftPackInfo.hasRead || !pushGiftGroupInfo.curPushGiftPackInfo.isValid
                   || pushGiftGroupInfo.curPushGiftPackInfo.pushGiftPackRefObj == null)
                    continue;
                
                if(pushGiftGroupInfo.curPushGiftPackInfo.pushGiftPackRefObj.isCanPopShowNode(_nodeTag))
                {
                    pushGiftGroupInfo.curPushGiftPackInfo.showTriggerPopWndNotice(_pushNoticeTriggerType);
                }
            }
        }
        
        #endregion
        
        #region 消息监听

        /// <summary>
        /// 礼包剩余购买次数变化回调
        /// 参数: _objs[0] = giftPackId (long), _objs[1] = oriLeftBuyCount (int), _objs[2] = curLeftBuyCount (int)
        /// </summary>
        private void _onGiftPackLeftBuyCountChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 3 || 
                !(_objs[0] is long _giftPackId) ||
                !(_objs[1] is long _oriLeftBuyCount) ||
                !(_objs[2] is long _curLeftBuyCount))
                return;

            if (_m_lPushGiftGroupInfoList != null)
            {
                foreach (var pushGiftGroupInfo in _m_lPushGiftGroupInfoList)
                {
                    // 找到对应礼包ID的推送礼包信息
                    if(pushGiftGroupInfo == null)
                        continue;

                    PushGiftPackInfo curPushGiftPackInfo = pushGiftGroupInfo.curPushGiftPackInfo;
                    if(curPushGiftPackInfo == null || curPushGiftPackInfo.giftPackId != _giftPackId)
                        continue;
                    
                    if (_oriLeftBuyCount <= 0 && _curLeftBuyCount > 0)//若原来没有剩余购买次数, 现在有了剩余购买次数
                    {
                        // 判断当前推送礼包是否有效, 若有效则相当于又重新触发了礼包
                        if (curPushGiftPackInfo.isValid)
                        {
                            _addNewActivePushGiftPack(pushGiftGroupInfo, curPushGiftPackInfo);
                        }
                    }
                    else if(_oriLeftBuyCount > 0 && _curLeftBuyCount <= 0)//若原来有剩余购买次数, 现在没有剩余购买次数
                    {
                        // 判断当前推送礼包是否无效, 若无效则需要使其失效处理
                        if (!curPushGiftPackInfo.isValid)
                        {
                            _disablePushGiftPack(curPushGiftPackInfo);
                            
                            // 由于购买导致的自动触发, 由服务端进行推送, 客户端不需要再尝试触发下一个礼包
                            // // 当推送礼包的可购买次数变为0时, 需要尝试是否可以自动触发下一个礼包
                            // if (curPushGiftPackInfo.leftCanBuyCount <= 0)
                            //     curPushGiftPackInfo.tryAfterBuyAutoTriggerNextPushGiftPack();
                        }
                    }
                    else// 单纯的购买次数变化
                    {
                        // 判断当前推送礼包还有效
                        if (curPushGiftPackInfo.isValid)
                        {
                            WinMsg.SendMsg(WinMsgType.PUSH_GIFT_PACK_INFO_CHG, curPushGiftPackInfo);
                        }
                    }
                }       
            }
        }
        
        #endregion
        
        #region S2C

        /// <summary>
        /// 推送礼包列表初始化响应
        /// </summary>
        /// <param name="_msg"></param>
        public void retPushGiftPackList(GS2GC_002_081_RetPushGiftPackList _msg)
        {
            if (_msg == null)
                return;
            
            // 清空原有数据
            _clear();
            
            // 初始化推送礼包组信息列表
            _initPushGiftGroupInfoList(_msg.getGroupList());
            
            // 设置初始化完成
            setInitDone();
        }

        /// <summary>
        /// 礼包组变更推送
        /// </summary>
        public void onPushGiftPackGroupChg(GS2GC_004_074_OnPushGiftPackGroupChg _msg)
        {
            if(_msg == null || !isInitDone)
                return;
            
            _onPushGiftPackGroupInfoChg(_msg.getGroupInfo());
        }
        
        #endregion

        #region C2S

        /// <summary>
        /// 请求推送礼包列表
        /// </summary>
        public void reqPushGiftPackList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_081_ReqPushGiftPackList());
        }

        /// <summary>
        /// 请求触发推送礼包
        /// </summary>
        public void reqTriggerPushGiftGroup(long groupId, Action<bool> _reqCallback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_046_ReqTriggerPushGiftGroup(groupId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_046_RetTriggerPushGiftGroup>((_isSuc, _msg) =>
                {
                    _reqCallback?.Invoke(_isSuc);
                }));
        }
        
        /// <summary>
        /// 请求标记推送礼包为已读
        /// </summary>
        public void reqMarkPushGiftAsRead(long _groupId, long _pushGiftId, Action<bool> _reqCallback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_048_ReqMarkPushGiftAsRead(_groupId, _pushGiftId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_048_RetMarkPushGiftAsRead>((_isSuc, _msg) =>
                {
                    _reqCallback?.Invoke(_isSuc);
                }));
        }

        #endregion
    }
}
