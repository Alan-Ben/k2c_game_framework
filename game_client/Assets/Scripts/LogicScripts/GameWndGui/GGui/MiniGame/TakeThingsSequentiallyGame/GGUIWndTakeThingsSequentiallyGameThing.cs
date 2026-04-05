using System;
using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTakeThingsSequentiallyGameThing : _ANPGGUIBasicSubWnd<GGUIMonoTakeThingsSequentiallyGameThing>, _ITakeThingsSequentiallyGameThingShow
    {
        private ETakeThingsSequentiallyGameThingState _m_eThingState = ETakeThingsSequentiallyGameThingState.NONE;   
        private TakeThingsSequentiallyGameController _m_gameController;
        
        private long _m_lThingAnimationSerializeId;//物品动画id
        
        public GGUIWndTakeThingsSequentiallyGameThing(GGUIMonoTakeThingsSequentiallyGameThing _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onThingClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onThingClick);
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lThingAnimationSerializeId = ALSerializeOpMgr.next();
        }
        
        protected override void _onReset()
        {
        }

        #region _ITakeThingsSequentiallyGameThingShow接口

        public long thingId { get { return wnd == null ? 0 : wnd.thingId; } }
        
        public void setGameController(TakeThingsSequentiallyGameController _gameController)
        {
            _m_gameController = _gameController;
        }
        
        /// <summary>
        /// 设置物品状态
        /// </summary>
        /// <param name="_thingState"></param>
        public void setThingState(ETakeThingsSequentiallyGameThingState _thingState)
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"[GGUIWndTakeThingsSequentiallyGameThing] 设置物品:{thingId}状态:{_thingState}, 原状态:{_m_eThingState}");
            }

            _m_eThingState = _thingState;
            if (wnd != null && wnd.stateShow != null)
            {
                wnd.stateShow.setShowData(_m_eThingState);
            }
        }

        public void showThing()
        {
            showWnd();
        }
        
        public void hideThing()
        {
            hideWnd();
        }
        
        public List<TakeThingsSequentiallyGameThingLock> thingLockList { get { return wnd == null ? null : wnd.thingLockList; } }

        #endregion

        /// <summary>
        /// 当物品被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onThingClick(GameObject _go)
        {
            // 当物品不可取走时
            if (_m_eThingState == ETakeThingsSequentiallyGameThingState.NOT_TAKEABLE)
            {
                // 播放不可取走时 点击动画
                _playAnimation(wnd == null ? string.Empty : wnd.onNotTakeableClickAnimName, () =>
                {
                    _sampleAnimation(wnd == null ? string.Empty : wnd.onNotTakeableClickAnimName, 0f);
                });
            }
            else if(_m_eThingState == ETakeThingsSequentiallyGameThingState.TAKEABLE)//当物品可取走时
            {
                _m_gameController?.takeThing(thingId);
            }
        }

        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_animationName"></param>
        /// <param name="_playDone"></param>
        private void _playAnimation(string _animationName, Action _playDone)
        {
            if (wnd == null || wnd.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _playDone?.Invoke();
                return;
            }

            long serializeId = _m_lThingAnimationSerializeId = ALSerializeOpMgr.next();
            wnd.thingAnimation.Play(_animationName, () =>
            {
                if (_m_lThingAnimationSerializeId != serializeId)
                    return;

                _playDone?.Invoke();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_animationName"></param>
        private void _sampleAnimation(string _animationName, float _normalizeTime)
        {
            if (wnd == null || wnd.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                return;
            }
            
            wnd.thingAnimation.Sample(_animationName, _normalizeTime);
        }

        #endregion
    }
}