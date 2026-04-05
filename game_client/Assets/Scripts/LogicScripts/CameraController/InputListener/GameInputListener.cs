using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GameInputListener : _AInput
    {
        private static GameInputListener _g_instance = new GameInputListener();
        public static GameInputListener instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GameInputListener();
                return _g_instance;
            }
        }

        //默认输入处理对象
        private _IGameInputDealer _m_idDefaultInputDealer = null;
        //附加输入处理对象，部分操作会优先处理附加对象的操作，如点击或者拖拽过程
        private _ACityAddInputDealer _m_idAdditionInputDealer = null;

        //每帧调用
        public override void Update()
        {
            _m_idDefaultInputDealer?.onUpdate();
            _m_idAdditionInputDealer?.onUpdate();
        }

        public override void OnPress(GameObject _go, bool _press, TouchInfo _touchInfo)
        {
            //增加操作序号
            if(_press)
            {
                //处理按下开始
                if(null != _m_idAdditionInputDealer)
                {
                    if(_m_idAdditionInputDealer.onPress(_touchInfo))
                        return;
                }

                if(null != _m_idDefaultInputDealer)
                    _m_idDefaultInputDealer.onPress(_touchInfo);
            }
            else
            {
                //处理弹起操作
                if(null != _m_idAdditionInputDealer)
                {
                    if(_m_idAdditionInputDealer.onUnPress(_touchInfo))
                        return;
                }

                if(null != _m_idDefaultInputDealer)
                    _m_idDefaultInputDealer.onUnPress(_touchInfo);
            }
        }

        //开始拖拽时的操作
        public override void OnDragStart(GameObject _go, TouchInfo _touchInfo, float _pressToDragTime)
        {
            //处理拖拽开始
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnDragStart(_go, _touchInfo, _pressToDragTime))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnDragStart(_go, _touchInfo, _pressToDragTime);
        }

        public override void OnDrag(GameObject _go, Vector2 _delta, TouchInfo _touchInfo)
        {
            //两个触摸点,则只做缩放
            if(InputListener.instance.touchCount >= 2)
                return;

            //处理拖拽过程操作
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnDrag(_go, _delta, _touchInfo))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnDrag(_go, _delta, _touchInfo);
        }

        public override void OnDragEnd(GameObject _go, TouchInfo _touchInfo)
        {
            //处理拖拽结束
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnDragEnd(_go, _touchInfo))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnDragEnd(_go, _touchInfo);
        }

        public override void OnClick(GameObject _go, TouchInfo _touchInfo)
        {
            if(InputListener.instance.touchCount >= 2)
            {
                return;
            }

            //处理点击操作
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnClick(_go, _touchInfo))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnClick(_go, _touchInfo);
        }

        public override void OnHold(GameObject _go, TouchInfo _touchInfo, float _holdingTime)
        {
            //处理点击操作
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnHold(_go, _touchInfo, _holdingTime))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnHold(_go, _touchInfo, _holdingTime);
        }

        public override void OnScaleChg(float _changeValue)
        {
            //处理缩放操作
            if(null != _m_idAdditionInputDealer)
            {
                if(_m_idAdditionInputDealer.OnScaleChg(-_changeValue))
                    return;
            }

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnScaleChg(_changeValue);
        }

        //进入时的操作处理
        public override void OnEnter()
        {
        }
        //退出操作时的处理
        public override void OnExit()
        {
            //设置默认输入对象
            setDefaultInputDealer(null);
            //设置输入对象
            setAdditionInputDealer(null);
        }

        /// <summary>
        /// 退出当前的默认处理对象
        /// </summary>
        /// <param name="_dealer"></param>
        public void quitDefaultInputDealer(_IGameInputDealer _dealer)
        {
            if (_dealer != _m_idDefaultInputDealer)
                return;

            if (null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnExit();

            _m_idDefaultInputDealer = null;
        }

        /**************
         * 设置输入处理对象
         **/
        public void setDefaultInputDealer(_IGameInputDealer _dealer)
        {
            if(_dealer == _m_idDefaultInputDealer)
                return;

            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnExit();

            _m_idDefaultInputDealer = _dealer;

            //设置输入对象
            if(null != _m_idDefaultInputDealer)
                _m_idDefaultInputDealer.OnEnter();
        }

        /**************
         * 设置输入处理对象
         **/
        public void setAdditionInputDealer(_ACityAddInputDealer _dealer)
        {
            if(_dealer == _m_idAdditionInputDealer)
                return;

            if(null != _m_idAdditionInputDealer)
                _m_idAdditionInputDealer.OnExit();

            _m_idAdditionInputDealer = _dealer;

            //设置输入对象
            if(null != _m_idAdditionInputDealer)
                _m_idAdditionInputDealer.OnEnter();
        }

        /**************
         * 退出附加处理对象
         **/
        public void quitAdditionInputDealer(_ACityAddInputDealer _dealer)
        {
            if(_dealer != _m_idAdditionInputDealer)
                return;

            if(null != _m_idAdditionInputDealer)
                _m_idAdditionInputDealer.OnExit();

            _m_idAdditionInputDealer = null;
        }
    }
}
