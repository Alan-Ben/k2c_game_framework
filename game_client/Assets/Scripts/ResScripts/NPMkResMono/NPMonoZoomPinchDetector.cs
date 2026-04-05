using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPMonoZoomPinchDetector : MonoBehaviour
    {
        // 当前聚焦的位置
        private Vector2 _m_focusScreenPos; 
        
        /// <summary>
        /// 当缩放值发生变化时
        /// </summary>
        public event Action<float> onScaleChg;
        public event Action onScaleChgStart;
        public event Action onScaleChgEnd;

        private bool _m_isStartChg = false;
        /// <summary>
        /// 当前想要放大的位置
        /// </summary>
        public Vector2 focusScreenPos { get { return _m_focusScreenPos; } }

#if UNITY_ANDROID || UNITY_IOS
        public void OnEnable()
        {
            _m_currentDistance = 0;
        }
#endif

        public void Update()
        {
            if (onScaleChg == null)
                return;

            float chgValue = _getScaleChgValue(out _m_focusScreenPos);
            onScaleChg?.Invoke(chgValue);
        }

#if UNITY_ANDROID || UNITY_IOS
        private float _m_currentDistance;
        private float _getScaleChgValue(out Vector2 _scaleChgCenter)
        {
            if (Input.touchCount < 2)
            {
                _m_currentDistance = 0;
                _scaleChgCenter = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);//Input.mousePosition;

                if(_m_isStartChg)
                {
                    onScaleChgEnd?.Invoke();
                    _m_isStartChg = false;
                }
                return 0;
            }

            float result = 0;
            Vector2 touch1Pos = Input.GetTouch(0).position;
            Vector2 touch2Pos = Input.GetTouch(1).position;

            if(!_m_isStartChg)
            {
                _scaleChgCenter = (touch1Pos + touch2Pos) / 2;
                _m_currentDistance = Vector2.SqrMagnitude(touch1Pos - touch2Pos);
                onScaleChgStart?.Invoke();
                _m_isStartChg = true;
            }
            else
            {
                _scaleChgCenter = _m_focusScreenPos;
                float newDistance = Vector2.SqrMagnitude(touch1Pos - touch2Pos);
                result = Mathf.Sqrt(newDistance / _m_currentDistance) - 1;
                _m_currentDistance = newDistance;
            }

            return result;
        }
#elif UNITY_STANDALONE
        private float _getScaleChgValue(out Vector2 _scaleChgCenter)
        {
            if (!Input.GetMouseButton(1))//右键没有按下不算缩放
            {
                _scaleChgCenter = Camera.main.ViewportToScreenPoint(Vector3.one * 0.5f);//Input.mousePosition;
                
                if(_m_isStartChg)
                {
                    onScaleChgEnd?.Invoke();
                    _m_isStartChg = false;
                }

                return 0;
            }

            if(!_m_isStartChg)
            {
                onScaleChgStart?.Invoke();
                _m_isStartChg = true;
            }
            
            _scaleChgCenter = Input.mousePosition;
            return Input.GetAxis("Mouse ScrollWheel");;
            
        }
#endif
    }
}