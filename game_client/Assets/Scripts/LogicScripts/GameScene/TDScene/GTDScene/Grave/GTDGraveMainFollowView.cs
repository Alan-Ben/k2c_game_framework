using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GTDGraveMainFollowView
    {
        [NotNull] private GTDGraveMainFollowMono _m_mono;
        
        private GGraveMainFollowInstance _m_followInstance;
        private GGUIGraveMainFollowItemController _m_followItemController;
        private GGravePlayerView _m_actorView;

        private bool _m_bIsInit;
        private bool _m_bInitDone;
        private Action _m_aOnInitDone;
        
        public GTDGraveMainFollowView([NotNull] GTDGraveMainFollowMono _mono)
        {
            _m_mono = _mono;

            _m_bIsInit = false;
            _m_bInitDone = false;
            _m_aOnInitDone = null;
        }

        public void init(long _cid, long _titleId, Action _onInitDoneAction = null)
        {
            if (_m_bInitDone)//先判断是否初始化完成, 若已经完成, 直接调用回调
            {
                _onInitDoneAction?.Invoke();
                return;
            }

            if (_m_bIsInit)//判断是否已经调用过init, 若已经调用过, 只需要注册回调
            {
                regOnInitDoneDelegate(_onInitDoneAction);
                return;
            }

            _m_bIsInit = true;
            
            if (_m_mono.uiFollowParent == null || _m_mono.uiFollowResPathId < 0)
            {
                _onInitDone();
            }
            else
            {
                GGUIWndGraveMainFollowRoot.instance.regInstance(_m_followInstance);
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(2);
                stepCounter.regAllDoneDelegate(_onInitDone);
                
                _m_followInstance = new GGraveMainFollowInstance(_m_mono.uiFollowParent);
                GGUIWndGraveMainFollowRoot.instance.regInstance(_m_followInstance);
                
                _m_followItemController = new GGUIGraveMainFollowItemController(_m_mono.graveTypeId, _cid, _titleId, _m_mono.uiFollowResPathId);
                _m_followInstance.addController(_m_followItemController);
                
                _m_followItemController.regItemWndLoadDoneDelegate(stepCounter.addDoneStepCount);
                
                if (_m_mono.needLoadPlayerGo && _cid > 0 && _m_mono.playerGoParent != null)
                {
                    GCommon.reqPlayerInfo(_cid, (_info) =>
                    {
                        if (_info == null )
                        {
                            stepCounter.addDoneStepCount();
                            return;
                        }

                        if (!_m_bIsInit) // 如果已经被销毁
                        {
                            stepCounter.addDoneStepCount();
                            return;
                        }
                        
                        _m_actorView = new GGravePlayerView(_info.skinRef?.td_show, _m_mono.playerGoParent);
                        _m_actorView.load(stepCounter.addDoneStepCount);
                    });
                }
            }
        }
        
        public void discard()
        {
            _m_aOnInitDone = null;
            _m_bIsInit = false;
            _m_bInitDone = false;
            
            _m_followItemController?.discard();
            _m_followItemController = null;

            if (_m_followInstance != null)
            {
                GGUIWndGraveMainFollowRoot.instance.removeInstance(_m_followInstance);
                _m_followInstance.discard();
                _m_followInstance = null;
            }

            if (_m_actorView != null)
            {
                _m_actorView.discard();
                _m_actorView = null;
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

        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_mono, true);
            
            _m_followItemController?.dealItemWnd((_wnd) =>
            {
                _wnd?.showWnd();
            });
        }

        public void hide()
        {
            ALUGUICommon.setGameObjEnable(_m_mono, false);
            
            _m_followItemController?.dealItemWnd((_wnd) =>
            {
                _wnd?.hideWnd();
            });
        }
    }
}