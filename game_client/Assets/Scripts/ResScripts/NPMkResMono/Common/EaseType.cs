using System;
using UnityEngine;

namespace GOE
{
    public enum EaseType
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
    }

    public static class EaseTypeExtension
    {
        private const float _k_PiOver2 = Mathf.PI * 0.5f;
        private const float _k_TwoPi = Mathf.PI * 2;

        public static float evaluate(this EaseType _easeType, float _time, float _duration, float _overshootOrAmplitude = 1.70158f, float _period = 0f)
        {
            switch (_easeType)
            {
                case EaseType.Linear:
                    return _time / _duration;
                case EaseType.InSine:
                    return -(float)Math.Cos(_time / _duration * _k_PiOver2) + 1;
                case EaseType.OutSine:
                    return (float)Math.Sin(_time / _duration * _k_PiOver2);
                case EaseType.InOutSine:
                    return -0.5f * ((float)Math.Cos(Mathf.PI * _time / _duration) - 1);
                case EaseType.InQuad:
                    return (_time /= _duration) * _time;
                case EaseType.OutQuad:
                    return -(_time /= _duration) * (_time - 2);
                case EaseType.InOutQuad:
                    if ((_time /= _duration * 0.5f) < 1) return 0.5f * _time * _time;
                    return -0.5f * ((--_time) * (_time - 2) - 1);
                case EaseType.InCubic:
                    return (_time /= _duration) * _time * _time;
                case EaseType.OutCubic:
                    return ((_time = _time / _duration - 1) * _time * _time + 1);
                case EaseType.InOutCubic:
                    if ((_time /= _duration * 0.5f) < 1) return 0.5f * _time * _time * _time;
                    return 0.5f * ((_time -= 2) * _time * _time + 2);
                case EaseType.InQuart:
                    return (_time /= _duration) * _time * _time * _time;
                case EaseType.OutQuart:
                    return -((_time = _time / _duration - 1) * _time * _time * _time - 1);
                case EaseType.InOutQuart:
                    if ((_time /= _duration * 0.5f) < 1) return 0.5f * _time * _time * _time * _time;
                    return -0.5f * ((_time -= 2) * _time * _time * _time - 2);
                case EaseType.InQuint:
                    return (_time /= _duration) * _time * _time * _time * _time;
                case EaseType.OutQuint:
                    return ((_time = _time / _duration - 1) * _time * _time * _time * _time + 1);
                case EaseType.InOutQuint:
                    if ((_time /= _duration * 0.5f) < 1) return 0.5f * _time * _time * _time * _time * _time;
                    return 0.5f * ((_time -= 2) * _time * _time * _time * _time + 2);
                case EaseType.InExpo:
                    return (_time == 0) ? 0 : (float)Math.Pow(2, 10 * (_time / _duration - 1));
                case EaseType.OutExpo:
                    if (_time == _duration) return 1;
                    return (-(float)Math.Pow(2, -10 * _time / _duration) + 1);
                case EaseType.InOutExpo:
                    if (_time == 0) return 0;
                    if (_time == _duration) return 1;
                    if ((_time /= _duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (_time - 1));
                    return 0.5f * (-(float)Math.Pow(2, -10 * --_time) + 2);
                case EaseType.InCirc:
                    return -((float)Math.Sqrt(1 - (_time /= _duration) * _time) - 1);
                case EaseType.OutCirc:
                    return (float)Math.Sqrt(1 - (_time = _time / _duration - 1) * _time);
                case EaseType.InOutCirc:
                    if ((_time /= _duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - _time * _time) - 1);
                    return 0.5f * ((float)Math.Sqrt(1 - (_time -= 2) * _time) + 1);
                case EaseType.InElastic:
                    float s0;
                    if (_time == 0) return 0;
                    if ((_time /= _duration) == 1) return 1;
                    if (_period == 0) _period = _duration * 0.3f;
                    if (_overshootOrAmplitude < 1)
                    {
                        _overshootOrAmplitude = 1;
                        s0 = _period / 4;
                    }
                    else s0 = _period / _k_TwoPi * (float)Math.Asin(1 / _overshootOrAmplitude);

                    return -(_overshootOrAmplitude * (float)Math.Pow(2, 10 * (_time -= 1)) * (float)Math.Sin((_time * _duration - s0) * _k_TwoPi / _period));
                case EaseType.OutElastic:
                    float s1;
                    if (_time == 0) return 0;
                    if ((_time /= _duration) == 1) return 1;
                    if (_period == 0) _period = _duration * 0.3f;
                    if (_overshootOrAmplitude < 1)
                    {
                        _overshootOrAmplitude = 1;
                        s1 = _period / 4;
                    }
                    else s1 = _period / _k_TwoPi * (float)Math.Asin(1 / _overshootOrAmplitude);

                    return (_overshootOrAmplitude * (float)Math.Pow(2, -10 * _time) * (float)Math.Sin((_time * _duration - s1) * _k_TwoPi / _period) + 1);
                case EaseType.InOutElastic:
                    float s;
                    if (_time == 0) return 0;
                    if ((_time /= _duration * 0.5f) == 2) return 1;
                    if (_period == 0) _period = _duration * (0.3f * 1.5f);
                    if (_overshootOrAmplitude < 1)
                    {
                        _overshootOrAmplitude = 1;
                        s = _period / 4;
                    }
                    else s = _period / _k_TwoPi * (float)Math.Asin(1 / _overshootOrAmplitude);

                    if (_time < 1)
                        return -0.5f * (_overshootOrAmplitude * (float)Math.Pow(2, 10 * (_time -= 1)) * (float)Math.Sin((_time * _duration - s) * _k_TwoPi / _period));
                    return _overshootOrAmplitude * (float)Math.Pow(2, -10 * (_time -= 1)) * (float)Math.Sin((_time * _duration - s) * _k_TwoPi / _period) * 0.5f + 1;
                case EaseType.InBack:
                    return (_time /= _duration) * _time * ((_overshootOrAmplitude + 1) * _time - _overshootOrAmplitude);
                case EaseType.OutBack:
                    return ((_time = _time / _duration - 1) * _time * ((_overshootOrAmplitude + 1) * _time + _overshootOrAmplitude) + 1);
                case EaseType.InOutBack:
                    if ((_time /= _duration * 0.5f) < 1)
                        return 0.5f * (_time * _time * (((_overshootOrAmplitude *= (1.525f)) + 1) * _time - _overshootOrAmplitude));
                    return 0.5f * ((_time -= 2) * _time * (((_overshootOrAmplitude *= (1.525f)) + 1) * _time + _overshootOrAmplitude) + 2);
                case EaseType.InBounce:
                    return _bounceEaseIn(_time, _duration);
                case EaseType.OutBounce:
                    return _bounceEaseOut(_time, _duration);
                case EaseType.InOutBounce:
                    return _bounceEaseInOut(_time, _duration);
                default:
                    return 0;
            }
        }

        private static float _bounceEaseIn(float _time, float _duration)
        {
            return 1 - _bounceEaseOut(_duration - _time, _duration);
        }

        private static float _bounceEaseOut(float _time, float _duration)
        {
            if ((_time /= _duration) < (1 / 2.75f))
            {
                return (7.5625f * _time * _time);
            }

            if (_time < (2 / 2.75f))
            {
                return (7.5625f * (_time -= (1.5f / 2.75f)) * _time + 0.75f);
            }

            if (_time < (2.5f / 2.75f))
            {
                return (7.5625f * (_time -= (2.25f / 2.75f)) * _time + 0.9375f);
            }

            return (7.5625f * (_time -= (2.625f / 2.75f)) * _time + 0.984375f);
        }

        private static float _bounceEaseInOut(float _time, float _duration)
        {
            if (_time < _duration * 0.5f)
            {
                return _bounceEaseIn(_time * 2, _duration) * 0.5f;
            }

            return _bounceEaseOut(_time * 2 - _duration, _duration) * 0.5f + 0.5f;
        }
    }
}