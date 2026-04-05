using ALPackage;
using Common.MarsEnum;
using JetBrains.Annotations;
using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    //火星相关
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 初始化火星基地相关配表
        /// </summary>
        private void _initMarsRefCore()
        {
            // 火星满意度表初始化，按照satisfaction_degree_per从小到大排序
            marsSatisfactionDegreeRefCore?.refList?.Sort((_a, _b) => 
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;
                
                return _a.satisfaction_degree_per.CompareTo(_b.satisfaction_degree_per);
            });
            
            // marsImmigrationRefCore 按照times从小到大排序
            marsImmigrationRefCore?.refList?.Sort((_a, _b) => 
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if (object.ReferenceEquals(_a, _b)) return 0;
                
                return _a.times.CompareTo(_b.times);
            });

            _initMarsTechnology();
        }

        /// <summary>
        /// 根据节点id获取火星航行配置
        /// </summary>
        /// <param name="_stageId"></param>
        /// <returns></returns>
        public MarsGoRouteRefObj getMarsGoRouteRefByStageId(long _stageId)
        {
            if (marsGoRouteRefCore == null)
                return null;

            foreach (MarsGoRouteRefObj goRouteRef in marsGoRouteRefCore.refList)
            {
                if (goRouteRef != null && goRouteRef.stage_id == _stageId)
                {
                    return goRouteRef;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_satisfactionDegreePer">满意度万分比</param>
        /// <returns></returns>
        public MarsSatisfactionDegreeRefObj getMarsSatisfactionDegreeRefByPer(int _satisfactionDegreePer)
        {
            if (marsSatisfactionDegreeRefCore == null || marsSatisfactionDegreeRefCore.refList == null || marsSatisfactionDegreeRefCore.refList.Count == 0)
                return null;
            
            // 因为初始化时已经排序，所以这里从后往前找第一个小于等于传入值的即可
            for (int i = marsSatisfactionDegreeRefCore.refList.Count - 1; i >= 0; i--)
            {
                MarsSatisfactionDegreeRefObj item = marsSatisfactionDegreeRefCore.refList[i];
                if (item == null)
                    continue;

                if (item.satisfaction_degree_per <= _satisfactionDegreePer)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 第times次移民的配表数据
        /// </summary>
        /// <returns></returns>
        public MarsImmigrationRefObj getMarsImmigrationRef(long _times)
        {
            MarsImmigrationRefObj resultRefObj = null;
            foreach (var item in marsImmigrationRefCore.refList)
            {
                if(item == null)
                    continue;

                if (item.times < _times)
                {
                    resultRefObj = item;
                }
                else if (item.times == _times)
                {
                    resultRefObj = item;
                    break;
                }
                else
                {
                    break;
                }
            }
            
            if(resultRefObj == null)
                Debug.LogError($"[GRefdataCoreMgr] getMarsImmigrationRef 未找到对应移民次数的配置，times={_times}");
            
            return resultRefObj;
        }

        #region 火星科技

        [NotNull] private Dictionary<EMarsTechnologyType, MarsTechnologyTypeLayerGroup> _m_dMarsTechnologyTypeLayerGroupDict = new Dictionary<EMarsTechnologyType, MarsTechnologyTypeLayerGroup>();
        
        [NotNull] public MarsTechnologyTypeLayerGroup getMarsTechnologyTypeLayerGroup(EMarsTechnologyType _technologyType)
        {
            if (!_m_dMarsTechnologyTypeLayerGroupDict.TryGetValue(_technologyType, out MarsTechnologyTypeLayerGroup typeLayerGroup) || typeLayerGroup == null)
            {
                typeLayerGroup = new MarsTechnologyTypeLayerGroup(_technologyType);
                _m_dMarsTechnologyTypeLayerGroupDict.Add(_technologyType, typeLayerGroup);
            }

            return typeLayerGroup;
        }
        
        public class MarsTechnologyTypeLayerGroup
        {
            private EMarsTechnologyType _m_eTechnologyType;
            [NotNull] private Dictionary<int, MarsTechnologyLayerRefObj> _m_dLayerDict = new Dictionary<int, MarsTechnologyLayerRefObj>();
            
            private int _m_iMaxLayer = MarsTechnologyLayerRefObj.RootLayer;// 最大层级

            public MarsTechnologyTypeLayerGroup(EMarsTechnologyType _technologyType)
            {
                _m_eTechnologyType = _technologyType;
            }
            
            public EMarsTechnologyType technologyType { get { return _m_eTechnologyType; } }
            public int maxLayer { get { return _m_iMaxLayer; } }
            
            /// <summary>
            /// 为某层级添加科技节点
            /// </summary>
            public void addTechnologyToLayer(int _layer, MarsTechnologyRefObj _technologyRef)
            {
                if(_technologyRef == null)
                    return;
                
                if (_layer < MarsTechnologyLayerRefObj.RootLayer)
                {
                    Debug.LogError($"[GRefdataCoreMgr] addTechnologyToLayer 添加的层级不能小于MarsTechnologyLayerRefObj.RootLayer:{MarsTechnologyLayerRefObj.RootLayer}, 当前传入层级:{_layer}, 科技id:{_technologyRef.id}");
                    return;
                }
                
                if (_layer > _m_iMaxLayer)
                    _m_iMaxLayer = _layer;
                
                if(_m_dLayerDict.TryGetValue(_layer, out MarsTechnologyLayerRefObj layerRef) && layerRef != null)
                {
                    layerRef.addTechnologyRef(_technologyRef);
                }
                else
                {
                    layerRef = new MarsTechnologyLayerRefObj(_layer);
                    layerRef.addTechnologyRef(_technologyRef);
                    _m_dLayerDict.Add(_layer, layerRef);
                }
            }

            /// <summary>
            /// 添加根节点科技
            /// </summary>
            public void addRootTechnology(MarsTechnologyRefObj _technologyRef)
            {
                if(_technologyRef == null)
                    return;
                
                if(_technologyRef.parent_list != null && _technologyRef.parent_list.Count > 0)
                {
                    Debug.LogError($"[GRefdataCoreMgr] addRootTechnology 添加的科技:{_technologyRef.id}不是根节点科技, 它有父节点");
                    return;
                }

                addTechnologyToLayer(MarsTechnologyLayerRefObj.RootLayer, _technologyRef);
            }
            
            /// <summary>
            /// 获取某层级的科技信息
            /// </summary>
            /// <param name="_layer"></param>
            /// <returns></returns>
            public MarsTechnologyLayerRefObj getTechnologyLayerRef(int _layer)
            {
                return _m_dLayerDict.GetValueOrDefault(_layer);
            }

            /// <summary>
            /// 获取第一个正在升级中或可升级的科技节点
            /// </summary>
            /// <returns></returns>
            public MarsTechnologyRefObj getFirstUpgradingOrCanUpgradeTechnology()
            {
                for (int layer = MarsTechnologyLayerRefObj.RootLayer; layer <= _m_iMaxLayer; layer++)
                {
                    MarsTechnologyLayerRefObj layerRefObj = getTechnologyLayerRef(layer);
                    if (layerRefObj == null)
                        continue;

                    foreach (var technologyRef in layerRefObj.technologyRefList)
                    {
                        EMarsTechnologyState technologyState = MarsUtil.getTechnologyState(technologyRef);
                        if (technologyState is EMarsTechnologyState.LEVEL_0_CONDITION_REACH or 
                            EMarsTechnologyState.NORMAL or EMarsTechnologyState.UPGRADED or EMarsTechnologyState.UPGRADEING)
                            return technologyRef;
                    }
                }

                return null;
                
            }
        }
        
        /// <summary>
        /// 火星科技层级信息
        /// </summary>
        public class MarsTechnologyLayerRefObj
        {
            public static int RootLayer = 1;// 根节点层级(直接固定)
            
            private int _m_iLayer;//当前层级
            [NotNull] [ItemNotNull] private List<MarsTechnologyRefObj> _m_lTechnologyRefList = new List<MarsTechnologyRefObj>();//当前层级拥有的科技

            public MarsTechnologyLayerRefObj(int _layer)
            {
                _m_iLayer = _layer;
            }
            
            public int layer { get { return _m_iLayer; } }
            [NotNull] [ItemNotNull] public List<MarsTechnologyRefObj> technologyRefList { get { return _m_lTechnologyRefList; } }

            /// <summary>
            /// 为该层级添加科技节点
            /// </summary>
            /// <param name="_technologyRef"></param>
            public void addTechnologyRef(MarsTechnologyRefObj _technologyRef)
            {
                if (_technologyRef == null)
                    return;
                
                if(_technologyRef.layer != 0 && _technologyRef.layer != _m_iLayer)
                {
                    // 已经分配过层级, 但是分配的层级和当前要分配层级不符
                    Debug.LogError($"[GRefdataCoreMgr MarsTechnologyLayerRefObj.addTechnologyRef] 科技:{_technologyRef.id} 层级错误, 它已经被分配到层级:{_technologyRef.layer}， 不能再分配到层级:{_m_iLayer}");
                }
                else if (_technologyRef.layer != 0 && _technologyRef.layer == _m_iLayer)
                {
                    // 已经分配过层级, 且分配的层级和当前要分配层级相符, 不做处理
                }
                else
                {
                    // 还未分配层级
                    _technologyRef.layer = _m_iLayer;
                    _m_lTechnologyRefList.Add(_technologyRef);
                }
            }
        }
        
        public MarsTechnologyLayerRefObj getMarsTechnologyLayerRefObj(EMarsTechnologyType _technologyType, int _layer)
        {
            MarsTechnologyTypeLayerGroup typeLayerGroup = getMarsTechnologyTypeLayerGroup(_technologyType);
            return typeLayerGroup.getTechnologyLayerRef(_layer);
        }
        
        /// <summary>
        /// 初始化火星科技
        /// </summary>
        public void _initMarsTechnology()
        {
            Dictionary<long, MarsTechnologyRefObj> technologyDict = new Dictionary<long, MarsTechnologyRefObj>();
            
            // 为每个科技节点建立父子关系
            foreach (var techRef in marsTechnologyRefCore.refList)
            {
                if(techRef == null)
                    continue;

                // 该科技没有父节点, 说明它是根节点
                if (techRef.parent_list == null || techRef.parent_list.Count <= 0)
                {
                    // 若是根节点, 则加入对应类型的根节点列表中
                    MarsTechnologyTypeLayerGroup technologytypeLayerGroup = getMarsTechnologyTypeLayerGroup(techRef.type);
                    technologytypeLayerGroup.addRootTechnology(techRef);
                }
                else// 该科技有父节点
                {
                    MarsTechnologyRefObj parentTechRefObj = null;
                    foreach (var parentId in techRef.parent_list)
                    {
                        if(!technologyDict.TryGetValue(parentId, out parentTechRefObj) || parentTechRefObj == null)
                        {
                            parentTechRefObj = marsTechnologyRefCore.getRef(parentId);
                            technologyDict[parentId] = parentTechRefObj;
                        }

                        if (parentTechRefObj == null)
                        {
                            Debug.LogError($"[GRefdataCoreMgr] _initMarsTechnology 未找到科技id:{techRef.id}的父节点科技id:{parentId}的配表数据");
                            continue;
                        }
                        
                        parentTechRefObj.childTechnologyList.Add(techRef);
                        techRef.parentTechnologyList.Add(parentTechRefObj);
                    }
                }
            }
            
            // 为每个科技节点分配层级
            foreach (var techTypeLayerGroup in _m_dMarsTechnologyTypeLayerGroupDict.Values)
            {
                if (techTypeLayerGroup == null)
                    continue;

                int layer = MarsTechnologyLayerRefObj.RootLayer;// 从根节点开始为每个科技节点分配层级
                MarsTechnologyLayerRefObj layerRefObj = techTypeLayerGroup.getTechnologyLayerRef(layer);
                while (layerRefObj != null && layerRefObj.technologyRefList.Count > 0)
                {
                    int nextLayer = layer + 1;//下一层
                    
                    // 遍历layer层的所有科技节点, 为它们的子节点分配到nextLayer层
                    foreach (var technologyRefObj in layerRefObj.technologyRefList)
                    {
                        if (technologyRefObj.childTechnologyList.Count > 0)
                        {
                            foreach (var childTechnologyRefObj in technologyRefObj.childTechnologyList)
                            {
                                techTypeLayerGroup.addTechnologyToLayer(nextLayer, childTechnologyRefObj);
                            }
                        }
                    }

                    // 继续遍历下一层
                    layer = nextLayer;
                    layerRefObj = techTypeLayerGroup.getTechnologyLayerRef(layer);
                }
            }
        }
        
        /// <summary>
        /// 获取火星科技等级配表id
        /// </summary>
        /// <returns></returns>
        public long getMarsTechnologyLevelRefId(long _teechnologyId, int _level)
        {
            return _teechnologyId * 100 + _level;//默认规则, 科技等级配表id = 科技id * 100 + 等级
        }
        
        /// <summary>
        /// 获取火星科技等级配置
        /// </summary>
        /// <param name="_level"></param>
        /// <returns></returns>
        public MarsTechnologyLevelRefObj getMarsTechnologyLevelRefObj(long _teechnologyId, int _level)
        {
            long technologyLevelId = getMarsTechnologyLevelRefId(_teechnologyId, _level);
            return marsTechnologyLevelRefCore.getRef(technologyLevelId);
        }

        /// <summary>
        /// 获取第一个正在升级中或可升级的科技节点通过技能类型
        /// </summary>
        /// <param name="_skillTypeTpye"></param>
        /// <returns></returns>
        public MarsTechnologyRefObj getFirstUpgradingOrCanUpgradeTechnologyBySkillType(int _skillTypeTpye, bool _onNotFindReturnFirst)
        {
            MarsTechnologyRefObj firstShowTechnologyRef = null;
            EMarsTechnologyState firstShowTechnologyState = EMarsTechnologyState.NONE;
            foreach (var technologyRef in GRefdataCoreMgr.instance.marsTechnologyRefCore.refList)
            {
                if(technologyRef == null || technologyRef.skill_type_id != _skillTypeTpye)
                    continue;
                
                EMarsTechnologyState technologyState = MarsUtil.getTechnologyState(technologyRef);
                if (technologyState is EMarsTechnologyState.LEVEL_0_CONDITION_REACH or 
                    EMarsTechnologyState.NORMAL or EMarsTechnologyState.UPGRADED or EMarsTechnologyState.UPGRADEING)
                    return technologyRef;

                if (firstShowTechnologyRef == null 
                    || (firstShowTechnologyState == EMarsTechnologyState.MAX_LEVEL && technologyState != EMarsTechnologyState.MAX_LEVEL)
                    || (firstShowTechnologyState == EMarsTechnologyState.LOCK && technologyState == EMarsTechnologyState.LEVEL_NOT0_CONDITION_NOTREACH)
                    || firstShowTechnologyState < technologyState)
                {
                    firstShowTechnologyRef = technologyRef;
                    firstShowTechnologyState = technologyState;
                }
            }

            return _onNotFindReturnFirst ? firstShowTechnologyRef : null;
        }

        /// <summary>
        /// 获取火星科技等级属性展示信息
        /// </summary>
        /// <param name="_technologyRefObj"></param>
        /// <param name="_lvlPropertyInfoList"></param>
        /// <param name="_allPropertyList"></param>
        /// <param name="_containsMarsPower"></param>
        public void getMarsTechnologyLvlPropertyShowInfo(MarsTechnologyRefObj _technologyRefObj, List<MarsLvlPropertyShowInfo> _lvlPropertyInfoList, List<_IPropertyShow> _allPropertyList, bool _containsMarsPower = true)
        {
            _lvlPropertyInfoList?.Clear();
            _allPropertyList?.Clear();
            if(_technologyRefObj == null)
                return;
            
            for(int level = 1; level <= _technologyRefObj.max_level; level++)
            {
                MarsTechnologyLevelRefObj levelRefObj = GRefdataCoreMgr.instance.getMarsTechnologyLevelRefObj(_technologyRefObj.id, level);
                if(levelRefObj == null)
                    continue;

                Dictionary<_IPropertyShow, long> levelPropertyDict = new Dictionary<_IPropertyShow, long>();
                levelRefObj.addPropertyToCollections(levelPropertyDict, _allPropertyList, _containsMarsPower);
                    
                _lvlPropertyInfoList?.Add(new MarsLvlPropertyShowInfo()
                {
                    lvl = level,
                    propertyValueDic = levelPropertyDict,
                });
            }
        }
        
        #endregion

        #region 加速

        /// <summary>
        /// 处理可以使用火星时间加速背包道具
        /// </summary>
        /// <param name="_timeType"></param>
        /// <param name="_func"> 返回false时 中断遍历</param>
        public void dealCanUseMarsTimeReduceBagItem(EMarsBagItemUseTimeType _timeType, Func<MarsBagItemTimeReduceRefObj, bool> _func)
        {
            if(_func == null)
                return;
            
            foreach (var timeReduceRefObj in marsBagItemTimeReduceRefCore.refList)
            {
                if(timeReduceRefObj == null)
                    continue;
                
                // 潜规则 : time_type为ALL的道具可以用于所有时间类型的加速
                if (timeReduceRefObj.time_type == _timeType || timeReduceRefObj.time_type == EMarsBagItemUseTimeType.ALL)
                {
                    if (!_func(timeReduceRefObj))
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        public long getMarsExploreMineCollectBonus(long _power)
        {
            long addPer = 0;

            List<MarsExploreCollectBonusRefObj> refList = marsExploreCollectBonusRefCore.refList;
            foreach (MarsExploreCollectBonusRefObj refObj in refList)
            {
                if (refObj.need_team_power >= _power)
                    break;

                addPer = refObj.add_collect_speed;
            }

            return addPer;
        }

        /// <summary>
        /// 根据全服累计抵达人数查询时长减少万分比，无匹配则返回0
        /// first=-1表示无下限，second=-1表示无上限
        /// </summary>
        /// <returns></returns>
        public long getMarsStageArriveReducePer()
        {
            long arriveCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.MARS_GO_ROUTE_ARRIVE_COUNT);
            List<WCGTripleLong> reduceList = GRefdataCoreMgr.instance.npGeneral.mars_go_route_arrive_reduce_list;
            if (reduceList == null)
                return 0;

            for (int i = 0; i < reduceList.Count; i++)
            {
                WCGTripleLong temp = reduceList[i];
                if (temp == null)
                    continue;

                if (WCGIntRange.inRange(arriveCount, temp.first(), temp.second()))
                    return temp.third();
            }
            return 0;
        }
    }
}