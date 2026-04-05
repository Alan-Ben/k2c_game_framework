using System;
using Common.ArenaObj;

namespace GOE
{
    /// <summary>
    /// 竞技场战报信息
    /// </summary>
    public class ArenaBattleReportInfo
    {
        //数据id
        private long _m_lDbId;
        //对手CID
        private long _m_lOpponentCid;
        //击败我方大臣数量
        private int _m_iDefeatHeroNum;
        //扣除影响力
        private int _m_iDeductinfluence;
        //时间戳
        private long _m_lTimestamp;

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
        /// 对手CID
        /// </summary>
        public long opponentCid => _m_lOpponentCid;
        /// <summary>
        /// 击败我方大臣数量
        /// </summary>
        public int defeatHeroNum => _m_iDefeatHeroNum;
        /// <summary>
        /// 扣除影响力
        /// </summary>
        public int deductinfluence => _m_iDeductinfluence;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp => _m_lTimestamp;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_info"></param>
        public ArenaBattleReportInfo(Arena_BattleReport _info)
        {
            updateInfo(_info);
        }

        public ArenaBattleReportInfo(long _dbId, Arena_BattleReportShow _showInfo)
        {
            if (_showInfo == null)
                return;

            _m_lDbId = _dbId;
            _m_lOpponentCid = _showInfo.getOpponentCid();
            _m_iDefeatHeroNum = _showInfo.getDefeatHeroNum();
            _m_iDeductinfluence = _showInfo.getDeductinfluence();
            _m_lTimestamp = _showInfo.getTimestamp();
        }


        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_baseInfo"></param>
        public void updateInfo(Arena_BattleReport _info)
        {
            if (_info == null || _info.getShowInfo() == null)
                return;

            _m_lDbId = _info.getDbId();
            _m_lOpponentCid = _info.getShowInfo().getOpponentCid();
            _m_iDefeatHeroNum = _info.getShowInfo().getDefeatHeroNum();
            _m_iDeductinfluence = _info.getShowInfo().getDeductinfluence();
            _m_lTimestamp = _info.getShowInfo().getTimestamp();
        }

        /// <summary>
        /// 获取玩家信息
        /// </summary>
        /// <param name="_onDone"></param>
        public void getPlayerInfo(Action<NPCommonSimplePlayerInfo> _onDone)
        {
            if (_onDone == null)
                return;

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
            NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_m_lOpponentCid, (_info) =>
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
