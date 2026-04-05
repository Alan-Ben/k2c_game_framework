using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _ANPGGUIMonoCommonFadeObject : MonoBehaviour
    {
        [ALInfo("若值在 activeRange 内，图片的 alpha 为 1 ，若不在范围内，则根据当前值和 fadeRange 的配置进行 alpha 的插值")]
        [ALHeader("有效范围和淡出范围")]
        public WCGFloatRange activeRange;
        public WCGFloatRange fadeRange;

        // 上淡出范围
        private WCGFloatRange _m_upperFadeRange;
        // 下淡出范围
        private WCGFloatRange _m_lowerFadeRange;

        public void Awake()
        {
            if (fadeRange == null || activeRange == null)
                return;

            if (fadeRange.max > activeRange.max)
                _m_upperFadeRange = new WCGFloatRange(activeRange.max, fadeRange.max);
            if (fadeRange.min < activeRange.min)
                _m_lowerFadeRange = new WCGFloatRange(fadeRange.min, activeRange.min);
        }

        public void OnDestroy()
        {
            _m_upperFadeRange = null;
            _m_lowerFadeRange = null;
        }

        /// <summary>
        /// 设置数值
        /// </summary>
        public void setValue(float _value)
        {
            if (activeRange == null)
                return;

            if (activeRange.inRange(_value))
            {
                _setAlphaValue(1);
                return;
            }

            if (_m_upperFadeRange != null && _m_upperFadeRange.inRange(_value))
            {
                _setAlphaValue(_value.Remap(_m_upperFadeRange.min, _m_upperFadeRange.max, 1, 0));
                return;
            }

            if (_m_lowerFadeRange != null && _m_lowerFadeRange.inRange(_value))
            {
                _setAlphaValue(_value.Remap(_m_lowerFadeRange.min, _m_lowerFadeRange.max, 0, 1));
                return;
            }
            
            _setAlphaValue(0);
        }

        protected abstract void _setAlphaValue(float _alphaValue);
    }
}