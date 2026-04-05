using System;
using System.Collections.Generic;
using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;
using JetBrains.Annotations;

namespace GOE
{
    public class GuildDungeonInfo
    {
        private long _m_dungeonRefreshTimeTag = 0; // 公会副本刷新时间标记
        private long _m_dungeonId;// 公会副本ID
        [NotNull]private GuildDungeonRefObj _m_dungeonRefObj;// 公会副本参考对象
        private int _m_lvl;// 公会副本等级
        private GuildDungeonLvlRefObj _m_lvlRefObj;// 公会副本参考对象
        
        private GuildDungeonMonster _m_bossMonster; // BOSS怪物
        [NotNull]private List<GuildDungeonMonster> _m_monsterList = new List<GuildDungeonMonster>(); // 怪物列表
        private int _m_xMaxLayerCount = 0; // x轴层数量
        private int _m_yMaxLayerCount = 0; // y轴层数量

        [NotNull]private List<long> _m_tagMonsterIdsList = new List<long>(); // 标记的怪物ID列表
        [NotNull]private List<long> _m_hasGainRewardMonsterIdsList = new List<long>(); // 已领取奖励怪物ID列表

        // 实例数据
        private long _m_instanceId; // 公会副本实例ID
        private long _m_instanceLvl; // 公会副本实例等级
        private long _m_startMs;// 开启时间

        
        public long instanceId { get { return _m_instanceId; } }
        public long dungeonId { get { return _m_dungeonId; } }

        public GuildDungeonRefObj dungeonRefObj => _m_dungeonRefObj;
        public int lvl { get { return _m_lvl; } }
        /// <summary>
        /// 如果有实例则为实例等级，没有则为设置等级
        /// </summary>
        public long instanceLvl { get { return _m_instanceLvl == 0 ? _m_lvl : _m_instanceLvl; } }
        public GuildDungeonLvlRefObj lvlRefObj
        {
            get
            {
                if (_m_lvlRefObj == null || _m_lvlRefObj.lvl != _m_lvl)
                    _m_lvlRefObj = dungeonRefObj?.getLvlRef(_m_lvl);
                return _m_lvlRefObj;
            }
        }
        
        public long startMs { get { return _m_startMs; } }
        
        /// <summary>
        /// 已经结束的怪物数量
        /// </summary>
        public int endMonsterCount {
            get
            {
                int count = 0;
                if (monsterList == null) 
                    return count;
                foreach (var monster in monsterList)
                {
                    if (monster != null && monster.hp <= 0)
                    {
                        count++;
                    }
                }
                return count;
            } 
        }

        public int totalMonsterCount
        {
            get
            {
                if (dungeonRefObj != null && dungeonRefObj.monoster_list != null) return dungeonRefObj.monoster_list.Count;
                return 0;
            }
        }

        public List<GuildDungeonMonster> monsterList => _m_monsterList;
        
        public GuildDungeonMonster bossMonster
        {
            get
            {
                if (_m_bossMonster == null)
                {
                    _m_bossMonster = getMonsterInfo(_m_dungeonRefObj.boss_id);
                }
                return _m_bossMonster;
            }
        }
        public int xMaxLayerCount => _m_xMaxLayerCount;
        public int yMaxLayerCount => _m_yMaxLayerCount;
        
        public List<long> tagMonsterIdsList => _m_tagMonsterIdsList;

        /// <summary>
        /// 开启副本所需的公会财富
        /// </summary>
        public long starCostGuildWealth
        {
            get
            {
                if (lvlRefObj != null) return lvlRefObj.star_cost_guild_wealth;
                return 0;
            }
        }

        public EGuildDungeonState state
        {
            get
            {
                if(NPPlayer.instance.guildComp.guildInfo == null)
                    return EGuildDungeonState.Lock;
                if(NPPlayer.instance.guildComp.guildInfo.level < _m_dungeonRefObj.unlock_need_guild_lvl)
                    return EGuildDungeonState.Lock;
                if(endMonsterCount >= totalMonsterCount)
                    return EGuildDungeonState.End;
                if (_m_startMs > 0)
                    return EGuildDungeonState.Started;
                
                return EGuildDungeonState.WaitStart;
            }
        }

        /// <summary>
        /// 有奖励可领取
        /// </summary>
        public bool hasRewardToGain
        {
            get
            {
                foreach (GuildDungeonMonster monster in _m_monsterList)
                {
                    if (monster != null && monster.isReward && monster.hp <=0 && !hasGainedRewardMonster(monster.monsterId))
                        return true;
                }
                return false;
            }
        }
        


        public GuildDungeonInfo(long _dungeonRefreshTimeTag,[NotNull] GuildDungeonRefObj _ref, GuildDungeon_SetInfo _info = null)
        {
            _m_dungeonRefreshTimeTag = _dungeonRefreshTimeTag;
            _m_dungeonId = _ref.id;
            _m_dungeonRefObj = _ref;
            if (_info != null) 
                _m_lvl = _info.getLvl();
            else
                _m_lvl = 1;
            _m_instanceLvl = 0;
            _m_startMs = 0;
            _m_monsterList = new List<GuildDungeonMonster>();
            _m_tagMonsterIdsList = new List<long>();
            _m_hasGainRewardMonsterIdsList = new List<long>();
            // 计算每个怪物的层级和位置信息
            Dictionary<long, MonsterLayerInfo> monsterLayerInfo = _calculateMonsterLayersAndPositions(dungeonRefObj.monoster_list);

            int yMaxLayerCount = 0;
            int xMaxLayerCount = 0;
            foreach (GuildDungeonMonsterRefObj monsterRefObj in dungeonRefObj.monoster_list)
            {
                if (monsterRefObj == null)
                    continue;

                long maxHp = lvlRefObj.getMonsterHp(monsterRefObj.monster_type);
                long hp = maxHp;

                // 获取怪物的层级信息
                if (monsterLayerInfo.ContainsKey(monsterRefObj.id))
                {
                    MonsterLayerInfo layerInfo = monsterLayerInfo[monsterRefObj.id];
                    if(layerInfo.layer > yMaxLayerCount)
                        yMaxLayerCount = layerInfo.layer;
                    if(layerInfo.totalInLayer > xMaxLayerCount)
                        xMaxLayerCount = layerInfo.totalInLayer;
                    _m_monsterList.Add(new GuildDungeonMonster(monsterRefObj,  this, layerInfo.layer, layerInfo.positionInLayer, layerInfo.totalInLayer, hp));
                }
                else
                {
                    // 如果没有找到层级信息，使用默认值
                    _m_monsterList.Add(new GuildDungeonMonster(monsterRefObj, this, 0, 0, 1, hp));
                }
            }

            _m_xMaxLayerCount = xMaxLayerCount;
            _m_yMaxLayerCount = yMaxLayerCount + 1;
            
        }
        public void updateSetInfo(long _dungeonRefreshTimeTag, GuildDungeon_SetInfo _info)
        {
            if (_info == null)
                return;
            if (_m_dungeonRefreshTimeTag != _dungeonRefreshTimeTag)
            {
                nextDayResetInfo(_m_dungeonRefreshTimeTag);
            }
            _m_lvl = _info.getLvl();
        }

        public void updateInstanceInfo(long _dungeonRefreshTimeTag, GuildDungeon_InstanceInfo _instanceInfo)
        {
            if (_instanceInfo == null)
                return;

            if (_m_dungeonRefreshTimeTag != _dungeonRefreshTimeTag)
            {
                nextDayResetInfo(_m_dungeonRefreshTimeTag);
            }
            _m_instanceId = _instanceInfo.getId();
            _m_instanceLvl = _instanceInfo.getLvl();
            _m_startMs = _instanceInfo.getStartMs();

            _updateMonsterInfo(_instanceInfo.getMonsterList());
            updateTagMonsterInfo(_instanceInfo.getTagMonsterIdLiist());
        }

        public void updateTagMonsterInfo(List<long> _tagMonsters)
        {
            _m_tagMonsterIdsList.Clear();
            List<long> tagList = _tagMonsters;
            if (tagList != null) 
                _m_tagMonsterIdsList.AddRange(tagList);
        }

        public void updateGainRewardMonster(List<GuildDungeon_DungeonMonster> gainedRewardMonsterIdList)
        {
            if(gainedRewardMonsterIdList == null)
                return;
            _m_hasGainRewardMonsterIdsList.Clear();
            foreach (var monster in gainedRewardMonsterIdList)
            {
                if(monster != null && monster.getId() == instanceId)
                    _m_hasGainRewardMonsterIdsList.Add(monster.getMonsterId());
            }
        }

        public bool hasGainedRewardMonster(long _monsterId)
        {
            return _m_hasGainRewardMonsterIdsList.Contains(_monsterId);
        }

        public GuildDungeonMonster getMonsterInfo(long _monsterId)
        {
            if (_m_monsterList == null || _m_monsterList.Count <= 0)
                return null;

            foreach (var monster in _m_monsterList)
            {
                if (monster != null && monster.monsterId == _monsterId)
                    return monster;
            }

            return null;
        }

        public void nextDayResetInfo(long _dungeonRefreshTimeTag)
        {
            _m_instanceId = 0;
            _m_instanceLvl = _m_lvl;
            _m_startMs = 0;
            _m_bossMonster = null;
            foreach (GuildDungeonMonster monster in _m_monsterList)
            {
                monster?.nextDayReset();
            }
            _m_dungeonRefreshTimeTag = _dungeonRefreshTimeTag;
            _m_tagMonsterIdsList.Clear();
            _m_hasGainRewardMonsterIdsList.Clear();
        }
        
        public bool hasTagMonster(long _monsterId)
        {
            return _m_tagMonsterIdsList.Contains(_monsterId);
        }

        private void _updateMonsterInfo(List<GuildDungeon_Monster> _monsterList)
        {
            if (_monsterList == null) return;
            
            foreach (GuildDungeon_Monster monsterData in _monsterList)
            {
                if(monsterData == null)
                    continue;
                GuildDungeonMonster monster = getMonsterInfo(monsterData.getMonsterId());
                monster?.updateInfo(monsterData.getHp(), monsterData.getIsReward());
            }
        }
        
        /// <summary>
        /// 怪物层级信息
        /// </summary>
        private struct MonsterLayerInfo
        {
            public int layer;           // 层级
            public int positionInLayer; // 在该层级中的位置索引
            public int totalInLayer;    // 该层级总怪物数量
            
            public MonsterLayerInfo(int layer, int positionInLayer, int totalInLayer)
            {
                this.layer = layer;
                this.positionInLayer = positionInLayer;
                this.totalInLayer = totalInLayer;
            }
        }
        
        /// <summary>
        /// 计算每个怪物的层级和位置信息
        /// </summary>
        /// <param name="monsterList">怪物列表</param>
        /// <returns>怪物ID到层级信息的映射</returns>
        private static Dictionary<long, MonsterLayerInfo> _calculateMonsterLayersAndPositions(List<GuildDungeonMonsterRefObj> monsterList)
        {
            // 首先计算基本的层级
            Dictionary<long, int> layers = _calculateMonsterLayers(monsterList);
            
            // 按层级分组怪物
            Dictionary<int, List<GuildDungeonMonsterRefObj>> layerGroups = new Dictionary<int, List<GuildDungeonMonsterRefObj>>();
            
            foreach (var monster in monsterList)
            {
                if (monster != null && layers.ContainsKey(monster.id))
                {
                    int layer = layers[monster.id];
                    if (!layerGroups.ContainsKey(layer))
                        layerGroups[layer] = new List<GuildDungeonMonsterRefObj>();
                    
                    layerGroups[layer].Add(monster);
                }
            }
            
            // 计算每个怪物在其层级中的位置
            Dictionary<long, MonsterLayerInfo> result = new Dictionary<long, MonsterLayerInfo>();
            
            foreach (var kvp in layerGroups)
            {
                int layer = kvp.Key;
                List<GuildDungeonMonsterRefObj> monstersInLayer = kvp.Value;
                int totalInLayer = monstersInLayer.Count;
                
                // 按照怪物在原始列表中的顺序排序
                monstersInLayer.Sort((a, b) => 
                {
                    int indexA = monsterList.IndexOf(a);
                    int indexB = monsterList.IndexOf(b);
                    return indexA.CompareTo(indexB);
                });
                
                // 为每个怪物分配位置索引
                for (int i = 0; i < monstersInLayer.Count; i++)
                {
                    result[monstersInLayer[i].id] = new MonsterLayerInfo(layer, i, totalInLayer);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 计算每个怪物的层级
        /// </summary>
        /// <param name="monsterList">怪物列表</param>
        /// <returns>怪物ID到层级的映射</returns>
        private static Dictionary<long, int> _calculateMonsterLayers(List<GuildDungeonMonsterRefObj> monsterList)
        {
            Dictionary<long, int> layers = new Dictionary<long, int>();
            Dictionary<long, GuildDungeonMonsterRefObj> monsterMap = new Dictionary<long, GuildDungeonMonsterRefObj>();
            
            // 建立ID到怪物对象的映射
            foreach (var monster in monsterList)
            {
                if (monster != null)
                {
                    monsterMap[monster.id] = monster;
                    layers[monster.id] = -1; // 初始化为-1，表示未计算
                }
            }
            
            // 递归计算每个怪物的层级
            foreach (var monster in monsterList)
            {
                if (monster != null && layers[monster.id] == -1)
                {
                    _calculateMonsterLayer(monster, monsterMap, layers);
                }
            }
            
            return layers;
        }
        
        /// <summary>
        /// 递归计算单个怪物的层级
        /// </summary>
        /// <param name="monster">当前怪物</param>
        /// <param name="monsterMap">怪物映射</param>
        /// <param name="layers">层级映射</param>
        /// <returns>怪物的层级</returns>
        private static int _calculateMonsterLayer(GuildDungeonMonsterRefObj monster, Dictionary<long, GuildDungeonMonsterRefObj> monsterMap, Dictionary<long, int> layers)
        {
            // 如果已经计算过，直接返回
            if (layers[monster.id] != -1)
                return layers[monster.id];
            
            int maxPreLayer = -1;
            
            // 如果有前置怪物，计算前置怪物的最大层级
            if (monster.pre_monster_id_list != null && monster.pre_monster_id_list.Count > 0)
            {
                foreach (long preMonsterID in monster.pre_monster_id_list)
                {
                    if (monsterMap.ContainsKey(preMonsterID))
                    {
                        int preLayer = _calculateMonsterLayer(monsterMap[preMonsterID], monsterMap, layers);
                        maxPreLayer = Math.Max(maxPreLayer, preLayer);
                    }
                }
            }
            
            // 当前怪物的层级 = 前置怪物的最大层级 + 1
            layers[monster.id] = maxPreLayer + 1;
            
            return layers[monster.id];
        }
        
        
    }
}