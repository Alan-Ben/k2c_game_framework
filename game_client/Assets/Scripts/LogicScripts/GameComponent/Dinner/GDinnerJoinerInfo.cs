using System;
using Common.DinnerEnum;
using Common.DinnerObj;

namespace GOE
{
    public class GDinnerJoinerInfo
    {
        private int _m_idx;
        private long _m_joinerId;// 参宴玩家id
        private string _m_name;
        private EDinnerJoinerType _m_joinerType;
        private long _m_score;
        private long _m_costId;
        private long _m_joinTimeMs;
        
        private NPCommonSimplePlayerInfo _m_playerInfo;
        
        public EDinnerJoinerType joinerType => _m_joinerType;
        public long joinerId => _m_joinerId;
        public string name => _m_name;
        public long score => _m_score;
        public long joinTimeMs => _m_joinTimeMs;
        public long costId => _m_costId;
        public NPCommonSimplePlayerInfo playerInfo => _m_playerInfo;
        
        
        private bool _m_isRegDetailInfo = false;

        public GDinnerJoinerInfo(Dinner_Joiner _dinnerJoiner)
        {
            if (_dinnerJoiner != null)
            {
                _m_joinerType = _dinnerJoiner.getJoinerType();
                _m_joinerId = _dinnerJoiner.getJoinerId();
                _m_costId = _dinnerJoiner.getCostId();
                _m_score = _dinnerJoiner.getScore();
                _m_joinTimeMs = _dinnerJoiner.getJoinTimeMs();
            }
            _m_isRegDetailInfo = false;
        }

        public GDinnerJoinerInfo(
            EDinnerJoinerType _joinerType,
            long _joinerId,
            long _score,
            long _joinTimeMs,
            long _costId)
        {
            _m_joinerType = _joinerType;
            _m_joinerId = _joinerId;
            _m_costId = _costId;
            _m_score = _score;
            _m_joinTimeMs = _joinTimeMs;
            _m_isRegDetailInfo = false;
        }
        public GDinnerJoinerInfo(
            EDinnerJoinerType _joinerType,
            long _joinerId,
            long _score,
            long _joinTimeMs,
            long _costId, NPCommonSimplePlayerInfo _playerInfo)
        {
            _m_joinerType = _joinerType;
            _m_joinerId = _joinerId;
            _m_costId = _costId;
            _m_score = _score;
            _m_joinTimeMs = _joinTimeMs;
            if (_playerInfo != null)
            {
                _m_playerInfo = _playerInfo;
                _m_name = _m_playerInfo.name;
                _m_isRegDetailInfo = true;
            }
            else
            {
                _m_isRegDetailInfo = false;
            }
        }
        /// <summary>
        /// 请求详细数据
        /// </summary>
        /// <param name="_dealDone">，</param>
        /// <param name="_isFouce"></param>
        public void regDetailInfo(Action<GDinnerJoinerInfo> _dealDone, bool _isForce = false)
        {
            if (_m_isRegDetailInfo && !_isForce)
            {
                _dealDone?.Invoke(this);
                return;
            }
            if (_m_joinerType == EDinnerJoinerType.PLAYER)
            {
                GCommon.reqPlayerInfo(_m_joinerId, (_info) =>
                {
                    _m_playerInfo = _info;
                    if (_m_playerInfo != null) 
                        _m_name = _m_playerInfo.name;
                    updateDetailInfo();
                    _dealDone?.Invoke(this);
                });
            }
            else if(_m_joinerType == EDinnerJoinerType.HERO)
            {
                HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_joinerId);
                if (heroRefObj != null) 
                    _m_name = heroRefObj.transName;
                updateDetailInfo();
                _dealDone?.Invoke(this);
            }
        }
        
        /// <summary>
        /// 更新详细信息
        /// </summary>
        /// <param name="_info"></param>
        private void updateDetailInfo()
        {
            _m_isRegDetailInfo = true;
        }
        
    }

}