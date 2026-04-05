using ALPackage;
using Common.NpPlayerInfoObj;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家房间皮肤组件
    /// </summary>
    public partial class PlayerRoomSkinComponent : _ANPBasicPlayerComponent
    {
        // 房间皮肤列表
        [NotNull] private readonly List<PlayerRoomSkinInfo> _m_lRoomSkinList;
        [NotNull] private CommonCountDownInfoMgr<PlayerRoomSkinInfo> _m_countDownInfoMgr;//房间皮肤相关的倒计时信息管理器
        // 当前使用的房间皮肤配表数据
        private PlayerRoomSkinRefObj _m_lCurrentRoomSkinRefObj;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlayerRoomSkinComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lRoomSkinList = new List<PlayerRoomSkinInfo>();
            _m_countDownInfoMgr = new CommonCountDownInfoMgr<PlayerRoomSkinInfo>(1f);
            _m_countDownInfoMgr.onCountDownTick += _onCountDownTick;
            _m_countDownInfoMgr.onCountDownFinish += _onItemCountDownFinish;
            _m_lCurrentRoomSkinRefObj = null;
        }


        /// <summary>
        /// 是否必须初始化
        /// </summary>
        public override bool isMustInit { get { return true; } }
        
        /// <summary>
        /// 组件类型
        /// </summary>
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_ROOM_SKIN; } }
        
        /// <summary>
        /// 依赖的组件
        /// </summary>
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public event Action onCountDownTick;
        
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqPlayerRoomSkinInit((_msg) =>
            {
                NPPlayer.instance.roomSkinComp.dealPreInitFunc(() =>
                {
                    NPPlayer.instance.roomSkinComp.retPlayerRoomSkinInit(_msg);
                });
            });
        }

        protected override void _dealInit()
        {
        }

        /// <summary>
        /// 组件加载完成时的调用
        /// </summary>
        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            
        }

        public override void onAllCompInited()
        {
        }

        /// <summary>
        /// 组件初始化失败的处理
        /// </summary>
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerRoomSkinComponent init Fail!!!");
        }

        /// <summary>
        /// 释放资源函数
        /// </summary>
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            
            _clear();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void _clear()
        {
            _m_lRoomSkinList.Clear();
            _m_countDownInfoMgr.clear();
            _m_lCurrentRoomSkinRefObj = null;

            onCountDownTick = null;
        }


        #region 属性访问

        /// <summary>
        /// 房间皮肤列表
        /// </summary>
        public List<PlayerRoomSkinInfo> roomSkinList { get { return _m_lRoomSkinList; } }

        /// <summary>
        /// 当前使用的房间皮肤配表数据
        /// </summary>
        public PlayerRoomSkinRefObj currentRoomSkinRefObj
        {
            get
            {
                if(_m_lCurrentRoomSkinRefObj == null || _m_lCurrentRoomSkinRefObj.id != NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.ROOM_SKIN))
                    _m_lCurrentRoomSkinRefObj = GRefdataCoreMgr.instance.playerRoomSkinRefCore.getRef(NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.ROOM_SKIN));//设置当前使用的房间皮肤数据
                
                if (_m_lCurrentRoomSkinRefObj == null)// 若当前没有使用的皮肤，返回默认皮肤
                    _m_lCurrentRoomSkinRefObj = GRefdataCoreMgr.instance.playerRoomSkinRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.default_player_room_skin);
                
                if (_m_lCurrentRoomSkinRefObj == null) // 若没有找到默认皮肤，返回配表中的第一个皮肤（理论上不应该出现这种情况）
                    _m_lCurrentRoomSkinRefObj = GRefdataCoreMgr.instance.playerRoomSkinRefCore.refList.SafeGet(0);
                
                return _m_lCurrentRoomSkinRefObj;
            }
        }
        
        public long currentRoomSkinId { get { return currentRoomSkinRefObj?.id ?? 0; } }

        #endregion


        #region 数据操作

        /// <summary>
        /// 添加或更新房间皮肤信息
        /// </summary>
        /// <param name="_roomSkinInfo"></param>
        private void _addOrUpdateRoomSkinInfo(PlayerInfo_RoomSkin _roomSkinInfo)
        {
            if (_roomSkinInfo == null)
                return;

            PlayerRoomSkinInfo existInfo = getRoomSkinInfo(_roomSkinInfo.getId());
            if (existInfo != null)
            {
                existInfo.updateInfo(_roomSkinInfo);
                WinMsg.SendMsg(WinMsgType.ON_ROOM_SKIN_INFO_CHG, existInfo);
            }
            else
            {
                existInfo = new PlayerRoomSkinInfo(_roomSkinInfo);
                _m_lRoomSkinList.Add(existInfo);
                WinMsg.SendMsg(WinMsgType.ON_ROOM_SKIN_ADD, existInfo);
            }
            
            if(!existInfo.isPermanent)//若不是永久的，加入倒计时管理器
                _m_countDownInfoMgr.addCountDown(existInfo);
        }
        
        /// <summary>
        /// 获取房间皮肤信息
        /// </summary>
        /// <param name="_skinId">皮肤ID</param>
        /// <returns>皮肤信息，未找到返回null</returns>
        public PlayerRoomSkinInfo getRoomSkinInfo(long _skinId)
        {
            if (_skinId <= 0)
                return null;

            for (int i = 0; i < _m_lRoomSkinList.Count; i++)
            {
                if (_m_lRoomSkinList[i] != null && _m_lRoomSkinList[i].id == _skinId)
                    return _m_lRoomSkinList[i];
            }

            return null;
        }

        /// <summary>
        /// 是否拥有指定房间皮肤
        /// </summary>
        /// <param name="_skinId">皮肤ID</param>
        /// <returns>是否拥有</returns>
        public bool hasRoomSkin(long _skinId)
        {
            return getRoomSkinInfo(_skinId) != null;
        }
        
        /// <summary>
        /// 获取皮肤状态
        /// </summary>
        public EPlayerRoomSkinState getSkinState(PlayerRoomSkinRefObj _refObj, PlayerRoomSkinInfo _skinInfo)
        {
            if (_refObj == null)
                return EPlayerRoomSkinState.NONE;

            // 判断是否拥有(默认皮肤默认视为拥有)
            if ((_skinInfo != null && _skinInfo.isValid) || _refObj.id == GRefdataCoreMgr.instance.npGeneral.default_player_room_skin)
            {
                // 已拥有，判断是否在使用中
                if (NPPlayer.instance.roomSkinComp.currentRoomSkinId == _refObj.id)
                    return EPlayerRoomSkinState.POSSESS_IN_USE;
                else
                    return EPlayerRoomSkinState.POSSESS_UNOCCUPIED;
            }
            else
            {
                return EPlayerRoomSkinState.NOT_GAIN;
            }
        }
        
        #endregion


        #region C2S

        /// <summary>
        /// 请求房间皮肤初始化
        /// </summary>
        public void reqPlayerRoomSkinInit(Action<GS2GC_002_087_RetPlayerRoomSkin> _retAction)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_087_ReqPlayerRoomSkin(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_087_RetPlayerRoomSkin>(( _msg) =>
                {
                    _retAction?.Invoke(_msg);
                }));
        }

        /// <summary>
        /// 请求查看房间皮肤（告知服务器该皮肤已查看过，服务器会将该皮肤的isViewed状态改为true）
        /// </summary>
        public void reqViewPlayerRoomSkin(long _roomSkinId)
        {
            NPGSClientListener.sendMsgByLog(new GC2GS.p021_PlayerInfo.GC2GS_021_047_ReqViewPlayerRoomSkin(_roomSkinId));
        }

        /// <summary>
        /// 请求设置当前使用的房间皮肤
        /// </summary>
        public void reqSetRoomSkin(long _roomSkinId)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_047_ReqSetRoomSkin(_roomSkinId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_047_RetSetRoomSkin>(( _msg) =>
                {
                }));
        }
        
        #endregion


        #region S2C

        /// <summary>
        /// 初始化玩家房间皮肤
        /// </summary>
        /// <param name="_msg">协议消息</param>
        public void retPlayerRoomSkinInit(GS2GC_002_087_RetPlayerRoomSkin _msg)
        {
            if (_msg == null)
                return;

            _m_lRoomSkinList.Clear();

            List<PlayerInfo_RoomSkin> roomSkinInfoList = _msg.getRoomSkinInfoList();
            if (roomSkinInfoList != null)
            {
                for (int i = 0; i < roomSkinInfoList.Count; i++)
                {
                    if (roomSkinInfoList[i] == null)
                        continue;

                    PlayerRoomSkinInfo roomSkinInfo = new PlayerRoomSkinInfo(roomSkinInfoList[i]);
                    _m_lRoomSkinList.Add(roomSkinInfo);
                    
                    if(!roomSkinInfo.isPermanent)// 若不是永久的，加入倒计时管理器
                        _m_countDownInfoMgr.addCountDown(roomSkinInfo);
                }
            }

            // 初始化完成
            setInitDone();
        }

        /// <summary>
        /// 房间皮肤添加
        /// </summary>
        /// <param name="_msg">协议消息</param>
        public void onRoomSkinAdd(GS2GC_021_092_OnRoomSkinAdd _msg)
        {
            if (_msg == null)
                return;

            _addOrUpdateRoomSkinInfo(_msg.getInfo());
        }

        /// <summary>
        /// 房间皮肤变更
        /// </summary>
        /// <param name="_msg">协议消息</param>
        public void onRoomSkinChg(GS2GC_021_093_OnRoomSkinChg _msg)
        {
            if (_msg == null)
                return;

            _addOrUpdateRoomSkinInfo(_msg.getInfo());
        }

        public void onRoomSkinDel(GS2GC_021_094_OnRoomSkinDel _msg)
        {
            if (_msg == null)
                return;

            long roomSkinId = _msg.getRoomSkinId();
            PlayerRoomSkinInfo existInfo = getRoomSkinInfo(roomSkinId);
            if (existInfo != null)
            {
                _m_lRoomSkinList.Remove(existInfo);
                _m_countDownInfoMgr.removeCountDown(existInfo);
                
                // 如果当前使用的皮肤被移除了，重置为默认皮肤
                if (_m_lCurrentRoomSkinRefObj != null && _m_lCurrentRoomSkinRefObj.id == roomSkinId)
                {
                    _m_lCurrentRoomSkinRefObj = null;
                    WinMsg.SendMsg(WinMsgType.ON_IN_USE_ROOM_SKIN_CHG);
                }

                WinMsg.SendMsg(WinMsgType.ON_ROOM_SKIN_DEL, existInfo);
            }
        }
        
        #endregion

        #region 消息监听

        private void _onCountDownTick()
        {
            onCountDownTick?.Invoke();
        }

        /// <summary>
        /// 有房间皮肤倒计时结束(皮肤过期)的消息
        /// </summary>
        /// <param name="_info"></param>
        private void _onItemCountDownFinish(PlayerRoomSkinInfo _info)
        {
            if (_info == null)
                return;

            // 如果当前使用的皮肤过期了，重置为默认皮肤
            if (_m_lCurrentRoomSkinRefObj != null && _m_lCurrentRoomSkinRefObj.id == _info.id)
            {
                _m_lCurrentRoomSkinRefObj = null;
                WinMsg.SendMsg(WinMsgType.ON_IN_USE_ROOM_SKIN_CHG);
            }

            WinMsg.SendMsg(WinMsgType.ON_ROOM_SKIN_INVALID, _info);
        }
        
        /// <summary>
        /// 当玩家参数变化
        /// </summary>
        private void _onPlayerParamChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is int playerParamInt))
                return;

            ENPPlayerParam playerParam = (ENPPlayerParam)playerParamInt;
            if (playerParam == ENPPlayerParam.ROOM_SKIN)
            {
                _m_lCurrentRoomSkinRefObj = GRefdataCoreMgr.instance.playerRoomSkinRefCore.getRef(NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.ROOM_SKIN));//设置当前使用的房间皮肤数据
                WinMsg.SendMsg(WinMsgType.ON_IN_USE_ROOM_SKIN_CHG);
            }
        }
        
        #endregion
    }
}
