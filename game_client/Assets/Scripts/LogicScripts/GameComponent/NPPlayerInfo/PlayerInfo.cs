using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;

using NPEnum;

namespace GOE
{
    //玩家数据
    public partial class PlayerInfo : PlayerBaseInfo
    {
        //属性
        /*/玩家属性容器*/
        [NotNull]protected NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
        //临时变量
        private PlayerLvlRefObj _m_lrCurLevelRef;
        private PlayerLvlRefObj _m_lrNextLevelRef;
        private PlayerLvlRefObj _m_lrPreLevelRef;
        private PlayerSkinRefObj _m_curSkinRef;
        private RedTipDealer _m_redDealer;

        public PlayerInfo() : base()
        {
            _m_lrCurLevelRef = null;
            _m_lrNextLevelRef = null;
            _m_lrPreLevelRef = null;
            _m_curSkinRef = null;

            _m_redDealer = new RedTipDealer(this);
            //玩家属性容器
            _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer("玩家加成");
        }


        public event Action onLevelChg;
        public PlayerSkinRefObj curSkinRef { get { return _m_curSkinRef; } }
        public PlayerLvlRefObj curLevelRef { get { return _m_lrCurLevelRef; } }
        public PlayerLvlRefObj nextLevelRef { get { return _m_lrNextLevelRef; } }
        public PlayerLvlRefObj preLevelRef { get { return _m_lrPreLevelRef; } }
        //玩家属性容器
        public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_pcPlayerPropertyContainer; } }

        //释放资源函数
        public override void clear()
        {
            base.clear();
            _m_redDealer?.clear();
        }

        /// <summary>
        /// 刷新所有红点
        /// </summary>
        public void refrshAllRedTip()
        {
            _m_redDealer?.refrshAll();
        }

        /// <summary>
        /// 刷新解锁大臣红点
        /// </summary>
        public void refreshUnlockHeroRedTip()
        {
            _m_redDealer?.refreshHeroUnlockState();
        }

        //CS参数更新
        public bool updatePlayerParam(int _index, long _paramValue)
        {
            //服务端枚举更新导致越界
            if(_m_arrPlayerParam.Length <= _index)
            {
                Debug.LogError_EditorOnly(string.Format("玩家参数更新类型数组越界～ 数组长度：{0}，index: {1}", _m_arrPlayerParam.Length, _index));
                return false;
            }

            long origParamValue = _m_arrPlayerParam[_index];
            _m_arrPlayerParam[_index] = _paramValue;

            //值变化
            if(origParamValue != _paramValue)
            {
                //处理值变化导致的更新
                _checkUpdate((NPEnum.ENPPlayerParam)_index, origParamValue, _paramValue);
                //广播玩家参数更新消息
                WinMsg.SendMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _index, _paramValue);
            }
            return true;
        }

        /// <summary>
        /// 如果变更的CS数据是动态数据，那么需要对变化作出响应处理
        /// </summary>
        /// <param name="_paramType">参数类型</param>
        /// <param name="_origValue">原值</param>
        /// <param name="_curValue">当前值</param>
        private void _checkUpdate(NPEnum.ENPPlayerParam _paramType, long _origValue, long _curValue)
        {
            switch(_paramType)
            {
                case NPEnum.ENPPlayerParam.LEVEL:
                    //如果等级到达1级、2级、4级，发送第三方埋点
                    // if (_curValue == 1)
                    //     GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.LEVEL1);
                    // if (_curValue == 2)
                    //     GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.LEVEL2);
                    // if (_curValue == 4)
                    //     GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.LEVEL4);

                    //减去上一级的属性
                    if (_m_lrCurLevelRef != null)
                    {
                        _m_pcPlayerPropertyContainer.removeModifier(_m_lrCurLevelRef.player_property);
                    }
                    //加上当前等级的属性
                    _m_lrCurLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(_curValue);
                    if(_m_lrCurLevelRef != null)
                    {
                        _m_pcPlayerPropertyContainer.addModifier(_m_lrCurLevelRef.player_property);
                    }
                    _m_lrNextLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(_curValue + 1);
                    _m_lrPreLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(_curValue - 1);

                    // 默认玩家信息的大臣解锁只和等级有关，如果有需要增加其它条件，再修改
                    _m_redDealer?.refreshHeroUnlockState();
                    _m_redDealer?.refreshLevelUpState();
                    
                    //发送消息刷新自定义加载prefab
                    GCommon.reloadCustomLoadPrefab();
                    onLevelChg?.Invoke();
                    break;
                case ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE:
                    _m_redDealer?.refreshDailyRewardState();
                    break;
                case ENPPlayerParam.PLAYER_SKIN:
                    _m_curSkinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_curValue);
                    //如果没有皮肤，显示默认的
                    if (_m_curSkinRef == null)
                        _m_curSkinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.default_player_skin);
                    break;
                case ENPPlayerParam.VIP_LVL:
                    VipRefObj oriVipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_origValue);
                    VipRefObj curVipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_curValue);
                    if(oriVipRef != null)
                        _m_pcPlayerPropertyContainer.removeModifier(oriVipRef.player_property);
                    if(curVipRef != null)
                        _m_pcPlayerPropertyContainer.addModifier(curVipRef.player_property);
                    //刷新红点
                    _m_redDealer?.refreshVipRewardRedTip();
                    _m_redDealer?.refreshVipRechargeRewardRedTip();
                    //显示VIP升级弹窗
                    for (long i = _origValue + 1; i <= _curValue; i++)
                    {
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_VIPLevelUpgrade(i));
                        
                        VipRefObj vipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(i);
                        if(vipRef != null)//尝试触发vip升级推送礼包
                            NPPlayer.instance.pushGiftComp.tryTriggerPushGiftPack(vipRef.trigger_push_gift_group_id, false);
                    }
                    break;
                case ENPPlayerParam.HAD_DRAW_VIP_REWARD_LIST:
                    _m_redDealer?.refreshVipRewardRedTip();
                    break;
                case ENPPlayerParam.HAD_DRAW_VIP_RECHARGE_REWARD_LIST:
                    _m_redDealer?.refreshVipRechargeRewardRedTip();
                    break;
            }
        }

        //获取玩家VIP等级
        public long getCurrentVIPLvl()
        {
            return this[NPEnum.ENPPlayerParam.VIP_LVL] ;
        }

        //获取玩家当前等级
        public long getCurrentLevel()
        {
            return this[NPEnum.ENPPlayerParam.LEVEL];
        }

        //获取玩家当前皮肤
        public long getCurrentSkinId()
        {
            return _m_curSkinRef.id;
        }

        //获取玩家当前头像id
        public long getCurrentIconId()
        {
            return curIcon.refId;
        }

        //获取玩家当前头像框id
        public long getCurrentIconBgkId()
        {
            return curIconBgk.refId;
        }

        //获取玩家当前气泡框id
        public long getCurrentBubbleId()
        {
            return curBubble.refId;
        }

        /// <summary>
        /// 赋值cid，GS登入之精辟就可以拿到cid，先赋值保证cid相关setting可以正常
        /// </summary>
        /// <param name="_cid"></param>
        public void setCid(long _cid)
        {
            _m_lCid = _cid;
        }
        
        //获取玩家性别
        public ENPGenderType getCurrentGenderType()
        {
            long prefabId = this[ENPPlayerParam.PREFAB];
            NPPlayerPrefabRefObj refObj = GRefdataCoreMgr.instance.playerPrefabRefCore.getRef(prefabId);
            if (null != refObj)
                return refObj.gender_type;

            return ENPGenderType.NONE;
        }
       

        public NPPlayerPrefabRefObj getCurrentPlayerPrefab()
        {
            return GRefdataCoreMgr.instance.playerPrefabRefCore.getRef(this[ENPPlayerParam.PREFAB]);
        }

        public void setData(GS2GC.p002_InitOp.GS2GC_002_001_RetPlayerInfo _msg)
        {
            _m_lCid = _msg.getCid();
            _m_sPlayerName = _msg.getCname();

            //加上基础属性
            _m_pcPlayerPropertyContainer.addModifier(GRefdataCoreMgr.instance.npGeneral.player_property);
            
            NPCommon_PlayerParam temp;
            for(int i = 0; i < _msg.getPlayerParams().Count; i++)
            {
                temp = _msg.getPlayerParams()[i];
                if(temp == null)
                    continue;

                _initParam(temp.getParamIndex(), temp.getValue());
            }

            //减去上一级的属性
            if(_m_lrCurLevelRef != null)
            {
                _m_pcPlayerPropertyContainer.removeModifier(_m_lrCurLevelRef.player_property);
            }
            //设置等级
            long level = getValue(NPEnum.ENPPlayerParam.LEVEL);
            _m_lrCurLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(level);
            if(_m_lrCurLevelRef != null)
            {
                _m_pcPlayerPropertyContainer.addModifier(_m_lrCurLevelRef.player_property);
            }
            _m_lrNextLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(level + 1);
            _m_lrPreLevelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(level - 1);
            _m_curSkinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(getValue(ENPPlayerParam.PLAYER_SKIN));
            //如果没有皮肤，显示默认的
            if (_m_curSkinRef == null)
                _m_curSkinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.default_player_skin);
            //设置VIP的玩家属性
            VipRefObj vipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(getValue(ENPPlayerParam.VIP_LVL));
            if(vipRef != null)
                _m_pcPlayerPropertyContainer.addModifier(vipRef.player_property);
            
            //刷新红点
            _m_redDealer?.init();
            onLevelChg?.Invoke();
        }

        //初始化数据处理
        protected void _initParam(int _index, long _paramValue)
        {
            //服务端枚举更新导致越界
            if(_m_arrPlayerParam.Length <= _index)
            {
                Debug.LogError_EditorOnly(string.Format("玩家参数更新类型数组越界～ 数组长度：{0}，index: {1}", _m_arrPlayerParam.Length, _index));
                return;
            }

            _m_arrPlayerParam[_index] = _paramValue;
        }

        //设置玩家名字
        public bool setPlayerName(string _name)
        {
            if(_m_sPlayerName == _name)
                return false;

            _m_sPlayerName = _name;
            return true;
        }

        /// <summary>
        /// 获取升级状态
        /// </summary>
        /// <param name="_noEnoughShowAccessWay"> 不足是否要弹出获取途径</param>
        /// <returns></returns>
        public ENPUpgradeState getUpgradeState(bool _noEnoughShowAccessWay = false)
        {
            if (null == _m_lrNextLevelRef)
                return ENPUpgradeState.LEVEL_MAX;

            return ENPUpgradeState.NORMAL;
        }
        
        public bool isDailyRewardDrawn()
        {
            long lastDrawDailyRewardDate = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE);
            int timeNow = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);

            return lastDrawDailyRewardDate == timeNow;
        }

        public bool canUpgrade()
        {
            return _m_lrNextLevelRef != null &&
                   GCommon.isItemEnough(ENPItemType.CURRENCY, (int)ECurrency.P_EXP, _m_lrNextLevelRef.exp, false) &&
                   NPPlayer.instance.specialItemComp.goldData.earnings >= _m_lrNextLevelRef.earnings;
        }

        /// <summary>
        /// 判断vip等级是否领取过奖励,true:已经领取  false：未领取
        /// </summary>
        /// <param name="_vipLevel"></param>
        /// <returns></returns>
        public bool isGetVipReward(int _vipLevel)
        {
            return (getValue(ENPPlayerParam.HAD_DRAW_VIP_REWARD_LIST) & (1 << _vipLevel)) != 0;
        }

        /// <summary>
        /// 判断vip等级是否领取过充值奖励,true:已经领取  false：未领取
        /// </summary>
        /// <param name="_vipLevel"></param>
        /// <returns></returns>
        public bool isGetVipRechargeReward(int _vipLevel)
        {
            return (getValue(ENPPlayerParam.HAD_DRAW_VIP_RECHARGE_REWARD_LIST) & (1 << _vipLevel)) != 0;
        }
        
        public PlayerInfo_IconShow getPlayerBriefInfo()
        {
            PlayerInfo_IconShow briefInfo = new PlayerInfo_IconShow();
            briefInfo.setCid(CID);
            briefInfo.setPlayerName(PlayerName);
            briefInfo.setIconId(getCurrentIconId());
            briefInfo.setIconBgkId(getCurrentIconBgkId());
            briefInfo.setBubbleId(getCurrentBubbleId());
            briefInfo.setVipLvl(getCurrentVIPLvl());
            briefInfo.setPlayerLvl(getCurrentLevel());
            briefInfo.setGuildId(NPPlayer.instance.guildComp.guildInfo?.guildId ?? 0);
            briefInfo.setGuildName(NPPlayer.instance.guildComp.guildInfo?.name ?? string.Empty);
            briefInfo.setIsOnline(true);
            briefInfo.setGuildSimpleName(NPPlayer.instance.guildComp.guildInfo?.simpleName ?? string.Empty);
            // briefInfo.setTotalPower();
            briefInfo.setEarnings(NPPlayer.instance.specialItemComp.goldData?.earnings ?? 0);
            briefInfo.setPlayerSkinId(getCurrentSkinId());
            briefInfo.setCurTitle(NPPlayer.instance.titleComp.curTitle);
            // briefInfo.setIsShow();
            return briefInfo;
        }
    }
}
