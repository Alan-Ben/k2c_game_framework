using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class CustomMonoEnableDealPlayerEffectInfo
    {
        [ALHeader("是否enable时执行效果")]
        public bool isEnableDeal = true;
        
        [ALHeader("效果字符串")]
        public string effectStr;
    }
    
    public class GGUICustomMonoEnableDealPlayerEffect : MonoBehaviour
    {
        [ALHeader("执行效果列表")]
        public List<CustomMonoEnableDealPlayerEffectInfo> effectList;

        private List<_NPPlayerEffectSerializeInfo> _m_lEnableDealEffectList;//enable时执行效果列表
        private List<_NPPlayerEffectSerializeInfo> _m_lDisableDealEffectList;//enable时执行效果列表

        //是否需要检测
        private bool _m_bNeedCheck = false;
        
        private void Awake()
        {
            if(effectList == null)
                return;
            
            _m_lEnableDealEffectList = new List<_NPPlayerEffectSerializeInfo>();
            _m_lDisableDealEffectList = new List<_NPPlayerEffectSerializeInfo>();
            foreach (var effectInfo in effectList)
            {
                if(effectInfo == null)
                    continue;
                
                if (effectInfo.isEnableDeal)
                {
                    _m_lEnableDealEffectList.Add(_NPPlayerEffectSerializeInfo.ReadFromString(effectInfo.effectStr));
                }
                else
                {
                    _m_lDisableDealEffectList.Add(_NPPlayerEffectSerializeInfo.ReadFromString(effectInfo.effectStr));
                }
            }
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
            
            _m_lEnableDealEffectList?.Clear();
            _m_lDisableDealEffectList?.Clear();
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

            // 延迟到下一帧进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }
        
        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if(_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

#if NP_GAME
            if (gameObject.activeInHierarchy)
            {
                if (_m_lEnableDealEffectList != null)
                {
                    foreach (var effect in _m_lEnableDealEffectList)
                    {
                        if(effect != null)
                            effect.dealEffect();
                    }
                }
            }
            else
            {
                if (_m_lDisableDealEffectList != null)
                {
                    foreach (var effect in _m_lDisableDealEffectList)
                    {
                        if(effect != null)
                            effect.dealEffect();
                    }
                }
            }
#endif
        }
    }
}