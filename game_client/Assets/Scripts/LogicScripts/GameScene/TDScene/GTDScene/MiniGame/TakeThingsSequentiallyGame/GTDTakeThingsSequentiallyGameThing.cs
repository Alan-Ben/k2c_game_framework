using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame.TakeThingsSequentiallyGame
{
    public class GTDTakeThingsSequentiallyGameThing : _ITakeThingsSequentiallyGameThingShow
    {
        [NotNull] private GTDMonoTakeThingsSequentiallyGameThing _m_tdMono;
        private ETakeThingsSequentiallyGameThingState _m_eThingState = ETakeThingsSequentiallyGameThingState.NONE;   
        private TakeThingsSequentiallyGameController _m_gameController;

        private long _m_lThingAnimationSerializeId;//物品动画id
        
        public GTDTakeThingsSequentiallyGameThing([NotNull]GTDMonoTakeThingsSequentiallyGameThing _mono)
        {
            _m_tdMono = _mono;
        }

        public void onInit()
        {
            if (_m_tdMono != null && _m_tdMono.clickMono != null)
            {
                _m_tdMono.clickMono.onClick += _onThingClick;
            }
        }

        public void onDiscard()
        {
            if (_m_tdMono != null && _m_tdMono.clickMono != null)
            {
                _m_tdMono.clickMono.onClick -= _onThingClick;
            }
        }
        
        #region _ITakeThingsSequentiallyGameThingShow接口

        public long thingId { get { return _m_tdMono == null ? 0 : _m_tdMono.thingId; } }
        
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
                Debug.Log($"[GTDTakeThingsSequentiallyGameThing] 设置物品:{thingId}状态:{_thingState}, 原状态:{_m_eThingState}");
            }

            _m_eThingState = _thingState;
            if (_m_tdMono != null && _m_tdMono.stateShow != null)
            {
                _m_tdMono.stateShow.setShowData(_m_eThingState);
            }
        }

        public void showThing()
        {
            ALUGUICommon.setGameObjEnable(_m_tdMono, true);
        }
        
        public void hideThing()
        {
            _m_lThingAnimationSerializeId = ALSerializeOpMgr.next();

            ALUGUICommon.setGameObjEnable(_m_tdMono, false);
        }
        
        public List<TakeThingsSequentiallyGameThingLock> thingLockList { get { return _m_tdMono == null ? null : _m_tdMono.thingLockList; } }

        #endregion
        
        /// <summary>
        /// 当物品被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onThingClick()
        {
            // 当物品不可取走时
            if (_m_eThingState == ETakeThingsSequentiallyGameThingState.NOT_TAKEABLE)
            {
                // 播放不可取走时 点击动画
                _playAnimation(_m_tdMono == null ? string.Empty : _m_tdMono.onNotTakeableClickAnimName, () =>
                {
                    _sampleAnimation(_m_tdMono == null ? string.Empty : _m_tdMono.onNotTakeableClickAnimName, 0f);
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
            if (_m_tdMono == null || _m_tdMono.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _playDone?.Invoke();
                return;
            }

            long serializeId = _m_lThingAnimationSerializeId = ALSerializeOpMgr.next();
            _m_tdMono.thingAnimation.Play(_animationName, () =>
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
            if (_m_tdMono == null || _m_tdMono.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                return;
            }
            
            _m_tdMono.thingAnimation.Sample(_animationName, _normalizeTime);
        }

        #endregion
    }
}