using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class AnecdoteViewMgr
    {
        [ItemNotNull, NotNull] private readonly List<AnecdotePosInfo> _m_allPosInfo;
        [ItemNotNull, NotNull] private readonly List<AnecdotePosView> _m_allPosView;
        // 是否初始化和当前的序列号
        private bool _m_isInit;
        private int _m_initSerialize;
        
        
        public AnecdoteViewMgr()
        {
            _m_allPosInfo = new List<AnecdotePosInfo>();
            _m_allPosView = new List<AnecdotePosView>();
        }


        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            NPPlayer.instance.anecdoteComp.getPosListNonAlloc(_m_allPosInfo);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_allPosInfo.Count + 1);
            stepCounter.regAllDoneDelegate(_complete);
            foreach (AnecdotePosInfo posInfo in _m_allPosInfo)
            {
                AnecdotePosView posView = new AnecdotePosView(posInfo);
                _m_allPosView.Add(posView);
                posView.load(stepCounter.addDoneStepCount);
            }
            
            NPPlayer.instance.anecdoteComp.onPosInfoAdd += _onPosInfoAdd;
            NPPlayer.instance.anecdoteComp.onPosInfoRemove += _onPosInfoRemove;
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_ANECDOTE_POS, _onSimulateClickAnecdotePos);
            WinMsg.RegisterMsgAct(WinMsgType.RFRESH_ANECDOTE_VIEW, refresh);

            stepCounter.addDoneStepCount();
        }
        public void refresh()
        {
            if (!_m_isInit)
                return;

            foreach (AnecdotePosView posView in _m_allPosView)
            {
                posView._onEventListChange();
            }
        }
        public void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;
            
            WinMsg.UnregisterMsgAct(WinMsgType.RFRESH_ANECDOTE_VIEW, refresh);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_ANECDOTE_POS, _onSimulateClickAnecdotePos);
            NPPlayer.instance.anecdoteComp.onPosInfoAdd -= _onPosInfoAdd;
            NPPlayer.instance.anecdoteComp.onPosInfoRemove -= _onPosInfoRemove;

            foreach (AnecdotePosView posView in _m_allPosView)
            {
                posView.discard();
            }
            
            _m_allPosView.Clear();
            _m_allPosInfo.Clear();
        }
        
        
        private void _onPosInfoAdd(AnecdotePosInfo _info)
        {
            if (_info == null)
                return;
            
            AnecdotePosView posView = new AnecdotePosView(_info);
            _m_allPosView.Add(posView);
            posView.load();
        }
        private void _onPosInfoRemove(AnecdotePosInfo _info)
        {
            if (_info == null)
                return;
            
            AnecdotePosView posView = _m_allPosView.FindAndRemove(_view => _view.posInfo == _info);
            posView?.discard();
        }
        private void _onSimulateClickAnecdotePos(object[] _params)
        {
            if (_params == null || _params.Length < 1)
                return;
            
            long posId = (long)_params[0];
            AnecdotePosView posView = _m_allPosView.Find(_view => _view.posInfo.posId == posId);
            if (posView == null)
                return;
            
            posView._simulateClick();
        }
    }
}