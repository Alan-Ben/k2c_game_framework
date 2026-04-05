using System;
using Common.GuildObj;
using Common.HeroObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    // public class MiddayDungeonHeroInfo
    // {
    //     private HeroInfo _m_heroInfo;
    //     private long _m_cid;
    //
    //     public HeroInfo heroInfo => _m_heroInfo;
    //     public MiddayDungeonHeroInfo(HeroInfo _info)
    //     {
    //         _m_heroInfo = _info;
    //         _m_cid = 0;
    //     }
    // }
    
    public class MiddayDungeonGuildHeroInfo : _IHeroCardShow 
    {
        private long _m_cid;
        //伙伴id
        private long _m_lId;
        //当前皮肤Id
        private long _m_lCurSkinId;
        //觉醒星级
        private long _m_lStar;
        //伙伴静态数据
        private HeroRefObj _m_heroRefObj;
        //当前等级信息
        private long _m_level;
        //实力值
        private long _m_lPower;

        private bool _m_hasReqPlayerName;
        private bool _m_hasGetPlayerName;
        private string _m_playerName;

        private Action<MiddayDungeonGuildHeroInfo> _m_onPlayerNameGet;

        public MiddayDungeonGuildHeroInfo(Guild_DispatchHeroDetailInfo _info)
        {
            if(_info == null)
                return;
            
            _m_cid = _info.getCid();
            
            _m_lId = _info.getHeroId();
            _m_heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_lId);
            _m_level = _info.getLevel();
            _m_lPower = _info.getPower();
            _m_lCurSkinId = _info.getSkinId();
            _m_hasReqPlayerName = false;
            _m_hasGetPlayerName = false;
        }
        public long cid => _m_cid;

        public long id => _m_lId;

        public HeroRefObj heroRefObj => _m_heroRefObj;

        public HeroStepRefObj curHeroStepRef => null;

        public long level { get { return _m_level; } }
        
        public long power { get { return _m_lPower; } }

        public long star { get { return _m_lStar; } }

        public bool isUnlock { get { return true; } }
        
        public HeroSkinRefObj curSkinRefObj { get { return GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_lCurSkinId); } }

        public string playerName { get { return _m_playerName; } }

        /// <summary>
        /// 获取总资质
        /// </summary>
        /// <returns></returns>
        public long getTotalTalent()
        {
            return 0;
        }

        public NPGTextureIndex getIcon()
        {
            return GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _m_lCurSkinId);
        }

        public NPGTextureIndex getCardImage()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.card_image;

            return null;
        }

        public NPGTextureIndex getCardBg()
        {
            if (_m_heroRefObj != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroRefObj.id);
                return qualityExtRef?.hero_card_bg;
            }
            return null;
        }

        /// <summary>
        /// 获取全身形象
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdShow()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.td_show;

            return null;
        }

        /// <summary>
        /// 获取形象背景
        /// </summary>
        /// <returns></returns>
        public NPGGoIndex getTdBg()
        {
            if (null != curSkinRefObj)
                return curSkinRefObj.td_bg_index;

            return null;
        }

        private void reqPlayerName()
        {
            _m_hasReqPlayerName = true;
            GCommon.reqPlayerInfo(_m_cid, _info =>
            {
                _m_playerName = _info.name;
                _m_hasGetPlayerName = true;
                _m_onPlayerNameGet?.Invoke(this);
            });
        }
        public void regPlayerInfo(Action<MiddayDungeonGuildHeroInfo> _dealDone)
        {
            if (_m_hasGetPlayerName)
            {
                _dealDone?.Invoke(this);
                return;
            }

            if (_m_onPlayerNameGet == null)
                _m_onPlayerNameGet = _dealDone;
            else 
                _m_onPlayerNameGet += _dealDone;
            if (!_m_hasReqPlayerName)
                reqPlayerName();
        }
    }
}