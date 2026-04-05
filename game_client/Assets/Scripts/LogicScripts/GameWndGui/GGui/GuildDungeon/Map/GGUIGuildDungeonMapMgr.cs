using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    public class GGUIGuildDungeonMapMgr
    {
        // private Rect _m_dragRect;
        private GGUIGuildDungeonMapMono _m_configMono;
        private GuildDungeonInfo _m_guildDungeonInfo;

        private List<GGUIWndGuildDungeonMapFollowItem> _m_lViewList;
        private GGUIGuildDungeonMapLineMgr _m_lineMgr;

        private bool _m_bIsInit;
        private bool _m_bInitDone;
        private Action _m_aOnInitDone;

        // public Rect dragRect => _m_dragRect;

        public GGUIGuildDungeonMapMgr()
        {
            _m_bIsInit = false;
            _m_lineMgr = new GGUIGuildDungeonMapLineMgr();
        }

        public void init([NotNull] GGUIGuildDungeonMapMono _mono, Action _onInitDoneAction = null)
        {
            _m_configMono = _mono;
         
            if (_m_bInitDone)//先判断是否初始化完成, 若已经完成, 直接调用回调
            {
                _onInitDoneAction?.Invoke();
                return;
            }

            regOnInitDoneDelegate(_onInitDoneAction);

            if (_m_bIsInit)//判断是否已经调用过init, 若已经调用过, 只需要注册回调
                return;

            _m_bIsInit = true;

            if (_m_configMono != null && _m_configMono.lineConfig != null)
            {
                _m_lineMgr?.init(_m_configMono.lineConfig);
            }


            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_onInitDone);
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 更新曲线参数（运行时调用）
        /// </summary>
        public void updateCurve()
        {
            if (_m_configMono != null && _m_lineMgr != null)
            {
                _m_lineMgr.updateLines(); // 重新绘制所有连线
            }
        }


        public void discard()
        {
            if (!_m_bIsInit)
                return;

            _m_bIsInit = false;
            _m_bInitDone = false;
            
            _m_configMono = null;
            
            _clearViewList();
            
            _m_lViewList = null;
            
            _m_lineMgr?.discard();
        }

        public void refreshMap(GuildDungeonInfo _guildDungeonInfo, Action _onSetDone = null)
        {
            _m_guildDungeonInfo = _guildDungeonInfo;
            // _m_dragRect = _dragRect;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                _onSetDone?.Invoke();
            });
        
            _clearViewList();
            
            if (_m_lViewList == null)
                _m_lViewList = new List<GGUIWndGuildDungeonMapFollowItem>();

            if (_m_configMono != null && _m_guildDungeonInfo != null )
            {
              
                float xCount = _m_guildDungeonInfo.xMaxLayerCount;
                float yCount = _m_guildDungeonInfo.yMaxLayerCount;
                yCount += 1; //加上起点
        
                float width = (xCount -1) * _m_configMono.width + _m_configMono.xBorder * 2;
                float height = (yCount -1) * _m_configMono.height + _m_configMono.yBorder * 2;
        
                var dragRect = new Rect(-width/2, -height/2, width, height);

                Vector2 delta = _m_configMono.instanceParent.sizeDelta;
                delta.y = dragRect.height;
                _m_configMono.instanceParent.sizeDelta = delta;
                if (_m_configMono.mapScrollRect != null) 
                    _m_configMono.mapScrollRect.verticalNormalizedPosition = 0f;
                List<GuildDungeonMonster> monsters = _m_guildDungeonInfo.monsterList;
                List<GuildDungeonMapLinePoint> linePoints = new List<GuildDungeonMapLinePoint>();
                if (monsters != null)
                {
                    foreach (GuildDungeonMonster monster in monsters)
                    {
                        if(monster == null)
                            continue;
                        
                        // 根据totalInLayer计算居中位置
                        float centerPosition = (_m_guildDungeonInfo.xMaxLayerCount - 1) * 0.5f;
                        float layerCenterOffset = (monster.totalInLayer - 1) * 0.5f;
                        float xPosition = centerPosition - layerCenterOffset + monster.positionInLayer;
                        
                        Vector3 wndPos = _m_configMono.mapOffsetPos + new Vector3(dragRect.x +_m_configMono.xBorder + _m_configMono.width * xPosition, dragRect.y + _m_configMono.yBorder + _m_configMono.height * (monster.layer + 1), 0);
                        stepCounter.chgTotalStepCount(1);
                        var worldPos = _m_configMono.instanceParent.TransformPoint(wndPos);

                        GGUIWndGuildDungeonMapFollowItem view = new GGUIWndGuildDungeonMapFollowItem(monster, worldPos, _m_configMono.instanceParent);
                        view.load();
                        view.showWnd(stepCounter.addDoneStepCount);
                        _m_lViewList.Add(view);
                        linePoints.Add(new GuildDungeonMapLinePoint(wndPos, monster));
                    }
                }
                // 在所有View创建完成后，创建连线
                if (linePoints.Count > 0)
                {
                    Vector3 startPointPos = _m_configMono.mapOffsetPos + new Vector3(dragRect.center.x, dragRect.y + _m_configMono.yBorder, 0);
                    if (_m_configMono.startPointGo != null)
                    {
                        Vector3 worldPos = _m_configMono.instanceParent.TransformPoint(startPointPos);
                        _m_configMono.startPointGo.position = worldPos;
                    }
                    _m_lineMgr?.createLines(linePoints, _m_guildDungeonInfo, startPointPos);
                }
            }
            stepCounter.addDoneStepCount();
            
        
        }
        
        private void _clearViewList()
        {
            if (_m_lViewList != null)
            {
                foreach (GGUIWndGuildDungeonMapFollowItem view in _m_lViewList)
                {
                    view?.hideWnd();
                    view?.discard();
                }
                _m_lViewList.Clear();
            }
        }
        
        public void regOnInitDoneDelegate(Action _action)
        {
            if(_action == null)
                return;

            if (_m_bInitDone)
            {
                _action();
                return;                
            }

            _m_aOnInitDone += _action;
        }

        private void _onInitDone()
        {
            if(!_m_bIsInit)//初始化完成后, 先判断_m_bIsInit是否为false, 为false则表示还未调用过init方法 或 已经调用了discard方法进行了销毁, 这两种情况都不需要后续操作
                return;

            _m_bInitDone = true;
            
            Action action = _m_aOnInitDone;
            _m_aOnInitDone = null;
            
            action?.Invoke();
        }
    }
}