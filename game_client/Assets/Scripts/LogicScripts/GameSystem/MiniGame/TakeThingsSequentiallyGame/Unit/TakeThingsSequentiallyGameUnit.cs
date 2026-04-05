using System;
using ALPackage;
using GOE.MiniGame.TakeThingsSequentiallyGame;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameUnit : _ATakeThingsSequentiallyGameUnit
    {
        [NotNull] private GGUIWndTakeThingsSequentiallyGame _m_gameMainWnd = GGUIWndTakeThingsSequentiallyGame.instance;
        [NotNull] private MainAdditionTakeThingsSequentiallyGameTDScene _m_gameTdScene = MainAdditionTakeThingsSequentiallyGameTDScene.instance;

        private GTDTakeThingsSequentiallyGamePrefab _m_gameTdPrefab;

        private long _m_lSerializeId;

        public TakeThingsSequentiallyGameUnit([NotNull] TakeThingsSequentiallyGameLogic _gameLogic, [NotNull] TakeThingsSequentiallyGameController _gameController) : base(_gameLogic, _gameController)
        {
        }
        

        public override void init()
        {
            _m_gameMainWnd.showWnd();
            _m_gameMainWnd.gamePrefab?.showWnd();
            
            _m_gameTdPrefab?.show();
        }

        public override void discard()
        {
            _m_lSerializeId = ALSerializeOpMgr.next();
            
            _m_gameMainWnd.gamePrefab?.hideWnd();
            _m_gameMainWnd.hideWnd();
            
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
                
                if(_m_gameLogic.takeThingsSequentiallyGameRefObj == null)
                {
                    stepCounter.addDoneStepCount();
                    return;
                }
                
                _m_gameMainWnd.loadGamePrefab(_m_gameLogic.takeThingsSequentiallyGameRefObj.ui_prefab_path, (_wnd) =>
                {
                    if (serializeId != _m_lSerializeId)
                    {
                        _wnd?.discard();
                        return;
                    }

                    if(_wnd != null)
                        hasPrefabWndOrTdLoad = true;
                   
                    stepCounter.addDoneStepCount();
                });
            });

            if (_m_gameLogic.takeThingsSequentiallyGameRefObj != null)
            {
                NPCommonAssetPathInfo tdPrefabPath = _m_gameLogic.takeThingsSequentiallyGameRefObj.td_prefab_path;
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
            _m_gameMainWnd.discardGamePrefab();
            _m_gameMainWnd.discard();

            if (_m_gameLogic.takeThingsSequentiallyGameRefObj != null)
            {
                _m_gameTdScene.pushBackGamePrefab(_m_gameLogic.takeThingsSequentiallyGameRefObj.td_prefab_path, _m_gameTdPrefab);
            }
            else if(_m_gameTdPrefab != null)
            {
                _m_gameTdPrefab?.onDiscard();
                GameObject.Destroy(_m_gameTdPrefab.go);
            }

            _m_gameTdPrefab = null;
        }

        public void dealAllThingShow(Action<_ITakeThingsSequentiallyGameThingShow> _action)
        {
            if(_action == null)
                return;
            
            _m_gameMainWnd.gamePrefab?.dealAllThingWnd(_action);
            _m_gameTdPrefab?.dealAllThingTd(_action);
        }
        
        
        public void setGameState(ETakeThingsSequentiallyGameState _gameState)
        {
            _m_gameMainWnd.gamePrefab?.setGameState(_gameState);
            _m_gameTdPrefab?.setGameState(_gameState);
        }
    }
}