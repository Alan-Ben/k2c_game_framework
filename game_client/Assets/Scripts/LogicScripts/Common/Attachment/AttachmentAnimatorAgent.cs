
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class AttachmentAnimatorAgent
    {
        [ItemNotNull][NotNull] private readonly List<Animator> _m_animatorList;
        
        [NotNull] private readonly MutexActionSetter _m_mutexActionSetter;
        [NotNull] private readonly Dictionary<int, FloatActionSetter> _m_floatSetterDic;
        
        public AttachmentAnimatorAgent(int _actionId, int _actionTriggerId)
        {
            _m_animatorList = new List<Animator>();

            _m_mutexActionSetter = new MutexActionSetter(_actionId, _actionTriggerId);
            _m_floatSetterDic = new Dictionary<int, FloatActionSetter>();
        }
        
        public void addAnimator(Animator _animator)
        {
            if (_animator == null)
                return;
            
            _m_animatorList.Add(_animator);
            _m_mutexActionSetter.setParameters(_animator);
            foreach (FloatActionSetter setter in _m_floatSetterDic.Values)
            {
                setter?.setParameters(_animator);
            }
        }

        public void removeAnimator(Animator _animator)
        {
            if (_animator == null)
                return;
            
            _m_animatorList.Remove(_animator);
        }

        public void SetFloat(int _id, float _value)
        {
            if (!_m_floatSetterDic.TryGetValue(_id, out FloatActionSetter setter) || setter == null)
            {
                setter = new FloatActionSetter(_id);
                _m_floatSetterDic[_id] = setter;
            }
            
            setter.setData(_value);
            foreach (Animator animator in _m_animatorList)
            {
                setter.setParameters(animator);
            }
        }

        public void SetMutexAction(int _type)
        {
            _m_mutexActionSetter.setData(_type);
            foreach (Animator animator in _m_animatorList)
            {
                _m_mutexActionSetter.setParameters(animator);
            }
        }

        private abstract class _AnimatorParametersSetter
        {
            public abstract void setParameters([NotNull] Animator _animator);
        }
        private class MutexActionSetter : _AnimatorParametersSetter
        {
            private readonly int _m_actionId;
            private readonly int _m_actionTriggerId;
            private int _m_type;
            
            public MutexActionSetter(int _actionId, int _actionTriggerId)
            {
                _m_actionId = _actionId;
                _m_actionTriggerId = _actionTriggerId;
                _m_type = 0;
            }

            public void setData(int _type)
            {
                _m_type = _type;
            }
            
            public override void setParameters(Animator _animator)
            {
                // 如果状态一样就不处理
                if (_m_type == _animator.GetInteger(_m_actionId))
                    return;
                
                _animator.SetInteger(_m_actionId, _m_type);
                if (_m_type != 0)
                    _animator.SetTrigger(_m_actionTriggerId);
            }
        }

        private class FloatActionSetter : _AnimatorParametersSetter
        {
            private readonly int _m_id;
            private float _m_value;
            
            public FloatActionSetter(int _id)
            {
                _m_id = _id;
                _m_value = 0;
            }

            public void setData(float _value)
            {
                _m_value = _value;
            }
            
            public override void setParameters(Animator _animator)
            {
                _animator.SetFloat(_m_id, _m_value);
            }
        }
    }
}