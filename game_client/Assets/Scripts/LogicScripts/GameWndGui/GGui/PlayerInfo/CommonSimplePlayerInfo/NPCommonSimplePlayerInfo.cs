using Common.NpChatObj;
using NPCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家通用的一个简要信息
    /// </summary>
    public class NPCommonSimplePlayerInfo
    {
        public string name;//名字
        public long cid;//cid
        public long level;//等级
        public long vipLvl;//vip等级
        public long totalPower;//总实力
        public long earnings;//赚速
        public long exp;//经验
        public long vipExp;//vip经验

        public long guildId;//联盟id
        public string guildName;//联盟名称
        public string guildSimpleName;//联盟简称

        public bool isOnline; //在线状态
        public long lastOfflineMs; //最后一次离线时间戳（毫秒）
        public long lastOnlineMs; //最后一次上线时间戳（毫秒）
        public PlayerInfo_CurTitle curTitleInfo;//当前称号信息
        public bool isShowTitle;//是否需要展示称号

        public List<long> powerDetailList;//国力分别属性
        private long _m_cuteActorId;//Q版形象id
        private CuteActorRefObj _m_cuteActorRef;//Q版形象配置
        private CommonPlayerCuteActorShowInfo _m_iCuteActorShowInfo;//Q版形象展示信息
        private PlayerLvlRefObj _m_levelRef;//等级配置

        private long _m_icon;//头像
        private long _m_iconBgk;//头像框
        private long _m_bubbleId;//气泡框id
        private long _m_lSkinId;//皮肤id

        private long _m_beLikeCount;

        //头像
        public long beLikeCount { get { return _m_beLikeCount; } }
        public long icon { get { return _m_icon == 0 ? GRefdataCoreMgr.instance.npGeneral.default_player_icon : _m_icon; } }
        //头像框
        public long icon_bgk { get { return _m_iconBgk == 0 ? GRefdataCoreMgr.instance.npGeneral.default_player_icon_bgk : _m_iconBgk; } }
        //气泡框
        public long bubbleId { get { return _m_bubbleId == 0 ? GRefdataCoreMgr.instance.npGeneral.chat_default_bubble_id : _m_bubbleId; } }
        //皮肤id
        public long skinId { get { return _m_lSkinId == 0 ? GRefdataCoreMgr.instance.npGeneral.default_player_skin : _m_lSkinId; } }
        public CuteActorRefObj cuteActorRef { get { return _m_cuteActorRef; } }
        public CommonPlayerCuteActorShowInfo cuteActorShowInfo { get { return _m_iCuteActorShowInfo; } }
        public PlayerLvlRefObj levelRef { get { return _m_levelRef; } }
        public PlayerSkinRefObj skinRef { get { return GRefdataCoreMgr.instance.playerSkinRefCore.getRef(skinId); } }

        public NPCommonSimplePlayerInfo()
        {

        }

        public NPCommonSimplePlayerInfo(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _playerInfo)
        {
            update(_playerInfo);
        }

        public NPCommonSimplePlayerInfo(PlayerInfo _playerInfo, PlayerInfo_CurTitle _curTitleInfo, bool _isShowTitle, long _vipExp)
        {
            if (null == _playerInfo)
                return;

            cid = _playerInfo.CID;
            name = _playerInfo.PlayerName;
            level = _playerInfo.getCurrentLevel();
            _m_icon = _playerInfo.getCurrentIconId();
            _m_iconBgk = _playerInfo.getCurrentIconBgkId();
            _m_bubbleId = _playerInfo.getCurrentBubbleId();
            vipLvl = _playerInfo.getCurrentVIPLvl();
            _m_lSkinId = _playerInfo.getCurrentSkinId();
            _m_levelRef = _playerInfo.curLevelRef;
            curTitleInfo = _curTitleInfo;
            isShowTitle = _isShowTitle;
            vipExp = _vipExp;
        }

        public NPCommonSimplePlayerInfo(PlayerInfo_IconShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _setSimpleInfo(_playerInfo);
        }

        public NPCommonSimplePlayerInfo(NPCommon_ChatPlayerContent _chatContent)
        {
            if (null == _chatContent)
                return;

            cid = _chatContent.getCid();
            name = _chatContent.getCName();
            _m_bubbleId = _chatContent.getBubbleId();
            _m_icon = _chatContent.getIconId();
            _m_iconBgk = _chatContent.getIconBgkId();
            _m_lSkinId = _chatContent.getPlayerSkinId();
            curTitleInfo = _chatContent.getCurTitle();
            isShowTitle = _chatContent.getIsShow();
            vipLvl = _chatContent.getVipLvl();
        }

        private void _setSimpleInfo(PlayerInfo_IconShow _playerIconInfo)
        {
            if (_playerIconInfo == null)
                return;

            cid = _playerIconInfo.getCid();
            name = _playerIconInfo.getPlayerName();
            level = _playerIconInfo.getPlayerLvl();
            _m_icon = _playerIconInfo.getIconId();
            _m_iconBgk = _playerIconInfo.getIconBgkId();
            _m_bubbleId = _playerIconInfo.getBubbleId();
            vipLvl = _playerIconInfo.getVipLvl();
            guildId = _playerIconInfo.getGuildId();
            guildName = _playerIconInfo.getGuildName();
            guildSimpleName = _playerIconInfo.getGuildSimpleName();
            isOnline = _playerIconInfo.getIsOnline();
            lastOfflineMs = _playerIconInfo.getLastOfflineMs();
            lastOnlineMs = _playerIconInfo.getLastOnlineMs();
            totalPower = _playerIconInfo.getTotalPower();
            earnings = _playerIconInfo.getEarnings();
            _m_levelRef = GRefdataCoreMgr.instance.playerLvlCore.getRef(level);
            _m_lSkinId = _playerIconInfo.getPlayerSkinId();
            curTitleInfo = _playerIconInfo.getCurTitle();
            isShowTitle = _playerIconInfo.getIsShow();
            exp = _playerIconInfo.getExp();
        }

        public void update(PlayerInfo_IconShow _playerIconInfo)
        {
            _setSimpleInfo(_playerIconInfo);
        }

        public void update(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _setSimpleInfo(_playerInfo.getIconShow());
            exp = _playerInfo.getExp();
            vipExp = _playerInfo.getVipExp();
            powerDetailList = _playerInfo.getAttrList().getValueList();
            _m_cuteActorId = _playerInfo.getCuteActorId();
            _m_cuteActorRef = GRefdataCoreMgr.instance.cuteActorRefCore.getRef((_m_cuteActorId == 0)
                ? GRefdataCoreMgr.instance.npGeneral.player_default_cute_actor_id :
                _m_cuteActorId);
            _m_beLikeCount = _playerInfo.getBeLikeCount();
            if (_m_iCuteActorShowInfo == null)
                _m_iCuteActorShowInfo = new CommonPlayerCuteActorShowInfo(_m_cuteActorRef?.go_index, Color.clear);
            else
                _m_iCuteActorShowInfo.update(_m_cuteActorRef?.go_index, Color.clear);
        }
    }
}