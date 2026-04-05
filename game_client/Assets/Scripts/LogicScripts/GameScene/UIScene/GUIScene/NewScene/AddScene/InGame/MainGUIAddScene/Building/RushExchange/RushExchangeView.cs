
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class RushExchangeView 
    {
        private GTDMonoRushExchange _m_mono;
        private GGUICommonFollowTarget _m_nameFollowTarget;
        private GGUIWndRushExchangeFollowerController _m_nameFollower;
        
        public void init(Action _complete)
        {
            _m_mono = MainAdditionBuildingTDScene.instance.getRushExchangeMono();
            if (_m_mono == null)
            {
                _complete?.Invoke();
                return;
            }

            if (_m_mono.hudTarget != null)
            {
                _m_nameFollowTarget = new GGUICommonFollowTarget(_m_mono.hudTarget, Vector3.zero);
                GGUIWndBuildingFollow.instance.regInstance(_m_nameFollowTarget);
            }

            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick += _onBuildingClick;

            if (_m_nameFollowTarget != null)
            {
                _m_nameFollower = new GGUIWndRushExchangeFollowerController();
                _m_nameFollowTarget.addController(_m_nameFollower);
            }
            _complete?.Invoke();
        }
        
        public void discard()
        {
            if (_m_mono == null)
                return;
            
            _m_nameFollowTarget?.discard();
            _m_nameFollowTarget = null;
            _m_nameFollower = null;
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onBuildingClick;

            _m_mono = null;
        }
        
        
        private void _onBuildingClick()
        {
            if(NPPlayer.instance.rushExchangeComp.rushExchangeState == ERushExchangeState.Lock)
                return;
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndRushExchangeMain.instance, GGUIWndRushExchangeMain.instance.showWnd, UINodeTagConst.C_RUSH_EXCHANGE_MAIN);
        }
    }
}