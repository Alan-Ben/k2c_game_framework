using UnityEngine;

namespace GOE
{
    public class SoftBorder2DPosLimiter : _APosLimiter
    {
        private Rect _m_stableRect;
        private Vector2 _m_softBorderSize;
        private _ALogicPlane2DPosGetter _m_logicPosGetter;
        
        public SoftBorder2DPosLimiter(Rect _stableRect, Vector2 _softBorderSize, _ALogicPlane2DPosGetter _logicPosGetter)
        {
            _refresh(_stableRect, _softBorderSize, _logicPosGetter);
        }
        
        /// <summary>
        /// 稳定的范围
        /// </summary>
        public Rect stableRect { get { return _m_stableRect; } }
        /// <summary>
        /// 边缘柔和拖拽范围
        /// </summary>
        public Vector2 softBorderSize { get { return _m_softBorderSize; } }
        /// <summary>
        /// 3D 转 2D 的坐标转换器
        /// </summary>
        public _ALogicPlane2DPosGetter posGetter { get { return _m_logicPosGetter; } }

        /// <summary>
        /// 限制输入的位置
        /// </summary>
        public override Vector3 limitPos(Vector3 _pos, out bool _isLimited)
        {
            _isLimited = false;
            if (_m_logicPosGetter == null)
                return _pos;

            // 获得输入位置的逻辑坐标
            Vector2 logicPos = new Vector2(_m_logicPosGetter.getLogicXPos(_pos), _m_logicPosGetter.getLogicYPos(_pos));
            // 判断 X 是否超出了稳定范围
            if (logicPos.x < _m_stableRect.xMin)
            {
                // 超出稳定范围计算新的位置
                float newX;
                // 如果有柔化边界存在，根据设置的柔化边缘的长度，重新映射当前的 X 坐标
                if (_m_softBorderSize.x > 0)
                    newX = _m_stableRect.xMin - _remapValueFromZeroToMax(_m_stableRect.xMin - logicPos.x, _m_softBorderSize.x);
                // 否则直接限制在稳定范围内
                else
                    newX = _m_stableRect.xMin;

                // 把限制好的逻辑坐标 X 赋值到实际坐标中
                _m_logicPosGetter.setLogicXPos(ref _pos, newX);
                _isLimited = true;
            }
            // 判断 X 是否超出的稳定范围，流程同上
            else if (logicPos.x > _m_stableRect.xMax)
            {
                float newX;
                if (_m_softBorderSize.x > 0)
                    newX = _m_stableRect.xMax + _remapValueFromZeroToMax(logicPos.x - _m_stableRect.xMax, _m_softBorderSize.x);
                else
                    newX = _m_stableRect.xMax;

                _m_logicPosGetter.setLogicXPos(ref _pos, newX);
                _isLimited = true;
            }

            // 判断 Y 是否超出了稳定范围，具体流程同上
            if (logicPos.y < _m_stableRect.yMin)
            {
                float newY;
                if (_m_softBorderSize.y > 0)
                    newY = _m_stableRect.yMin - _remapValueFromZeroToMax(_m_stableRect.yMin - logicPos.y, _m_softBorderSize.y);
                else
                    newY = _m_stableRect.yMin;

                _m_logicPosGetter.setLogicYPos(ref _pos, newY);
                _isLimited = true;
            }
            else if (logicPos.y > _m_stableRect.yMax)
            {
                float newY;
                if (_m_softBorderSize.y > 0)
                    newY = _m_stableRect.yMax + _remapValueFromZeroToMax(logicPos.y - _m_stableRect.yMax, _m_softBorderSize.y);
                else
                    newY = _m_stableRect.yMax;

                _m_logicPosGetter.setLogicYPos(ref _pos, newY);
                _isLimited = true;
            }

            return _pos;
        }

        public override bool isPosInStableArea(Vector3 _pos, out Vector3 _closestPos)
        {
            if (_m_logicPosGetter == null)
            {
                _closestPos = _pos;
                return true;
            }

            bool isInStableArea = true;
            // 获得输入位置的逻辑坐标
            Vector2 logicPos = new Vector2(_m_logicPosGetter.getLogicXPos(_pos), _m_logicPosGetter.getLogicYPos(_pos));
            // 判断 X 是否超出了稳定范围
            if (logicPos.x < _m_stableRect.xMin)
            {
                // 把限制好的逻辑坐标 X 赋值到实际坐标中
                _m_logicPosGetter.setLogicXPos(ref _pos, _m_stableRect.xMin);
                isInStableArea = false;
            }
            // 判断 X 是否超出的稳定范围，流程同上
            else if (logicPos.x > _m_stableRect.xMax)
            {
                _m_logicPosGetter.setLogicXPos(ref _pos, _m_stableRect.xMax);
                isInStableArea = false;
            }

            // 判断 Y 是否超出了稳定范围，具体流程同上
            if (logicPos.y < _m_stableRect.yMin)
            {
                _m_logicPosGetter.setLogicYPos(ref _pos, _m_stableRect.yMin);
                isInStableArea = false;
            }
            else if (logicPos.y > _m_stableRect.yMax)
            {
                _m_logicPosGetter.setLogicYPos(ref _pos, _m_stableRect.yMax);
                isInStableArea = false;
            }
            
            // 赋值给最近的点
            _closestPos = _pos;
            return isInStableArea;
        }

        protected void _refresh(Rect _stableRect, Vector2 _softBorderSize, _ALogicPlane2DPosGetter _logicPosGetter)
        {
            _m_stableRect = _stableRect;
            _m_softBorderSize = _softBorderSize;
            _m_logicPosGetter = _logicPosGetter;
        }

        protected virtual float _remapValueFromZeroToMax(float _value, float _max)
        {
            return _max * (1 - 1 / (_value / _max + 1));
        }

        public override void debugDrawUpdate(Vector3 _originPos, Color _color)
        {
            if (_m_logicPosGetter == null)
                return;
            
            // 绘制拖动可视范围
            Vector3[] dragMoveRectPoints = new Vector3[] { _originPos, _originPos, _originPos, _originPos };
            // 左下
            _m_logicPosGetter.setLogicXPos(ref dragMoveRectPoints[0], _m_stableRect.xMin);
            _m_logicPosGetter.setLogicYPos(ref dragMoveRectPoints[0], _m_stableRect.yMin);
            // 左上
            _m_logicPosGetter.setLogicXPos(ref dragMoveRectPoints[1], _m_stableRect.xMin);
            _m_logicPosGetter.setLogicYPos(ref dragMoveRectPoints[1], _m_stableRect.yMax);
            // 右上
            _m_logicPosGetter.setLogicXPos(ref dragMoveRectPoints[2], _m_stableRect.xMax);
            _m_logicPosGetter.setLogicYPos(ref dragMoveRectPoints[2], _m_stableRect.yMax);
            // 右下
            _m_logicPosGetter.setLogicXPos(ref dragMoveRectPoints[3], _m_stableRect.xMax);
            _m_logicPosGetter.setLogicYPos(ref dragMoveRectPoints[3], _m_stableRect.yMin);
            // 绘制
            DebugPlus.DrawPath(dragMoveRectPoints, true, _color);
        }
    }
}