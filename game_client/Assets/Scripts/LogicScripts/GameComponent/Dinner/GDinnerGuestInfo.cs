using System;
using Common.DinnerEnum;
using Common.DinnerObj;

namespace GOE
{
    public class GDinnerGuestInfo
    {
        private Common.DinnerEnum.EDinnerJoinerType _m_joinerType;// 赴宴对象类型
        private long _m_joinerId;// 赴宴对象ID
        private long _m_costId;// 赴宴花费配置ID
        private long _m_coin;// 赴宴玩家宴会币
        private long _m_score;// 宴玩家宴会人气
        private NPCommonSimplePlayerInfo _m_playerInfo;
        private HeroRefObj _m_heroRefObj;
        private string _m_name;
        private bool _m_isRegDetailInfo = false;

        public Common.DinnerEnum.EDinnerJoinerType joinerType => _m_joinerType;
        public long joinerId => _m_joinerId;
        public long costId => _m_costId;
        public long coin => _m_coin;
        public long score => _m_score;
        public string name => _m_name;
        
        public GDinnerGuestInfo(Dinner_ResultGuestInfo _guestInfo)
        {
            _m_joinerType = _guestInfo.getJoinerType();
            _m_joinerId = _guestInfo.getJoinerId();
            _m_costId = _guestInfo.getCostId();
            _m_coin = _guestInfo.getCoin();
            _m_score = _guestInfo.getScore();
        }
        
        /// <summary>
        /// 请求详细数据
        /// </summary>
        /// <param name="_dealDone">，</param>
        /// <param name="_isFouce"></param>
        public void regDetailInfo(Action<GDinnerGuestInfo> _dealDone, bool _isFouce = false)
        {
            if (_m_isRegDetailInfo && !_isFouce)
            {
                _dealDone?.Invoke(this);
                return;
            }
            switch (_m_joinerType)
            {
                case EDinnerJoinerType.NONE:
                    _dealDone?.Invoke(this);
                    break;
                case EDinnerJoinerType.PLAYER:
                    GCommon.reqPlayerInfo(_m_joinerId, (_playerInfo) =>
                    {
                        if(_playerInfo == null)
                            return;
                        _m_playerInfo = _playerInfo;
                        _m_name = _m_playerInfo.name;
                        _dealDone?.Invoke(this);

                    });
                    break;
                case EDinnerJoinerType.HERO:
                    _m_heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_joinerId);
                    if (_m_heroRefObj != null)
                        _m_name = TextTranslate.instance.getLanguage(TransKeyConst.dinner_guest_hero_name, _m_heroRefObj.transName);
                    _dealDone?.Invoke(this);
                    break;
            }
            
            
        }
    }
}