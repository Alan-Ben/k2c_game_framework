using System;
using ALPackage;
using DG.Tweening;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 找东西小游戏物品
    /// </summary>
    public class GGUIWndFindThingsGameThing : _ATALBasicUISubWnd<GGUIMonoFindThingsGameThing>
    {
        private bool _m_bHasFind;//是否找到
        
        private long _m_lAnimationSerializeId;//动画序列号id
        private long _m_lFlySerializeId;//飞行序列号id
        private Tween _m_FlyTween;

        public _IFindThingsGameThingInfo thingInfo { get { return wnd == null ? null : wnd.thingInfo; } }
        public bool hasFind { get { return _m_bHasFind; } }

        public event Action<GGUIWndFindThingsGameThing, Vector2> onThingClick;
        
        public GGUIWndFindThingsGameThing(GGUIMonoFindThingsGameThing _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combinePointerUp(wnd.btnClick, _onThingPointUp);
        }
        
        protected override void _onDiscard()
        {
            _discardFlyTween();
            onThingClick = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombinePointerUp(wnd.btnClick, _onThingPointUp);
            }
        }
        
        protected override void _onShowWnd()
        {
            _m_bHasFind = false;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.notFindShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.findShowGoList, false);
            }
        }

        protected override void _onHideWnd()
        {
            _m_lAnimationSerializeId = ALSerializeOpMgr.next();
            _m_lFlySerializeId = ALSerializeOpMgr.next();
            _discardFlyTween();
        }

        protected override void _onReset()
        {
            _discardFlyTween();
        }

        /// <summary>
        /// 设置该物品被找到
        /// </summary>
        /// <param name="_flyTarget">物品飞行目标</param>
        /// <param name="_onFlyDone">物品飞行完成回调</param>
        /// <param name="_animationShowDone">物品找到动画表现完成回调</param>
        /// <param name="_allDone">飞行和动画都完成回调</param>
        public void setThingFind(Transform _flyTarget, Action _onFlyDone, Action _animationShowDone = null, Action _allDone = null)
        {
            _m_bHasFind = true;
            if (wnd == null)
            {
                _onFlyDone?.Invoke();
                _animationShowDone?.Invoke();
                _allDone?.Invoke();
                return;
            }
            
            ALUGUICommon.setGameObjEnable(wnd.notFindShowGoList, false);
            ALUGUICommon.setGameObjEnable(wnd.findShowGoList, true);

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if(!isShow)
                    return;
                
                _allDone?.Invoke();
            });
            
            // 播放找到动画
            _playAnimation(wnd.findAnimName, ()=>
            {
                _animationShowDone?.Invoke();
                stepCounter.addDoneStepCount();
            });
            
            // 进行飞行
            _thingFly(_flyTarget, ()=>
            {
                _onFlyDone?.Invoke();
                stepCounter.addDoneStepCount();
            });
        }
        
        private void _thingFly(Transform _target, Action _flyDone)
        {
            if (wnd == null || _target == null)
            {
                _flyDone?.Invoke();
                return;
            }
            
            long _serializeId = _m_lFlySerializeId = ALSerializeOpMgr.next();
            Action itemFly = () =>
            {
                if (wnd == null || _m_lFlySerializeId != _serializeId)
                    return;

                if (_target == null || rectTransform == null)
                {
                    _flyDone?.Invoke();
                    return;
                }

                _discardFlyTween();
                _m_FlyTween = rectTransform
                    .DOMove(_target.position, wnd.flyTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (_m_lFlySerializeId != _serializeId)
                            return;

                        _flyDone?.Invoke();
                    });
            };

            if (wnd.startFlyDelay <= 0)
            {
                itemFly();
            }
            else
            {
                CommonTaskController.CommonActionAddMonoTask(itemFly, wnd.startFlyDelay);   
            }
        }
        
        private void _discardFlyTween()
        {
            if(_m_FlyTween != null)
                _m_FlyTween.Kill();
            _m_FlyTween = null;
        }
        
        private void _playAnimation(string _animationName, Action _onPlayDone)
        {
            if (wnd == null)
                return;

            long _serializeId = _m_lAnimationSerializeId = ALSerializeOpMgr.next();
            
            if (wnd.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            wnd.thingAnimation.Play(_animationName, () =>
            {
                if (_m_lAnimationSerializeId != _serializeId)
                    return;

                _onPlayDone?.Invoke();
            });
        }

        private void _sampleAnimation(string _animationName, float _normalizedTime = 0.0f)
        {
            if (wnd == null || wnd.thingAnimation == null || string.IsNullOrEmpty(_animationName))
            {
                return;
            }
            
            wnd.thingAnimation.Sample(_animationName, _normalizedTime);
        }

        /// <summary>
        /// 当物体被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onThingPointUp(Vector2 _vector2)
        {
            if(_m_bHasFind)//若已经被找到, 不进行响应
                return;
            
            onThingClick?.Invoke(this, _vector2);
        }
    }
}