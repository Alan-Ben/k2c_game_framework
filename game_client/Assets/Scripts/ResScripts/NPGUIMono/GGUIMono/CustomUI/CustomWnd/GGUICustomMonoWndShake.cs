using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 窗口震动CustomMono，监听消息后执行受击震动
    /// </summary>
    public class GGUICustomMonoWndShake : MonoBehaviour, _IShakable
    {
        [ALHeader("受击震动的根节点")]
        public RectTransform transShakeRoot;

        [ALHeader("默认受击震动的剧烈程度")]
        public float defaultShakeIntensity = 20f;

        [ALHeader("默认受击震动的持续时间")]
        public float defaultShakeDuration = 0.2f;

        public int taskSerialize { get { return _m_iShakeTaskSerialize; } }
        public Vector3 shakeOriginPos { get { return _m_vShakeOriginPos; } }
        public RectTransform shakeRoot { get { return transShakeRoot; } }

        // 震动任务序列号
        private int _m_iShakeTaskSerialize;
        // 震动原点
        private Vector3 _m_vShakeOriginPos;

        private void OnEnable()
        {
            if (transShakeRoot != null)
                _m_vShakeOriginPos = transShakeRoot.anchoredPosition;

            WinMsg.RegisterMsg(WinMsgType.SIMULATE_TRIGGER_WND_SHAKE, _onTriggerWndShake);
        }

        private void OnDisable()
        {
            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();

            if (transShakeRoot != null)
                transShakeRoot.anchoredPosition = _m_vShakeOriginPos;

            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_TRIGGER_WND_SHAKE, _onTriggerWndShake);
        }

        // 参数：_objs[0] = float 震动强度（可选），_objs[1] = float 震动持续时间（可选）
        private void _onTriggerWndShake(params object[] _objs)
        {
            float shakeIntensity = defaultShakeIntensity;
            float shakeDuration = defaultShakeDuration;

            if (_objs != null)
            {
                if (_objs.Length >= 1 && _tryGetFloatValue(_objs[0], out float msgShakeIntensity))
                    shakeIntensity = msgShakeIntensity;

                if (_objs.Length >= 2 && _tryGetFloatValue(_objs[1], out float msgShakeDuration))
                    shakeDuration = msgShakeDuration;
            }

            _playShake(shakeIntensity, shakeDuration);
        }

        private void _playShake(float _shakeIntensity, float _shakeDuration)
        {
            if (transShakeRoot == null)
                return;

            if (_shakeIntensity <= 0f || _shakeDuration <= 0f)
            {
                transShakeRoot.anchoredPosition = _m_vShakeOriginPos;
                return;
            }

            _m_iShakeTaskSerialize = ALSerializeOpMgr.next();
            new ShakeTask(this, _shakeIntensity, _shakeDuration, true).deal();
        }

        private bool _tryGetFloatValue(object _obj, out float _value)
        {
            _value = 0f;

            if (_obj == null)
                return false;

            if (_obj is float floatValue)
            {
                _value = floatValue;
                return true;
            }

            if (_obj is double doubleValue)
            {
                _value = (float)doubleValue;
                return true;
            }

            if (_obj is int intValue)
            {
                _value = intValue;
                return true;
            }

            if (_obj is long longValue)
            {
                _value = longValue;
                return true;
            }

            if (_obj is string stringValue)
                return ALCommon.TryParseFloat(stringValue, out _value);

            return false;
        }
    }
}