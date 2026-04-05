using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGameUnit : _AQteClickOpportunityGameUnit
    {
        [NotNull] private GGUIWndQteClickOpportunityGame _m_gameMainWnd = GGUIWndQteClickOpportunityGame.instance;

        public QteClickOpportunityGameUnit([NotNull] QteClickOpportunityGameLogic _gameLogic, [NotNull] QteClickOpportunityGameController _gameController) : base(_gameLogic, _gameController)
        {
        }

        private GGUIWndQteClickOpportunityGamePrefab gamePrefabWnd { get { return _m_gameMainWnd.gamePrefab; } }

        public override void init()
        {
            _m_gameMainWnd.showWnd();
            gamePrefabWnd?.showWnd();
        }

        public override void discard()
        {
            gamePrefabWnd?.hideWnd();
            _m_gameMainWnd.hideWnd();
        }
        
        /// <summary>
        /// 预加载资源
        /// </summary>
        public void preLoadAsset(Action _complete, Action _fail)
        {
            discradPreLoadAsset();
            
            long serializeId = _m_gameLogic.gameSerializeId;
            bool hasPrefabWndOrTdLoad = false;//是否有窗口或者场景prefab加载成功

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serializeId != _m_gameLogic.gameSerializeId)
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
                if (serializeId != _m_gameLogic.gameSerializeId)
                {
                    _m_gameMainWnd.discard();
                    return;
                }
                
                if(_m_gameLogic.qteClickOpportunityGameRefObj == null)
                {
                    stepCounter.addDoneStepCount();
                    return;
                }
                
                _m_gameMainWnd.loadGamePrefab(_m_gameLogic.qteClickOpportunityGameRefObj.ui_prefab_path, (_wnd) =>
                {
                    if (serializeId != _m_gameLogic.gameSerializeId)
                    {
                        _wnd?.discard();
                        return;
                    }

                    if(_wnd != null)
                        hasPrefabWndOrTdLoad = true;
                   
                    stepCounter.addDoneStepCount();
                });
            });
        }

        /// <summary>
        /// 销毁预加载的窗口
        /// </summary>
        public void discradPreLoadAsset()
        {
            _m_gameMainWnd.discardGamePrefab();
            _m_gameMainWnd.discard();
        }

        public void dealAllItemShow(Action<_IQteClickOpportunityGameItemShow> _action)
        {
            if(_action == null)
                return;
            
            gamePrefabWnd?.dealAllItemWnd(_action);
        }
        
        
        public void setGameState(EQteClickOpportunityGameState _gameState)
        {
            gamePrefabWnd?.setGameState(_gameState);
        }
    }
}