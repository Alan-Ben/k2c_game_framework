using ALPackage;
using Common.ChildObj;
using GC2GS.p014_ChildOp;
using GS2GC.p002_InitOp;
using GS2GC.p014_ChildOp;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    public partial class PlayerChildComponent : _ANPBasicPlayerComponent
    {
        [ItemNotNull, NotNull] private readonly List<ChildInfo> _m_childList;
        [ItemNotNull, NotNull] private readonly List<SeatInfo> _m_seatList;
        [ItemNotNull, NotNull] private readonly List<UnmarriedInfo> _m_unmarriedAdultList;
        [ItemNotNull, NotNull] private readonly List<MarriedInfo> _m_marriedAdultList;
        [ItemNotNull, NotNull] private readonly List<AdultEngageRequestInfo> _m_engageRequestInfoList;
        [NotNull] private readonly RedTipDealer _m_redTipDealer;
        private ReadEngageRequestSaver _m_readEngageRequestSaver;

        private long _m_totalChildEarnings; // 所有子嗣的总收益（包括成年的）
        private long _m_totalAdultEarnings; // 所有成年子嗣的总收益
        
        
        public PlayerChildComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_childList = new List<ChildInfo>();
            _m_seatList = new List<SeatInfo>();
            _m_unmarriedAdultList = new List<UnmarriedInfo>();
            _m_marriedAdultList = new List<MarriedInfo>();
            _m_engageRequestInfoList = new List<AdultEngageRequestInfo>();
            _m_redTipDealer = new RedTipDealer(this);
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.CHILD; } }
        public override ENPPlayerCompType[] dependCompList { get { return new [] { ENPPlayerCompType.CONSORT }; } }
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 所有子嗣的总收益（包括成年的）
        /// </summary>
        public long totalChildEarnings { get { return _m_totalChildEarnings; } }
        /// <summary>
        /// 所有成年子嗣的总收益
        /// </summary>
        public long totalAdultEarnings { get { return _m_totalAdultEarnings; } }
        
        
        public event Action<SeatInfo> onSeatLazyCdChg;
        public event Action onEngageRequestReceiveChg;
        public event Action onUnmarriedAdultStateChg;
        public event Action onUnmarriedAdultCountChg;
        public event Action onMarriedAdultChg;
        public event Action onAdultEarningsChg;
        public event Action<ChildInfo> onChildLevelChg;
        

        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_013_ReqChildList(), 
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_013_RetChildList>(_msg =>
                { dealPreInitFunc(() => _initData(_msg)); }));
        }
        protected override void _dealInit()
        {
            // NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_013_ReqChildList(), 
            //     new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_013_RetChildList>(_msg =>
            //         { dealPreInitFunc(() => _initData(_msg)); }));
        }
        protected override void _onInitDone()
        {
            try
            {
                _m_readEngageRequestSaver = new ReadEngageRequestSaver(this);
                _m_readEngageRequestSaver.init();
            }
            catch (Exception e)
            {
                Debug.LogError($"{_m_readEngageRequestSaver.GetType()} init时出错：{e}");
                _m_readEngageRequestSaver.delete();
                _m_readEngageRequestSaver = new ReadEngageRequestSaver(this);
                _m_readEngageRequestSaver.init();
            }
            _m_redTipDealer.init();
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerChildComponent init fail");
        }
        protected override void _discard()
        {
            _m_redTipDealer.clear();
        }
        
        
        /// <summary>
        /// 获取未结业的子嗣列表
        /// </summary>
        public void getChildInfoList(List<ChildInfo> _childInfoList)
        {
            if (_childInfoList == null)
                return;

            _childInfoList.Clear();
            _childInfoList.AddRange(_m_childList);
        }
        [Pure]
        public ChildInfo getChildInfo(long _id)
        {
            foreach (ChildInfo childInfo in _m_childList)
            {
                if (childInfo.id == _id)
                    return childInfo;
            }

            return null;
        }
        [Pure]
        public SeatInfo getSeatInfo(long _id)
        {
            foreach (SeatInfo seatInfo in _m_seatList)
            {
                if (seatInfo.id == _id)
                    return seatInfo;
            }

            return null;
        }
        [Pure]
        public List<SeatInfo> getSeatList()
        {
            return new List<SeatInfo>(_m_seatList);
        }
        public void getSeatListNonAlloc(List<SeatInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_seatList);
        }
        [Pure, NotNull, ItemNotNull]
        public List<UnmarriedInfo> getUnmarriedChildList()
        {
            return new List<UnmarriedInfo>(_m_unmarriedAdultList);
        }
        public void getUnmarriedChildListNonAlloc(List<UnmarriedInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_unmarriedAdultList);
        }
        [Pure]
        public int getUnmarriedChildCount()
        {
            return _m_unmarriedAdultList.Count;
        }
        [Pure]
        public int getCanTrainChildCount()
        {
            int count = 0;
            for (int i = 0; i < _m_childList.Count; i++)
            {
                ChildInfo childInfo = _m_childList[i];
                if (childInfo != null && !childInfo.canGraduate() && !string.IsNullOrEmpty(childInfo.name))
                {
                    count++;
                }
            }
            return count;
        }
        [Pure]
        public List<MarriedInfo> getMarriedChildList()
        {
            return new List<MarriedInfo>(_m_marriedAdultList);
        }
        /// <summary>
        /// 通过成年子嗣id查找子嗣信息（未婚/已婚均支持），异步回调
        /// </summary>
        public void getAdultChildInfoById(long _adultId, Action<_IChildInfo> _complete)
        {
            if (_complete == null) return;
            foreach (UnmarriedInfo info in _m_unmarriedAdultList)
            {
                if (info != null && info.adultId == _adultId)
                {
                    _complete.Invoke(info.adultInfo);
                    return;
                }
            }
            foreach (MarriedInfo info in _m_marriedAdultList)
            {
                if (info != null && info.adultId == _adultId)
                {
                    info.getMyAdultInfo((_adultInfo) => _complete.Invoke(_adultInfo));
                    return;
                }
            }
            _complete.Invoke(null);
        }
        public void getMarriedChildListNonAlloc(List<MarriedInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_marriedAdultList);
        }
        [Pure]
        public List<AdultEngageRequestInfo> getEngageRequestInfoList()
        {
            clearDisableEngageRequest();
            return new List<AdultEngageRequestInfo>(_m_engageRequestInfoList);
        }
        public void getEngageRequestInfoListNonAlloc(List<AdultEngageRequestInfo> _list)
        {
            if (_list == null)
                return;

            clearDisableEngageRequest();
            
            _list.Clear();
            _list.AddRange(_m_engageRequestInfoList);
        }
        /// <summary>
        /// 获取已命名未毕业的子嗣数量
        /// </summary>
        public long getChildNamedCount()
        {
            long count = 0;
            for (int i = 0; i < _m_childList.Count; i++)
            {
                if(!string.IsNullOrEmpty(_m_childList[i].name))
                    count++;
            }

            return count;
        }
        /// <summary>
        /// 获取子嗣数量（包括未取名）
        /// </summary>
        /// <returns></returns>
        public long getChildTotalCount()
        {
            return _m_childList.Count;
        }
        [Pure]
        public bool isRead(AdultEngageRequestInfo _info)
        {
            return _m_readEngageRequestSaver?.isSaved(_info) ?? true;
        }
        public void readAllEngageRequest()
        {
            _m_redTipDealer.allEngageRequestRead();
            _m_readEngageRequestSaver?.readAllEngageRequest();
        }
        
        public void reqSetChildName(long _id, string _name, Action<bool> _complete = null)
        {
            ChildInfo childInfo = getChildInfo(_id);
            if (childInfo == null)
            {
                ALLog.Error($"[PlayerChildComponent] Error 找不到子嗣, id: {_id}");
                _complete?.Invoke(false);
                return;
            }

            if (!string.IsNullOrEmpty(childInfo.name))
            {
                ALLog.Error($"[PlayerChildComponent] Error 子嗣已经有名字了, id: {_id}, name: {childInfo.name}");
                _complete?.Invoke(false);
                return;
            }

            if (CharacterDetermineMgr.instance.isIllegal(_name))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_illegalNameTip_none);
                _complete?.Invoke(false);
                return;
            }

            if (!CharacterDetermineMgr.instance.isSuitableLength(_name, 1, GRefdataCoreMgr.instance.npGeneral.child_name_length_limit, true))
            {
                _complete?.Invoke(false);
                return;
            }

            GC2GS_014_001_ReqSetChildName protocol = GSWriter_014_ChildOp.make_001_ReqSetChildName(_id, _name);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_001_RetSetChildName>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqTrainChild(long _id, Action<bool, GS2GC_014_002_RetTrainChild> _complete = null)
        {
            ChildInfo childInfo = getChildInfo(_id);
            if (childInfo == null)
            {
                ALLog.Error($"[PlayerChildComponent] Error 找不到子嗣, id: {_id}");
                _complete?.Invoke(false, null);
                return;
            }
            
            if (string.IsNullOrEmpty(childInfo.name))
            {
                ALLog.Error($"[PlayerChildComponent] Error 子嗣还没有取名字, id: {_id}");
                _complete?.Invoke(false, null);
                return;
            }

            if (childInfo.canGraduate())
            {
                ALLog.Error($"[PlayerChildComponent] Error 子嗣已经可以毕业了, id: {_id}");
                _complete?.Invoke(false, null);
                return;
            }
            
            if (childInfo.seatInfo.energy <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_educatingEnergyEmpty_none);
                _complete?.Invoke(false, null);
                return;
            }
            
            if (!GCommon.isItemEnough(childInfo.getEducationCost(), true))
                return;

            // 采用客户端预测
            ClientRequestPrediction_014_002_TrainChild clientPrediction = new ClientRequestPrediction_014_002_TrainChild(_id);
            GS2GC_014_002_RetTrainChild clientPredictionResponse = clientPrediction.send();
            _complete?.Invoke(true, clientPredictionResponse);

            // // 采用原生服务端方式
            // GC2GS_014_002_ReqTrainChild protocol = GSWriter_014_ChildOp.make_002_ReqTrainChild(_id);
            // if (_complete != null)
            //     NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_002_RetTrainChild>(_complete));
            // else
            //     NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqAddSeatEnergy(long _seatId, int _addCount, Action<bool> _complete = null)
        {
            NPCommonItem consumeItem = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_item;
            if (!GCommon.isItemEnough(new NPCommonCostItem(consumeItem, _addCount), true))
            {
                _complete?.Invoke(false);
                return;
            }
            
            GC2GS_014_003_ReqAddSeatEnergy protocol = GSWriter_014_ChildOp.make_003_ReqAddSeatEnergy(_seatId, _addCount);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_003_RetAddSeatEnergy>((_isSuc, _msg) => _complete(_isSuc)));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }
        public void reqSetChildGraduate(long _id, Action<bool, GS2GC_014_004_RetSetChildGraduate> _complete = null)
        {
            ChildInfo childInfo = getChildInfo(_id);
            if (childInfo == null)
            {
                ALLog.Error($"[PlayerChildComponent] Error 找不到子嗣, id: {_id}");
                _complete?.Invoke(false, null);
                return;
            }
            
            if (!childInfo.canGraduate())
            {
                ALLog.Error($"[PlayerChildComponent] Error 子嗣等级不够, id: {_id}, lvl: {childInfo.level}");
                _complete?.Invoke(false, null);
                return;
            }

            GC2GS_014_004_ReqSetChildGraduate protocol = GSWriter_014_ChildOp.make_004_ReqSetChildGraduate(_id);
            if (_complete != null)
                NPGSClientListener.sendRequestByLog(protocol, new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_004_RetSetChildGraduate>(_complete));
            else
                NPGSClientListener.sendMsgByLog(protocol);
        }

        /// <summary>
        /// 请求玩家子嗣是否结婚的信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_adultId"></param>
        /// <param name="_complete"></param>
        public void reqCidAdultIsMarried(long _cid, long _adultId, Action<bool, GS2GC_014_030_RetCidAdultIsMarried> _complete)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_030_ReqCidAdultIsMarried(_cid, _adultId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_030_RetCidAdultIsMarried>((_isSuc, _msg) =>
                {
                    //本地记录已组队状态
                    if (_msg.getIsMarried())
                        AccountSettingMgr.instance.childSaver.addChatShareHadMarriedAdultId(_adultId);

                    _complete?.Invoke(_isSuc, _msg);
                }));
        }


        private void _initData(GS2GC_002_013_RetChildList _msg)
        {
            if (_msg == null)
            {
                setInitFail();
                return;
            }
            
            _m_seatList.Clear();
            List<Child_SeatInfo> serverSeatList = _msg.getSeatList();
            foreach (Child_SeatInfo serverSeat in serverSeatList)
            {
                if (serverSeat == null)
                    continue;
                
                SeatInfo seatInfo = new SeatInfo(serverSeat);
                // baseRef 在这里是有可能为 null 的，通过这里排除掉所有 ref 为 null 的建筑，后续可以默认 baseRef 不为 null
                if (seatInfo.baseRef == null)
                {
                    ALLog.Error("[PlayerChildComponent] Error 找不到席位的配置, seatId: " + serverSeat.getSeatId());
                    continue;
                }
                
                _m_seatList.Add(seatInfo);
            }
            
            _m_childList.Clear();
            List<Child_Info> serverChildList = _msg.getChildList();
            foreach (Child_Info serverChild in serverChildList)
            {
                if (serverChild == null)
                    continue;
                
                ChildInfo childInfo = new ChildInfo(serverChild);
                if (childInfo.guardian == null)
                {
                    ALLog.Error("[PlayerChildComponent] Error 找不到监护人, consortId: " + serverChild.getConsortId());
                    continue;
                }
                if (childInfo.initResRef == null || childInfo.qualityRef == null || childInfo.careerRef == null || childInfo.attrRef == null || childInfo.basicAttrRef == null)
                {
                    ALLog.Error($"[PlayerChildComponent] Error 找不到初始化配置, initResId: {serverChild.getInitResId()}, quality: {serverChild.getQuality()}, careerId: {serverChild.getCareerId()}");
                    continue;
                }
                if (childInfo.seatInfo == null)
                {
                    ALLog.Error("[PlayerChildComponent] Error 找不到席位, 或席位中已经有子嗣了, seatId: " + serverChild.getSeatId());
                    continue;
                }
                
                _m_childList.Add(childInfo);
            }

            _m_unmarriedAdultList.Clear();
            List<Adult_UnmarriedInfo> serverUnmarriedAdultList = _msg.getUnmarriedAdultList();
            foreach (Adult_UnmarriedInfo serverUnmarriedAdult in serverUnmarriedAdultList)
                _m_unmarriedAdultList.Add(new UnmarriedInfo(serverUnmarriedAdult));
            
            _m_marriedAdultList.Clear();
            List<long> serverMarriedAdultIdList = _msg.getMarriedAdultIdList();
            foreach (long serverMarriedAdultId in serverMarriedAdultIdList)
                _m_marriedAdultList.Add(new MarriedInfo(serverMarriedAdultId));
            while (_m_marriedAdultList.Count > GRefdataCoreMgr.instance.npGeneral.married_adult_limit)
                _m_marriedAdultList.RemoveAt(0);

            _m_engageRequestInfoList.Clear();
            List<Adult_ToMeApplyBaseInfo> serverEngageRequestInfoList = _msg.getApplyToMeBaseList();
            foreach (Adult_ToMeApplyBaseInfo serverEngageRequestInfo in serverEngageRequestInfoList)
            {
                if (serverEngageRequestInfo == null)
                    continue;
                
                _m_engageRequestInfoList.Add(new AdultEngageRequestInfo(serverEngageRequestInfo));
            }

            _m_totalChildEarnings = _msg.getBonus();
            _m_totalAdultEarnings = _msg.getAdultBonus();

            setInitDone();
        }
        private void clearDisableEngageRequest()
        {
            for (int i = _m_engageRequestInfoList.Count - 1; i >= 0; i--)
            {
                AdultEngageRequestInfo info = _m_engageRequestInfoList[i];
                if (!info.isEnable)
                {
                    _m_engageRequestInfoList.RemoveAt(i);
                    _m_redTipDealer.engageRequestRead(info);
                    _m_readEngageRequestSaver?.removeEngageRequest(info, false);
                }
            }
            
            _m_readEngageRequestSaver?.saveSetting();
        }


        internal void _onChildAdd(GS2GC_014_050_OnChildAdd _msg)
        {
            Child_Info serverChild = _msg?.getChild();
            if (serverChild == null)
                return;
            
            ChildInfo childInfo = new ChildInfo(serverChild);
            if (childInfo.guardian == null)
            {
                ALLog.Error("[PlayerChildComponent] Error 找不到监护人, consortId: " + serverChild.getConsortId());
                return;
            }
            if (childInfo.initResRef == null || childInfo.qualityRef == null || childInfo.careerRef == null || childInfo.attrRef == null || childInfo.basicAttrRef == null)
            {
                ALLog.Error($"[PlayerChildComponent] Error 找不到初始化配置, initResId: {serverChild.getInitResId()}, quality: {serverChild.getQuality()}, careerId: {serverChild.getCareerId()}");
                return;
            }
            if (childInfo.seatInfo == null)
            {
                ALLog.Error("[PlayerChildComponent] Error 找不到席位, 或席位中已经有子嗣了, seatId: " + serverChild.getSeatId());
                return;
            }
            
            _m_childList.Add(childInfo);
            _m_redTipDealer.onChildAddOrRemove();
            // // 这里是为了让子嗣的弹窗在妃子邀约之后弹出，先简单处理为延后一帧，方法比较 hack，后续可以考虑优化
            // ALCommonActionMonoTask.addNextFrameTask(() =>
            // {
            //     NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildGet(childInfo));
            // });
        }
        internal void _onChildNameChg(GS2GC_014_051_OnChildNameChg _msg)
        {
            if (_msg == null)
                return;
            
            ChildInfo childInfo = getChildInfo(_msg.getId());
            childInfo?._updateName(_msg.getName());
            _m_redTipDealer.onChildNamed();
        }
        internal void _onChildLvlChg(GS2GC_014_052_OnChildLvlChg _msg)
        {
            if (_msg == null)
                return;
            
            ChildInfo childInfo = getChildInfo(_msg.getId());
            childInfo?._updateLvl(_msg.getLvl());
            onChildLevelChg?.Invoke(childInfo);
        }
        internal void _onChildRemove(GS2GC_014_057_OnChildRemove _msg)
        {
            if (_msg == null)
                return;

            ChildInfo childInfo = getChildInfo(_msg.getId());
            if (childInfo == null)
                return;
            
            _m_childList.Remove(childInfo);
            childInfo.seatInfo._setChildInfo(null);
            _m_redTipDealer.onChildAddOrRemove();
        }
        internal void _onSeatChg(GS2GC_014_053_OnSeatChg _msg)
        {
            if (_msg == null)
                return;

            Child_SeatInfo serverSeat = _msg.getSeat();
            if (serverSeat == null)
                return;
            
            SeatInfo seatInfo = getSeatInfo(serverSeat.getSeatId());
            if (seatInfo == null)
                return;

            seatInfo._updateData(serverSeat);
            onSeatLazyCdChg?.Invoke(seatInfo);
            _m_redTipDealer.onSeatChg();
        }
        internal void _onUnmarriedAdultAdd(GS2GC_014_054_OnUnmarriedAdultAdd _msg)
        {
            if (_msg == null)
                return;
            
            _m_unmarriedAdultList.Add(new UnmarriedInfo(_msg.getAdult()));
            onUnmarriedAdultCountChg?.Invoke();
        }
        internal void _onMarriedAdultAdd(GS2GC_014_055_OnMarriedAdultAdd _msg)
        {
            if (_msg == null)
                return;

            _m_unmarriedAdultList.FindAndRemove(_info => _info.adultId == _msg.getAdult().getAdult().getId());
            MarriedInfo marriedInfo = new MarriedInfo(_msg.getAdult());
            _m_marriedAdultList.Add(marriedInfo);
            if (_m_marriedAdultList.Count > GRefdataCoreMgr.instance.npGeneral.married_adult_limit)
                _m_marriedAdultList.RemoveAt(0);
            onUnmarriedAdultCountChg?.Invoke();
            onMarriedAdultChg?.Invoke();
        }
        internal void _onToMeApplyAdd(GS2GC_014_056_OnToMeApplyAdd _msg)
        {
            if (_msg == null)
                return;

            AdultEngageRequestInfo info = new AdultEngageRequestInfo(_msg.getToMeApply());
            _m_engageRequestInfoList.Add(info);
            _m_redTipDealer.unreadEngageRequestAdd(info);
            clearDisableEngageRequest();
            onEngageRequestReceiveChg?.Invoke();
        }
        internal void _onToMeApplyDel(GS2GC_014_058_OnToMeApplyDel _msg)
        {
            if (_msg == null)
                return;

            AdultEngageRequestInfo info = _m_engageRequestInfoList.FindAndRemove(_info => _info.adultId == _msg.getApplyAdultId());
            _m_redTipDealer.engageRequestRead(info);
            _m_readEngageRequestSaver?.removeEngageRequest(info);
            onEngageRequestReceiveChg?.Invoke();
        }
        internal void _onUnmarriedAdultStatusChg(GS2GC_014_059_OnUnmarriedAdultStatusChg _msg)
        {
            if (_msg == null)
                return;

            UnmarriedInfo unmarriedInfo = _m_unmarriedAdultList.Find(_info => _info.adultId == _msg.getId());
            unmarriedInfo?.chgStatus(_msg.getStatus(), _msg.getExpiredTs(), _msg.getMinAllowBonus());
            onUnmarriedAdultStateChg?.Invoke();
        }
        internal void _onToMeApplyClear(GS2GC_014_060_OnToMeApplyClear _msg)
        {
            if (_msg == null)
                return;
            
            _m_engageRequestInfoList.Clear();
            _m_redTipDealer.allEngageRequestRead();
            _m_readEngageRequestSaver?.clearEngageRequest();
            onEngageRequestReceiveChg?.Invoke();
        }
        internal void _onChildBonusSumChg(GS2GC_014_061_OnChildBonusSumChg _msg)
        {
            if (_msg == null)
                return;
            
            _m_totalChildEarnings = _msg.getBonus();
            NPPlayer.instance.specialItemComp.goldData.recalEarnings();
        }
        internal void _onChildBonusChg(GS2GC_014_062_OnChildBonusChg _msg)
        {
            if (_msg == null)
                return;

            long childId = _msg.getId();
            ChildInfo childInfo = getChildInfo(childId);
            childInfo?._updateEarnings(_msg.getBonus());
        }
        internal void _onAdultBonusSumChg(GS2GC_014_063_OnAdultBonusSumChg _msg)
        {
            if (_msg == null)
                return;
            
            _m_totalAdultEarnings = _msg.getBonus();
            onAdultEarningsChg?.Invoke();
        }
    }
}