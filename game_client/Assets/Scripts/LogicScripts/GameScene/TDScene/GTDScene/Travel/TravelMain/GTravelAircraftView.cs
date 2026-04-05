using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    public class GTravelAircraftView : _AGTravelMainCommonView<GTDTravelAircraftMono>
    {
        [NotNull] private LogicPlane2DPosGetterAdaptiveWorldPos _m_AircraftPosGetter;// 经过飞机父节点、法向量为摄像机forward方向的位置获取器
        
        private Tweener _m_tweener;
        
        public GTravelAircraftView(NPGGoIndex _goIndex, Transform _parent) : base(_goIndex, _parent)
        {
            _m_AircraftPosGetter = new LogicPlane2DPosGetterAdaptiveWorldPos(
                _parent != null ? _parent.position : Vector3.zero
                , CameraController.instance.cameraForward);
        }

        protected override void _onLoadedEx(GameObject _go)
        {
            if (_m_viewTrans != null)
            {
                _m_viewTrans.localPosition = Vector3.zero;
                _m_viewTrans.localScale = Vector3.one;
                _m_viewTrans.localRotation = Quaternion.identity;
            }
            
            // 加载完成后，先隐藏，等需要显示时才显示
            hide();
        }

        protected override void _onDiscardEx()
        {
            _stopFlyAnimation();
            killTweener();
        }

        protected override void _onHideEx()
        {
            _stopFlyAnimation();
            killTweener();
        }

        protected override void _onShowEx()
        {
        }
        
        public void killTweener()
        {
            if(_m_tweener != null)
                _m_tweener.Kill();
            _m_tweener = null;
        }

        /// <summary>
        /// 获取贝塞尔曲线控制点：在过起终点中点、垂直于起终点连线、
        /// 上下各延伸起终点距离一半的线段上随机取一点
        /// </summary>
        private Vector2 _getBezierControlPoint(Vector2 _start, Vector2 _end)
        {
            Vector2 midPoint = (_start + _end) * 0.5f;
            Vector2 dir = _end - _start;
            float dist = dir.magnitude;

            // 垂直于起终点连线的方向
            Vector2 perpendicular = new Vector2(-dir.y, dir.x);
            if (perpendicular.sqrMagnitude > 0.0001f)
                perpendicular.Normalize();
            else
                perpendicular = Vector2.up;

            float halfLen = dist * 0.5f;
            float offset = Random.Range(0, 2) == 0 ? -halfLen : halfLen;// 随机选择控制点在线段的上方还是下方

            if (itemMono != null && itemMono.bezierControlPointDisRate != null)
            {
                return midPoint + perpendicular * offset * itemMono.bezierControlPointDisRate.getRandomValue();
            }
            else
            {
                return midPoint + perpendicular * offset;
            }
        }

        public void fly(Vector3 _startPos, Vector3 _endPos, Action _onFlyComplete = null)
        {
            if (itemMono == null || itemMono.transform == null || itemMono.flyTime <= 0)
            {
                _onFlyComplete?.Invoke();
                return;
            }

            killTweener();//若有正在飞行的动画，则先停止
            
            Vector2 startAircraftLogicPos = _m_AircraftPosGetter.getLogicPos(_startPos);// 获取飞机飞行起始位置在逻辑坐标系中的位置
            Vector2 endAircraftLogicPos = _m_AircraftPosGetter.getLogicPos(_endPos);// 获取飞机飞行结束位置在逻辑坐标系中的位置

            // 取起始点与终点构成矩形的非起始点与终点的顶点作为贝塞尔曲线控制点
            Vector2 controlLogicPos = _getBezierControlPoint(startAircraftLogicPos, endAircraftLogicPos);

            // 设置飞机初始位置
            Vector3 startAircraftRealPos = _m_AircraftPosGetter.getWorldPosByLogicPos(startAircraftLogicPos);
            itemMono.transform.position = startAircraftRealPos;

            // 计算初始切线方向并设置飞机朝向
            Vector2 initTangent = BezierUtils.CalculateCubicBezierTangent(0f, startAircraftLogicPos, controlLogicPos, endAircraftLogicPos);
            _applyAircraftRotation(initTangent);
            
            // 显示飞机
            show();
            
            // 播放飞行动画
            _playFlyAnimation();

            // 使用 t 值从 0 到 1 沿着二阶贝塞尔曲线飞行
            float tweenValue = 0f;
            _m_tweener = DOTween.To(() => tweenValue, _t =>
            {
                tweenValue = _t;
                if (itemMono == null || itemMono.transform == null)
                    return;

                // 根据 t 值计算贝塞尔曲线上的位置
                Vector2 curLogicPos = BezierUtils.CalculateCubicBezierPoint(_t, startAircraftLogicPos, controlLogicPos, endAircraftLogicPos);
                Vector3 curWorldPos = _m_AircraftPosGetter.getWorldPosByLogicPos(curLogicPos);
                itemMono.transform.position = curWorldPos;

                // 根据 t 值计算切线方向，用于飞机朝向
                Vector2 tangent = BezierUtils.CalculateCubicBezierTangent(_t, startAircraftLogicPos, controlLogicPos, endAircraftLogicPos);
                _applyAircraftRotation(tangent);
            }, 1f, itemMono.flyTime).OnComplete(() =>
            {
                _stopFlyAnimation();
                hide();//飞行完成后, 隐藏飞机
                
                _onFlyComplete?.Invoke();
            });
        }

        #region 动画

        /// <summary>
        /// 播放飞行动画
        /// </summary>
        private void _playFlyAnimation()
        {
            if (itemMono == null || itemMono.animation == null || string.IsNullOrEmpty(itemMono.flyAniName))
                return;
            
            itemMono.animation.ForcePlay(itemMono.flyAniName);
        }

        /// <summary>
        /// 停止飞行动画
        /// </summary>
        private void _stopFlyAnimation()
        {
            if (itemMono == null || itemMono.animation == null)
                return;
            
            itemMono.animation.Stop();
        }

        #endregion

        /// <summary>
        /// 根据逻辑平面上的切线向量设置飞机的朝向和翻转
        /// </summary>
        private void _applyAircraftRotation(Vector2 _tangent)
        {
            if (itemMono == null || itemMono.transform == null)
                return;

            if (_tangent.sqrMagnitude < 0.0001f)
                return;

            float angle = Mathf.Atan2(_tangent.y, _tangent.x) * Mathf.Rad2Deg;

            // // 根据切线水平分量决定飞机左右翻转(飞机改为俯视, 不需要翻转了)
            // if (_tangent.x < 0)
            //     itemMono.transform.localScale = new Vector3(-1, 1, 1);
            // else
            //     itemMono.transform.localScale = new Vector3(1, 1, 1);

            // 设置飞机旋转（沿摄像机forward方向旋转）
            itemMono.transform.localEulerAngles = CameraController.instance.cameraForward.normalized * angle;
        }
    }
}