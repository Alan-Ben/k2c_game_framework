using System.Collections.Generic;
using System.Linq;
using ALPackage;
using Common.CommonFuncObj;
using GC2GS.p030_ShopOp;
using GS2GC.p002_InitOp;
using GS2GC.p030_ShopOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 礼包组件
    /// </summary>
    public class GiftPackComponent : _ANPBasicPlayerComponent
    {
        //礼包购买信息字典，<礼包id,GiftPackInfo>
        [NotNull] private Dictionary<long , GiftPackInfo> _m_giftPackInfoDict = new Dictionary<long, GiftPackInfo>();
        //待扣减的礼包次数，<礼包id,待扣减的次数>，在服务端更新购买次数前会先添加到这个字典中，等服务端更新购买次数后再清除，避免服务端未更新购买次数时超过限制次数购买
        [NotNull] private Dictionary<long , long> _m_pendingInfoDict = new Dictionary<long, long>();
        //定时任务
        private ALCommonEnableTaskController _m_checkTask;

        //构造函数
        public GiftPackComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = {};
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GIFT_PACK; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 活动页面礼包组类型列表
        /// </summary>
        public EGiftPackGroupShowType[] activityPageGroupTypeList { get { return new[] {EGiftPackGroupShowType.ACTIVITY, EGiftPackGroupShowType.GUILD }; } }
        /// <summary>
        /// 常驻页面礼包组类型列表
        /// </summary>
        public EGiftPackGroupShowType[] permanentPageGroupTypeList { get { return new[] { EGiftPackGroupShowType.DAILY, EGiftPackGroupShowType.WEEKLY, EGiftPackGroupShowType.RANK_RUSH }; } }
        /// <summary>
        /// 火星常驻页面礼包组类型列表
        /// </summary>
        public EGiftPackGroupShowType[] marsPermanentPageGroupTypeList { get { return new[] { EGiftPackGroupShowType.MARS_DAILY, EGiftPackGroupShowType.MARS_WEEKLY }; } }

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
            reqGiftPackInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, onActivityChg);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff += _onRemoveBuff;
            _startCheck();
        }

        public override void onAllCompInited()
        {
            refreshFreeRedTip();
            refreshFirstRechargeRedTip();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GiftPackComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, onActivityChg);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff -= _onRemoveBuff;
            _clear();
            _stopCheck();
        }

        //析构函数
        private void _clear()
        {
            _m_giftPackInfoDict?.Clear();
            _m_pendingInfoDict?.Clear();
        }

        #region 检查更新限购次数

        //开启定时检查
        private void _startCheck()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheck, 1.0f);
        }

        //关闭定时检查
        private void _stopCheck()
        {
            _m_checkTask.setDisable();
        }

        //每秒检查礼包是否需要刷新
        private void _tickCheck()
        {
            List<long> giftPackIdList = null;
            foreach (GiftPackInfo giftPackInfo in _m_giftPackInfoDict.Values)
            {
                if (giftPackInfo != null && !giftPackInfo.isRefreshing && giftPackInfo.nextRefreshTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    //如果礼包信息需要刷新，则添加到待刷新列表中
                    if (giftPackIdList == null)
                        giftPackIdList = new List<long>();
                    giftPackIdList.Add(giftPackInfo.giftPackId);
                    //标记为正在刷新
                    giftPackInfo.isRefreshing = true; 
                }
            }

            //如果有需要刷新的礼包，则请求刷新
            if (giftPackIdList != null && giftPackIdList.Count > 0)
                reqRefreshGiftPack(giftPackIdList);
        }

        #endregion

        /// <summary>
        /// 新增待扣减的礼包购买次数
        /// </summary>
        /// <param name="_giftPackId">礼包id</param>
        public void addPendingBuyCount(long _giftPackId)
        {
            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_giftPackId);
            //如果礼包购买次数是无限制的，则不需要处理
            if(giftPackRef == null || giftPackRef.isNotLimit)
                return;

            if (_m_pendingInfoDict.ContainsKey(_giftPackId))
                _m_pendingInfoDict[_giftPackId]++;
            else
                _m_pendingInfoDict[_giftPackId] = 1;

            // WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_CHG, _giftPackId);
        }

        /// <summary>
        /// 去除待扣减的礼包购买次数
        /// </summary>
        /// <param name="_giftPackId"></param>
        public void removePendingBuyCount(long _giftPackId)
        {
            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_giftPackId);
            //如果礼包购买次数是无限制的，则不需要处理
            if (giftPackRef == null || giftPackRef.isNotLimit)
                return;

            if (_m_pendingInfoDict.ContainsKey(_giftPackId))
            {
                _m_pendingInfoDict[_giftPackId]--;
                if (_m_pendingInfoDict[_giftPackId] <= 0)
                    _m_pendingInfoDict.Remove(_giftPackId);
            }

            // WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_CHG, _giftPackId);
        }

        /// <summary>
        /// 获取待扣减的礼包购买次数
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <returns></returns>
        public long getPendingBuyCount(long _giftPackId)
        {
            if (_m_pendingInfoDict.TryGetValue(_giftPackId, out long pendingCount))
                return pendingCount;
            else
                return 0;
        }

        /// <summary>
        /// 获取礼包剩余购买次数
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <returns></returns>
        public long getGiftPackLeftCount(long _giftPackId)
        {
            if(_m_giftPackInfoDict.TryGetValue(_giftPackId, out GiftPackInfo _info) && _info != null)
            {
                //无限制购买次数的礼包默认返回999
                if (_info.giftPackRef != null && _info.giftPackRef.isNotLimit)
                    return NPConst.GIFT_PACK_NOT_LIMIT_COUNT;

                //如果有待扣减的次数，则从剩余购买次数中减去待扣减的次数
                return _info.leftBuyCount - getPendingBuyCount(_giftPackId);
            }
            else
            {
                //如果没有找到礼包信息，则用限购次数计算
                GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_giftPackId);

                //无限制购买次数的礼包返回999
                if (giftPackRef != null && giftPackRef.isNotLimit)
                    return NPConst.GIFT_PACK_NOT_LIMIT_COUNT;

                return giftPackRef != null ? giftPackRef.buy_limit_count - getPendingBuyCount(_giftPackId) : 0;
            }
        }

        /// <summary>
        /// 获取礼包已购买次数
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <returns></returns>
        public long getGiftPackHadBuyCount(long _giftPackId)
        {
            if (_m_giftPackInfoDict.TryGetValue(_giftPackId, out GiftPackInfo _info) && _info != null)
            {
                return _info.hadBuyCount + getPendingBuyCount(_giftPackId); ; 
            }

            return getPendingBuyCount(_giftPackId);
        }

        /// <summary>
        /// 获取礼包下次刷新时间
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <returns></returns>
        public long getGiftPackNextRefreshTimeMs(long _giftPackId)
        {
            if (_m_giftPackInfoDict.TryGetValue(_giftPackId, out GiftPackInfo _info))
                return _info != null ? _info.nextRefreshTimeMs : 0;
            else
                return 0;
        }

        /// <summary>
        /// 获取礼包是否售罄（剩余购买次数 小于等于 0）
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <returns></returns>
        public bool isGiftPackSellOut(long _giftPackId)
        {
            GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_giftPackId);
            if (giftPackRef == null)
                return true;

            //无限制购买次数
            if (giftPackRef.isNotLimit)
                return false;

            return getGiftPackLeftCount(_giftPackId) <= 0;
        }

        /// <summary>
        /// 检查是否有可用的免费礼包
        /// </summary>
        /// <param name="_giftPackGroupRef"></param>
        /// <returns></returns>
        public bool checkHaveAvailableFreeGiftPack(GiftPackGroupRefObj _giftPackGroupRef)
        {
            if (_giftPackGroupRef == null || _giftPackGroupRef.gift_pack_id_list == null || _giftPackGroupRef.gift_pack_id_list.Count <= 0)
                return false;

            for (int i = 0; i < _giftPackGroupRef.gift_pack_id_list.Count; i++)
            {
                long giftPackId = _giftPackGroupRef.gift_pack_id_list[i];
                GiftPackRefObj giftPackRef = GRefdataCoreMgr.instance.giftPackRefCore.getRef(giftPackId);

                //判断是否是免费礼包并且礼包组展示中,还有剩余购买次数则返回true
                if (giftPackRef != null && giftPackRef.isFree && _giftPackGroupRef.canShow && getGiftPackLeftCount(giftPackId) > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 刷新免费礼包红点
        /// </summary>
        public void refreshFreeRedTip()
        {
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_ACTIVITY_TAB, 0);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_PERMANENT_TAB, 0);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_MARS_CASH_GIFT_PACK_PERMANENT_TAB, 0);

            GRefdataCoreMgr.instance.giftPackGroupRefCore.dealAllRef(_giftPackGroup =>
            {
                if (_giftPackGroup != null)
                {
                    long redTipCount = 0;
                    if (checkHaveAvailableFreeGiftPack(_giftPackGroup))
                        redTipCount++;

                    //如果有免费红点，设置到对应页签上
                    if (redTipCount > 0)
                    {
                        if (activityPageGroupTypeList.Contains(_giftPackGroup.show_type))
                            RedTipMgr.instance.addCountByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_ACTIVITY_TAB, redTipCount);

                        if (permanentPageGroupTypeList.Contains(_giftPackGroup.show_type))
                            RedTipMgr.instance.addCountByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_PERMANENT_TAB, redTipCount);

                        if (marsPermanentPageGroupTypeList.Contains(_giftPackGroup.show_type))
                            RedTipMgr.instance.addCountByRefRedTipId(RedTipConst.RED_MARS_CASH_GIFT_PACK_PERMANENT_TAB, redTipCount);
                    }
                }
            });
        }

        /// <summary>
        /// 刷新首充礼包红点
        /// </summary>
        public void refreshFirstRechargeRedTip()
        {
            long redTipCount = 0;
            long hadBuyCount = getGiftPackHadBuyCount(GRefdataCoreMgr.instance.npGeneral.first_recharge_gift_pack_id);
            if (hadBuyCount > 0)
            {
                GRefdataCoreMgr.instance.firstRechargeDayRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null)
                    {
                        NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(_ref.buff_id);
                        bool canGetReward = buffInfo != null && buffInfo.layer > 0 && (_ref.condition == null || _ref.condition.isEmpty || _ref.condition.IsEnable(null));
                        if (canGetReward)
                            redTipCount++;
                    }
                });
            }

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FIRST_RECHARGE, redTipCount);
        }

        /// <summary>
        /// 新增礼包信息
        /// </summary>
        /// <param name="_info"></param>
        private void _addGiftPackInfo(GiftPack_Info _info)
        {
            if (_info == null)
                return;

            long giftPackId = _info.getGiftPackId();
            
            GiftPackInfo giftPackInfo = new GiftPackInfo(_info);
            if (null == giftPackInfo.giftPackRef)
            {
                ALLog.Error($"can not find gift packe ref - id:{_info.getGiftPackId()}");
                return;
            }

            _m_giftPackInfoDict[giftPackId] =  giftPackInfo;

            //处理待扣减的购买次数
            _dealPendingBuyCount(giftPackInfo, giftPackInfo.giftPackRef.buy_limit_count, giftPackInfo.nextRefreshTimeMs);
            
            WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_ADD, _info.getGiftPackId());
            
            // 因为是新增的礼包信息，所以对服务器来说, 改礼包之前一定是没有购买记录的, 即没有购买过, 所以已购买次数为可购买次数上限
            long oriLeftBuyCount = giftPackInfo.giftPackRef != null && !giftPackInfo.giftPackRef.isNotLimit ? giftPackInfo.giftPackRef.buy_limit_count : NPConst.GIFT_PACK_NOT_LIMIT_COUNT;
            long curLeftBuyCount = giftPackInfo.leftBuyCount;//获取服务器给的的剩余可购买次数
            if(oriLeftBuyCount != curLeftBuyCount)
                WinMsg.SendMsg(WinMsgType.ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG, giftPackId, oriLeftBuyCount, curLeftBuyCount);
        }

        /// <summary>
        /// 更新礼包信息
        /// </summary>
        /// <param name="_info"></param>
        private void _updateGiftPackInfo(GiftPack_Info _info, bool _sendMsg = true)
        {
            if (_info == null)
                return;

            if (_m_giftPackInfoDict.TryGetValue(_info.getGiftPackId(), out GiftPackInfo giftPackInfo) && giftPackInfo != null)
            {
                //原先的剩余购买次数和下次刷新时间
                long oriLeftCount = giftPackInfo.leftBuyCount;
                long oriNextRefreshTime = giftPackInfo.nextRefreshTimeMs;

                //更新礼包信息
                giftPackInfo.updateInfo(_info);

                //处理待扣减的购买次数
                _dealPendingBuyCount(giftPackInfo, oriLeftCount, oriNextRefreshTime);

                //如果需要发送消息，则发送消息
                if (_sendMsg)
                    WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_CHG, _info.getGiftPackId());
                
                long curLeftBuyCount = giftPackInfo.leftBuyCount;//获取服务器给的的剩余可购买次数
                if(oriLeftCount != curLeftBuyCount)
                    WinMsg.SendMsg(WinMsgType.ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG, _info.getGiftPackId(), oriLeftCount, curLeftBuyCount);
            }
            else
            {
                _addGiftPackInfo(_info);
            }
        }

        /// <summary>
        /// 移除礼包信息
        /// </summary>
        /// <param name="_giftPackId"></param>
        private void _removeGiftPackInfo(long _giftPackId)
        {
            _m_giftPackInfoDict.TryGetValue(_giftPackId, out GiftPackInfo giftPackInfo);
            _m_giftPackInfoDict.Remove(_giftPackId);
            _m_pendingInfoDict.Remove(_giftPackId);

            WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_REMOVE, _giftPackId);
            
            long oriLeftBuyCount = giftPackInfo?.leftBuyCount ?? 0;//获取移除前的剩余购买次数
            // 移除后, 礼包变为没有购买记录, 即剩余购买次数变为限购次数上限
            long curLeftBuyCount = giftPackInfo != null && giftPackInfo.giftPackRef != null && !giftPackInfo.giftPackRef.isNotLimit ? giftPackInfo.giftPackRef.buy_limit_count : NPConst.GIFT_PACK_NOT_LIMIT_COUNT;
            if(oriLeftBuyCount != curLeftBuyCount)
                WinMsg.SendMsg(WinMsgType.ON_SERVER_GIFT_PACK_LEFT_BUY_COUNT_CHG, _giftPackId, oriLeftBuyCount, curLeftBuyCount);
        }

        /// <summary>
        /// 处理待扣减的购买次数
        /// </summary>
        /// <param name="_giftPackInfo"></param>
        /// <param name="_oriLeftCount"></param>
        /// <param name="_oriNextRefreshTime"></param>
        private void _dealPendingBuyCount(GiftPackInfo _giftPackInfo, long _oriLeftCount, long _oriNextRefreshTime)
        {
            if(_giftPackInfo == null || _giftPackInfo.giftPackRef == null || _giftPackInfo.giftPackRef.isNotLimit)
                return;

            //当前的剩余购买次数和下次刷新时间
            long curLeftCount = _giftPackInfo.leftBuyCount;
            long curNextRefreshTime = _giftPackInfo.nextRefreshTimeMs;

            //礼包id
            long giftPackId = _giftPackInfo.giftPackId;

            //如果剩余购买次数变少了，则需要更新待扣减次数字典
            if (_oriLeftCount > curLeftCount)
            {
                if (_m_pendingInfoDict.ContainsKey(giftPackId))
                {
                    _m_pendingInfoDict[giftPackId] = _m_pendingInfoDict[giftPackId] - (_oriLeftCount - curLeftCount);
                    if (_m_pendingInfoDict[giftPackId] <= 0)
                        _m_pendingInfoDict.Remove(giftPackId);
                }
            }//如果剩余购买次数变多了说明是重置了，则清空待扣减的次数
            else if (_oriLeftCount < curLeftCount)
            {
                _m_pendingInfoDict.Remove(giftPackId);
            }//如果购买次数一样，刷新时间变化了说明是重置了，则清空待扣减的次数
            else if (_oriNextRefreshTime != curNextRefreshTime)
            {
                _m_pendingInfoDict.Remove(giftPackId);
            }
        }

        #region 消息事件
        
        /// <summary>
        /// 活动变化
        /// </summary>
        /// <param name="_objects"></param>
        private void onActivityChg(params object[] _objects)
        {
            refreshFreeRedTip();
        }
        /// <summary>
        /// buff变化消息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_layer"></param>
        /// <param name="_curLeftTimeMS"></param>
        private void _onBuffChg(NPPlayerBuffInfo _info, int _layer, long _curLeftTimeMS)
        {
            refreshFirstRechargeRedTip();
        }

        /// <summary>
        /// buff移除消息
        /// </summary>
        /// <param name="_info"></param>
        private void _onRemoveBuff(NPPlayerBuffInfo _info)
        {
            refreshFirstRechargeRedTip();
        }

        /// <summary>
        /// 发生跨天
        /// </summary>
        private void _onCrossDay()
        {
            refreshFirstRechargeRedTip();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 礼包初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retGiftPackInit(GS2GC_002_070_RetGiftPackInit _msg)
        {
            if (_msg == null)
                return;

            //先清空数据
            _clear();

            //初始化礼包列表
            for (int i = 0; i < _msg.getInfoList().Count; i++)
            {
                _addGiftPackInfo(_msg.getInfoList()[i]);
            }

            setInitDone();
        }

        /// <summary>
        /// 礼包刷新推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGiftPackRefresh(GS2GC_030_060_OnGiftPackRefresh _msg)
        {
            if(_msg == null)
                return;

            List<long> giftPackIdList = new List<long>();
            for (int i = 0; i < _msg.getGiftPack().Count; i++)
            {
                giftPackIdList.Add(_msg.getGiftPack()[i].getGiftPackId());
                _updateGiftPackInfo(_msg.getGiftPack()[i], false);
            }
            refreshFreeRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GIFT_PACK_LIST_REFRESH, giftPackIdList);
        }

        /// <summary>
        /// 礼包变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGiftPackBuyRecordChg(GS2GC_030_061_OnGiftPackBuyRecordChg _msg)
        {
            if (_msg == null)
                return;

            _updateGiftPackInfo(_msg.getGiftPackInfo());
            refreshFreeRedTip();
        }

        /// <summary>
        /// 礼包新增推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGiftPackBuyRecordAdd(GS2GC_030_062_OnGiftPackBuyRecordAdd _msg)
        {
            if(_msg == null)
                return;

            _addGiftPackInfo(_msg.getGiftPackInfo());
            refreshFreeRedTip();
        }

        /// <summary>
        /// 礼包记录移除推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGiftPackBuyRecordRemove(GS2GC_030_063_OnGiftPackBuyRecordRemove _msg)
        {
            if (_msg == null)
                return;

            foreach (long giftPackId in _msg.getGiftPackIdList())
            {
                _removeGiftPackInfo(giftPackId);
            }
            refreshFreeRedTip();
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求礼包初始化
        /// </summary>
        public void reqGiftPackInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_070_ReqGiftPackInit());
        }

        /// <summary>
        /// 请求刷新礼包
        /// </summary>
        /// <param name="giftPackList"></param>
        public void reqRefreshGiftPack(List<long> giftPackList)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_030_021_ReqRefreshGiftPack(giftPackList),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_021_RetRefreshGiftPack>(null));
        }

        /// <summary>
        /// 请求购买礼包（免费或者道具购买）
        /// </summary>
        /// <param name="_giftPackId"></param>
        public void reqBuyGiftPack(long _giftPackId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_030_022_ReqBuyGiftPack(_giftPackId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_022_RetBuyGiftPack>(null));
        }

        #endregion
    }
}
