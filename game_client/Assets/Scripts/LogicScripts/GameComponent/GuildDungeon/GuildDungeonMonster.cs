using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;
using JetBrains.Annotations;

namespace GOE
{
    public class GuildDungeonMonster
    {
        private long _m_monsterId;// 怪物ID
        private int _m_layer;// 层级
        private int _m_positionInLayer;// 在层级中的位置
        private int _m_totalInLayer;// 该层级总怪物数量
        private GuildDungeonInfo _m_dungeonInfo; // 所属副本ID
        [NotNull]private GuildDungeonMonsterRefObj _m_monsterRefObj;// 怪物对象
        private GuildDungeonMonsterShowRefObj _m_monsterShowRefObj;// 怪物展示对象
        
        private long _m_hp;// 当前怪物血量
        private bool _m_isReward; // 是否有奖励
        
        public GuildDungeonInfo dungeonInfo => _m_dungeonInfo;
        public long monsterId => _m_monsterId;
        public GuildDungeonMonsterRefObj monsterRefObj => _m_monsterRefObj;
        public long hp
        {
            get
            {
                // 获取当前怪物血量，判断副本实例id，如果没有则怪物实例不存在，血量为满血
                if (_m_dungeonInfo != null && _m_dungeonInfo.instanceId <= 0)
                {
                    GuildDungeonLvlRefObj lvlRef = _m_dungeonInfo.dungeonRefObj.getLvlRef(_m_dungeonInfo.lvl);

                    if (lvlRef != null) return lvlRef.getMonsterHp(_m_monsterRefObj.monster_type);
                }
                return _m_hp;
            }
        }

        public int layer => _m_layer;
        public int positionInLayer => _m_positionInLayer;
        public int totalInLayer => _m_totalInLayer;
        public long maxHp
        {
            get
            {
                if (_m_dungeonInfo != null && _m_dungeonInfo.dungeonRefObj != null)
                {
                    GuildDungeonLvlRefObj instanceLvlRef = _m_dungeonInfo.dungeonRefObj.getLvlRef(_m_dungeonInfo.instanceLvl);

                    if (instanceLvlRef != null) return instanceLvlRef.getMonsterHp(_m_monsterRefObj.monster_type);
                }
                return 0;
            }
        }

        public bool isReward => _m_monsterRefObj.monster_type == EGuildDungeon_MonsterType.BOSS || _m_isReward;

        public GuildDungeonMonsterShowRefObj monsterShowRefObj
        {
            get
            {
                if (_m_monsterShowRefObj == null)
                {
                    _m_monsterShowRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterShowRefCore.getRef(_m_monsterRefObj.monster_show_id);
                }
                return _m_monsterShowRefObj;
            }
        }

        public NPGTextureIndex icon => monsterShowRefObj?.icon;

        public GuildDungeonMonster([NotNull]GuildDungeonMonsterRefObj _monsterRefObj, GuildDungeonInfo _dungeonInfo, int _layer, int _positionInLayer, int _totalInLayer, long _hp)
        {
            _m_dungeonInfo = _dungeonInfo;
            _m_monsterId = _monsterRefObj.id;
            _m_monsterRefObj = _monsterRefObj;
            _m_layer = _layer;
            _m_positionInLayer = _positionInLayer;
            _m_totalInLayer = _totalInLayer;
            _m_hp = _hp;
        }

        public void nextDayReset()
        {
            _m_hp = maxHp;
            _m_isReward = false;
        }

        public void updateInfo( long _hp, bool _isReward)
        {
            _m_hp = _hp;
            _m_isReward = _isReward;
        }

        /// <summary>
        /// 更新怪物血量
        /// </summary>
        /// <param name="_hp">新的血量值</param>
        public void updateHp(long _hp)
        {
            _m_hp = _hp;
        }
    }
}