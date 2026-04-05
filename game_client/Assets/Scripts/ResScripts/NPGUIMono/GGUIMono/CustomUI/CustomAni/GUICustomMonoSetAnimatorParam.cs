using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public enum EAnimatorParamType
    {
        Trigger,
        Bool,
        Int,
        Float,
    }
    
    [Serializable]
    public class AnimatorParamData
    {
        [ALHeader("参数类型")]
        public EAnimatorParamType paramType;

        [ALHeader("参数名")]
        public string paramName;
        
        [ALHeader("enable时设置的值(若为Trigger类型, 填true代表触发, false代表重置)")]
        public string enableParamValue;
        
        [ALHeader("disable时设置的值(若为Trigger类型, 填true代表触发, false代表重置)")]
        public string disableParamValue;
    }
    
    public class GUICustomMonoSetAnimatorParam : MonoBehaviour
    {
        [ALHeader("设置参数的Animator")]
        public Animator ani;

        [ALHeader("设置参数的Animator参数列表")]
        public List<AnimatorParamData> paramDataList;

        [NotNull] private Dictionary<string, int> _m_dParamNameToHash = new Dictionary<string, int>();
        
        //是否需要检测
        private bool _m_bNeedCheck = false;
        
        
        private void Awake()
        {
            if(ani == null || paramDataList == null)
                return;
            
            foreach (var paramData in paramDataList)
            {
                if (paramData == null)
                    continue;

                _getParamHash(paramData.paramName, out int _hash);
            }
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
            
            _m_dParamNameToHash.Clear();
        }

        private void OnEnable()
        {
            _m_bNeedCheck = true;

            // 延迟到下一帧进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }
        
        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }
        
        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if(_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

#if NP_GAME
            if (paramDataList != null)
            {
                foreach (var paramData in paramDataList)
                {
                    if(paramData == null)
                        continue;

                    _setAnimatorParamValue(paramData.paramType, paramData.paramName, gameObject.activeInHierarchy ? paramData.enableParamValue : paramData.disableParamValue);
                }
            }
#endif
        }

        /// <summary>
        /// 设置Animator参数值
        /// </summary>
        private void _setAnimatorParamValue(EAnimatorParamType _paramType, string _paramName, string _paramValue)
        {
            if(ani == null)
                return;
            
            if (string.IsNullOrEmpty(_paramName) || string.IsNullOrEmpty(_paramValue))
            {
                return;
            }

            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"[GUICustomMonoSetAnimatorParam _setAnimatorParamValue] 设置[Animator参数:{_paramName}] [类型:{_paramType}] 值为:{_paramValue}");
            }
            
            switch (_paramType)
            {
                case EAnimatorParamType.Bool:
                    if (bool.TryParse(_paramValue, out bool boolValue))
                    {
                        if (_getParamHash(_paramName, out int _hash))
                        {
                            ani.SetBool(_hash, boolValue);
                        }
                        else
                        {
                            ani.SetBool(_paramName, boolValue);
                        }
                    }
                    else
                    {
                        Debug.LogError($"[GUICustomMonoSetAnimatorParam _setAnimatorParamValue] [设置Animator参数:{_paramName}] [类型:{_paramType}] 失败, _paramValue:{_paramValue}不是一个bool值", this);
                    }
                    break;
                    
                case EAnimatorParamType.Float:
                    if (ALCommon.TryParseFloat(_paramValue, out float floatValue))
                    {
                        if (_getParamHash(_paramName, out int _hash))
                        {
                            ani.SetFloat(_hash, floatValue);
                        }
                        else
                        {
                            ani.SetFloat(_paramName, floatValue);
                        }
                    }
                    else
                    {
                        Debug.LogError($"[GUICustomMonoSetAnimatorParam _setAnimatorParamValue] [设置Animator参数:{_paramName}] [类型:{_paramType}] 失败, _paramValue:{_paramValue}不是一个float值", this);
                    }
                    break;
                    
                case EAnimatorParamType.Int:
                    if (int.TryParse(_paramValue, out int intValue))
                    {
                        if (_getParamHash(_paramName, out int _hash))
                        {
                            ani.SetInteger(_hash, intValue);
                        }
                        else
                        {
                            ani.SetInteger(_paramName, intValue);
                        }
                    }
                    else
                    {
                        Debug.LogError($"[GUICustomMonoSetAnimatorParam _setAnimatorParamValue] [设置Animator参数:{_paramName}] [类型:{_paramType}] 失败, _paramValue:{_paramValue}不是一个int值", this);
                    }
                    break;
                    
                case EAnimatorParamType.Trigger:
                    if (bool.TryParse(_paramValue, out bool triggerValue))
                    {
                        if (_getParamHash(_paramName, out int _hash))
                        {
                            if (triggerValue)
                                ani.SetTrigger(_hash);
                            else
                                ani.ResetTrigger(_hash);
                        }
                        else
                        {
                            if (triggerValue)
                                ani.SetTrigger(_paramName);
                            else
                                ani.ResetTrigger(_paramName);
                        }
                    }
                    else
                    {
                        Debug.LogError($"[GUICustomMonoSetAnimatorParam _setAnimatorParamValue] [设置Animator参数:{_paramName}] [类型:{_paramType}] 失败, _paramValue:{_paramValue}不是一个bool值", this);
                    }
                    break;
            }
        }
        
        private bool _getParamHash(string _paramName, out int _hash)
        {
            _hash = 0;
            if (string.IsNullOrEmpty(_paramName))
                return false;
            
            if (_m_dParamNameToHash.TryGetValue(_paramName, out _hash))
                return true;
            
            _hash = Animator.StringToHash(_paramName);
            _m_dParamNameToHash.Add(_paramName, _hash);
            return true;
        }
    }
}