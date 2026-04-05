using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GTDTreasureHuntLabSceneMgr
    {
        [NotNull] private GTDMonoTreasureHuntLab _m_sceneMono;

        private bool _m_bIsInit;
        private bool _m_bInitDone;
        private Action _m_aOnInitDone;

        // private List<GTDTreasureHuntLabTreasureView> _m_lTreasureViewList;
        private Dictionary<TreasureHuntTreasureRefObj, GTDTreasureHuntLabTreasureView> _m_dTreasureViewDic = new Dictionary<TreasureHuntTreasureRefObj, GTDTreasureHuntLabTreasureView>();
        private GTDTreasureHuntLabTreasureView _m_nowShowTreasureView;
        
        public GTDTreasureHuntLabSceneMgr([NotNull] GTDMonoTreasureHuntLab _mono)
        {
            _m_sceneMono = _mono;
        }

        public void init(Action _onInitDoneAction = null)
        {
            if (_m_bInitDone)//先判断是否初始化完成, 若已经完成, 直接调用回调
            {
                _onInitDoneAction?.Invoke();
                return;
            }

            //注册回调
            regOnInitDoneDelegate(_onInitDoneAction);
            if (_m_bIsInit)//判断是否已经调用过init, 若已经调用过, 直接返回
                return;

            _m_bIsInit = true;

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(_onInitDone);
            
            stepCounter.addDoneStepCount();
        }
        
        public void discard()
        {
            _m_aOnInitDone = null;
            _m_bIsInit = false;
            _m_bInitDone = false;

            _m_nowShowTreasureView = null;
            if (_m_dTreasureViewDic != null)
            {
                foreach (var treasureView in _m_dTreasureViewDic.Values)
                {
                    treasureView?.discard();
                }
                _m_dTreasureViewDic.Clear();
            }
            _m_dTreasureViewDic = null;
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

        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_sceneMono.gameObject, true);
            
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_LAB_REFRESH_SELECTED_TREASURE, _onRefreshSelectedTreasure);
        }

        public void hide()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_LAB_REFRESH_SELECTED_TREASURE, _onRefreshSelectedTreasure);
            
            dealAllTreasureView((_treasureView) =>
            {
                _treasureView?.hide();
            });
            
            ALUGUICommon.setGameObjEnable(_m_sceneMono.gameObject, false);
        }

        private void _refreshNowShowTreasureView(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_nowShowTreasureView?.hide();

            if (_treasureInfo == null || _treasureInfo.treasureRefObj == null)
            {
                if (_m_sceneMono != null)
                {
                    ALUGUICommon.setGameObjEnable(_m_sceneMono.hasSelectedTreasureShow, false);
                    ALUGUICommon.setGameObjEnable(_m_sceneMono.noSelectedTreasureShow, true);
                }
                return;
            }
            
            ALUGUICommon.setGameObjEnable(_m_sceneMono.hasSelectedTreasureShow, true);
            ALUGUICommon.setGameObjEnable(_m_sceneMono.noSelectedTreasureShow, false);
            
            _m_nowShowTreasureView = _getTreasureView(_treasureInfo.treasureRefObj);
            if(_m_nowShowTreasureView == null)
                return;
            
            _m_nowShowTreasureView.regLoadDoneDelegate(() =>
            {
                _m_nowShowTreasureView.setData(_treasureInfo);
                _m_nowShowTreasureView.show();
            });
        }
        
        private GTDTreasureHuntLabTreasureView _getTreasureView(TreasureHuntTreasureRefObj _huntTreasureRefObj)
        {
            if (_huntTreasureRefObj == null)
                return null;

            if (_m_dTreasureViewDic == null)
                _m_dTreasureViewDic = new Dictionary<TreasureHuntTreasureRefObj, GTDTreasureHuntLabTreasureView>();
            
            if(!_m_dTreasureViewDic.TryGetValue(_huntTreasureRefObj, out var treasureView) || treasureView == null)
            {
                treasureView = new GTDTreasureHuntLabTreasureView(_huntTreasureRefObj.td_go_index, _m_sceneMono.treasureLoadParent);
                treasureView.load();

                _m_dTreasureViewDic[_huntTreasureRefObj] = treasureView;
            }

            return treasureView;
        }
        
        public void dealAllTreasureView(Action<GTDTreasureHuntLabTreasureView> _action)
        {
            if(_action == null || _m_dTreasureViewDic == null)
                return;
            
            foreach (var treasureView in _m_dTreasureViewDic.Values)
            {
                if(treasureView == null)
                    continue;
            
                _action(treasureView);
            }
        }
        
        private void _onRefreshSelectedTreasure(params object[] _objects)
        {
            if(_objects == null || _objects.Length < 1)
                return;

            if(_objects[0] is _ITreasureHuntTreasureInfo _treasureInfo)
                _refreshNowShowTreasureView(_treasureInfo);
            else
                _refreshNowShowTreasureView(null);
        }
    }
}