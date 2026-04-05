using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class DragBoxGameUnit : _ADragBoxGameUnit
    {
        [NotNull] private GGUIWndDragBoxGame _m_gameMainWnd = GGUIWndDragBoxGame.instance;
        [NotNull] private MainAdditionDragBoxGameTDScene _m_gameTdScene = MainAdditionDragBoxGameTDScene.instance;

        private GTDDragBoxGamePrefab _m_gameTdPrefab;
        
        private long _m_lSerializeId;
        
        public DragBoxGameUnit([NotNull] DragBoxGameLogic _gameLogic, [NotNull] DragBoxGameController _gameController) : base(_gameLogic, _gameController)
        {
        }
        
        public GTDMonoDragBoxGamePlayerCuteActor playerCuteActorMono { get { return _m_gameTdPrefab?.playerCuteActorMono; } }

        public override void init()
        {
            _m_gameMainWnd.showWnd();
            _m_gameTdScene.setGameLogic(_m_gameLogic);
            
            _m_gameTdPrefab?.show();
        }

        public override void discard()
        {
            _m_lSerializeId = ALSerializeOpMgr.next();
            
            _m_gameMainWnd.hideWnd();
            _m_gameTdScene.setGameLogic(null);
            
            _m_gameTdPrefab?.hide();
        }
        
        /// <summary>
        /// 预加载资源
        /// </summary>
        public void preLoadAsset(Action _complete, Action _fail)
        {
            discradPreLoadAsset();
            
            long serializeId = _m_lSerializeId = ALSerializeOpMgr.next();
            bool hasPrefabWndOrTdLoad = false;//是否有窗口或者场景prefab加载成功

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serializeId != _m_lSerializeId)
                {
                    discradPreLoadAsset();
                    return;
                }
                
                if (hasPrefabWndOrTdLoad)
                    _complete?.Invoke();
                else
                    _fail?.Invoke();
            });
            
            _m_gameMainWnd.load(() =>
            {
                if (serializeId != _m_lSerializeId)
                {
                    _m_gameMainWnd.discard();
                    return;
                }
                
                stepCounter.addDoneStepCount();
            });

            if (_m_gameLogic.dragBoxGameRefObj != null)
            {
                NPCommonAssetPathInfo tdPrefabPath = _m_gameLogic.dragBoxGameRefObj.td_prefab_path;
                _m_gameTdScene.popGamePrefab(tdPrefabPath,
                    (_gameTdPrefab) =>
                    {
                        if (serializeId != _m_lSerializeId)
                        {
                            _m_gameTdScene.pushBackGamePrefab(tdPrefabPath, _gameTdPrefab);
                            return;
                        }
                        
                        if(_gameTdPrefab != null)
                            hasPrefabWndOrTdLoad = true;

                        _m_gameTdPrefab = _gameTdPrefab;
                        stepCounter.addDoneStepCount();
                    });
            }
            else
            {
                stepCounter.addDoneStepCount();
            }
        }

        /// <summary>
        /// 销毁预加载的窗口
        /// </summary>
        public void discradPreLoadAsset()
        {
            _m_gameMainWnd.discard();

            if (_m_gameLogic.dragBoxGameRefObj != null)
            {
                _m_gameTdScene.pushBackGamePrefab(_m_gameLogic.dragBoxGameRefObj.td_prefab_path, _m_gameTdPrefab);
            }
            else if(_m_gameTdPrefab != null)
            {
                _m_gameTdPrefab?.onDiscard();
                GameObject.Destroy(_m_gameTdPrefab.go);
            }

            _m_gameTdPrefab = null;
        }

        public void dealAllBoxShow(Action<_IDragBoxGameBoxShow> _action)
        {
            if(_action == null)
                return;
            
            _m_gameTdPrefab?.dealAllBox(_action);
        }
        
        
    }
}