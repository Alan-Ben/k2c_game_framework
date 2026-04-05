using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GTDPlayerCuteActor
    {
        private NPGGoIndex _m_goIndex;
        private Color _m_cSkinColor;

        private EPlayerCuteActorTagType _m_eActorTagType;

        private GTDMonoPlayerCuteActor _m_cuteActorMono;
        private GTDMonoPlayerCuteActorAnimationEventTrigger _m_animationEventTrigger;
        
        [NotNull]private CommonSfxCtrlContainer _m_sfxContainer= new CommonSfxCtrlContainer();
        
        private GTDPlayerCuteActor(NPGGoIndex _goIndex, Color _skinColor, EPlayerCuteActorTagType _actorTagType, GTDMonoPlayerCuteActor _cuteActorMono)
        {
            _m_cuteActorMono = _cuteActorMono;

            _m_goIndex = _goIndex;
            setSKinColor(_skinColor);

            setPlayerCuteActorTagType(_actorTagType);
            
            if (_m_cuteActorMono != null && _m_cuteActorMono.animator != null)
            {
                _m_animationEventTrigger = _m_cuteActorMono.animator.GetComponent<GTDMonoPlayerCuteActorAnimationEventTrigger>();
                if (_m_animationEventTrigger == null)
                    _m_animationEventTrigger = _m_cuteActorMono.animator.gameObject.AddComponent<GTDMonoPlayerCuteActorAnimationEventTrigger>();

                if (_m_animationEventTrigger != null)
                {
                    _m_animationEventTrigger.OnRunFootsteps += _onRunFootsteps;
                }
            }
        }
        
        public GameObject gameObject { get { return _m_cuteActorMono == null ? null : _m_cuteActorMono.gameObject; } }
        
        public Vector3 uiFollowOffset { get { return _m_cuteActorMono == null ? Vector3.zero : _m_cuteActorMono.uiFollowOffset; } }

        public Animator actorAnimator { get { return _m_cuteActorMono == null ? null : _m_cuteActorMono.animator; } }
        
        protected Transform rotateTarget
        {
            get
            {
                if (_m_cuteActorMono == null)
                    return null;

                return _m_cuteActorMono.rotateTarget == null ? _m_cuteActorMono.transform : _m_cuteActorMono.rotateTarget;
            }
        }

        public void setPlayerCuteActorTagType(EPlayerCuteActorTagType _tagType)
        {
            _m_eActorTagType = _tagType;
            
            if(_m_cuteActorMono == null)
                return;
            
            RuntimeAnimatorController controller = _m_cuteActorMono.getAnimatorController(_tagType);
            _setAnimatorController(controller);
        }

        public void setSKinColor(Color _skinColor)
        {
            if(_m_cuteActorMono == null || _skinColor == Color.clear || _skinColor == _m_cSkinColor)
                return;

            _m_cSkinColor = _skinColor;
            _m_cuteActorMono.setSkinColor(_m_cSkinColor);
        }
        
        #region transform

        public void setParent(Transform _parent, bool _worldRotationStay = true)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.transform== null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setParent 时 _m_cuteActorMono 为空");
                return;
            }
            
            Quaternion rotation = getRotation(_worldRotationStay);//记录变化父节点前的rotation或localRotation
            _m_cuteActorMono.transform.SetParent(_parent);

            _m_cuteActorMono.transform.localRotation = Quaternion.identity;//重置最外层旋转
            setRotation(rotation, _worldRotationStay);//修改变化父节点后的rotation或localRotation
        }
        
        public void setParent(Transform _parent, bool _worldPositionStay, bool _worldRotationStay)
        {
            if (_m_cuteActorMono == null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setParent 时 _m_cuteActorMono 为空");
                return;
            }
            
            Quaternion rotation = getRotation(_worldRotationStay);//记录变化父节点前的rotation或localRotation
            _m_cuteActorMono.transform.SetParent(_parent, _worldPositionStay);
            
            _m_cuteActorMono.transform.localRotation = Quaternion.identity;//重置最外层旋转
            setRotation(rotation, _worldRotationStay);//修改变化父节点后的rotation或localRotation
        }

        public void setPosition(Vector3 _position, bool _isWorld)
        {
            if (_m_cuteActorMono == null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setPosition 时 _m_cuteActorMono 为空");
                return;
            }

            if (_isWorld)
            {
                _m_cuteActorMono.transform.position = _position;
            }
            else
            {
                _m_cuteActorMono.transform.localPosition = _position;
            }
        }

        public void setScale(Vector3 _scale)
        {
            if (_m_cuteActorMono == null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setScale 时 _m_cuteActorMono 为空");
                return;
            }

            _m_cuteActorMono.transform.localScale = _scale;
        }

        public Quaternion getRotation(bool _isWorld)
        {
            Transform tranform = rotateTarget;
            if(tranform == null)
                return Quaternion.identity;

            return _isWorld ? tranform.rotation : tranform.localRotation;
        }
        
        public void setRotation(Quaternion _rotation, bool _isWorld)
        {
            Transform tranform = rotateTarget;
            if (tranform == null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setRotation 时 rotateTarget 为空");
                return;
            }

            if (_isWorld)
            {
                tranform.rotation = _rotation;
            }
            else
            {
                tranform.localRotation = _rotation;
            }
        }

        public Vector3 getEulerAngles(bool _isWorld)
        {
            Transform tranform = rotateTarget;
            if(tranform == null)
                return Vector3.zero;

            return _isWorld ? tranform.eulerAngles : tranform.localEulerAngles;
        }
        
        public void setEulerAngles(Vector3 _eulerAngles, bool _isWorld)
        {
            Transform tranform = rotateTarget;
            if (tranform == null)
            {
                Debug.LogError("[**GTDPlayerCuteActor**] setEulerAngles 时 rotateTarget 为空");
                return;
            }

            if (_isWorld)
            {
                tranform.eulerAngles = _eulerAngles;
            }
            else
            {
                tranform.localEulerAngles = _eulerAngles;
            }
        }
        
        public void resetTransform()
        {
            if (_m_cuteActorMono != null && _m_cuteActorMono.transform != null)
                _m_cuteActorMono.transform.localRotation = Quaternion.identity;
            
            setPosition(Vector3.zero, false);
            setRotation(Quaternion.identity, false);
        }

        #endregion

        #region Animator
        
        private void _setAnimatorController(RuntimeAnimatorController _controller)
        {
            if (_controller == null || _m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.runtimeAnimatorController = _controller;
        }
        
        public void setAnimatorTrigger(string _triggerName)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetTrigger(_triggerName);
        }
        
        public void setAnimatorTrigger(int _triggerHash)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetTrigger(_triggerHash);
        }

        public void resetAnimatorTrigger(string _triggerName)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.ResetTrigger(_triggerName);
        }
        
        public void resetAnimatorTrigger(int _triggerHash)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.ResetTrigger(_triggerHash);
        }
        
        public void setAnimatorBool(string _boolName, bool _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetBool(_boolName, _value);
        }
        
        public void setAnimatorBool(int _boolHash, bool _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetBool(_boolHash, _value);
        }
        
        public void setAnimatorFloat(string _floatName, float _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetFloat(_floatName, _value);
        }
        
        public void setAnimatorFloat(int _floatHash, float _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetFloat(_floatHash, _value);
        }
        
        public void setAnimatorInt(string _intName, int _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetInteger(_intName, _value);
        }
        
        public void setAnimatorInt(int _intHash, int _value)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.SetInteger(_intHash, _value);
        }
        
        public void playAnimatorState(string _stateName)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateName);
        }

        public void playAnimatorState(string _stateName, int _layerIndex)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateName, _layerIndex);
        }
        
        public void playAnimatorState(string _stateName, int _layerIndex,  float _normalizedTime)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateName, _layerIndex, _normalizedTime);
        }
        
        public void playAnimatorState(int _stateHash)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateHash);
        }
        
        public void playAnimatorState(int _stateHash, int _layerIndex)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateHash, _layerIndex);
        }
        
        public void playAnimatorState(int _stateHash, int _layerIndex,  float _normalizedTime)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.Play(_stateHash, _layerIndex, _normalizedTime);
        }
        
        public void crossFadeAnimatorState(string _stateName, float _fadeTime)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.CrossFade(_stateName, _fadeTime);
        }
        
        public void crossFadeAnimatorState(int _stateHash, float _fadeTime)
        {
            if (_m_cuteActorMono == null || _m_cuteActorMono.animator == null)
                return;
            
            _m_cuteActorMono.animator.CrossFade(_stateHash, _fadeTime);
        }

        #endregion

        #region 动画事件

        /// <summary>
        /// 跑动时, 脚步落下事件
        /// </summary>
        public event Action onRunFootsteps;
        private void _onRunFootsteps()
        {
            onRunFootsteps?.Invoke();
        }

        #endregion
        
        #region 特效

        public CommonTDSfxObj playSfx(long _sfxId)
        {
            if(_sfxId <= 0 || _m_cuteActorMono == null)
                return null;
            
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(_sfxId, _m_cuteActorMono.sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);

            return commonTdSfxObj;
        }

        public void discardSfx(CommonTDSfxObj _obj)
        {
            if(_obj == null)
                return;
            
            _obj.forceDiscard();
        }

        public void discardAllSfx()
        {
            _m_sfxContainer?.clear();
        }

        #endregion
        
        public void discard()
        {
            _m_sfxContainer?.clear();
            _m_sfxContainer = null;

            resetTransform();

            if (_m_animationEventTrigger != null)
            {
                _m_animationEventTrigger.OnRunFootsteps -= _onRunFootsteps;
            }
            _m_animationEventTrigger = null;
            
            if (_m_cuteActorMono != null)
            {
                GGoIndexCacheMgr.instance.pushbackItem(_m_goIndex, _m_cuteActorMono.gameObject);
                _m_cuteActorMono = default;
            }
        }
        
        public static void createPlayerCuteActor(_IPlayerCuteActorShowInfo _cuteActorShowInfo, EPlayerCuteActorTagType _actorTagType, Transform _parent, [NotNull]Action<GTDPlayerCuteActor> _complete)
        {
            if (_cuteActorShowInfo == null)
            {
                Debug.LogError($"[**GTDPlayerCuteActor**] 加载Q版形象失败 _cuteActorShowInfo == null");
                _complete.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(_cuteActorShowInfo.actorGoIndex, _go =>
            {
                if (_go == null)
                {
                    Debug.LogError($"[**GTDPlayerCuteActor**] 加载Q版形象 {_cuteActorShowInfo.actorGoIndex} 失败");
                    _complete.Invoke(null);
                    return;
                }

                GTDMonoPlayerCuteActor mono = _go.GetComponent<GTDMonoPlayerCuteActor>();
                if (mono == null)
                {
                    Debug.LogError($"[**GTDPlayerCuteActor**] Q版形象资源 {_cuteActorShowInfo.actorGoIndex} 不含有 GTDMonoPlayerCuteActor ");
                    _complete.Invoke(null);
                    return;
                }

                ALUGUICommon.setGameObjEnable(mono, true);
                GTDPlayerCuteActor playerCuteActor = new GTDPlayerCuteActor(_cuteActorShowInfo.actorGoIndex, _cuteActorShowInfo.actorSkinColor, _actorTagType, mono);
                playerCuteActor.resetTransform();
                playerCuteActor.setParent(_parent);
                
                _complete.Invoke(playerCuteActor); 
            });
        }
    }
}