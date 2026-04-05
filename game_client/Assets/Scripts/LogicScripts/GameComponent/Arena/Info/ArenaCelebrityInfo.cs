using Common.ArenaObj;
using NPCommon;
using System;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜信息
    /// </summary>
    public class ArenaCelebrityInfo
    {
        //数据id
        private long _m_lDbId;
        //攻击者CID
        private long _m_lAttackerCid;
        //攻击者名字
        private string _m_sAttackerName;
        //防守者名字
        private string _m_sDefenderName;
        //击败大臣数量
        private int _m_iDefeatHeroNum;
        //是否指定攻击
        private bool _m_bIsSelectAttack;
        //发生时间戳
        private long _m_lTimeMs;
        //是否是机器人
        private bool _m_bIsBot;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_simplePlayerInfo;
        //是否获取玩家信息完成
        private bool _m_bIsGetPlayerInfoDone;
        //是否正在获取玩家信息
        private bool _m_bIsGettingPlayerInfo;
        //获取玩家信息完成回调
        private Action<NPCommonSimplePlayerInfo> _m_aOnGetPlayerInfoDelegate;


        /// <summary>
        /// 数据id
        /// </summary>
        public long dbId => _m_lDbId;
        /// <summary>
        /// 攻击者CID
        /// </summary>
        public long attackerCid => _m_lAttackerCid;
        /// <summary>
        /// 攻击者名字
        /// </summary>
        public string attackerName => _m_sAttackerName;
        /// <summary>
        /// 防守者名字
        /// </summary>
        public string defenderName => _m_sDefenderName;
        /// <summary>
        /// 击败大臣数量
        /// </summary>
        public int defeatHeroNum => _m_iDefeatHeroNum;
        /// <summary>
        /// 是否指定攻击
        /// </summary>
        public bool isSelectAttack => _m_bIsSelectAttack;
        /// <summary>
        /// 发生时间戳
        /// </summary>
        public long timeMs => _m_lTimeMs;
        /// <summary>
        /// 是否是机器人
        /// </summary>
        public bool isBot => _m_bIsBot;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_info"></param>
        public ArenaCelebrityInfo(Arena_CelebrityRankInfo _info, bool _isBot = false)
        {
            _m_bIsBot = _isBot;
            updateInfo(_info);
        }


        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Arena_CelebrityRankInfo _info)
        {
            if (_info == null)
                return;

            _m_lDbId = _info.getDbId();
            _m_lAttackerCid = _info.getAttackerCid();
            _m_sAttackerName = _info.getAttackerName();
            _m_sDefenderName = _info.getDefenderName();
            _m_iDefeatHeroNum = _info.getDefeatHeroNum();
            _m_bIsSelectAttack = _info.getIsSelectAttack();
            _m_lTimeMs = _info.getTimeMs();
        }

        /// <summary>
        /// 获取玩家信息
        /// </summary>
        /// <param name="_onDone"></param>
        public void getPlayerInfo(Action<NPCommonSimplePlayerInfo> _onDone)
        {
            if (_onDone == null)
                return;

            //如果是机器人，直接返回机器人信息
            if (_m_bIsBot)
            {
                PlayerInfo_IconShow iconShow = new PlayerInfo_IconShow();
                iconShow.setIconId(GRefdataCoreMgr.instance.npGeneral.arena_bot_icon_id);
                iconShow.setIconBgkId(GRefdataCoreMgr.instance.npGeneral.arena_bot_icon_bgk_id);
                iconShow.setPlayerName(_m_sAttackerName);
                long level = AccountSettingMgr.instance.accountSetting.arenaFightBotLevel;
                if (level <= 0)
                {
                    level = GRefdataCoreMgr.instance.npGeneral.arena_bot_level_list.GetRandomItem();
                    //记录这次打的机器人等级
                    AccountSettingMgr.instance.accountSetting.setArenaFightBotLevel(level);
                }
                iconShow.setPlayerLvl(level);

                NPCommonSimplePlayerInfo botInfo = new NPCommonSimplePlayerInfo(iconShow);
                _onDone.Invoke(botInfo);
                return;
            }

            if (_m_bIsGetPlayerInfoDone && _m_simplePlayerInfo != null)
            {
                _onDone.Invoke(_m_simplePlayerInfo);
                return;
            }

            if (_m_aOnGetPlayerInfoDelegate == null)
                _m_aOnGetPlayerInfoDelegate = _onDone;
            else
                _m_aOnGetPlayerInfoDelegate += _onDone;

            if (!_m_bIsGettingPlayerInfo)
                _reqPlayerInfo();
        }

        /// <summary>
        /// 请求玩家信息
        /// </summary>
        private void _reqPlayerInfo()
        {
            _m_bIsGettingPlayerInfo = true;
            NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_m_lAttackerCid, (_info) =>
            {
                _m_bIsGetPlayerInfoDone = true;
                _m_bIsGettingPlayerInfo = false;
                _m_simplePlayerInfo = new NPCommonSimplePlayerInfo(_info);

                //执行回调
                Action<NPCommonSimplePlayerInfo> onDone = _m_aOnGetPlayerInfoDelegate;
                _m_aOnGetPlayerInfoDelegate = null;
                onDone?.Invoke(_m_simplePlayerInfo);
            });
        }
    }
}
