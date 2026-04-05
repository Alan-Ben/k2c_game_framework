using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    //摇杆按下松开的时候自定义动画播放
    public class NPGGUICustomMonoStickPressPlayAni : MonoBehaviour
    {
        [ALHeader("延迟播放动画时间")]
        public float delayAniTime = 2f;
        [ALHeader("动画animation")]
        public Animation animation;
        [ALHeader("摇杆按下的时候播放的动画")]
        public string pressDownAniName;
        [ALHeader("摇杆松开的时候播放的动画")]
        public string pressUpAniName;

    #if NP_GAME

        private bool _m_pressDown = false; //是否按下，
        private float _m_pressStickValueTime = 0;//按下开始移动的时间点
        private int _m_serialize = 0;//计时的序列号
        private bool _m_isInPressDown = false;//是否有播放了按下的动画

        private void OnEnable()
        {
            WinMsg.RegisterMsg(WinMsgType.STICK_PRESS_VALUE, _onStickPressValue);
        }

        private void OnDisable()
        {
            _m_serialize = ALSerializeOpMgr.next();
            WinMsg.UnregisterMsg(WinMsgType.STICK_PRESS_VALUE, _onStickPressValue);
        }
        
        private void _onStickPressValue(object[] __objs)
        {
            if (__objs.Length == 2)
            {
                if (__objs[0] is bool _pressDown && __objs[1] is float _stickValue)
                {
                    _m_pressDown = _pressDown;
                    
                    if (_pressDown)
                    {
                        if (_stickValue > 0 && _m_pressStickValueTime == 0) //还没有移动，记录开始移动
                        {
                            //只要开始开始移动，开始计时，除非摇杆抬起，不然都算是持续移动中
                            _m_pressStickValueTime = Time.realtimeSinceStartup;
                            _m_serialize = ALSerializeOpMgr.next();
                            int serialize = _m_serialize;
                            ALCommonTaskController.CommonActionAddMonoTask(() =>
                            {
                                if (serialize != _m_serialize)
                                    return;
                                _dealPressDown();
                            }, delayAniTime);
                        }
                    }
                    else
                    {
                        _dealPressUp();
                    }
                }
            }
        }
        
        private void _dealPressDown()
        {
            if (_m_isInPressDown) //没有已经播放按下的动画，不用重复播放
                return;
            if (null != animation)
            {
                animation.Play(pressDownAniName);
            }
            _m_isInPressDown = true;
        }

        private void _dealPressUp()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _m_pressStickValueTime = 0;
            if (!_m_isInPressDown) //没有播放按下的动画，不用播放抬起的动画
                return;
            if (null != animation)
            {
                animation.Play(pressUpAniName);
            }
            _m_isInPressDown = false;
        }

#endif
    }
}