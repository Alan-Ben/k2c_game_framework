using System;
using System.Collections.Generic;
using ALPackage;
using GC2GS.p002_InitOp;
using GC2GS.p042_GuildRelatedOp;
using GS2GC.p002_InitOp;
using GS2GC.p042_GuildRelatedOp;
using Common.GuildObj;
using Common.GuildEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱管理组件
    /// </summary>
    public class GuildBoxComponent : _ANPBasicPlayerComponent
    {
        // 宝箱数量映射表
        [NotNull] private Dictionary<EGuildBoxType, GuildBoxTypeDetail> _m_boxTypeMap = new Dictionary<EGuildBoxType, GuildBoxTypeDetail>();
        // 当前宝箱活跃点
        private long _m_lActivePoint;
        // 当前宝箱活跃点对应联盟等级
        private int _m_iTargetGuildLevel;
        // 是否匿名分享联盟宝箱
        private bool _m_bIsGuildBoxShareAnonymous;

        public GuildBoxComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        private static readonly ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.GUILD };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GUILD_BOX; } }
        public override ENPPlayerCompType[] dependCompList => _g_DependComp;

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 是否匿名分享联盟宝箱
        /// </summary>
        public bool isGuildBoxShareAnonymous { get { return _m_bIsGuildBoxShareAnonymous; } set { _m_bIsGuildBoxShareAnonymous = value; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqGuildBoxInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.RegisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            //检查并移除过期宝箱
            checkAndRemoveInvalidBox();
            //刷新红点提示
            _refreshRedTip();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            foreach (GuildBoxTypeDetail boxTypeDetail in _m_boxTypeMap.Values)
            {
                boxTypeDetail?.discard();
            }
            _m_boxTypeMap?.Clear();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
        }

        /// <summary>
        /// 获取指定类型宝箱的数量
        /// </summary>
        public int getBoxCount(EGuildBoxType _boxType)
        {
            if ( _m_boxTypeMap.TryGetValue(_boxType, out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null) 
                return boxTypeDetail.canGainCount;
            return 0;
        }

        /// <summary>
        /// 获取指定id的宝箱数量
        /// </summary>
        /// <param name="_boxId"></param>
        /// <returns></returns>
        public int getBoxCount(long _boxId)
        {
            int count = 0;
            foreach (GuildBoxTypeDetail boxTypeDetail in _m_boxTypeMap.Values)
            {
                if(boxTypeDetail != null)
                    count += boxTypeDetail.getBoxCountById(_boxId);
            }
            return count;
        }

        public List<GuildBoxInfo> getGuildBoxList(EGuildBoxType _boxType)
        {
            if ( _m_boxTypeMap.TryGetValue(_boxType, out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null) 
                return boxTypeDetail.getBoxList();
            return new List<GuildBoxInfo>();
        }

        /// <summary>
        /// 获取当前活跃宝箱活跃点
        /// </summary>
        /// <returns></returns>
        public long getActiveBoxCurScore()
        {
            return _m_lActivePoint;
        }

        /// <summary>
        /// 获取当前活跃宝箱目标活跃点
        /// </summary>
        /// <returns></returns>
        public long getActiveBoxMaxScore()
        {
            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(_m_iTargetGuildLevel);
            return guildLevelRef != null ? guildLevelRef.gain_box_need_active_point : 0;
        }

        /// <summary>
        /// 获取目标活跃宝箱id
        /// </summary>
        /// <returns></returns>
        public long getTargetActiveBoxId()
        {
            GuildLevelRefObj guildLevelRef = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(_m_iTargetGuildLevel);
            return guildLevelRef != null ? guildLevelRef.gain_guild_box_id : 0;
        }

        /// <summary>
        /// 根据宝箱类型获取新增宝箱数量
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public long getBoxAddCountByType(EGuildBoxType _type)
        {
            if(_m_boxTypeMap.TryGetValue(_type, out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null)
                return boxTypeDetail.addCount;
            return 0;
        }

        /// <summary>
        /// 检查并移除过期宝箱
        /// </summary>
        public bool checkAndRemoveInvalidBox()
        {
            //是否有移除过期宝箱
            bool isRemoved = false;

            //检查当前宝箱数据
            isRemoved = (_m_boxTypeMap.ContainsKey(EGuildBoxType.GUILD_ACTIVE_BOX) && _m_boxTypeMap[EGuildBoxType.GUILD_ACTIVE_BOX].checkAndRemoveInvalidBox());
            isRemoved = (_m_boxTypeMap.ContainsKey(EGuildBoxType.GUILD_FREE_BOX) && _m_boxTypeMap[EGuildBoxType.GUILD_FREE_BOX].checkAndRemoveInvalidBox()) || isRemoved;
            isRemoved = (_m_boxTypeMap.ContainsKey(EGuildBoxType.GUILD_GIFT_BOX) && _m_boxTypeMap[EGuildBoxType.GUILD_GIFT_BOX].checkAndRemoveInvalidBox()) || isRemoved;

            //检查本地保存的宝箱数据
            isRemoved = AccountSettingMgr.instance.guildSaver.clearInvalidBox() || isRemoved;

            //如果有移除过期宝箱则刷新红点
            if (isRemoved)
                _refreshRedTip();

            return isRemoved;
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (NPPlayer.instance.guildComp.isJoinGuild())
            {
                // 统计所有类型宝箱的总数
                if (_m_boxTypeMap != null)
                {
                    int freeBoxCount = 0;
                    int giftBoxCount = 0;
                    int activeBoxCount = 0;
                    if (_m_boxTypeMap.TryGetValue(EGuildBoxType.GUILD_FREE_BOX, out GuildBoxTypeDetail freeBoxTypeDetail) && freeBoxTypeDetail != null)
                    {
                        freeBoxCount = freeBoxTypeDetail.canGainCount;
                        //判断是否超出每日免费宝箱上限
                        NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.guild_box_claim_fixed_cd_id);
                        if (fixedCdInfo != null)
                        {
                            if (freeBoxCount > fixedCdInfo.getCount())
                                freeBoxCount = fixedCdInfo.getCount();
                        }
                    }
                    if(_m_boxTypeMap.TryGetValue(EGuildBoxType.GUILD_GIFT_BOX, out GuildBoxTypeDetail giftBoxTypeDetail) && giftBoxTypeDetail != null)
                        giftBoxCount = giftBoxTypeDetail.canGainCount;
                    if (_m_boxTypeMap.TryGetValue(EGuildBoxType.GUILD_ACTIVE_BOX, out GuildBoxTypeDetail activeBoxTypeDetail) && activeBoxTypeDetail != null)
                        activeBoxCount = activeBoxTypeDetail.canGainCount;
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_FREE_BOX, freeBoxCount);
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_GIFT_BOX, giftBoxCount);
                    RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_ACTIVE_BOX, activeBoxCount);
                }
            }
            else
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_FREE_BOX, 0);
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_GIFT_BOX, 0);
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_ACTIVE_BOX, 0);
            }
        }


        #region 消息事件

        /// <summary>
        /// 离开联盟
        /// </summary>
        private void _onLeaveGuild()
        {
            // 清空数据
            foreach (GuildBoxTypeDetail boxTypeDetail in _m_boxTypeMap.Values)
            {
                boxTypeDetail?.discard();
            }
            _m_boxTypeMap?.Clear();
            _m_lActivePoint = 0;
            _m_iTargetGuildLevel = 0;

            // 刷新红点
            _refreshRedTip();
        }

        /// <summary>
        /// 加入联盟
        /// </summary>
        private void _onJoinGuild()
        {
            //请求初始化
            _reqGuildBoxInit();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 初始化回包
        /// </summary>
        public void retGuildBoxInit(GS2GC_002_082_RetGuildBoxInit _msg)
        {
            _m_boxTypeMap?.Clear();
            if (_msg != null)
            {
                List<Guild_BoxInfo> boxList = _msg.getBoxList();
                if (boxList != null)
                {
                    List<Guild_BoxInfo> freeBoxList = new List<Guild_BoxInfo>();
                    List<Guild_BoxInfo> giftBoxList = new List<Guild_BoxInfo>();
                    List<Guild_BoxInfo> activeBoxList = new List<Guild_BoxInfo>();
                    foreach (Guild_BoxInfo countInfo in boxList)
                    {
                        if (countInfo != null)
                        {
                            GuildBoxRefObj boxRef = GRefdataCoreMgr.instance.guildBoxRefCore.getRef(countInfo.getBoxId());
                            if (boxRef != null )
                            {
                                if(boxRef.type == EGuildBoxType.GUILD_FREE_BOX)
                                    freeBoxList.Add(countInfo);
                                else if(boxRef.type == EGuildBoxType.GUILD_GIFT_BOX)
                                    giftBoxList.Add( countInfo);
                                else if(boxRef.type == EGuildBoxType.GUILD_ACTIVE_BOX)
                                    activeBoxList.Add(countInfo);
                            }
                            
                        }
                    }
                    _m_boxTypeMap.Add(EGuildBoxType.GUILD_ACTIVE_BOX, new GuildBoxTypeDetail(EGuildBoxType.GUILD_ACTIVE_BOX, activeBoxList));
                    _m_boxTypeMap.Add(EGuildBoxType.GUILD_GIFT_BOX, new GuildBoxTypeDetail(EGuildBoxType.GUILD_GIFT_BOX, giftBoxList));
                    _m_boxTypeMap.Add(EGuildBoxType.GUILD_FREE_BOX, new GuildBoxTypeDetail(EGuildBoxType.GUILD_FREE_BOX, freeBoxList));
                }
            }

            // 设置活跃点和当前活跃点对应等级
            _m_lActivePoint = _msg.getActivePoint();
            _m_iTargetGuildLevel = _msg.getTargetLvl();
            _m_bIsGuildBoxShareAnonymous = _msg.getIsGuildBoxShareAnonymous();

            // 刷新红点提示
            _refreshRedTip();

            if (!isInitDone)
                setInitDone();
        }

        /// <summary>
        /// 联盟宝箱奖励物品列表推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildBoxRewardShow(GS2GC_042_056_OnGuildBoxRewardShow _msg)
        {
            if (_msg == null)
                return;

            if (_m_boxTypeMap.TryGetValue(_msg.getBoxType(), out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null)
            {
                boxTypeDetail.updateBoxReward(_msg.getRewardList());
            }

            // 刷新红点提示
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_BOX_REWARD_SHOW);
        }

        /// <summary>
        /// 新增联盟宝箱推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildBoxAdd(GS2GC_042_057_OnGuildBoxAdd _msg)
        {
            if (_msg == null)
                return;
            if (_m_boxTypeMap.TryGetValue(_msg.getBoxType(), out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null)
            {
                boxTypeDetail.addNewBoxList(_msg.getBoxList());
            }
            else
            {
                _m_boxTypeMap[_msg.getBoxType()] = new GuildBoxTypeDetail(_msg.getBoxType(), _msg.getBoxList());
            }

            // 刷新红点提示
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_BOX_DETAIL_ADD);
        }

        /// <summary>
        /// 联盟宝箱数量变更推送
        /// </summary>
        public void onGuildBoxAddCountChg(GS2GC_042_055_OnGuildBoxAddCountChg _msg)
        {
            if (_msg == null)
                return;

            if ( _m_boxTypeMap.TryGetValue(_msg.getBoxType(), out GuildBoxTypeDetail boxTypeDetail) && boxTypeDetail != null) 
                boxTypeDetail.addBoxAddCount(_msg.getAddCount());

            // 刷新红点提示
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_BOX_COUNT_CHG, _msg.getBoxType());
        }

        /// <summary>
        /// 联盟活跃点变化数据
        /// </summary>
        /// <param name="_msg"></param>
        public void onGuildActivePointChg(GS2GC_042_058_OnGuildActivePointChg _msg)
        {
            if (_msg == null)
                return;

            _m_lActivePoint = _msg.getActivePoint();
            _m_iTargetGuildLevel = _msg.getTargetLvl();

            // 刷新红点提示
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_GUILD_BOX_ACTIVE_POINT_CHG);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqGuildBoxInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_082_ReqGuildBoxInit());
        }

        /// <summary>
        /// 获取可领取联盟宝箱列表
        /// </summary>
        /// <param name="_boxType">宝箱类型</param>
        /// <param name="_callback">回调，返回宝箱列表</param>
        public void reqGetGuildBoxList(EGuildBoxType _boxType, Action _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_005_ReqGetGuildBoxList(_boxType),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_042_005_RetGetGuildBoxList>((_msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        /// <summary>
        /// 领取联盟宝箱奖励列表
        /// </summary>
        /// <param name="_boxType">宝箱类型</param>
        /// <param name="_callback">回调，返回是否成功</param>
        public void reqGainGuildRewardBoxList(EGuildBoxType _boxType, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_006_ReqGainGuildRewardBoxList(_boxType),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_042_006_RetGainGuildRewardBoxList>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 领取单个联盟宝箱奖励
        /// </summary>
        /// <param name="_boxType">宝箱类型</param>
        /// <param name="_id">宝箱实例ID</param>
        /// <param name="_callback">回调，返回是否成功</param>
        public void reqGainGuildRewardBox(EGuildBoxType _boxType, long _id, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_007_ReqGainGuildRewardBox(_boxType, _id),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_042_007_RetGainGuildRewardBox>((_succ, _msg) =>
                {
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 请求设置联盟宝箱匿名分享
        /// </summary>
        /// <param name="_isAnonymous"></param>
        public void reqSetGuildBoxShareAnonymous(bool _isAnonymous)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_008_ReqSetGuildBoxShareAnonymous(_isAnonymous),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_042_008_RetSetGuildBoxShareAnonymous>(null));
        }

        #endregion
    }
}