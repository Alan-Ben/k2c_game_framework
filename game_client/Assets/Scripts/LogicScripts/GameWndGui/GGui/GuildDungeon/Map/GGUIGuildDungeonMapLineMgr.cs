using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// 联盟PVE地图连线管理器
    /// </summary>
    public class GGUIGuildDungeonMapLineMgr
    {
        private GuildDungeonMapLineConfigMono _m_sceneMono;
        private List<GuildDungeonMapLine> _m_lineList;

        public GGUIGuildDungeonMapLineMgr()
        {
            _m_lineList = new List<GuildDungeonMapLine>();
        }

        public void init([NotNull]GuildDungeonMapLineConfigMono _sceneMono)
        {
            _m_sceneMono = _sceneMono;
        }

        public void discard()
        {
            clearAllLines();
            _m_lineList = null;
        }

        public void createLines(List<GuildDungeonMapLinePoint> viewList, GuildDungeonInfo dungeonInfo, Vector3 _startPointPos)
        {
            if (viewList == null || dungeonInfo == null)
                return;

            clearAllLines();

            // 创建怪物ID到View的映射
            Dictionary<long, GuildDungeonMapLinePoint> monsterViewMap = new Dictionary<long, GuildDungeonMapLinePoint>();
            foreach (var view in viewList)
            {
                if (view != null && view.monster != null)
                {
                    monsterViewMap[view.monster.monsterId] = view;
                }
            }

            // 统计每个怪物的出度（作为from点的连接数）和入度（作为to点的连接数）
            Dictionary<long, int> outDegreeMap = new Dictionary<long, int>(); // 出度：作为前置怪物被引用的次数
            Dictionary<long, int> inDegreeMap = new Dictionary<long, int>();  // 入度：前置怪物的数量
            
            // 统计起点的出度（有多少怪物没有前置）
            int startPointOutDegree = 0;
            
            foreach (var monster in dungeonInfo.monsterList)
            {
                if (monster == null || monster.monsterRefObj == null)
                    continue;

                long monsterId = monster.monsterId;
                
                // 初始化入度
                int preCount = monster.monsterRefObj.pre_monster_id_list?.Count ?? 0;
                inDegreeMap[monsterId] = preCount;
                
                // 统计起点连接数
                if (preCount == 0)
                {
                    startPointOutDegree++;
                }
                
                // 统计出度
                if (monster.monsterRefObj.pre_monster_id_list != null)
                {
                    foreach (long preMonsterID in monster.monsterRefObj.pre_monster_id_list)
                    {
                        if (!outDegreeMap.ContainsKey(preMonsterID))
                            outDegreeMap[preMonsterID] = 0;
                        outDegreeMap[preMonsterID]++;
                    }
                }
            }

            // 用于记录每个点当前已使用的连接索引
            Dictionary<long, int> fromIndexMap = new Dictionary<long, int>();
            Dictionary<long, int> toIndexMap = new Dictionary<long, int>();
            int startPointIndex = 0; // 起点当前连接索引

            // 为每个怪物创建到其前置怪物的连线
            foreach (var monster in dungeonInfo.monsterList)
            {
                if (monster == null || monster.monsterRefObj == null)
                    continue;

                if (!monsterViewMap.ContainsKey(monster.monsterId))
                    continue;

                GuildDungeonMapLinePoint toView = monsterViewMap[monster.monsterId];
                if(toView == null)
                    continue;
                if (monster.monsterRefObj.pre_monster_id_list == null || monster.monsterRefObj.pre_monster_id_list.Count == 0)
                {
                    // 从起点出发的连线，计算起点的x偏移
                    float startXOffset = 0;
                    float baseXOffset = _m_sceneMono?.xPosOffset ?? 0;
                    
                    if (startPointOutDegree > 1)
                    {
                        // 居中分布：从 -(n-1)/2 到 (n-1)/2
                        startXOffset = (startPointIndex - (startPointOutDegree - 1) * 0.5f) * baseXOffset;
                        startPointIndex++;
                    }
                    
                    // 计算to点的x偏移（根据入度）
                    float toXOffset = 0;
                    int toInDegree = inDegreeMap.ContainsKey(monster.monsterId) ? inDegreeMap[monster.monsterId] : 0;
                    if (toInDegree > 1)
                    {
                        if (!toIndexMap.ContainsKey(monster.monsterId))
                            toIndexMap[monster.monsterId] = 0;
                        
                        int currentToIndex = toIndexMap[monster.monsterId];
                        toXOffset = (currentToIndex - (toInDegree - 1) * 0.5f) * baseXOffset;
                        toIndexMap[monster.monsterId]++;
                    }
                   
                    Vector3 fromOffset = new Vector3(startXOffset, _m_sceneMono?.fromYPosOffset ?? 0 , 0);
                    Vector3 toOffset = new Vector3(toXOffset, -_m_sceneMono?.toYPosOffset ?? 0 , 0);
                    // 起点连线：fromMonster为null表示解锁状态
                    createLine(_startPointPos + fromOffset, toView.getWorldPosition() + toOffset, true, null);
                    continue;
                }
                
                foreach (long preMonsterID in monster.monsterRefObj.pre_monster_id_list)
                {
                    if (monsterViewMap.ContainsKey(preMonsterID))
                    {
                        GuildDungeonMapLinePoint fromView = monsterViewMap[preMonsterID];
                        if(fromView == null)
                            continue;
                        bool flip = fromView.monster != null && toView.monster != null && fromView.monster.totalInLayer < toView.monster.totalInLayer;

                        float fromXOffset = 0;
                        float toXOffset = 0;
                        float baseXOffset = _m_sceneMono?.xPosOffset ?? 0;
                        
                        // 计算from点的x偏移（根据出度）
                        int fromOutDegree = outDegreeMap.ContainsKey(preMonsterID) ? outDegreeMap[preMonsterID] : 0;
                        if (fromOutDegree > 1)
                        {
                            if (!fromIndexMap.ContainsKey(preMonsterID))
                                fromIndexMap[preMonsterID] = 0;
                            
                            int currentFromIndex = fromIndexMap[preMonsterID];
                            // 居中分布：从 -(n-1)/2 到 (n-1)/2
                            fromXOffset = (currentFromIndex - (fromOutDegree - 1) * 0.5f) * baseXOffset;
                            fromIndexMap[preMonsterID]++;
                        }
                        
                        // 计算to点的x偏移（根据入度）
                        int toInDegree = inDegreeMap.ContainsKey(monster.monsterId) ? inDegreeMap[monster.monsterId] : 0;
                        if (toInDegree > 1)
                        {
                            if (!toIndexMap.ContainsKey(monster.monsterId))
                                toIndexMap[monster.monsterId] = 0;
                            
                            int currentToIndex = toIndexMap[monster.monsterId];
                            // 居中分布：从 -(n-1)/2 到 (n-1)/2
                            toXOffset = (currentToIndex - (toInDegree - 1) * 0.5f) * baseXOffset;
                            toIndexMap[monster.monsterId]++;
                        }
                        
                        Vector3 fromOffset = new Vector3(fromXOffset, _m_sceneMono?.fromYPosOffset ?? 0 , 0);
                        Vector3 toOffset = new Vector3(toXOffset, -_m_sceneMono?.toYPosOffset ?? 0 , 0);
                        createLine(fromView.getWorldPosition() + fromOffset, toView.getWorldPosition() + toOffset, flip, fromView.monster);
                    }
                }
            }
        }


        private void createLine(Vector3 _fromPos, Vector3 _toPos, bool _flip, GuildDungeonMonster _fromMonster)
        {
            if (_m_sceneMono.lineParent == null)
                return;

            GameObject lineGO = null;
            if (_m_sceneMono.linePrefab != null)
            {
                lineGO = Object.Instantiate(_m_sceneMono.linePrefab, _m_sceneMono.lineParent);
            }
            else
            {
                // 创建默认连线GameObject
                lineGO = new GameObject("DungeonLine");
                lineGO.transform.SetParent(_m_sceneMono.lineParent);
            }
            if(lineGO != null)
                lineGO.transform.localPosition = Vector3.zero;

            GuildDungeonMapLine line = new GuildDungeonMapLine(lineGO);
            
            line.setLineData(_fromPos, _toPos, _flip);
            
            if (_m_sceneMono != null)
            {
                Vector2 start = _m_sceneMono.curveStrength * new Vector2(_m_sceneMono.bezierStartX, _m_sceneMono.bezierStartY);
                Vector2 end   = _m_sceneMono.curveStrength * new Vector2(_m_sceneMono.bezierEndX, _m_sceneMono.bezierEndY);
                line.setCurveParams(_m_sceneMono.curvePoints,  _m_sceneMono.lineWidth, _m_sceneMono.lineStyle, 
                    _m_sceneMono.allowInvertPointOrder, start, end, _m_sceneMono.cornerRadius);
            }
            line.init();
            
            // 根据fromMonster的血量设置材质
            // fromMonster == null 表示起点，使用解锁材质
            // fromMonster.hp <= 0 表示怪物已击败，使用解锁材质
            // 否则使用未解锁材质
            if (_m_sceneMono != null)
            {
                Material lineMaterial = null;
                if (_fromMonster == null || _fromMonster.hp <= 0)
                {
                    // 解锁状态：起点或怪物血量为0
                    lineMaterial = _m_sceneMono.unlockedMaterial;
                }
                else
                {
                    // 未解锁状态：怪物血量>0
                    lineMaterial = _m_sceneMono.lockedMaterial;
                }
                
                if (lineMaterial != null)
                {
                    line.setLineMaterial(lineMaterial);
                }
            }
            
            if(_m_lineList == null)
                _m_lineList = new List<GuildDungeonMapLine>();
            _m_lineList.Add(line);
        }

        public void clearAllLines()
        {
            if (_m_lineList != null)
            {
                foreach (var line in _m_lineList)
                {
                    line?.discard();
                }
                _m_lineList.Clear();
            }
        }

        public void updateLines()
        {
            if (_m_lineList == null) return;
            foreach (var line in _m_lineList)
            {
                if(line == null )
                    continue;
                if (_m_sceneMono != null)
                {
                    Vector2 start = _m_sceneMono.curveStrength * new Vector2(_m_sceneMono.bezierStartX, _m_sceneMono.bezierStartY);
                    Vector2 end   = _m_sceneMono.curveStrength * new Vector2(_m_sceneMono.bezierEndX, _m_sceneMono.bezierEndY);
                    line.setCurveParams(_m_sceneMono.curvePoints,  _m_sceneMono.lineWidth, _m_sceneMono.lineStyle, 
                        _m_sceneMono.allowInvertPointOrder, start, end, _m_sceneMono.cornerRadius);
                }
                line.updateLine();
            }
        }
    }

}
