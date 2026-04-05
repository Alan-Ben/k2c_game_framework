using ALPackage;
using Common.InnObj;
using Common.PrivilegeCardEnum;
using Common.PrivilegeCardObj;
using CommonEnum;
using GC2GS.p021_PlayerInfo;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using ILRuntime.CLR.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 权益卡组件
    /// </summary>
    public class PrivilegeCardComponent : _ANPBasicPlayerComponent
    {
        //权益卡信息字典
        [NotNull] private Dictionary<EPrivilegeCardType, PrivilegeCardInfo> _m_dPrivilegeCardDic = new Dictionary<EPrivilegeCardType, PrivilegeCardInfo>();
        //玩家属性容器
        [NotNull] private NPPlayerPropertyContainer _m_playerPropertyContainer = new NPPlayerPropertyContainer("privilegeCard");
        //权益卡bonus管理器
        [NotNull] private readonly CommonUnionBonusMgr _m_privilegeCardBonusMgr = new CommonUnionBonusMgr(EUnionBonusMgrTag.PRIVILEGE_CARD);
        //定时任务
        private ALCommonEnableTaskController _m_checkTask;
        //需要移除的权益卡类型列表
        [NotNull] private List<EPrivilegeCardType> _m_lNeedRemoveType = new List<EPrivilegeCardType>();

        //构造函数
        public PrivilegeCardComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PRIVILEGE_CARD; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 玩家属性容器
        /// </summary>
        public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_playerPropertyContainer; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqPrivilegeCardInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_privilegeCardBonusMgr.setParent(NPPlayer.instance.playerBonusMgr);
            _startCheck();
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PrivilegeCardComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            _clear();
        }

        public override void onAllCompInited()
        {
        }

        //析构函数
        private void _clear()
        {
            _m_privilegeCardBonusMgr.clear();
            _m_dPrivilegeCardDic.Clear();
            _m_lNeedRemoveType.Clear();
            _stopCheck();
        }

        /// <summary>
        /// 根据类型获取权益卡信息
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public PrivilegeCardInfo getPrivilegeCardInfo(EPrivilegeCardType _type)
        {
            if (_m_dPrivilegeCardDic.TryGetValue(_type, out PrivilegeCardInfo cardInfo))
                return cardInfo;

            return null;
        }

        /// <summary>
        /// 获取属性加成值
        /// </summary>
        /// <param name="_cardType"></param>
        /// <param name="_propType"></param>
        /// <returns></returns>
        public long getAddPropValue(EPrivilegeCardType _cardType, EBonusPropertyType _propType)
        {
            //是否拥有并且激活该权益卡
            if (_m_dPrivilegeCardDic.TryGetValue(_cardType, out PrivilegeCardInfo info) && info != null && info.isActivate && info.refObj != null && info.refObj.bonus_prop_modifier != null)
                return info.refObj.bonus_prop_modifier.getPropValue(_propType);

            return 0;
        }

        /// <summary>
        /// 更新玩家属性
        /// </summary>
        /// <param name="_oldModifier"></param>
        /// <param name="_newModifier"></param>
        private void _replacePlayerProperty(NPPlayerPropertyModifier _oldModifier, NPPlayerPropertyModifier _newModifier)
        {
            if(_oldModifier != null)
                _m_playerPropertyContainer.removeModifier(_oldModifier);

            if(_newModifier != null)
                _m_playerPropertyContainer.addModifier(_newModifier);
        }

        /// <summary>
        /// 更新属性加成
        /// </summary>
        /// <param name="_oldModifier"></param>
        /// <param name="_newModifier"></param>
        private void _replaceBonus(PlayerBonusPropertyModifier _oldModifier, PlayerBonusPropertyModifier _newModifier)
        {
            if(_oldModifier != null)
                _m_privilegeCardBonusMgr.removeTotalModifier(_oldModifier);

            if (_newModifier != null)
                _m_privilegeCardBonusMgr.addTotalModifier(_newModifier);
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            long redTipCount = 0;
            foreach (PrivilegeCardInfo cardInfo in _m_dPrivilegeCardDic.Values)
            {
                if (cardInfo != null && cardInfo.canGetRewardToday)
                    redTipCount++;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_PRIVILEGE_CARD, redTipCount);
        }

        /// <summary>
        /// 发生了跨天
        /// </summary>
        private void _onCrossDay()
        {
            _refreshRedTip();
        }

        /// <summary>
        /// 开启定时检查
        /// </summary>
        private void _startCheck()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheck, 1.0f);
        }

        /// <summary>
        /// 关闭定时检查
        /// </summary>
        private void _stopCheck()
        {
            _m_checkTask.setDisable();
        }

        /// <summary>
        /// 定时检查
        /// </summary>
        private void _tickCheck()
        {
            //获取需要移除的权益卡类型
            _m_lNeedRemoveType.Clear();
            foreach (PrivilegeCardInfo cardInfo in _m_dPrivilegeCardDic.Values)
            {
                if (cardInfo != null && !cardInfo.isActivate)
                    _m_lNeedRemoveType.Add(cardInfo.cardType);
            }

            //移除过期的权益卡
            if (_m_lNeedRemoveType.Count > 0)
            {
                foreach (EPrivilegeCardType cardType in _m_lNeedRemoveType)
                {
                    if(!_m_dPrivilegeCardDic.ContainsKey(cardType))
                        continue;

                    PrivilegeCardInfo cardInfo = _m_dPrivilegeCardDic[cardType];
                    //移除玩家属性
                    _replacePlayerProperty(cardInfo?.refObj?.player_pro, null);
                    //移除属性加成
                    _replaceBonus(cardInfo?.refObj?.bonus_prop_modifier, null);
                    //移除数据
                    _m_dPrivilegeCardDic.Remove(cardType);
                    //刷新建筑属性加成
                    NPPlayer.instance.buildingComp.recalAllBusinessBuilding();
                    //发送消息
                    WinMsg.SendMsg(WinMsgType.ON_PRIVILEGE_CARD_REMOVE, cardType);
                }
                _m_lNeedRemoveType.Clear();
                //刷新红点
                _refreshRedTip();
            }
        }

        #region S2C

        /// <summary>
        /// 权益卡初始化
        /// </summary>
        /// <param name="_msg"></param>
        public void retPrivilegeCardInit(GS2GC_002_027_RetPrivilegeCardInit _msg)
        {
            if (_msg == null)
                return;

            for (int i = 0; i < _msg.getCardList().Count; i++)
            {
                PrivilegeCardObj_Info cardInfo = _msg.getCardList()[i];
                if(cardInfo == null)
                    continue;

                PrivilegeCardInfo info = new PrivilegeCardInfo(cardInfo);
                if (info.isActivate)
                {
                    _m_dPrivilegeCardDic[cardInfo.getCardType()] = info;
                    //更新玩家属性
                    _replacePlayerProperty(null, info.refObj?.player_pro);
                    //更新属性加成
                    _replaceBonus(null, info?.refObj?.bonus_prop_modifier);
                }
            }

            //刷新红点
            _refreshRedTip();

            setInitDone();
        }

        /// <summary>
        /// 权益卡变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onPrivilegeCardChg(GS2GC_021_054_OnPrivilegeCardChg _msg)
        {
            if (_msg == null || _msg.getCard() == null) 
                return;

            //原本是否激活
            bool oriIsActivate = false;
            PrivilegeCardInfo cardInfo = null;

            if (_m_dPrivilegeCardDic.TryGetValue(_msg.getCard().getCardType(), out cardInfo) && cardInfo != null)
            {
                oriIsActivate = cardInfo.isActivate;
                cardInfo.updateInfo(_msg.getCard());
            }
            else
            {
                oriIsActivate = false;
                cardInfo = new PrivilegeCardInfo(_msg.getCard());
                //如果有效，则加入字典
                if (cardInfo.isActivate)
                    _m_dPrivilegeCardDic[_msg.getCard().getCardType()] = cardInfo;
            }

            //如果之前未激活，当前已激活，则更新属性加成
            if (!oriIsActivate && cardInfo.isActivate)
            {
                //更新玩家属性
                _replacePlayerProperty(null, cardInfo.refObj?.player_pro);
                //更新属性加成
                _replaceBonus(null, cardInfo.refObj?.bonus_prop_modifier);
            }

            //如果之前已激活，当前未激活，则移除属性加成和数据
            if (oriIsActivate && !cardInfo.isActivate)
            {
                //移除玩家属性
                _replacePlayerProperty(cardInfo.refObj?.player_pro, null);
                //移除属性加成
                _replaceBonus(cardInfo.refObj?.bonus_prop_modifier, null);
                //移除数据
                _m_dPrivilegeCardDic.Remove(_msg.getCard().getCardType());
                //发送消息
                WinMsg.SendMsg(WinMsgType.ON_PRIVILEGE_CARD_REMOVE, _msg.getCard().getCardType());
            }

            //刷新红点
            _refreshRedTip();
            //刷新建筑属性加成
            NPPlayer.instance.buildingComp.recalAllBusinessBuilding();

            //发送消息
            WinMsg.SendMsg(WinMsgType.ON_PRIVILEGE_CARD_CHG, _msg.getCard().getCardType());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求权益卡初始化
        /// </summary>
        public void reqPrivilegeCardInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_027_ReqPrivilegeCardInit());
        }

        /// <summary>
        /// 请求领取权益卡每日奖励
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_callback"></param>
        public void reqGainPrivilegeCardDailyReward(EPrivilegeCardType _type, Action<bool, GS2GC_021_004_RetGainPrivilegeCardDailyReward> _callback = null)
        {
            //请求领奖
            NPGSClientListener.sendRequestByLog(new GC2GS_021_004_ReqGainPrivilegeCardDailyReward(_type),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_021_004_RetGainPrivilegeCardDailyReward>(_callback));
        }

        #endregion
    }
}
