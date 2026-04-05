using ALPackage;
using Common.HeroObj;
using GC2GS.p018_PlayerSkinOp;
using GS2GC.p002_InitOp;
using GS2GC.p018_PlayerSkinOp;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤管理器
    /// </summary>
    public partial class PlayerSkinComponent : _ANPBasicPlayerComponent
    {
        //皮肤列表
        [NotNull] private List<PlayerSkinInfo> _m_lSkinList;
        //红点处理器
        [NotNull] private RedTipDealer _m_redTipDealer;

        //构造函数
        public PlayerSkinComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lSkinList = new List<PlayerSkinInfo>();
            _m_redTipDealer = new RedTipDealer(this);
        }

        //属性
        public override bool isMustInit { get { return true; } }
        //组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_SKIN; } }
        //依赖的组件
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

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
            //请求玩家皮肤初始化
            reqPlayerSkinInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onBagItemChg);
        }

        public override void onAllCompInited()
        {
            //红点初始化
            _m_redTipDealer.init();
            _refreshRedTip();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerSkinComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onBagItemChg);
            clear();
        }

        public void clear()
        {
            _m_redTipDealer.clear();
            _m_lSkinList.Clear();
        }

        /// <summary>
        /// 获取皮肤信息
        /// </summary>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public PlayerSkinInfo getSkinInfo(long _skinId)
        {
            if(_skinId <= 0)
                return null;

            for (int i = 0; i < _m_lSkinList.Count; i++)
            {
                if(_m_lSkinList[i] != null && _m_lSkinList[i].skinId == _skinId)
                    return _m_lSkinList[i];
            }

            return null;
        }

        /// <summary>
        /// 是否需要展示获得非强制红点
        /// </summary>
        /// <param name="_skinRefId"></param>
        /// <returns></returns>
        public bool needShowGainRedTip(long _skinRefId)
        {
            return _m_redTipDealer.needShowRedTip(RedTipConst.RED_PLAYER_SKIN, _skinRefId);
        }

        /// <summary>
        /// 设置获得非强制红点已读
        /// </summary>
        /// <param name="_skinRefId"></param>
        public void setReadGainRedTip(long _skinRefId)
        {
            _m_redTipDealer.setReadRedTip(RedTipConst.RED_PLAYER_SKIN, _skinRefId);
        }

        //刷新红点
        private void _refreshRedTip()
        {
            long count = 0;
            GRefdataCoreMgr.instance.playerSkinRefCore.dealAllRef(_skinRef =>
            {
                if (_skinRef != null && !_skinRef.is_hide)
                {
                    PlayerSkinInfo skinInfo = getSkinInfo(_skinRef.id);
                    if (skinInfo == null && 
                        _skinRef.unlock_item != null && 
                        _skinRef.unlock_item.getItemType() != ENPItemType.NONE && 
                        GCommon.isItemEnough(_skinRef.unlock_item,false))
                    {
                        //未获得是否有足够道具解锁
                        count++;
                    }
                    else if(skinInfo != null && 
                            skinInfo.curSkinLevelRef != null && 
                            skinInfo.nextSkinLevelRef != null && 
                            skinInfo.nextSkinLevelRef.upgrade_cost != null && 
                            skinInfo.nextSkinLevelRef.upgrade_cost.getItemType() != ENPItemType.NONE && 
                            GCommon.isItemEnough(skinInfo.curSkinLevelRef.upgrade_cost, false))
                    {
                        //已获得是否有足够道具升级
                        count++;
                    }
                }
            });
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_PLAYER_SKIN, count);
        }

        //背包道具变更
        private void _onBagItemChg(params object[] _objs)
        {
            _refreshRedTip();
        }

        #region S2C

        /// <summary>
        /// 初始化玩家称号
        /// </summary>
        /// <param name="_msg"></param>
        public void retPlayerTitleInit(GS2GC_002_005_RetPlayerSkinInit _msg)
        {
            if (_msg == null)
                return;

            _m_lSkinList.Clear();

            for (int i = 0; i < _msg.getSkinList().Count; i++)
            {
                if(_msg.getSkinList()[i] == null)
                    continue;

                _m_lSkinList.Add(new PlayerSkinInfo(_msg.getSkinList()[i]));
            }

            //初始化完成
            setInitDone();
        }

        /// <summary>
        /// 玩家皮肤数据变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onPlayerSkinChg(GS2GC_018_060_OnPlayerSkinChg _msg)
        {
            if (_msg == null)
                return;

            PlayerSkinInfo skinInfo = getSkinInfo(_msg.getInfo().getSkinId());
            if (skinInfo != null)
            {
                int oriLevel = skinInfo.level;
                skinInfo.updateInfo(_msg.getInfo());
                WinMsg.SendMsg(WinMsgType.ON_PLAYER_SKIN_CHG, _msg.getInfo().getSkinId());

                if(oriLevel != skinInfo.level)
                    WinMsg.SendMsg(WinMsgType.ON_PLAYER_SKIN_LEVEL_CHG, _msg.getInfo().getSkinId(), oriLevel, skinInfo.level);

            }
            else
            {
                skinInfo = new PlayerSkinInfo(_msg.getInfo());
                _m_lSkinList.Add(skinInfo);

                //刷新红点
                _m_redTipDealer.onGainSkin(skinInfo.skinRefObj);

                WinMsg.SendMsg(WinMsgType.ON_PLAYER_SKIN_ADD, _msg.getInfo().getSkinId());
            }

        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求玩家皮肤初始化
        /// </summary>
        public void reqPlayerSkinInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_005_ReqPlayerSkinInit());
        }

        /// <summary>
        /// 请求解锁皮肤
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_callback"></param>
        public void reqUnlockPlayerSkin(long _skinId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_010_ReqUnlockPlayerSkin(_skinId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_018_010_RetUnlockPlayerSkin>((_isSuc, info) =>
                {
                    if (null != _callback)
                        _callback(_isSuc);
                }));
        }

        /// <summary>
        /// 请求穿戴皮肤
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_callback"></param>
        public void reqSetCurPlayerSkin(long _skinId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_011_ReqSetCurPlayerSkin(_skinId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_018_011_RetSetCurPlayerSkin>((_isSuc, info) =>
                {
                    if (null != _callback)
                        _callback(_isSuc);
                }));
        }

        /// <summary>
        /// 请求脱下皮肤
        /// </summary>
        /// <param name="_callback"></param>
        public void reqUnsetCurPlayerSkin(Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_012_ReqUnsetCurPlayerSkin(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_018_012_RetUnsetCurPlayerSkin>((_isSuc, info) =>
                {
                    if (null != _callback)
                        _callback(_isSuc);
                }));
        }

        /// <summary>
        /// 请求升级皮肤
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_callback"></param>
        public void reqUpgradePlayerSkin(long _skinId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_018_013_ReqUpgradePlayerSkin(_skinId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_018_013_RetUpgradePlayerSkin>((_isSuc, info) =>
                {
                    if (null != _callback)
                        _callback(_isSuc);
                }));
        }

        #endregion
    }
}
